using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg.Primitives {
	public class CsgHalfSpace : CsgNode, IIntersectableObject {

		private Matrix4Affine transformToWorld;


		public CsgHalfSpace() {
			Width = double.PositiveInfinity;
		}

		public double Width { get; set; }

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

			if (ray.Direction.Y.IsAlmostZero()) {
				return;
			}

			double tZero = -ray.StartPoint.Y / ray.Direction.Y;
			double tWid = (-Width - ray.StartPoint.Y) / ray.Direction.Y;

			Vector3 tZeroPosWorld = transformToWorld.Transform(ray.StartPoint + tZero * ray.Direction);
			Vector3 tWidPosWorld = double.IsInfinity(tWid)
				? (ray.Direction.Y > 0.0 ? Vector3.NegativeInfinity : Vector3.PositiveInfinity)
				: (transformToWorld.Transform(ray.StartPoint + tWid * ray.Direction));

			double tZeroDistSgnSq = (tZeroPosWorld - ray.RayWorldCoords.StartPoint).LengthSquared * (tZero >= 0.0 ? 1 : -1);
			double tWidDistSgnSq = (tWidPosWorld - ray.RayWorldCoords.StartPoint).LengthSquared * (tWid >= 0.0 ? 1 : -1);

			if (ray.Direction.Y > 0.0) {
				// ray going away from half plane
				outIntersections.Add(new Intersection(this, true, ray, tWid, tWidPosWorld, tWidDistSgnSq));
				outIntersections.Add(new Intersection(this, false, ray, tZero, tZeroPosWorld, tZeroDistSgnSq));
			}
			else {
				// ray going into half plane
				outIntersections.Add(new Intersection(this, true, ray, tZero, tZeroPosWorld, tZeroDistSgnSq));
				outIntersections.Add(new Intersection(this, false, ray, tWid, tWidPosWorld, tWidDistSgnSq));
			}

		}

		public void CompleteIntersection(Intersection intersection) {

			Vector3 localIntPt = intersection.Ray.StartPoint + intersection.RayParameter * intersection.Ray.Direction;
			Debug.Assert(localIntPt.Y.IsAlmostZero());

			intersection.Normal = new Vector3(0, 1, 0);

			intersection.TextureCoord.X = localIntPt.X;
			intersection.TextureCoord.Y = localIntPt.Z;

		}

	}
}
