using System.Collections.Generic;
using System.Diagnostics;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public class CsgObjectNode : CsgNode, IIntersectableObject {

		protected Matrix4Affine transformToWorld;

		protected IIntersectableObject intersectableObject;

		public IMaterial material;


		public CsgObjectNode(IIntersectableObject intObject, IMaterial material) {
			intersectableObject = intObject;
			this.material = material;
		}



		public override void PrecomputeWorldTransform(Matrix4Affine worldTransform) {
			transformToWorld = worldTransform;
		}

		public override int Intersect(Ray ray, ICollection<Intersection> outIntersections) {
			var tempIntersections = new List<Intersection>();  // TODO: improve performance
			int isecCount = intersectableObject.Intersect(ray, tempIntersections);
			Debug.Assert(isecCount == tempIntersections.Count);

			for (int i = 0; i < isecCount; ++i) {
				var isec = tempIntersections[i];
				isec.IntersectedObject = this;
				isec.Position = transformToWorld.Transform(ray.GetPointAt(isec.RayParameter));
				isec.RayDistanceSqSigned = (isec.Position - ray.RayWorldCoords.StartPoint).LengthSquared * (isec.RayParameter >= 0.0 ? 1 : -1);
				outIntersections.Add(isec);
			}

			return isecCount;
		}

		public void CompleteIntersection(Intersection intersection) {
			intersectableObject.CompleteIntersection(intersection);
			intersection.Normal = transformToWorld.TransformNormal(intersection.Normal);
			intersection.Material = material;
		}

	}
}
