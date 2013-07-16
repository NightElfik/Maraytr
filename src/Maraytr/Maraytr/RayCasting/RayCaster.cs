using System.Collections.Generic;
using Maraytr.Lights;
using Maraytr.Numerics;
using Maraytr.Rendering;
using Maraytr.Scenes;

namespace Maraytr.RayCasting {
	public class RayCaster : IImageFunction {

		protected Scene scene;


		public RayCaster(Scene scene) {
			this.scene = scene;
		}

		public bool CountShadows { get; set; }

		public Size Size { get { return scene.Camera.Size; } }

		public bool CountAmbientOcclusion { get; set; }

		public int AmbientOcclusionSamplesCount { get; set; }

		
		public virtual ColorRgbt GetSample(double x, double y, IntegrationState intState) {

			Ray ray = scene.Camera.GetRay(x, y);
			if (ray == null) {
				return scene.BgColor;
			}

			var isec = getIntersection(ray);
			if (isec == null) {
				return scene.BgColor;
			}

			return evaluateIntersection(isec, intState);
		}

		public Intersection GetIntersectionAt(double x, double y) {
			return getIntersection(scene.Camera.GetRay(x, y));
		}

		protected Intersection getIntersection(Ray ray) {
			
			var intersections = new List<Intersection>();
			scene.SceneRoot.Intersect(ray, intersections);

			if (intersections.Count == 0) {
				return null;
			}

			Intersection intersec = null;
			foreach (var iSec in intersections) {
				if (iSec.RayDistanceSqSigned.IsEpsilonGreaterThanZero() && iSec.IsEnter) {
					if (intersec == null || iSec.RayDistanceSqSigned < intersec.RayDistanceSqSigned) {
						intersec = iSec;
					}
				}
			}

			return intersec;
		}


		protected ColorRgbt evaluateIntersection(Intersection intersec, IntegrationState intState) {

			intersec.CompleteIntersection();

			Vector3 viewerDirection = -intersec.Ray.RayWorldCoords.Direction;
			Vector3 intersecPos = intersec.Position;
			Vector3 surfaceNormal = intersec.Normal;
			if (intersec.InverseNormal) {
				intersec.Normal *= -1.0;
			}
			surfaceNormal.NormalizeThis();

			if (CountAmbientOcclusion) {

			}

			var mat = intersec.Material;
			ColorRgbt baseColor = mat.Texture == null
				? mat.BaseColor
				: mat.Texture.GetColorAt(intersec.TextureCoord) * mat.BaseColor;
			ColorRgbt totalColor = scene.AmbientLight * baseColor;
			
			foreach (var lightSource in scene.Lights) {

				Vector3 lightPos = lightSource.GetPosition(intState);

				if (CountShadows && !isPointDirectlyVisibleFrom(intersecPos, lightPos)) {
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
