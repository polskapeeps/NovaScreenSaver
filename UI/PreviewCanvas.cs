using NovaScreenSaver.Settings;

namespace NovaScreenSaver.UI
{
    internal class PreviewCanvas : RendererView
    {
        public void Apply(Config cfg)
        {
            Initialize(cfg);
            Start();
        }
    }
}
