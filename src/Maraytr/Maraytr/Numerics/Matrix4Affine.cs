using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Maraytr.Numerics {
	public class Matrix4Affine /*: IEquatable<Matrix4Affine>*/ {

		public double M11;
		public double M12;
		public double M13;
		public double M14;

		public double M21;
		public double M22;
		public double M23;
		public double M24;

		public double M31;
		public double M32;
		public double M33;
		public double M34;

		public const double M41 = 0.0;
		public const double M42 = 0.0;
		public const double M43 = 0.0;
		public const double M44 = 1.0;


		public static Matrix4Affine operator *(Matrix4Affine left, double right) {
			var m = new Matrix4Affine();
			m.M11 = left.M11 * right;
			m.M12 = left.M12 * right;
			m.M13 = left.M13 * right;
			m.M14 = left.M14 * right;
			m.M21 = left.M21 * right;
			m.M22 = left.M22 * right;
			m.M23 = left.M23 * right;
			m.M24 = left.M24 * right;
			m.M31 = left.M31 * right;
			m.M32 = left.M32 * right;
			m.M33 = left.M33 * right;
			m.M34 = left.M34 * right;
			return m;
		}

		public static Matrix4Affine operator *(double left, Matrix4Affine right) {
			var m = new Matrix4Affine();
			m.M11 = right.M11 * left;
			m.M12 = right.M12 * left;
			m.M13 = right.M13 * left;
			m.M14 = right.M14 * left;
			m.M21 = right.M21 * left;
			m.M22 = right.M22 * left;
			m.M23 = right.M23 * left;
			m.M24 = right.M24 * left;
			m.M31 = right.M31 * left;
			m.M32 = right.M32 * left;
			m.M33 = right.M33 * left;
			m.M34 = right.M34 * left;
			return m;
		}

		public static Matrix4Affine operator *(Matrix4Affine left, Matrix4Affine right) {
			var m = new Matrix4Affine();
			m.M11 = left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31;
			m.M12 = left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32;
			m.M13 = left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33;
			m.M14 = left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14;
			m.M21 = left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31;
			m.M22 = left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32;
			m.M23 = left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33;
			m.M24 = left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24;
			m.M31 = left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31;
			m.M32 = left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32;
			m.M33 = left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33;
			m.M34 = left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34;
			//m.M11 = left.M11 * right.M11 + left.M12 * right.M21 + left.M13 * right.M31 + left.M14 * right.M41;
			//m.M12 = left.M11 * right.M12 + left.M12 * right.M22 + left.M13 * right.M32 + left.M14 * right.M42;
			//m.M13 = left.M11 * right.M13 + left.M12 * right.M23 + left.M13 * right.M33 + left.M14 * right.M43;
			//m.M14 = left.M11 * right.M14 + left.M12 * right.M24 + left.M13 * right.M34 + left.M14 * right.M44;
			//m.M21 = left.M21 * right.M11 + left.M22 * right.M21 + left.M23 * right.M31 + left.M24 * right.M41;
			//m.M22 = left.M21 * right.M12 + left.M22 * right.M22 + left.M23 * right.M32 + left.M24 * right.M42;
			//m.M23 = left.M21 * right.M13 + left.M22 * right.M23 + left.M23 * right.M33 + left.M24 * right.M43;
			//m.M24 = left.M21 * right.M14 + left.M22 * right.M24 + left.M23 * right.M34 + left.M24 * right.M44;
			//m.M31 = left.M31 * right.M11 + left.M32 * right.M21 + left.M33 * right.M31 + left.M34 * right.M41;
			//m.M32 = left.M31 * right.M12 + left.M32 * right.M22 + left.M33 * right.M32 + left.M34 * right.M42;
			//m.M33 = left.M31 * right.M13 + left.M32 * right.M23 + left.M33 * right.M33 + left.M34 * right.M43;
			//m.M34 = left.M31 * right.M14 + left.M32 * right.M24 + left.M33 * right.M34 + left.M34 * right.M44;
			//m.M41 = left.M41 * right.M11 + left.M42 * right.M21 + left.M43 * right.M31 + left.M44 * right.M41;
			//m.M42 = left.M41 * right.M12 + left.M42 * right.M22 + left.M43 * right.M32 + left.M44 * right.M42;
			//m.M43 = left.M41 * right.M13 + left.M42 * right.M23 + left.M43 * right.M33 + left.M44 * right.M43;
			//m.M44 = left.M41 * right.M14 + left.M42 * right.M24 + left.M43 * right.M34 + left.M44 * right.M44;
			return m;
		}


		#region Static creation method

		public static Matrix4Affine CreateIdentity() {
			var m = new Matrix4Affine();
			m.M11 = 1.0;
			m.M22 = 1.0;
			m.M33 = 1.0;
			return m;
		}

		public static Matrix4Affine CreateTranslation(Vector3 translation) {
			var m = new Matrix4Affine();
			m.M11 = 1.0;
			m.M22 = 1.0;
			m.M33 = 1.0;

			m.M14 = translation.X;
			m.M24 = translation.Y;
			m.M34 = translation.Z;
			return m;
		}

		public static Matrix4Affine CreateScale(double scale) {
			var m = new Matrix4Affine();
			m.M11 = scale;
			m.M22 = scale;
			m.M33 = scale;
			return m;
		}

		public static Matrix4Affine CreateScale(Vector3 scale) {
			var m = new Matrix4Affine();
			m.M11 = scale.X;
			m.M22 = scale.Y;
			m.M33 = scale.Z;
			return m;
		}

		public static Matrix4Affine CreateRotationX(double angleRad) {
			double sin = Math.Sin(angleRad);
			double cos = Math.Cos(angleRad);
			var m = new Matrix4Affine();
			m.M11 = 1;
			m.M22 = cos;
			m.M23 = sin;
			m.M32 = -sin;
			m.M33 = cos;
			return m;
		}

		public static Matrix4Affine CreateRotationY(double angleRad) {
			double sin = Math.Sin(angleRad);
			double cos = Math.Cos(angleRad);
			var m = new Matrix4Affine();
			m.M11 = cos;
			m.M13 = -sin;
			m.M22 = 1;
			m.M31 = sin;
			m.M33 = cos;
			return m;
		}

		public static Matrix4Affine CreateRotationAngleAxis(double angleRad, Vector3 axis) {
			Contract.Requires(axis.IsNormalized);

			double cos = Math.Cos(angleRad);
			double iCos = 1 - cos;
			double sin = Math.Sin(angleRad);
			var m = new Matrix4Affine();
			
			m.M11 = axis.X * axis.X * iCos + cos;
			m.M12 = axis.X * axis.Y * iCos + axis.Z * sin;
			m.M13 = axis.X * axis.Z * iCos - axis.Y * sin;

			m.M21 = axis.X * axis.Y * iCos - axis.Z * sin;
			m.M22 = axis.Y * axis.Y * iCos + cos;
			m.M23 = axis.Y * axis.Z * iCos + axis.X * sin;

			m.M31 = axis.X * axis.Z * iCos + axis.Y * sin;
			m.M32 = axis.Y * axis.Z * iCos - axis.X * sin;
			m.M33 = axis.Z * axis.Z * iCos + cos;

			return m;
		}

		public static Matrix4Affine CreateRotationVectorToVector(Vector3 originalDirection, Vector3 desiredDirection) {

			Vector3 axis = desiredDirection.Cross(originalDirection);
			if (axis.IsAlmostZero) {
				if (originalDirection.Dot(desiredDirection) > 0) {
					return CreateIdentity();
				}
				else {
					return CreateScale(-1);
				}
			}

			double angle = originalDirection.AngleTo(desiredDirection);
			axis.NormalizeThis();
			return CreateRotationAngleAxis(angle, axis);
		}

		#endregion


		public Matrix4Affine() { }

		public Matrix4Affine(IEnumerable<double> values) {
			int n = 0;
			foreach (double number in values) {
				this[n++] = number;
			}
		}

		public double this[int n] {
			get {
				Contract.Requires<ArgumentException>(n >= 0 && n < 16);

				switch (n) {
					case 0: return M11;
					case 1: return M12;
					case 2: return M13;
					case 3: return M14;
					case 4: return M21;
					case 5: return M22;
					case 6: return M23;
					case 7: return M24;
					case 8: return M31;
					case 9: return M32;
					case 10: return M33;
					case 11: return M34;
					case 12: return M41;
					case 13: return M42;
					case 14: return M43;
					case 16: return M44;
					default: throw new InvalidOperationException();
				}
			}
			set {
				Contract.Requires<ArgumentException>(n >= 0 && n < 12);

				switch (n) {
					case 0: M11 = value; return;
					case 1: M12 = value; return;
					case 2: M13 = value; return;
					case 3: M14 = value; return;
					case 4: M21 = value; return;
					case 5: M22 = value; return;
					case 6: M23 = value; return;
					case 7: M24 = value; return;
					case 8: M31 = value; return;
					case 9: M32 = value; return;
					case 10: M33 = value; return;
					case 11: M34 = value; return;
					default: throw new InvalidOperationException();
				}
			}
		}

		public double this[int x, int y] {
			get {
				Contract.Requires<ArgumentException>(x >= 0 && x < 4);
				Contract.Requires<ArgumentException>(y >= 0 && y < 3);

				switch (x + y * 4) {
					case 0: return M11;
					case 1: return M12;
					case 2: return M13;
					case 3: return M14;
					case 4: return M21;
					case 5: return M22;
					case 6: return M23;
					case 7: return M24;
					case 8: return M31;
					case 9: return M32;
					case 10: return M33;
					case 11: return M34;
					case 12: return M41;
					case 13: return M42;
					case 14: return M43;
					case 16: return M44;
					default: throw new InvalidOperationException();
				}
			}
			set {
				Contract.Requires<ArgumentException>(x >= 0 && x < 4);
				Contract.Requires<ArgumentException>(y >= 0 && y < 3);

				switch (x + y * 4) {
					case 0: M11 = value; return;
					case 1: M12 = value; return;
					case 2: M13 = value; return;
					case 3: M14 = value; return;
					case 4: M21 = value; return;
					case 5: M22 = value; return;
					case 6: M23 = value; return;
					case 7: M24 = value; return;
					case 8: M31 = value; return;
					case 9: M32 = value; return;
					case 10: M33 = value; return;
					case 11: M34 = value; return;
					default: throw new InvalidOperationException();
				}
			}
		}


		public double Determinant() {

			// expanding by minor M44 == 1.0 -> det A
			// A = [ M   b  ]
			//     [ 0   1  ]
			// det(A) = det(M);

			return M11 * (M22 * M33 - M32 * M23)
				- M12 * (M21 * M33 - M23 * M31)
				+ M13 * (M21 * M32 - M22 * M31);

			//double s0 = M11 * M22 - M21 * M12;
			//double s1 = M11 * M23 - M21 * M13;
			//double s2 = M11 * M24 - M21 * M14;
			//double s3 = M12 * M23 - M22 * M13;
			//double s4 = M12 * M24 - M22 * M14;
			//double s5 = M13 * M24 - M23 * M14;

			//double c5 = M33 * M44 - M43 * M34;
			//double c4 = M32 * M44 - M42 * M34;
			//double c3 = M32 * M43 - M42 * M33;
			//double c2 = M31 * M44 - M41 * M34;
			//double c1 = M31 * M43 - M41 * M33;
			//double c0 = M31 * M42 - M41 * M32;

			//return s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0;
		}

		public Matrix4Affine Inverse() {

			// A = [ M   b  ]
			//     [ 0   1  ]
			// inv(A) = [ inv(M)   -inv(M) * b ]
			//          [   0            1     ]

			double det = M11 * (M22 * M33 - M32 * M23)
				- M12 * (M21 * M33 - M23 * M31)
				+ M13 * (M21 * M32 - M22 * M31);

			Debug.Assert(!det.IsAlmostZero());

			double invDet = 1.0 / det;

			var inv = new Matrix4Affine();

			inv.M11 = (M22 * M33 - M32 * M23) * invDet;
			inv.M12 = (M13 * M32 - M33 * M12) * invDet;
			inv.M13 = (M12 * M23 - M22 * M13) * invDet;

			inv.M21 = (M23 * M31 - M33 * M21) * invDet;
			inv.M22 = (M11 * M33 - M31 * M13) * invDet;
			inv.M23 = (M13 * M21 - M23 * M11) * invDet;

			inv.M31 = (M21 * M32 - M31 * M22) * invDet;
			inv.M32 = (M12 * M31 - M32 * M11) * invDet;
			inv.M33 = (M11 * M22 - M21 * M12) * invDet;

			inv.M14 = -(inv.M11 * M14 + inv.M12 * M24 + inv.M13 * M34);
			inv.M24 = -(inv.M21 * M14 + inv.M22 * M24 + inv.M23 * M34);
			inv.M34 = -(inv.M31 * M14 + inv.M32 * M24 + inv.M33 * M34);

			return inv;

			//double s0 = M11 * M22 - M21 * M12;
			//double s1 = M11 * M23 - M21 * M13;
			//double s2 = M11 * M24 - M21 * M14;
			//double s3 = M12 * M23 - M22 * M13;
			//double s4 = M12 * M24 - M22 * M14;
			//double s5 = M13 * M24 - M23 * M14;

			//double c5 = M33 * M44 - M43 * M34;
			//double c4 = M32 * M44 - M42 * M34;
			//double c3 = M32 * M43 - M42 * M33;
			//double c2 = M31 * M44 - M41 * M34;
			//double c1 = M31 * M43 - M41 * M33;
			//double c0 = M31 * M42 - M41 * M32;

			//double det = s0 * c5 - s1 * c4 + s2 * c3 + s3 * c2 - s4 * c1 + s5 * c0;
			//Debug.Assert(!det.IsAlmostZero());

			//double invDet = 1.0 / det;

			//var inv = new Matrix4();

			//inv.M11 = (M22 * c5 - M23 * c4 + M24 * c3) * invDet;
			//inv.M12 = (-M12 * c5 + M13 * c4 - M14 * c3) * invDet;
			//inv.M13 = (M42 * s5 - M43 * s4 + M44 * s3) * invDet;
			//inv.M14 = (-M32 * s5 + M33 * s4 - M34 * s3) * invDet;

			//inv.M21 = (-M21 * c5 + M23 * c2 - M24 * c1) * invDet;
			//inv.M22 = (M11 * c5 - M13 * c2 + M14 * c1) * invDet;
			//inv.M23 = (-M41 * s5 + M43 * s2 - M44 * s1) * invDet;
			//inv.M24 = (M31 * s5 - M33 * s2 + M34 * s1) * invDet;

			//inv.M31 = (M21 * c4 - M22 * c2 + M24 * c0) * invDet;
			//inv.M32 = (-M11 * c4 + M12 * c2 - M14 * c0) * invDet;
			//inv.M33 = (M41 * s4 - M42 * s2 + M44 * s0) * invDet;
			//inv.M34 = (-M31 * s4 + M32 * s2 - M34 * s0) * invDet;

			//inv.M41 = (-M21 * c3 + M22 * c1 - M23 * c0) * invDet;
			//inv.M42 = (M11 * c3 - M12 * c1 + M13 * c0) * invDet;
			//inv.M43 = (-M41 * s3 + M42 * s1 - M43 * s0) * invDet;
			//inv.M44 = (M31 * s3 - M32 * s1 + M33 * s0) * invDet;

			//return inv;
		}


		public Vector3 Transform(Vector3 v) {
			return new Vector3(
				M11 * v.X + M12 * v.Y + M13 * v.Z + M14,
				M21 * v.X + M22 * v.Y + M23 * v.Z + M24,
				M31 * v.X + M32 * v.Y + M33 * v.Z + M34);
		}

		public Vector3 TransformVector(Vector3 v) {
			return new Vector3(
				M11 * v.X + M12 * v.Y + M13 * v.Z,
				M21 * v.X + M22 * v.Y + M23 * v.Z,
				M31 * v.X + M32 * v.Y + M33 * v.Z);
		}

		/// <summary>
		/// Transforms normal vector by given transformation. Result normal is not normalized.
		/// </summary>
		/// <remarks>
		/// Transformation can not be applied directly to normal because scale transformation do not preserve normals.
		/// But we can apply transformation to tangent vectors.
		/// </remarks>
		public Vector3 TransformNormal(Vector3 normal) {
			Contract.Requires<ArgumentException>(!normal.IsAlmostZero);
			Contract.Ensures(!Contract.Result<Vector3>().IsAlmostZero);

			Vector3 v1, v2;
			GeometryHelper.FindPerpendicular(normal, out v1, out v2);
			v1 = TransformVector(v1);
			v2 = TransformVector(v2);
			return v1.Cross(v2);

		}

		public bool IsAlmostEqualTo(Matrix4Affine other) {
			return M11.IsAlmostEqualTo(other.M11)
				&& M12.IsAlmostEqualTo(other.M12)
				&& M13.IsAlmostEqualTo(other.M13)
				&& M14.IsAlmostEqualTo(other.M14)
				&& M21.IsAlmostEqualTo(other.M21)
				&& M22.IsAlmostEqualTo(other.M22)
				&& M23.IsAlmostEqualTo(other.M23)
				&& M24.IsAlmostEqualTo(other.M24)
				&& M31.IsAlmostEqualTo(other.M31)
				&& M32.IsAlmostEqualTo(other.M32)
				&& M33.IsAlmostEqualTo(other.M33)
				&& M34.IsAlmostEqualTo(other.M34);
		}

		//public override bool Equals(object obj) {
		//	if (obj is Matrix4Affine) {
		//		return this == obj as Matrix4Affine;
		//	}
		//	else {
		//		return false;
		//	}
		//}

		//public bool Equals(Matrix4Affine other) {
		//	return this == other;
		//}
	}
}
