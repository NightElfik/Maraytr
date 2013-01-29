using System;
using System.Diagnostics.Contracts;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Cameras {
	public class PerspectiveCamera : ICamera {


		protected Vector3 centerPosition;
		protected Vector3 lookAtPoint;
		protected Vector3 upVector;

		protected double viewWidth;
		protected double viewHeight;
		protected double horizFieldOfView;


		// precomputed values
		Vector3 viewDirection;
		Vector3 xStep;
		Vector3 yStep;


		public PerspectiveCamera(Vector3 position, Vector3 lookAt, Vector3 up, double width, double height, double fovInDeg) {

			Contract.Requires<ArgumentException>(up.LengthSquared > 0.0);

			centerPosition = position;
			lookAtPoint = lookAt;
			upVector = up;

			viewWidth = width;
			viewHeight = height;
			horizFieldOfView = fovInDeg * MathHelper.DegToRad;

			precountValues();
		}

		public Size Size {
			get { return new Size(viewWidth, viewHeight); }
			set {
				viewWidth = value.Width;
				viewHeight = value.Height;
				precountValues();
			}
		}

		public Ray GetRay(double x, double y) {

			if (x < 0 || x >= viewWidth || y < 0 || y >= viewHeight) {
				return null;  // outside of camera view area
			}

			double dx = x - (viewWidth / 2);
			double dy = y - (viewHeight / 2);

			Vector3 rayDirPoint = viewDirection + dx * xStep + dy * yStep;
			Vector3 direction = rayDirPoint + viewDirection;
			direction.NormalizeThis();

			return new Ray(centerPosition, direction);
		}



		private void precountValues() {

			// direction of camera: v
			// up vector: u
			// x axis = v x u
			// y asis = up
			// at left-most edge of view area we want to have direction of ray rotated by fov/2
			// |dx| / |v| = tan(fov / 2)
			// since |v| == 1 we have |dx| = tan(fov / 2)
			// so ray at veri left edge will be computed as: r = d + tan(fov / 2) * xAxis
			// we will use same step for y axis

			viewDirection = lookAtPoint - centerPosition;
			viewDirection.NormalizeThis();
			upVector.NormalizeThis();

			Vector3 xAxis = viewDirection.Cross(upVector);
			xAxis.NormalizeThis();

			Vector3 yAxis = upVector;
			yAxis.NormalizeThis();

			double fovStep = (2 * Math.Tan(horizFieldOfView * 0.5)) / viewWidth;
			xStep = fovStep * xAxis;
			yStep = fovStep * yAxis;

		}

	}
}
