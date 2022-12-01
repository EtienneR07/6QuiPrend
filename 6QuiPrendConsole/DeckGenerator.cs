using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole
{
    public static class DeckGenerator
    {
        public static List<Card> GenerateDeck()
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

            return deckList;
        }

        private static bool SameDigit(int number)
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
    }
}
