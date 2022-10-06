using System;

namespace WindowsInput
{
    public class Win32Window
    {
        public IntPtr hWnd { get; set; }
        public string WindowName { get; set; } = "";
        public string ExePath { get; set; }
        public int ProcessId { get; set; }
        public int ThreadId { get; set; }
        public bool Retreived { get; set; } = false;
        // public string nextWindow { get; set; } = "";
    }
}