using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole.Strategies
{
    public abstract class StrategyBase
    {
        protected int NumberOfPlayers;
        public IList<Card> Hand = new List<Card>();

        public StrategyBase(int numberOfPlayers)
        {
            NumberOfPlayers = numberOfPlayers;
        }

        public virtual void ReceiveHand(IList<Card> cards)
        {
            Hand = cards;
        }

        public abstract Card GetChosenCard(IEnumerable<GameStack> stacks);

        public abstract int GetBoughtStack(IEnumerable<GameStack> getCurrentGameState);

        public abstract void NotifyNewGame();
    }
}
