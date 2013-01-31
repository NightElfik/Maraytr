using System.Collections.Generic;
using Maraytr.Cameras;
using Maraytr.Lights;
using Maraytr.Materials;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Scenes {
	public class Scene {


		public Scene() {
			Lights = new List<ILightSource>();
		}


		public IIntersectable SceneRoot { get; set; }

		public ColorRgbt BgColor { get; set; }

		public ICamera Camera { get; set; }

		public ColorRgbt AmbientLight { get; set; }
		public List<ILightSource> Lights { get; set; }

		public IReflectionModel ReflectionModel { get; set; }

	}
}
