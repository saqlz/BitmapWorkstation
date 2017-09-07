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
            byte[] imageArray;
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
        public static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            return ConvertByteArrayToBitmapImage(ConvertBitmapToByteArray(bitmap));
        }

        /// <summary>
        /// 将Bitmap转换成byte数组
        /// </summary>
        public static byte[] ConvertBitmapToByteArray(Bitmap bitmap)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, bitmap.RawFormat);
                    return ms.ToArray();
                }
            }
            catch(Exception)
            {
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// 将Bitmap转换成byte数组后继续转换成BitmapImage
        /// </summary>
        public static BitmapImage ConvertByteArrayToBitmapImage(byte[] imageArray)
        {
            using (var ms = new MemoryStream(imageArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        /// <summary>
        /// 将Bitmap转换成BitmapImage
        /// </summary>
        [Obsolete]
        public static BitmapImage ConvertBitmapToBitmapImage1(Bitmap bitmap)
        {
            var bitmapImage = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        public static BitmapImage GetBitmapFromMemory(string filePath, int imgWidth, int imgHeight)
        {
            Bitmap bitmap = null;
            byte[] imageArray;
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var binaryReader = new BinaryReader(fileStream);
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin); //Move to first
                imageArray = binaryReader.ReadBytes(Convert.ToInt32(binaryReader.BaseStream.Length));
            }

            var imageLength = imageArray.Length;
            using (var ms = new MemoryStream(imageLength + 14 + 40)) //为头腾出54个长度的空间
            {
                var buffer = new byte[13];
                buffer[0] = 0x42; //bitmap 固定常数
                buffer[1] = 0x4D; //bitmap 固定常数
                ms.Write(buffer, 0, 2); //先写入头的前两个字节

                buffer = BitConverter.GetBytes(imageLength);
                ms.Write(buffer, 0, 4); //把这个长度写入头中去

                buffer = BitConverter.GetBytes(0);
                ms.Write(buffer, 0, 4); //在写入4个字节长度的数据到头中去

                buffer = BitConverter.GetBytes(54);
                ms.Write(buffer, 0, 4); //固定常数也就是十六进制的0x36

                buffer = BitConverter.GetBytes(40); //写入信息头的长度biSize
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes(imgWidth); //写入信息头的图像宽度biWidth
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes(imgHeight); //写入信息头的图像高度biHeight
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes((short) 1); //写入信息头的biPlanes
                ms.Write(buffer, 0, 2);
                buffer = BitConverter.GetBytes((short) 24); //写入信息头的biBitCount
                ms.Write(buffer, 0, 2);
                buffer = BitConverter.GetBytes(0); //写入信息头的biCompression
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes(0); //写入信息头的biSizeImage
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes(0); //写入信息头的biXPelsPerMeter
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes(0); //写入信息头的biYPelsPerMeter
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes(0); //写入信息头的biClrUsed
                ms.Write(buffer, 0, 4);
                buffer = BitConverter.GetBytes(0); //写入信息头的biClrImportant
                ms.Write(buffer, 0, 4);
                ms.Write(imageArray, 0, imageLength);
                bitmap = new Bitmap(ms); //用内存流构造出一幅bitmap的图片
            }
            bitmap.Save("D:\\test.bmp");
            return ConvertBitmapToBitmapImage(bitmap);
        }
        
        ///// <summary>
        ///// Test 
        ///// </summary>
        //public static ImageFormat GetImageFormat(Bitmap bitmap)
        //{
        //    var ms = new MemoryStream();
        //    foreach (var format in new ImageFormat[]{
        //                            ImageFormat.Bmp,
        //                            ImageFormat.Emf,
        //                            ImageFormat.Exif,
        //                            ImageFormat.Gif,
        //                            ImageFormat.Icon,
        //                            ImageFormat.Jpeg,
        //                            ImageFormat.MemoryBmp,
        //                            ImageFormat.Png,
        //                            ImageFormat.Tiff,
        //                            ImageFormat.Wmf})
        //    {

        //        try
        //        {
        //            bitmap.Save(ms, format);
        //            return format;
        //        }
        //        catch (Exception) { }
        //    }
        //    return ImageFormat.Bmp;
        //}

    }
}
