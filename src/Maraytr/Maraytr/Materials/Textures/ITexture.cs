using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Materials.Textures {
	public interface ITexture {

		ColorRgbt GetColorAt(Intersection intersection);

	}
}
