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
            //var result = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format8bppIndexed);

            //BitmapData data = result.LockBits(new Rectangle(0, 0, result.Width, result.Height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            //// Copy the bytes from the image into a byte array
            //byte[] bytes = new byte[data.Height * data.Stride];
            //List<short> pixelValues = new List<short>();
            //Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);

            //for (int y = 0; y < bmp.Height; y++)
            //{
            //    for (int x = 0; x < bmp.Width; x++)
            //    {
            //        var c = bmp.GetPixel(x, y);
            //        var rgb = (byte)((c.R + c.G + c.B) / 3);
            //        pixelValues.Add((short)((c.R + c.G + c.B) / 3));

            //        bytes[x * data.Stride + y] = rgb;
            //    }
            //}
            //this.Average = (short)pixelValues.Average(x => x);

            //// Copy the bytes from the byte array into the image
            //Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);

            //result.UnlockBits(data);


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
