using System.Windows;
using NovaScreenSaver.Rendering;
using NovaScreenSaver.Settings;

namespace NovaScreenSaver.UI
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow(Config cfg)
        {
            InitializeComponent();
            LoadToUI(cfg);

            SpeedSlider.ValueChanged += (_, __) => RefreshPreview();
            ComplexitySlider.ValueChanged += (_, __) => RefreshPreview();
            IntensitySlider.ValueChanged += (_, __) => RefreshPreview();
            ScaleSlider.ValueChanged += (_, __) => RefreshPreview();
            StyleBox.SelectionChanged += (_, __) => RefreshPreview();
            PaletteBox.SelectionChanged += (_, __) => RefreshPreview();
            ClockCheck.Checked += (_, __) => RefreshPreview();
            ClockCheck.Unchecked += (_, __) => RefreshPreview();
        }

        private void LoadToUI(Config cfg)
        {
            StyleBox.SelectedIndex = (int)cfg.Style;
            PaletteBox.SelectedIndex = (int)cfg.Palette;
            SpeedSlider.Value = cfg.Speed;
            ComplexitySlider.Value = cfg.Complexity;
            IntensitySlider.Value = cfg.Intensity;
            ScaleSlider.Value = cfg.ResolutionScale;
            ClockCheck.IsChecked = cfg.ShowClock;
            RefreshPreview();
        }

        private Config Collect()
        {
            return new Config
            {
                Style = (VisualStyle)StyleBox.SelectedIndex,
                Palette = (PaletteName)PaletteBox.SelectedIndex,
                Speed = SpeedSlider.Value,
                Complexity = (int)ComplexitySlider.Value,
                Intensity = IntensitySlider.Value,
                ResolutionScale = ScaleSlider.Value,
                ShowClock = ClockCheck.IsChecked ?? false
            };
        }

        private void RefreshPreview() => Preview.Apply(Collect());

        private void OnSave(object sender, RoutedEventArgs e)
        {
            ConfigService.Save(Collect());
            DialogResult = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
