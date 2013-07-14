using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Rendering {
	public class SupersamplingImageFunction : IImageFunction {


		protected IImageFunction imageFunction;

		public SupersamplingImageFunction(IImageFunction imgFunc) {
			imageFunction = imgFunc;
		}

		public int SuperSampling { get; set; }

		public Size Size { get { return imageFunction.Size; } }


		public ColorRgbt GetSample(double x, double y, IntegrationState intState) {

			int ss = SuperSampling;
			double fraction = 1.0 / ss;

			ColorRgbt colorSum = new ColorRgbt();

			for (int dy = 0; dy < ss; ++dy) {
				double yCoord = y + dy * fraction;
				for (int dx = 0; dx < ss; ++dx) {
					colorSum += imageFunction.GetSample(x + dx * fraction, yCoord, new IntegrationState(dy * ss + dx, ss));
				}
			}

			return colorSum * (1.0f / (ss * ss));

		}

	}
}
