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
                    bool result = HandleSDialogue(adjacent, level, out message, out unlockDoors);
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

        private static bool HandleSDialogue(Point guardPos, Level level, out string resultMessage, out bool unlockDoors)
        {
            resultMessage = "";
            unlockDoors = false;

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
                    PlayRockPaperScissors(guardPos, level);
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
                    PlayRockPaperScissors(guardPos, level);
                    return true;
                }
            }
        }

        private static bool PlayRockPaperScissors(Point guardPos, Level level)
        {
            string[] allowedSigns = { "papier", "kamien", "nozyce" };
            const string firstAllowedSign = "papier";
            const string secondAllowedSign = "kamien";
            const string thirdAllowedSign = "nozyce";
            Random random = new Random();

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Podaj znak ({string.Join("/", allowedSigns)}):");

                string firstSign = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;

                while (!allowedSigns.Contains(firstSign))
                {
                    Console.WriteLine("Nawet tego cię matka nie nauczyła?..");
                    Console.WriteLine($"Podaj POPRAWNY znak! ({string.Join("/", allowedSigns)}):");
                    firstSign = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;
                }

                string secondSign = allowedSigns[random.Next(allowedSigns.Length)];
                Console.WriteLine($"Strażnik wybrał: {secondSign}");

                if (firstSign == secondSign)
                {
                    Console.WriteLine("Remis! Szykuje się dogrywka! Nie wypuszczę Cię tak szybko!");
                    Console.ReadKey();
                }
                else if (
                    (firstSign == firstAllowedSign && secondSign == thirdAllowedSign) ||
                    (firstSign == secondAllowedSign && secondSign == firstAllowedSign) ||
                    (firstSign == thirdAllowedSign && secondSign == secondAllowedSign)
                )
                {
                    Console.WriteLine("W porządku, wygrałeś.. uciekaj, będę tęsknił...");
                    Console.ReadKey();
                    level.SetCellVisual(guardPos, '.');
                    Console.Clear();
                    return true;
                }
                else
                {
                    Console.WriteLine("No to chyba sobie tu postoisz. Gramy dalej!");
                    Console.ReadKey();
                    Console.Clear();
                    return false;
                }
            }
        }
    }
}