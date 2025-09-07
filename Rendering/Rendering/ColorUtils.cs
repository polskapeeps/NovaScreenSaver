namespace NovaScreenSaver.Rendering
{
    internal static class ColorUtils
    {
        public static int Pack(System.Windows.Media.Color c)
            => (c.B) | (c.G << 8) | (c.R << 16) | (255 << 24);

        public static System.Windows.Media.Color Lerp(System.Windows.Media.Color a, System.Windows.Media.Color b, double t)
        {
            t = System.Math.Clamp(t, 0, 1);
            byte r = (byte)(a.R + (b.R - a.R) * t);
            byte g = (byte)(a.G + (b.G - a.G) * t);
            byte b2 = (byte)(a.B + (b.B - a.B) * t);
            return System.Windows.Media.Color.FromArgb(255, r, g, b2);
        }
    }
}
