using OpenTK.Windowing.Desktop;
using System;

namespace UASgrafkom
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new OpenTK.Mathematics.Vector2i(1600, 900),
                Title = "Proyek UAS"
            };

            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}
