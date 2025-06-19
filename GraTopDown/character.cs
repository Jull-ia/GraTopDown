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

        public string UseItem()
        {

            if (heldItem == "HealingPotion")
            {
                if (Lives.Current < Lives.Max)

                {
                    Lives.AddLife();
                     heldItem = null;
                    return "Użyto mikstury leczącej!";
                   
                }
                else
                {
                    return "Masz już maskymanlną liczbę żyć!";
                }

            }
            else
            {
                return "Brak przedmiotu do użycia";
            }

        }

    }
}