namespace TMS_1;

class Game
{
    
    public Game(int roundsToPlay)
    {
        RoundsToPlay = roundsToPlay;
    }
    
    private int _roundsPlayed = 0; // сохранить состояние можно в полях

    public int RoundsToPlay { get; } // авто-свойство (приватное поле и простой геттер + сеттер)

    public bool UserWon { get; private set; }

    public string DisplayRoundsPlayed(bool someParam, int roundsPlayed = 2, int someAdditionalParam = 3) => $"Game has {roundsPlayed} rounds played";

    public void Play()
    {
        Console.WriteLine("Hello this is Rock Paper Scissors");
        Console.WriteLine("Enter your step");

        UserWon = false;

        do
        {
            Console.WriteLine("1 - Rock");
            Console.WriteLine("2 - Paper");
            Console.WriteLine("3 - Scissors");
            Console.WriteLine("0 - Exit");

            _roundsPlayed++;

            var userInput = Console.ReadLine(); // "5"

            int userChoice; // there is no value

            // parse user input = string into int 
            // put result to out result param
            // return if parse was successful

            if (!int.TryParse(userInput, out userChoice) || !(userChoice >= 0 && userChoice <= 3))
            {
                Console.WriteLine(userChoice);
                Console.WriteLine("Invalid input");
                continue;
            }

            if (userChoice == 0)
            {
                return;
            }
            
            string userChoiceString = CalculateMoveName(userChoice);
            Console.WriteLine($"You chose {userChoiceString}");
            
            var random = new Random();
            var computerChoice = random.Next(1, 4); // generate random number 1-3

            string computerChoiceString = CalculateMoveName(computerChoice);
            
            //CalculateMoveName()

            Console.WriteLine($"Computer chose {computerChoiceString}");

            var winnerExists = TryCalculateWinner(userChoice, computerChoice, out var winner);
            if (winnerExists)
            {
                Console.WriteLine($"{winner} won the round");
            }
            else
            {
                Console.WriteLine("It's a draw");
            }
            
        } while (_roundsPlayed < RoundsToPlay);
    }

    private string CalculateMoveName(int moveNumber)
    {
        switch (moveNumber)
        {
            case 1:
                return "Rock";
            case 2:
                return "Paper";
            default:
                return "Scissors";
        }
    }

    private bool TryCalculateWinner(int player1Move, int player2Move, out string winner)
    {
        if (player1Move == player2Move)
        {
            winner = "";
            return false; 
        }

        if (player1Move == 1 && player2Move == 3 || player1Move == 2 && player2Move == 1 ||
            player1Move == 3 && player2Move == 2)
        {
            winner = "Player 1";
        }
        else
        {
            winner = "Player 2";
        }

        return true;
    }
}