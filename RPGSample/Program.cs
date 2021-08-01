using System;

namespace RPGSample
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new RPGSample())
                game.Run();
        }
    }
}
