using System;
using System.Runtime.InteropServices;

namespace CustomCapes.Common.Native {

    public static class User32 {
        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetForegroundWindow (IntPtr hWnd);
        
    }

}