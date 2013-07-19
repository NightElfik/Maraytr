using System;

namespace Maraytr.Numerics {
	public class SpiralIndexer {

		/// <remarks>
		/// http://stackoverflow.com/questions/3706219/
		/// </remarks>
		public Point2 GetCoordinates(int i) {
			int j = (int)Math.Round(Math.Sqrt(i));  // Safe to use round because number will never be ?.50.
			int k = Math.Abs(j * j - i) - j;  // k is 0 for main diagonal x = y and abs(k) gives distance to diagonal.
			int l = j * j - i - (j % 2);
			int sign = (int)Math.Pow(-1, j);
			return new Point2((l + k) / 2 * sign, (l - k) / 2 * sign);
		}

	}
}
