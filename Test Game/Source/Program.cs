using System;

namespace Test_Game {
	public static class Program {
		public static TestGame g_Game = null;

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args) {
            using (TestGame g_Game = new TestGame())
            {
                g_Game.Run();
			}
		}
	}
}

