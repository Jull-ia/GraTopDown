const string firstAllowedSign = "paper";
const string secondAllowedSign = "rock";
const string thirdAllowedSign = "scissors";

string[] allowedSigns = ["paper", "rock", "scissors"];
Random random = new Random();

while (true)
{
    Console.Clear();
    Console.WriteLine($"Podaj znak ({string.Join("/", allowedSigns)}):");

    string firstSign = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;

    while (!allowedSigns.Contains(firstSign))
    {
        Console.WriteLine("Nawet tego cię matka nie nauczyła?..");
        Console.WriteLine($"Podaj POPRAWNY znak! ({string.Join("/", allowedSigns)}):");
        firstSign = Console.ReadLine()?.ToLower().Trim() ?? string.Empty;
    }

    // Strażnik losuje znak
    string secondSign = allowedSigns[random.Next(allowedSigns.Length)];
    Console.WriteLine($"Strażnik wybrał: {secondSign}");

    if (firstSign == secondSign)
    {
        Console.WriteLine("Remis! Szykuje się dogrywka!");
        Console.ReadKey();
    }
    else if (
        (firstSign == firstAllowedSign && secondSign == thirdAllowedSign) ||
        (firstSign == secondAllowedSign && secondSign == firstAllowedSign) ||
        (firstSign == thirdAllowedSign && secondSign == secondAllowedSign)
    )
    {
        Console.WriteLine("W porządku, wygrałeś.. Strażnik odchodzi.");
        Console.ReadKey();
        break;
        // gracz wygrał – strażnik znika
    }
    else
    {
        Console.WriteLine("No to chyba sobie tu postoisz"); // gracz przegrał – strażnik zostaje
        Console.ReadKey();
    }
}
