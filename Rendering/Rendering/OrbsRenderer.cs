using System;
using System.Collections.Generic;
using NovaScreenSaver.Settings;

namespace NovaScreenSaver.Rendering
{
    internal sealed class OrbsRenderer : IRenderer
    {
        private struct Orb
        {
            public float X, Y, VX, VY, R;
            public System.Windows.Media.Color Color;
        }

        private readonly List<Orb> _orbs = new();
        private readonly Random _rng = new Random(1234);
        private Palette _palette = Palette.Get(PaletteName.Ocean);

        public void Initialize(int width, int height, Config cfg)
        {
            _palette = Palette.Get(cfg.Palette);
            _orbs.Clear();
            int count = 90 + cfg.Complexity * 40;
            for (int i = 0; i < count; i++)
            {
                var t = _rng.NextDouble();
                var c = _palette.Sample(t);
                _orbs.Add(new Orb
                {
                    X = (float)(_rng.NextDouble() * width),
                    Y = (float)(_rng.NextDouble() * height),
                    VX = (float)((_rng.NextDouble() - 0.5) * 20),
                    VY = (float)((_rng.NextDouble() - 0.5) * 20),
                    R = (float)(8 + _rng.NextDouble() * (18 + cfg.Intensity * 10)),
                    Color = System.Windows.Media.Color.FromArgb(220, c.R, c.G, c.B)
                });
            }
        }

        public void Render(Span<int> pixels, int width, int height, double time, Config cfg)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                int p = pixels[i];
                byte b = (byte)(p & 255);
                byte g = (byte)((p >> 8) & 255);
                byte r = (byte)((p >> 16) & 255);
                float fade = 0.86f;
                r = (byte)(r * fade); g = (byte)(g * fade); b = (byte)(b * fade);
                pixels[i] = (255 << 24) | (r << 16) | (g << 8) | b;
            }

            float dt = (float)(0.016 * cfg.Speed);
            for (int i = 0; i < _orbs.Count; i++)
            {
                var o = _orbs[i];
                float fx = (float)(PerlinNoise.Noise(o.X * 0.002, o.Y * 0.002, time * 0.2) - 0.5);
                float fy = (float)(PerlinNoise.Noise(o.X * 0.002 + 10.5, o.Y * 0.002 + 23.1, time * 0.2) - 0.5);
                o.VX += fx * (2 + cfg.Intensity * 2);
                o.VY += fy * (2 + cfg.Intensity * 2);
                o.VX *= 0.98f; o.VY *= 0.98f;
                o.X += o.VX * dt * 60; o.Y += o.VY * dt * 60;
                if (o.X < -o.R) o.X = width + o.R;
                if (o.Y < -o.R) o.Y = height + o.R;
                if (o.X > width + o.R) o.X = -o.R;
                if (o.Y > height + o.R) o.Y = -o.R;
                DrawOrb(pixels, width, height, o);
                _orbs[i] = o;
            }
        }

        private static void DrawOrb(Span<int> pixels, int width, int height, Orb o)
        {
            int minX = Math.Max(0, (int)(o.X - o.R - 1));
            int maxX = Math.Min(width - 1, (int)(o.X + o.R + 1));
            int minY = Math.Max(0, (int)(o.Y - o.R - 1));
            int maxY = Math.Min(height - 1, (int)(o.Y + o.R + 1));
            float rr = o.R * o.R;

            for (int y = minY; y <= maxY; y++)
            {
                int idx = y * width + minX;
                float dy = y - o.Y;
                for (int x = minX; x <= maxX; x++, idx++)
                {
                    float dx = x - o.X;
                    float d2 = dx * dx + dy * dy;
                    if (d2 > rr) continue;
                    float a = MathF.Exp(-d2 / (0.5f * rr));
                    byte ar = (byte)(o.Color.R * a);
                    byte ag = (byte)(o.Color.G * a);
                    byte ab = (byte)(o.Color.B * a);

                    int p = pixels[idx];
                    int b = p & 255, g = (p >> 8) & 255, r = (p >> 16) & 255;
                    int nr = r + ar; if (nr > 255) nr = 255;
                    int ng = g + ag; if (ng > 255) ng = 255;
                    int nb = b + ab; if (nb > 255) nb = 255;
                    pixels[idx] = (255 << 24) | (nr << 16) | (ng << 8) | nb;
                }
            }
        }
    }
}
