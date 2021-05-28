using System;
using System.Collections;
using System.Linq;

namespace Hnefatafl
{
	class Program
	{
		struct Move {
			public int x;
			public int y;
			public int destx;
			public int desty;
		}
		
		public static Move parseMove(string s) {
			string[] positions = s.ToUpper().Trim().Split(new string[] {"=>"}, 2, StringSplitOptions.RemoveEmptyEntries);
			Move move;
			if (Enumerable.Range('A', 'K').Contains(positions[0][0])) {
				move.y = (int)positions[0][0] - 65;
			} else {
				throw Exception("y position not in range of A-K. y must be before x");
			}
			if (Enumerable.Range('A', 'K').Contains(positions[1][0])) {
				move.desty = (int)positions[1][0] - 65;
			} else {
				throw Exception("desty position not in range of A-K. desty must be before destx");
			}
			try {
				positions[0].Remove(0);
				positions[1].Remove(0);
				move.x = Convert.ToInt32(positions[0]);
				move.destx = Convert.ToInt32(positions[1]);
				return move;
			}
			catch (Exception e) {
				Console.WriteLine(e.Message);
				throw Exception("Was not able to Convert the x and destx positions");
			}
		}
		
		public static void Main(string[] args)
		{
			Game game = new Game();
			game.print();
			Console.Write("Enter the moves (\n\n\nFormat COL ROW => COL ROW dest)\n");
			
			int origRow;
			int origCol;
			while (true) {
				origRow = Console.CursorTop;
				origCol = Console.CursorLeft;
				Console.SetCursorPosition(0, 0);
				game.print();
				Console.SetCursorPosition(origCol, origRow);
				
			}
			
			
			

			Console.ReadKey(true);
		}
	}
}
