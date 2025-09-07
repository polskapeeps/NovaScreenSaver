using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NovaScreenSaver.Rendering;
using NovaScreenSaver.Settings;

namespace NovaScreenSaver
{
    public partial class RendererView : System.Windows.Controls.UserControl
    {
        private WriteableBitmap? _wb;
        private IRenderer? _renderer;
        private readonly Stopwatch _sw = new Stopwatch();
        private TimeSpan _prev;
        private double _time;
        private bool _running;
        private Config _cfg = Config.Defaults();

        public RendererView() { InitializeComponent(); }

        public void Initialize(Config cfg)
        {
            _cfg = cfg;
            ClockText.Visibility = cfg.ShowClock ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            _renderer = _cfg.Style switch
            {
                VisualStyle.Aurora => new AuroraRenderer(),
                VisualStyle.Orbs => new OrbsRenderer(),
                _ => new AuroraRenderer()
            };
        }

        public void Start()
        {
            if (_running) return;
            _running = true;
            _sw.Restart();
            CompositionTarget.Rendering += OnRendering;
        }

        public void Stop()
        {
            if (!_running) return;
            _running = false;
            CompositionTarget.Rendering -= OnRendering;
            _sw.Stop();
        }

        private void EnsureBitmap()
        {
            if (_wb != null) return;
            double scale = Math.Clamp(_cfg.ResolutionScale, 0.35, 1.0);
            int w = Math.Max(320, (int)(ActualWidth * scale));
            int h = Math.Max(180, (int)(ActualHeight * scale));
            if (w <= 0 || h <= 0) { w = 960; h = 540; }
            _wb = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgra32, null);
            Image.Source = _wb;
            RenderOptions.SetBitmapScalingMode(Image, BitmapScalingMode.HighQuality);
            _renderer?.Initialize(w, h, _cfg);
        }

        private unsafe void OnRendering(object? sender, EventArgs e)
        {
            if (!_running) return;
            EnsureBitmap();
            if (_wb == null || _renderer == null) return;

            var now = _sw.Elapsed;
            var dt = (now - _prev).TotalSeconds;
            _prev = now;
            dt = Math.Clamp(dt, 0, 0.05);
            _time += dt * _cfg.Speed;

            _wb.Lock();
            var ptr = (int*)_wb.BackBuffer.ToPointer();
            _renderer.Render(new Span<int>(ptr, _wb.PixelWidth * _wb.PixelHeight), _wb.PixelWidth, _wb.PixelHeight, _time, _cfg);
            _wb.AddDirtyRect(new System.Windows.Int32Rect(0, 0, _wb.PixelWidth, _wb.PixelHeight));
            _wb.Unlock();

            if (_cfg.ShowClock) ClockText.Text = DateTime.Now.ToString("HH:mm");
        }
    }
}
