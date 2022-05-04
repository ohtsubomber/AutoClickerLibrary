using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AutoClicker.Actions
{
    public static class Display
    {
        public static Bitmap GetScreenShot()
        {
            // プライマリスクリーン全体
            int width = 0;
            int height = 0;
            foreach (var screen in Screen.AllScreens)
            {
                width += screen.Bounds.Width;
                if (height < screen.Bounds.Height)
                {
                    height = screen.Bounds.Height;
                }
            }
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            // 画面全体をコピーする
            graphics.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), bitmap.Size);
            // グラフィックスの解放
            graphics.Dispose();
            return bitmap;
        }

        public static async Task<(System.Drawing.Point, System.Drawing.Size)> GetImagePosAsync(string imagePath, double threshold=0.9)
        {
            return await Task.Run<(System.Drawing.Point, System.Drawing.Size)>(() =>
            {
                var screenshot = GetScreenShot();
                using (var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screenshot))
                {
                    using (var temp = OpenCvSharp.Extensions.BitmapConverter.ToMat(new Bitmap(imagePath)))
                    {
                        using (Mat result = new Mat())
                        {

                            // テンプレートマッチ
                            Cv2.MatchTemplate(mat, temp, result, TemplateMatchModes.CCoeffNormed);

                            // 類似度が最大/最小となる画素の位置を調べる
                            OpenCvSharp.Point minloc, maxloc;
                            double minval, maxval;
                            Cv2.MinMaxLoc(result, out minval, out maxval, out minloc, out maxloc);

                            // しきい値で判断
                            if (maxval >= threshold)
                            {
                                return (new System.Drawing.Point(maxloc.X, maxloc.Y), new System.Drawing.Size(temp.Width, temp.Height));
                            }
                            else
                            {
                                return (new System.Drawing.Point(-1, -1), new System.Drawing.Size(temp.Width, temp.Height));
                            }

                        }
                    }
                }
            });
           
        }
    }
}
