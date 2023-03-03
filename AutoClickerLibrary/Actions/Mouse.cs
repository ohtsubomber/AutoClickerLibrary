using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoClicker.Actions
{
    public static class Mouse
    {
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void SetCursorPos(int X, int Y);

        [DllImport("User32.dll")]
        private static extern bool GetCursorPos(out POINT lppoint);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        private const int MOUSEEVENTF_MIDDLEUP = 0x40;

        [DllImportAttribute("gdi32.dll")]
        private static extern int BitBlt
        (
            IntPtr hdcDest,     // handle to destination DC (device context)
            int nXDest,         // x-coord of destination upper-left corner
            int nYDest,         // y-coord of destination upper-left corner
            int nWidth,         // width of destination rectangle
            int nHeight,        // height of destination rectangle
            IntPtr hdcSrc,      // handle to source DC
            int nXSrc,          // x-coordinate of source upper-left corner
            int nYSrc,          // y-coordinate of source upper-left corner
            System.Int32 dwRop  // raster operation code
        );

        public enum ClickTypes
        {
            LEFT = 0x1,
            RIGHT = 0x2,
            DOUBLE = 0x4
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X { get; set; }
            public int Y { get; set; }
            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }


        public static async Task SetPosAsync(int X, int Y)
        {
            await Task.Run(() => { SetCursorPos(X, Y); });
        }

        public static Point GetPos()
        {
            var pt = new POINT();
            GetCursorPos(out pt);
            return pt;
        }

        public static Color GetPixelColor(this Point pt)
        {
            var screen = Actions.Display.GetScreenShot();
            using (Graphics gdest = Graphics.FromImage(screen))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, pt.X, pt.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            return screen.GetPixel(0, 0);
        }

        public static async Task ClickAsync()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static async Task DoubleClickAsync()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            await Task.Delay(200);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static async Task RightClickAsync()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static async Task MiddleClickAsync()
        {
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, 0);
            await Task.Delay(100);
            mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, 0);
        }

        public static async Task MoveToImage(string imagePath,int? xOffset=null,int? yOffset = null)
        {
            (var targetPosition, var targetSize) = await Actions.Display.GetImagePosAsync(imagePath);
            if (xOffset == null || yOffset == null)
            {
                xOffset = targetSize.Width / 2;
                yOffset = targetSize.Height / 2;
            }
            int x = targetPosition.X + (int)xOffset;
            int y = targetPosition.Y + (int)yOffset;
            await SetPosAsync(x, y);
        }
        public static async Task ClickImage(string imagePath, ClickTypes clickType, int? xOffset=null, int? yOffset=null)
        {
            await MoveToImage(imagePath,xOffset,yOffset);
            await Task.Delay(200);
            switch (clickType)
            {
                case ClickTypes.LEFT:
                    await ClickAsync();
                    break;
                case ClickTypes.RIGHT:
                    await RightClickAsync();
                    break;
                case ClickTypes.DOUBLE:
                    await DoubleClickAsync();
                    break;
            }
        }
    }
}
