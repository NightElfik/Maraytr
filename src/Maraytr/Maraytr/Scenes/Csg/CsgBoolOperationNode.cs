using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public class CsgBoolOperationNode : CsgNode {


		public CsgBoolOperation Operation { get; set; }


		
		public CsgBoolOperationNode(CsgBoolOperation boolOperation) : base() {
			Operation = boolOperation;
		}

		public CsgBoolOperationNode(CsgBoolOperation boolOperation, Matrix4Affine localTransform) : base(localTransform) {
			Operation = boolOperation;
		}

		
		public override int Intersect(Ray globalRay, IList<Intersection> outIntersections) {
			Contract.Requires<ArgumentNullException>(ChildrensCount >= 1);
			Contract.Requires<ArgumentNullException>(outIntersections != null);
			Contract.Ensures(outIntersections.Count % 2 == 0);  // The number of intersections is always even (enters and exits are paired).

			var intersections = new List<Intersection>();
			
			if (Operation == CsgBoolOperation.Difference) {
				childrens[0].Intersect(globalRay, intersections);
				if (intersections.Count == 0) {
					return 0;
				}

				var minuends = intersections.Select(x => x.IntersectedObject).ToList();

				for (int i = 1; i < childrens.Count; ++i) {
					childrens[i].Intersect(globalRay, intersections);
				}

				intersections.Sort();
				return computeDifference(intersections, minuends, outIntersections);
			}

			// Gather intersections from all childs.
			for (int i = 0; i < childrens.Count; ++i) {
				childrens[i].Intersect(globalRay, intersections);
			}

			if (intersections.Count == 0) {
				return 0;
			}

			intersections.Sort();

			switch (Operation) {
				case CsgBoolOperation.Union: return computeIntersection(intersections, outIntersections, 1);
				case CsgBoolOperation.Intersection: return computeIntersection(intersections, outIntersections, childrens.Count);
				case CsgBoolOperation.Xor: throw new NotImplementedException();
				default: throw new InvalidOperationException("Invalid operation of CSG node.");
			}
		}
		

		private int computeIntersection(List<Intersection> intersections, ICollection<Intersection> outIntersections, int requiredObjectsCount) {

			int insideCount = 0;
			int count = 0;

			foreach (var intersec in intersections) {
				if (intersec.IsEnter) {
					++insideCount;
					if (insideCount == requiredObjectsCount) {
						outIntersections.Add(intersec);  // Add entering intersection to the result.
						++count;
					}
				}
				else {
					if (insideCount == requiredObjectsCount) {
						outIntersections.Add(intersec);  // Add leaving intersection to the result.
						++count;
					}
					--insideCount;
				}
			}

			Debug.Assert(insideCount == 0);
			return count;
		}

		private int computeDifference(List<Intersection> intersections, List<IIntersectableObject> minuends, ICollection<Intersection> outIntersections) {


			// minuend − subtrahend = difference
			bool inMinuend = false;
			int inSubtrahendCount = 0;
			int count = 0;

			foreach (var intersec in intersections) {
				bool isMinuend = minuends.Contains(intersec.IntersectedObject);

				if (isMinuend) {
					if (intersec.IsEnter) {
						Debug.Assert(inMinuend == false);
						inMinuend = true;
						if (inSubtrahendCount == 0) {
							// minuend started normally
							outIntersections.Add(intersec);
							++count;
						}
						// else do nothing -- minuend started covered by subtrahend
					}
					else {
						Debug.Assert(inMinuend == true);
						inMinuend = false;
						if (inSubtrahendCount == 0) {
							// minuend ended normally
							outIntersections.Add(intersec);
							++count;
						}
						// else do nothing -- minuend ended covered by subtrahend
					}
				}
				else {
					if (intersec.IsEnter) {
						++inSubtrahendCount;
						if (inSubtrahendCount == 1 && inMinuend) {
							// subtrahend started in minuend, end minuend
							intersec.IsEnter = false;
							intersec.InverseNormal ^= true;
							outIntersections.Add(intersec);
							++count;
						}
						// else do nothing -- subtrahend started alone
					}
					else {
						Debug.Assert(inSubtrahendCount > 0);
						--inSubtrahendCount;
						if (inSubtrahendCount == 0 && inMinuend) {
							// subtrahend ended in minuend, start minuend
							intersec.IsEnter = true;
							intersec.InverseNormal ^= true;
							outIntersections.Add(intersec);
							++count;
						}
						// else do nothing -- minuend ended covered by subtrahend
					}
				}
			}
			return count;
		}

	}
}
