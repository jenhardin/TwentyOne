using System.Collections.Generic; //NOTE: This is needed for any Lists.
using System.Linq; //NOTE: This is needed for lambda expressions.


namespace Casino.TwentyOne
{
    //NOTE: The purpose for creating is to call a bunch of methods associated with the TwentyOneGame.
    public class TwentyOneRules
    {
        //NOTE: Now we a re going to create a dictionary of card values.
        //NOTE: So far everything we have made has been public because we wanted everything to be accessible.
        //      But right now we want to make this dictionary private beacuse if we mark it public and somebody later
        //      wanted to use that same name, it could couse a conflict .

        //NOTE: The reason for private methods, or classes are so that classes outside of the class where it is being
        //      created don't have access to it. If it is being marked private in this class for example, only this
        //      class, 'TwentyOneRules' can call it (because it is only meant to be used inside of this class). Now if
        //      we made something public in this class, that means it is meant for classes/methods not only to have access 
        //      to it inside of this class, but also for the classes outside of this class to also have access
        //      (to be able to call it).

        //NOTE: This was made 'static' becuase we don't want to have to create a 'TwwntyOneRelus' object in order to
        //      access this. All of the methods we are going to create below are static because they are just basic helper 
        //      methods that we are going to call to perform some logic inside of our game.
        //NOTE: A Name converntion for Private classes is to use an underscore, '_', ahead of it. This will be a dictionary
        //      had the 'Face' as a key and int as a the value for that key.
        //NOTE: So technically we are instantiating a new deictionary and with all of our objects.
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {
            //NOTE: The reason why we didn't use an enum for values (even though enums can hold number values to them) 
            //      is because it doesn't handle dulicate values, so in our situation it using it won't work for us.
            //NOTE: Below we are creating a dictionary of all the cards and their values, that way we can calculate the 
            //      value of our cards, so we can calculate the value of our hand.
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,

            //NOTE: Below we assigned 'Ace' a value of 1 and we will program the player to decide to add add 10 
            //      to it in a specific method since in TwentyOne Ace can be 1 or 11 depending which ever is 
            //      better for the player to play.
            [Face.Ace] = 1
        };

        //NOTE: This method was marked private because it was meant to only be accessed by this class 
        //      (called in this class).
        private static int[] GetAllPossibleHandValues(List<Card> Hand)
        {
            //NOTE: 'aceCount' will hold the amount of Aces on 'Hand'.
            int aceCount = Hand.Count(x => x.Face == Face.Ace);

            //NOTE: 'result' will hold an new empty array with the dynamic length of 
            //      the amount of Aces on 'Hand' plus 1.
            int[] result = new int[aceCount + 1];

            //NOTE: 'value' holds an integer of the sum of all card values on hand
            //      (with 'Aces', if any, valued as 1 in the addition of cards).
            int value = Hand.Sum(x => _cardValues[x.Face]);

            //NOTE: 'result' array, which has a size of 1 plus however many Aces there are in the 'Hand', 
            //      will have the the sum of all card values (with 'Aces', if any, valued as 1 in 
            //      the addition of cards) in index 0.
            //      
            result[0] = value;

            //NOTE: If the length of the 'result' array is 1, then there are No Aces (since having an Ace or 
            //      Aces, would have generated an array with a length of 2 or more).
            if (result.Length == 1) return result;
            

            //NOTE: If we reached this code and sucessfully enter the for loop, means that the 'result'
            //      array's length is definitely 2 or higher (which mean there at least 1 Ace card in the hand).
            for (int i = 1; i < result.Length; i++)
            {
                //NOTE: The code below adds the Ace card located in the second index reference, '1', multiplies that time 10,
                //      that '20' gets added to the current 'value', which holds the sum of the hand (including that Ace card
                //      that has a value of 1) and that gets stored back into 'value'. Then that new 'value' value will get stored,
                //      in that current index in the current iteration (which is the second position in the array) in the  'result'
                //      array.
                value += (i * 10);
                result[i] = value;
            }

            //NOTE: Essentially the array, 'result' in each index, will hold all the possible ways (card sums) you 
            //      can play with the current hand (considering that there will be more than one way to add the 
            //      cards because of 'Aces'.
            return result;

        }

        //NOTE: BlackJack only happens when the player's first two card hand gives them a win (sums to 21).
        public static bool CheckForBlackJack(List<Card> Hand)
        {
            //NOTE: First we call the method 'GetAllPossibleHandValues' (which returns an 
            //      array with all the possible ways (card sums) you can play the 'Hand'.
            int[] possibleValues = GetAllPossibleHandValues(Hand);

            //NOTE: If a player gets a BlackJack, 21 will always be your the highest value in 
            //      the array ('possibleValues'). So we take that max value from the 'possibleValues'
            //      array ans store it in 'value'.
            int value = possibleValues.Max();

            //NOTE: If 'value' is 21 then this method will return true, else it will return false.
            if (value == 21) return true;
            else return false;
        }

        //NOTE: We needed a method that when the player did too many 'hits' and their cards add up to
        //      more than 21, then their hand is 'busted'.
        public static bool IsBusted(List<Card> Hand)
        {
            //NOTE: This method takes in a 'Hand' (List of card) then we us the method we created in this class,
            //      'GetAllPossibleHandValues', and checks if the lowest value in the array that was returned
            //      and store that to int, 'value'.
            int Value = GetAllPossibleHandValues(Hand).Min();

            //NOTE: If the minimum value of the array that contains all possible hand values to play is more than 21, then 
            //      'isBusted' will return 'true'.
            if (Value > 21) return true;
            else return false;
        }

        //NOTE: The rule that we wanna give this dealer is, if the and is above 16 and below 22 (between the values of 17 and 21),
        //      Then the dealer will stay.
        public static bool ShouldDealerStay(List<Card> Hand)
        {
            int[] possibleHnadValues = GetAllPossibleHandValues(Hand);
            foreach (int value in possibleHnadValues)
            {
                if (value > 16 && value < 22)
                {
                    return true;
                }
            }
            return false;
        }

        //NOTE: This method was created so that we can figure out if the player won (returns true), 
        //      lost (returns false) or tied (returns null).
        //NOTE: Normally bools are not nullable (cannot equal to 'null'), but after adding the 
        //      question mark like this 'bool?' on the returning type when declaring a method.
        public static bool? CompareHands(List<Card> PlayerHand, List<Card> DealerHand)
        {
            int[] playerResults = GetAllPossibleHandValues(PlayerHand);
            int[] dealerResults = GetAllPossibleHandValues(DealerHand);

            int playerScore = playerResults.Where(x => x < 22).Max();
            int dealerScore = dealerResults.Where(x => x < 22).Max();

            if (playerScore > dealerScore) return true;
            else if (playerScore < dealerScore) return false;
            else return null;
        }



    }
}
