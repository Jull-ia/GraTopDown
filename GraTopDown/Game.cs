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
            level.PlaceHealingPotions(2); // mikstury
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

        public void Run()
        {
            var lastNpcMoveTime = DateTime.Now;
            int npcMoveIntervalMs = 320;

            while (player.Lives.IsAlive)
            {
                Console.SetCursorPosition(0, 0);
                player.Lives.Display();
                player.DisplayPotionCount(); // Pokaż liczbę mikstur
                level.Display();

                if ((DateTime.Now - messageShownTime).TotalMilliseconds < messageDisplayDuration)
                {
                    Console.WriteLine(infoMessage.PadRight(Console.WindowWidth));
                }
                else
                {
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                }

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
                        case ConsoleKey.Q:
                            infoMessage = player.UseItem();
                            messageShownTime = DateTime.Now;
                            break;
                        case ConsoleKey.Escape: return;
                    }

                    if (level.IsWalkable(newPosition.x, newPosition.y))
                    {
                        if (IsNpcAtPosition(newPosition))
                        {
                            HandlePlayerHit();
                        }
                        else
                        {
                            level.LeaveCell(playerPosition);
                            playerPosition = newPosition;

                            // Sprawdź teleport
                            if (level.GetCellVisual(playerPosition) == 'o')
                            {
                                playerPosition = level.GetOtherTeleport(playerPosition);
                            }

                            // Sprawdź zebranie mikstury
                            if (level.CollectHealingPotion(playerPosition))
                            {
                                player.AddPotion();
                                infoMessage = "Zebrano miksturę!";
                                messageShownTime = DateTime.Now;
                            }

                            level.OccupyCell(playerPosition, player);
                        }
                    }
                }

                var now = DateTime.Now;
                if ((now - lastNpcMoveTime).TotalMilliseconds >= npcMoveIntervalMs)
                {
                    MoveNPCs();
                    lastNpcMoveTime = now;
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
                {
                    HandlePlayerHit();
                }

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
            {
                if (npcPos.Equals(pos))
                    return true;
            }
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