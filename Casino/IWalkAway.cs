
namespace Casino.Interfaces
{
    /*NOTE: An interface is very similar to an abstract class in that there are no implementation details. How are abstract
            classes differnt from Interfaces? Well in the .NET framework is built in a way that one class can inherit from 
            only one other class. Thus, classes cannot have multiple base classes. 
            eg: 'public class TwentyOneGame : Game, Dealer' (implying 'Dealer is a base class')
      NOTE: Not being eable to have a class inherit from more than one class can be limiting to a programmer. For example, if you wanted to
            tweak an abstract class so it meets a group of other classes needs. You wouldn't want to change that abstract class becuase now
            you'd be unnecesarily and unwillingly tweak other classes that weren't meant to be changed. So, since multiple inheritance is 
            needed, that's how an 'interface' came about. The .NET framwork supports multipule inheritance of interfaces. In other words,
            you can only inherit one base class, but in addion to that base class, you can iherit as many interfaces as you want.
            */
    //NOTE: Everything is public in an interface, so you don't have to write 'public' in front.
    //NOTE: A naming convention for Interfaces is to start them with the capital letter 'I' so that everyone knows immediately it is an interface.
    interface IWalkAway
    {
        //NOTE: Similarly to abstract methods in abstract classes, in an interface, the inheriting class 'MUST' include the 
        //      method and its implementation.
        //NOTE: See 'TwentyOneGames.cs' and you can see how 'TwentyOneGame' class can inherit 'Game' class and 'IWalkAway' Inteface.
        //      You will also be able to see the 'mandatory' 'WalkAway' method inside of that class with its implementation.
        void WalkAway(Player player);
    }
}
