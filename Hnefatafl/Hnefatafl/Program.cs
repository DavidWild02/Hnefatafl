using System;
using System.Collections;
using System.Linq;

namespace Hnefatafl
{
    public struct Position
    {
        public int x;
        public int y;
    }

    class Program
	{
        public static void Main(string[] args) {
            Position cursor = new Position();
            Game game = new Game();

            game.printBoard();

            while (true)
            {
                cursor.x = cursor.y = 5;
                game.printField(cursor, Selection.Hovered);
                Position? source = null, dest = null;

                // Getting the move input
                while (true)
                {
                    ConsoleKey key = HiddenReader.readKey().Key;
                    if (key == ConsoleKey.Enter)
                    {
                        if (source == null)
                        {
                            source = cursor;
                            Console.CursorLeft--;
                            game.printField(cursor, Selection.Selected);
                        }
                        else if (source.Equals(cursor))
                        {
                            source = null;
                            Console.CursorLeft--;
                            game.printField(cursor, Selection.Hovered);
                        }
                        else if (dest == null)
                        {
                            dest = cursor;
                            break;
                        }
                    }
                    else
                    {
                        Position cPrevPos = cursor;
                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                if (cursor.y == 0) continue;
                                cursor.y--;
                                break;
                            case ConsoleKey.DownArrow:
                                if (cursor.y == 10) continue;
                                cursor.y++;
                                break;
                            case ConsoleKey.LeftArrow:
                                if (cursor.x == 0) continue;
                                cursor.x--;
                                break;
                            case ConsoleKey.RightArrow:
                                if (cursor.x == 10) continue;
                                cursor.x++;
                                break;
                            default:
                                continue;
                        }
                        
                        if (source != null)
                        {
                            game.printField(cPrevPos, (source.Equals(cPrevPos)) ? Selection.Selected : Selection.Normal);
                            game.printField(cursor, (source.Equals(cursor)) ? Selection.Selected : Selection.Hovered);
                        } else
                        {
                            game.printField(cPrevPos, Selection.Normal);
                            game.printField(cursor, Selection.Hovered);
                        }
                    }
                }

                Position source_ = source.Value, dest_ = dest.Value;


                // Validating the Move
                if (!game.isValidMove(source_.x, source_.y, dest_.x, dest_.y))
                {
                    game.printField(source_, Selection.Normal);
                    game.printField(dest_, Selection.Normal);
                    continue;
                }

                // Executing and Evaluating
                game.move(source_.x, source_.y, dest_.x, dest_.y);

                game.printField(source_, Selection.Normal);
                game.printField(dest_, Selection.Normal);

                if (game.Victory)
                {
                    Console.CursorTop = 22;
                    Console.CursorLeft = 0;
                    Console.WriteLine("\n\n");
                    Console.WriteLine(game.BlackMove ? "The King gets killed!\nThe Attackers win!" : "The King escapes!\nThe Defenders win!");
                    break;
                }

                game.next();
            }

			Console.ReadKey(true);
		}
	}
}
