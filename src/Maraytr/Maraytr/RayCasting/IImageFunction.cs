using Maraytr.Numerics;

namespace Maraytr.RayCasting {
	public interface IImageFunction {

		Size Size { get; }

		ColorRgbt GetSample(double x, double y, IntegrationState intState);

	}
}
