using System.Collections.Generic;
using Maraytr.Materials;

namespace Maraytr.RayCasting {
	public interface IIntersectable {

		void Intersect(Ray ray, ICollection<Intersection> outIntersections);

	}

	public interface IIntersectableObject : IIntersectable {

		IMaterial Material { get; set; }

		void CompleteIntersection(Intersection intersection);

	}
}
