using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Maraytr.Numerics;

namespace Maraytr {
	public static class GeometryHelper {

		/// <summary>
		/// Finds two vectors which are original to given vector and to each other.
		/// </summary>
		/// <param name="v">Non-zero vector.</param>
		/// <param name="p1">First vector perpendicular to p.</param>
		/// <param name="p2">Second vector perpendicular to p.</param>
		public static void FindPerpendicular(Vector3 v, out Vector3 p1, out Vector3 p2) {

			Contract.Requires<ArgumentException>(!v.IsZero);
			Contract.Ensures(Contract.ValueAtReturn(out p1).Cross(Contract.ValueAtReturn(out p2)).HasSameDirectionAs(v));

			double ax = Math.Abs(v.X);
			double ay = Math.Abs(v.Y);
			double az = Math.Abs(v.Z);

			if (ax >= az && ay >= az) {  // ax, ay are dominant
				p1.X = -v.Y;
				p1.Y = v.X;
				p1.Z = 0.0;
			}
			else if (ax >= ay && az >= ay) {  // ax, az are dominant
				p1.X = -v.Z;
				p1.Y = 0.0;
				p1.Z = v.X;
			}
			else {  // ay, az are dominant
				p1.X = 0.0;
				p1.Y = -v.Z;
				p1.Z = v.Y;
			}

			Debug.Assert(v.Dot(p1).IsAlmostZero());

			p2 = v.Cross(p1);
		}

	}
}
