using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyImage
    {
        /// <summary>
        /// 根据指定的文件名或者是url，判断是否是一个图片文件
        /// </summary>
        /// <param name="filePathOrUrl"></param>
        /// <returns></returns>
        public static Boolean IsImageFile(String filePathOrUrl)
        {
            List<String> imgTypes = GetUnknowImageType();
            return MyString.EndsWith(filePathOrUrl, imgTypes);
        }

        /// <summary>
        /// 获取已知的图片类型，返回图片类型列表：.jpeg,.jpg。。。。。。。
        /// </summary>
        /// <returns></returns>
        public static List<String> GetUnknowImageType()
        {
            List<String> imgs = new List<String>();
            imgs.Add(".jpeg");
            imgs.Add(".jpg");
            imgs.Add(".gif");
            imgs.Add(".bmp");
            imgs.Add(".emf");
            imgs.Add(".exif");
            imgs.Add(".icon");
            imgs.Add(".png");
            imgs.Add(".tiff");
            imgs.Add(".wmf");
            imgs.Add(".jfif");
            imgs.Add(".tif");
            return imgs;
        }

        /// <summary>
        /// 根据图片的类型，获取ImageFormat的类型，输入的字符串不要包含点号。格式应该输入如：jpg;gif等
        /// </summary>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static ImageFormat GetImageFormatByImageType(String inputImageType)
        {
            ImageFormat imageType = ImageFormat.Bmp;
            if (inputImageType == "jpeg" || inputImageType == "jpg")
                imageType = ImageFormat.Jpeg;
            else if (inputImageType == "gif")
                imageType = ImageFormat.Gif;
            else if (inputImageType == "bmp")
                imageType = ImageFormat.Bmp;
            else if (inputImageType == "emf")
                imageType = ImageFormat.Emf;
            else if (inputImageType == "exif")
                imageType = ImageFormat.Exif;
            else if (inputImageType == "icon")
                imageType = ImageFormat.Icon;
            else if (inputImageType == "png")
                imageType = ImageFormat.Png;
            else if (inputImageType == "tiff")
                imageType = ImageFormat.Tiff;
            else if (inputImageType == "wmf")
                imageType = ImageFormat.Wmf;
            return imageType;
        }

        /// <summary>
        /// 根据ImageFormat的类型，获取图片的类型（图片文件的后缀名）
        /// </summary>
        /// <param name="formatType"></param>
        /// <returns></returns>
        public static String GetImageTypeByImageFormat(ImageFormat formatType)
        {
            if (formatType == ImageFormat.Bmp)
                return "bmp";
            if (formatType == ImageFormat.Emf)
                return "emf";
            if (formatType == ImageFormat.Exif)
                return "exif";
            if (formatType == ImageFormat.Gif)
                return "gif";
            if (formatType == ImageFormat.Icon)
                return "icon";
            if (formatType == ImageFormat.Jpeg)
                return "jpeg";
            if (formatType == ImageFormat.Png)
                return "png";
            if (formatType == ImageFormat.Tiff)
                return "tiff";
            if (formatType == ImageFormat.Wmf)
                return "wmf";
            return "bmp";
        }

        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }

        /// <summary>
        /// 根据图片路径获取图片的款高度(以像素为单位)
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static Int32[] GetImageWidthHeight(String imagePath)
        {
            Image image = Image.FromFile(imagePath);
            return new Int32[] { image.Width, image.Height };
        }

        /// <summary>
        /// 把图片转换成base64编码
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static String ToBase64(String imagePath)
        {
            if (String.IsNullOrEmpty(imagePath)) return null;
            Byte[] data = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// 控制图片不能大于最大大小
        /// </summary>
        /// <param name="imagepath">图片路径</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="maxSize">最大的大小（字节为单位）</param>
        public static void MinSize(Int32 width, Int32 height, String imagepath, Int32 maxSize = 0x75300)
        {
            FileStream imageStream = null;
            try
            {
                imageStream = new FileStream(imagepath, FileMode.Open);
                if (imageStream.Length > maxSize)
                {
                    imageStream.Dispose();
                    Image img = Image.FromFile(imagepath);
                    Bitmap bitmap = new Bitmap(width, height);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.Clear(Color.White);
                    graphics.InterpolationMode = InterpolationMode.High;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    Rectangle rt1 = new Rectangle(0, 0, width, height);
                    graphics.DrawImage(img, rt1);
                    graphics.Dispose();
                    img.Dispose();
                    bitmap.Save(imagepath);
                    bitmap.Dispose();
                }
            }
            finally
            {
                if (imageStream != null) imageStream.Dispose();
            }
        }

        /// <summary>
        /// 调整图片的尺寸（改变图片的比例）
        /// </summary>
        /// <param name="imageFileFullPath"></param>
        /// <param name="newImageFileFullPath"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static void ResizeImage(String imageFileFullPath, String newImageFileFullPath, Int32 newWidth, Int32 newHeight)
        {
            Graphics oGraphic = null;
            Bitmap oBitMap = null;
            try
            {
                oBitMap = (Bitmap)Bitmap.FromFile(imageFileFullPath);

                Bitmap oNewImage = new Bitmap(newWidth, newHeight);
                oGraphic = Graphics.FromImage(oNewImage);

                //设置高质量插值法  
                oGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度  
                oGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                oGraphic.Clear(Color.Transparent);

                oGraphic.DrawImage(oBitMap, new Rectangle(0, 0, newWidth, newHeight));
                oNewImage.Save(newImageFileFullPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            finally
            {
                if (oGraphic != null) oGraphic.Dispose();
                if (oBitMap != null) oBitMap.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">原始路径</param>
        /// <param name="thumbnailPath">生成缩略图路径</param>
        /// <param name="width">缩略图的宽</param>
        /// <param name="height">缩略图的高</param>
        public static void MakeThumbnail(String originalImagePath, String thumbnailPath, Int32 width, Int32 height)
        {
            FileStream imageStream = null;
            try
            {
                Image img = Image.FromFile(originalImagePath);
                if (img.Height <= height && img.Width <= width) return;
                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                //graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                Rectangle rt1 = new Rectangle(0, 0, width, height);
                graphics.DrawImage(img, rt1);
                graphics.Dispose();
                img.Dispose();
                bitmap.Save(thumbnailPath);
                bitmap.Dispose();
            }
            finally
            {
                if (imageStream != null) imageStream.Dispose();
            }
        }

        /// <summary>
        /// 生成缩略图//带压缩图片不压缩22k压缩2k
        /// </summary>
        /// <param name="originalImagePath">原始路径</param>
        /// <param name="thumbnailPath">生成缩略图路径</param>
        /// <param name="width">缩略图的宽</param>
        /// <param name="height">缩略图的高</param>
        /// <param name="Ys">是否压缩图片质量</param>
        public static void MakeThumbnail(String originalImagePath, String thumbnailPath, Int32 width, Int32 height, Boolean Ys)
        {
            //获取原始图片  
            Image originalImage = Image.FromFile(originalImagePath);
            //缩略图画布宽高  
            Int32 towidth = width;
            Int32 toheight = height;
            //原始图片写入画布坐标和宽高(用来设置裁减溢出部分)  
            Int32 x = 0;
            Int32 y = 0;
            Int32 ow = originalImage.Width;
            Int32 oh = originalImage.Height;
            //原始图片画布,设置写入缩略图画布坐标和宽高(用来原始图片整体宽高缩放)  
            Int32 bg_x = 0;
            Int32 bg_y = 0;
            Int32 bg_w = towidth;
            Int32 bg_h = toheight;
            //倍数变量  
            Double multiple = 0;
            //获取宽长的或是高长与缩略图的倍数  
            if (originalImage.Width >= originalImage.Height)
                multiple = (Double)originalImage.Width / (Double)width;
            else
                multiple = (Double)originalImage.Height / (Double)height;
            //上传的图片的宽和高小等于缩略图  
            if (ow <= width && oh <= height)
            {
                //缩略图按原始宽高  
                bg_w = originalImage.Width;
                bg_h = originalImage.Height;
                //空白部分用背景色填充  
                bg_x = Convert.ToInt32(((Double)towidth - (Double)ow) / 2);
                bg_y = Convert.ToInt32(((Double)toheight - (Double)oh) / 2);
            }
            else //上传的图片的宽和高大于缩略图
            {
                //宽高按比例缩放  
                bg_w = Convert.ToInt32((Double)originalImage.Width / multiple);
                bg_h = Convert.ToInt32((Double)originalImage.Height / multiple);
                //空白部分用背景色填充  
                bg_y = Convert.ToInt32(((Double)height - (Double)bg_h) / 2);
                bg_x = Convert.ToInt32(((Double)width - (Double)bg_w) / 2);
            }
            //新建一个bmp图片,并设置缩略图大小.  
            Image bitmap = new Bitmap(towidth, toheight);
            //新建一个画板  
            Graphics g = Graphics.FromImage(bitmap);
            //设置高质量插值法  
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = SmoothingMode.HighQuality;
            //清空画布并设置背景色  
            //g.Clear(System.Drawing.ColorTranslator.FromHtml("#FFF"));
            //在指定位置并且按指定大小绘制原图片的指定部分  
            //第一个System.Drawing.Rectangle是原图片的画布坐标和宽高,第二个是原图片写在画布上的坐标和宽高,最后一个参数是指定数值单位为像素  
            g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, bg_w, bg_h), new System.Drawing.Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
            //g.DrawImage(originalImage, new System.Drawing.Rectangle(bg_x, bg_y, bg_w, bg_h), new System.Drawing.Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);

            if (Ys)
            {
                ImageCodecInfo encoder = GetEncoderInfo("image/jpeg");
                try
                {
                    if (encoder != null)
                    {
                        System.Drawing.Imaging.EncoderParameters encoderParams = new System.Drawing.Imaging.EncoderParameters(1);
                        //设置 jpeg 质量为 60
                        encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)60);
                        bitmap.Save(thumbnailPath, encoder, encoderParams);
                        encoderParams.Dispose();
                    }
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
            }
            else
            {
                try
                {
                    //获取图片类型  
                    String fileExtension = System.IO.Path.GetExtension(originalImagePath).ToLower();
                    //按原图片类型保存缩略图片,不按原格式图片会出现模糊,锯齿等问题.  
                    switch (fileExtension)
                    {
                        case ".gif": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Gif); break;
                        case ".jpg": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg); break;
                        case ".bmp": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Bmp); break;
                        case ".png": bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Png); break;
                    }
                }
                finally
                {
                    originalImage.Dispose();
                    bitmap.Dispose();
                    g.Dispose();
                }
            }
        }

        /// <summary>
        /// 把图标转成image类型的对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="imageType">图片格式</param>
        /// <returns></returns>
#if NET20
        public static Image ToImage(Icon target, ImageFormat imageType = null)
#else 
        public static Image ToImage(this Icon target, ImageFormat imageType = null)
#endif
        {
            MemoryStream ms = new MemoryStream();
            Bitmap bitmap = Bitmap.FromHicon(target.Handle);
            Image image = Image.FromHbitmap(bitmap.GetHbitmap());
            image.Save(ms, imageType ?? ImageFormat.Jpeg);
            return image;
        }

        /// <summary>  
        /// 判断图片的背景是否为透明  
        /// 如果为透明则修改图片的背景为白色  
        /// 如果不透明则不修改图片的背景颜色  
        /// </summary>  
        /// <param name="src"></param>  
        /// <returns></returns>  
        public static Bitmap PTransparentAdjust(Bitmap src)
        {
            int w = src.Width;
            int h = src.Height;
            Bitmap dstBitmap = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Imaging.BitmapData srcData = src.LockBits(new System.Drawing.Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Imaging.BitmapData dstData = dstBitmap.LockBits(new System.Drawing.Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* pIn = (byte*)srcData.Scan0.ToPointer();
                byte* pOut = (byte*)dstData.Scan0.ToPointer();
                byte* p;
                int stride = srcData.Stride;
                int r, g, b, a;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        p = pIn;
                        b = pIn[0];
                        g = pIn[1];
                        r = pIn[2];
                        a = pIn[3];
                        if (a == 0)
                        {
                            pOut[1] = (byte)255;
                            pOut[2] = (byte)255;
                            pOut[3] = (byte)255;
                            pOut[0] = (byte)255;
                        }
                        else
                        {
                            pOut[1] = (byte)g;
                            pOut[2] = (byte)r;
                            pOut[3] = (byte)a;
                            pOut[0] = (byte)b;
                        }
                        pIn += 4;
                        pOut += 4;
                    }
                    pIn += srcData.Stride - w * 4;
                    pOut += srcData.Stride - w * 4;
                }
                src.UnlockBits(srcData);
                dstBitmap.UnlockBits(dstData);
                return dstBitmap;
            }
        }

        /// <summary>
        /// 把图标转成image类型的对象
        /// </summary>
        /// <param name="target"></param>
        /// <param name="operType"></param>
        /// <returns></returns>
#if NET20
        public static Boolean TryToImage(Icon target, out Image image, ImageFormat imageType = null, Image defaultValue = null)
#else 
        public static Boolean TryToImage(this Icon target, out Image image, ImageFormat imageType = null, Image defaultValue = null)
#endif
        {
            image = defaultValue;
            try
            {
                MemoryStream ms = new MemoryStream();
                Bitmap bitmap = Bitmap.FromHicon(target.Handle);
                image = Image.FromHbitmap(bitmap.GetHbitmap());
                image.Save(ms, imageType ?? ImageFormat.Jpeg);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region "私有方法"

        /// <summary>
        /// 根据 mime 类型，返回编码器
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            System.Drawing.Imaging.ImageCodecInfo result = null;
            System.Drawing.Imaging.ImageCodecInfo[] encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < encoders.Length; i++)
            {
                if (encoders[i].MimeType == mimeType)
                {
                    result = encoders[i];
                    break;
                }

            }
            return result;
        }

        #endregion
    }
}
