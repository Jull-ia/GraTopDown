namespace GameProject
{
    class Snake
    {
        private List<Point> body;
        private int directionIndex;
        private List<Point> path;
        public Snake(List<Point> path)
        {
            this.path = path;
            this.body = new List<Point>();
            this.directionIndex = 0;

            // Bazyliszek składa się z 4 segmentów: ssss
            for (int i = 0; i < 4; i++)
            {
                int index = (path.Count + directionIndex - i) % path.Count;
                body.Add(path[index]);
            }
        }

        public void Move()
        {
            directionIndex = (directionIndex + 1) % path.Count;
            Point nextHead = path[directionIndex];

            body.Insert(0, nextHead);
            body.RemoveAt(body.Count - 1);
        }
        public Point GetHead()
                {
                    return body[0];
                }
        public List<Point> GetBody()
        {
            return new List<Point>(body);
        }
    }
}