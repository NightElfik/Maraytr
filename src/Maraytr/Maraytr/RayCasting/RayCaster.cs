using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;
using Maraytr.Scenes;

namespace Maraytr.RayCasting {
	public class RayCaster : IImageFunction {

		private Scene scene;


		public RayCaster(Scene scene) {
			this.scene = scene;
		}


		public Size Size { get { return scene.Camera.Size; } }


		public ColorRgbt GetSample(double x, double y, IntegrationState intState) {

			Ray ray = scene.Camera.GetRay(x, y);
			if (ray == null) {
				return scene.BgColor;
			}

			var intersections = new List<Intersection>();
			scene.SceneRoot.Intersect(ray, intersections);

			if (intersections.Count == 0) {
				return scene.BgColor;
			}

			Intersection intersec = null;
			foreach (var iSec in intersections) {
				if (iSec.RayDistanceSqSigned > 0 && iSec.IsEnter) {
					if (intersec == null || iSec.RayDistanceSqSigned < intersec.RayDistanceSqSigned) {
						intersec = iSec;
					}
				}
			}

			if (intersec == null) {
				return scene.BgColor;
			}

			var intersecDetails = intersec.ComputeIntersectionDetails();

			Vector3 viewerDirection = -ray.Direction;
			Vector3 intersecPos = intersec.Position;
			Vector3 surfaceNormal = intersecDetails.Normal * (intersec.InverseNormal ? -1 : 1);
			surfaceNormal.NormalizeThis();
			
				
			var mat = intersec.IntersectedObject.Material;
			ColorRgbt baseColor = mat.Texture != null ? mat.Texture.GetColorAt(intersecDetails.TextureCoord) : new ColorRgbt(1, 1, 1);
			ColorRgbt totalColor = new ColorRgbt();

			foreach (var lightSource in scene.Lights) {

				Vector3 lightPos = lightSource.GetPosition(intState);
				// check if light is visible
				Vector3 lightDir = lightPos - intersecPos;
				lightDir.NormalizeThis();

				ColorRgbt light = lightSource.GetEmittedLightTo(intersecPos, intState);
				ColorRgbt reflection = scene.ReflectionModel.CountReflection(mat, surfaceNormal, lightDir, viewerDirection);

				totalColor += light * reflection;

			}

			return baseColor * totalColor;

		}

	}
}
