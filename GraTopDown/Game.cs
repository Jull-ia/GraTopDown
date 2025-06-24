namespace GameProject
{
    public class Game
    {
        private Level level;
        private Point playerPosition;
        private Character player;
        private List<NPC> npcs = new();
        private Dictionary<NPC, Point> npcPositions = new();
        private Snake snake;
        private List<Point> snakePath  = new List<Point>();
        private string infoMessage = "";
        private DateTime messageShownTime = DateTime.MinValue;
        private const int messageDisplayDuration = 3000;
        private Point snakeStartRoomPosition;
        private readonly Point snakeHitReturnPoint = new Point(31, 14);
    
        public Game()
        {
            level = new Level();
            player = new Character('@');
            playerPosition = level.GetStartNearFirstTeleport(5);
            level.OccupyCell(playerPosition, player);

            InitNPCs();
            snake = InitSnake();
            snakeStartRoomPosition = snakePath[0];
            level.PlaceHealingPotions(2);
        }

        private void InitNPCs()
        {
            var path1 = new List<Point>();
            for (int y = 3; y <= 15; y++)
            {
                path1.Add(new Point(14, y));
            }

            var npc1 = new NPC(path1, '$');
            npcs.Add(npc1);
            npcPositions[npc1] = path1[0];
            level.OccupyCell(path1[0], npc1);

            var path2 = new List<Point>();
            for (int x = 28; x <= 34; x++)
            {
                path2.Add(new Point(x, 9));
            }

            var npc2 = new NPC(path2, '$');
            npcs.Add(npc2);
            npcPositions[npc2] = path2[0];
            level.OccupyCell(path2[0], npc2);
        }

        private Snake InitSnake()
        {
            for (int x = 36; x <= 41; x++)
                snakePath.Add(new Point(x, 13));

            for (int y = 14; y <= 15; y++)
                snakePath.Add(new Point(41, y));

            for (int x = 40; x >= 36; x--)
                snakePath.Add(new Point(x, 15));

            for (int y = 14; y >= 13; y--)
                snakePath.Add(new Point(36, y));

            return new Snake(snakePath);
        }

        public void Run()
        {
            ShowInfo();

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            Console.Clear();

            var lastNpcMoveTime = DateTime.Now;
            int npcMoveIntervalMs = 320;

            var lastSnakeMoveTime = DateTime.Now;
            int snakeMoveIntervalMs = 300;

            while (player.Lives.IsAlive)
            {
                Console.SetCursorPosition(0, 0);
                player.Lives.Display();
                Console.SetCursorPosition(0, 1);
                Console.WriteLine($"Mikstury: {player.GetPotionCount()}".PadRight(Console.WindowWidth));
                Console.WriteLine("Inwentarz: " + player.GetInventoryDisplay().PadRight(Console.WindowWidth));
                level.Display();

                if ((DateTime.Now - messageShownTime).TotalMilliseconds < messageDisplayDuration)
                    Console.WriteLine(infoMessage.PadRight(Console.WindowWidth));
                else
                    Console.WriteLine(new string(' ', Console.WindowWidth));

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    Point newPosition = playerPosition;

                    switch (key)
                    {
                        case ConsoleKey.W: newPosition.y -= 1; break;
                        case ConsoleKey.S: newPosition.y += 1; break;
                        case ConsoleKey.A: newPosition.x -= 1; break;
                        case ConsoleKey.D: newPosition.x += 1; break;
                        case ConsoleKey.Escape: return;
                        case ConsoleKey.H: // używanie mikstury leczenia
                            infoMessage = player.UseItem();
                            messageShownTime = DateTime.Now;
                            break;
                        case ConsoleKey.Q: //używanie kluczy tak jak w nazwie xD
                            if (level.UseKey(playerPosition, player))
                            {
                                infoMessage = "Użyto klucza, drzwi zostały otwarte.";
                            }
                            else
                            {
                                infoMessage = player.Inventory.Contains('?')
                                    ? "Nie można użyć klucza tutaj."
                                    : "Nie masz klucza!";
                            }

                            messageShownTime = DateTime.Now;
                            break;

                    }

                    if (level.IsWalkable(newPosition.x, newPosition.y)) //Kolizje z elementami
                    {
                        if (IsNpcAtPosition(newPosition))
                        {
                            HandlePlayerHit(false);
                            infoMessage = "Zostałeś złapany przez strażnika!";
                            messageShownTime = DateTime.Now;
                        }
                        else if (IsSnakeAtPosition(newPosition))
                        {
                            HandlePlayerHit(true);
                            infoMessage = "Zostałeś złapany przez Bazyliszka!";
                            messageShownTime = DateTime.Now;
                        }

                        else
                        {
                            level.LeaveCell(playerPosition);
                            playerPosition = newPosition;

                            if (level.GetCellVisual(playerPosition) == 'o')
                                playerPosition = level.GetOtherTeleport(playerPosition);


                            char cell = level.GetCellVisual(playerPosition);

                            if (cell == 'M') //Wygrana gracza (META)
                            {
                                Console.Clear();
                                Console.WriteLine("Gratulacje! Udało ci się uciec!");
                                Console.WriteLine("[Wciśnij ENTER aby wyjść z gry..]");
                                while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
                                return;

                            }

                            bool wasPotion = (cell == '8');
                            bool wasKey = (cell == '?');


                            if (wasPotion)
                            {
                                infoMessage = "Zebrałeś miksturę! [Naciśnij H aby jej użyć.]";
                                messageShownTime = DateTime.Now;

                            }

                            else if (wasKey)
                            {
                                player.AddItemToInventory('?');
                                infoMessage = "Zebrałeś klucz! [Naciśnij Q aby go użyć.]";
                                messageShownTime = DateTime.Now;
                                level.SetCellVisual(playerPosition, '.');


                            }
                            level.OccupyCell(playerPosition, player);
                            if (DialogueManager.TryTriggerDialogue(playerPosition, level, out string msg, out bool unlockDoors))
                            {
                                infoMessage = "Rozmowa z więźniem zakończona.";
                                messageShownTime = DateTime.Now;

                                if (unlockDoors)
                                    level.OpenSpecificDoor();
                            }
                        }
                    }
                }

                DateTime now = DateTime.Now;

                if ((now - lastNpcMoveTime).TotalMilliseconds >= npcMoveIntervalMs)
                {
                    MoveNPCs();
                    lastNpcMoveTime = now;
                }

                if ((now - lastSnakeMoveTime).TotalMilliseconds >= snakeMoveIntervalMs)
                {
                    level.ClearSnake(snake.GetBody());
                    snake.Move();
                    level.DrawSnake(snake.GetBody());

                    if (IsSnakeAtPosition(playerPosition))
                        HandlePlayerHit(true);

                    lastSnakeMoveTime = now;
                }

                Thread.Sleep(10);
            }

            Console.Clear();
            Console.WriteLine("KONIEC GRY. Straciłeś wszystkie życia.");
            Console.WriteLine("[Wciśnij ENTER aby wyjść z gry..]");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            return;

        }

        private static void ShowInfo()
        {
            // Wstęp do gry
            Console.Title = "Prison Escape";
            Console.Clear();
            Console.WriteLine("Witaj w Prison Escape!");
            Console.WriteLine();
            Console.WriteLine("Twoim celem jest ucieczka z więzienia. Szukaj kluczy do wyjścia, unikaj strażników i nie daj się zauważyć!");
            Console.WriteLine("Od miesięcy planowałeś ten podkop. Znasz to miejsce na wylot. Droga nie jest długa, lecz nie jest łatwa.");
            Console.WriteLine("Musisz wreszcie uciec z tego miejsca!");
            Console.WriteLine();

            Console.WriteLine("Sterowanie:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" W - Góra\n S - Dół\n A - Lewo\n D - Prawo");
            Console.WriteLine();
            Console.WriteLine(" Q - Użycie klucza\n H - Leczenie");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("[Wciśnij ENTER aby rozpocząć grę...]");
        }

        private void MoveNPCs()
        {
            foreach (var npc in npcs)
            {
                Point current = npcPositions[npc];
                Point next = npc.GetNextMove();

                if (next.Equals(playerPosition))
                    HandlePlayerHit(false);

                if (level.IsWalkable(next.x, next.y) && !npcPositions.ContainsValue(next))
                {
                    level.LeaveCell(current);
                    npcPositions[npc] = next;
                    level.OccupyCell(next, npc);
                }
            }
        }

        private bool IsNpcAtPosition(Point pos)
        {
            foreach (var npcPos in npcPositions.Values)
                if (npcPos.Equals(pos)) return true;
            return false;
        }

        private bool IsSnakeAtPosition(Point pos)
        {
            foreach (var segment in snake.GetBody())
                if (segment.Equals(pos)) return true;
            return false;
        }

        private void HandlePlayerHit(bool hitBySnake = false)
        {
            player.Lives.LoseLife();
            level.LeaveCell(playerPosition);

            if (hitBySnake)
            {
                playerPosition = snakeHitReturnPoint;
            }
            else
            {
                playerPosition = level.GetStartNearFirstTeleport(5);
            }
            level.OccupyCell(playerPosition, player);
        }
    }
}
