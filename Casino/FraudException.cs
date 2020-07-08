using System;

namespace Casino
{
    //NOTE: Here we have two exceptions, one overloading the other one and inheriting the same behavior that
    //      the base class, Exeption, has. 
    public class FraudException : Exception
    {
        //NOTE: This is a good example of a 'Constructor Chain Call'.
        public FraudException()
            : base() { } //NOTE: What this line is doing is inheriting from the base exception which is class, 'Exception'.

        public FraudException(string message)
            : base(message) { }  //NOTE: Initializes a new instance of the System.Exception class with a specified error message.    
    }
}
