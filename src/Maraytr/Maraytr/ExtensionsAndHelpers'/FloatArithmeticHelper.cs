using System;
using Maraytr.Numerics;

namespace Maraytr {
	public static class FloatArithmeticHelper {

		public const double DBL_EPSILON = 1.0E-07;
		public const float SGL_EPSILON = 1.0E-05f;


		public static bool IsAlmostZero(this double value) {
			return value < 10.0 * DBL_EPSILON && value > -10.0 * DBL_EPSILON;
		}

		public static bool IsAlmostEqualTo(this double value1, double value2) {

			// in case they are Infinities (then epsilon check does not work)
			if (value1 == value2) {
				return true;
			}

			// This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < DBL_EPSILON
			double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * DBL_EPSILON;
			double delta = value1 - value2;
			return (-eps < delta) && (eps > delta);

		}

		public static bool IsAlmostEqualTo(this float value1, float value2) {

			// in case they are Infinities (then epsilon check does not work)
			if (value1 == value2) {
				return true;
			}

			// This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < SGL_EPSILON
			double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * SGL_EPSILON;
			double delta = value1 - value2;
			return (-eps < delta) && (eps > delta);

		}

		public static bool IsEpsilonGreaterThanZero(this double value) {
			return value > 10 * DBL_EPSILON;
		}

		public static bool IsAlmostEqualTo(this Vector3 v1, Vector3 v2) {

			return v1.X.IsAlmostEqualTo(v2.X) && v1.Y.IsAlmostEqualTo(v2.Y) && v1.Z.IsAlmostEqualTo(v2.Z);

		}


	}
}
