using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg.Primitives {
	public class CsgCube : CsgNode, IIntersectableObject {

		private Matrix4Affine transformToWorld;


		public IMaterial Material { get; set; }

		public override void PrecomputeWorldTransform(Matrix4Affine worldTransform) {
			transformToWorld = worldTransform;
		}



		public override void Intersect(Ray ray, ICollection<Intersection> outIntersections) {

			// http://www.siggraph.org/education/materials/HyperGraph/raytrace/rtinter3.htm
			double tMin = Double.NegativeInfinity;
			double tMax = Double.PositiveInfinity;

			char intersectePlaneMin = '\0';
			char intersectePlaneMax = '\0';

			// X axis
			if (ray.Direction.X.IsAlmostZero()) {
				if (ray.StartPoint.X < 0 || ray.StartPoint.X > 1) {
					return;
				}
			}
			else {
				double mul = 1.0 / ray.Direction.X;
				double t1 = -ray.StartPoint.X * mul;
				double t2 = t1 + mul;

				if (mul > 0.0) {
					if (t1 > tMin) {
						tMin = t1;
						intersectePlaneMin = 'X';
					}
					if (t2 < tMax) {
						tMax = t2;
						intersectePlaneMax = 'x';
					}
				}
				else {
					if (t2 > tMin) {
						tMin = t2;
						intersectePlaneMin = 'x';
					}
					if (t1 < tMax) {
						tMax = t1;
						intersectePlaneMax = 'X';
					}
				}

				if (tMax < 0.0 || tMin > tMax) {
					return;
				}
			}

			// Y axis
			if (ray.Direction.Y.IsAlmostZero()) {
				if (ray.StartPoint.Y < 0 || ray.StartPoint.Y > 1) {
					return;
				}
			}
			else {
				double mul = 1.0 / ray.Direction.Y;
				double t1 = -ray.StartPoint.Y * mul;
				double t2 = t1 + mul;

				if (mul > 0.0) {
					if (t1 > tMin) {
						tMin = t1;
						intersectePlaneMin = 'Y';
					}
					if (t2 < tMax) {
						tMax = t2;
						intersectePlaneMax = 'y';
					}
				}
				else {
					if (t2 > tMin) {
						tMin = t2;
						intersectePlaneMin = 'y';
					}
					if (t1 < tMax) {
						tMax = t1;
						intersectePlaneMax = 'Y';
					}
				}

				if (tMax < 0.0 || tMin > tMax) {
					return;
				}
			}

			// Z axis
			if (ray.Direction.Z.IsAlmostZero()) {
				if (ray.StartPoint.Z < 0 || ray.StartPoint.Z > 1) {
					return;
				}
			}
			else {
				double mul = 1.0 / ray.Direction.Z;
				double t1 = -ray.StartPoint.Z * mul;
				double t2 = t1 + mul;

				if (mul > 0.0) {
					if (t1 > tMin) {
						tMin = t1;
						intersectePlaneMin = 'Z';
					}
					if (t2 < tMax) {
						tMax = t2;
						intersectePlaneMax = 'z';
					}
				}
				else {
					if (t2 > tMin) {
						tMin = t2;
						intersectePlaneMin = 'z';
					}
					if (t1 < tMax) {
						tMax = t1;
						intersectePlaneMax = 'Z';
					}
				}

				if (tMax < 0.0 || tMin > tMax) {
					return;
				}
			}

			Debug.Assert(!double.IsInfinity(tMin) && !double.IsInfinity(tMax));

			Vector3 enterPosWorld = transformToWorld.Transform(ray.StartPoint + tMin * ray.Direction);
			Vector3 leavePosWorld = transformToWorld.Transform(ray.StartPoint + tMax * ray.Direction);

			double enterDistSqSgn = (enterPosWorld - ray.RayWorldCoords.StartPoint).LengthSquared * (tMin >= 0.0 ? 1 : -1);
			double leaveDistSqSgn = (leavePosWorld - ray.RayWorldCoords.StartPoint).LengthSquared * (tMax >= 0.0 ? 1 : -1);

			outIntersections.Add(new Intersection(this, true, ray, tMin, enterPosWorld, enterDistSqSgn, intersectePlaneMin));
			outIntersections.Add(new Intersection(this, false, ray, tMin, leavePosWorld, leaveDistSqSgn, intersectePlaneMax));
		}


		public void CompleteIntersection(Intersection intersection) {

			Vector3 localIsec = intersection.LocalIntersectionPt;

			switch ((char)intersection.AdditionalData) {
				case 'x': intersection.Normal = Vector3.XAxis; break;
				case 'X': intersection.Normal = -Vector3.XAxis; break;
				case 'y': intersection.Normal = Vector3.YAxis; break;
				case 'Y': intersection.Normal = -Vector3.YAxis; break;
				case 'z': intersection.Normal = Vector3.ZAxis; break;
				case 'Z': intersection.Normal = -Vector3.ZAxis; break;
				default: throw new InvalidOperationException("Additional data should contain information about intersected plane.");
			}

			switch ((char)intersection.AdditionalData) {
				case 'x':
				case 'X':
					intersection.TextureCoord.X = localIsec.Z;
					intersection.TextureCoord.Y = localIsec.Y;
					break;
				case 'y':
				case 'Y':
					intersection.TextureCoord.X = localIsec.X;
					intersection.TextureCoord.Y = localIsec.Z;
					break;
				case 'z':
				case 'Z':
					intersection.TextureCoord.X = localIsec.X;
					intersection.TextureCoord.Y = localIsec.Y;
					break;
				default: throw new InvalidOperationException("Additional data should contain information about intersected plane.");
			}

		}

	}
}
