using System;
using System.Collections.Generic;
using System.Threading;

namespace GameProject
{
    class Game
    {
        private Level level;
        private Point playerPosition;
        private Character player;

        private List<NPC> npcs = new();
        private Dictionary<NPC, Point> npcPositions = new();

        private Snake snake;
        private List<Point> snakePath;

        private string infoMessage = "";
        private DateTime messageShownTime = DateTime.MinValue;
        private const int messageDisplayDuration = 5000;

        public Game()
        {
            level = new Level();
            player = new Character('@');
            playerPosition = level.GetStartNearFirstTeleport(5);
            level.OccupyCell(playerPosition, player);

            InitNPCs();
            InitSnake();
            level.PlaceHealingPotions(2);
        }

        private void InitNPCs()
        {
            var path = new List<Point>();
            for (int y = 3; y <= 15; y++)
            {
                path.Add(new Point(14, y));
            }

            var npc = new NPC(path);
            npcs.Add(npc);
            npcPositions[npc] = path[0];
            level.OccupyCell(path[0], npc);
        }

        private void InitSnake()
        {
            snakePath = new List<Point>();

            // Prawo
            for (int x = 36; x <= 41; x++)
                snakePath.Add(new Point(x, 13));

            // Dół
            for (int y = 14; y <= 15; y++)
                snakePath.Add(new Point(41, y));

            // Lewo
            for (int x = 40; x >= 36; x--)
                snakePath.Add(new Point(x, 15));

            // Góra
            for (int y = 14; y >= 13; y--)
                snakePath.Add(new Point(36, y));

            snake = new Snake(snakePath);
        }

        public void Run()
        {
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
                        case ConsoleKey.H:
                            infoMessage = player.UseItem();
                            messageShownTime = DateTime.Now;
                            break;
                        case ConsoleKey.Q:
                            if (player.UseKey())
                            {
                                if (level.UseKey())
                                {
                                    infoMessage = "Użyto klucza, drzwi zostały otwarte.";
                                }
                                else
                                {
                                    infoMessage = "Nie można użyć klucza tutaj.";
                                    player.Inventory.Add('?');
                                }
                            }
                            else
                            {
                                infoMessage = "Nie masz klucza!";
                            }
                            messageShownTime = DateTime.Now;
                            break;

                    }

                    if (level.IsWalkable(newPosition.x, newPosition.y))
                    {
                        if (IsNpcAtPosition(newPosition) || IsSnakeAtPosition(newPosition))
                        {
                            HandlePlayerHit();
                        }
                        else
                        {
                            level.LeaveCell(playerPosition);
                            playerPosition = newPosition;

                            if (level.GetCellVisual(playerPosition) == 'o')
                                playerPosition = level.GetOtherTeleport(playerPosition);


                            if (level.GetCellVisual(playerPosition) == '8')
                            {
                                player.CollectHealingPotion();
                                level.OccupyCell(playerPosition, player); 
                                infoMessage = "Zebrałeś miksturę!";
                                messageShownTime = DateTime.Now;
                            }
                            else if (level.GetCellVisual(playerPosition) == '?')
                            {
                                player.AddItemToInventory('?');
                                 level.OccupyCell(playerPosition, player); 
                                 infoMessage = "Zebrałeś klucz!";
                                 messageShownTime = DateTime.Now;
                            }
                            else
                            {
                                level.OccupyCell(playerPosition, player);
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
                        HandlePlayerHit();

                    lastSnakeMoveTime = now;
                }

                Thread.Sleep(10);
            }

            Console.Clear();
            Console.WriteLine("KONIEC GRY. Straciłeś wszystkie życia.");
            Console.ReadKey();
        }

        private void MoveNPCs()
        {
            foreach (var npc in npcs)
            {
                Point current = npcPositions[npc];
                Point next = npc.GetNextMove();

                if (next.Equals(playerPosition))
                    HandlePlayerHit();

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

        private void HandlePlayerHit()
        {
            player.Lives.LoseLife();
            level.LeaveCell(playerPosition);
            playerPosition = level.GetStartNearFirstTeleport(5);
            level.OccupyCell(playerPosition, player);
        }
    }
}