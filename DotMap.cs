using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
                        int value = image.Bitmap.GetPixel(i, k).B;
                        bool isDot = value >= 150;
                        this.Pixels[i, k] = isDot;
                    }
                }
            }
            this.SaveBlackWhiteImage();
        }

        public string[] GetDots()
        {
            int width = this.Pixels.GetLength(0);
            int height = this.Pixels.GetLength(1);
            this.DotStrings = new string[width];
            int counter = 0;
            for (int i = 0; i < height; i += 3)
            {
                StringBuilder row = new StringBuilder();
                byte[] bytes = new byte[width];
                for (int k = 0; k < width; k += 2)
                {
                    bool[] binary = {
                        this.Pixels[k, i],
                        this.Pixels[k, i + 1],
                        this.Pixels[k, i + 2],
                        this.Pixels[k + 1, i],
                        this.Pixels[k + 1, i + 1],
                        this.Pixels[k + 1, i + 2]
                    };
                    byte charByte = this.EncodeBool(binary);
                    char character = Convert.ToChar(charByte + 10240);
                    row.Append(character);
                }
                string rowString = row.ToString();
                this.DotStrings[counter++] = rowString;
            }

            return this.DotStrings;
        }

        private void SaveBlackWhiteImage()
        {
            int width = this.Pixels.GetLength(0);
            int height = this.Pixels.GetLength(1);
            Bitmap bitmap = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
            {
                for (int k = 0; k < height; k++)
                {
                    Color color = Color.White;
                    if (this.Pixels[i, k])
                        color = Color.Black;
                    bitmap.SetPixel(i, k, color);                                       
                }
            }

            bitmap.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "blackwhite.jpg"));
        }

        byte EncodeBool(bool[] source)
        {
            byte result = 0;
            // This assumes the array never contains more than 8 elements!
            int index = 8 - source.Length;

            // Loop through the array
            foreach (bool b in source)
            {
                // if the element is 'true' set the bit at that position
                if (b)
                    result |= (byte)(1 << (7 - index));

                index++;
            }

            return result;
        }
    }
}
