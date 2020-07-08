using System;
using System.Collections.Generic;


namespace Casino
{
    public abstract class Game
    {

        //NOTE:Here in this class we removed the 'Dealer' overall becuase we realized dealer is very specific to TwentyOneGame.

        //NOTE: The code above used to be :
        //      public List<Player> Players { get; set; }
        //      public Dictionary<Player, int> Bets { get; set; }
        //NOTE: It needed changing because we needed to instantiate an empty property before it
        //      can be used and populated in the program.

        //NOTE: What the The Tech Academy video told me to do (to instantiate properties):
        //      private List<Player> _players = new List<Player>();
        //      private Dictionary<Player, int> _bets = new Dictionary<Player, int>();
        //      public List<Player> Players { get { return _players; } set { _players = value; } }
        //      public Dictionary<Player, int> Bets { get { return _bets; } set { _bets = value; } }

        //NOTE: This is what intellicense told me to do instead. (to instatiate my properties this way
        //      so they can be used in the program). This process must be done with 'collections' properties. 
        public List<Player> Players { get; set; } = new List<Player>();
        public Dictionary<Player, int> Bets { get; set; } = new Dictionary<Player, int>();


        public string Name { get; set; }
        public abstract void Play();
        public virtual void ListPlayers()
        {

            foreach (Player player in Players)
            {
                Console.WriteLine(player.Name);
            }
        }
    }
}
