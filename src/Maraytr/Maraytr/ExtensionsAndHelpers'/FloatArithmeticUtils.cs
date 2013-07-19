using System;
using Maraytr.Numerics;

namespace Maraytr {
	public static class FloatArithmeticUtils {

		public const double DBL_EPSILON = 1E-08;
		public const float SGL_EPSILON = 1E-04f;


		/// <remarks>
		/// Faster equivalent to isAlmostEqualTo(value, 0).
		/// </remarks>
		public static bool IsAlmostZero(this double value) {
			return value < DBL_EPSILON && value > -DBL_EPSILON;
		}

		public static bool IsAlmostEqualTo(this double left, double right) {
			// Check direct equality in case they are infinities where epsilon check does not work.
			if (left == right) {
				return true;
			}
			// This computes (|left-right| / (|left| + |right| + 10.0)) < (DOUBLE_EPSILON / 10.0).
			double eps = (Math.Abs(left) + Math.Abs(right) + 10.0) * (DBL_EPSILON / 10.0);
			double delta = left - right;
			return (-eps < delta) && (eps > delta);

		}

		public static bool IsAlmostLessThan(this double left, double right) {
			// Check direct relation for speed and to correctly handle infinities.
			if (left < right) {
				return true;
			}

			// Return true if numbers are almost equal, otherwise false.
			return IsAlmostEqualTo(left, right);
		}

		public static bool IsAlmostGreaterThan(this double left, double right) {
			// Check direct relation for speed and to correctly handle infinities.
			if (left > right) {
				return true;
			}

			// Return true if numbers are almost equal, otherwise false.
			return IsAlmostEqualTo(left, right);
		}

		public static bool IsAlmostBetween(this double value, double min, double max) {
			return IsAlmostGreaterThan(value, min) && IsAlmostLessThan(value, max);
		}


		public static bool IsAlmostZero(this float value) {
			return value < SGL_EPSILON && value > -SGL_EPSILON;
		}

		public static bool IsAlmostEqualTo(this float value1, float value2) {

			// in case they are Infinities (then epsilon check does not work)
			if (value1 == value2) {
				return true;
			}

			// This computes (|value1-value2| / (|value1| + |value2| + 10.0)) < SGL_EPSILON
			double eps = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * (SGL_EPSILON / 10);
			double delta = value1 - value2;
			return (-eps < delta) && (eps > delta);

		}

		public static bool IsEpsilonGreaterThanZero(this double value) {
			return value > DBL_EPSILON;
		}

		public static bool IsAlmostEqualTo(this Vector3 v1, Vector3 v2) {

			return v1.X.IsAlmostEqualTo(v2.X) && v1.Y.IsAlmostEqualTo(v2.Y) && v1.Z.IsAlmostEqualTo(v2.Z);

		}


	}
}
