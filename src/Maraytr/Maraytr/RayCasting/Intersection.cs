using System;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.Scenes;

namespace Maraytr.RayCasting {
	public class Intersection : IComparable<Intersection> {


		public IIntersectableObject IntersectedObject;

		public Ray Ray;

		/// <summary>
		/// Parameter T of ray (giving position of intersection).
		/// </summary>
		public double RayParameter;

		/// <summary>
		/// Position in world coordinates.
		/// </summary>
		public Vector3 Position;

		/// <summary>
		/// Signed distance (squared) to ray origin (minus means behind ray).
		/// </summary>
		public double RayDistanceSqSigned;

		/// <summary>
		/// True if ray enters the object with this intersection.
		/// False if it leaves.
		/// </summary>
		public bool IsEnter;

		public bool InverseNormal;

		public Vector3 Normal;

		public Vector2 TextureCoord;

		public IMaterial Material;

		public object AdditionalData;
		

		public Intersection(IIntersectableObject intersectedObject, bool isEnter, Ray localRay, double rayParameter,
				object additionalData = null) {
			IntersectedObject = intersectedObject;
			IsEnter = isEnter;
			Ray = localRay;
			RayParameter = rayParameter;
			InverseNormal = false;
			AdditionalData = additionalData;
		}

		public Vector3 LocalIntersectionPt { get { return Ray.GetPointAt(RayParameter); } } 

		public void CompleteIntersection() {
			IntersectedObject.CompleteIntersection(this);
		}
		

		#region IComparable<Intersection> Members

		public int CompareTo(Intersection other) {
			return RayDistanceSqSigned.CompareTo(other.RayDistanceSqSigned);
		}

		#endregion
	}
}
