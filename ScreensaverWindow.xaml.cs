using System;
using System.Windows;

namespace NovaScreenSaver
{
    public partial class ScreensaverWindow : Window
    {
        private System.Drawing.Point _mouseOrigin;
        private readonly bool _isPreview;
        private readonly System.Windows.Forms.Screen? _screen;
        private readonly Settings.Config _cfg;

        public ScreensaverWindow(Settings.Config cfg, System.Windows.Forms.Screen? screen, bool isPreview = false)
        {
            InitializeComponent();
            _cfg = cfg; _screen = screen; _isPreview = isPreview;
            Loaded += OnLoaded;
            MouseMove += (_, __) => CheckMouse();
            MouseDown += (_, __) => Exit();
            KeyDown += (_, __) => Exit();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (!_isPreview && _screen != null)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                Left = _screen.Bounds.Left; Top = _screen.Bounds.Top;
                Width = _screen.Bounds.Width; Height = _screen.Bounds.Height;
            }
            _mouseOrigin = System.Windows.Forms.Control.MousePosition;
            Renderer.Initialize(_cfg);
            Renderer.Start();
        }

        private void CheckMouse()
        {
            if (_isPreview) return;
            var pos = System.Windows.Forms.Control.MousePosition;
            if (Math.Abs(pos.X - _mouseOrigin.X) > 8 || Math.Abs(pos.Y - _mouseOrigin.Y) > 8) Exit();
        }

        private void Exit() => System.Windows.Application.Current.Shutdown();
    }
}
