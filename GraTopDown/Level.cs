using System;
using System.Collections.Generic;

namespace GameProject
{
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
            "############.....#---------#..[?]..#",
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

        private Dictionary<Point, Point> keysAndDoors = new()
        {
            { new Point(31, 8), new Point(33, 14) }
        };

        private Cell[][] levelData;
        private List<Point> teleportPoints = new();
        private Random random = new();

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
                && levelData[y][x].Visual != 'T'
                && levelData[y][x].Visual != '_'
                && levelData[y][x].Visual != '|';
        }

        private void OpenDoors(Point pos)
        {
            if (keysAndDoors.TryGetValue(pos, out Point doorPosition))
                levelData[doorPosition.y][doorPosition.x].Visual = '/';
        }

        public void OccupyCell(Point pos, Character character)
        {
            if (pos.y < 0 || pos.y >= levelData.Length || pos.x < 0 || pos.x >= levelData[pos.y].Length)
                return;

            char current = levelData[pos.y][pos.x].Visual;

            // Otwórz drzwi
            if (current == '!' || current == '*')
            {
                OpenDoors(pos);
            }

            // Zbieranie mikstury
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

        public char GetCellVisual(Point pos)
        {
            if (pos.y < 0 || pos.y >= levelData.Length) return ' ';
            if (pos.x < 0 || pos.x >= levelData[pos.y].Length) return ' ';

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
                if (!point.Equals(current))
                    return point;
            }
            return current;
        }

        public void PlaceHealingPotions(int count)
        {
            int placed = 0;
            while (placed < count)
            {
                int y = random.Next(levelData.Length);
                int x = random.Next(levelData[y].Length);

                char visual = levelData[y][x].Visual;
                if (visual == '.' || visual == ' ')
                {
                    levelData[y][x].Visual = '8';
                    placed++;
                }
            }
        }
    }
}