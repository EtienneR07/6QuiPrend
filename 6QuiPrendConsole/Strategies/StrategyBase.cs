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

        public void UpdateStack(int stackId, Stack<Card> updatedStack)
        {
            Stacks[stackId] = updatedStack;
        }

        public abstract Card GetChosenCard(IEnumerable<GameStack> stacks);

        public abstract int ChoseStack(Card card);

        public abstract int GetChosenStack(IEnumerable<GameStack> getCurrentGameState);
    }
}
