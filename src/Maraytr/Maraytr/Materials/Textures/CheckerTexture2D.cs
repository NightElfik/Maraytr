using System;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Materials.Textures {
	public class CheckerTexture2D : ITexture {

		public double UFrequency { get; set; }
		public double VFrequency { get; set; }

		public ColorRgbt EvenColor { get; set; }
		public ColorRgbt OddColor { get; set; }

		public CheckerTexture2D() {
			UFrequency = 1;
			VFrequency = 1;

			OddColor = new ColorRgbt(0.3f, 0.3f, 0.3f);
			EvenColor = new ColorRgbt(1, 1, 1);
		}


		public ColorRgbt GetColorAt(Intersection intersection) {

			double u = intersection.TextureCoord.X * UFrequency;
			double v = intersection.TextureCoord.Y * VFrequency;

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
