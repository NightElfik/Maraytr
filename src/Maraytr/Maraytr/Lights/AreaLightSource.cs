using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Lights {
	public class AreaLightSource : ILightSource {

		protected Vector3 position;
		protected ColorRgbt emittedLight;

		protected Vector3 directionX;
		protected Vector3 directionY;



		public AreaLightSource(Vector3 pos, ColorRgbt light, Vector3 dir1, Vector3 dir2) {
			position = pos;
			emittedLight = light;
			directionX = dir1;
			directionY = dir2;
		}


		#region ILightSource Members

		public Vector3 GetPosition(IntegrationState intState) {

			Vector3 dx = directionX * (1.0 / intState.StepsCountSqrt);
			Vector3 dy = directionY * (1.0 / intState.StepsCountSqrt);

			double x = (intState.CurrentStep % intState.StepsCountSqrt) - (intState.StepsCountSqrt / 2);
			double y = (intState.CurrentStep / intState.StepsCountSqrt) - (intState.StepsCountSqrt / 2);

			return position + x * dx + y * dy;

		}

		public ColorRgbt GetEmittedLightTo(Vector3 point, IntegrationState intState) {
			return emittedLight;
		}

		#endregion
	}
}
