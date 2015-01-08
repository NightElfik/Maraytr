using Maraytr.Cameras;
using Maraytr.Lights;
using Maraytr.Materials;
using Maraytr.Materials.Textures;
using Maraytr.Numerics;
using Maraytr.Primitives;
using Maraytr.Scenes;
using Maraytr.Scenes.Csg;

namespace Maraytr {
	public class DemoScenesFactory {

		private Cube unitCube = new Cube();
		private Sphere unitSphere = new Sphere();
		private HalfSpace thinPlane = new HalfSpace() {
			Width = 0.1
		};

		public ColorRgbt BgColor { get; set; }

		public Scene CreateSimpleBwScene() {
			var scene = createEmptyScene();

			scene.Camera = new PerspectiveCamera(new Vector3(0, 1, 6), new Vector3(0, 0, 0), new Vector3(0, 6, -1), 640, 480, 90);
			scene.AmbientLight = new ColorRgbt(1, 1, 1);

			var cube = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(0, 0, 0),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
					ShininessCoef = 0
				},
				Matrix4Affine.CreateTranslation(new Vector3(-1.6, 0, 0.2)) * Matrix4Affine.CreateScale(0.8)
			);

			var sphere = new CsgObjectNode(unitSphere,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(0, 0, 0),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
					ShininessCoef = 0
				},
				Matrix4Affine.CreateTranslation(new Vector3(1, 0, 1)) * Matrix4Affine.CreateScale(0.6)
			);

			var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Union);
			sceneRoot.AddChild(cube, sphere);

			sceneRoot.PrecomputeTransformCaches(Matrix4Affine.CreateIdentity());
			scene.SceneRoot = sceneRoot;

			return scene;
		}


		public Scene CreateSimpleColoredScene() {
			var scene = createEmptyScene();

			scene.Camera = new PerspectiveCamera(new Vector3(0, 1, 6), new Vector3(0, 0, 0), new Vector3(0, 6, -1), 640, 480, 90);
			scene.AmbientLight = new ColorRgbt(1, 1, 1);

			var cube1 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(1, 0, 0),
				},
				Matrix4Affine.CreateTranslation(new Vector3(-1.6, 0, 0.2)) * Matrix4Affine.CreateScale(0.8)
			);

			var sphere = new CsgObjectNode(unitSphere,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(0, 0, 1),
				},
				Matrix4Affine.CreateTranslation(new Vector3(1.5, 0, 0))
			);

			var plane = new CsgObjectNode(thinPlane,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(1, 1, 1),
				},
				Matrix4Affine.CreateIdentity()
			);

			var cube2 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(1, 1, 0),
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, 0, 1)) * Matrix4Affine.CreateScale(0.6)
			);

			var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Union);
			sceneRoot.AddChild(cube1, sphere, plane, cube2);

			sceneRoot.PrecomputeTransformCaches(Matrix4Affine.CreateIdentity());
			scene.SceneRoot = sceneRoot;

			return scene;
		}

		public Scene CreateSimpleTexturesScene() {
			var scene = createEmptyScene();

			scene.Camera = new PerspectiveCamera(new Vector3(1, 4.1, 4.9), new Vector3(0, 0, 0), new Vector3(0, 6, -1), 640, 480, 90);
			//scene.AmbientLight = new ColorRgbt(1, 1, 1);

			//scene.AmbientLight = new ColorRgbt(0.1f, 0.1f, 0.1f);
			//scene.Lights.Add(new PointLight(new Vector3(0.6, 5, 0), new ColorRgbt(1, 1, 1)));

			scene.AmbientLight = new ColorRgbt(0.1f, 0.1f, 0.1f);
			scene.Lights.Add(new AreaLightSource(new Vector3(0.6, 8, 0), new ColorRgbt(1, 1, 1), Vector3.ZAxis, Vector3.XAxis));

			var cube1 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					Texture = new StripsTexture2D() { Frequency = 11, Orientation = false },
					BaseColor = new ColorRgbt(1, 0, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
				},
				Matrix4Affine.CreateTranslation(new Vector3(-1.6, 0, 0.2)) * Matrix4Affine.CreateScale(0.8)
			);

			var sphere = new CsgObjectNode(unitSphere,
				new PhongMaterial() {
					Texture = new StripsTexture2D() { Frequency = 40, Orientation = true },
					BaseColor = new ColorRgbt(0, 0, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
				},
				Matrix4Affine.CreateTranslation(new Vector3(1.5, 0, 0))
			);

			var plane = new CsgObjectNode(thinPlane,
				new PhongMaterial() {
					Texture = new CheckerTexture2D(),
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(0.7f, 0.7f, 0.7f),
				},
				Matrix4Affine.CreateIdentity()
			);

			var cube2 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					Texture = new CheckerTexture2D() { UFrequency = 5, VFrequency = 5 },
					BaseColor = new ColorRgbt(1, 1, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, 0, 1)) * Matrix4Affine.CreateScale(0.6)
			);

			var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Union);
			sceneRoot.AddChild(cube1, sphere, plane, cube2);

			sceneRoot.PrecomputeTransformCaches(Matrix4Affine.CreateIdentity());
			scene.SceneRoot = sceneRoot;
			return scene;
		}

		public Scene CreateSimpleReflectScene() {
			var scene = createEmptyScene();

			scene.Camera = new PerspectiveCamera(new Vector3(1, 4.1, 4.9), new Vector3(0, 0, 0), new Vector3(0, 6, -1), 640, 480, 90);

			scene.AmbientLight = new ColorRgbt(0.1f, 0.1f, 0.1f);
			scene.Lights.Add(new AreaLightSource(new Vector3(0.6, 6, 2), new ColorRgbt(1, 1, 1), Vector3.ZAxis, Vector3.XAxis));

			var cube1 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					Texture = new StripsTexture2D() { Frequency = 11, Orientation = false },
					BaseColor = new ColorRgbt(1, 0, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					ReflectionFactor = 0.3f,
				},
				Matrix4Affine.CreateTranslation(new Vector3(-1.6, 0, 0.2)) * Matrix4Affine.CreateRotationY(33 * MathHelper.DegToRad)
					* Matrix4Affine.CreateScale(0.8)
			);

			var sphere = new CsgObjectNode(unitSphere,
				new PhongMaterial() {
					Texture = new StripsTexture2D() { Frequency = 40, Orientation = true },
					BaseColor = new ColorRgbt(0, 0, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					ReflectionFactor = 0.3f,
				},
				Matrix4Affine.CreateTranslation(new Vector3(1.5, 0, 0))
			);

			var plane = new CsgObjectNode(thinPlane,
				new PhongMaterial() {
					Texture = new CheckerTexture2D(),
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(0.7f, 0.7f, 0.7f),
					ReflectionFactor = 0.5f,
				},
				Matrix4Affine.CreateIdentity()
			);

			var cube2 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					Texture = new CheckerTexture2D() { UFrequency = 5, VFrequency = 5 },
					BaseColor = new ColorRgbt(1, 1, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					ReflectionFactor = 0.3f,
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, 0, 1)) * Matrix4Affine.CreateScale(0.6)
			);

			var mirrorPlane = new CsgObjectNode(thinPlane,
				new PhongMaterial() {
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					ReflectionFactor = 0.95f,
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, 0, -1.2)) * Matrix4Affine.CreateRotationY(-10 * MathHelper.DegToRad)
					* Matrix4Affine.CreateRotationX(80 * MathHelper.DegToRad)
			);

			var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Union);
			sceneRoot.AddChild(cube1, sphere, plane, cube2, mirrorPlane);

			sceneRoot.PrecomputeTransformCaches(Matrix4Affine.CreateIdentity());
			scene.SceneRoot = sceneRoot;
			return scene;
		}


		public Scene CreateIntroScene() {
			var scene = createEmptyScene();

			scene.Camera = new PerspectiveCamera(new Vector3(0, 1, 6), new Vector3(0, 0, 0), new Vector3(0, 6, -1), 640, 480, 90);
			scene.AmbientLight = new ColorRgbt(0.1f, 0.1f, 0.1f);
			scene.Lights.Add(new AreaLightSource(new Vector3(2, 7, 10), new ColorRgbt(1, 1, 1), Vector3.ZAxis, Vector3.XAxis));

			var cube1 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(1, 0, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ShininessCoef = 128
				},
				Matrix4Affine.CreateTranslation(new Vector3(-1.6, 0, 0.2)) * Matrix4Affine.CreateScale(0.6)
			);

			var sphere3 = new CsgObjectNode(unitSphere,
				new PhongMaterial() {
					Texture = new StripsTexture2D() { Frequency = 40, Orientation = true },
					BaseColor = new ColorRgbt(0, 0, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ReflectionFactor = 0.5f,
					ShininessCoef = 128
				},
				Matrix4Affine.CreateTranslation(new Vector3(1.5, 0, 0))
			);

			var plane1 = new CsgObjectNode(thinPlane,
				new PhongMaterial() {
					Texture = new CheckerTexture2D(),
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
				},
				Matrix4Affine.CreateIdentity()
			);

			var plane2 = new CsgObjectNode(thinPlane,
				new PhongMaterial() {
					Texture = new CheckerTexture2D(),
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, -0.35, 0))
			);

			var sphere2 = new CsgObjectNode(unitSphere,
				new PhongMaterial() {
					Texture = new CheckerTexture2D() { UFrequency = 30, VFrequency = 30 },
					BaseColor = new ColorRgbt(0, 1, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ShininessCoef = 128
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, 0, 1)) * Matrix4Affine.CreateScale(1.4)
			);

			var sphereIntersec1 = new CsgObjectNode(unitSphere,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(0, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0.5f, 0.5f, 0.5f),
					ShininessCoef = 256
				},
				Matrix4Affine.CreateTranslation(new Vector3(0.4, 0, 0))
			);

			var cubeIntersec2 = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					Texture = new CheckerTexture2D() { UFrequency = 5, VFrequency = 5 },
					BaseColor = new ColorRgbt(1, 1, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ShininessCoef = 128
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, -0.1, -0.1))
			);


			var mirrorPlane = new CsgObjectNode(thinPlane,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(0, 0, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
					ReflectionFactor = 1f,
					ShininessCoef = 1
				},
				Matrix4Affine.CreateRotationX(90 * MathHelper.DegToRad)
			);

			var mirrorCut = new CsgObjectNode(unitCube,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(0.5f, 0.5f, 0.5f),
					DiffuseReflectionCoef = new ColorRgbt(0.5f, 0.5f, 0.5f),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0)
				},
				Matrix4Affine.CreateTranslation(new Vector3(0, 0.5, -0.5)) * Matrix4Affine.CreateScale(new Vector3(4, 1.8, 1))
			);


			var union = new CsgBoolOperationNode(CsgBoolOperation.Union);
			union.AddChild(cube1, sphere3, plane1, plane2);

			var diff = new CsgBoolOperationNode(CsgBoolOperation.Difference);
			diff.AddChild(union, sphere2);

			var intersec = new CsgBoolOperationNode(CsgBoolOperation.Intersection,
				Matrix4Affine.CreateTranslation(new Vector3(0.8, 0, 3)) * Matrix4Affine.CreateScale(0.5));
			intersec.AddChild(sphereIntersec1, cubeIntersec2);

			var mirror = new CsgBoolOperationNode(CsgBoolOperation.Intersection,
				Matrix4Affine.CreateTranslation(new Vector3(-2, 0, -2)) * Matrix4Affine.CreateRotationX(-15 * MathHelper.DegToRad));
			mirror.AddChild(mirrorPlane, mirrorCut);

			var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Union);
			sceneRoot.AddChild(diff, intersec, mirror);

			sceneRoot.PrecomputeTransformCaches(Matrix4Affine.CreateIdentity());
			scene.SceneRoot = sceneRoot;

			return scene;
		}


		//public Scene CreateMengerSpongeScene() {
		//	var scene = createEmptyScene();
		//	scene.BgColor = new ColorRgbt(1, 1, 1);
		//	scene.Camera = new PerspectiveCamera(new Vector3(1.8, 2.1, 8), new Vector3(1.4, 1.3, 1.5), new Vector3(0, 1, 0), 640, 480, 90);

		//	scene.Lights.Add(new PointLight(new Vector3(2, 7, 10), new ColorRgbt(1, 1, 1)));

		//	var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Union);
		//	var mengerSponge = new CsgObjectNode(
		//		new MengerSponge() {
		//			IterationsCount = 2
		//		},
		//		new PhongMaterial() {
		//			BaseColor = new ColorRgbt(1, 1, 1),
		//			DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
		//			SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
		//			ReflectionFactor = 0.5f,
		//		}
		//	);
		//	sceneRoot.AddChildNode(mengerSponge, Matrix4Affine.CreateIdentity());

		//	sceneRoot.PrecomputeWorldTransform(Matrix4Affine.CreateIdentity());
		//	scene.SceneRoot = sceneRoot;
		//	return scene;
		//}


		private Scene createEmptyScene() {
			var scene = new Scene();
			scene.BgColor = BgColor;
			scene.ReflectionModel = new PhongReflectionModel();
			return scene;
		}

		public object sphere { get; set; }

		public object cube2 { get; set; }
	}
}
