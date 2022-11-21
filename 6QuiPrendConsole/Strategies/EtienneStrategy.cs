using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole.Strategies
{
    public class EtienneStrategy : StrategyBase
    {
        public EtienneStrategy(int numberOfPlayers) : base(numberOfPlayers)
        {
        }

        public override Card GetChosenCard(IEnumerable<GameStack> tuples)
        {
            throw new NotImplementedException();
        }

        public virtual int ChoseStack(Card card)
        {
            throw new NotImplementedException();
        }

        public override int GetChosenStack(IEnumerable<GameStack> getCurrentGameState)
        {
            throw new NotImplementedException();
        }
    }
}
