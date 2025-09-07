using System.Windows.Media;

namespace NovaScreenSaver.Rendering
{
    public enum PaletteName { Ocean, Sunset, Forest, Neon, Monochrome }

    internal sealed class Palette
    {
        private readonly Color[] _stops;
        public Palette(Color[] stops) { _stops = stops; }

        public Color Sample(double t)
        {
            if (_stops.Length == 0) return Colors.Black;
            if (_stops.Length == 1) return _stops[0];
            t = System.Math.Clamp(t, 0, 1);
            double scaled = t * (_stops.Length - 1);
            int i = (int)System.Math.Floor(scaled);
            int j = System.Math.Min(i + 1, _stops.Length - 1);
            double local = scaled - i;
            return ColorUtils.Lerp(_stops[i], _stops[j], local);
        }

        public static Palette Get(PaletteName name) => name switch
        {
            PaletteName.Ocean => new Palette(new[] { Color.FromRgb(2,12,30), Color.FromRgb(5,61,115), Color.FromRgb(12,130,180), Color.FromRgb(120,220,255) }),
            PaletteName.Sunset => new Palette(new[] { Color.FromRgb(20,2,20), Color.FromRgb(138,35,135), Color.FromRgb(255,86,33), Color.FromRgb(255,200,112) }),
            PaletteName.Forest => new Palette(new[] { Color.FromRgb(2,20,10), Color.FromRgb(12,80,40), Color.FromRgb(30,150,70), Color.FromRgb(160,220,120) }),
            PaletteName.Neon => new Palette(new[] { Color.FromRgb(0,0,0), Color.FromRgb(0,255,170), Color.FromRgb(255,20,220), Color.FromRgb(255,255,255) }),
            PaletteName.Monochrome => new Palette(new[] { Color.FromRgb(0,0,0), Color.FromRgb(255,255,255) }),
            _ => new Palette(new[] { Colors.Black, Colors.White })
        };
    }
}
