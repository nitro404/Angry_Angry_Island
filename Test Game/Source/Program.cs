using System;

namespace Angry_Angry_Island {
	public static class Program {
		[STAThread]
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args) {
			using(AngryAngryIsland game = new AngryAngryIsland()) {
				game.Run();
			}
		}
	}
}

