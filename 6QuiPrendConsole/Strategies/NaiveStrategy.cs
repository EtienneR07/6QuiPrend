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
            var chosenCard = Cards.FirstOrDefault();

            return chosenCard;
        }

        public override int ChoseStack(Card card)
        {
            var number = card.Number;

            var topCards = new Dictionary<int, Card>();
            foreach (var stack in Stacks)
            {
                topCards.Add(stack.Key, stack.Value.Peek());
            }

            if (topCards.All(tc => tc.Value.Number > number)) return 1;

           return topCards
                .ToDictionary(x => x.Key, x => number - x.Value.Number)
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .FirstOrDefault();
        }

        public override int GetChosenStack(IEnumerable<GameStack> getCurrentGameState)
        {
            return 1;
        }
    }
}
