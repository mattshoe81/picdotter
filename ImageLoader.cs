using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicDotter
{
    class ImageLoader
    {

        public static Image Load(string filePath)
        {
            return new Image(filePath);
        }

    }
}
