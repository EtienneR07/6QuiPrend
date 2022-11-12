using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole.Strategies
{
    public class EtienneStrategy : StrategyBase
    {
        public EtienneStrategy(int numberOfPlayers) : base(numberOfPlayers)
        {
        }

        public override Card GetChosenCard()
        {
            throw new NotImplementedException();
        }

        public override int ChoseStack(Card card)
        {
            throw new NotImplementedException();
        }
    }
}
