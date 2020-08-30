using BattleShip.BLL.GameFlow;
using System;


namespace BattleShip.UI
{
    class Program
    {
        static void Main()
        {
            Game game = new Game();
            game.StartMenu();
        }

        public class Game
        {
            private string playerA, playerB;
            Player nextPlayer; 

            public void StartMenu()
            {
                Console.WriteLine("Let's play Battleship!");
                Console.WriteLine("This is a two-player game.");
                playerA = GetPlayerName("the first player");
                playerB = GetPlayerName("the second player");
                Player player1 = new Player()
                {
                    name = playerA
                };
                Player player2 = new Player()
                {
                    name = playerB
                };
                Console.WriteLine($"The battle is on between {player1.name} and {player2.name}!");
                Player currentPlayer = CoinFlip(player1, player2);
                if (currentPlayer == player1) nextPlayer = player2;
                else nextPlayer = player1;
                Console.WriteLine($"We've randomly selected {currentPlayer.name} to go first. {nextPlayer.name} will go second.");
                Console.WriteLine("Hit enter to continue ... ");
                Console.ReadLine();
                Console.Clear();
                GameFlow newGame = new GameFlow();
                newGame.PlayGame(currentPlayer, nextPlayer);
            }

            private string GetPlayerName(string namePrompt)
            {
                Console.Write($"Please enter the name of {namePrompt}: ");
                return Console.ReadLine();
            }

            private Player CoinFlip(Player player1, Player player2)
            {
                Random flip = new Random();
                int HeadsOrTails = flip.Next(2);
                if (HeadsOrTails == 1)
                {
                    return player1;
                }
                else
                {
                    return player2;
                }
            }
        }
    }
}

