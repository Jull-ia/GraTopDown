using System.Collections.Generic;
using System;

namespace GameProject
{
    class Character
    {
        public List<char> Inventory { get; } = new();
        public char Symbol { get; }
        public Lives Lives { get; }

        public List<string> HeldItems { get; }
        private int potionCount = 1; 
        public int PotionCount => potionCount;

        public Character(char symbol)
        {
            Symbol = symbol;
            HeldItems = new List<string>();
            HeldItems.Add("HealingPotion");
            Lives = new Lives();
        }

        public void AddItemToInventory(char itemSymbol)
        {
            Inventory.Add(itemSymbol);
        }

        public string GetInventoryDisplay()
        {
            if (Inventory.Count == 0)
                return "[][][]";

            int slots = 3;
            var displayItems = new List<string>();

            for (int i = 0; i < slots; i++)
            {
                if (i < Inventory.Count)
                    displayItems.Add($"[{Inventory[i]}]");
                else
                    displayItems.Add("[ ]");
            }
            return string.Join("", displayItems);

        }

        public bool UseKey()
        {
            if (Inventory.Contains('?'))
            {
                Inventory.Remove('?');
                return true;

            }
            return false;

        }




        public void CollectHealingPotion()
        {
            potionCount++;
        }

        public string UseItem()
        {
            if (potionCount > 0 && Lives.Heal()) 
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