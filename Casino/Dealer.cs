using System;
using System.Collections.Generic;
using System.IO; //NOTE: This is neede for 'StreamWriter'.
using System.Linq;


namespace Casino
{
    public class Dealer
    {
        public string Name { get; set; }
        public Deck Deck { get; set; }
        public int Balance { get; set; }

        public void Deal(List<Card> Hand)
        {
            //NOTE: 'using System.Linq;' to use the integrated 'First()' method.
            //NOTE: What we are doing here is passing in a 'Deck' of 'Cards' (property of Deck), then getting the 
            //      first card in that list ( using 'First()' ), then adding it to the 'Hand' (which is a list of cards)  
            //	    that was passed in, in the 'Deal' method.
            Hand.Add(Deck.Cards.First());

            //NOTE: Here we added are appending the dealer's cards (all the cards used) to a 'log.txt' file
            string card = string.Format(Deck.Cards.First().ToString() + "\n");
            
            //NOTE: This line is done so that the player can see the card on the console.
            Console.WriteLine(card);

            //NOTE: 'StreamWriter' is what lets us append stings into the 'log.txt' file. 
            //      The first Parameter talkes the path of the file, the second paramerter
            //      tells the function if it should replace or append the string values
            using (StreamWriter file = new StreamWriter(@"C:\Users\jenem\Documents\StudentDrills_Utah\The-Tech-Academy-Basic-C-Sharp-Projects-master\Intro to C-Sharp\TwentyOne\log.txt", true))
            {
                //NOTE: Below we will log the dateTime of each card dealt at the moment that is being dealt with.
                file.WriteLine(DateTime.Now);
                file.WriteLine(card);
            }

            //NOTE: The line below removes the card from the deck, at index 0 ('RemoveAt' is an integrated method for lists).
            Deck.Cards.RemoveAt(0);

        }
        //NOTE: In this class the dealer has a few properties, but a dealer needs to do more.
        //      In blackjack a dealer also has a hand, they also 'stay' and 'bust' so it would
        //      be a nice approach to have a 'TwentyOneDealer' class which inherits 'Dealer'.

    }
}
