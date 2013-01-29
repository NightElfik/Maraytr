using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;

namespace Maraytr.Materials.Textures {
	public interface ITexture {

		ColorRgbt GetColorAt(Vector2 coords);

	}
}
