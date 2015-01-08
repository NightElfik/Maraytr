using System.Collections.Generic;
using Maraytr.Materials;

namespace Maraytr.RayCasting {
	public interface IIntersectable {

		int Intersect(Ray ray, IList<Intersection> outIntersections);

	}

	public interface IIntersectableObject : IIntersectable {
		
		void CompleteIntersection(Intersection intersection);

	}
}
