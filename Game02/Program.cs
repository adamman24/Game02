using System;

namespace Game02
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game02())
                game.Run();
        }
    }
}
