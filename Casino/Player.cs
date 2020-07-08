using System;
using System.Collections.Generic;


namespace Casino
{
    public class Player
    {

        public Player(string name) : this (name, 100)
        {
        }
        //NOTE: Below is a 'Player' constructor (Constructors go at the top of the class).
        //      Here we have a constructor that takes two arguments and we are assigning them
        //      to properties in this object.
        public Player(string name, int beginningBalance)
        {
            Hand = new List<Card>();
            Balance = beginningBalance;
            Name = name;
        }

        //NOTE: The code above used to be :
        //      public List<Card> Hand { get; set; }
        //NOTE: It needed changing because we needed to instantiate an empty property before it
        //      can be used and populated in the program.

        //NOTE: What the The Tech Academy video told me to do (to instantiate properties):
        //private List<Card> _hand = new List<Card>();
        //public List<Card> Hand { get { return _hand; } set { _hand = value; } }

        //NOTE: This is what intellicense told me to do instead. (to instatiate my properties this way
        //      so they can be used in the program).This process must be done with 'collections' properties. 
        public List<Card> Hand { get; set; } = new List<Card>();


        public int Balance { get; set; }
        public string Name { get; set; }
        public bool isActivelyPlaying { get; set; }

        //NOTE: Now we are adding a property for player called 'Stay', which is an action more for
        //      'TwentyOneGame' (eg. hit or stay) than any other game. In reality we would normally
        //      create a 'TwentyOnePlayer' class , but it is decided to do this so we don't have
        //      to create so many classes.
        public bool Stay { get; set; }

        //NOTE: Here we are creating a Global Unique Identifier property for 'Player'.
        //NOTE: The purpose of Guid is to generate an identifier and it applies particularly well
        //      with people. The odds of generating a repeated Guid are astronomical due to the
        //      complexity and how large a Guid is.
        public Guid Id { get; set; }

        //NOTE: Here we are creating a method that a 'Player' object can call, that would allow it to
        //      bet for the game or not.
        public bool Bet(int amount)
        {
            if (Balance - amount < 0)
            {
                Console.WriteLine("You do not have enought to place a bet that size.");
                return false;
            }
            else
            {
                //NOTE: If we get to this code, then the bet has been successfully placed and the
                //      Player balance has been subtracted by the betting amount.
                Balance -= amount;
                return true;
            }
        }
        public static Game operator+ (Game game, Player player)
        {
            game.Players.Add(player);
            return game;
        }
        public static Game operator- (Game game, Player player)
        {
            game.Players.Remove(player);
            return game;
        }

    }
}
