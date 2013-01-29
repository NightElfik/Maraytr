using System;
using System.Drawing;
using System.Windows.Forms;
using Maraytr.Cameras;
using Maraytr.Lights;
using Maraytr.Materials;
using Maraytr.Materials.Textures;
using Maraytr.Numerics;
using Maraytr.RayCasting;
using Maraytr.Scenes;
using Maraytr.Scenes.Csg;
using Maraytr.Scenes.Csg.Primitives;

namespace Maraytr.WinFormsUi {
	public partial class frmMainWindow : Form {
		public frmMainWindow() {
			InitializeComponent();
		}

		private void btnRender_Click(object sender, EventArgs e) {

			var scene = new Scene();
			scene.BgColor = new ColorRgbt(0, 0, 0, 1);
			scene.Camera = new PerspectiveCamera(new Vector3(0, 1, 6), new Vector3(0, 0, 0), new Vector3(0, 6, -1), 512, 512, 90);
			scene.Lights.Add(new PointLight(new Vector3(0, 3, 1), new ColorRgbt(1, 1, 1)));

			var sphere1 = new CsgSphere();
			sphere1.Material = new PhongMaterial() {
				Texture = new CheckerTexture() { UFrequency = 30, VFrequency = 30 },
				DiffuseReflectionCoef = new ColorRgbt(1, 0, 0),
				SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
				ShininessCoef = 128
			};
			var sphere2 = new CsgSphere();
			sphere2.Material = new PhongMaterial() {
				Texture = new CheckerTexture() { UFrequency = 30, VFrequency = 30 },
				DiffuseReflectionCoef = new ColorRgbt(0, 1, 0),
				SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
				ShininessCoef = 128
			};
			var sphere3 = new CsgSphere();
			sphere3.Material = new PhongMaterial() {
				Texture = new CheckerTexture() { UFrequency = 30, VFrequency = 30 },
				DiffuseReflectionCoef = new ColorRgbt(0, 0, 1),
				SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
				ShininessCoef = 128
			};
			var plane = new CsgHalfSpace();
			plane.Material = new PhongMaterial() {
				Texture = new CheckerTexture(),
				DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
				SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
				ShininessCoef = 1
			};

			var union = new CsgBoolOperationNode(CsgBoolOperation.Union);
			union.AddChildNode(sphere1, Matrix4Affine.CreateTranslation(new Vector3(-1, 0, 0)));
			union.AddChildNode(sphere3, Matrix4Affine.CreateTranslation(new Vector3(1, 0, 0)));
			union.AddChildNode(plane, Matrix4Affine.CreateIdentity());

			var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Difference);
			sceneRoot.AddChildNode(union, Matrix4Affine.CreateIdentity());
			sceneRoot.AddChildNode(sphere2, Matrix4Affine.CreateTranslation(new Vector3(0, 0, 1)) * Matrix4Affine.CreateScale(1.5));
			sceneRoot.PrecomputeWorldTransform(Matrix4Affine.CreateIdentity());
			scene.SceneRoot = sceneRoot;

			scene.ReflectionModel = new PhongReflectionModel();

			var rayCaster = new RayCaster(scene);

			int width = (int)rayCaster.Size.Width;
			int height = (int)rayCaster.Size.Height;

			var img = new Bitmap(width, height);

			//rayCaster.GetSample(25, 512-413, new IntegrationState());

			for (int y = 0; y < height; ++y) {
				for (int x = 0; x < width; ++x) {
					ColorRgbt color = rayCaster.GetSample(x, y, new IntegrationState());
					img.SetPixel(x, height - y - 1,
						Color.FromArgb((byte)((1 - color.T.Clamp01()) * 255),
							(byte)(color.R.Clamp01() * 255), (byte)(color.G.Clamp01() * 255), (byte)(color.B.Clamp01() * 255)));
				}
			}

			pbImage.Image = img;

		}

		private void pbImage_MouseMove(object sender, MouseEventArgs e) {
			Text = string.Format("{0}  {1}", e.X, e.Y);
		}
	}
}
