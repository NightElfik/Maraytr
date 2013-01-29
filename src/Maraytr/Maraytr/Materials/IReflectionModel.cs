using Maraytr.Numerics;

namespace Maraytr.Materials {
	public interface IReflectionModel {

		/// <param name="material">Material.</param>
		/// <param name="surfaceNormal">Normalized surface normal vector.</param>
		/// <param name="lightDirection">Normalized light direction vector (from serface to light).</param>
		/// <param name="viewDirection">Normalized view direction vector (from surface to viewer).</param>
		ColorRgbt CountReflection(IMaterial material, Vector3 surfaceNormal, Vector3 lightDirection, Vector3 viewDirection);


	}
}
