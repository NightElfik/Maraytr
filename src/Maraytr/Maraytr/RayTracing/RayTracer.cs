using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;
using Maraytr.RayCasting;
using Maraytr.Scenes;

namespace Maraytr.RayTracing {
	public class RayTracer : RayCaster {

		public int MaxTraceDepth { get; set; }

		public double MinContribution { get; set; }


		public RayTracer(Scene scene) : base(scene) { }

		public override ColorRgbt GetSample(double x, double y, IntegrationState intState) {
			
			Ray ray = scene.Camera.GetRay(x, y);
			if (ray == null) {
				return scene.BgColor;
			}

			return traceRay(ray, intState, 0, 1);

		}
		

		protected ColorRgbt traceRay(Ray ray, IntegrationState intState, int depth, double contribution) {

			var isec = getIntersection(ray);
			if (isec == null) {
				return scene.BgColor;
			}

			ColorRgbt color = evaluateIntersection(isec, intState);

			contribution *= isec.Material.ReflectionFactor;
			if (depth >= MaxTraceDepth || contribution < MinContribution) {
				return color;
			}

			Vector3 reflectedDir = computeReflection(isec.Normal, -ray.Direction);
			reflectedDir.NormalizeThis();

			ColorRgbt reflectedColor = traceRay(new Ray(isec.Position, reflectedDir), intState, depth + 1, contribution);
			return color + reflectedColor * isec.Material.ReflectionFactor;
		}

		protected Vector3 computeReflection(Vector3 normal, Vector3 direction) {
			double k = normal.Dot(direction);
			return (k + k) * normal - direction;
		}



	}
}
