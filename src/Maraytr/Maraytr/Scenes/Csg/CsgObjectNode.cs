using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes.Csg {
	public class CsgObjectNode : CsgNode, IIntersectableObject {

		public IIntersectableObject IntersectableObject { get; set; }

		public IMaterial Material { get; set; }


		private Matrix4Affine globalToLocalTransformCache;

		private Matrix4Affine localToGlobalTransformCache;


		public CsgObjectNode(IIntersectableObject intObject, IMaterial material, Matrix4Affine transform)
			: base(transform) {

			IntersectableObject = intObject;
			Material = material;
		}

		public override void PrecomputeTransformCaches(Matrix4Affine globalTransform) {			
			localToGlobalTransformCache = globalTransform * LocalTransform;
			globalToLocalTransformCache = localToGlobalTransformCache.Inverse();

			// Not needed, we dont have/dont care about our childs.
			//base.PrecomputeTransformCaches(globalToLocalTransformCache);
		}


		public override int Intersect(Ray globalRay, IList<Intersection> outIntersections) {
			Contract.Assert(localToGlobalTransformCache != null);
			Contract.Assert(globalToLocalTransformCache != null);

			int startIndex = outIntersections.Count;

			Ray localRay = globalRay.Transform(globalToLocalTransformCache);
			int isecCount = IntersectableObject.Intersect(localRay, outIntersections);
			Debug.Assert(outIntersections.Count == startIndex + isecCount);

			for (int i = 0; i < isecCount; ++i) {
				var isec = outIntersections[startIndex + i];
				isec.IntersectedObject = this;
				isec.Position = localToGlobalTransformCache.Transform(localRay.GetPointAt(isec.RayParameter));
				isec.RayDistanceSqSigned = (isec.Position - globalRay.StartPoint).LengthSquared * (isec.RayParameter >= 0.0 ? 1 : -1);
			}

			return isecCount;
		}

		public void CompleteIntersection(Intersection intersection) {
			IntersectableObject.CompleteIntersection(intersection);
			intersection.Normal = localToGlobalTransformCache.TransformNormal(intersection.Normal);
			intersection.Normal.NormalizeThis();
			intersection.Material = Material;
		}

	}
}
