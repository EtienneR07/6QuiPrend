using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole.Strategies
{
    public class NaiveStrategy : StrategyBase
    {
        public NaiveStrategy(int numberOfPlayers) : base(numberOfPlayers)
        {
        }

        public override Card GetChosenCard(IEnumerable<GameStack> tuples)
        {
            var chosenCard = Hand.FirstOrDefault();

            return chosenCard;
        }

        public override int GetBoughtStack(IEnumerable<GameStack> getCurrentGameState)
        {
            return 1;
        }

        public override void NotifyNewGame()
        {
        }
    }
}
