using System;
using System.Threading;


namespace GameProject
{
    class Game
    {
        private Level level;
        private Point playerPosition;
        private Character player;

        public Game()
        {
            level = new Level();
            player = new Character('@');
            playerPosition = level.GetStartNearFirstTeleport(5);
            level.OccupyCell(playerPosition, player);
        }

        public void Run()
        {
            while (true)
            {
               
                Console.SetCursorPosition(0, 0);
                level.Display();

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    Point newPosition = playerPosition;

                    switch (key)
                    {
                        case ConsoleKey.W: newPosition.y -= 1; break;
                        case ConsoleKey.S: newPosition.y += 1; break;
                        case ConsoleKey.A: newPosition.x -= 1; break; //zmien na dictionary
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

                Thread.Sleep(50);
            }
        }
    }
}