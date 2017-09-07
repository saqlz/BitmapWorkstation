using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BitmapWorkstation.Helper
{
    public static class ByteArrayToBitmap
    {
        /// <summary>
        /// 使用BitmapData Class来将文件中读取为byte[]后生成Bitmap
        /// 难点是byte[]不能直接生成Bitmap，因为Bitmap存在信息头
        /// </summary>
        public static Bitmap GeneratedBitmap(string filePath, int imgWidth, int imgHeight, 
            PixelFormat format = PixelFormat.Format24bppRgb)
        {
            var bitmap = new Bitmap(imgWidth, imgHeight, format);
            var imageArray = new byte[0];
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var binaryReader = new BinaryReader(fileStream);
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin); //Move to first
                imageArray = binaryReader.ReadBytes(Convert.ToInt32(binaryReader.BaseStream.Length));
            }

            //Using BitmapData
            var bitmapImageData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var channel = 3;
            unsafe
            {
                for (var heightIndex = 0; heightIndex < bitmap.Height; heightIndex++)
                {
                    byte* rowPointer = (byte*)bitmapImageData.Scan0 + heightIndex*bitmapImageData.Stride;
                    for (var widthIndex = 0; widthIndex < bitmap.Width; widthIndex++)
                    {
                        var offset = (heightIndex*bitmap.Width + widthIndex) * channel;
                        rowPointer[widthIndex*channel + 0] = imageArray[offset + 2];     //Blue
                        rowPointer[widthIndex * channel + 1] = imageArray[offset + 1] ;  //Green
                        rowPointer[widthIndex * channel + 2] = imageArray[offset + 0];   //Red
                    }
                }
            }
            bitmap.UnlockBits(bitmapImageData);
            return bitmap;
        }

        /// <summary>
        /// 将Bitmap转换成BitmapImage
        /// </summary>
        public static BitmapImage ConvertBitmapToImage(Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

//         public static ImageFormat GetImageFormat(Bitmap bitmap)
//         {
//             var ms = new MemoryStream();
//             foreach (var format in new ImageFormat[]{
//                   ImageFormat.Bmp,
//                   ImageFormat.Emf,
//                   ImageFormat.Exif,
//                   ImageFormat.Gif,
//                   ImageFormat.Icon,
//                   ImageFormat.Jpeg,
//                   ImageFormat.MemoryBmp,
//                   ImageFormat.Png,
//                   ImageFormat.Tiff,
//                   ImageFormat.Wmf})
//             {
//                
//                 try
//                 {
//                     bitmap.Save(ms, format);
//                     return format;
//                 }
//                 finally { }
//             }
//             return ImageFormat.Bmp;
//         }

    }
}
