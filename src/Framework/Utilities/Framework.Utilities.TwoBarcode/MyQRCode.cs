using System;
using System.IO;
using System.Drawing;

using DreamCube.Foundation.Basic.Utility;

//引入第三方的dll类库
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace DreamCube.Framework.Utilities.TwoBarcode
{
    public static class MyQRCode
    {
        /// <summary>
        /// 读取二维码图片获取数据
        /// </summary>
        /// <param name="imageFileFullPath"></param>
        /// <returns></returns>
        public static String Read(String imageFileFullPath)
        {
            QRCodeDecoder decoder = new QRCodeDecoder();
            return decoder.decode(new QRCodeBitmapImage(new Bitmap(imageFileFullPath)), System.Text.Encoding.UTF8);
        }

        /// <summary>
        /// 生成二维码图片
        /// </summary>
        /// <param name="data">二维码的数据</param>
        /// <param name="imageFileFullPath">图片文件路径</param>
        /// <param name="imageWidth">二维码图片宽度</param>
        /// <param name="imageHeight">二维码图片高度</param>
        /// <param name="logoImagePath">如果需要在二维码中间插入Logo，这里指定Logo的路径</param>
        /// <param name="logoWidth"></param>
        /// <param name="logoHeight"></param>
        public static void MakeQRCodeImage(String data, String imageFileFullPath, Int32 imageWidth = 165, Int32 imageHeight = 165, String logoImagePath = "", Int32 logoWidth = 0, Int32 logoHeight = 0)
        {
            //图片文件附件名
            String fileExtensionName = MyString.RightOfLast(imageFileFullPath, ".", true);
            String newImageFilePath = MyString.LeftOfLast(imageFileFullPath, ".", true, "") + "_temp." + fileExtensionName;
            QRCodeEncoder oEncoder = new QRCodeEncoder();
            oEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            oEncoder.QRCodeVersion = 0;
            System.Drawing.Bitmap oBitMap = oEncoder.Encode(data, System.Text.Encoding.UTF8);
            oBitMap.Save(newImageFilePath);
            oBitMap.Dispose();
            if (String.IsNullOrEmpty(logoImagePath))
            {
                //不需要插入Logo
                MyImage.ResizeImage(newImageFilePath, imageFileFullPath, imageWidth, imageHeight);
                MyIO.FileDelete(newImageFilePath);
            }
            else
            {
                String newImageWithoutLogoFilePath = MyString.LeftOfLast(imageFileFullPath, ".", true, "") + "_temp2." + fileExtensionName;
                MyImage.ResizeImage(newImageFilePath, newImageWithoutLogoFilePath, imageWidth, imageHeight);
                WriteLogoToErWeiMa(newImageWithoutLogoFilePath, logoImagePath, imageFileFullPath, logoWidth, logoHeight);
                MyIO.FileDelete(newImageWithoutLogoFilePath);
            }
        }

        /// <summary>
        /// 将Logo写入二维码
        /// </summary>
        /// <param name="sErWeiMaImagePath">原二维码图片的绝对路径</param>
        /// <param name="sLogoImagePath">Logo图片的绝对路径</param>
        /// <param name="sNewErWeiMaIamgePath">新二维码图片的保存路径</param>
        /// <param name="iLogoWidth">Logo的宽度</param>
        /// <param name="iLogoHeight">Logo的高度</param>
        /// <returns></returns>
        public static void WriteLogoToErWeiMa(String sErWeiMaImagePath, String sLogoImagePath, String sNewErWeiMaIamgePath, Int32 iLogoWidth = 0, Int32 iLogoHeight = 0)
        {
            FileStream fs = new FileStream(sLogoImagePath, FileMode.Open);
            Bitmap oLogoBitMap = (Bitmap)Bitmap.FromStream(fs);
            fs.Dispose();
            fs.Close();

            fs = new FileStream(sErWeiMaImagePath, FileMode.Open);
            Bitmap oEWMBitMap = (Bitmap)Bitmap.FromStream(fs);

            fs.Dispose();
            fs.Close();

            if (iLogoWidth == 0) iLogoWidth = oLogoBitMap.Width;

            if (iLogoHeight == 0) iLogoHeight = oLogoBitMap.Height;

            Graphics oGraphic = Graphics.FromImage(oEWMBitMap);
            oGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            oGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Int32 x = oEWMBitMap.Width / 2 - iLogoWidth / 2;
            Int32 y = oEWMBitMap.Height / 2 - iLogoHeight / 2;

            oGraphic.DrawImage(oLogoBitMap, new Rectangle(x, y, iLogoWidth, iLogoHeight));
            oEWMBitMap.Save(sNewErWeiMaIamgePath);
        }
    }
}
