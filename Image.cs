using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PicDotter
{
    public class Image
    {
        public Bitmap Bitmap { get; set; }
        public int Average { get; set; }
        public string Name { get; set; }

        public Image(string filePath)
        {
            Bitmap original = new Bitmap(filePath);
            this.Bitmap = this.ToGrayscale(original);
            this.Bitmap.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "grayscaled.jpg"));
            int index = filePath.LastIndexOf('\\') + 1;
            if (index < 0)
                index = filePath.LastIndexOf('/') + 1;
            if (index < 0)
                index = 0;
            this.Name = filePath.Substring(index);
            this.ComputeAverage();
        }

        private void ComputeAverage()
        {
            List<int> values = new List<int>();
            for (int i = 0; i < Bitmap.Width; i++)
            {
                for (int k = 0; k < Bitmap.Height; k++)
                {
                    int value = this.Bitmap.GetPixel(i, k).R;
                    values.Add(value);
                }
            }

            int average = (int)values.Average();
            this.Average = average;
        }

        private Bitmap ToGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }


    }
}
