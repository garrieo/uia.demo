using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Windows.Shell;
using System.Windows;

namespace uia_test_console
{
    class RoundedCornersTest
    {

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public Rectangle ToRectangle()
            {
                return Rectangle.FromLTRB(Left, Top, Right, Bottom);
            }
        }
        //[DllImport("dwmapi.dll")]
        //static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out Rect pvAttribute, int cbAttribute);
      
        [DllImport("dwmapi.dll", ExactSpelling = true, PreserveSig = true)]
        public static extern uint DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbsize);
        [DllImport("user32.dll")]
        static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);



        [Flags]
        public enum DwmWindowAttribute : uint
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_PASSIVE_UPDATE_MODE,
            DWMWA_USE_HOSTBACKDROPBRUSH,
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,         // [set] BOOL, Allows a window to either use the accent color, or dark, according to the user Color Mode preferences.
            DWMWA_WINDOW_CORNER_PREFERENCE = 33,
            DWMWA_BORDER_COLOR,
            DWMWA_CAPTION_COLOR,
            DWMWA_TEXT_COLOR,
            DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
            DWMWA_LAST
        }

        public static Rectangle GetWindowRectangle(IntPtr handle)
        {

            int cloaked = -1;

            int sizeOfff = Marshal.SizeOf(typeof(int));
            uint res = DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.DWMWA_WINDOW_CORNER_PREFERENCE, ref cloaked , sizeOfff);

            IntPtr hrgn = CreateRectRgn(0, 0, 0, 9999);
            int regionType = GetWindowRgn(handle, hrgn);

            Rectangle rected = Rectangle.Empty;

            Rect rect = new Rect();

            int size = Marshal.SizeOf(typeof(Rect));

        //    int res2 = DwmGetWindowAttribute(handle, 99, out rect, size);
           // int res = DwmGetWindowAttribute(handle, (int)DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS, out rect, size);

            // Debug.WriteLine(res.ToString("x") + " " + size + " " + handle + " " + (int)DwmWindowAttribute.DWMWA_EXTENDED_FRAME_BOUNDS);

            // allow returning of desktop and aero windows
            if (rected.Width == 0)
            {
                //   GetWindowRect(handle, out rect);
                rected = rect.ToRectangle();
                // Debug.WriteLine("Using GetWindowRect");
            }


            //Debug.WriteLine(rected.ToString());
            return rected;
        }
    }
}
