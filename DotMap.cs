using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicDotter
{
    class DotMap
    {
        private bool[,] Pixels;
        private string[] DotStrings;
        
        public DotMap(Image image)
        {
            int paddedWidth = image.Bitmap.Width + image.Bitmap.Width % 2;
            int paddedHeight = image.Bitmap.Height + image.Bitmap.Height % 3;
            this.Pixels = new bool[paddedWidth, paddedHeight];
            for (int i = 0; i < image.Bitmap.Width; i++)
            {
                for (int k = 0; k < image.Bitmap.Height; k++)
                {
                    if (image.Bitmap.Width < i || image.Bitmap.Height < k)
                    {
                        this.Pixels[i, k] = false;
                    }
                    else
                    {
                        int value = image.Bitmap.GetPixel(i, k).R;
                        bool isDot = value >= image.Average;
                        this.Pixels[i, k] = isDot;
                    }
                }
            }
        }

        public string[] GetDots()
        {
            int width = this.Pixels.GetLength(0) / 2;
            int height = this.Pixels.GetLength(1) / 3;
            this.DotStrings = new string[width];
            int counter = 0;
            for (int i = 0; i < width; i += 2)
            {
                StringBuilder row = new StringBuilder();
                for (int k = 0; k < height; k += 3)
                {
                    bool[] binary = {
                        this.Pixels[i, k],
                        this.Pixels[i, k + 1],
                        this.Pixels[i + 1, k],
                        this.Pixels[i + 1, k + 1],
                        this.Pixels[i + 2, k],
                        this.Pixels[i + 2, k + 1]
                    };
                    byte charByte = this.EncodeBool(binary);
                    char character = Convert.ToChar(charByte + 2800);
                    row.Append(character);
                }
                this.DotStrings[counter++] = row.ToString();
            }

            return this.DotStrings;
        }

        byte EncodeBool(bool[] arr)
        {
            byte val = 0;
            foreach (bool b in arr)
            {
                val <<= 1;
                if (b) val |= 1;
            }
            return val;
        }
    }
}
