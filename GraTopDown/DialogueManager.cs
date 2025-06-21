using System;
using System.Collections.Generic;
using System.Threading;
using GameProject;

namespace GameProject
{
    static class DialogueManager
    {
        private static HashSet<Point> talkedToPrisoners = new();

        public static bool TryTriggerDialogue(Point playerPos, Level level)
        {
            foreach (var offset in new[]
            {
                new Point(0, 1), new Point(0, -1),
                new Point(1, 0), new Point(-1, 0)
            })
            {
                var adjacent = new Point(playerPos.x + offset.x, playerPos.y + offset.y);
                char visual = level.GetCellVisual(adjacent);

                if (visual == 'P' && !talkedToPrisoners.Contains(adjacent))
                {
                    bool result = HandleDialogue();
                    talkedToPrisoners.Add(adjacent);
                    return result;
                }
            }

            return false;
        }

        private static bool HandleDialogue()
        {
            Console.Clear();
            Console.WriteLine("Więzień: To chyba nie twoja kolej na branie prysznica...");
            Console.WriteLine("1 - Uciekam stąd, wiesz gdzie znajdę klucz do drzwi?");
            Console.WriteLine("2 - Co ty możesz wiedzieć, teraz ja się myję!");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    Console.WriteLine("Więzień: HAHA, Ty i te twoje pomysły. Jakiekolwiek klucze na pewno będą w biurze Strażnika.");
                    Console.WriteLine("\n[Wciśnij Enter aby kontynuować]");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    Console.Clear();
                    return true;
                }
                else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Console.WriteLine("Więzień: Dobra dobra, ale nie zużyj całej ciepłej wody!");
                    Console.WriteLine("\n[Wciśnij Enter aby kontynuować]");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    Console.Clear();
                    return true;
                }
            }
        }
    }
}