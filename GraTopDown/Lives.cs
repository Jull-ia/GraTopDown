namespace GameProject
{
    class Lives
    {
        private int currentLives;

        public Lives(int initialLives = 3)
        {
            currentLives = initialLives;
        }

        public int Current => currentLives;

        public bool IsAlive => currentLives > 0;

        public void LoseLife()
        {
            if (currentLives > 0)
                currentLives--;
            // Console.Beep(); // sygnał dźwiękowy
        }

        public void Reset(int lives = 3)
        {
            currentLives = lives;
        }

        public void Display()
        {
            Console.WriteLine($"Życia: {currentLives}"); // wyswietla liczbe zyc
        }
    }
}