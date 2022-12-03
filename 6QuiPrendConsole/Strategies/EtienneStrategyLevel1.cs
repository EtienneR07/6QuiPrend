using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole.Strategies
{
    public class EtienneStrategyLevel1 : StrategyBase
    {
        public List<Card> CurrentGameCards = new List<Card>();

        public EtienneStrategyLevel1(int numberOfPlayers) : base(numberOfPlayers)
        {
            CurrentGameCards = DeckGenerator.GenerateDeck();
        }

        public override void ReceiveHand(IList<Card> cards)
        {
            Hand = cards;
            var numberToRemove = cards.Select(c => c.Number).ToList();
            CurrentGameCards.RemoveAll(c => numberToRemove.Contains(c.Number));
        }

        public override Card GetChosenCard(IEnumerable<GameStack> gameState)
        {
            var cardWithEval = new Dictionary<Card, decimal>();
            foreach (var card in Hand)
            {
                cardWithEval.Add(card, EvaluateCard(card, gameState.ToList()));
            }

            return cardWithEval
                .OrderBy(c => c.Value)
                .ThenBy(c => c.Key.Bullheads)
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

        private decimal EvaluateCard(Card card, IEnumerable<GameStack> gameState)
        {
            var stackForCard = gameState
                .OrderByDescending(s => s.TopCard)
                .FirstOrDefault(s => s.TopCard < card.Number);

            int possiblePointsGained;
            if (stackForCard == null)
            {
                possiblePointsGained = gameState.Min(s => s.StackValue);
                return 1 * possiblePointsGained;
            }

            if (5 - stackForCard.CardCount >= NumberOfPlayers)
            {
                return 0;
            }

            possiblePointsGained = stackForCard.StackValue;
            if (stackForCard.CardCount == 5)
            {
                return 1 * possiblePointsGained;
            }

            var numberOfCardsBetween = CurrentGameCards
                .Where(x => x.Number > stackForCard.StackValue && x.Number < card.Number)
                .Count();

            return (numberOfCardsBetween/ CurrentGameCards.Count()) * possiblePointsGained;
        }

        public override void NotifyNewGame()
        {
            CurrentGameCards = DeckGenerator.GenerateDeck();
        }
    }
}
