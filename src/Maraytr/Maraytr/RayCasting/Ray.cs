using System;
using System.Diagnostics.Contracts;
using Maraytr.Numerics;

namespace Maraytr.RayCasting {
	public class Ray {

		public readonly Ray RayWorldCoords;

		public readonly Vector3 StartPoint;

		public readonly Vector3 Direction;


		public Ray(Vector3 startPoint, Vector3 direction) {
			Contract.Requires<ArgumentException>(direction.IsNormalized);

			StartPoint = startPoint;
			Direction = direction;
			RayWorldCoords = this;
		}

		public Ray(Vector3 startPoint, Vector3 direction, Ray rayWorldCoords) {
			Contract.Requires<ArgumentException>(direction.IsNormalized);

			StartPoint = startPoint;
			Direction = direction;
			RayWorldCoords = rayWorldCoords;
		}

		/// <summary>
		/// Transforms this ray by given transformation returning new instance of transformed ray.
		/// </summary>
		public Ray Transform(Matrix4Affine transformation) {
			Vector3 startPoint = transformation.Transform(StartPoint);
			Vector3 direction = transformation.TransformVector(Direction);
			direction.NormalizeThis();
			return new Ray(startPoint, direction, RayWorldCoords);

		}

	}
}
