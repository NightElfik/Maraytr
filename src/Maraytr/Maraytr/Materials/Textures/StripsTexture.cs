using System;
using Maraytr.Numerics;

namespace Maraytr.Materials.Textures {
	public class StripsTexture : ITexture {

		public double Frequency { get; set; }
		public bool Orientation { get; set; }

		public ColorRgbt EvenColor { get; set; }
		public ColorRgbt OddColor { get; set; }

		public StripsTexture() {
			Frequency = 1;

			OddColor = new ColorRgbt(0.3f, 0.3f, 0.3f);
			EvenColor = new ColorRgbt(1, 1, 1);
		}


		public ColorRgbt GetColorAt(Vector2 coords) {

			double t = (Orientation ? coords.X : coords.Y) * Frequency;

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
