using System;

namespace Maraytr {
	public static class MathHelper {

		public const double DegToRad = Math.PI / 180.0;
		public const double RadToDeg = 180.0 / Math.PI;

		public static int Clamp(this int value, int min, int max) {
			if (value < min) {
				return min;
			}
			else if (value > max) {
				return max;
			}
			else {
				return value;
			}
		}

		public static float Clamp(this float value, float min, float max) {
			if (value < min) {
				return min;
			}
			else if (value > max) {
				return max;
			}
			else {
				return value;
			}
		}

		public static double Clamp(this double value, double min, double max) {
			if (value < min) {
				return min;
			}
			else if (value > max) {
				return max;
			}
			else {
				return value;
			}
		}

		public static float Clamp01(this float value) {
			if (value < 0) {
				return 0;
			}
			else if (value > 1) {
				return 1;
			}
			else {
				return value;
			}
		}

		public static double Clamp01(this double value) {
			if (value < 0) {
				return 0;
			}
			else if (value > 1) {
				return 1;
			}
			else {
				return value;
			}
		}

	}
}
