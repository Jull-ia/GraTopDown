using GameProject;

class Cell
{
    public char Visual { get; private set; }
    private Character occupant;
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
        Console.Write(occupant != null ? occupant.Symbol : Visual);
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