using GameProject;

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

    public void Display()
    {
        if (occupant?.Symbol == '@')
            Console.ForegroundColor = ConsoleColor.Magenta;
        if(Visual == 'T')
            Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(occupant != null ? occupant.Symbol : Visual);
        Console.ResetColor();
    }

    public void Occupy(Character character)
    {
        occupant = character;
    }

    public void Leave()
    {
        occupant = null;
    }
}