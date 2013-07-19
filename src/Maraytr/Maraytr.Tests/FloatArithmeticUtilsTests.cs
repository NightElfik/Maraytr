using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maraytr.Tests {
	[TestClass]
	public class FloatArithmeticUtilsTests {

		[TestMethod]
		public void IsAlmostZero_Trivial() {
			Assert.IsTrue(FloatArithmeticUtils.IsAlmostZero(0.0));
			Assert.IsTrue(FloatArithmeticUtils.IsAlmostZero(1e-8));
			Assert.IsTrue(FloatArithmeticUtils.IsAlmostZero(-1e-8));

			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(1.0));
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(-1.0));
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(1e-3));
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(-1e-3));
		}

		[TestMethod]
		public void IsAlmostZero_SpecialConstants() {
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(double.NaN));
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(double.PositiveInfinity));
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(double.NegativeInfinity));
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(double.MinValue));
			Assert.IsFalse(FloatArithmeticUtils.IsAlmostZero(double.MaxValue));
		}

		private void testIsAlmostEqualToCommutatively(double left, double right, bool expected) {
			Assert.AreEqual(expected, FloatArithmeticUtils.IsAlmostEqualTo(left, right));
			Assert.AreEqual(expected, FloatArithmeticUtils.IsAlmostEqualTo(right, left));
		}

		[TestMethod]
		public void IsAlmostEqualTo_AroundZero() {
			testIsAlmostEqualToCommutatively(0.0, 0.0, true);
			testIsAlmostEqualToCommutatively(0.0, 1e-8, true);
			testIsAlmostEqualToCommutatively(0.0, -1e-8, true);

			testIsAlmostEqualToCommutatively(0.0, 1e-3, false);
			testIsAlmostEqualToCommutatively(0.0, -1e-3, false);
		}

		[TestMethod]
		public void IsAlmostEqualTo_LargeNumbers() {
			testIsAlmostEqualToCommutatively(1e8, 1e8 + 1, true);
			testIsAlmostEqualToCommutatively(1e8, 1e8 - 1, true);
			testIsAlmostEqualToCommutatively(-1e8, -1e8 + 1, true);
			testIsAlmostEqualToCommutatively(-1e8, -1e8 - 1, true);

			testIsAlmostEqualToCommutatively(1e8, 1e8 + 1e5, false);
			testIsAlmostEqualToCommutatively(1e8, 1e8 - 1e5, false);
			testIsAlmostEqualToCommutatively(-1e8, -1e8 + 1e5, false);
			testIsAlmostEqualToCommutatively(-1e8, -1e8 - 1e5, false);
		}

		[TestMethod]
		public void IsAlmostEqualTo_SpecialConstants() {
			testIsAlmostEqualToCommutatively(double.NaN, 0.0, false);
			testIsAlmostEqualToCommutatively(double.NaN, double.NaN, false);
			testIsAlmostEqualToCommutatively(double.NaN, double.MinValue, false);
			testIsAlmostEqualToCommutatively(double.NaN, double.MaxValue, false);
			testIsAlmostEqualToCommutatively(double.NaN, double.PositiveInfinity, false);
			testIsAlmostEqualToCommutatively(double.NaN, double.NegativeInfinity, false);

			testIsAlmostEqualToCommutatively(double.PositiveInfinity, double.PositiveInfinity, true);
			testIsAlmostEqualToCommutatively(double.PositiveInfinity, 0.0, false);
			testIsAlmostEqualToCommutatively(double.PositiveInfinity, double.MinValue, false);
			testIsAlmostEqualToCommutatively(double.PositiveInfinity, double.MaxValue, false);

			testIsAlmostEqualToCommutatively(double.NegativeInfinity, double.NegativeInfinity, true);
			testIsAlmostEqualToCommutatively(double.NegativeInfinity, 0.0, false);
			testIsAlmostEqualToCommutatively(double.NegativeInfinity, double.MinValue, false);
			testIsAlmostEqualToCommutatively(double.NegativeInfinity, double.MaxValue, false);

			testIsAlmostEqualToCommutatively(double.MinValue, 0.0, false);
			testIsAlmostEqualToCommutatively(double.MinValue, double.MinValue, true);
			testIsAlmostEqualToCommutatively(double.MinValue, double.MaxValue, false);

			testIsAlmostEqualToCommutatively(double.MaxValue, 0.0, false);
			testIsAlmostEqualToCommutatively(double.MaxValue, double.MaxValue, true);

			testIsAlmostEqualToCommutatively(double.Epsilon, 0.0, true);
			testIsAlmostEqualToCommutatively(double.Epsilon, double.Epsilon, true);
		}
	}
}
