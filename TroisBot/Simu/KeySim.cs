using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TroisBot.Simu
{
    class KeySim
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint uMsg, int wParam, int lParam);



        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const uint WM_MOUSEMOVE = 0x0200;
        private const uint WM_RBUTTONDOWN = 0x0204;
        private const uint WM_RBUTTONUP = 0x0205;

        private IntPtr handle;

        public KeySim()
        {
            handle = FindWindow(null, "World of Warcraft");
        }

        public void KeyDown(int key)
        {
            SendMessage(handle, WM_KEYDOWN, key, 0);
        }
        public void KeyUp(int key)
        {

            SendMessage(handle, WM_KEYUP, key, 0);
        }
    }
}
