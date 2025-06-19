namespace GameProject
{
    class Character
    {
        private string ? heldItem;
        public char Symbol { get; }
        public Lives Lives { get; }

        public Character(char symbol)
        {
            Symbol = symbol;
            heldItem = "HealingPotion";
            Lives = new Lives();
        }

        public void UseItem()
        {

            if (heldItem == "HealingPotion")
            {
                if (Lives.Current < Lives.Max)

                {
                    Lives.AddLife();
                    Console.WriteLine("Użyto mikstury leczącej!");
                    heldItem = null;
                }
                else
                {
                    Console.WriteLine("Masz już maskymanlną liczbę żyć!");
                }

            }
            else
            {
                Console.WriteLine("Brak przedmiotu do użycia");
            }

            Thread.Sleep(1000);
            Console.Clear();
        }

    }
}