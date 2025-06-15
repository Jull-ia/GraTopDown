// public class DialogChecker
// {
//     private char[,] map;

//     public DialogChecker(char[,] map)
//     {
//         this.map = map;
//     }

//     public void Check(int playerX, int playerY)
//     {
//         for (int dy = -1; dy <= 1; dy++)
//         {
//             for (int dx = -1; dx <= 1; dx++)
//             {
//                 int nx = playerX + dx;
//                 int ny = playerY + dy;

//                 if (IsInBounds(nx, ny) && map[ny, nx] == 'P')
//                 {
//                     Console.WriteLine("P: Witaj, podróżniku!");
//                     return;
//                 }
//             }
//         }
//     }

//     private bool IsInBounds(int x, int y)
//     {
//         return y >= 0 && y < map.GetLength(0) && x >= 0 && x < map.GetLength(1);
//     }
// }