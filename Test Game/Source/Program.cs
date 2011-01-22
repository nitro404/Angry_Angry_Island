using System;

namespace Test_Game {
	static class Program {
		public static TestGame game;

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

