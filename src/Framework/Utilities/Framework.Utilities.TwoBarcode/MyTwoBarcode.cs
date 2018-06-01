using System;
using System.Drawing;

//二维码的dll
using com.google.zxing;
using COMMON = com.google.zxing.common;

namespace DreamCube.Framework.Utilities.TwoBarcode
{
    /// <summary>
    /// 二维码
    /// </summary>
    public static class MyTwoBarcode
    {
        /// <summary>
        /// 读取二维码图片获取数据
        /// </summary>
        /// <param name="fileName"></param>
        public static String Read(String fileName)
        {
            Image img = Image.FromFile(fileName);
            Bitmap bmap = new Bitmap(img);
            LuminanceSource source = new RGBLuminanceSource(bmap, bmap.Width, bmap.Height);
            com.google.zxing.BinaryBitmap bitmap = new com.google.zxing.BinaryBitmap(new COMMON.HybridBinarizer(source));
            Result result = new MultiFormatReader().decode(bitmap);
            return result.Text;
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="fileName">图片文件路径</param>
        /// <param name="data">二维码的数据</param>
        /// <returns></returns>
        public static void Generate(String imageFileFullPath, String data)
        {
            try
            {
                COMMON.ByteMatrix byteMatrix = new MultiFormatWriter().encode(data, BarcodeFormat.QR_CODE, 350, 350);
                WriteToFile(byteMatrix, System.Drawing.Imaging.ImageFormat.Png, imageFileFullPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region "私有方法"

        private static void WriteToFile(COMMON.ByteMatrix matrix, System.Drawing.Imaging.ImageFormat format, string file)
        {
            Bitmap bmap = ToBitmap(matrix);
            bmap.Save(file, format);
        }

        private static Bitmap ToBitmap(COMMON.ByteMatrix matrix)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bmap.SetPixel(x, y, matrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
                }
            }
            return bmap;
        }

        #endregion
    }
}
