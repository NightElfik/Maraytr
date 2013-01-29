using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg.Primitives {
	public class CsgHalfSpace : CsgNode, IIntersectableObject {

		private Matrix4Affine transformToWorld;
		

		public IMaterial Material { get; set; }

		public override void PrecomputeWorldTransform(Matrix4Affine worldTransform) {
			transformToWorld = worldTransform;
		}

		public override void Intersect(Ray ray, ICollection<Intersection> outIntersections) {

			// Plane XZ: x = [x, 0, z] (y = 0)
			// Ray width start s and direction d (normalized): x = s + t d
			// Combined:
			// 0 = Sy + t Dy
			// -Sy / Dy = t

			if (ray.Direction.Y.IsAlmostEqualTo(0.0)) {
				return;
			}

			double tZero = -ray.StartPoint.Y / ray.Direction.Y;
			Vector3 tPosWorld = transformToWorld.Transform(ray.StartPoint + tZero * ray.Direction);
			double tDistSgnSq = (tPosWorld - ray.RayWorldCoords.StartPoint).LengthSquared * (tZero >= 0.0 ? 1 : -1);

			if (ray.Direction.Y > 0.0) {
				// ray going away from half plane
				outIntersections.Add(new Intersection(this, true, ray, double.NegativeInfinity, Vector3.NegativeInfinity, double.NegativeInfinity));
				outIntersections.Add(new Intersection(this, false, ray, tZero, tPosWorld, tDistSgnSq));
			}
			else {
				// ray going into half plane
				outIntersections.Add(new Intersection(this, true, ray, tZero, tPosWorld, tDistSgnSq));
				outIntersections.Add(new Intersection(this, false, ray, double.PositiveInfinity, Vector3.PositiveInfinity, double.PositiveInfinity));
			}		

		}

		public IntersectionDetails ComputeIntersectionDetails(Intersection intersection) {

			var details = new IntersectionDetails();

			Vector3 localIntPt = intersection.Ray.StartPoint + intersection.RayParameter * intersection.Ray.Direction;

			details.Normal = new Vector3(0, 1, 0);

			details.TextureCoord.X = localIntPt.X;
			details.TextureCoord.Y = localIntPt.Z;

			Debug.Assert(localIntPt.Y.IsAlmostEqualTo(0.0));

			return details;

		}

	}
}
