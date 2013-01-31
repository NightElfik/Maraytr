using System;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.Scenes;

namespace Maraytr.RayCasting {
	public class Intersection : IComparable<Intersection> {


		public readonly IIntersectableObject IntersectedObject;

		public readonly Ray Ray;

		/// <summary>
		/// Parameter T of ray (giving position of intersection).
		/// </summary>
		public readonly double RayParameter;

		/// <summary>
		/// Position in world coordinates.
		/// </summary>
		public readonly Vector3 Position;

		/// <summary>
		/// Signed distance (squared) to ray origin (minus means behind ray).
		/// </summary>
		public readonly double RayDistanceSqSigned;

		/// <summary>
		/// True if ray enters the object with this intersection.
		/// Flase if leaves.
		/// </summary>
		public bool IsEnter;

		public bool InverseNormal;

		public Vector3 Normal;

		public Vector2 TextureCoord;

		public object AdditionalData;


		public Intersection(IIntersectableObject intersectedObject, bool isEnter, Ray localRay, double rayParameter,
				Vector3 worldsPos, double rayDistSqSgn, object additionalData = null) {
			IntersectedObject = intersectedObject;
			IsEnter = isEnter;
			Ray = localRay;
			RayParameter = rayParameter;
			Position = worldsPos;
			RayDistanceSqSigned = rayDistSqSgn;
			InverseNormal = false;
			AdditionalData = additionalData;
		}

		public Vector3 LocalIntersectionPt { get { return Ray.StartPoint + RayParameter * Ray.Direction; } } 

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
