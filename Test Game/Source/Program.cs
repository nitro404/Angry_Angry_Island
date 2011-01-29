using System;

namespace Test_Game {
	public static class Program {
		[STAThread]
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args) {
			using(TestGame game = new TestGame()) {
				game.Run();
			}
		}
	}
}

