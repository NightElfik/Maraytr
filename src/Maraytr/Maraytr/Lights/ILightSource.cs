using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Lights {
	public interface ILightSource {

		Vector3 GetPosition(IntegrationState intState);

		/// <remarks>
		/// For same integration step should return emmited light from corresponding position returned by GetPosition() method.
		/// </remarks>
		ColorRgbt GetEmittedLightTo(Vector3 point, IntegrationState intState);

	}
}
