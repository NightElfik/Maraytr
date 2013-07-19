using System;
using System.Diagnostics.Contracts;

namespace Maraytr.Numerics {
	/// <summary>
	/// Generates uniformly distributed points on unit sphere.
	/// </summary>
	public class UniformSphericalRandom {

		private Random random = new Random();

		public Vector3 Next() {
			Contract.Ensures(Contract.Result<Vector3>().IsNormalized);

			double u = random.NextDouble() * 2.0 - 1.0;  // [-1, 1) - should be [-1, 1] but thats minor issue
			double phi = random.NextDouble() * 2 * Math.PI;  // [0, 2pi)
			double sqrtU = Math.Sqrt(1.0 - u * u);
			return new Vector3(sqrtU * Math.Cos(phi), sqrtU * Math.Sin(phi), u);
		}

		/// <summary>
		/// Returns random point from positive x hemisphere.
		/// </summary>
		public Vector3 NextFromHemisphere() {
			Contract.Ensures(Contract.Result<Vector3>().IsNormalized);
			Contract.Ensures(Contract.Result<Vector3>().X >= 0);

			double u = random.NextDouble();  // [0, 1) - should be [0, 1] but thats minor issue
			double phi = random.NextDouble() * 2 * Math.PI;  // [0, 2pi)
			double sqrtU = Math.Sqrt(1.0 - u * u);
			return new Vector3(u, sqrtU * Math.Cos(phi), sqrtU * Math.Sin(phi));
		}

	}
}
