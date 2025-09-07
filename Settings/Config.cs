namespace NovaScreenSaver.Settings
{
    public enum VisualStyle { Aurora = 0, Orbs = 1 }

    public sealed class Config
    {
        public VisualStyle Style { get; set; } = VisualStyle.Aurora;
        public NovaScreenSaver.Rendering.PaletteName Palette { get; set; } = NovaScreenSaver.Rendering.PaletteName.Ocean;
        public double Speed { get; set; } = 1.0;
        public int Complexity { get; set; } = 2;
        public double Intensity { get; set; } = 1.0;
        public double ResolutionScale { get; set; } = 0.75;
        public bool ShowClock { get; set; } = false;
        public static Config Defaults() => new Config();
    }
}
