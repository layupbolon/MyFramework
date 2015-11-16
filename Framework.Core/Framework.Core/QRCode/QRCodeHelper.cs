using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Framework.Core.QRCode
{
    /// <summary>
    /// QRCode二维码辅助类
    /// </summary>
    public class QRCodeHelper
    {
        protected const int ModuleSizeInPixels = 5;
        protected const int PaddingSizeInPixels = 2;
        protected const float InlineSizeInProportion = 0.2f;

        /// <summary>
        /// 根据文字内容等信息构建相应QRCode格式二维码图片
        /// </summary>
        /// <param name="content">二维码内容信息</param>
        /// <returns></returns>
        public static Image BuildQRCode(String content)
        {
            return BuildQRCode(content, -1, -1,null);
        }

        /// <summary>
        /// 根据文字内容等信息构建相应QRCode格式二维码图片
        /// </summary>
        /// <param name="content">二维码内容信息</param>
        /// <param name="inline">内嵌图片，如为Null则不做内嵌</param>
        /// <returns></returns>
        public static Image BuildQRCode(String content, Image inline)
        {
            return BuildQRCode(content, -1, -1, inline);
        }

        /// <summary>
        /// 根据文字内容等信息构建相应QRCode格式二维码图片
        /// </summary>
        /// <param name="content">二维码内容信息</param>
        /// <param name="width">图片宽度，如小于0则使用默认尺寸</param>
        /// <param name="height">图片高度，如小于0则使用默认尺寸</param>
        /// <returns></returns>
        public static Image BuildQRCode(String content, int width, int height)
        {
            return BuildQRCode(content, width, height, null);
        }

        /// <summary>
        /// 根据文字内容等信息构建相应QRCode格式二维码图片
        /// </summary>
        /// <param name="content">二维码内容信息</param>
        /// <param name="width">图片宽度，如小于0则使用默认尺寸</param>
        /// <param name="height">图片高度，如小于0则使用默认尺寸</param>
        /// <param name="inline">内嵌图片，如为Null则不做内嵌</param>
        /// <returns></returns>
        public static Image BuildQRCode(String content, int width, int height, Image inline)
        {
            return BuildQRCode(content, ErrorCorrectionLevel.M, width, height, inline);
        }

        /// <summary>
        /// 根据文字内容等信息构建相应QRCode格式二维码图片
        /// </summary>
        /// <param name="content">二维码内容信息</param>
        /// <param name="level">纠错级别</param>
        /// <param name="width">图片宽度，如小于0则使用默认尺寸</param>
        /// <param name="height">图片高度，如小于0则使用默认尺寸</param>
        /// <param name="inline">内嵌图片，如为Null则不做内嵌</param>
        /// <returns>二维码图片</returns>
        public static Image BuildQRCode(String content, ErrorCorrectionLevel level, int width, int height, Image inline)
        {

            QrEncoder encoder = new QrEncoder(level);
            QrCode code = null;
            if (!encoder.TryEncode(content, out code))
                return null;


            GraphicsRenderer render = new GraphicsRenderer(new FixedModuleSize(ModuleSizeInPixels, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            DrawingSize cSize = render.SizeCalculator.GetSize(code.Matrix.Width);
            Size oSize = new Size(cSize.CodeWidth, cSize.CodeWidth) + new Size(2 * PaddingSizeInPixels, 2 * PaddingSizeInPixels);

            Image img = new Bitmap(oSize.Width, oSize.Height);

            using (Graphics g = Graphics.FromImage(img))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                render.Draw(g, code.Matrix);
                if (inline != null)
                {
                    int iw = (int)(oSize.Width * InlineSizeInProportion);
                    int ih = (int)(oSize.Height * InlineSizeInProportion);
                    int il = (oSize.Width - iw) / 2;
                    int it = (oSize.Height - ih) / 2;
                    g.DrawImage(inline, it, il, iw, ih);
                    Pen pen = new Pen(Color.White, 1);
                    using (GraphicsPath path = CreateRoundedRectanglePath(new Rectangle(it - 1, il - 1, iw + 1, ih + 1), 4))
                    {
                        g.DrawPath(pen, path);
                    }

                }
            }

            if (width > 0 && height > 0)
            {
                int w = width > 0 ? width : code.Matrix.Width;
                int h = height > 0 ? height : code.Matrix.Height;
                Image imgCode = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(imgCode))
                {
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, 0, 0, width, height);
                }

                img.Dispose();
                img = imgCode;

            }


            return img;
        }

        /// <summary>
        /// 创建圆角矩形图形路径
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="cornerRadius">圆角角度</param>
        /// <returns></returns>
        internal static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        } 

    }
}
