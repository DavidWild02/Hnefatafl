using System;
using System.Linq;

namespace Hnefatafl
{	
	enum Field {
		King,
		Protector,
		Attacker,
		Empty,
		Finish,
		Throne,
		ThroneWithKing,
	}

    public enum Selection
    {
        Normal,
        Hovered,
        Selected,
    }
    /// <summary>
    /// Description of Game.
    /// </summary>
    /// 
    public class Game
	{
        public bool Victory { private set; get; }
        public bool BlackMove { private set; get; }

        private Field[,] board = new Field[11, 11];
		

		public Game()
		{
			this.initGame();
		}


        public void initGame()
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    this.board[i, j] = Field.Empty;
                }
            }

            this.board[0, 0] = this.board[0, 10] = this.board[10, 0] = this.board[10, 10] = Field.Finish;

            for (int i = 0; i < 5; i++)
            {
                this.board[3 + i, 0] = this.board[0, 3 + i] = Field.Attacker;
                this.board[3 + i, 10] = this.board[10, 3 + i] = Field.Attacker;
            }
            this.board[5, 1] = this.board[1, 5] = this.board[5, 9] = this.board[9, 5] = Field.Attacker;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this.board[4 + i, 4 + j] = Field.Protector;
                }
            }
            this.board[3, 5] = this.board[5, 3] = this.board[7, 5] = this.board[5, 7] = Field.Protector;

            this.board[5, 5] = Field.ThroneWithKing;

            this.Victory = false;
            this.BlackMove = true;
        }


        public bool isValidMove(int x, int y, int destx, int desty) {
			if ((board[x, y] == Field.Attacker && BlackMove) || 
	   				(!BlackMove && 
	     				(board[x, y] == Field.Protector
	                    || board[x, y] == Field.ThroneWithKing 
	                    || board[x, y] == Field.King)
	   				)
			   ) {
                bool allEmpty = true;
                bool kingDirectsToFinish = (board[destx, desty] == Field.Finish && board[x, y] == Field.King && !BlackMove);
                if (x == destx) {
					if (desty < y) {
						int temp = desty - 1;
						desty = y - 1;
						y = temp;
					}    
                    for (int i = y + 1; i <= desty; i++) {
						if (board[x, i] != Field.Empty && !kingDirectsToFinish) {
                            allEmpty = false;
						}
					}
				} else if (y == desty) {
					if (destx < x) {
						int temp = destx - 1;
						destx = x - 1;
						x = temp;
					}
					for (int i = x + 1; i <= destx; i++) {
						if (board[i, y] != Field.Empty && !kingDirectsToFinish) {
							allEmpty = false;
						}
					}
				}
                return allEmpty;
            }
			return false;
		}
		

		public void move(int x, int y, int destx, int desty) {
			Field piece;
			if (board[x, y] == Field.ThroneWithKing) {
				piece = Field.King;
				board[x, y] = Field.Throne;
			} else {
				piece = board[x, y];
				board[x, y] = Field.Empty;
			}
			if (piece == Field.King && board[destx, desty] == Field.Finish) {
                this.Victory = true;
			} else if (piece == Field.King && board[destx, desty] == Field.Throne) {
				board[destx, desty] = Field.ThroneWithKing;
				return;
           }
			board[destx, desty] = piece;
            evaluate(destx, desty);
		}
		

		private void evaluateKingCaptcher(int x, int y) {
			if (board[x, y] != Field.King) return;
			
			Field[] surrounders = {Field.Attacker, Field.Finish, Field.Throne};
			int numSurrounders = 0;
			if (x != 0 ) {
				if (surrounders.Contains(board[x-1, y])) numSurrounders++;
			} else numSurrounders++;
			if (x != 10 ) {
				if (surrounders.Contains(board[x+1, y])) numSurrounders++;
			} else numSurrounders++;
			if (y != 0 ) {
				if (surrounders.Contains(board[x, y-1])) numSurrounders++;
			} else numSurrounders++;
			if (y != 10 ) {
				if (surrounders.Contains(board[x, y+1])) numSurrounders++;
			} else numSurrounders++;
			
			if (numSurrounders == 4) {
                this.Victory = true;
		    }
		}
		

		private void evaluateCaptcher(int x, int y, int xHelper, int yHelper, Field[] helpers, Field enemy) {
			if (board[x, y] == enemy && helpers.Contains(board[xHelper, yHelper])) {
				board[x, y] = Field.Empty;
			}
		}
		

		private void evaluate(int x, int y) {
			if (BlackMove) {
				Field[] helpers = {Field.Attacker, Field.Throne, Field.Finish};
				if (x > 0) {
					evaluateKingCaptcher(x-1, y);
					if (x > 1) evaluateCaptcher(x-1, y, x-2, y, helpers, Field.Protector);
				}
				if (x < 10) {
					evaluateKingCaptcher(x+1, y);
					if (x < 9) evaluateCaptcher(x+1, y, x+2, y, helpers, Field.Protector);
				}
				if (y > 0) {
					evaluateKingCaptcher(x, y-1);
					if (y > 1) evaluateCaptcher(x, y-1, x, y-2, helpers, Field.Protector);
				}
				if (y < 10) {
					evaluateKingCaptcher(x, y+1);
					if (y < 9) evaluateCaptcher(x, y+1, x, y+2, helpers, Field.Protector);
				}
			} else {
				Field[] helpers = {Field.Protector, Field.Throne, Field.Finish, Field.King, Field.ThroneWithKing};
				if (x > 1) {
					 evaluateCaptcher(x-1, y, x-2, y, helpers, Field.Attacker);
				}
				if (x < 9) {
					evaluateCaptcher(x+1, y, x+2, y, helpers, Field.Attacker);
				}
				if (y > 1) {
					evaluateCaptcher(x, y-1, x, y-2, helpers, Field.Attacker);
				}
				if (y < 9) {
					evaluateCaptcher(x, y+1, x, y+2, helpers, Field.Attacker);
				}
			}
		}
		

		public void next() {
			this.BlackMove = !this.BlackMove;
		}
		

		private void drawSeperatorLine() {
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("+");
			for (int j = 0; j < 11; j++) {
				Console.Write("---+");
			}
			Console.WriteLine();
		}
		

		public void printBoard() {
			drawSeperatorLine();
			for (int i = 0; i < 11; i++) {
                Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("| ");
				for (int j = 0; j < 11; j++) {
                    this.printField(i, j, Selection.Normal);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write(" | ");	
				}
				Console.ForegroundColor = ConsoleColor.White;
				Console.Write((char)(65 + i));
				Console.Write("\n");
				drawSeperatorLine();
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("  ");
			for (int i = 0; i < 11; i++) {
				Console.Write(i + "   ");
			}
			Console.WriteLine();
		}

        
        public void printField(Position pos, Selection sel)
        {
            Console.CursorTop = 1 + 2 * pos.y;
            Console.CursorLeft = 2 + 4 * pos.x;

            this.printField(pos.x, pos.y, sel);
        }


        private void printField(int x, int y, Selection selection)
        {
            Console.ForegroundColor = ConsoleColor.White;
            switch (selection)
            {
                case Selection.Normal:
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case Selection.Hovered:
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case Selection.Selected:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
            }
            switch (this.board[x, y])
            {
                case Field.King:
                case Field.ThroneWithKing:
                    Console.Write('K');
                    break;
                case Field.Attacker:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write('O');
                    break;
                case Field.Protector:
                    Console.Write('O');
                    break;
                case Field.Empty:
                    Console.Write(' ');
                    break;
                case Field.Finish:
                case Field.Throne:
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (selection == Selection.Normal) Console.BackgroundColor = ConsoleColor.White;
                    Console.Write('X');                 
                    break;
            }
        }
	}
}
