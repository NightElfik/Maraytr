
namespace Maraytr.Numerics {
	public struct ColorRgbt {

		public float R;
		public float G;
		public float B;
		public float T;


		public ColorRgbt(float r, float g, float b, float t = 0f) {
			R = r;
			G = g;
			B = b;
			T = t;
		}


		public static ColorRgbt operator +(ColorRgbt left, ColorRgbt right) {
			return new ColorRgbt(left.R + right.R, left.G + right.G, left.B + right.B, left.T + right.T);
		}

		public static ColorRgbt operator *(ColorRgbt left, ColorRgbt right) {
			return new ColorRgbt(left.R * right.R, left.G * right.G, left.B * right.B, left.T * right.T);
		}

		public static ColorRgbt operator *(ColorRgbt left, float right) {
			return new ColorRgbt(left.R * right, left.G * right, left.B * right, left.T * right);
		}

		public static ColorRgbt operator *(float left, ColorRgbt right) {
			return new ColorRgbt(left * right.R, left * right.G, left * right.B, left * right.T);
		}
		

	}
}
