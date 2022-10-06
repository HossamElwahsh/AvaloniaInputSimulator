﻿using System;
using WindowsInput.Native;

namespace WindowsInput
{
    /// <summary>
    /// The contract for a service that interprets the state of input devices.
    /// </summary>
    public interface IInputDeviceStateAdaptor
    {
        /// <summary>
        /// Determines whether the specified key is up or down.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is down; otherwise, <c>false</c>.
        /// </returns>
        bool IsKeyDown(VirtualKeyCode keyCode);

        /// <summary>
        /// Determines whether the specified key is up or down.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is up; otherwise, <c>false</c>.
        /// </returns>
        bool IsKeyUp(VirtualKeyCode keyCode);

        /// <summary>
        /// get current focus window hWnd Handle pointer
        /// </summary>
        /// <returns></returns>
        public IntPtr? WhichWindow();


        /// <summary>
        /// gets windows title
        /// </summary>
        /// <returns></returns>
        public string GetActiveWindowTitle();
        
        
        /// <summary>
        /// Set Focus to given hWnd Pointer Handle
        /// </summary>
        /// <param name="hWnd">Window pointer handle to focus on</param>
        /// <returns>
        /// - null: if focus failed <br/>
        /// - IntPtr: previous focused pointer if success </returns>
        public IntPtr? SetFocus(IntPtr hWnd);
        
        /// <summary>
        /// gets windows title
        /// </summary>
        /// <returns></returns>
        public string GetActiveWindowTitle(IntPtr hWnd);
        
        
        /// <summary>
        /// gets current focused window process file name
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetActiveWindowFileName();
        
        
        /// <summary>
        /// gets windows process file name for a specific hWnd IntPtr window handle
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetActiveWindowFileName(IntPtr hWnd);

        
        /// <summary>
        /// Determines whether the physical key is up or down at the time the function is called regardless of whether the application thread has read the keyboard event from the message pump.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is down; otherwise, <c>false</c>.
        /// </returns>
        bool IsHardwareKeyDown(VirtualKeyCode keyCode);

        /// <summary>
        /// Determines whether the physical key is up or down at the time the function is called regardless of whether the application thread has read the keyboard event from the message pump.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the key is up; otherwise, <c>false</c>.
        /// </returns>
        bool IsHardwareKeyUp(VirtualKeyCode keyCode);

        /// <summary>
        /// Determines whether the toggling key is toggled on (in-effect) or not.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
        /// <returns>
        /// 	<c>true</c> if the toggling key is toggled on (in-effect); otherwise, <c>false</c>.
        /// </returns>
        bool IsTogglingKeyInEffect(VirtualKeyCode keyCode);
    }
}