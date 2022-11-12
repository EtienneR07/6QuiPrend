using _6QuiPrendConsole.Objects;
using _6QuiPrendConsole.Strategies;

namespace _6QuiPrendConsole
{
    public class GameService
    {

        private int EndingScore;

        private List<Player> Players = new();
        private Stack<Card> CardDeck = new();
        private Dictionary<int, Stack<Card>> Stacks = new();
        private Dictionary<Guid, List<int>> PlayerCardsDictionary = new();

        public void CreateGame(int endingScore)
        {
            EndingScore = endingScore;

            Players.Add(new Player("Etienne", new NaiveStrategy(2)));
            Players.Add(new Player("Etienne 2", new NaiveStrategy(2)));

            PlayerCardsDictionary = Players.ToDictionary(p => p.Id, p => new List<int>());
        }

        public List<Player> PlayGame()
        {
            while (!HasALoser())
            {
                if (Players.Any(x => x.Strategy.Cards.Count == 0))
                {
                    Reset();
                    GenerateDeck();
                    SetupStacks();
                    DistributeCards();
                }
                
                var turnCards = new Dictionary<Player, Card>();
                foreach (var player in Players)
                {
                    var chosenCard = player.Strategy.GetChosenCard();

                    if (!PlayerCardsDictionary.ContainsKey(player.Id)
                        || !PlayerCardsDictionary[player.Id].Contains(chosenCard.Number))
                        throw new InvalidOperationException("Chosen card not in player's hand");

                    player.Strategy.Cards.Remove(chosenCard);

                    turnCards.Add(player, chosenCard);
                }

                var playerOrder = turnCards.OrderBy(c => c.Value.Number).ToList();

                foreach (var playerWithCard in playerOrder)
                {
                    var stackId = playerWithCard.Key.Strategy.ChoseStack(playerWithCard.Value);

                    if (!ValidChoice(stackId, playerWithCard.Value.Number))
                        throw new InvalidOperationException("Chosen stack is invalid according to the rules");

                    PlaceCard(stackId, playerWithCard);

                    foreach (var player in Players)
                    {
                        player.Strategy.UpdateStack(stackId, new Stack<Card>(Stacks[stackId]));
                    }
                }

            }

            return Players;
        }

        private void PlaceCard(int stackId, KeyValuePair<Player, Card> playerWithCard)
        {
            if (Stacks[stackId].Count == 5 || Stacks[stackId].Peek().Number > playerWithCard.Value.Number)
            {
                var points = Stacks[stackId].Sum(s => s.Bullheads);
                playerWithCard.Key.Score += points;
                Stacks[stackId].Clear();
            }

            Stacks[stackId].Push(playerWithCard.Value);
        }

        private bool ValidChoice(int stackId, int chosenCardNumber)
        {
            var topCards = new Dictionary<int, Card>();
            foreach (var stack in Stacks)
            {
                topCards.Add(stack.Key, stack.Value.Peek());
            }

            if (topCards.All(tc => tc.Value.Number > chosenCardNumber)) return true;

            var onlyValidStackId = topCards
                .ToDictionary(x => x.Key, x => chosenCardNumber - x.Value.Number)
                .OrderBy(x => x.Value)
                .Select(x => x.Key)
                .FirstOrDefault();

            if (stackId != onlyValidStackId) return false;

            return true;
        }

        private void GenerateDeck()
        {
            var deckList = new List<Card>();

            for (var i = 1; i <= 104; i++)
            {
                var bullHeads = 1;
                if (i == 55)
                {
                    bullHeads = 7;
                }
                else if (SameDigit(i))
                {
                    bullHeads = 5;
                }
                else if (i % 10 == 0)
                {
                    bullHeads = 3;
                }
                else if (i % 10 == 5)
                {
                    bullHeads = 2;
                }

                var card = new Card
                {
                    Bullheads = bullHeads,
                    Number = i
                };

                deckList.Add(card);
            }
            var rng = new Random();
            deckList = deckList.OrderBy(c => rng.Next()).ToList();
            CardDeck = new Stack<Card>(deckList);
        }

        private bool SameDigit(int number)
        {
            var lastDigit = number % 10;

            while (number != 0)
            {
                var currentDigit = number % 10;

                number /= 10;

                if (currentDigit != lastDigit)
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasALoser()
        {
            return Players.Any(p => p.Score >= EndingScore);
        }

        private void DistributeCards()
        {
            foreach (var player in Players)
            {
                var playerHand = new List<Card>();

                for (var i = 0; i < 10; i++)
                {
                    playerHand.Add(CardDeck.Pop());
                }

                player.Strategy.ReceiveCards(playerHand);

                PlayerCardsDictionary[player.Id] = playerHand.Select(x => x.Number).ToList();
            }
        }

        private void SetupStacks()
        {
            Stacks.Add(1, new Stack<Card>(new[] { CardDeck.Pop() }));
            Stacks.Add(2, new Stack<Card>(new[] { CardDeck.Pop() }));
            Stacks.Add(3, new Stack<Card>(new[] { CardDeck.Pop() }));
            Stacks.Add(4, new Stack<Card>(new[] { CardDeck.Pop() }));

            foreach (var player in Players)
            {
                player.Strategy.ReceiveStacks(new Dictionary<int, Stack<Card>>(Stacks));
            }
        }

        private void Reset()
        {
            CardDeck = new Stack<Card>();
            Stacks = new Dictionary<int, Stack<Card>>();
            PlayerCardsDictionary = new Dictionary<Guid, List<int>>();
        }
    }
}
