using System.Collections.Generic;

namespace GameProject
{
    class Character
    {
        public List<string> HeldItems { get; }
        public char Symbol { get; }
        public Lives Lives { get; }

        public Character(char symbol)
        {
            Symbol = symbol;
            HeldItems = new List<string>();
            HeldItems.Add("HealingPotion");
            Lives = new Lives();
        }

        public string UseItem()
        {
            if (HeldItems.Contains("HealingPotion"))
            {
                if (Lives.Current < Lives.Max)
                {
                    Lives.AddLife();
                    HeldItems.Remove("HealingPotion");
                    return "Użyto mikstury leczącej!";
                }
                else
                {
                    return "Masz już maksymalną liczbę żyć!";
                }
            }
            else
            {
                return "Brak mikstury do użycia.";
            }
        }

        public void CollectHealingPotion()
        {
            HeldItems.Add("HealingPotion");
        }
    }
}