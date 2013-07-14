using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public class CsgBoolOperationNode : CsgNode {

		private List<CsgNode> childrens = new List<CsgNode>();
		private List<Matrix4Affine> childTransforms = new List<Matrix4Affine>();
		//private List<Matrix4Affine> childToWorldTransforms = new List<Matrix4Affine>();


		public CsgBoolOperationNode(CsgBoolOperation boolOperation) {
			Operation = boolOperation;
		}

		public CsgBoolOperation Operation { get; set; }

		public int ChildrensCount { get { return childrens.Count; } }

		//public IEnumerable<CsgNode> Childrens { get { return childrens; } }

		//public IEnumerable<Matrix4> ChildTransforms { get { return childTransforms; } }


		public void AddChildNode(CsgNode node, Matrix4Affine transformation) {
			childrens.Add(node);
			childTransforms.Add(transformation.Inverse());
		}


		public override void PrecomputeWorldTransform(Matrix4Affine worldTransform) {
			for (int i = 0; i < childrens.Count; ++i) {
				var inv = childTransforms[i].Inverse();
				var childToWorld = worldTransform * inv;
				childrens[i].PrecomputeWorldTransform(childToWorld);
			}
		}


		public override int Intersect(Ray ray, ICollection<Intersection> outIntersections) {

			Contract.Requires<ArgumentNullException>(ChildrensCount >= 1);
			Contract.Requires<ArgumentNullException>(outIntersections != null);
			Contract.Ensures(outIntersections.Count % 2 == 0);  // number of intersections is always even (enter and exit are paired).

			var intersections = new List<Intersection>();

			if (Operation == CsgBoolOperation.Difference) {
				childrens[0].Intersect(ray.Transform(childTransforms[0]), intersections);
				if (intersections.Count == 0) {
					return 0;
				}

				var minuends = intersections.Select(x => x.IntersectedObject).ToList();

				for (int i = 1; i < childrens.Count; ++i) {
					childrens[i].Intersect(ray.Transform(childTransforms[i]), intersections);
				}

				intersections.Sort();
				return computeDifference(intersections, minuends, outIntersections);
			}

			// gather intersections from all childs
			for (int i = 0; i < childrens.Count; ++i) {
				childrens[i].Intersect(ray.Transform(childTransforms[i]), intersections);
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
						outIntersections.Add(intersec);  // add entering intersection to the result
						++count;
					}
				}
				else {
					if (insideCount == requiredObjectsCount) {
						outIntersections.Add(intersec);  // add leaving intersection to the result
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
