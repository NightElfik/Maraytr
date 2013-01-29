using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;

namespace Maraytr.Materials {
	public class PhongReflectionModel : IReflectionModel {

		public ColorRgbt CountReflection(IMaterial material, Vector3 surfaceNormal, Vector3 lightDirection, Vector3 viewDirection) {

			Contract.Requires<ArgumentException>(material is PhongMaterial);

			PhongMaterial mat = material as PhongMaterial;

			float diffIntensity = (float)lightDirection.Dot(surfaceNormal);
			ColorRgbt diffuseColor = diffIntensity * mat.DiffuseReflectionCoef;

			Vector3 halfVector = lightDirection + viewDirection;
			halfVector.NormalizeThis();

			float specularIntensity = (float)Math.Pow(surfaceNormal.Dot(halfVector), mat.ShininessCoef);
			ColorRgbt specularColor = specularIntensity * mat.SpecularReflectionCoef;

			return diffuseColor + specularColor;
		}
	}
}
