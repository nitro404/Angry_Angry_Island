using System;

namespace Firecracker_Engine {
	static class Program {
		static void Main(string[] args) {
			using(Firecracker game = new Firecracker()) {
				game.Run();
			}
		}
	}
}

