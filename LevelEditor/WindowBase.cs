using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace LevelEditor
{
	[TemplatePart(Name = "PART_CloseButton", Type = typeof(ButtonBase)), TemplatePart(Name = "PART_MaximizeButton", Type = typeof(ButtonBase)), TemplatePart(Name = "PART_TitleBar", Type = typeof(UIElement)), TemplatePart(Name = "PART_Toolbar", Type = typeof(Panel)), TemplatePart(Name = "PART_MinimizeButton", Type = typeof(ButtonBase))]
	public class WindowBase : Window
	{
		#region DependencyProperties

		public static readonly DependencyProperty HasCloseProperty = DependencyProperty.Register("HasClose", typeof(bool), typeof(WindowBase), new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public static readonly DependencyProperty HasMaximizeProperty = DependencyProperty.Register("HasMaximize", typeof(bool), typeof(WindowBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
		public static readonly DependencyProperty HasMinimizeProperty = DependencyProperty.Register("HasMinimize", typeof(bool), typeof(WindowBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));
		
		#endregion

		#region Constants and Fields

		private const string DefaultDialogWindowStyle = "Window_Base_Style";
		
		private HwndSource windowSource;
		
		private WindowBaseAPI wbAPI;

		#endregion 

		#region Constructors

		public WindowBase()
		{
			InitializeWindow();
		}

		#endregion

		#region Initialize

		protected virtual void InitializeWindow()
		{
			this.SetResourceReference(StyleProperty, DefaultDialogWindowStyle);
			this.Loaded += new RoutedEventHandler(WindowBase_Loaded);
			this.Unloaded += new RoutedEventHandler(WindowBase_Unloaded);
			this.PreviewMouseMove += new MouseEventHandler(ResetCursor);
		}

		#endregion

		#region Loaded / Unloaded

		void WindowBase_Loaded(object sender, RoutedEventArgs e)
		{
			wbAPI = new WindowBaseAPI();
			wbAPI.MinWidowHeight = this.MinHeight;
			wbAPI.MinWidowWidth = this.MinWidth;

			windowSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
			wbAPI.SetStyleAndExStyle(windowSource.Handle);
			windowSource.AddHook(wbAPI.MaximizedSizeFixWindowProc);
		}

		protected void WindowBase_Unloaded(object sender, RoutedEventArgs e)
		{
			Resources.MergedDictionaries.Clear();
		}

		#endregion

		#region Methods
		
		private void CloseWindow()
		{
			base.Close();
		}

		private void MinimizeWindow()
		{
			base.WindowState = WindowState.Minimized;
		}

		private void MoveWindow()
		{
			base.DragMove();
		}

		protected virtual void HandleMinMaxInfo()
		{
			base.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
		}

		private void MaximizeOrRestoreWindow()
		{
			if (base.WindowState == WindowState.Normal)
			{
				base.WindowState = WindowState.Maximized;
			}
			else if (base.WindowState == WindowState.Maximized)
			{
				base.WindowState = WindowState.Normal;
			}
		}

		protected virtual void SetHeaderEvents()
		{
			UIElement elementTitleBar = base.GetTemplateChild("PART_TitleBar") as UIElement;
			if (elementTitleBar != null)
			{
				elementTitleBar.MouseLeftButtonDown += delegate(object sender, MouseButtonEventArgs e)
				{
					if (e.ClickCount == 2)
					{
						if (this.HasMaximize)
						{
							ButtonBase templateChild = base.GetTemplateChild("PART_MaximizeButton") as ButtonBase;
							if ((templateChild != null) && templateChild.IsVisible)
							{
								this.MaximizeOrRestoreWindow();
							}
						}
					}
					else
					{
						this.MoveWindow();
					}
				};
			}

			ButtonBase buttonClose = base.GetTemplateChild("PART_CloseButton") as ButtonBase;
			if (buttonClose != null)
			{
				buttonClose.Click += delegate
				{
					this.CloseWindow();
				};
			}

			ButtonBase buttonMinimize = base.GetTemplateChild("PART_MinimizeButton") as ButtonBase;
			if (buttonMinimize != null)
			{
				buttonMinimize.Click += delegate
				{
					this.MinimizeWindow();
				};
			}

			ButtonBase buttonMaximize = base.GetTemplateChild("PART_MaximizeButton") as ButtonBase;
			if (buttonMaximize != null)
			{
				buttonMaximize.Click += delegate
				{
					this.MaximizeOrRestoreWindow();
				};
			}

		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			this.SetHeaderEvents();
			this.HandleMinMaxInfo();

			this.AddResizeEventToBorders();
		}

		#endregion

		#region Properties

		public bool HasClose
		{
			get
			{
				return (bool)base.GetValue(HasCloseProperty);
			}
			set
			{
				base.SetValue(HasCloseProperty, value);
			}
		}

		public bool HasMaximize
		{
			get
			{
				return (bool)base.GetValue(HasMaximizeProperty);
			}
			set
			{
				base.SetValue(HasMaximizeProperty, value);
			}
		}

		public bool HasMinimize
		{
			get
			{
				return (bool)base.GetValue(HasMinimizeProperty);
			}
			set
			{
				base.SetValue(HasMinimizeProperty, value);
			}
		}

		#endregion

		#region Resize Methods

		private void ResizeWindow(WindowBaseAPI.ResizeDirection direction)
		{
			if (Mouse.LeftButton == MouseButtonState.Pressed)
			{
				wbAPI.ResizeWindow(windowSource.Handle, direction);
			}
		}

		protected void ResetCursor(object sender, MouseEventArgs e)
		{
			if (Mouse.LeftButton != MouseButtonState.Pressed)
			{
				this.Cursor = Cursors.Arrow;
			}
		}
			
		protected void AddResizeEventToBorders()
		{
			Border rectLeft = base.GetTemplateChild("BORDER_Left") as Border;
			if (rectLeft != null) rectLeft.PreviewMouseDown += new MouseButtonEventHandler(Resize);
			
			Border rectRight = base.GetTemplateChild("BORDER_Right") as Border;
			if (rectRight != null) rectRight.PreviewMouseDown += new MouseButtonEventHandler(Resize);
			
			Border rectTop = base.GetTemplateChild("BORDER_Top") as Border;
			if (rectTop != null) rectTop.PreviewMouseDown += new MouseButtonEventHandler(Resize);
			
			Border rectBottom = base.GetTemplateChild("BORDER_Bottom") as Border;
			if (rectBottom != null) rectBottom.PreviewMouseDown += new MouseButtonEventHandler(Resize);
			
			Border rectLeftTop = base.GetTemplateChild("BORDER_LeftTop") as Border;
			if (rectLeftTop != null) rectLeftTop.PreviewMouseDown += new MouseButtonEventHandler(Resize);
		
			Border rectTopLeft = base.GetTemplateChild("BORDER_TopLeft") as Border;
			if (rectTopLeft != null) rectTopLeft.PreviewMouseDown += new MouseButtonEventHandler(Resize);
		
			Border rectRightTop = base.GetTemplateChild("BORDER_RightTop") as Border;
			if (rectRightTop != null) rectRightTop.PreviewMouseDown += new MouseButtonEventHandler(Resize);
		
			Border rectTopRight = base.GetTemplateChild("BORDER_TopRight") as Border;
			if (rectTopRight != null) rectTopRight.PreviewMouseDown += new MouseButtonEventHandler(Resize);
		
			Border rectRightBottom = base.GetTemplateChild("BORDER_RightBottom") as Border;
			if (rectRightBottom != null) rectRightBottom.PreviewMouseDown += new MouseButtonEventHandler(Resize);
		
			Border rectBottomRight = base.GetTemplateChild("BORDER_BottomRight") as Border;
			if (rectBottomRight != null) rectBottomRight.PreviewMouseDown += new MouseButtonEventHandler(Resize);
		
			Border rectBottomLeft = base.GetTemplateChild("BORDER_BottomLeft") as Border;
			if (rectBottomLeft != null) rectBottomLeft.PreviewMouseDown += new MouseButtonEventHandler(Resize);

			Border rectLeftBottom = base.GetTemplateChild("BORDER_LeftBottom") as Border;
			if (rectLeftBottom != null) rectLeftBottom.PreviewMouseDown += new MouseButtonEventHandler(Resize);
		}

		private void Resize(object sender, MouseButtonEventArgs e)
		{
			Border clickedBorder = sender as Border;

			switch (clickedBorder.Name)
			{
				case "BORDER_Top":
					this.Cursor = Cursors.SizeNS;
					ResizeWindow(WindowBaseAPI.ResizeDirection.TOP);
					break;
				case "BORDER_Bottom":
					this.Cursor = Cursors.SizeNS;
					ResizeWindow(WindowBaseAPI.ResizeDirection.BOTTOM);
					break;
				case "BORDER_Left":
					this.Cursor = Cursors.SizeWE;
					ResizeWindow(WindowBaseAPI.ResizeDirection.LEFT);
					break;
				case "BORDER_Right":
					this.Cursor = Cursors.SizeWE;
					ResizeWindow(WindowBaseAPI.ResizeDirection.RIGHT);
					break;
				case "BORDER_TopLeft":
					this.Cursor = Cursors.SizeNWSE;
					ResizeWindow(WindowBaseAPI.ResizeDirection.TOPLEFT);
					break;
				case "BORDER_LeftTop":
					this.Cursor = Cursors.SizeNWSE;
					ResizeWindow(WindowBaseAPI.ResizeDirection.TOPLEFT);
					break;
				case "BORDER_TopRight":
					this.Cursor = Cursors.SizeNESW;
					ResizeWindow(WindowBaseAPI.ResizeDirection.TOPRIGHT);
					break;
				case "BORDER_RightTop":
					this.Cursor = Cursors.SizeNESW;
					ResizeWindow(WindowBaseAPI.ResizeDirection.TOPRIGHT);
					break;
				case "BORDER_LeftBottom":
					this.Cursor = Cursors.SizeNESW;
					ResizeWindow(WindowBaseAPI.ResizeDirection.BOTTOMLEFT);
					break;
				case "BORDER_BottomLeft":
					this.Cursor = Cursors.SizeNESW;
					ResizeWindow(WindowBaseAPI.ResizeDirection.BOTTOMLEFT);
					break;
				case "BORDER_BottomRight":
					this.Cursor = Cursors.SizeNWSE;
					ResizeWindow(WindowBaseAPI.ResizeDirection.BOTTOMRIGHT);
					break;
				case "BORDER_RightBottom":
					this.Cursor = Cursors.SizeNWSE;
					ResizeWindow(WindowBaseAPI.ResizeDirection.BOTTOMRIGHT);
					break;
				default:
					break;
			}
		}

		#endregion
	}
}
