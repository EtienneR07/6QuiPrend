namespace _6QuiPrendConsole // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gameService = new GameService();
                                   
            gameService.CreateGame(66);

            var players = gameService.PlayGame();

            var ranked = players.OrderBy(p => p.Score);

            foreach (var p in ranked)
            {
                Console.Write($"{p.Name} finished with {p.Score} \n");
            }
        }
    }
}
