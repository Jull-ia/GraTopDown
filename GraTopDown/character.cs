using System.Collections.Generic;
using System;

namespace GameProject
{
    class Character
    {
        public List<string> HeldItems { get; }
        public char Symbol { get; }
        public Lives Lives { get; }

        private int potionCount = 1; // zaczynamy z 1 miksturą

        public int PotionCount => potionCount;

        public Character(char symbol)
        {
            Symbol = symbol;
            HeldItems = new List<string>();
            HeldItems.Add("HealingPotion");
            Lives = new Lives();
        }

        public void CollectHealingPotion()
        {
            potionCount++;
        }

        public string UseItem()
        {
            if (potionCount > 0 && Lives.Heal()) // Heal() zwraca true jeśli udało się uleczyć
            {
                potionCount--;
                return "Użyto mikstury. Odzyskano życie.";
            }
            return "Brak mikstur lub pełne zdrowie.";
        }

        public int GetPotionCount() => potionCount;

        public void DisplayPotionCount()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Mikstury: {PotionCount}".PadRight(Console.WindowWidth));
            Console.ResetColor();
        }
    }
}