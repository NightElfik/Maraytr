using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;

namespace Maraytr.Materials {
	public class PhongReflectionModel : IReflectionModel {

		public ColorRgbt CountReflection(IMaterial material, ColorRgbt baseColor, Vector3 surfaceNormal, Vector3 lightDirection, Vector3 viewDirection) {

			Contract.Requires<ArgumentException>(material is PhongMaterial);

			PhongMaterial mat = material as PhongMaterial;


			ColorRgbt resultColor = ColorRgbt.Black;

			float diffFactor = (float)lightDirection.Dot(surfaceNormal);
			if (diffFactor <= 0) {
				return resultColor;
			}

			resultColor += baseColor * diffFactor * mat.DiffuseReflectionCoef;

			Vector3 halfVector = lightDirection + viewDirection;
			halfVector.NormalizeThis();

			float specularFactor = (float)Math.Pow(surfaceNormal.Dot(halfVector), mat.ShininessCoef);
			resultColor += specularFactor * mat.SpecularReflectionCoef;

			return resultColor;
		}
	}
}
