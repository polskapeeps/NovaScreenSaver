using System;
using AuroraScreenSaver.Settings;

namespace AuroraScreenSaver.Rendering
{
    internal sealed class AuroraRenderer : IRenderer
    {
        private Palette _palette = Palette.Get(PaletteName.Ocean);

        public void Initialize(int width, int height, Config cfg)
        {
            _palette = Palette.Get(cfg.Palette);
        }

        public void Render(Span<int> pixels, int width, int height, double time, Config cfg)
        {
            _palette = Palette.Get(cfg.Palette);

            double freq = 0.7 * cfg.Complexity;
            double t = time * 0.15;
            double invW = 1.0 / Math.Max(1, width);
            double invH = 1.0 / Math.Max(1, height);

            int idx = 0;
            for (int y = 0; y < height; y++)
            {
                double ny = y * invH;
                for (int x = 0; x < width; x++, idx++)
                {
                    double nx = x * invW;
                    double n1 = PerlinNoise.Noise(nx * (freq * 1.0) + t * 0.50, ny * (freq * 1.0) + t * 0.40, t * 0.30);
                    double n2 = PerlinNoise.Noise(nx * (freq * 2.0) - t * 0.40, ny * (freq * 2.0) + 11.3, t * 0.55);
                    double n3 = PerlinNoise.Noise(nx * (freq * 4.0) + 3.7, ny * (freq * 4.0) - t * 0.25, t * 0.80);

                    double v = n1 * 0.55 + n2 * 0.30 + n3 * 0.15;
                    v = 0.5 + 0.5 * Math.Sin((v * 2.4 + t * 0.8) * Math.PI);
                    v = Math.Pow(v, 1.0 / (0.8 + cfg.Intensity * 0.4));

                    var c = _palette.Sample(v);
                    pixels[idx] = ColorUtils.Pack(c);
                }
            }
        }
    }
}
