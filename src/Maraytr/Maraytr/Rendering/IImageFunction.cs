using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Rendering {
	public interface IImageFunction {

		Size Size { get; }

		ColorRgbt GetSample(double x, double y);

	}
}
