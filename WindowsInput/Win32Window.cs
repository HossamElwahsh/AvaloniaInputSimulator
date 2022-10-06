using System;
using System.IO;

namespace WindowsInput
{
    /// <summary>
    /// Win32Window Model
    /// </summary>
    public class Win32Window
    {
        /// <summary>
        /// Window hWnd Handle
        /// </summary>
        public IntPtr hWnd { get; set; }
        /// <summary>
        /// Window Title Name
        /// </summary>
        public string WindowName { get; set; } = "";
        /// <summary>
        /// Exe Full Path
        /// </summary>
        public string ExePath { get; set; }

        /// <summary>
        /// All lower case exe name for Win32 Process
        /// </summary>
        public string ExeName
        {
            get => Path.GetFileName(ExePath).ToLower();
            set { ExeName = value; }
        }

        /// <summary>
        /// Process ID AKA PID in windows task manager
        /// </summary>
        public int ProcessId { get; set; }
        /// <summary>
        /// Current Thread ID
        /// </summary>
        public int ThreadId { get; set; }
        /// <summary>
        /// true if this model is initiated with data
        /// </summary>
        public bool Retreived { get; set; }
        // public string nextWindow { get; set; } = "";
    }
}