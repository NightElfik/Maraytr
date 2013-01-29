using System.Collections.Generic;
using Maraytr.Materials;
using Maraytr.Numerics;

namespace Maraytr.RayCasting {
	public interface IIntersectable {

		void Intersect(Ray ray, ICollection<Intersection> outIntersections);

	}

	public interface IIntersectableObject : IIntersectable {

		IMaterial Material { get; set; }

		//Matrix4Affine TransformToWorld { get; }

		IntersectionDetails ComputeIntersectionDetails(Intersection intersection);

	}
}
