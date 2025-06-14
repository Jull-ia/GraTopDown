// using System;
// using System.Collections.Generic;
// using System.Threading;

// class Program
// {
//     static void Main()
//     {
//         Console.CursorVisible = false;

//         Game game = new Game();
//         game.Run();
//     }
// }

// class Game
// {
//     private Level level;
//     private Point playerPosition;
//     private Character player;

//     public Game()
//     {
//         level = new Level();
//         player = new Character('@');
//         playerPosition = level.GetStartNearFirstTeleport(5); // teraz poprawnie przed Occupy
//         level.OccupyCell(playerPosition, player);
//     }

//     public void Run()
//     {
//         while (true)
//         {
//             Console.SetCursorPosition(0, 0);
//             level.Display();

//             if (Console.KeyAvailable)
//             {
//                 ConsoleKey key = Console.ReadKey(true).Key;
//                 Point newPosition = playerPosition;

//                 switch (key)
//                 {
//                     case ConsoleKey.W:
//                         newPosition.y -= 1;
//                         break;
//                     case ConsoleKey.S:
//                         newPosition.y += 1;
//                         break;
//                     case ConsoleKey.A:
//                         newPosition.x -= 1;
//                         break;
//                     case ConsoleKey.D:
//                         newPosition.x += 1;
//                         break;
//                     case ConsoleKey.Escape:
//                         return;
//                 }

//                 if (level.IsWalkable(newPosition.x, newPosition.y))
//                 {
//                     level.LeaveCell(playerPosition);
//                     playerPosition = newPosition;

//                     // teleportacja
//                     if (level.GetCellVisual(playerPosition) == 'o')
//                     {
//                         playerPosition = level.GetOtherTeleport(playerPosition);
//                     }

//                     level.OccupyCell(playerPosition, player);
//                 }
//             }

//             Thread.Sleep(50); // Odświeżanie
//         }
//     }
// }

// class Level
// {
//     private string[] levelVisuals = new string[]
//     {

//             "##################",
//             "#....==....#.....#",
//             "#........../.....#",
//             "#..........#.....#",
//             "############.....#",
//             "#....==....#.....#",
//             "#........../.....#",
//             "#..........#.....#---------#########",
//             "############.....#---------#..[!]..#",
//             "#....==....#.....#---------#.......#",
//             "#o........./.....#---------#.......#",
//             "#..........#.....#-------####/###############",
//             "############.....#########.......#..........######",
//             "#........................#.......#..........#....#",
//             "#o......................./......./........../..../",
//             "#........................#.......#..........#....#",
//             "##########################.......#..........######",
//             "-------------------------####/#########/#####",
//             "-------------------------#.....#----#......#",
//             "-------------------------#.....#----#......#",
//             "-------------------------#.....#----########",
//             "-------------------------#.....#",
//             "-------------------------#.....#",
//             "-------------------------#######",

//         };
   

//     private Cell[][] levelData;
//     private List<Point> teleportPoints = new();

//     public Level()
//     {
//         levelData = new Cell[levelVisuals.Length][];
//         for (int y = 0; y < levelVisuals.Length; y++)
//         {
//             string row = levelVisuals[y];
//             Cell[] dataRow = new Cell[row.Length];
//             for (int x = 0; x < row.Length; x++)
//             {
//                 char symbol = row[x];
//                 if (symbol == 'o')
//                 {
//                     teleportPoints.Add(new Point(x, y));
//                 }
//                 dataRow[x] = new Cell(symbol, x, y);
//             }
//             levelData[y] = dataRow;
//         }
//     }

//     public void Display()
//     {
//         for (int y = 0; y < levelData.Length; y++)
//         {
//             for (int x = 0; x < levelData[y].Length; x++)
//             {
//                 levelData[y][x].Display();
//             }
//             Console.WriteLine();
//         }
//     }

//     public bool IsWalkable(int x, int y)
//     {
//         return y >= 0 && y < levelData.Length
//             && x >= 0 && x < levelData[y].Length
//             && levelData[y][x].Visual != '#'
//             && levelData[y][x].Visual != '/'; // blokuj drzwi
//     }

//     public void OccupyCell(Point pos, Character character)
//     {
//         levelData[pos.y][pos.x].Occupy(character);
//     }

//     public void LeaveCell(Point pos)
//     {
//         levelData[pos.y][pos.x].Leave();
//     }

//     public char GetCellVisual(Point pos)
//     {
//         return levelData[pos.y][pos.x].Visual;
//     }

//     public Point GetStartNearFirstTeleport(int offsetX)
//     {
//         if (teleportPoints.Count < 1)
//             throw new InvalidOperationException("No teleport points found.");

//         Point first = teleportPoints[0];
//         return new Point(first.x + offsetX, first.y);
//     }

//     public Point GetOtherTeleport(Point current)
//     {
//         foreach (var point in teleportPoints)
//         {
//             if (!(point.x == current.x && point.y == current.y))
//                 return point;
//         }
//         return current;
//     }
// }

// class Cell
// {
//     public char Visual { get; private set; }
//     private Character occupant;
//     public int X { get; }
//     public int Y { get; }

//     public Cell(char visual, int x, int y)
//     {
//         Visual = visual;
//         X = x;
//         Y = y;
//     }

//     public void Display()
//     {
//         Console.Write(occupant != null ? occupant.Symbol : Visual);
//     }

//     public void Occupy(Character character)
//     {
//         occupant = character;
//     }

//     public void Leave()
//     {
//         occupant = null;
//     }
// }

// class Character
// {
//     public char Symbol { get; }

//     public Character(char symbol)
//     {
//         Symbol = symbol;
//     }
// }

// struct Point
// {
//     public int x;
//     public int y;

//     public Point(int x, int y)
//     {
//         this.x = x;
//         this.y = y;
//     }
// }
