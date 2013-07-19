using System;
using Maraytr.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Maraytr.Tests {
	[TestClass]
	public class SpiralIndexerTests {
		[TestMethod]
		public void GetCoordinates_Basic() {
			var spiralIndexer = new SpiralIndexer();
			Assert.AreEqual(new Point2(0, 0), spiralIndexer.GetCoordinates(0));
			Assert.AreEqual(new Point2(1, 0), spiralIndexer.GetCoordinates(1));
			Assert.AreEqual(new Point2(1, 1), spiralIndexer.GetCoordinates(2));
			Assert.AreEqual(new Point2(0, 1), spiralIndexer.GetCoordinates(3));
			Assert.AreEqual(new Point2(-1, 1), spiralIndexer.GetCoordinates(4));
			Assert.AreEqual(new Point2(-1, 0), spiralIndexer.GetCoordinates(5));
			Assert.AreEqual(new Point2(-1, -1), spiralIndexer.GetCoordinates(6));
			Assert.AreEqual(new Point2(0, -1), spiralIndexer.GetCoordinates(7));
			Assert.AreEqual(new Point2(1, -1), spiralIndexer.GetCoordinates(8));
			Assert.AreEqual(new Point2(2, -1), spiralIndexer.GetCoordinates(9));
			Assert.AreEqual(new Point2(2, 0), spiralIndexer.GetCoordinates(10));
			Assert.AreEqual(new Point2(2, 1), spiralIndexer.GetCoordinates(11));
			Assert.AreEqual(new Point2(2, 2), spiralIndexer.GetCoordinates(12));
			Assert.AreEqual(new Point2(1, 2), spiralIndexer.GetCoordinates(13));
			Assert.AreEqual(new Point2(0, 2), spiralIndexer.GetCoordinates(14));
			Assert.AreEqual(new Point2(-1, 2), spiralIndexer.GetCoordinates(15));
			Assert.AreEqual(new Point2(-2, 2), spiralIndexer.GetCoordinates(16));
			Assert.AreEqual(new Point2(-2, 1), spiralIndexer.GetCoordinates(17));
		}

		[TestMethod]
		public void GetCoordinates_BigIndices() {
			var spiralIndexer = new SpiralIndexer();

			// Each square has 4*2*i elements.
			int sum = 0;
			for (int i = 1; i < 16; ++i) {
				sum += 4 * 2 * i;
				Assert.AreEqual(new Point2(i, -i), spiralIndexer.GetCoordinates(sum));
			}

		}
	}
}
