using System.Collections.Generic;

namespace GameProject
{
    class Character
    {
        public List<string> HeldItems { get; }
        public char Symbol { get; }
        public Lives Lives { get; }

        public int PotionCount { get; private set; } = 1; // gracz startuje z 1 miksturą

        public Character(char symbol)
        {
            Symbol = symbol;
            HeldItems = new List<string>();
            HeldItems.Add("HealingPotion");
            Lives = new Lives();
        }

        public string UseItem()
        {
            if (PotionCount > 0)
            {
                if (Lives.Current < Lives.Max)
                {
                    Lives.AddLife();
                    PotionCount--;
                    return "Użyto mikstury leczącej!";
                }
                else
                {
                    return "Masz już maksymalną liczbę żyć!";
                }
            }
            else
            {
                return "Brak mikstur do użycia.";
            }
        }

        public void AddPotion()
        {
            PotionCount++;
        }

        public void CollectHealingPotion()
        {
            HeldItems.Add("HealingPotion");
        }
    public void DisplayPotionCount()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Mikstury: {PotionCount}".PadRight(Console.WindowWidth));
            Console.ResetColor();
        }
    }
}