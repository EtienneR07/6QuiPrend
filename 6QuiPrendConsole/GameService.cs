using _6QuiPrendConsole.Objects;
using _6QuiPrendConsole.Strategies;

namespace _6QuiPrendConsole
{
    public class GameService
    {
        private int EndingScore;

        public List<Player> Players = new();
        private Stack<Card> CardDeck = new();
        private Dictionary<int, Stack<Card>> Stacks = new();
        private Dictionary<Guid, List<int>> PlayerCardsDictionary = new();

        public void CreateGame(int endingScore)
        {
            EndingScore = endingScore;

            Players.Add(new Player("Etienne AI", new NaiveStrategy(2)));
            Players.Add(new Player("Dumb AI", new NaiveStrategy(2)));

            PlayerCardsDictionary = Players.ToDictionary(p => p.Id, p => new List<int>());
        }

        public List<Player> PlayGame()
        {
            foreach (var player in Players)
            {
                player.Score = 0;
            }
            while (!HasALoser())
            {
                if (Players.Any(x => x.Strategy.Cards.Count == 0))
                {
                    Reset();
                    Shuffle();
                    DistributeCards();
                    SetupStacks();
                }

                var turnCards = new Dictionary<Player, Card>();
                foreach (var player in Players)
                {
                    var chosenCard = player.Strategy.GetChosenCard(GetCurrentGameState());

                    if (!PlayerCardsDictionary.ContainsKey(player.Id)
                        || !PlayerCardsDictionary[player.Id].Contains(chosenCard.Number))
                        throw new InvalidOperationException("Chosen card not in player's hand");

                    player.Strategy.Cards.Remove(chosenCard);

                    turnCards.Add(player, chosenCard);
                }

                var playerOrder = turnCards.OrderBy(c => c.Value.Number).ToList();

                foreach (var playerWithCard in playerOrder)
                {
                    if (CanPlaceCard(playerWithCard))
                        PlaceCard(playerWithCard);
                    else
                        BuyStack(playerWithCard, playerWithCard.Key.Strategy.GetBoughtStack(GetCurrentGameState()));
                }
            }

            return Players;
        }

        private void BuyStack(KeyValuePair<Player, Card> playerWithCard, int chosenStack)
        {
            var points = Stacks[chosenStack].Sum(s => s.Bullheads);
            playerWithCard.Key.Score += points;
            Stacks[chosenStack].Clear();
            Stacks[chosenStack].Push(playerWithCard.Value);
        }

        private IEnumerable<GameStack> GetCurrentGameState()
        {
            return Stacks.Select(s => new GameStack(s));
        }

        private bool CanPlaceCard(KeyValuePair<Player, Card> playerWithCard)
        {
            return Stacks.Any(s => s.Value.Peek().Number > playerWithCard.Value.Number);
        }

        private void PlaceCard(KeyValuePair<Player, Card> playerWithCard)
        {
            var stackId = Stacks.OrderBy(s => s.Value.Peek().Number)
                .First(s => s.Value.Peek().Number > playerWithCard.Value.Number)
                .Key;

            if (Stacks[stackId].Count == 5)
            {
                var points = Stacks[stackId].Sum(s => s.Bullheads);
                playerWithCard.Key.Score += points;
                Stacks[stackId].Clear();
            }

            Stacks[stackId].Push(playerWithCard.Value);
        }


        private void Shuffle()
        {
            var deckList = DeckGenerator.GenerateDeck();
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

    public class GameStack
    {
        public int TopCard;
        public int CardCount;
        public int StackValue;
        public int StackId;

        public GameStack(KeyValuePair<int, Stack<Card>> cardStack)
        {
            StackId = cardStack.Key;
            TopCard = cardStack.Value.Peek().Number;
            CardCount = cardStack.Value.Count;
            StackValue = cardStack.Value.Sum(c => c.Bullheads);
        }
    }
}