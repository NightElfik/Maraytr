using System;
using System.Diagnostics.Contracts;

namespace Maraytr.Numerics {
	public struct Vector3 {

		public double X;
		public double Y;
		public double Z;


		public Vector3(double x, double y, double z) {
			X = x;
			Y = y;
			Z = z;
		}

		public static Vector3 operator -(Vector3 v) {
			return new Vector3(-v.X, -v.Y, -v.Z);
		}

		public static Vector3 operator +(Vector3 left, Vector3 right) {
			return new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
		}

		public static Vector3 operator -(Vector3 left, Vector3 right) {
			return new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
		}

		public static Vector3 operator *(Vector3 left, double right) {
			return new Vector3(left.X * right, left.Y * right, left.Z * right);
		}

		public static Vector3 operator *(double left, Vector3 right) {
			return new Vector3(left * right.X, left * right.Y, left * right.Z);
		}

		public static readonly Vector3 Zero = new Vector3(0, 0, 0);
		public static readonly Vector3 XAxis = new Vector3(1, 0, 0);
		public static readonly Vector3 YAxis = new Vector3(0, 1, 0);
		public static readonly Vector3 ZAxis = new Vector3(0, 0, 1);
		public static readonly Vector3 PositiveInfinity =new Vector3(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity);
		public static readonly Vector3 NegativeInfinity = new Vector3(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity);


		public double Length { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
		public double LengthSquared { get { return X * X + Y * Y + Z * Z; } }

		public bool IsNormalized { get { return LengthSquared.IsAlmostEqualTo(1.0); } }

		public bool IsZero { get { return LengthSquared.IsAlmostZero(); } }


		public Vector3 Normalize() {
			Contract.Requires<InvalidOperationException>(!IsZero);

			double length = Math.Sqrt(X * X + Y * Y + Z * Z);
			return new Vector3(X / length, Y / length, Z / length);
		}

		public void NormalizeThis() {
			Contract.Requires<InvalidOperationException>(!IsZero);

			double length = Math.Sqrt(X * X + Y * Y + Z * Z);
			X /= length;
			Y /= length;
			Z /= length;
		}

		public bool HasSameDirectionAs(Vector3 v) {
			if (IsZero || v.IsZero) {
				return false;
			}

			return Normalize().IsAlmostEqualTo(v.Normalize());
		}

		public double Dot(Vector3 v) {
			return X * v.X + Y * v.Y + Z * v.Z;
		}

		public Vector3 Cross(Vector3 v) {
			return new Vector3(Y * v.Z - Z * v.Y, Z * v.X - X * v.Z, X * v.Y - Y * v.X);
		}

	}
}
