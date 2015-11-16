using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Drawing
{
    /// <summary>
    /// 图片大小规格
    /// </summary>
    [Serializable]
    public struct ImageSize
    {
        /// <summary>
        /// 商品图片大小规格80*60
        /// </summary>
        public static readonly ImageSize P80;
        /// <summary>
        /// 商品图片大小规格120*90
        /// </summary>
        public static readonly ImageSize P120;
        /// <summary>
        /// 商品图片大小规格240*180
        /// </summary>
        public static readonly ImageSize P240;
        /// <summary>
        /// 商品图片大小规格800*600
        /// </summary>
        public static readonly ImageSize P800;
        /// <summary>
        /// 商品图片大小规格1200*900
        /// </summary>
        public static readonly ImageSize P1200;
        /// <summary>
        /// 商品图片大小规格1600*1200
        /// </summary>
        public static readonly ImageSize P1600;
        /// <summary>
        /// 图片原始大小规格
        /// </summary>
        public static readonly ImageSize Original;
        /// <summary>
        /// 图片大小规格空值
        /// </summary>
        public static readonly ImageSize Empty;

        static ImageSize()
        {
            P80 = new ImageSize() { Width = 80, Height = 60, IsOriginal = false };
            P120 = new ImageSize() { Width = 120, Height = 90, IsOriginal = false };
            P240 = new ImageSize() { Width = 240, Height = 180, IsOriginal = false };
            P800 = new ImageSize() { Width = 800, Height = 600, IsOriginal = false };
            P1200 = new ImageSize() { Width = 1200, Height = 900, IsOriginal = false };
            P1600 = new ImageSize() { Width = 1600, Height = 1200, IsOriginal = false };
            Original = new ImageSize() { Width = 0, Height = 0, IsOriginal = true };
            Empty = new ImageSize() { Width = 0, Height = 0, IsOriginal = false };
        }

        public ImageSize(int w, int h)
        {
            m_IsZoom = true;
            m_IsOriginal = false;
            m_Width = w;
            m_Height = h;
        }

        public ImageSize(Size size)
        {
            m_IsZoom = true;
            m_IsOriginal = false;
            m_Width = size.Width;
            m_Height = size.Height;
        }

        #region Width
        private bool m_IsZoom;
        /// <summary>
        /// 是否等比
        /// </summary>
        public bool IsZoom
        {
            get { return m_IsZoom; }
            set { m_IsZoom = value; }
        }
        #endregion

        #region Width
        private int m_Width;
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return m_Width; }
            set { m_Width = value; }
        }
        #endregion

        #region Height
        private int m_Height;
        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return m_Height; }
            set { m_Height = value; }
        }
        #endregion

        #region IsOriginal
        private bool m_IsOriginal;
        /// <summary>
        /// 是否原始图片大小
        /// </summary>
        public bool IsOriginal
        {
            get { return m_IsOriginal; }
            set { m_IsOriginal = value; }
        }
        #endregion

        #region IsEmpty
        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return ((!this.m_IsOriginal) && (this.m_Width == 0) && (this.m_Height == 0));
            }
        }
        #endregion

        #region ToString
        public override string ToString()
        {

            if (this.IsOriginal)
                return "Original";

            if (this == P80)
                return "P80";

            if (this == P120)
                return "P120";

            if (this == P240)
                return "P240";

            if (this == P800)
                return "P800";

            if (this == P1200)
                return "P1200";

            if (this == P1600)
                return "P1600";

            return string.Empty;
        }
        #endregion

        #region Parse
        public static ImageSize Parse(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Empty;

            switch (name.ToUpper())
            {
                case "P80":
                    return P80;
                case "P120":
                    return P120;
                case "P240":
                    return P240;
                case "P800":
                    return P800;
                case "P1200":
                    return P1200;
                case "P1600":
                    return P1600;
                case "ORIGINAL":
                    return Original;
                default:
                    return Empty;
            }
        }
        #endregion

        #region Equals
        public override bool Equals(object obj)
        {
            if (!(obj is ImageSize))
            {
                return false;
            }
            ImageSize size = (ImageSize)obj;
            return ((size.Width == this.Width) && (size.Height == this.Height));
        }
        #endregion

        #region GetHashCode
        public override int GetHashCode()
        {
            return (this.m_Width ^ this.m_Width);
        }
        #endregion

        #region Operator overloading
        public static bool operator ==(ImageSize sz1, ImageSize sz2)
        {
            return sz1.IsZoom && sz2.IsZoom
                ? (sz1.Width == sz2.Width) && (sz1.Height == sz2.Height) && (sz1.IsOriginal == sz2.IsOriginal)
                : (sz1.Width == sz2.Width) && (sz1.IsOriginal == sz2.IsOriginal);
        }

        public static bool operator !=(ImageSize sz1, ImageSize sz2)
        {
            return !(sz1 == sz2);
        }

        public static implicit operator Size(ImageSize p)
        {
            return new Size(p.Width, p.Height);
        }
        #endregion


    }
}
