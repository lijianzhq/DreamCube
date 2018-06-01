using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

//自定义命名空间
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.ValidateNO
{
    /// <summary>
    /// 验证码字符串类
    /// </summary>
    public class ValidateString
    {
        #region "私有字段"

        /// <summary>
        /// 伪随机数生成器
        /// </summary>
        private readonly Random rand;

        /// <summary>
        /// 字体名称
        /// </summary>
        private String fontFamilyName;

        /// <summary>
        /// 生成图片的高度
        /// </summary>
        private Int32 height;

        /// <summary>
        /// 生成图片的宽度
        /// </summary>
        private Int32 width;

        /// <summary>
        /// 随机字符
        /// </summary>
        private String randomTextChars;

        /// <summary>
        /// 随机文本的长度
        /// </summary>
        private Int32 randomTextLength;

        #endregion

        #region "公共属性"

        /// <summary>
        /// 返回标志此验证码串的guid值
        /// </summary>
        public String UniqueId
        {
            get;
            private set;
        }

        /// <summary>
        /// 返回
        /// </summary>
        public DateTime RenderedAt
        {
            get;
            private set;
        }

        /// <summary>
        /// 字体
        /// </summary>
        public String Font
        {
            get { return fontFamilyName; }
            set
            {
                if (MyFont.IsLegalFontFamilyName(value))
                    fontFamilyName = value;
                else
                    fontFamilyName = FontFamily.GenericSansSerif.Name;
            }
        }

        /// <summary>
        /// 字体的弯曲变形程度
        /// </summary>
        public FontWarpFactor FontWarp
        {
            get;
            set;
        }

        /// <summary>
        /// 背景噪音
        /// </summary>
        public BackgroundNoiseLevel BackgroundNoise
        {
            get;
            set;
        }

        /// <summary>
        /// 线条噪音
        /// </summary>
        public LineNoiseLevel LineNoise
        {
            get;
            set;
        }

        /// <summary>
        /// 用于生成验证码的字符串；生成的验证码会从此字符串中获取字符来生成随机串的
        /// </summary>
        public String TextChars
        {
            get { return randomTextChars; }
            set
            {
                randomTextChars = value;
                Text = MyRand.GetRandomChars(randomTextChars, randomTextLength);
            }
        }

        /// <summary>
        /// 验证码的文本内容
        /// </summary>
        public String Text
        {
            get;
            private set;
        }

        /// <summary>
        /// 验证码的长度，字符的个数
        /// </summary>
        public Int32 TextLength
        {
            get { return randomTextLength; }
            set 
            {
                randomTextLength = value;
                Text = MyRand.GetRandomChars(randomTextChars, randomTextLength);
            }
        }

        /// <summary>
        /// 验证码图片的宽度
        /// </summary>
        public Int32 Width
        {
            get { return width; }
            set
            {
                if (value < 60)
                    throw new ArgumentOutOfRangeException("width", value,
                                        String.Format(Properties.Resources.ExceptionWidthIllegal, "60"));
                width = value;
            }
        }

        /// <summary>
        /// 验证码图片的高度
        /// </summary>
        public Int32 Height
        {
            get { return height; }
            set
            {
                if (value < 20)
                    throw new ArgumentOutOfRangeException("height", value,
                                        String.Format(Properties.Resources.ExceptionHeightIllegal, "20"));
                height = value;
            }
        }

        /// <summary>
        /// 字体类型数组
        /// </summary>
        public String[] DefaultFontFamilys
        {
            get;
            set;
        }

        /// <summary>
        /// 验证码图片的背景颜色
        /// </summary>
        public Color BackColor
        {
            get;
            set;
        }

        /// <summary>
        /// 验证码字体的颜色
        /// </summary>
        public Color FontColor
        {
            get;
            set;
        }

        /// <summary>
        /// 噪音点的颜色
        /// </summary>
        public Color NoiseColor
        {
            get;
            set;
        }

        /// <summary>
        /// 噪音线条的颜色
        /// </summary>
        public Color LineColor
        {
            get;
            set;
        }

        #endregion

        #region "公共方法"

        public ValidateString()
        {
            LineColor = Color.Black;
            NoiseColor = Color.Black;
            FontColor = Color.Black;
            BackColor = Color.White;
            rand = new Random();
            FontWarp = FontWarpFactor.Low;
            BackgroundNoise = BackgroundNoiseLevel.Low;
            LineNoise = LineNoiseLevel.Low;
            width = 120;
            height = 48;
            randomTextLength = 5;
            randomTextChars = "ABCDEFGHJKLNPQRTUVXYZ23465789";
            fontFamilyName = "";
            DefaultFontFamilys = MyString.SplitEx("arial;arial black;comic sans ms;courier new;estrangelo edessa;franklin gothic medium;" +
                                 "georgia;lucida console;lucida sans unicode;managal;microsoft sans serif;palatino linotype;" +
                                 "sylfaen;tahoma;times new roman;trebuchet ms;verdana", ";", StringSplitOptions.None);
            Text = MyRand.GetRandomChars(randomTextChars, randomTextLength);
            RenderedAt = DateTime.Now;
            UniqueId = MyGuid.To_N(Guid.NewGuid());
        }

        /// <summary>
        /// 根据当前的配置信息生成验证码图片
        /// </summary>
        /// <returns></returns>
        public Bitmap RenderImage()
        {
            return DoRenderImage();
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <returns></returns>
        private Bitmap DoRenderImage()
        {
            Font fnt;
            Rectangle rect;
            Brush br;
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                rect = new Rectangle(0, 0, width, height);
                using (br = new SolidBrush(BackColor))
                {
                    gr.FillRectangle(br, rect);
                }
                Int32 charOffset = 0;
                Double charWidth = width / randomTextLength;
                Rectangle rectChar;
                using (br = new SolidBrush(FontColor))
                {
                    foreach (Char c in Text)
                    {
                        using (fnt = GetFont())
                        {
                            rectChar = new Rectangle(Convert.ToInt32(charOffset * charWidth), 0, Convert.ToInt32(charWidth), height);
                            //变形字体
                            using (GraphicsPath gp = TextPath(c.ToString(), fnt, rectChar))
                            {
                                WarpText(gp, rectChar);
                                gr.FillPath(br, gp);
                            }
                        }
                        charOffset += 1;
                    }
                }
                AddNoise(gr, rect);
                AddLine(gr, rect);
            }
            return bmp;
        }

        /// <summary>
        /// 根据枚举值添加
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="rect"></param>
        private void AddNoise(Graphics graphics, Rectangle rect)
        {
            Int32 density = 0;
            Int32 size = 0;
            switch (this.BackgroundNoise)
            {
                case BackgroundNoiseLevel.Node:
                    return;
                case BackgroundNoiseLevel.Low:
                    density = 30;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.Medium:
                    density = 18;
                    size = 40;
                    break;
                case BackgroundNoiseLevel.High:
                    density = 16;
                    size = 39;
                    break;
                case BackgroundNoiseLevel.Extreme:
                    density = 12;
                    size = 38;
                    break;
            }
            using (var br = new SolidBrush(this.NoiseColor))
            {
                Int32 max = Convert.ToInt32(Math.Max(rect.Width, rect.Height) / size);
                for (Int32 i = 0; i <= Convert.ToInt32((rect.Width * rect.Height) / density); i++)
                    graphics.FillEllipse(br, rand.Next(rect.Width), rand.Next(rect.Height), rand.Next(max), rand.Next(max));
            }
        }

        /// <summary>
        /// 根据指定的枚举程度添加曲线干扰
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="rect"></param>
        private void AddLine(Graphics graphics, Rectangle rect)
        {
            Int32 length = 0;
            Single width = 1.0f;
            Int32 lineCount = 0;
            switch (this.LineNoise)
            {
                case LineNoiseLevel.None:
                    return;
                case  LineNoiseLevel.Low:
                    length = 4;
                    width = Convert.ToSingle(height / 31.25);
                    lineCount = 1;
                    break;
                case LineNoiseLevel.Medium:
                    length = 5;
                    width = Convert.ToSingle(height / 27.7777);
                    lineCount = 1;
                    break;
                case LineNoiseLevel.High:
                    length = 3;
                    width = Convert.ToSingle(height / 25);
                    lineCount = 2;
                    break;
                case LineNoiseLevel.Extreme:
                    length = 3;
                    width = Convert.ToSingle(height / 22.7272);
                    lineCount = 3;
                    break;
            }
            var pf = new PointF[length + 1];
            using (var p = new Pen(this.LineColor, width))
            {
                for (Int32 j = 1; j <= lineCount; j++)
                {
                    for (Int32 i = 0; i <= length; i++)
                        pf[i] = RandomPoint(rect);
                    graphics.DrawCurve(p, pf, 1.75f);
                }
            }
        }

        /// <summary>
        /// 根据指定的枚举值对提供的字符串图形路径进行弯曲变形
        /// </summary>
        /// <param name="textPath"></param>
        /// <param name="rect"></param>
        private void WarpText(GraphicsPath textPath, Rectangle rect)
        {
            Single warpDivisor = 1.0f;
            Single rangeModifier = 1.0f;
            switch (this.FontWarp)
            {
                case FontWarpFactor.None:
                    return;
                case FontWarpFactor.Low:
                    warpDivisor = 6f;
                    rangeModifier = 1f;
                    break;
                case FontWarpFactor.Medium:
                    warpDivisor = 5f;
                    rangeModifier = 1.3f;
                    break;
                case FontWarpFactor.High:
                    warpDivisor = 4.5f;
                    rangeModifier = 1.4f;
                    break;
                case FontWarpFactor.Extreme:
                    warpDivisor = 4f;
                    rangeModifier = 1.5f;
                    break;
            }
            var rectF = new RectangleF(Convert.ToSingle(rect.Left), 0, Convert.ToSingle(rect.Width), rect.Height);
            Int32 hrange = Convert.ToInt32(rect.Height / warpDivisor);
            Int32 wrange = Convert.ToInt32(rect.Width / warpDivisor);
            Int32 left = rect.Left - Convert.ToInt32(wrange * rangeModifier);
            Int32 top = rect.Top - Convert.ToInt32(hrange * rangeModifier);
            Int32 width = rect.Left + rect.Width + Convert.ToInt32(wrange * rangeModifier);
            Int32 height = rect.Top + rect.Height + Convert.ToInt32(hrange * rangeModifier);
            if (left < 0) left = 0;
            if (top < 0) top = 0;
            if (width > this.Width) width = this.Width;
            if (height > this.Height) height = this.Height;
            PointF leftTop = RandomPoint(left, left + wrange, top, top + hrange);
            PointF rightTop = RandomPoint(width - wrange, width, top, top + hrange);
            PointF leftBottom = RandomPoint(left, left + wrange, height - hrange, height);
            PointF righBottom = RandomPoint(width - wrange, width, height - hrange, height);
            var points = new[] { leftTop, rightTop, leftBottom, righBottom };
            var m = new Matrix();
            m.Translate(0, 0);
            textPath.Warp(points, rectF, m, WarpMode.Perspective, 0);
        }

        /// <summary>
        /// 在指定的x和y坐标范围内，创建一个随机点
        /// </summary>
        /// <param name="xmin"></param>
        /// <param name="xmax"></param>
        /// <param name="ymin"></param>
        /// <param name="ymax"></param>
        /// <returns></returns>
        private PointF RandomPoint(Int32 xmin, Int32 xmax, Int32 ymin, Int32 ymax)
        {
            return new PointF(rand.Next(xmin, xmax), rand.Next(ymin, ymax));
        }

        /// <summary>
        /// 在指定的矩形范围内创建一个随机点
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private PointF RandomPoint(Rectangle rect)
        {
            return RandomPoint(rect.Left, rect.Width, rect.Top, rect.Bottom);
        }

        /// <summary>
        /// 返回一个包含指定字符串和字体的图形路径
        /// </summary>
        /// <param name="s"></param>
        /// <param name="f"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private GraphicsPath TextPath(String s, Font f, Rectangle r)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Near;
            sf.LineAlignment = StringAlignment.Near;
            var gp = new GraphicsPath();
            gp.AddString(s, f.FontFamily, (Int32)f.Style, f.Size, r, sf);
            return gp;
        }

        /// <summary>
        /// 获取合适大小的验证码的字体
        /// </summary>
        /// <returns></returns>
        private Font GetFont()
        {
            Single size = 0.0f;
            String name = fontFamilyName;
            if (String.IsNullOrEmpty(fontFamilyName))
            {
                name = MyList.GetRandomItem<String>(DefaultFontFamilys);
            }
            switch (FontWarp)
            {
                case FontWarpFactor.None:
                    size = Convert.ToInt32(height * 0.7);
                    break;
                case FontWarpFactor.Low:
                    size = Convert.ToInt32(height * 0.8);
                    break;
                case FontWarpFactor.Medium:
                    size = Convert.ToInt32(height * 0.85);
                    break;
                case FontWarpFactor.High:
                    size = Convert.ToInt32(height * 0.9);
                    break;
                case FontWarpFactor.Extreme:
                    size = Convert.ToInt32(height * 0.95);
                    break;
            }
            return new Font(name, size, FontStyle.Bold);
        }

        #endregion
    }
}
