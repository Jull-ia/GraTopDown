using System;
using System.Collections.Generic;
using System.Threading;
using GameProject;

namespace GameProject
{
    static class DialogueManager
    {
        private static HashSet<Point> talkedToPrisoners = new();

        public static bool TryTriggerDialogue(Point playerPos, Level level, out string message, out bool unlockDoors)
        {
            foreach (var offset in new[] {
                new Point(0, 1), new Point(0, -1),
                new Point(1, 0), new Point(-1, 0)
            })
            {
                var adjacent = new Point(playerPos.x + offset.x, playerPos.y + offset.y);
                char visual = level.GetCellVisual(adjacent);

                if (visual == 'P' && !talkedToPrisoners.Contains(adjacent))
                {
                    bool result = HandleDialogue(out message, out unlockDoors);
                    talkedToPrisoners.Add(adjacent);
                    return result;
                }
            }

            message = "";
            unlockDoors = false;
            return false;
        }

        private static bool HandleDialogue(out string resultMessage, out bool unlockDoors)
        {
            resultMessage = "";
            unlockDoors = false;

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
                    resultMessage = "Więzień powiedział, że drzwi są już otwarte!";
                    unlockDoors = true;

                    Console.WriteLine("\n[Wciśnij Enter aby kontynuować]");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    Console.Clear();
                    return true;
                }
                else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Console.WriteLine("Więzień: Dobra dobra, ale nie zużyj całej ciepłej wody!");
                    resultMessage = "Rozmowa zakończona bez efektu.";
                    unlockDoors = false;

                    Console.WriteLine("\n[Wciśnij Enter aby kontynuować]");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    Console.Clear();
                    return true;
                }
            }
        }
    }
}