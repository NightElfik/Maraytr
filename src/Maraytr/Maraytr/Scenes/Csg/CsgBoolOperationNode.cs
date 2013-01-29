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

				//if (i == childToWorldTransforms.Count) {
				//	childToWorldTransforms.Add(childToWorld);
				//}
				//else {
				//	childToWorldTransforms[i] = childToWorld;
				//}

				childrens[i].PrecomputeWorldTransform(childToWorld);
			}
		}


		public override void Intersect(Ray ray, ICollection<Intersection> outIntersections) {

			Contract.Requires<ArgumentNullException>(outIntersections != null);
			Contract.Requires<InvalidOperationException>(Operation == CsgBoolOperation.Difference && ChildrensCount == 2 || Operation != CsgBoolOperation.Difference);
			Contract.Ensures(outIntersections.Count % 2 == 0);  // number of intersections is allways even (enter and exit are paired).

			var intersections = new List<Intersection>();

			if (Operation == CsgBoolOperation.Difference) {
				Debug.Assert(childrens.Count == 2);

				childrens[0].Intersect(ray.Transform(childTransforms[0]), intersections);
				if (intersections.Count == 0) {
					return;
				}

				var minuends = intersections.Select(x => x.IntersectedObject).ToList();

				childrens[1].Intersect(ray.Transform(childTransforms[1]), intersections);
				intersections.Sort();

				computeDifference(intersections, minuends, outIntersections);
				return;
			}

			// gather intersections from all childs
			for (int i = 0; i < childrens.Count; ++i) {
				childrens[i].Intersect(ray.Transform(childTransforms[i]), intersections);
			}

			if (intersections.Count == 0) {
				return;
			}

			intersections.Sort();

			switch (Operation) {
				case CsgBoolOperation.Union: computeUnion(intersections, outIntersections); break;
				case CsgBoolOperation.Intersection: throw new NotImplementedException();
				case CsgBoolOperation.Xor: throw new NotImplementedException();
				default: throw new InvalidOperationException("Invalid operation of CSG node.");
			}
		}


		private void computeUnion(List<Intersection> intersections, ICollection<Intersection> outIntersections) {

			int insideCount = 0;

			foreach (var intersec in intersections) {

				if (insideCount == 0) {
					Debug.Assert(intersec.IsEnter);  // the first intersection should be entering
					outIntersections.Add(intersec);  // add entering intersection to the result
					++insideCount;
				}
				else {
					if (intersec.IsEnter) {
						++insideCount;
					}
					else {
						if (insideCount == 1) {
							outIntersections.Add(intersec);  // add leaving intersection to the result
						}
						--insideCount;
					}
				}

			}

			Debug.Assert(insideCount == 0);
		}

		private void computeDifference(List<Intersection> intersections, List<IIntersectableObject> minuends, ICollection<Intersection> outIntersections) {


			// minuend − subtrahend = difference
			bool inMinuend = false;
			bool inSubtrahend = false;

			foreach (var intersec in intersections) {
				bool isMinuend = minuends.Contains(intersec.IntersectedObject);

				if (isMinuend) {
					if (intersec.IsEnter) {
						Debug.Assert(inMinuend == false);
						inMinuend = true;
						if (!inSubtrahend) {
							// minuend started normally
							outIntersections.Add(intersec);
						}
						// else do nothing -- minuend started covered by subtrahend
					}
					else {
						Debug.Assert(inMinuend == true);
						inMinuend = false;
						if (!inSubtrahend) {
							// minuend ended normally
							outIntersections.Add(intersec);
						}
						// else do nothing -- minuend ended covered by subtrahend
					}
				}
				else {
					if (intersec.IsEnter) {
						Debug.Assert(inSubtrahend == false);
						inSubtrahend = true;
						if (inMinuend) {
							// subtrahend started in minuend, end minuend
							intersec.IsEnter = false;
							intersec.InverseNormal ^= true;
							outIntersections.Add(intersec);
						}
						// else do nothing -- subtrahend started alone
					}
					else {
						Debug.Assert(inSubtrahend == true);
						inSubtrahend = false;
						if (inMinuend) {
							// subtrahend ended in minuend, start minuend
							intersec.IsEnter = true;
							intersec.InverseNormal ^= true;
							outIntersections.Add(intersec);
						}
						// else do nothing -- minuend ended covered by subtrahend
					}
				}
			}

		}

	}
}
