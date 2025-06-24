namespace GameProject
{
    class Level
    {
        private string[] levelVisuals = new string[]
        {

            "##################",
            "#....==....#.....#",
            "#..........|.....#",
            "#..........#.....#",
            "############.....#",
            "#....==....#.....#",
            "#..........|.....#",
            "#..........#.....#---------#########",
            "############.....#---------#..[?]..#",
            "#....==....#.....#---------#.......###########################################",
            "#o.........|.....#---------#.......#--------------T--------------------------#",
            "#..........#.....#-------####_###############------------T------------T------#",
            "############.....#########.......#..........######---------------------------#",
            "#........................#.......#..........#....#-------------T-------------#",
            "#o......................./.......|.....?....|...S/------------------------M--#",
            "#........................#.......#..........#....#---------------------------#",
            "##########################.......#..........######------T------------T-------#",
            "-------------------------####/#########_#####--------------------------------#",
            "-------------------------#.....#----#......#--------------------T------------#",
            "-------------------------##...##----#....?.###################################",
            "-------------------------#.....#----########",
            "-------------------------##...##",
            "-------------------------#.....#",
            "-------------------------#######",
        };

        private Dictionary<Point, Point> keysAndDoors = new() //Pary kluczy i drzwi
        {
            { new Point(31, 8), new Point(33, 14) }, //klucz 1 / drzwi 1 
            
            {new Point(39,14), new Point(39,17) }, //klucz 2 / drzwi 2

            {new Point(41,19), new Point(44,14) }, //klucz 3 / drzwi 3
        };

        private Cell[][] levelData;
        private List<Point> teleportPoints = new();

        private int unlockedDoorsCount = 0;

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
            PlacePrisoners();
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
                && levelData[y][x].Visual != 'T'
                && levelData[y][x].Visual != '_'
                && levelData[y][x].Visual != '|'
                && levelData[y][x].Visual != '=';
        }

        public bool UseKey(Point playerPosition, Character character) //otwieranie drzwi w odpowiedniej kolejności
        {
            if (unlockedDoorsCount >= keysAndDoors.Count)
                return false;

            var pair = keysAndDoors.ElementAt(unlockedDoorsCount);
            Point doorPos = pair.Value;

            if (IsAdjacent(playerPosition, doorPos)) //sprawdzanie czy gracz jest przy drziwach
            {
                if (character.UseKey())
                {
                    OpenDoors(doorPos);
                    unlockedDoorsCount++;
                    return true;
                }
            }

            return false;
        }
       
        private bool IsAdjacent(Point a, Point b) 
        {
            int dx = Math.Abs(a.x - b.x);
            int dy = Math.Abs(a.y - b.y);
            return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
        }

        private void OpenDoors(Point doorPosition)
        {
            foreach (var pair in keysAndDoors)
            {
                if (pair.Value.Equals(doorPosition))
                {
                    char doorChar = levelData[doorPosition.y][doorPosition.x].Visual;
                    if (doorChar == '|' || doorChar == '_')
                    {
                        levelData[doorPosition.y][doorPosition.x].Visual = '/';
                    }
                    break; 
                }
            }
        }

        public void OpenSpecificDoor() //drzwi ktore otwieraja sie po skonczonym dialogu
        {
            Point doorPosition = new Point(29, 11);
            if (levelData[doorPosition.y][doorPosition.x].Visual == '_')
            {
                levelData[doorPosition.y][doorPosition.x].Visual = '/';
            }
        }

        public void OccupyCell(Point pos, Character character)
        {
            if (pos.y < 0 || pos.y >= levelData.Length || pos.x < 0 || pos.x >= levelData[pos.y].Length)
                return;

            char current = levelData[pos.y][pos.x].Visual;

            //zbieranie klucza
            if (current == '?')
            {
                character.AddItemToInventory('?');
                levelData[pos.y][pos.x].Visual = '.';
            }

            // zbieranie mikstury
            if (current == '8')
            {
                character.CollectHealingPotion();
                levelData[pos.y][pos.x].Visual = '.';
            }

            levelData[pos.y][pos.x].Occupy(character);
        }

        public void LeaveCell(Point pos)
        {
            if (pos.y < 0 || pos.y >= levelData.Length) return;
            if (pos.x < 0 || pos.x >= levelData[pos.y].Length) return;

            levelData[pos.y][pos.x].Leave();
        }
        public void SetCellVisual(Point pos, char symbol)
                {
                    levelData[pos.y][pos.x].Visual = symbol;
                }

        public char GetCellVisual(Point pos)
        {
            if (pos.y < 0 || pos.y >= levelData.Length) return ' ';
            if (pos.x < 0 || pos.x >= levelData[pos.y].Length) return ' ';

            return levelData[pos.y][pos.x].Visual;
        }

        public Point GetStartNearFirstTeleport(int offsetX)
        {
            if (teleportPoints.Count < 1)
                throw new InvalidOperationException("Brak teleportu");

            Point first = teleportPoints[0];
            return new Point(first.x + offsetX, first.y);
        }

        public Point GetOtherTeleport(Point current)
        {
            foreach (var point in teleportPoints)
            {
                if (!point.Equals(current))
                    return point;
            }
            return current;
        }

        public void PlaceHealingPotions(int count)
        {
            Random rand = new Random();
            int placed = 0;

            while (placed < count)
            {
                int y = rand.Next(levelData.Length);
                int x = rand.Next(levelData[y].Length);

                // zeby potki sie nie spawnowały w celach wiezniow
                bool inForbiddenArea = x >= 1 && x <= 11 && y >= 1 && y <= 8;
                if (inForbiddenArea)
                    continue;

                Cell cell = levelData[y][x];
                if (cell.Visual == '.' && !cell.IsOccupied())
                {
                    cell.Visual = '8';
                    placed++;
                }
            }
        }

        public bool CollectHealingPotion(Point pos)
        {
            var cell = levelData[pos.y][pos.x];
            if (cell.Visual == '8')
            {
                cell.Visual = '.';
                return true;
            }
            return false;
        }

        public void DrawSnake(IEnumerable<Point> body)
        {
            foreach (var pos in body)
            {
                if (IsInsideBounds(pos))
                    levelData[pos.y][pos.x].Visual = 's';
            }
        }

        public void ClearSnake(IEnumerable<Point> body)
        {
            foreach (var pos in body)
            {
                if (IsInsideBounds(pos))
                    levelData[pos.y][pos.x].Visual = '.';
            }
        }

        private bool IsInsideBounds(Point pos)
        {
            return pos.y >= 0 && pos.y < levelData.Length &&
                pos.x >= 0 && pos.x < levelData[pos.y].Length;
        }

        public void PlacePrisoners()
        {
            Random rand = new Random();

            List<(int x1, int x2, int y1, int y2, int count)> prisonerZones = new()
            {
                (1, 10, 2, 3, 1),
                (1, 10, 6, 7, 1),
                (27, 32, 18, 22, 1)
            };

            foreach (var (x1, x2, y1, y2, count) in prisonerZones)
            {
                int placed = 0;

                while (placed < count)
                {
                    int x = rand.Next(x1, x2 + 1);
                    int y = rand.Next(y1, y2 + 1);

                    if (IsInsideBounds(new Point(x, y)))
                    {
                        var cell = levelData[y][x];
                        if (cell.Visual == '.' && !cell.IsOccupied())
                        {
                            cell.Visual = 'P';
                            placed++;
                        }
                    }
                }
            }
        }

        public void ReplaceChar(char oldChar, char newChar)
        {
            for (int y = 0; y < levelData.Length; y++)
            {
                for (int x = 0; x < levelData[y].Length; x++)
                {
                    if (levelData[y][x].Visual == oldChar)
                    {
                        levelData[y][x].Visual = newChar;
                    }
                }
            }
        }

    }
}