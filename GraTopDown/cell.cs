namespace GameProject
{
    class Cell
    {
        public char Visual { get; set; }
        private Character? occupant;
        public int X { get; }
        public int Y { get; }

        public Cell(char visual, int x, int y)
        {
            Visual = visual;
            X = x;
            Y = y;
        }

        public void Display()  // kolorowanie elementów
        {
            if (occupant?.Symbol == '@')
                Console.ForegroundColor = ConsoleColor.Magenta;

            if (occupant?.Symbol == '$')
                Console.ForegroundColor = ConsoleColor.Red;

            if (Visual == 'T')
                Console.ForegroundColor = ConsoleColor.Green;

            if (Visual == '8')
                Console.ForegroundColor = ConsoleColor.Cyan;

            if (Visual == 'M')
                Console.ForegroundColor = ConsoleColor.Yellow;

            if (Visual == 'o')
                Console.ForegroundColor = ConsoleColor.DarkGray;

            if (Visual == '/')
                Console.ForegroundColor = ConsoleColor.DarkYellow;

            if (Visual == '-')
                Console.ForegroundColor = ConsoleColor.DarkGreen;

            if (Visual == '=')
                Console.ForegroundColor = ConsoleColor.DarkMagenta;

            if (Visual == '?')
                Console.ForegroundColor = ConsoleColor.Blue;

            if (Visual == 's')
                Console.ForegroundColor = ConsoleColor.Red;

            if (Visual == 'P')
                Console.ForegroundColor = ConsoleColor.Magenta;

            if (Visual == 'S')
                Console.ForegroundColor = ConsoleColor.DarkRed;

            Console.Write(occupant != null ? occupant.Symbol : Visual);
            Console.ResetColor();
        }


        public void Occupy(Character character)
        {
            occupant = character;
        }

        public bool IsOccupied()
        {
            return occupant != null;
        }
        public void Leave()
        {
            occupant = null;
        }

    }
}