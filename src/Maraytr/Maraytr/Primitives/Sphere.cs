using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg.Primitives {
	public class Sphere : IIntersectableObject {
				
		public int Intersect(Ray ray, ICollection<Intersection> outIntersections) {

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
			if (discrOver4 <= 0.0 || discrOver4.IsAlmostZero()) {
				return 0;  // 0 or 1 solution, but we want two.
			}

			double discrOver4Sqrt = Math.Sqrt(discrOver4);
			double tEnter = -sd - discrOver4Sqrt;
			double tLeave = -sd + discrOver4Sqrt;

			Debug.Assert(tEnter < tLeave);

			outIntersections.Add(new Intersection(this, true, ray, tEnter));
			outIntersections.Add(new Intersection(this, false, ray, tLeave));
			return 2;
		}

		public void CompleteIntersection(Intersection intersection) {

			Vector3 localIntPt = intersection.LocalIntersectionPt;

			intersection.Normal = localIntPt;

			intersection.TextureCoord.X = Math.Atan2(localIntPt.Z, localIntPt.X) / (2.0 * Math.PI) + 0.5;
			intersection.TextureCoord.Y = Math.Atan2(1, localIntPt.Y) / Math.PI;

		}

	}
}
