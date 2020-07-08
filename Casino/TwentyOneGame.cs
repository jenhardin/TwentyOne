using System;
using System.Collections.Generic;
using System.Linq; //NOTE: This is needed to use lambda expresssions.
using Casino.Interfaces;

namespace Casino.TwentyOne
{
    public class TwentyOneGame : Game, IWalkAway
    {
        //NOTE: Here we added a 'Dealer' property because we removed it from the 'Game' class,
        //      becuase a delaer is more associated with the Game 'TwentyOneGame' and is now
        //      of a 'Dealer' object.
        public TwentyOneDealer Dealer { get; set; }
        public override void Play()
        {
            //NOTE: Now we are going to work on this 'Play()' method, where pretty much everything happens.

            //NOTE: Here we creating a new dealer.
            Dealer = new TwentyOneDealer();

            //NOTE: We are building this with a list of players in mind however, there will only be one player.
            //      It is good practice just in case we want to add more players the future.
            foreach(Player player in Players)
            {
                //NOTE: Here we are giving a new hand to each player, every time 'Play()' is called.
                player.Hand = new List<Card>();

                //NOTE: Here we are just making sure that that the 'Stay' property is false 
                //      for all players at the start of of the game.
                player.Stay = false;
            }

            //NOTE: We also want the dealers hand to have a new hand, every time 'Play()' is called.
            Dealer.Hand = new List<Card>();

            //NOTE: Here we are making sure the dealer's 'Stay' value is 'false' at the start of the game.
            Dealer.Stay = false;

            //NOTE: Here we are giving assigning the dealer a new deck of cards. 
            Dealer.Deck = new Deck();

            //NOTE: Here we are shuffling the dealer's deck which is being used for the game.
            Dealer.Deck.Shuffle();

            foreach (Player player in Players)
            {
                //NOTE: Here it uses the same validation logic as 'Program.cs' under 'TwentyOne'
                bool validAnswer = false;
                int bet = 0;
                while (!validAnswer)
                {
                    Console.WriteLine("Place your bet!");
                    validAnswer = int.TryParse(Console.ReadLine(), out bet);
                    if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals");
                }

                //NOTE: Here we are checking if the player's bet is negative, and if it is, the user
                //      is probably trying to commit a fraudulent act.
                if (bet < 0)
                {
                    //NOTE: Here we are deliberately 'throwing' an exception we created, 'FraudException' which
                    //      inherits from the intergated c# 'Exception' class.
                    //NOTE: If the line below gets executed, the try-catch in 'Program.cs' will 'catch' the exception,
                    //      and proceed with specific instructions.
                    throw new FraudException("Security! Kick this person out.");
                    //NOTE: Our FraudException class has a 'Constructor Chain Call' so that if the parameter passes in a string, 
                    //      for e.g. if we had FraudException object, 'ex', 'ex.Message' boue be equal to string, "Security! Kick this person out."
                }

                //NOTE: We created a bet method in the 'Player class' which takes in a 'bet' int  and
                //returns a boolean based on the player's balance.
                bool successfullyBet = player.Bet(bet);

                if (!successfullyBet)
                {
                    //NOTE: Even though this is a void method we can still have it retun nothing.
                    //      All the line below, 'return;', is doing is stopping the code from
                    //      continuing to run.
                    return;
                }

                //NOTE: If no 'if (!successfullyBet)' doesn't get hit, the code below happens.
                //NOTE: Now we gotta figure out how to organize and keep track of which players made
                //      which bet. One of the best ways to do that is by using a dictionary because
                //      dictionaries are made up of a collection of key-value pairs. The key would be 
                //      the player, and the value would be the player's bet. Then whoever wins doesn't lose
                //      on their balance (those who lose, will) and gains additional cash.
                //NOTE: We created a 'Bets' dictionry property in the 'Game' class, which will hold 
                //      take in player objects as keys and an int (the player's bet) for the value.
                //NOTE: Below we have added the 'player' object as the key and assigned the 'bet' value to to
                //      the 'Bets' dictionary.
                Bets[player] = bet;
            }

            //NOTE: In Most 21 games, the dealers cards are both face down or one of them face up. In 
            //      ours, we are going to make both face up (just for simplicity)

            //NOTE: Here we are giving each player a card twice. So all players end up with two on hand.
            for (int i = 0; i < 2; i++)
            {
                //NOTE: In this 'foreach' loop we are checking any players if the have a have a blackjack.
                Console.WriteLine("Dealing...");
                foreach (Player player in Players)
                {
                    Console.Write("{0}: ", player.Name);

                    //NOTE: What the 'Deal' method does is take in a players hand (which is empty at this point) and 
                    //      removes one card out of the dealers 'Deck' property (which is a list of cards) in index 0 and 
                    //      adds it to the player hand. It also displays which card that is on the console.
                    Dealer.Deal(player.Hand);

                    //NOTE: Now we are cheking for a BlackJack (if the players has a hand of 21 with the first 2 cards).
                    //NOTE: Since we are inside of the for loop, i = 1 which means 'as soon as the player has two cards on hand'.
                    if (i == 1)
                    {
                        //NOTE: Now we will call the 'CheckForBlackJack' method in class, 'TwentyOneRules'. We are able to
                        //      call the method this way without instantiating 'TwentyOneRules' because the method, 'CheckForBlackJack'
                        //      is static (objects of the method's class can't be instantiated). Fun fact, this method is private thus 
                        //      only 'TwentyOneRules' has access to it, but this class is able to access 'TwentyOneRules' because  
                        //      'TwentyOneRules' is public.
                        //NOTE: 'CheckForBlackJack' returns a bool (true or false) and stores it in bool, 'blackJack'.
                        bool blackJack = TwentyOneRules.CheckForBlackJack(player.Hand);

                        if (blackJack)
                        {
                            //NOTE: If this code was executed, that means bool, 'blackJack' was true.
                            //NOTE: The current 'player' wins 150% of their bet.
                            double balanceWon = Bets[player] * 1.5;

                            Console.WriteLine("BlackJack! {0} wins {1}", player.Name, balanceWon);

                            //NOTE: 150% of their bet plus their bet gets added to the current player's account.
                            player.Balance += Convert.ToInt32(balanceWon + Bets[player]);

                            //NOTE: The current game finishes.
                            return;
                        }
                    }
                }
                //NOTE: This happens when blackjack doesn't happen for the player.

                Console.Write("Dealer: ");

                //NOTE: Here the dealer is getting a hand from a deck (two cards), they get displyed on the console.
                Dealer.Deal(Dealer.Hand);

                //NOTE: This is a similar logic to the player getting a BlackJack, except its the dealer this time.
                if (i == 1)
                {
                    bool blackJack = TwentyOneRules.CheckForBlackJack(Dealer.Hand);
                    if (blackJack)
                    {
                        Console.WriteLine("Dealer has BlackJack! Everyone loses!");

                        //NOTE: 'KeyValuePair' is technically a dictionary type.
                        foreach (KeyValuePair<Player, int> entry in Bets)
                        {
                            //NOTE: if we got all the way to this line of the code that means the dealer
                            //      got a blackjack an everyone literally loses.
                            //NOTE: The code below is all of the player bets being added to the dealer's
                            //      balance.
                            Dealer.Balance += entry.Value;
                        }
                        //NOTE: The current game finishes.
                        return;
                    }
                }
            }

            //NOTE: Here the playes are deciding whether to stay or hit.
            foreach (Player player in Players)
            {
                //NOTE: Player.Stay was set to false earlier
                while (!player.Stay)
                {
                    Console.WriteLine("Your cards are: ");
                    foreach (Card card in player.Hand)
                    {
                        //NOTE: Here 'Console.Write' was used instead of 'Console.WriteLine' So that the players
                        //      hand can be shown th the console in the same line, iteration after iterarion.
                        Console.Write("{0} ", card.ToString());
                    }
                    Console.WriteLine("\n\nHit or Stay?");
                    string answer = Console.ReadLine().ToLower();
                    if (answer == "stay")
                    {
                        player.Stay = true;
                        break;
                    }
                    else if (answer == "hit")
                    {
                        Dealer.Deal(player.Hand);
                    }
                    bool busted = TwentyOneRules.IsBusted(player.Hand);
                    if (busted)
                    {
                        Dealer.Balance += Bets[player];
                        Console.WriteLine("{0} Busted! You lose your bet of {1}. Your balance isnow {2}.", player.Name, Bets[player], player.Balance);
                        Console.WriteLine("Do you want to play again?");
                        answer = Console.ReadLine().ToLower();
                        if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
                        {
                            player.isActivelyPlaying = true;

                            //NOTE: The current game finishes.
                            return;
                        }
                        else
                        {
                            player.isActivelyPlaying = false;

                            //NOTE: The current game finishes.
                            return;
                        }
                    }
                }
            }

            Dealer.isBusted = TwentyOneRules.IsBusted(Dealer.Hand);

            Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
            while(!Dealer.Stay && !Dealer.isBusted)
            {
                Console.WriteLine("Dealer is hitting...");
                Dealer.Deal(Dealer.Hand);
                Dealer.isBusted = TwentyOneRules.IsBusted(Dealer.Hand);
                Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
            }
            if (Dealer.Stay)
            {
                Console.WriteLine("Dealer is staying");
            }
            if (Dealer.isBusted)
            {

                Console.WriteLine("Dealer Busted!");

                //NOTE: Here where are iterating through keyValuePairs in 'Bets'
                foreach (KeyValuePair<Player, int> entry in Bets)
                {
                    Console.WriteLine("{0} won {1}!", entry.Key.Name, entry.Value);

                    //NOTE: In the comment below, I am explaining what this lambda expression is doing.
                    //NOTE: Now we are looking a the 'Players' which is a property of the 'Game' class and
                    //      is a list containig 'player' objects. 'Where' will return a List that meets a condition.
                    //      The condition is for each player name in 'Players', if it is the same as the current
                    //      iteration of a key (specifically the players name) in the 'Bets' dictionary, then
                    //      'First()' takes the first element in that list (Which is the player matched), takes
                    //      the 'Balance' of that player, and that balance gets updated. We take what the 
                    //      player bet ('entry.Value'), and times it 2 then add that to the players balance.
                    //NOTE: This is gonna happen for each bet in the 
                    Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2);

                    //NOTE: now we are taking the dealer and minus the bet from the delaer.
                    Dealer.Balance -= entry.Value;
                }
                //NOTE: This return will end the round.
                return;
            }
            foreach (Player player in Players)
            {
                //NOTE: The question mark after bool enables that boolean to be nullable (can hanve a value 
                //      equals to null. The reason we would want to do this is because a tie is possible in a
                //      this game, which means if bool, 'PlayerWon' is neither true or false then it must be 'null',
                //      which we can now program for that scenario to signify a tie.
                bool? playerWon = TwentyOneRules.CompareHands(player.Hand, Dealer.Hand);
                if (playerWon == null)
                {
                    Console.WriteLine("Push! No one wins.");
                    player.Balance += Bets[player];
                    Bets.Remove(player);
                }
                else if (playerWon == true)
                {
                    Console.WriteLine("{0} won {1}!", player.Name, Bets[player]);
                    player.Balance += (Bets[player] * 2);
                    Dealer.Balance -= Bets[player];
                }
                else
                {
                    Console.WriteLine("Dealer wins {0}!", Bets[player]);
                    Dealer.Balance += Bets[player];

                }
                Console.WriteLine("Play Again?");
                string answer = Console.ReadLine().ToLower();
                if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
                {
                    player.isActivelyPlaying = true;
                }
                else
                {
                    player.isActivelyPlaying = false;
                }
            }
        }
        
        public override void ListPlayers()
        {
            Console.WriteLine("21 Players");
            base.ListPlayers();
        }
        public void WalkAway(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
