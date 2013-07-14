using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Materials.Textures;
using Maraytr.Numerics;

namespace Maraytr.Materials {
	public class PhongMaterial : IMaterial {

		public ColorRgbt BaseColor { get; set; }

		public ITexture Texture { get; set; }

		public float ReflectionFactor { get; set; }

		public ColorRgbt SpecularReflectionCoef;

		public ColorRgbt DiffuseReflectionCoef;

		public float ShininessCoef;

		public float Transparency;

		public float RefractionIndex;

	}
}
