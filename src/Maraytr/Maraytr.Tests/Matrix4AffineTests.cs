using Maraytr.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maraytr.Tests {
	[TestClass]
	public class Matrix4AffineTests {
		[TestMethod]
		public void InverseTests() {

			var actual = new Matrix4Affine(new double[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8 }).Inverse();

			var expected = new Matrix4Affine(new double[] { -39, -7, 34, -191, 15, 5, -14, 67, 30, 4, -22, 122 }) * (1.0 / 18.0);

			Assert.IsTrue(expected.IsAlmostEqualTo(actual));
		}

		[TestMethod]
		public void MultiplyTests() {

			var actual = new Matrix4Affine(new double[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8 })
				* (new Matrix4Affine(new double[] { -39, -7, 34, -191, 15, 5, -14, 67, 30, 4, -22, 122 }) * (1.0 / 18.0));

			var expected = Matrix4Affine.CreateIdentity();

			Assert.IsTrue(expected.IsAlmostEqualTo(actual));


			var actual2 = new Matrix4Affine(new double[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8 })
				* new Matrix4Affine(new double[] { 9, 7, 9, 3, 2, 3, 8, 4, 6, 2, 6, 4 });

			var expected2 = new Matrix4Affine(new double[] { 53, 32, 59, 30, 75, 66, 129, 65, 81, 54, 99, 55 });

			Assert.IsTrue(expected2.IsAlmostEqualTo(actual2));

		}
	}
}
