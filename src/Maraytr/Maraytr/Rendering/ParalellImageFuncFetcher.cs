using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Rendering {
	public class ParalellImageFuncFetcher {

		private IImageFunction imgFunc;
		private ColorRgbt[,] resultArray;

		private int width, height;

		delegate void FetchDel();

		public ParalellImageFuncFetcher(IImageFunction iFunc) {
			imgFunc = iFunc;
		}

		public IAsyncResult FetchAsync(ColorRgbt[,] result) {

			resultArray = result;
			width = (int)imgFunc.Size.Width;
			height = (int)imgFunc.Size.Height;

			var del = new FetchDel(fetchImgFunc);
			return del.BeginInvoke(null, null);
		}

		private void fetchImgFunc() {
			Parallel.For(0, width * height, i => {
				int x = i % width;
				int y = i / width;
				resultArray[y, x] = imgFunc.GetSample(x, y, new IntegrationState());
			});
		}

	}
}
