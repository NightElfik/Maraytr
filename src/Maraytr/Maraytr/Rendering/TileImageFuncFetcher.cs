using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maraytr.Numerics;
using Maraytr.RayCasting;

namespace Maraytr.Rendering {
	public class TileImageFuncFetcher {


		private IImageFunction imgFunc;
		private ColorRgbt[,] resultArray;

		private int totalWidth, totalHeight;
		private int width, height;
		private int tileSize;

		delegate void FetchDel();

		private SpiralIndexer indexer = new SpiralIndexer();

		private IntegrationState emptyIntState = new IntegrationState();


		public TileImageFuncFetcher(IImageFunction iFunc, int tileSize) {
			imgFunc = iFunc;
			this.tileSize = tileSize;
		}

		public IAsyncResult FetchAsync(ColorRgbt[,] result) {

			resultArray = result;
			totalWidth = (int)imgFunc.Size.Width;
			totalHeight = (int)imgFunc.Size.Height;
			width = (totalWidth + tileSize - 1) / tileSize;
			height = (totalHeight + tileSize - 1) / tileSize;

			var del = new FetchDel(fetchImgFunc);
			return del.BeginInvoke(null, null);
		}

		private void fetchImgFunc() {
			Parallel.For(0, width * height, i => {
				var coord = indexer.GetCoordinates(i);
				int baseX = coord.X * tileSize;
				int baseY = coord.Y * tileSize;
				int maxX = Math.Min(tileSize, totalWidth - baseX);
				int maxY = Math.Min(tileSize, totalHeight - baseY);

				for (int y = 0; y < maxY; ++y) {
					for (int x = 0; x < maxX; ++x) {
						resultArray[baseY + y, baseX + x] = imgFunc.GetSample(x, y, emptyIntState);
					}
				}
			});
		}

	}
}
