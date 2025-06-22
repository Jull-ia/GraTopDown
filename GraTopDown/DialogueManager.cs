using System;
using System.Collections.Generic;
using System.Threading;
using GameProject;

namespace GameProject
{
    static class DialogueManager
    {
        private static HashSet<Point> talkedToPrisoners = new();

        private static HashSet<Point> talkedToS = new();

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
                    bool result = HandlePrisonerDialogue(out message, out unlockDoors);
                    talkedToPrisoners.Add(adjacent);
                    return result;
                }

                if (visual == 'S' && !talkedToS.Contains(adjacent))
                {
                    bool result = HandleSDialogue(out message, out unlockDoors);
                    talkedToS.Add(adjacent);
                    return result;
                }
            }

            message = "";
            unlockDoors = false;
            return false;
        }

        private static bool HandlePrisonerDialogue(out string resultMessage, out bool unlockDoors)
        {
            resultMessage = "";
            unlockDoors = true;

            Console.Clear();
            Console.WriteLine("Więzień: To chyba nie twoja kolej na branie prysznica...");
            Console.WriteLine("1 - Uciekam stąd, wiesz gdzie znajdę klucz do bocznych drzwi?");
            Console.WriteLine("2 - Taa taa... wiesz może kiedy te boczne drzwi będą dla nas otwarte?");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    Console.WriteLine("Więzień: HAHA, Ty i te twoje pomysły. Jakieś klucze na pewno będą w biurze Strażnika, ale nie wiesz tego ode mnie... powodzenia młody.");
                    unlockDoors = true;

                    Console.WriteLine("\n[Wciśnij Enter aby kontynuować]");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    Console.Clear();
                    return true;
                }
                else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Console.WriteLine("A skąd ja mam to wiedzieć? Chociaż słyszałem, że jeden ze strażników tak często gubił klucze, że w biurze jest zawsze zapasowa para. Nie wiesz tego ode mnie...");
                    unlockDoors = true;

                    Console.WriteLine("\n[Wciśnij Enter aby kontynuować]");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    Console.Clear();
                    return true;
                }
            }
        }

        private static bool HandleSDialogue(out string resultMessage, out bool unlockDoors)
        {
            resultMessage = "";
            unlockDoors = false; // na przykład, ten NPC nie odblokowuje drzwi

            Console.Clear();
            Console.WriteLine("Strażnik kumpel: Co ty tu robisz? Wiesz, że nie powinno cię tu być.");
            Console.WriteLine("1 - Uciekam stąd. Pozwól mi wyjść przyjacielu.");
            Console.WriteLine("2 - Proszę przesuń się, nie mam zbyt wiele czasu.");

            while (true)
            {
                var key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    Console.WriteLine("Strażnik: Wiesz, że nie powinienem... No dobrze, puszczę cię, ale tylko jak wygrasz ze mną w papier kamień nożyce!");
                    unlockDoors = false;

                    Console.WriteLine("\n[Wciśnij Enter aby kontynuować]");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

                    Console.Clear();
                    return true;
                }
                else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Console.WriteLine("Strażnik: Zagraj ze mną w papier kamień nożyce a zrobię o co prosisz.");
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