using System;

namespace Firecracker_Engine {
	static class Program {
        [STAThread]
		static void Main(string[] args) {
			using(Firecracker game = new Firecracker()) {
				game.Run();
			}
		}
	}
}

