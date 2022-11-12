using _6QuiPrendConsole.Strategies;

namespace _6QuiPrendConsole.Objects
{
    public class Player
    {
        public Player(string name, StrategyBase strategy)
        {
            Name = name;
            Strategy = strategy;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        public string Name { get; set; }

        public int Score { get; set; }

        public StrategyBase Strategy { get; set; }
    }
}
