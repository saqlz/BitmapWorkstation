using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapWorkstation.Helper
{
    public static class ByteArrayToBitmap
    {
        public static Bitmap GeneratedBitmap(string filePath, int imgWidth, int imgHeight, PixelFormat format = PixelFormat.Format24bppRgb)
        {
            var bitmapImage = new Bitmap(imgWidth, imgHeight, format);

            return bitmapImage;
        }
        


    }
}
