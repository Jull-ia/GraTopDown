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
        private Lives lives;

        private List<NPC> npcs = new List<NPC>();
        private Dictionary<NPC, Point> npcPositions = new Dictionary<NPC, Point>();

        public Game()
        {
            level = new Level();
            player = new Character('@');
            lives = new Lives(3);
            playerPosition = level.GetStartNearFirstTeleport(5);
            level.OccupyCell(playerPosition, player);

            InitNPCs();
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

            while (lives.IsAlive)
            {
                Console.SetCursorPosition(0, 0);
                level.Display();

                // Wyświetl życia
                Console.WriteLine($"Życia: {lives.Current}");

                // --- Gracz ---
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
                    }

                    if (level.IsWalkable(newPosition.x, newPosition.y))
                    {
                        // Sprawdź, czy wchodzisz na NPC
                        if (IsNpcAtPosition(newPosition))
                        {
                            HandlePlayerHit();
                        }
                        else
                        {
                            level.LeaveCell(playerPosition);
                            playerPosition = newPosition;

                            if (level.GetCellVisual(playerPosition) == 'o')
                            {
                                playerPosition = level.GetOtherTeleport(playerPosition);
                            }

                            level.OccupyCell(playerPosition, player);
                        }
                    }
                }

                // --- NPC ruch ---
                var now = DateTime.Now;
                if ((now - lastNpcMoveTime).TotalMilliseconds >= npcMoveIntervalMs)
                {
                    MoveNPCs();
                    lastNpcMoveTime = now;
                }

                Thread.Sleep(10);
            }

            // --- Koniec gry ---
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

                // Jeśli NPC wejdzie na gracza
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
            lives.LoseLife();
            level.LeaveCell(playerPosition);
            playerPosition = level.GetStartNearFirstTeleport(5);
            level.OccupyCell(playerPosition, player);
        }
    }
}