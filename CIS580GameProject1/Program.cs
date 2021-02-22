using System;

namespace CIS580GameProject1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameProject1())
                game.Run();
        }
    }
}
