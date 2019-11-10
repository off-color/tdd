﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Program
    {
        public static void Main()
        {
            foreach (var generatedPictureInfo in GetGeneratedPictureInfos())
            {
                var rectangles = GetLayoutedRectangles(generatedPictureInfo);
                var visualizer = new Visualizer(generatedPictureInfo.PictureSize);
                var bitmap = visualizer.GetLayoutBitmap(rectangles, Color.Aqua, Color.Black);
                bitmap.Save($"{generatedPictureInfo.Name}.png", ImageFormat.Png);
            }
        }

        private static IEnumerable<Rectangle> GetLayoutedRectangles(PictureInfo pictureInfo)
        {
            var layouter = new CircularCloudLayouter(pictureInfo.PictureCenter, pictureInfo.PictureSize);

            return pictureInfo.RectangleSizes.Select(s => layouter.PutNextRectangle(s));
        }

        private static IEnumerable<PictureInfo> GetGeneratedPictureInfos()
        {
            yield return new PictureInfo(new Point(400, 300), new Size(800, 600),
                Enumerable.Repeat(new Size(100, 50), 20).Concat(Enumerable.Repeat(new Size(100, 25), 25))
                    .Concat(Enumerable.Repeat(new Size(50, 25), 9)).Prepend(new Size(200, 100)), "descending layout");

            yield return new PictureInfo(new Point(400, 300), new Size(800, 600),
                Enumerable.Repeat(new Size(10, 10), 1000), "many little squares");

            yield return new PictureInfo(new Point(400, 300), new Size(800, 600),
                Enumerable.Repeat(new Size(75, 25), 30).Concat(Enumerable.Repeat(new Size(100, 50), 20))
                    .Concat(Enumerable.Repeat(new Size(150, 50), 10)), "ascending layout");
        }
    }
}