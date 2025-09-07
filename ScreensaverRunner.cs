using System;
using System.Collections.Generic;
using System.Linq;
using NovaScreenSaver.Settings;

namespace NovaScreenSaver
{
    internal static class ScreensaverRunner
    {
        public static void Run(string[] args)
        {
            var cfg = ConfigService.Load();
            if (args == null || args.Length == 0) { ShowConfig(cfg); return; }

            var a = string.Join(" ", args).Trim();
            if (a.StartsWith("/c", StringComparison.OrdinalIgnoreCase)) { ShowConfig(cfg); return; }
            if (a.StartsWith("/s", StringComparison.OrdinalIgnoreCase)) { RunScreensaver(cfg); return; }
            if (a.StartsWith("/p", StringComparison.OrdinalIgnoreCase))
            {
                IntPtr parent = IntPtr.Zero;
                foreach (var token in args.Skip(1))
                    if (long.TryParse(token, out var h)) { parent = new IntPtr(h); break; }
                if (parent == IntPtr.Zero)
                {
                    var digits = new string(a.SkipWhile(c => !char.IsDigit(c)).TakeWhile(char.IsDigit).ToArray());
                    if (long.TryParse(digits, out var hh)) parent = new IntPtr(hh);
                }
                PreviewHost.ShowPreview(parent, cfg);
                return;
            }
            ShowConfig(cfg);
        }

        private static void RunScreensaver(Config cfg)
        {
            var wins = new List<ScreensaverWindow>();
            foreach (var s in System.Windows.Forms.Screen.AllScreens)
            {
                var w = new ScreensaverWindow(cfg, s);
                wins.Add(w);
                w.Show();
            }
            foreach (var w in wins)
                w.Closed += (_, __) => System.Windows.Application.Current.Shutdown();
        }

        private static void ShowConfig(Config cfg)
        {
            var w = new UI.SettingsWindow(cfg);
            w.ShowDialog();
            System.Windows.Application.Current.Shutdown();
        }
    }
}
