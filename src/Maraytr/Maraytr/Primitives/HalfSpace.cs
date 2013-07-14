using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg.Primitives {
	public class HalfSpace : IIntersectableObject {
		

		public HalfSpace() {
			Width = double.PositiveInfinity;
		}

		public double Width { get; set; }

		public IMaterial Material { get; set; }


		public int Intersect(Ray ray, ICollection<Intersection> outIntersections) {

			// Plane XZ: x = [x, 0, z] (y = 0)
			// Ray width start s and direction d (normalized): x = s + t d
			// Combined:
			// 0 = Sy + t Dy
			// -Sy / Dy = t

			if (ray.Direction.Y.IsAlmostZero()) {
				return 0;
			}

			double tZero = -ray.StartPoint.Y / ray.Direction.Y;
			double tWid = (-Width - ray.StartPoint.Y) / ray.Direction.Y;

			if (ray.Direction.Y > 0.0) {
				// ray going away from half plane
				outIntersections.Add(new Intersection(this, true, ray, tWid, false));
				outIntersections.Add(new Intersection(this, false, ray, tZero, true));
			}
			else {
				// ray going into half plane
				outIntersections.Add(new Intersection(this, true, ray, tZero, true));
				outIntersections.Add(new Intersection(this, false, ray, tWid, false));
			}
			return 2;
		}

		public void CompleteIntersection(Intersection intersection) {

			Vector3 localIntPt = intersection.Ray.StartPoint + intersection.RayParameter * intersection.Ray.Direction;

			if ((bool)intersection.AdditionalData) { // top intersection
				intersection.Normal = Vector3.YAxis;
			}
			else {  // bottom intersection
				intersection.Normal = -Vector3.YAxis;
			}

			intersection.TextureCoord.X = localIntPt.X;
			intersection.TextureCoord.Y = localIntPt.Z;

		}

	}
}
