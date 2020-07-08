

namespace Casino
{
    //NOTE: Here we are changinig 'Card' from a 'class' to a 'struct' becuase once we set a value, we don't want it to change.
    //      Also, the Card class doesn't inherit from or to any other classes, which makes it qualify to be struct.
    public struct Card
    {
        public Suit Suit { get; set; }
        public Face Face { get; set; }

        //NOTE: Here we are creating a custom (overriding) a version of an existing 'ToString()' method.
        public override string ToString()
        {
            //NOTE: The point of overriding the method is to be able to call Card.ToString and have it
            //      show us the card object (made up of a 'Face' and a 'Suit') in a nice string that
            //      humans who are playing this game can understand. 
            return string.Format("{0} of {1}", Face, Suit);
        }

    }

    //NOTE: Combinations of the enums below is what helps us create the cards in the 'Deck' class.

    //NOTE: Here we are creating enum 'Suit'.
    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }
    //NOTE: Here we are creating enum 'Face'.
    public enum Face
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}
