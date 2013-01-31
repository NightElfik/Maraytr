using System.Collections.Generic;
using Maraytr.Lights;
using Maraytr.Numerics;
using Maraytr.Rendering;
using Maraytr.Scenes;

namespace Maraytr.RayCasting {
	public class RayCaster : IImageFunction {

		private Scene scene;


		public RayCaster(Scene scene) {
			this.scene = scene;
		}


		public Size Size { get { return scene.Camera.Size; } }

		public ColorRgbt GetSample(double x, double y) {
			return GetSample(x, y, new IntegrationState());
		}

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

			intersec.CompleteIntersection();

			Vector3 viewerDirection = -ray.Direction;
			Vector3 intersecPos = intersec.Position;
			Vector3 surfaceNormal = intersec.Normal * (intersec.InverseNormal ? -1 : 1);
			surfaceNormal.NormalizeThis();
			
				
			var mat = intersec.IntersectedObject.Material;
			ColorRgbt baseColor = mat.Texture == null
				? mat.BaseColor
				: mat.Texture.GetColorAt(intersec.TextureCoord) * mat.BaseColor;
			ColorRgbt totalColor = scene.AmbientLight * baseColor;

			foreach (var lightSource in scene.Lights) {
				
				Vector3 lightPos = lightSource.GetPosition(intState);

				if (!isPointDirectlyVisibleFrom(intersecPos, lightPos)) {
					continue;
				}

				Vector3 lightDir = lightPos - intersecPos;
				lightDir.NormalizeThis();

				ColorRgbt light = lightSource.GetEmittedLightTo(intersecPos, intState);
				ColorRgbt reflection = scene.ReflectionModel.CountReflection(mat, baseColor, surfaceNormal, lightDir, viewerDirection);

				totalColor += light * reflection;

			}

			return totalColor;

		}


		private bool isPointDirectlyVisibleFrom(Vector3 startPt, Vector3 pt) {

			var ray = new Ray(startPt, (pt - startPt).Normalize());

			var intersections = new List<Intersection>();
			scene.SceneRoot.Intersect(ray, intersections);

			if (intersections.Count == 0) {
				return true;
			}

			double ptDistSq = (pt - startPt).LengthSquared;

			foreach (var iSec in intersections) {
				if (iSec.RayDistanceSqSigned.IsEpsilonGreaterThanZero() && iSec.RayDistanceSqSigned < ptDistSq) {
					return false;
				}
			}

			return true;
		}

	}
}
