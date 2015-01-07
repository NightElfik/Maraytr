using System;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Lights {
	public class PointLight : ILightSource {

		protected Vector3 position;
		protected ColorRgbt emittedLight;


		public PointLight(Vector3 pos, ColorRgbt light) {
			position = pos;
			emittedLight = light;
		}


		public Vector3 GetPosition(IntegrationState intState) {
			return position;
		}

		public ColorRgbt GetEmittedLightTo(Vector3 point, IntegrationState intState) {
			return emittedLight;
		}

	}
}
