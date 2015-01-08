using System.Collections.Generic;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public abstract class CsgNode : IIntersectable {

		protected List<CsgNode> children = new List<CsgNode>();

		/// <summary>
		/// Parent node (null for root).
		/// </summary>
		public CsgNode Parent { get; set; }

		public bool IsRoot { get { return Parent == null; } }

		public IEnumerable<CsgNode> Children { get { return children; } }
				
		public bool IsLeaf { get { return children.Count == 0; } }

		public int ChildrenCount { get { return children.Count; } }

		public Matrix4Affine LocalTransform { get; set; }
				
		
		public CsgNode() {
			LocalTransform = Matrix4Affine.CreateIdentity();
		}

		public CsgNode(Matrix4Affine localTransform) {
			LocalTransform = localTransform;
		}

		
		public void AddChild(CsgNode node) {
			children.Add(node);
		}
		
		public void AddChild(params CsgNode[] nodes) {
			children.AddRange(nodes);
		}

		public void AddChild(IEnumerable<CsgNode> nodes) {
			children.AddRange(nodes);
		}

		public bool RemoveChild(CsgNode node) {
			return children.Remove(node);
		}

		public void ClearChildren() {
			children.Clear();
		}

		public virtual void PrecomputeTransformCaches(Matrix4Affine globalTransform) {		
			var t = globalTransform * LocalTransform;
			foreach (var child in children) {
				child.PrecomputeTransformCaches(t);
			}
		}


		public abstract int Intersect(Ray globalRay, IList<Intersection> outIntersections);

	}

}
