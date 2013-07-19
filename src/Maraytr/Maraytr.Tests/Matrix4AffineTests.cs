using System;
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

		[TestMethod]
		public void CreateRotationAngleAxis_XAxis() {

			for (double angle = 0; angle < 2 * Math.PI; angle += 1) {
				var actual = Matrix4Affine.CreateRotationAngleAxis(angle, Vector3.XAxis);
				var expected = Matrix4Affine.CreateRotationX(angle);
				Assert.IsTrue(expected.IsAlmostEqualTo(actual));
			}

		}

		[TestMethod]
		public void CreateRotationVectorToVector_Axes() {

			var initial = Vector3.YAxis;
			var expected = Vector3.XAxis;
			var actual = Matrix4Affine.CreateRotationVectorToVector(initial, expected).Transform(initial);
			Assert.IsTrue(expected.IsAlmostEqualTo(actual));

			initial = Vector3.ZAxis;
			expected = Vector3.YAxis;
			actual = Matrix4Affine.CreateRotationVectorToVector(initial, expected).Transform(initial);
			Assert.IsTrue(expected.IsAlmostEqualTo(actual));

			initial = Vector3.YAxis;
			expected = Vector3.ZAxis;
			actual = Matrix4Affine.CreateRotationVectorToVector(initial, expected).Transform(initial);
			Assert.IsTrue(expected.IsAlmostEqualTo(actual));

		}

		[TestMethod]
		public void CreateRotationVectorToVector_Basic() {

			// Those vectors have the same lengths so they will be equal after rotation.
			var initial = new Vector3(-3, 2, -1);
			var expected = new Vector3(1, 2, 3);
			var actual = Matrix4Affine.CreateRotationVectorToVector(initial, expected).Transform(initial);
			Assert.IsTrue(expected.IsAlmostEqualTo(actual));

			initial = new Vector3(-5.4, 4.8, 0.5);
			expected = new Vector3(0.005, -2.2, -4.1).Normalize() * initial.Length;
			actual = Matrix4Affine.CreateRotationVectorToVector(initial, expected).Transform(initial);
			Assert.IsTrue(expected.IsAlmostEqualTo(actual));

		}

		[TestMethod]
		public void CreateRotationVectorToVector_Colinear() {

			var initial = Vector3.XAxis;
			var expected = initial;
			var actual = Matrix4Affine.CreateRotationVectorToVector(initial, expected).Transform(initial);
			Assert.IsTrue(expected.IsAlmostEqualTo(actual));

			initial = new Vector3(1, 2, -3);
			expected = -initial;
			actual = Matrix4Affine.CreateRotationVectorToVector(initial, expected).Transform(initial);
			Assert.IsTrue(expected.IsAlmostEqualTo(actual));

		}
	}
}
