using System;
using System.Collections.Generic;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg.Primitives {
	public class CsgSphere : CsgNode, IIntersectableObject {

		private Matrix4Affine transformToWorld;


		public IMaterial Material { get; set; }

		public override void PrecomputeWorldTransform(Matrix4Affine worldTransform) {
			transformToWorld = worldTransform;
		}

		public override void Intersect(Ray ray, ICollection<Intersection> outIntersections) {

			// Sphere with center c and radius r: |x - c|^2 = r^2
			// Ray width start s and direction d (normalized): x = s + t d
			// But we have unit sphere in coord center: |x|^2 = 1
			// Combined:
			// |s + t d|^2 = 1
			// (s + t d).(s + t d) = 1
			// t^2 d.d + 2 t s.d + s.s - 1 = 0
			// d is unit vector thus d.d = 1
			// t^2 + 2 t s.d + s.s - 1 = 0
			//
			// D = b^2 - 4 a c
			// D = 4 (s.d)^2 - 4 s.s + 4
			// D / 4 = (s.d)^2 - s.s + 1
			//
			// t = (-b +- sqrt(D)) / (2 a)
			// t = (-2 s.d +- sqrt( 4 (s.d)^2 - 4 s.s + 4)) / 2
			// t = -s.d +- sqrt((s.d)^2 - s.s + 1)

			double sd = ray.StartPoint.Dot(ray.Direction);
			double ss = ray.StartPoint.Dot(ray.StartPoint);

			double discrOver4 = sd * sd - ss + 1;
			if (discrOver4 <= 0.0 || discrOver4.IsAlmostEqualTo(0.0)) {
				return;  // 0 or 1 solution, but we want two
			}

			double discrOver4Sqrt = Math.Sqrt(discrOver4);
			double tEnter = -sd - discrOver4Sqrt;
			double tLeave = -sd + discrOver4Sqrt;

			Vector3 enterPosWorld = transformToWorld.Transform(ray.StartPoint + tEnter * ray.Direction);
			Vector3 leavePosWorld = transformToWorld.Transform(ray.StartPoint + tLeave * ray.Direction);
			
			double enterDistSqSgn = (enterPosWorld - ray.RayWorldCoords.StartPoint).LengthSquared * (tEnter >= 0.0 ? 1 : -1);
			double leaveDistSqSgn = (leavePosWorld - ray.RayWorldCoords.StartPoint).LengthSquared * (tLeave >= 0.0 ? 1 : -1);

			outIntersections.Add(new Intersection(this, true, ray, tEnter, enterPosWorld, enterDistSqSgn));
			outIntersections.Add(new Intersection(this, false, ray, tLeave, leavePosWorld, leaveDistSqSgn));

		}

		public IntersectionDetails ComputeIntersectionDetails(Intersection intersection) {

			var details = new IntersectionDetails();

			Vector3 localIntPt = intersection.Ray.StartPoint + intersection.RayParameter * intersection.Ray.Direction;

			details.Normal = GeometryHelper.TransformNormal(localIntPt, transformToWorld);

			details.TextureCoord.X = Math.Atan2(localIntPt.Y, localIntPt.X) / (2.0 * Math.PI) + 0.5;
			details.TextureCoord.Y = Math.Atan2(1, localIntPt.Z) / Math.PI;

			return details;

		}

	}
}
