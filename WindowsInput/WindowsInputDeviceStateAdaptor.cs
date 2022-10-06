﻿using System;
using System.Text;
using WindowsInput.Native;

namespace WindowsInput
{
    /// <summary>
    /// An implementation of <see cref="IInputDeviceStateAdaptor"/> for Windows by calling the native <see cref="NativeMethods.GetKeyState"/> and <see cref="NativeMethods.GetAsyncKeyState"/> methods.
    /// </summary>
    public class WindowsInputDeviceStateAdaptor : IInputDeviceStateAdaptor
    {

        /// <summary>
        /// Determines whether the specified key is up or down by calling the GetKeyState function. (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is down; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information. 
        /// An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated. 
        /// To retrieve state information for all the virtual keys, use the GetKeyboardState function. 
        /// An application can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for Bthe nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and right instances of those keys. 
        /// VK_LSHIFT
        /// VK_RSHIFT
        /// VK_LCONTROL
        /// VK_RCONTROL
        /// VK_LMENU
        /// VK_RMENU
        /// 
        /// These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
        /// </remarks>
        public bool IsKeyDown(VirtualKeyCode keyCode)
        {
            Int16 result = NativeMethods.GetKeyState((UInt16)keyCode);
            return (result < 0);
        }
        
        /// <summary>
        /// Determines whether the specified key is up or downby calling the <see cref="NativeMethods.GetKeyState"/> function. (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is up; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information. 
        /// An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated. 
        /// To retrieve state information for all the virtual keys, use the GetKeyboardState function. 
        /// An application can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for Bthe nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and right instances of those keys. 
        /// VK_LSHIFT
        /// VK_RSHIFT
        /// VK_LCONTROL
        /// VK_RCONTROL
        /// VK_LMENU
        /// VK_RMENU
        /// 
        /// These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
        /// </remarks>
        public bool IsKeyUp(VirtualKeyCode keyCode)
        {
            return !IsKeyDown(keyCode);
        }

        /// <summary>
        /// Gets current foreground focused window handle
        /// </summary>
        /// <returns></returns>
        public IntPtr? WhichWindow()
        {
            return NativeMethods.GetForegroundWindow();
        }
        
        /// <summary>
        /// gets windows title
        /// </summary>
        /// <returns></returns>
        public string GetActiveWindowTitle()
        {
            IntPtr? hWnd = WhichWindow();
            if (hWnd is null)
            {
                return "no window";
            }

            int bufSize = 100;
            StringBuilder buffer = new StringBuilder(bufSize);
            Int32 result = NativeMethods.GetWindowTextA((IntPtr)hWnd, buffer, bufSize);
            // if (result) 
            // {
            return buffer.ToString();
            // }                

        }

        /// <summary>
        /// Set Focus to given hWnd Pointer Handle
        /// </summary>
        /// <param name="hWnd">Window pointer handle to focus on</param>
        /// <returns>
        /// - null: if focus failed <br/>
        /// - IntPtr: previous focused pointer if success </returns>
        public IntPtr? SetFocus(IntPtr hWnd)
        {
            NativeMethods.SetForegroundWindow(hWnd);
            return WhichWindow();
        }

        /// <summary>
        /// Retrieves the full name of the executable image for the specified process
        /// </summary>
        /// <param name="hWnd">A handle to the process. </param>
        /// <returns>Window Exe Name.</returns>
        public string GetWindowExeName(IntPtr hWnd)
        {
            int capacity = 1024;
            StringBuilder sb = new StringBuilder(capacity);
            var processId = GetProcessAndThreadId(hWnd)[1];
            IntPtr handle = NativeMethods.OpenProcess((uint)ProcessAccessFlags.QueryLimitedInformation, 
                false, (uint)processId);
            NativeMethods.QueryFullProcessImageName(handle, 0, sb, ref capacity);
            string fullPath = sb.ToString(0, capacity);
            return fullPath;
        }
        /// <summary>
        /// Retrieves the full name of the executable image for the active window process
        /// </summary>
        /// <returns>Window Exe Name.</returns>
        public string GetActiveWindowExeName()
        {
            IntPtr? hWnd = WhichWindow();
            if (hWnd is null)
            {
                return "no window";
            }
         
            int bufSize = 1024;
            StringBuilder buffer = new StringBuilder(bufSize);

            NativeMethods.QueryFullProcessImageName((IntPtr)hWnd, 0, buffer, ref bufSize);
            return buffer.ToString(0, bufSize);
        }
        
        

        /// <summary>
        /// Get Active Win Data as <see cref="Win32Window"/> object
        /// </summary>
        /// <returns></returns>
        public Win32Window GetActiveWindowData()
        {
            IntPtr? hWnd = WhichWindow();
            if (hWnd is null)
            {
                return new Win32Window();
            }
            else
            {
                return GetWindowData((IntPtr)hWnd);
            }
        }
        
        /// <summary>
        ///  Get hWnd Win Data as <see cref="Win32Window"/> object
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public Win32Window GetWindowData(IntPtr hWnd)
        {
            var ids = GetProcessAndThreadId((IntPtr)hWnd);
            IntPtr handle = NativeMethods.OpenProcess((uint)ProcessAccessFlags.QueryLimitedInformation,
                false, (uint)ids[1]);
            
            int capacity = 1024;
            StringBuilder sb = new StringBuilder(capacity);
            NativeMethods.QueryFullProcessImageName(handle, 0, sb, ref capacity);
            var win = new Win32Window()
            {
                hWnd = hWnd,
                Retreived = true,
                WindowName = GetActiveWindowTitle((IntPtr)hWnd),
                ThreadId = ids[0],
                ProcessId = ids[1],
                ExePath = sb.ToString()
            };
            
            return win;                
        }

        private enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }
        
        private int[] GetProcessAndThreadId(IntPtr hWnd)
        {
            uint processId = 0;
            uint threadId = NativeMethods.GetWindowThreadProcessId(hWnd, out processId);
            return new[] {(int)threadId, (int)processId};
        }
        
        /// <summary>
        /// gets windows title for a specific hWnd IntPtr window handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetActiveWindowTitle(IntPtr hWnd)
        {
            int bufSize = 100;
            StringBuilder buffer = new StringBuilder(bufSize);
            Int32 result = NativeMethods.GetWindowTextA((IntPtr)hWnd, buffer, bufSize);
            // if (result) 
            // {
            return buffer.ToString();
        }

        /// <summary>
        /// Determines whether the physical key is up or down at the time the function is called regardless of whether the application thread has read the keyboard event from the message pump by calling the <see cref="NativeMethods.GetAsyncKeyState"/> function. (See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx)
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is down; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The GetAsyncKeyState function works with mouse buttons. However, it checks on the state of the physical mouse buttons, not on the logical mouse buttons that the physical buttons are mapped to. For example, the call GetAsyncKeyState(VK_LBUTTON) always returns the state of the left physical mouse button, regardless of whether it is mapped to the left or right logical mouse button. You can determine the system's current mapping of physical mouse buttons to logical mouse buttons by calling 
        /// Copy CodeGetSystemMetrics(SM_SWAPBUTTON) which returns TRUE if the mouse buttons have been swapped.
        /// 
        /// Although the least significant bit of the return value indicates whether the key has been pressed since the last query, due to the pre-emptive multitasking nature of Windows, another application can call GetAsyncKeyState and receive the "recently pressed" bit instead of your application. The behavior of the least significant bit of the return value is retained strictly for compatibility with 16-bit Windows applications (which are non-preemptive) and should not be relied upon.
        /// 
        /// You can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the vKey parameter. This gives the state of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. 
        /// 
        /// Windows NT/2000/XP: You can use the following virtual-key code constants as values for vKey to distinguish between the left and right instances of those keys. 
        /// 
        /// Code Meaning 
        /// VK_LSHIFT Left-shift key. 
        /// VK_RSHIFT Right-shift key. 
        /// VK_LCONTROL Left-control key. 
        /// VK_RCONTROL Right-control key. 
        /// VK_LMENU Left-menu key. 
        /// VK_RMENU Right-menu key. 
        /// 
        /// These left- and right-distinguishing constants are only available when you call the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
        /// </remarks>
        public bool IsHardwareKeyDown(VirtualKeyCode keyCode)
        {
            var result = NativeMethods.GetAsyncKeyState((UInt16)keyCode);
            return (result < 0);
        }

        /// <summary>
        /// Determines whether the physical key is up or down at the time the function is called regardless of whether the application thread has read the keyboard event from the message pump by calling the <see cref="NativeMethods.GetAsyncKeyState"/> function. (See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx)
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is up; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The GetAsyncKeyState function works with mouse buttons. However, it checks on the state of the physical mouse buttons, not on the logical mouse buttons that the physical buttons are mapped to. For example, the call GetAsyncKeyState(VK_LBUTTON) always returns the state of the left physical mouse button, regardless of whether it is mapped to the left or right logical mouse button. You can determine the system's current mapping of physical mouse buttons to logical mouse buttons by calling 
        /// Copy CodeGetSystemMetrics(SM_SWAPBUTTON) which returns TRUE if the mouse buttons have been swapped.
        /// 
        /// Although the least significant bit of the return value indicates whether the key has been pressed since the last query, due to the pre-emptive multitasking nature of Windows, another application can call GetAsyncKeyState and receive the "recently pressed" bit instead of your application. The behavior of the least significant bit of the return value is retained strictly for compatibility with 16-bit Windows applications (which are non-preemptive) and should not be relied upon.
        /// 
        /// You can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the vKey parameter. This gives the state of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. 
        /// 
        /// Windows NT/2000/XP: You can use the following virtual-key code constants as values for vKey to distinguish between the left and right instances of those keys. 
        /// 
        /// Code Meaning 
        /// VK_LSHIFT Left-shift key. 
        /// VK_RSHIFT Right-shift key. 
        /// VK_LCONTROL Left-control key. 
        /// VK_RCONTROL Right-control key. 
        /// VK_LMENU Left-menu key. 
        /// VK_RMENU Right-menu key. 
        /// 
        /// These left- and right-distinguishing constants are only available when you call the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
        /// </remarks>
        public bool IsHardwareKeyUp(VirtualKeyCode keyCode)
        {
            return !IsHardwareKeyDown(keyCode);
        }

        /// <summary>
        /// Determines whether the toggling key is toggled on (in-effect) or not by calling the <see cref="NativeMethods.GetKeyState"/> function.  (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the toggling key is toggled on (in-effect); otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information. 
        /// An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated. 
        /// To retrieve state information for all the virtual keys, use the GetKeyboardState function. 
        /// An application can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and right instances of those keys. 
        /// VK_LSHIFT
        /// VK_RSHIFT
        /// VK_LCONTROL
        /// VK_RCONTROL
        /// VK_LMENU
        /// VK_RMENU
        /// 
        /// These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
        /// </remarks>
        public bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
        {
            Int16 result = NativeMethods.GetKeyState((UInt16)keyCode);
            return (result & 0x01) == 0x01;
        }
    }
}