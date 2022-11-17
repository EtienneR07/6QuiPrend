using _6QuiPrendConsole.Objects;

namespace _6QuiPrendConsole // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gameService = new GameService();

            gameService.CreateGame(66);
            var winDictionnary = new Dictionary<Player, int>();
            for (int i = 0; i < 100000; i++)
            {
                var players = gameService.PlayGame();
                var winner = players.OrderBy(p => p.Score).First();
                
                if (winDictionnary.ContainsKey(winner)) winDictionnary[winner]++;
                else winDictionnary.Add(winner, 1);
            }

            
            foreach (var p in winDictionnary)
            {
                Console.Write($"{p.Key.Name} won {p.Value} Games \n");
            }
        }
    }
}