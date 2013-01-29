using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public abstract class CsgNode : IIntersectable {


		//protected Matrix4Affine transformToParent;
		//protected Matrix4Affine transformFromParent;
		//protected Matrix4Affine transformToWorld;


		/// <summary>
		/// Parent node (null for root).
		/// </summary>
		public CsgNode Parent { get; set; }

		bool IsRootNode { get { return Parent == null; } }

		//public Matrix4Affine TransformToParent {
		//	get { return transformToParent; }
		//	set {
		//		Contract.Requires<ArgumentException>(!value.Determinant().IsAlmostEqualTo(0.0));
		//		transformToParent = value;
		//	}
		//}

		//public Matrix4Affine TransformFromParent {
		//	get { return transformFromParent; }
		//	set {
		//		Contract.Requires<ArgumentException>(!value.Determinant().IsAlmostEqualTo(0.0));
		//		transformFromParent = value;
		//	}
		//}

		//public Matrix4Affine TransformToWorld {
		//	get { return transformToWorld; }
		//}


		public abstract void PrecomputeWorldTransform(Matrix4Affine worldTransform);


		public abstract void Intersect(Ray ray, ICollection<Intersection> outIntersections);

	}

}
