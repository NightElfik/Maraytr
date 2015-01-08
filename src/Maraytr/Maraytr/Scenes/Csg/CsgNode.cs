using System.Collections.Generic;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public abstract class CsgNode : IIntersectable {

		protected List<CsgNode> childrens = new List<CsgNode>();

		/// <summary>
		/// Parent node (null for root).
		/// </summary>
		public CsgNode Parent { get; set; }

		public bool IsRoot { get { return Parent == null; } }

		public IEnumerable<CsgNode> Childrens { get { return childrens; } }
				
		public bool IsLeaf { get { return childrens.Count == 0; } }

		public int ChildrensCount { get { return childrens.Count; } }

		public Matrix4Affine LocalTransform { get; set; }
				
		
		public CsgNode() {
			LocalTransform = Matrix4Affine.CreateIdentity();
		}

		public CsgNode(Matrix4Affine localTransform) {
			LocalTransform = localTransform;
		}

		
		public void AddChild(CsgNode node) {
			childrens.Add(node);
		}
		
		public void AddChild(params CsgNode[] nodes) {
			childrens.AddRange(nodes);
		}

		public void AddChild(IEnumerable<CsgNode> nodes) {
			childrens.AddRange(nodes);
		}

		public bool RemoveChild(CsgNode node) {
			return childrens.Remove(node);
		}

		public void ClearChildrens() {
			childrens.Clear();
		}

		public virtual void PrecomputeTransformCaches(Matrix4Affine globalTransform) {		
			var t = globalTransform * LocalTransform;
			foreach (var child in childrens) {
				child.PrecomputeTransformCaches(t);
			}
		}


		public abstract int Intersect(Ray globalRay, IList<Intersection> outIntersections);

	}

}
