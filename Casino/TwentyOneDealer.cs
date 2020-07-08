using System;
using System.Collections.Generic;
using System.Text;

namespace Casino.TwentyOne
{
    public class TwentyOneDealer: Dealer
    {
        //NOTE: The code above used to be :
        //      public List<Card> Hand { get; set; }
        //NOTE: It needed changing because we needed to instantiate an empty property before
        //      can be used and populated in the program.

        //NOTE: What the The Tech Academy video told me to do (to instantiate properties):
        //private List<Card> _hand = new List<Card>();
        //public List<Card> Hand { get { return _hand; } set { _hand = value; } }

        //NOTE: This is what intellicense told me to do instead. (to instatiate my properties this way
        //      so they can be used in the program.) This process must be done with 'collections' properties. 
        public List<Card> Hand { get; set; } = new List<Card>();


        public bool Stay { get; set; }
        public bool isBusted { get; set; }
    }
}
