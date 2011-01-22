using System;

namespace EmptyGame
{
    static class Program
    {
        public static EmptyGame g_Game = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EmptyGame g_Game = new EmptyGame())
            {
                g_Game.Run();
            }
        }
    }
}

