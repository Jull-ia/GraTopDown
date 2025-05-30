// class Player
// {
//     public string name;
//     public int x;
//     public int y;

//     public Player(string name)
//     {
//         this.name = name;
//     }
// }


using System.Drawing;

class Player : Character
{
    public string chosenAction;

    private Dictionary<ConsoleKey, string> keyBindings;

    public Player(string name, string avatar, Point position, Dictionary<ConsoleKey, string> keyActionMap) : base(name, avatar, position)
    {
        keyBindings = keyActionMap;
    }

    public override string ChooseAction()
    {
        ConsoleKeyInfo pressedKeyInfo = Console.ReadKey(true);
        chosenAction = keyBindings.GetValueOrDefault(pressedKeyInfo.Key, "none");
        return chosenAction;
    }

    public override void Move(Point direction, Level level, List<Character> characters)
    {
        base.Move(direction, level, characters);

    }
}