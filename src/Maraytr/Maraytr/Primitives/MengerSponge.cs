using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Primitives {
	public class MengerSponge : IIntersectableObject {

		public int IterationsCount { get; set; }

		private Vector3[] normalsStorage = new Vector3[6] {
			-Vector3.XAxis,
			Vector3.XAxis,
			-Vector3.YAxis,
			Vector3.YAxis,
			-Vector3.ZAxis,
			Vector3.ZAxis,
		};

		public int Intersect(Ray ray, ICollection<Intersection> outIntersections) {
			return intersect(ray, outIntersections, 0);
		}

		private int intersect(Ray ray, ICollection<Intersection> outIntersections, int iteration) {
			++iteration;

			var intersections = new List<TempIntersection>();

			for (int planeCoordIndex = 0; planeCoordIndex < 3; ++planeCoordIndex) {
				if (ray.Direction[planeCoordIndex].IsAlmostZero()) {
					continue;  // Ray parallel to this set of planes.
				}

				int prevCoordIndex = (planeCoordIndex + 2) % 3;
				int nextCoordIndex = (planeCoordIndex + 1) % 3;
				double epsilon = FloatArithmeticUtils.DBL_EPSILON * iteration;

				for (int planePosition = 0; planePosition < 4; ++planePosition) {
					// Intersection with x/y/z plane.
					double t = (planePosition - ray.StartPoint[planeCoordIndex]) / ray.Direction[planeCoordIndex];
					if (t <= 0.0 || t < epsilon) {
						continue;  // Not interesting intersection.
					}

					var localPt = ray.GetPointAt(t);
					Debug.Assert(localPt[planeCoordIndex].IsAlmostEqualTo(planePosition));
					double prevCoord = localPt[prevCoordIndex];
					double nextCoord = localPt[nextCoordIndex];

					if (prevCoord < 0.0 || prevCoord >= 3.0 || nextCoord < 0.0 || nextCoord >= 3.0) {
						continue;  // Outside of sponge.
					}


					if (planePosition == 0 || planePosition == 3) {
						// Border planes.
						if (prevCoord >= 1.0 && prevCoord < 2.0 && nextCoord >= 1.0 && nextCoord < 2.0) {
							continue;  // Empty middle section of the sponge.
						}
					}
					else {
						// Middle planes.
						int intSum = (int)prevCoord + (int)nextCoord;
						if (intSum % 2 == 0) {
							continue;
						}
					}

					intersections.Add(new TempIntersection() {
						T = t,
						NormalIndex = (byte)(planeCoordIndex * 2 + planePosition % 2),
						PlanePos = (byte)planePosition,
						PlaneCoordIndex = (byte)planeCoordIndex,
						PrevCoord = prevCoord,
						NextCoord = nextCoord,
					});
				}
			}

			Debug.Assert(intersections.Count % 2 == 0);

			if (intersections.Count == 0) {
				return 0;
			}

			intersections.Sort((x, y) => x.T.CompareTo(y.T));


			if (iteration >= IterationsCount) {
				var firstIsec = intersections[0];

				outIntersections.Add(new Intersection(this, true, ray, firstIsec.T) {
					Normal = normalsStorage[firstIsec.NormalIndex],
				});
				return 1;
			}

			var childIsecs = new List<Intersection>();

			for (int i = 0; i < intersections.Count; i += 2) {
				var isec = intersections[i];
				Vector3 sectorCoords = getSectorCoords(isec);

				// Translate ray.
				Ray newRay = new Ray((ray.StartPoint - sectorCoords) * 3.0, ray.Direction, ray.RayWorldCoords);
				childIsecs.Clear();
				int isecCount = intersect(newRay, childIsecs, iteration);
				if (isecCount > 0) {
					foreach (var childIsec in childIsecs) {
						//childIsec.Position = (childIsec.Position * (1.0 / 3.0)) + sectorCoords;
						//childIsec.RayParameter = (childIsec.Position - ray.StartPoint).Length;
						//childIsec.Ray = ray;
						childIsec.RayParameter /= 3.0;
						outIntersections.Add(childIsec);
					}
					return isecCount;
				}
			}

			return 0;
		}

		public void CompleteIntersection(Intersection intersection) {
			// Normal and tex coord already computed.
		}

		protected Vector3 getSectorCoords(TempIntersection intersection) {
			int planeCoordIndex = intersection.PlaneCoordIndex;
			int planePos = intersection.PlanePos;
			int planePosSign = -(planePos % 2);  // 0 for planes 0 and 2, -1 for planes 1 and 3 (matches the normal).
			Vector3 sectorCoords = new Vector3();
			
			sectorCoords[(planeCoordIndex + 2) % 3] = Math.Floor(intersection.PrevCoord);
			sectorCoords[planeCoordIndex] = planePos + planePosSign;
			sectorCoords[(planeCoordIndex + 1) % 3] = Math.Floor(intersection.NextCoord);
			return sectorCoords;
		}

		protected class TempIntersection {
			public double T;
			public byte NormalIndex;
			public byte PlanePos;
			public byte PlaneCoordIndex;
			public double PrevCoord;
			public double NextCoord;
		}


	}
}
