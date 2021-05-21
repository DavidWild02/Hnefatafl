/*
 * Created by SharpDevelop.
 * User: WiDa0502
 * Date: 23.04.2021
 * Time: 09:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

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
			
		}
		
		public static void Main(string[] args)
		{
			Game game = new Game();
			game.print();
			
			
			

			Console.ReadKey(true);
		}
	}
}