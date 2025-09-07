using System;
using NovaScreenSaver.Settings;

namespace NovaScreenSaver.Rendering
{
    public interface IRenderer
    {
        void Initialize(int width, int height, Config cfg);
        void Render(Span<int> pixels, int width, int height, double time, Config cfg);
    }
}
