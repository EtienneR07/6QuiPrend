using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole.Strategies
{
    public abstract class StrategyBase
    {
        protected int NumberOfPlayers;
        public IList<Card> Cards = new List<Card>();
        protected Dictionary<int, Stack<Card>> Stacks = new();

        public StrategyBase(int numberOfPlayers)
        {
            NumberOfPlayers = numberOfPlayers;
        }

        public void ReceiveCards(IList<Card> cards)
        {
            Cards = cards;
        }

        public void ReceiveStacks(Dictionary<int, Stack<Card>> stacks)
        {
            Stacks = stacks;
        }

        public abstract Card GetChosenCard(IEnumerable<GameStack> stacks);

        public abstract int GetBoughtStack(IEnumerable<GameStack> getCurrentGameState);
    }
}
