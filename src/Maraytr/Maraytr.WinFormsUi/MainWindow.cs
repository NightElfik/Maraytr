using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Maraytr.Cameras;
using Maraytr.Lights;
using Maraytr.Materials;
using Maraytr.Materials.Textures;
using Maraytr.Numerics;
using Maraytr.RayCasting;
using Maraytr.RayTracing;
using Maraytr.Rendering;
using Maraytr.Scenes;
using Maraytr.Scenes.Csg;
using Maraytr.Scenes.Csg.Primitives;

namespace Maraytr.WinFormsUi {
	public partial class frmMainWindow : Form {

		private RayTracer rayTracer;


		public frmMainWindow() {
			InitializeComponent();

			var cube = new Cube();
			var sphere = new Sphere();
			var plane = new HalfSpace() {
				Width = 0.1
			};

			var scene = new Scene();
			scene.BgColor = new ColorRgbt(0, 0, 0, 1);
			scene.Camera = new PerspectiveCamera(new Vector3(0, 1, 6), new Vector3(0, 0, 0), new Vector3(0, 6, -1), 640, 480, 90);
			scene.AmbientLight = new ColorRgbt(0.05f, 0.05f, 0.05f);
			//scene.Lights.Add(new PointLight(new Vector3(0, 6, 3), new ColorRgbt(1, 1, 1)));
			scene.Lights.Add(new AreaLightSource(new Vector3(0, 6, 3), new ColorRgbt(1, 1, 1), Vector3.ZAxis, Vector3.XAxis));

			var cube1 = new CsgObjectNode(cube,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(1, 0, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ShininessCoef = 128
				});
			var sphere2 = new CsgObjectNode(sphere,
				new PhongMaterial() {
					Texture = new CheckerTexture() { UFrequency = 30, VFrequency = 30 },
					BaseColor = new ColorRgbt(0, 1, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ShininessCoef = 128
				});
			var sphere3 = new CsgObjectNode(sphere,
				new PhongMaterial() {
					Texture = new StripsTexture() { Frequency = 40, Orientation = true },
					BaseColor = new ColorRgbt(0, 0, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ReflectionFactor = 0.5f,
					ShininessCoef = 128
				});
			var plane1 = new CsgObjectNode(plane,
				new PhongMaterial() {
					Texture = new CheckerTexture(),
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
				});
			var plane2 = new CsgObjectNode(plane,
				new PhongMaterial() {
					Texture = new CheckerTexture(),
					BaseColor = new ColorRgbt(1, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
				});

			var sphereIntersec1 = new CsgObjectNode(sphere,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(0, 1, 1),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0.5f, 0.5f, 0.5f),
					ShininessCoef = 256
				});
			var cubeIntersec2 = new CsgObjectNode(cube,
				new PhongMaterial() {
					Texture = new CheckerTexture() { UFrequency = 5, VFrequency = 5 },
					BaseColor = new ColorRgbt(1, 1, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(1, 1, 1),
					ShininessCoef = 128
				});


			var mirrorPlane = new CsgObjectNode(plane,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(0, 0, 0),
					DiffuseReflectionCoef = new ColorRgbt(1, 1, 1),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0),
					ReflectionFactor = 1f,
					ShininessCoef = 1
				});

			var mirrorCut = new CsgObjectNode(cube,
				new PhongMaterial() {
					BaseColor = new ColorRgbt(0.5f, 0.5f, 0.5f),
					DiffuseReflectionCoef = new ColorRgbt(0.5f, 0.5f, 0.5f),
					SpecularReflectionCoef = new ColorRgbt(0, 0, 0)
				});


			var union = new CsgBoolOperationNode(CsgBoolOperation.Union);
			union.AddChildNode(cube1, Matrix4Affine.CreateTranslation(new Vector3(-1.6, 0, 0.2)) * Matrix4Affine.CreateScale(0.6));
			union.AddChildNode(sphere3, Matrix4Affine.CreateTranslation(new Vector3(1.5, 0, 0)));
			union.AddChildNode(plane1, Matrix4Affine.CreateIdentity());
			union.AddChildNode(plane2, Matrix4Affine.CreateTranslation(new Vector3(0, -0.35, 0)));

			var diff = new CsgBoolOperationNode(CsgBoolOperation.Difference);
			diff.AddChildNode(union, Matrix4Affine.CreateIdentity());
			diff.AddChildNode(sphere2, Matrix4Affine.CreateTranslation(new Vector3(0, 0, 1)) * Matrix4Affine.CreateScale(1.4));

			var intersec = new CsgBoolOperationNode(CsgBoolOperation.Intersection);
			intersec.AddChildNode(sphereIntersec1, Matrix4Affine.CreateTranslation(new Vector3(0.4, 0, 0)));
			intersec.AddChildNode(cubeIntersec2, Matrix4Affine.CreateTranslation(new Vector3(0, -0.1, -0.1)));

			var mirror = new CsgBoolOperationNode(CsgBoolOperation.Intersection);
			mirror.AddChildNode(mirrorPlane, Matrix4Affine.CreateRotationX(90 * MathHelper.DegToRad));
			mirror.AddChildNode(mirrorCut, Matrix4Affine.CreateTranslation(new Vector3(0, 0.5, -0.5))
				* Matrix4Affine.CreateScale(new Vector3(4, 1.8, 1)));

			var sceneRoot = new CsgBoolOperationNode(CsgBoolOperation.Union);
			sceneRoot.AddChildNode(diff, Matrix4Affine.CreateIdentity());
			sceneRoot.AddChildNode(intersec, Matrix4Affine.CreateTranslation(new Vector3(0.8, 0, 3)) * Matrix4Affine.CreateScale(0.5));
			sceneRoot.AddChildNode(mirror, Matrix4Affine.CreateTranslation(new Vector3(-2, 0, -2))
				* Matrix4Affine.CreateRotationX(15 * MathHelper.DegToRad));

			sceneRoot.PrecomputeWorldTransform(Matrix4Affine.CreateIdentity());
			scene.SceneRoot = sceneRoot;

			scene.ReflectionModel = new PhongReflectionModel();

			rayTracer = new RayTracer(scene) {
				MaxTraceDepth = 16,
				MinContribution = 0.01
			};


		}

		private void btnRender_Click(object sender, EventArgs e) {

			int width = (int)rayTracer.Size.Width;
			int height = (int)rayTracer.Size.Height;

			var img = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			//rayTracer.GetSample(550, height - 300, new IntegrationState());

			var resultArr = new ColorRgbt[height, width];
			var ssif = new SupersamplingImageFunction(rayTracer) { SuperSampling = 1 };
			var pff = new ParalellImageFuncFetcher(ssif);
			var asyncResult = pff.FetchAsync(resultArr);

			pbImage.Size = img.Size;
			pbImage.Image = img;

			while (!asyncResult.IsCompleted) {
				copyResult(img, resultArr);
				pbImage.Invalidate();
				Application.DoEvents();
				Thread.Sleep(200);
			}

			copyResult(img, resultArr);
			pbImage.Invalidate();


		}

		private void copyResult(Bitmap img, ColorRgbt[,] colorArr) {

			int width = img.Width;
			int height = img.Height;

			var imgData = img.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			unsafe {
				byte* scan0 = (byte*)imgData.Scan0;
				int stride = imgData.Stride;

				for (int y = 0; y < height; y++) {
					byte* ptr = scan0 + y * stride;
					for (int x = 0; x < width; x++) {
						ColorRgbt color = colorArr[height - y - 1, x];
						ptr[0] = (byte)(color.B.Clamp01() * 255);
						ptr[1] = (byte)(color.G.Clamp01() * 255);
						ptr[2] = (byte)(color.R.Clamp01() * 255);
						ptr[3] = (byte)((1 - color.T.Clamp01()) * 255);
						ptr += 4;
					}
				}
			}

			img.UnlockBits(imgData);

		}

		private void pbImage_MouseMove(object sender, MouseEventArgs e) {
			Bitmap img = pbImage.Image as Bitmap;
			Text = string.Format("{0}  {1}  #{2}", e.X, e.Y, img != null ? Convert.ToString(img.GetPixel(e.X, e.Y).ToArgb(), 16) : "??");
		}

		private void btnSave_Click(object sender, EventArgs e) {
			if (pbImage.Image == null) {
				return;
			}

			if (saveFileDialog.ShowDialog(this) != DialogResult.OK) {
				return;
			}

			pbImage.Image.Save(saveFileDialog.FileName, ImageFormat.Png);
		}

		private void pbImage_MouseClick(object sender, MouseEventArgs e) {
			rayTracer.GetSample(e.X, (int)rayTracer.Size.Height - e.Y, new IntegrationState());
		}

	}
}
