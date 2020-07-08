using System;
using System.Collections.Generic;

namespace Casino
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();

            //NOTE: For the most part we changed everything is this 'Deck' constructor after changing card values from simple 'string' 
            //      data types to 'enum' data types.

            //NOTE: Again, we will approach creating a deck of cards with a nested for loop.
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    //NOTE: with our logic we will instantiate a card 52 times.
                    Card card = new Card();

                    //NOTE: Here we are type casting int 'i' to an enum type, 'Face',  and int 'j' to an enum type, 'Suit'. These two lines
                    //      of code will store the name of the current 'face' and 'suit' in this iteration and store it in the 'Face' and 'Suit' 
                    //      properties of that current card objectin the iteration.
                    card.Face = (Face)i;
                    card.Suit = (Suit)j;

                    //NOTE: Here we are adding the card which will have a combination of a 'Face' enum and a 'Suit' enum. This will happen
                    //      52 times, adding  52 cards to the 'Cards' property (wich is a list of card objects).
                    Cards.Add(card);
                }
            }
        }
        public List<Card> Cards { get; set; }

        //NOTE: Here we had to make a couple of changes to the 'Shuffle' method since it was preoviously in the 'Program' class.
        //      Now the 'Shuffle' method is no longer 'static' because an object of the (Deck) class where this method resides in , is what
        //      is going to call this method. Since, a deck object can change, it's method can no longer be fixed/'static'.
        //NOTE: The 'Shuffle' method will no longer to take in a 'deck' to return a deck, it will only shuffle the cards, applying it to
        //      the deck object that called it, thus, the method must now be void 'void'. Also, this method is no longer going have 
        //      an 'out parameter' because it is no longer needed, and it will only have an 'optional parameter' to take in.
        //NOTE: Another major change was that since we are now in the 'Deck' class, we no longer need to refernce deck.Cards to get a hold
        //      the Deck class property ('Cards'), which is needed to creata a new list of cards however many times the user wants to.
        public void Shuffle(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                List<Card> TempList = new List<Card>();
                Random random = new Random();
                while (Cards.Count > 0)

                {
                    int randomIndex = random.Next(0, Cards.Count);
                    TempList.Add(Cards[randomIndex]);
                    Cards.RemoveAt(randomIndex);
                }
                Cards = TempList;
            }
        }

    }
}
