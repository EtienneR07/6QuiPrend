using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole.Strategies
{
    public class EtienneStrategyLevel1 : StrategyBase
    {
        public IList<Card> Cards = new List<Card>();

        public EtienneStrategyLevel1(int numberOfPlayers) : base(numberOfPlayers)
        {
            Cards = DeckGenerator.GenerateDeck();
        }

        public override Card GetChosenCard(IEnumerable<GameStack> gameState)
        {
            var cardWithEval = new Dictionary<Card, int>();
            foreach (var card in Cards)
            {
                cardWithEval.Add(card, EvaluateCard(card, gameState.ToList()));
            }

            return cardWithEval
                .OrderBy(c => c.Value)
                .Select(c => c.Key)
                .FirstOrDefault();
        }

        public override int GetBoughtStack(IEnumerable<GameStack> gameState)
        {
            return gameState
                .OrderBy(s => s.StackValue)
                .Select(s => s.StackId)
                .FirstOrDefault();
        }

        private int EvaluateCard(Card card, IEnumerable<GameStack> gameState)
        {
            return 0;
        }
    }
}
