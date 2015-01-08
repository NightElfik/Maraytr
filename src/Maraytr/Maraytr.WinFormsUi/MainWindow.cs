using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using Maraytr.Numerics;
using Maraytr.Primitives;
using Maraytr.RayCasting;
using Maraytr.RayTracing;
using Maraytr.Rendering;

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


			var sceneFac = new DemoScenesFactory();
			
			//var scene = sceneFac.CreateSimpleReflectScene();	

			var scene = sceneFac.CreateIntroScene();	

			rayTracer = new RayTracer(scene) {
				MaxTraceDepth = 16,
				MinContribution = 0.01,
				ComputeShadows = true,
				//ShowNormals = true,
				//CountAmbientOcclusion = true,
				//AmbientOcclusionSamplesCount = 1 << 1,
				//AmbientOcclusionOnly = true,
			};
		}

		private void btnRender_Click(object sender, EventArgs e) {

			int width = (int)rayTracer.Size.Width;
			int height = (int)rayTracer.Size.Height;

			var img = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			//rayTracer.GetSample(550, height - 300, new IntegrationState());

			var resultArr = new ColorRgbt[height, width];
			var ssif = new SupersamplingImageFunction(rayTracer) { SuperSampling = 4 };
			var pff = new ParalellImageFuncFetcher(ssif);
			var asyncResult = pff.FetchAsync(resultArr);

			pbImage.Size = img.Size;
			pbImage.Image = img;

			while (!asyncResult.IsCompleted) {
				copyResult(img, resultArr);
				pbImage.Invalidate();
				Application.DoEvents();
				Thread.Sleep(500);
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

			if (rayTracer != null && img != null) {
				try {
					//var isec = rayTracer.GetIntersectionAt(e.X, img.Height - e.Y);
					//tsslStatus.Text = "Distance: " + (Math.Sqrt(Math.Abs(isec.RayDistanceSqSigned)) * Math.Sign(isec.RayDistanceSqSigned)).ToString();
				}
				catch { }
			}
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
			rayTracer.GetSample(e.X, (int)rayTracer.Size.Height - e.Y, new IntegrationState(1, 1));
		}

	}
}
