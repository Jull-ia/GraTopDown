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

        private List<NPC> npcs = new List<NPC>();
        private Dictionary<NPC, Point> npcPositions = new Dictionary<NPC, Point>();

        public Game()
        {
            level = new Level();
            player = new Character('@');
            playerPosition = level.GetStartNearFirstTeleport(5);
            level.OccupyCell(playerPosition, player);

            InitNPCs();
        }

        private void InitNPCs()
        {
            var path = new List<Point>
                {
                new Point(14, 3),
                new Point(14, 4),
                new Point(14, 5),
                new Point(14, 6),
                new Point(14, 7),
                new Point(14, 8),
                new Point(14, 9),
                new Point(14, 10),
                new Point(14, 11)
            };

            var npc = new NPC(path);
            npcs.Add(npc);
            npcPositions[npc] = path[0];
            level.OccupyCell(path[0], npc);
        }

        public void Run()
        {
            while (true)
            {
                Console.SetCursorPosition(0, 0);

                MoveNPCs(); // <- porusz NPC

                level.Display();

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
                        level.LeaveCell(playerPosition);
                        playerPosition = newPosition;

                        if (level.GetCellVisual(playerPosition) == 'o')
                        {
                            playerPosition = level.GetOtherTeleport(playerPosition);
                        }

                        level.OccupyCell(playerPosition, player);
                    }
                }

                Thread.Sleep(10);  //prędkość npc
            }
        }

        private void MoveNPCs()
        {
            foreach (var npc in npcs)
            {
                Point current = npcPositions[npc];
                Point next = npc.GetNextMove();

                if (level.IsWalkable(next.x, next.y) && !npcPositions.ContainsValue(next))
                {
                    level.LeaveCell(current);
                    npcPositions[npc] = next;
                    level.OccupyCell(next, npc);
                }
            }
        }
    }
}

