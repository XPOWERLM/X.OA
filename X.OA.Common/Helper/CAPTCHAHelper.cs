using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace X.OA.Common.Helper
{
    public static class CAPTCHAHelper
    {
        #region Expire
        //public static Bitmap GetCAPTCHA()
        //{
        //    #region expire

        //    //// 创建画布
        //    //Bitmap bmp = new Bitmap(100, 50);

        //    //// 获取绘制工具
        //    //Graphics gp = Graphics.FromImage(bmp);
        //    //gp.Clear(Color.Black);

        //    //string printable = "abcdefghijklmnopqrstuvwxyz1234567890";
        //    //string strCP = "";
        //    //Random r = new Random();


        //    ///*  Notice
        //    // *  strCP += printable[new Random().Next(printable.Length)];
        //    // *  will get same result
        //    // */


        //    //for (int i = 0; i < 4; i++)
        //    //{
        //    //    strCP += printable[r.Next(printable.Length)];
        //    //}


        //    //// Draw CAPTCHA
        //    //gp.DrawString(strCP, new Font(new FontFamily("黑体"), 30F), new SolidBrush(Color.Aqua), new PointF(1, 2));
        //    //str = strCP;
        //    //// Save as OutputStream
        //    //return bmp; 
        //    #endregion


        //} 
        #endregion

        static Random random = new Random();


        /// <summary>
        /// Must Complement System.Web.SessionState.IRequiresSessionState ; Auto clear Session["CAPTCHA"]
        /// </summary>
        public static bool CheckCAPTCHA
        {
            get
            {
                var sessionCAPTCHA = HttpContext.Current.Session["CAPTCHA"]?.ToString() ?? $"Null:{random.Next()}";
                HttpContext.Current.Session["CAPTCHA"] = null;
                return (HttpContext.Current.Request["CAPTCHA"] ?? $"Null:{random.Next()}").Equals(sessionCAPTCHA, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        #region 验证码长度(默认6个验证码的长度)
        static int length = 6;

        #endregion

        #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
        static int fontSize = 40;

        #endregion

        #region 边框补(默认1像素)
        static int padding = 2;

        #endregion

        #region 是否输出燥点(默认不输出)
        static bool chaos = true;

        #endregion

        #region 输出燥点的颜色(默认灰色)
        static Color chaosColor = Color.LightGray;

        #endregion

        #region 自定义背景色(默认白色)
        static Color backgroundColor = Color.White;

        #endregion

        #region 自定义随机颜色数组
        static Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

        #endregion

        #region 自定义字体数组
        static string[] fonts = { "Arial", "Georgia" };

        #endregion

        #region 自定义随机码字符串序列(使用逗号分隔)
        static string codeSerial = "1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,k,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z";
        //string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";

        #endregion

        #region 产生波形滤镜效果

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// 正弦曲线Wave扭曲图片（Edit By 51aspx.com）
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
        /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
        /// <returns></returns>
        public static Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);

            // 将位图背景填充为白色
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // 取得当前点的颜色
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }



        #endregion

        #region 生成校验码图片
        public static Bitmap CreateImageCode(string code)
        {
            int fSize = fontSize;
            int fWidth = fSize + padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + padding * 2;
            int imageHeight = fSize * 2 + padding;

            Bitmap image = new Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(backgroundColor);

            Random rand = new Random();

            //给背景添加随机生成的燥点
            if (chaos)
            {

                Pen pen = new Pen(chaosColor, 0);
                int c = length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - fontSize - padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //随机字体和颜色的验证码字符
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(colors.Length - 1);
                findex = rand.Next(fonts.Length - 1);

                f = new Font(fonts[findex], fSize, FontStyle.Bold);
                b = new SolidBrush(colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //画一个边框 边框颜色为Color.Gainsboro
            g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            g.Dispose();

            //产生波形（Add By 51aspx.com）
            image = TwistImage(image, true, 8, 4);

            return image;
        }

        /// <summary>
        /// Create CAPTCHA
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static byte[] CreateBytesCode(string code)
        {
            using (MemoryStream imgStream = new MemoryStream())
            {
                CreateImageCode(code).Save(imgStream, ImageFormat.Jpeg);
                return imgStream.ToArray();
            }
        }
        #endregion

        #region 将创建好的图片输出到页面
        public static void CreateImageOnPage(string code, HttpContext context)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap image = CreateImageCode(code);

            image.Save(ms, ImageFormat.Jpeg);

            context.Response.ClearContent();
            context.Response.ContentType = "image/Jpeg";
            context.Response.BinaryWrite(ms.GetBuffer());

            ms.Close();
            ms = null;
            image.Dispose();
            image = null;
        }
        #endregion

        #region 生成随机字符码
        /// <summary>
        /// Generate ramdom string
        /// </summary>
        /// <param name="codeLen">Length</param>
        /// <returns></returns>
        public static string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = length;
            }

            string[] arr = codeSerial.Split(',');

            string code = "";

            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);

                code += arr[randValue];
            }

            return code;
        }

        /// <summary>
        /// Generate random string
        /// </summary>
        /// <returns></returns>
        public static string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }

        #endregion

    }
}
