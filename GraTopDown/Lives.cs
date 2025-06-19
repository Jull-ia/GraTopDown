namespace GameProject
{
    class Lives
    {
        private int currentLives;
        private int maxLives = 3;

        public Lives(int initialLives = 3)
        {
            currentLives = initialLives;
        }

        public int Current => currentLives;
        public int Max => maxLives;
        public bool IsAlive => currentLives > 0;

        public void LoseLife()
        {
            if (currentLives > 0)
                currentLives--;
            // Console.Beep(); // sygnał dźwiękowy
        }

        public void AddLife()
        {
            if (currentLives < maxLives)
                currentLives++;
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