using System;
using System.Text;

namespace hangmanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup:
            bool runGame = true;
            int faultyness = -1;
            int guessType = 0;
            string guess = "";

            //game loop start
            Console.WriteLine("Welcome, todays activities include the game \"hangman\".\n\nThe game will proceed as follows: the computer will pick a word from a list of options and your job as the player is to guess the word.\nYou will guess one letter at a time or if you are confident you may attempt to guess the whole word.\nEvery letter you guess correctly will fill the appropriate space in the \"discovered word\" you will be presented with before each guess.\nWhen you guess and incorrect letter you will loose one of your ten starting lives.");
            while (runGame)
            {
                //individual run setup
                bool win = false;
                int lives = 10;
                string gameWord = words();
                StringBuilder tried = new StringBuilder(); ;
                char[,] gameSave = new char[gameWord.Length, 2];
                gameSave = parArrays(gameWord);
                Console.WriteLine("Press any button to start the game!");
                Console.ReadKey();

                //Guessing
                while (lives != 0 && win == false)
                {
                    discovered(gameWord.Length, gameSave);
                    Console.WriteLine("you have " + lives + " lives remaining!");
                    Console.WriteLine("you have tried the following words and letters: " + tried);
                    Console.WriteLine("Input the letter or word you wish to guess with:");
                    while (faultyness != 0)
                    {
                        guess = Console.ReadLine().ToLower();
                        faultyness = faultCheck(guess, tried);
                        switch (faultyness)
                        {
                            case 0:
                                break;
                            case 1:
                                Console.WriteLine("You must input something to guess, try again.");
                                guess = Console.ReadLine().ToLower();
                                break;
                            case 2:
                                Console.WriteLine("A number is not a valid guess, try again.");
                                guess = Console.ReadLine().ToLower();
                                break;
                            case 3:
                                Console.WriteLine("You have already attempted this guess, try again.");
                                guess = Console.ReadLine().ToLower();
                                break;
                        }
                    }
                    faultyness = -1;
                    tried.Append(guess + ", ");
                    guessType = guessCheck(guess);
                    switch (guessType)
                    {
                        case 0:
                            if (gameWord.Contains(guess))
                            {
                                Console.WriteLine("you have guessed a correct letter!");
                                gameSave = letterCorrect(gameSave, gameWord, guess);
                            }
                            else
                            {
                                Console.WriteLine("That was incorrect, you have lost a life");
                                lives = lives - 1;
                            }
                            break;

                        case 1:
                            if (wordCheck(gameWord, guess))
                            {
                                Console.WriteLine("you have guessed a correct word!");
                                win = true;
                            }
                            else
                            {
                                Console.WriteLine("That was incorrect, you have lost a life");
                                lives = lives - 1;
                            }
                            break;
                    }
                    win = winCheck(gameSave, gameWord);
                }
                //play again?
                runGame = playAgain();
            }
        }

        //This method holds the list of words and picks one at random for the game
        static string words()
        {
            Random rnd = new Random();
            string[] wordList = new string[]
            {
                "distribute",
                "ethnic",
                "craft",
                "tolerate",
                "fitness",
                "interest",
                "insist",
                "regular",
                "cousin",
                "wriggle",
                "demonstration",
                "courage",
                "shadow",
                "fork",
                "beautiful",
                "operational",
                "architecture",
                "concrete",
                "irony",
                "nationalism"
            };
            string pickedWord = wordList[rnd.Next(19)];
            return pickedWord;
        }

        //setup for char arrays
        static char[,] parArrays(string gameWord)
        {
            int length = gameWord.Length;
            char[,] output = new char[length, 2];
            char[] line1 = gameWord.ToCharArray();
            StringBuilder line2 = new StringBuilder("", length);
            for (int i = 0; i < length; i++)
            {
                line2.Append("_");
            }
            string converter = line2.ToString();
            char[] line2Array = converter.ToCharArray();
            for (int i = 0; i < length; i++)
            {
                output[i, 0] = line1[i];
                output[i, 1] = line2Array[i];
            }
            return output;
        }

        //print the word in "as discovered" state
        static void discovered(int length, char[,] gameSave)
        {
            Console.Clear();
            Console.Write("the word as you've discovered it so far is as follows: ");
            for (int i = 0; i < length; i++)
            {
                Console.Write(gameSave[i,1]);
            }
            Console.WriteLine("");
        }

        //Makes sure that the input is a leagal entery
        static int faultCheck(string guess, StringBuilder tried)
        {
            if (guess == "") return 1;
            bool isDouble = double.TryParse(guess, out double i);
            if (isDouble) return 2;
            string converted = tried.ToString();
            if (converted.Contains(guess.ToLower())) return 3;
            return 0;
        }

        //Letter or word check
        static int guessCheck(string guess)
        {
            bool isLetter = char.TryParse(guess, out char i);
            if (isLetter) return 0;
            return 1;
        }
        
        //Letter guessing
        static char[,] letterCorrect(char[,] gameSave, string word, string guess)
        {
            int guessPos = -1;
            char.TryParse(guess, out char guessConverted);
            do
            {
                guessPos++;
                guessPos = word.IndexOf(guessConverted, guessPos);
                if (guessPos != -1) gameSave[guessPos, 1] = guessConverted;
            } while (guessPos != -1);
            return gameSave;
        }

        //Word guessing
        static bool wordCheck(string gameWord, string guess)
        {
            if (guess == gameWord) return true;
            return false;
        }

        //win check
        static bool winCheck(char[,] gameSave, string gameWord)
        {
            int length = gameWord.Length;
            StringBuilder discovered = new StringBuilder("", length);
            for (int i = 0; i < length; i++)
            {
                discovered.Append(gameSave[i, 1]);
            }
            string converted = discovered.ToString();
            if (converted.Contains("_")) return false;
            Console.WriteLine("Congratulations! you have guessed the word and won the game!");
            return true;
        }

        //method to decide if the player would like to play again or to stop the program
        static bool playAgain()
        {
            Console.WriteLine("Would you like to play again? (yes/no)");
            string input = Console.ReadLine().ToLower();
            while (input != "yes" && input != "no")
            {
                Console.WriteLine("What you had input was not a yes or a no, please input an accepted answer.");
                input = Console.ReadLine().ToLower();
            }
            if (input == "yes") return true;
            return false;
        }
    }
}
