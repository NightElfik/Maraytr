using System;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Materials.Textures {
	public class StripsTexture2D : ITexture {

		public double Frequency { get; set; }
		public bool Orientation { get; set; }

		public ColorRgbt EvenColor { get; set; }
		public ColorRgbt OddColor { get; set; }

		public StripsTexture2D() {
			Frequency = 1;

			OddColor = new ColorRgbt(0.3f, 0.3f, 0.3f);
			EvenColor = new ColorRgbt(1, 1, 1);
		}


		public ColorRgbt GetColorAt(Intersection intersection) {

			double t = (Orientation ? intersection.TextureCoord.X : intersection.TextureCoord.Y) * Frequency;

			long ti = (long)Math.Floor(t);

			if ((ti & 1) == 0) {
				return EvenColor;
			}
			else {
				return OddColor;
			}

		}

	}
}
