using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Cameras {
	public interface ICamera {

		Size Size { get; set; }

		Ray GetRay(double x, double y);

	}
}
