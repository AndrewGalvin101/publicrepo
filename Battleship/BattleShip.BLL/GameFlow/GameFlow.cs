using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;
using System;

namespace BattleShip.BLL.GameFlow
{
    

    public class Player
    {
        public string name;
        public Board board;
    }

    public class GameFlow
    {
        public const string alpha = " ABCDEFGHIJ";

        //should contain two boards, keep track of which player's turn it is, and process each players turn;
        /* 	A player's turn is as follows:
            1) Show a grid with marks from the their board's shot history. 
            Place a yellow M in a coordinate if a shot has been fired and missed at that location 
            or a red H if a shot has been fired that has hit.
            2) Prompt the user for a coordinate entry (ex: B10).
            3) Validate the entry; if valid, create a coordinate object, 
            convert the letter to a number, and call the opponent board's FireShot() method.
            4) Retrieve the response from the FireShot method and display an appropriate message to the user.
            5) Remember to prompt to continue and clear the screen to keep things clean. */



        public void PlayGame(Player currentPlayer, Player nextPlayer)
        {
            SetUpBoard(currentPlayer);
            Console.Clear();
            SetUpBoard(nextPlayer);
            Console.Clear();
            Console.WriteLine("Time to start shooting.");
            FireShotResponse response;
            do
            {
                printBoard(currentPlayer);
                response = ShootYourShot(currentPlayer);
                if (response.ShotStatus == ShotStatus.Duplicate || response.ShotStatus == ShotStatus.Invalid)
                {
                    Console.Clear();
                    continue;
                }
                if (response.ShotStatus == ShotStatus.Victory)
                {
                    break;
                }

                Console.Clear();
                Player temp = nextPlayer;
                nextPlayer = currentPlayer;
                currentPlayer = temp;
            } while (response.ShotStatus != ShotStatus.Victory);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Game over! Congratulations, {currentPlayer.name}!");
            Console.ResetColor();
        }

        private FireShotResponse ShootYourShot(Player currentPlayer)
        {
            Coordinate nextShot = InputAndValidateCoordinate(currentPlayer, CoordinateRequestType.shot);
            FireShotResponse response = currentPlayer.board.FireShot(nextShot);
            if (response.ShotStatus == ShotStatus.Duplicate)
            {
                Console.WriteLine("You already tried that one. Try again.");
            }
            else if (response.ShotStatus == ShotStatus.Hit)
            {
                Console.WriteLine($"Hit! You hit the {response.ShipImpacted}");
            }
            else if (response.ShotStatus == ShotStatus.Miss)
            {
                Console.WriteLine("Miss!");
            }
            else if (response.ShotStatus == ShotStatus.Invalid)
            {
                Console.WriteLine("Invalid. Try again.");
            }
            else if (response.ShotStatus == ShotStatus.HitAndSunk)
            {
                Console.WriteLine($"Hit and sunk! You sank the {response.ShipImpacted}");
            }
            if (response.ShotStatus == ShotStatus.Victory)
            {
                Console.WriteLine($"Hit and sunk! You sank the {response.ShipImpacted}");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("You did it!! You sank all five ships! You won the game!!!");
                Console.ResetColor();
            }
            Console.Write("Press enter to continue ...");
            Console.ReadLine();
            return response;
        }

        private void printBoard(Player currentPlayer)
        {
            Console.WriteLine(" 12345678910");  //TODO: convert to const
            for (int x = 1; x <= 10; x++)
            {
                Console.Write(alpha[x]);
                for (int y = 1; y <= 10; y++)
                {
                    Coordinate toCheck = new Coordinate(x, y);
                    //TODO: make into switch statement 
                    if (currentPlayer.board.GetShotHistory(toCheck) != ShotHistory.Unknown)
                    {
                        if (currentPlayer.board.GetShotHistory(toCheck) == ShotHistory.Miss)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("M");
                            Console.ResetColor();
                        }
                        else if (currentPlayer.board.GetShotHistory(toCheck) == ShotHistory.Hit)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("H");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.Write("_");
                    }
                }
                Console.WriteLine();
            }
        }

        private enum CoordinateRequestType
        {
            placement,
            shot,
        }

        private Coordinate InputAndValidateCoordinate(Player currentPlayer, CoordinateRequestType request, ShipType ship = ShipType.Destroyer)
        {
            //   In Battleship, each player has their own hidden board which is a 10 x 10 grid
            //   where the rows are denoted by letters A-J and the columns by the numbers 1-10.
            // 	Coordinates in the game board are numbers, ex: 5, 10.
            //  Players enter coordinates using letters and numbers, ex: E10. 
            //  Create a method for prompting, validating, and translating a user's coordinate

            int x, y;

            if (request == CoordinateRequestType.shot)
            {
                Console.Write($"Shoot your shot, {currentPlayer.name}: ");
            }
            else
            {
                Console.Write($"Where would you like to place your {ship}, {currentPlayer.name}? ");
            }

            while (true)
            {
                string playerEntry = Console.ReadLine();
                if (playerEntry.Length < 2 || playerEntry.Length > 3)
                {
                    Console.Write("Invalid entry. Coordinates must be 2 or 3 characters long, such as E10 or g3. Try again: ");
                    continue;
                }
                if (String.IsNullOrEmpty(playerEntry) || Char.IsDigit(playerEntry[0]) || Char.IsLetter(playerEntry[1]))
                {
                    Console.Write("Invalid entry. Coordinates must begin with a letter and include one or two digits, such as E10 or g3. Try again: ");
                    continue;
                }
                string xCoordinate = char.ToUpper(playerEntry[0]) + "";
                if (alpha.Contains(xCoordinate))
                {
                    x = alpha.IndexOf(xCoordinate);
                }
                else
                {
                    Console.Write("Invalid entry. Coordinates must begin with a letter between A and J. Try again: ");
                    continue;
                }

                string yCoordinate = "";
                if (playerEntry.Length == 2)
                {
                    yCoordinate += playerEntry[1];
                }
                else
                {
                    yCoordinate += (playerEntry[1] + "" + playerEntry[2]);  
                }

                int.TryParse(yCoordinate, out y);
                if (y < 1 || y > 10)
                {
                    Console.Write("Invalid entry. The numerical part of the coordinate must be between 1 and 10. Try again: ");
                    continue;
                }
                break;
            }

            Coordinate a = new Coordinate(x, y);
            return a;
        }


        /* Create a setup workflow object which creates a board instance for the game workflow with ships populated by the user.
         * Each player should be prompted to place their ships on their board by giving a starting coordinate and a direction. 
         * Clear the screen when a player is finished so the other player can't cheat! */

        private void SetUpBoard(Player player)
        {
            player.board = new Board();
            Console.WriteLine($"Okay {player.name}, let's get your board set up.");

            PutShipOnBoard(player, ShipType.Destroyer);
            PutShipOnBoard(player, ShipType.Submarine);
            PutShipOnBoard(player, ShipType.Cruiser);
            PutShipOnBoard(player, ShipType.Carrier);
            PutShipOnBoard(player, ShipType.Battleship);

        }

        private void PutShipOnBoard(Player player, ShipType typeOfShip)
        {
            PlaceShipRequest newPlacement = new PlaceShipRequest
            {
                ShipType = typeOfShip
            };


            while (true)
            {
                newPlacement.Coordinate = InputAndValidateCoordinate(player, CoordinateRequestType.placement, typeOfShip);

                Console.WriteLine($"And which direction do you want to place it in, {player.name}?");
                Console.Write($"Enter 1 for Up, 2 for Down, 3 for Left or 4 for Right: ");

                string shipDirection = Console.ReadLine();
                bool validDirection = int.TryParse(shipDirection, out int result);
                if (!validDirection || (1 > result || result > 4))
                {
                    Console.WriteLine("Invalid entry. Try again.");
                    continue;
                }
                else
                {
                    newPlacement.Direction = (ShipDirection)result;
                    ShipPlacement attemptToPlaceShip = player.board.PlaceShip(newPlacement);
                    if (attemptToPlaceShip == ShipPlacement.NotEnoughSpace)
                    {
                        Console.WriteLine("Not enough space in that direction. Try again.");
                        continue;
                    }
                    else if (attemptToPlaceShip == ShipPlacement.Overlap)
                    {
                        Console.WriteLine("That overlaps with another ship. Try again.");
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}

