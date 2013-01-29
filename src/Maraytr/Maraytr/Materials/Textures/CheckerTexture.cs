using System;
using Maraytr.Numerics;

namespace Maraytr.Materials.Textures {
	public class CheckerTexture : ITexture {

		public double UFrequency { get; set; }
		public double VFrequency { get; set; }

		public ColorRgbt EvenColor { get; set; }
		public ColorRgbt OddColor { get; set; }

		public CheckerTexture() {
			UFrequency = 1;
			VFrequency = 1;

			EvenColor = new ColorRgbt(1, 1, 1);
		}


		public ColorRgbt GetColorAt(Vector2 coords) {

			double u = coords.X * UFrequency;
			double v = coords.Y * VFrequency;

			long ui = (long)Math.Floor(u);
			long vi = (long)Math.Floor(v);

			if (((ui + vi) & 1) == 0) {
				return EvenColor;
			}
			else {
				return OddColor;
			}

		}

	}
}
