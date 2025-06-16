using System.Collections.Generic;

namespace GameProject
{
    class NPC : Character
    {
        private List<Point> path;
        private int currentIndex;
        private int direction = 1;

        public NPC(List<Point> path) : base('$')
        {
            this.path = path;
            currentIndex = 0;
        }

        public Point GetNextMove()
        {
            if (path.Count == 0)
                return new Point(0, 0); // lub rzuć wyjątek

            currentIndex += direction;

            if (currentIndex >= path.Count)
            {
                currentIndex = path.Count - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }

            return path[currentIndex];
        }

        public Point GetCurrentPosition()
        {
            return path[currentIndex];
        }
    }
}