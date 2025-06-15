using GameProject;
class Level
{
    private string[] levelVisuals = new string[]
    {

            "##################",
            "#.P..==....#.....#",
            "#..........|.....#",
            "#..........#.....#",
            "############.....#",
            "#...P==....#.....#",
            "#..........|.....#",
            "#..........#.....#---------#########",
            "############.....#---------#..[!]..#",
            "#....==....#.....#---------#.......###########################################",
            "#o.........|.....#---------#.......#--------------T--------------------------#",
            "#..........#.....#-------####/###############------------T------------T------#",
            "############.....#########.......#..........######---------------------------#",
            "#........................#.......#..........#....#-------------T-------------#",
            "#o......................./.......|........../....|------------------------M--#",
            "#........................#.......#..........#....#---------------------------#",
            "##########################.......#..........######------T------------T-------#",
            "-------------------------####/#########_#####--------------------------------#",
            "-------------------------#.....#----#......#--------------------T------------#",
            "-------------------------##...##----#....*.###################################",
            "-------------------------#....P#----########",
            "-------------------------##...##",
            "-------------------------#P....#",
            "-------------------------#######",

    };

    private Dictionary<Point, Point> keysAndDoors = new Dictionary<Point, Point>()
    { 
        {new Point(31, 8), new Point(33, 14) }
    };
    private Cell[][] levelData;
    private List<Point> teleportPoints = new();

    public Level()
    {
        levelData = new Cell[levelVisuals.Length][];
        for (int y = 0; y < levelVisuals.Length; y++)
        {
            string row = levelVisuals[y];
            Cell[] dataRow = new Cell[row.Length];
            for (int x = 0; x < row.Length; x++)
            {
                char symbol = row[x];
                if (symbol == 'o')
                {
                    teleportPoints.Add(new Point(x, y));
                }
                dataRow[x] = new Cell(symbol, x, y);
            }
            levelData[y] = dataRow;
        }
    }

    public void Display()
    {
        for (int y = 0; y < levelData.Length; y++)
        {
            for (int x = 0; x < levelData[y].Length; x++)
            {
                levelData[y][x].Display();
            }
            Console.WriteLine();
        }
    }

    public bool IsWalkable(int x, int y)
    {
        return y >= 0 && y < levelData.Length
            && x >= 0 && x < levelData[y].Length
            && levelData[y][x].Visual != '#'
            && levelData[y][x].Visual != 'T' // no nie można przez drzewa
            && levelData[y][x].Visual != '_'  // zablkowane drzwi poziom
            && levelData[y][x].Visual != '|'; // zablkowane drzwi pion


    }


    private void OpenDoors(Point pos)
    {
        //for (int y = 0; y < levelData.Length; y++)
        //{
        //for (int x = 0; x < levelData[y].Length; x++)
        //{
        //if (levelData[y][x].Visual == '|' || levelData[y][x].Visual == '_')
        //{
        if(keysAndDoors.TryGetValue(pos, out Point doorPosition))
            levelData[doorPosition.y][doorPosition.x].Visual = '/'; 
               // }
            //}
        //}
        //1.) 8,31 -> 14, 33
    }


    public void OccupyCell(Point pos, Character character)
    {
        char current = levelData[pos.y][pos.x].Visual;

        // Jeśli gracz wchodzi na ! lub * = otworz drzwi
        if (current == '!' || current == '*')
        {
            OpenDoors(pos);
        }

        levelData[pos.y][pos.x].Occupy(character);
    }

    public void LeaveCell(Point pos)
    {
        levelData[pos.y][pos.x].Leave();
    }

    public char GetCellVisual(Point pos)
    {
        return levelData[pos.y][pos.x].Visual;
    }

    public Point GetStartNearFirstTeleport(int offsetX)
    {
        if (teleportPoints.Count < 1)
            throw new InvalidOperationException("No teleport points found.");

        Point first = teleportPoints[0];
        return new Point(first.x + offsetX, first.y);
    }

    public Point GetOtherTeleport(Point current)
    {
        foreach (var point in teleportPoints)
        {
            if (!(point.x == current.x && point.y == current.y))
                return point;
        }
        return current;
    }
}