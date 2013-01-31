using Maraytr.Materials.Textures;
using Maraytr.Numerics;

namespace Maraytr.Materials {
	public interface IMaterial {

		ColorRgbt BaseColor { get; }

		ITexture Texture { get; }

	}
}
