// Console.CursorVisible = false;
// Player hero = new Player("Snake");

// Console.WriteLine($"({hero.x}, {hero.y})");


// while (true)
// {
//     ConsoleKeyInfo pressedKeyInfo = Console.ReadKey(true);

//     if (pressedKeyInfo.Key == ConsoleKey.A)
//     {
//         hero.x -= 1;
//     }
//     if (pressedKeyInfo.Key == ConsoleKey.D)
//     {
//         hero.x += 1;
//     }
//     if (pressedKeyInfo.Key == ConsoleKey.W)
//     {
//         hero.y -= 1;
//     }
//     if (pressedKeyInfo.Key == ConsoleKey.S)
//     {
//         hero.y += 1;
//     }

//     Console.SetCursorPosition(0, 0);
//     Console.WriteLine($"({hero.x}, {hero.y})");

//     Console.SetCursorPosition(hero.x, hero.y);
//     Console.Write("@");
// }

// Console.WriteLine("Press Space to continue...");
// ConsoleKeyInfo consoleKeyInfo;

// do
// {
//     consoleKeyInfo = Console.ReadKey(true);
// } while (consoleKeyInfo.Key != ConsoleKey.Spacebar);


using System.Drawing;

Console.CursorVisible = false;

Dictionary<ConsoleKey, string> keyActionMap = new Dictionary<ConsoleKey, string>();
keyActionMap.Add(ConsoleKey.A, "moveLeft");
keyActionMap.Add(ConsoleKey.D, "moveRight");
keyActionMap.Add(ConsoleKey.W, "moveUp");
keyActionMap.Add(ConsoleKey.S, "moveDown");
keyActionMap.Add(ConsoleKey.Escape, "quitGame");

Dictionary<string, Point> directionsMap = new Dictionary<string, Point>();
directionsMap.Add("moveLeft", new Point(-1, 0));
directionsMap.Add("moveRight", new Point(1, 0));
directionsMap.Add("moveUp", new Point(0, -1));
directionsMap.Add("moveDown", new Point(0, 1));

Point startingPoint = new Point(3, 10);


Player hero = new Player("Prisoner", "@", startingPoint, keyActionMap);
hero.speed = 1;
characters.Add(hero);
