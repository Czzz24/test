using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace EF.Component.Tools
{
    public class ImageHelper
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 在图片上增加文字水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_sy">生成的带文字水印的图片路径</param>
        public static void AddWater(string Path, string Path_sy, string addText)
        {
            Image image = Image.FromFile(Path);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(image, 0, 0, image.Width, image.Height);
            Font f = new Font("Arial", 12);
            Brush b = new SolidBrush(Color.Blue);

            g.DrawString(addText, f, b, 35, 35);
            g.Dispose();

            image.Save(Path_sy);
            image.Dispose();
        }

        /// <summary>
        /// 在图片上生成图片水印
        /// </summary>
        /// <param name="Path">原服务器图片路径</param>
        /// <param name="Path_syp">生成的带图片水印的图片路径</param>
        /// <param name="Path_sypf">水印图片路径</param>
        public static void AddWaterPic(string Path, string Path_syp, string Path_sypf)
        {
            Image image = Image.FromFile(Path);
            Image copyImage = Image.FromFile(Path_sypf);
            Graphics g = Graphics.FromImage(image);
            g.DrawImage(copyImage, new Rectangle(image.Width - copyImage.Width, image.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            g.Dispose();

            image.Save(Path_syp);
            image.Dispose();
        }

        /// <summary>
        /// 二进制转图片
        /// </summary>
        /// <param name="streamByte">二进制流</param>
        /// <returns></returns>
        public static Image BinaryToImg(byte[] streamByte)
        {
            MemoryStream ms = new MemoryStream(streamByte);
            Image img = Image.FromStream(ms);
            return img;
        }

        /// <summary>
        /// 图片转二进制
        /// </summary>
        /// <param name="FilePath">图片路径</param>
        /// <returns>返回二进制数据</returns>
        public static byte[] ImgToBinary(string FilePath)
        {
            FileStream fs = new FileStream(FilePath, FileMode.Open);//可以是其他重载方法
            byte[] byData = new byte[fs.Length];
            fs.Read(byData, 0, byData.Length);
            fs.Close();
            return byData;
        }

        /// <summary>
        /// 图片对象转二进制
        /// </summary>
        /// <param name="imgPhoto">图片对象</param>
        /// <returns>返回二进制数据</returns>
        public static byte[] ImgToBinary(Image imgPhoto)
        {
            //将Image转换成流数据，并保存为byte[]
            MemoryStream mstream = new MemoryStream();
            imgPhoto.Save(mstream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byData = new byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length);
            mstream.Close();
            return byData;
        }

        /// <summary>
        /// 字符串转二进制
        /// </summary>
        /// <param name="ImgStr">图片字符串</param>
        /// <returns></returns>
        public static byte[] StrToBinary(string ImgStr)
        {
            byte[] imageBytes = Convert.FromBase64String(ImgStr);
            return imageBytes;
        }

        /// <summary>
        /// byte流转String字符串
        /// </summary>
        /// <param name="imageByte">图片字节流</param>
        /// <returns>返回字符串</returns>
        public static string BinaryToStr(byte[] imageByte)
        {
            string picStr = Convert.ToBase64String(imageByte);
            return picStr;
        }

        /// <summary>
        /// 字符串转byte数组带横线
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(string hexString)
        {
            string[] strSplit = hexString.Split('-');
            byte[] returnBytes = new byte[hexString.Length];
            for (int i = 0; i < strSplit.Length; i++)
            {
                returnBytes[i] = byte.Parse(strSplit[i], System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            return returnBytes;
        }

        /// <summary>
        /// 字符串转byte数组不带横线
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToBytes(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = byte.Parse(hexString.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            }
            return returnBytes;
        }

        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="path">图片路径</param><br>    
        public static string decodeImgToBase64(string path)
        {
            try
            {
                Bitmap bmp = new Bitmap(path);
                MemoryStream ms = new MemoryStream();
                var suffix = path.Substring(path.LastIndexOf('.') + 1,
                    path.Length - path.LastIndexOf('.') - 1).ToLower();
                var suffixName = suffix == "png"
                    ? ImageFormat.Png
                    : suffix == "jpg" || suffix == "jpeg"
                        ? ImageFormat.Jpeg
                        : suffix == "bmp"
                            ? ImageFormat.Bmp
                            : suffix == "gif"
                                ? ImageFormat.Gif
                                : ImageFormat.Jpeg;

                bmp.Save(ms, suffixName);
                byte[] arr = new byte[ms.Length]; ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length); ms.Close();
                return "data:image/"+ suffixName + ";base64," + Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        ///<summary>
        /// 获取图片后缀
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public string GetImageExt(Image image)
        {
            string imageExt = "";
            var RawFormatGuid = image.RawFormat.Guid;
            if (ImageFormat.Png.Guid == RawFormatGuid)
            {
                imageExt = "png";
            }
            if (ImageFormat.Jpeg.Guid == RawFormatGuid)
            {
                imageExt = "jpg";
            }
            if (ImageFormat.Bmp.Guid == RawFormatGuid)
            {
                imageExt = "bmp";
            }
            if (ImageFormat.Gif.Guid == RawFormatGuid)
            {
                imageExt = "gif";
            }
            if (ImageFormat.Icon.Guid == RawFormatGuid)
            {
                imageExt = "icon";
            }
            return imageExt;
        }
    }
}
