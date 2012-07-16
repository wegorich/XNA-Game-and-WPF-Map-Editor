using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace LevelEditor
{
	/// <summary>
	/// Base Dialog API methods and interfaces.
	/// </summary>
	internal class WindowBaseAPI
	{
		#region Constants

		// The WM_GETMINMAXINFO message is sent to a window when the size or
		// position of the window is about to change.
		// An application can use this message to override the window's
		// default maximized size and position, or its default minimum or
		// maximum tracking size.
		private const int WM_GETMINMAXINFO = 0x0024;

		// Returns a handle to the display monitor that is nearest to the window.
		private const int MONITOR_DEFAULTTONEAREST = 2;
		
		// Sets a new window style.
		private const int GWL_STYLE = -16;
		
		// Sets a new extended window style. 
		private const int GWL_EXSTYLE = -20;

		// The window has a sizing border. Same as the WS_SIZEBOX style.
		private const int WS_THICKFRAME = 0x00040000;

		// The window is initially minimized. Same as the WS_ICONIC style.
		private const int WS_MINIMIZE = 0x20000000;

		// The window is initially maximized.
		private const int WS_MAXIMIZE = 0x00100000;

		// The window has a double border; the window can, optionally, be created with a title bar by specifying the WS_CAPTION style in the dwStyle parameter.
		private const int WS_EX_DLGMODALFRAME = 0x00000001;

		// The window has a border with a sunken edge.
		private const int WS_EX_CLIENTEDGE = 0x00000200;

		// The window has a three-dimensional border style intended to be used for items that do not accept user input.
		private const int WS_EX_STATICEDGE = 0x00020000;

		// The window has a thin-line border.
		private const int WS_BORDER = 0x00800000;

		// The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
		private const int WS_DLGFRAME = 0x00400000;

		// The window has a title bar (includes the WS_BORDER style).
		private const int WS_CAPTION = WS_BORDER | WS_DLGFRAME;
		
		// A window receives this message when the user chooses a command from the Window menu 
		// or when the user chooses the maximize button, minimize button, restore button, or close button.
		private const int WM_SYSCOMMAND = 0x112;

		#endregion

		#region Structs
		/// <summary>
		/// Native Windows API-compatible POINT struct
		/// </summary>
		[Serializable, StructLayout(LayoutKind.Sequential)]
		private struct POINT
		{
			public int X;
			public int Y;
		}
		/// <summary>
		/// The RECT structure defines the coordinates of the upper-left
		/// and lower-right corners of a rectangle.
		/// </summary>
		/// <see cref="http://msdn.microsoft.com/en-us/library/dd162897%28VS.85%29.aspx"/>
		/// <remarks>
		/// By convention, the right and bottom edges of the rectangle
		/// are normally considered exclusive.
		/// In other words, the pixel whose coordinates are ( right, bottom )
		/// lies immediately outside of the the rectangle.
		/// For example, when RECT is passed to the FillRect function, the rectangle
		/// is filled up to, but not including,
		/// the right column and bottom row of pixels. This structure is identical
		/// to the RECTL structure.
		/// </remarks>
		[StructLayout(LayoutKind.Sequential, Pack = 0)]
		public struct RECT
		{
			/// <summary>
			/// The x-coordinate of the upper-left corner of the rectangle.
			/// </summary>
			public int Left;

			/// <summary>
			/// The y-coordinate of the upper-left corner of the rectangle.
			/// </summary>
			public int Top;

			/// <summary>
			/// The x-coordinate of the lower-right corner of the rectangle.
			/// </summary>
			public int Right;

			/// <summary>
			/// The y-coordinate of the lower-right corner of the rectangle.
			/// </summary>
			public int Bottom;
		}

		/// <summary>
		/// The MINMAXINFO structure contains information about a window's
		/// maximized size and position and its minimum and maximum tracking size.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/ms632605%28VS.85%29.aspx"/>
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct MINMAXINFO
		{
			/// <summary>
			/// Reserved; do not use.
			/// </summary>
			public POINT Reserved;

			/// <summary>
			/// Specifies the maximized width (POINT.x)
			/// and the maximized height (POINT.y) of the window.
			/// For top-level windows, this value
			/// is based on the width of the primary monitor.
			/// </summary>
			public POINT MaxSize;

			/// <summary>
			/// Specifies the position of the left side
			/// of the maximized window (POINT.x)
			/// and the position of the top
			/// of the maximized window (POINT.y).
			/// For top-level windows, this value is based
			/// on the position of the primary monitor.
			/// </summary>
			public POINT MaxPosition;

			/// <summary>
			/// Specifies the minimum tracking width (POINT.x)
			/// and the minimum tracking height (POINT.y) of the window.
			/// This value can be obtained programmatically
			/// from the system metrics SM_CXMINTRACK and SM_CYMINTRACK.
			/// </summary>
			public POINT MinTrackSize;

			/// <summary>
			/// Specifies the maximum tracking width (POINT.x)
			/// and the maximum tracking height (POINT.y) of the window.
			/// This value is based on the size of the virtual screen
			/// and can be obtained programmatically
			/// from the system metrics SM_CXMAXTRACK and SM_CYMAXTRACK.
			/// </summary>
			public POINT MaxTrackSize;
		}

		/// <summary>
		/// The WINDOWINFO structure contains window information.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/ms632610%28VS.85%29.aspx"/>
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct WINDOWINFO
		{
			/// <summary>
			/// The size of the structure, in bytes.
			/// The caller must set this to sizeof(WINDOWINFO).
			/// </summary>
			public uint Size;

			/// <summary>
			/// Pointer to a RECT structure
			/// that specifies the coordinates of the window.
			/// </summary>
			public RECT Window;

			/// <summary>
			/// Pointer to a RECT structure
			/// that specifies the coordinates of the client area.
			/// </summary>
			public RECT Client;

			/// <summary>
			/// The window styles. For a table of window styles,
			/// <see cref="http://msdn.microsoft.com/en-us/library/ms632680%28VS.85%29.aspx">
			/// CreateWindowEx
			/// </see>.
			/// </summary>
			public uint Style;

			/// <summary>
			/// The extended window styles. For a table of extended window styles,
			/// see CreateWindowEx.
			/// </summary>
			public uint ExStyle;

			/// <summary>
			/// The window status. If this member is WS_ACTIVECAPTION,
			/// the window is active. Otherwise, this member is zero.
			/// </summary>
			public uint WindowStatus;

			/// <summary>
			/// The width of the window border, in pixels.
			/// </summary>
			public uint WindowBordersWidth;

			/// <summary>
			/// The height of the window border, in pixels.
			/// </summary>
			public uint WindowBordersHeight;

			/// <summary>
			/// The window class atom (see
			/// <see cref="http://msdn.microsoft.com/en-us/library/ms633586%28VS.85%29.aspx">
			/// RegisterClass
			/// </see>).
			/// </summary>
			public ushort WindowType;

			/// <summary>
			/// The Windows version of the application that created the window.
			/// </summary>
			public ushort CreatorVersion;
		}

		/// <summary>
		/// The MONITORINFO structure contains information about a display monitor.
		/// The GetMonitorInfo function stores information in a MONITORINFO structure.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/dd145065%28VS.85%29.aspx"/>
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct MONITORINFO
		{
			/// <summary>
			/// The size, in bytes, of the structure. Set this member
			/// to sizeof(MONITORINFO) (40) before calling the GetMonitorInfo function.
			/// Doing so lets the function determine
			/// the type of structure you are passing to it.
			/// </summary>
			public int Size;

			/// <summary>
			/// A RECT structure that specifies the display monitor rectangle,
			/// expressed in virtual-screen coordinates.
			/// Note that if the monitor is not the primary display monitor,
			/// some of the rectangle's coordinates may be negative values.
			/// </summary>
			public RECT Monitor;

			/// <summary>
			/// A RECT structure that specifies the work area rectangle
			/// of the display monitor that can be used by applications,
			/// expressed in virtual-screen coordinates.
			/// Windows uses this rectangle to maximize an application on the monitor.
			/// The rest of the area in rcMonitor contains system windows
			/// such as the task bar and side bars.
			/// Note that if the monitor is not the primary display monitor,
			/// some of the rectangle's coordinates may be negative values.
			/// </summary>
			public RECT WorkArea;

			/// <summary>
			/// The attributes of the display monitor.
			/// This member can be the following value:
			/// 1 : MONITORINFOF_PRIMARY
			/// </summary>
			public uint Flags;
		}

		/// <summary>
		/// The APPBARDATA structure contains information about a system appbar message. 
		/// This structure is used with the SHAppBarMessage function.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb773184(v=vs.85).aspx"/>
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct APPBARDATA
		{
			/// <summary>
			/// The size of the structure, in bytes.
			/// </summary>
			public int cbSize;
			/// <summary>
			/// The handle to the appbar window.
			/// </summary>
			public IntPtr hWnd;
			/// <summary>
			/// An application-defined message identifier. 
			/// The application uses the specified identifier for notification messages 
			/// that it sends to the appbar identified by the hWnd member. 
			/// This member is used when sending the ABM_NEW message.
			/// </summary>
			public int uCallbackMessage;
			/// <summary>
			/// A value that specifies an edge of the screen. 
			/// This member is used when sending the ABM_GETAUTOHIDEBAR, ABM_QUERYPOS,
			/// ABM_SETAUTOHIDEBAR, and ABM_SETPOS messages. This member can be one of the 
			/// following values.
			/// </summary>
			public int uEdge;
			/// <summary>
			/// A RECT structure to contain the bounding rectangle, 
			/// in screen coordinates, of an appbar or the Windows taskbar. 
			/// This member is used when sending the ABM_GETTASKBARPOS, ABM_QUERYPOS, 
			/// and ABM_SETPOS messages.
			/// </summary>
			public RECT rc;
			/// <summary>
			/// A message-dependent value. This member is used with the ABM_SETAUTOHIDEBAR 
			/// and ABM_SETSTATE messages.
			/// </summary>
			public bool lParam;
		}

		/// <summary>
		/// The ABMsg enum contains APPBAR message value to send.
		/// </summary>
		public enum ABMsg
		{
			ABM_NEW = 0,
			ABM_REMOVE = 1,
			ABM_QUERYPOS = 2,
			ABM_SETPOS = 3,
			ABM_GETSTATE = 4,
			ABM_GETTASKBARPOS = 5,
			ABM_ACTIVATE = 6,
			ABM_GETAUTOHIDEBAR = 7,
			ABM_SETAUTOHIDEBAR = 8,
			ABM_WINDOWPOSCHANGED = 9,
			ABM_SETSTATE = 10
		}

		/// <summary>
		/// The ABEdge enum contains value that specifies an position of the TaskBar element on the screen. 
		/// </summary>
		public enum ABEdge
		{
			ABE_LEFT = 0,
			ABE_TOP = 1,
			ABE_RIGHT = 2,
			ABE_BOTTOM = 3
		}

		/// <summary>
		/// The ResizeDirection enum contains value that specifies an direction of the window resizing
		/// </summary>
		public enum ResizeDirection
		{
			LEFT = 1,
			RIGHT = 2,
			TOP = 3,
			TOPLEFT = 4,
			TOPRIGHT = 5,
			BOTTOM = 6,
			BOTTOMLEFT = 7,
			BOTTOMRIGHT = 8,
		}

		#endregion

		#region Imported methods

		/// <summary>
		/// The FindWindow function retrieves a handle to the top-level window whose class name and window name match the specified strings. 
		/// This function does not search child windows. This function does not perform a case-sensitive search.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms633499(v=vs.85).aspx"/>
		/// </summary>
		/// <param name="lpClassName">The class name or a class atom created by a previous call to the RegisterClass or RegisterClassEx function. 
		/// The atom must be in the low-order word of lpClassName; the high-order word must be zero. </param>
		/// <param name="lpWindowName">The window name (the window's title). If this parameter is NULL, all window names match. </param>
		/// <returns>If the function succeeds, the return value is a handle to the window that has the specified class name and window name.</returns>
		[DllImport("user32.dll", SetLastError = true)]
		static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		/// <summary>
		/// The SHAppBarMessage function sends an appbar message to the system.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/bb762108(v=vs.85).aspx"/>
		/// </summary>
		/// <param name="dwMessage">Appbar message value to send</param>
		/// <param name="pData">The address of an APPBARDATA structure. The content of the structure on entry and on exit depends on the 
		/// value set in the dwMessage parameter.</param>
		/// <returns>This function returns a message-dependent value.</returns>
		[DllImport("shell32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

		/// <summary>
		/// The GetWindowInfo function retrieves information about the specified window.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/ms633516%28VS.85%29.aspx"/>
		/// </summary>
		/// <param name="hwnd">The window handle.</param>
		/// <param name="pwi">The reference to WINDOWINFO structure.</param>
		/// <returns>true on success</returns>
		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);
				
		/// <summary>
		/// This function changes an attribute of the specified window.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/aa926164.aspx"/>
		/// </summary>
		/// <param name="hwnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value to be set.</param>
		/// <param name="dwNewLong">Specifies the replacement value.</param>
		/// <returns>The previous value of the specified 32-bit integer indicates success. Zero indicates failure.</returns>
		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

		/// <summary>
		/// This function retrieves information about the specified window.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/ms960886.aspx"/>
		/// </summary>
		/// <param name="hwnd">Handle to the window and, indirectly, the class to which the window belongs.</param>
		/// <param name="nIndex">Specifies the zero-based offset to the value to be set.</param>
		/// <returns>The requested 32-bit value indicates success. Zero indicates failure.</returns>
		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

		/// <summary>
		/// The MonitorFromWindow function retrieves a handle to the display monitor
		/// that has the largest area of intersection with the bounding rectangle
		/// of a specified window.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/dd145064%28VS.85%29.aspx"/>
		/// </summary>
		/// <param name="hwnd">The window handle.</param>
		/// <param name="dwFlags">Determines the function's return value
		/// if the window does not intersect any display monitor.</param>
		/// <returns>
		/// Monitor HMONITOR handle on success or based on dwFlags for failure
		/// </returns>
		[DllImport("user32.dll")]
		private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		/// <summary>
		/// The GetMonitorInfo function retrieves information about a display monitor
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/dd144901%28VS.85%29.aspx"/>
		/// </summary>
		/// <param name="hMonitor">A handle to the display monitor of interest.</param>
		/// <param name="lpmi">
		/// A pointer to a MONITORINFO structure that receives information
		/// about the specified display monitor.
		/// </param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);


		/// <summary>
		/// The SendMessage function sends the specified message to a window or windows. 
		/// The SendMessage function calls the window procedure for the specified window and does not return 
		/// until the window procedure has processed the message.
		/// <seealso cref="http://msdn.microsoft.com/en-us/library/windows/desktop/ms644950(v=vs.85).aspx"/>
		/// </summary>
		/// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
		/// <param name="Msg">The message to be sent.</param>
		/// <param name="wParam">Additional message-specific information.</param>
		/// <param name="lParam">Additional message-specific information.</param>
		/// <returns></returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		#endregion

		#region Properties

		/// <summary>
		/// Min width of a window
		/// </summary>
		internal double MinWidowWidth { get; set; }

		/// <summary>
		/// Min height of a window
		/// </summary>
		internal double MinWidowHeight { get; set; }

		#endregion

		/// <summary>
		/// Window procedure callback.
		/// Hooked to a WPF maximized window works around a WPF bug:
		/// https://connect.microsoft.com/VisualStudio/feedback/details/363288/maximised-wpf-window-not-covering-full-screen?wa=wsignin1.0#tabs
		/// possibly also:
		/// https://connect.microsoft.com/VisualStudio/feedback/details/540394/maximized-window-does-not-cover-working-area-after-screen-setup-change?wa=wsignin1.0
		/// </summary>
		/// <param name="hwnd">The window handle.</param>
		/// <param name="msg">The window message.</param>
		/// <param name="wParam">The wParam (word parameter).</param>
		/// <param name="lParam">The lParam (long parameter).</param>
		/// <param name="handled">
		/// if set to <c>true</c> - the message is handled
		/// and should not be processed by other callbacks.
		/// </param>
		/// <returns></returns>
		internal IntPtr MaximizedSizeFixWindowProc(
			IntPtr hwnd,
			int msg,
			IntPtr wParam,
			IntPtr lParam,
			ref bool handled)
		{
			switch (msg)
			{
				case WM_GETMINMAXINFO:
					// Handle the message and mark it as handled,
					// so other callbacks do not touch it
					WmGetMinMaxInfo(hwnd, lParam);
					handled = true;
					break;
			}
			return (IntPtr)0;
		}

		/// <summary>
		/// The WmGetMinMaxInfo function creates and populates the MINMAXINFO structure for a maximized window.
		/// Puts the structure into memory address given by lParam.
		/// Only used to process a WM_GETMINMAXINFO message.
		/// </summary>
		/// <param name="hwnd">The window handle.</param>
		/// <param name="lParam">The lParam.</param>
		private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
		{
			// Get the MINMAXINFO structure from memory location given by lParam
			MINMAXINFO mmi =
				(MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

			// Get the monitor that overlaps the window or the nearest
			IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
			if (monitor != IntPtr.Zero)
			{
				// Get monitor information
				MONITORINFO monitorInfo = new MONITORINFO();
				monitorInfo.Size = Marshal.SizeOf(typeof(MONITORINFO));
				GetMonitorInfo(monitor, ref monitorInfo);

				// The display monitor rectangle.
				// If the monitor is not the primary display monitor,
				// some of the rectangle's coordinates may be negative values
				RECT rcWorkArea = monitorInfo.WorkArea;
				RECT rcMonitorArea = monitorInfo.Monitor;
				
				// Set the dimensions of the window in maximized state
				mmi.MaxPosition.X = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
				mmi.MaxPosition.Y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
				mmi.MaxSize.X = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
				mmi.MaxSize.Y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top);

				// Set the maximum drag X size for the window
				mmi.MaxTrackSize.X = mmi.MaxSize.X;
				// Set the maximum drag Y size for the window
				mmi.MaxTrackSize.Y = mmi.MaxSize.Y;
				mmi.MinTrackSize.X = (int)MinWidowWidth;
				mmi.MinTrackSize.Y = (int)MinWidowHeight;

				// Set the working area depending of TaskBar position and AutoHide status
				mmi = AdjustWorkingAreaForAutoHide(monitor, mmi);
			}

			// Copy the structure to memory location specified by lParam.
			// This concludes processing of WM_GETMINMAXINFO.
			Marshal.StructureToPtr(mmi, lParam, true);
		}

		/// <summary>
		/// The SetStyleAndExStyle function defines the default and extended styles for the window.
		/// Used to remove the border around the window if the AllowTransparency property is set as True value.
		/// </summary>
		/// <param name="windowHandle">The window handle.</param>
		internal void SetStyleAndExStyle(IntPtr windowHandle)
		{
			int longStyle = GetWindowLong(windowHandle, GWL_STYLE);
			longStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZE | WS_MAXIMIZE);
			SetWindowLong(windowHandle, GWL_STYLE, longStyle);

			int longExStyle = GetWindowLong(windowHandle, GWL_EXSTYLE);
			longExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
			SetWindowLong(windowHandle, GWL_EXSTYLE, longExStyle);
		}

		/// <summary>
		/// The GetEdge function defines the position of TaskBar
		/// </summary>
		/// <param name="rc">A RECT structure to contain the bounding rectangle, in screen coordinates, of an appbar or the Windows taskbar.</param>
		/// <returns>A value that specifies an edge of the screen</returns>
		private int GetEdge(RECT rc)
		{
			int uEdge = -1;
			if (rc.Top == rc.Left && rc.Bottom > rc.Right)
				uEdge = (int)ABEdge.ABE_LEFT;
			else if (rc.Top == rc.Left && rc.Bottom < rc.Right)
				uEdge = (int)ABEdge.ABE_TOP;
			else if (rc.Top > rc.Left)
				uEdge = (int)ABEdge.ABE_BOTTOM;
			else
				uEdge = (int)ABEdge.ABE_RIGHT;
			return uEdge;
		}

		/// <summary>
		/// The AdjustWorkingAreaForAutoHide function defines the working area depending of TaskBar position and AutoHide status
		/// </summary>
		/// <param name="monitorContainingApplication">The monitor that overlaps the window</param>
		/// <param name="mmi">The MINMAXINFO structure of a maximized window</param>
		/// <returns>Returns MINMAXINFO structure for a maximized window</returns>
		private MINMAXINFO AdjustWorkingAreaForAutoHide(IntPtr monitorContainingApplication, MINMAXINFO mmi)
		{
			// Get a handle to the top-level window whose class name and window name match the "Shell_TrayWnd" string
			IntPtr hwnd = FindWindow("Shell_TrayWnd", null);
			if (hwnd == null) return mmi;

			// Get a handle to the monitor that overlaps the window or the nearest
			IntPtr monitorWithTaskbarOnIt = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
			if (!monitorContainingApplication.Equals(monitorWithTaskbarOnIt)) return mmi;

			// Gets the structure with information about a system appbar message
			APPBARDATA abd = new APPBARDATA();
			abd.cbSize = Marshal.SizeOf(abd);
			abd.hWnd = hwnd;
			// Send an appbar message to the system
			SHAppBarMessage((int)ABMsg.ABM_GETTASKBARPOS, ref abd);
			// Define the AutoHide status of TaskBar
			bool autoHide = System.Convert.ToBoolean(SHAppBarMessage((int)ABMsg.ABM_GETSTATE, ref abd));
			if (!autoHide) return mmi;

			// Get a position of TaskBar
			int uEdge = GetEdge(abd.rc);
			// Get a size of window depending of TaskBar position
			switch (uEdge)
			{
				case (int)ABEdge.ABE_LEFT:
					mmi.MaxPosition.X += 2;
					mmi.MaxTrackSize.X -= 2;
					mmi.MaxSize.X -= 2;
					break;
				case (int)ABEdge.ABE_RIGHT:
					mmi.MaxSize.X -= 2;
					mmi.MaxTrackSize.X -= 2;
					break;
				case (int)ABEdge.ABE_TOP:
					mmi.MaxPosition.Y += 2;
					mmi.MaxTrackSize.Y -= 2;
					mmi.MaxSize.Y -= 2;
					break;
				case (int)ABEdge.ABE_BOTTOM:
					mmi.MaxSize.Y -= 2;
					mmi.MaxTrackSize.Y -= 2;
					break;
				default:
					return mmi;
			}
			return mmi;
		}

		/// <summary>
		/// The ResizeWindow function resizes window in custom direction
		/// </summary>
		/// <param name="windowHandle">The window handle.</param>
		/// <param name="direction">Enum value that specifies an direction of the window resizing</param>
		internal void ResizeWindow(IntPtr windowHandle, ResizeDirection direction)
		{
			SendMessage(windowHandle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
		}
	}
}