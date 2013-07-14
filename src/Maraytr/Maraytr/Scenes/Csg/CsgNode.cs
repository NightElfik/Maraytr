using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public abstract class CsgNode : IIntersectable {

		/// <summary>
		/// Parent node (null for root).
		/// </summary>
		public CsgNode Parent { get; set; }

		bool IsRootNode { get { return Parent == null; } }


		public abstract void PrecomputeWorldTransform(Matrix4Affine worldTransform);


		public abstract int Intersect(Ray ray, ICollection<Intersection> outIntersections);

	}

}
