using System;

namespace GameProject
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;

            Game game = new Game();
            game.Run();
        }
    }
}