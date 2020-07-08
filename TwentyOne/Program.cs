/*
THE TECH ACADEMY PROGRAM
- This Program represents a game known as 'TwentyOne' aka BlackJack.
- This games have aspects of it that demonstrates very well every basic feature in C#
- The end result won't be a perfect program by any means but it will be a good model of the important part of C#.
 */
using System;
using System.Collections.Generic;
using System.Data; //NOTE: This is neede to set SQL databse types.
using System.Data.SqlClient; //NOTE: This is neede to connect and disconnect to/from a database.
using System.IO;
using Casino;
using Casino.TwentyOne;

namespace TwentyOne
{
    class Program
    {
        static void Main(string[] args)
        {
            const string casinoName = "Grand Hotel and Casino";

            Console.WriteLine("Welcome to the {0}. Let's start by telling me your name.", casinoName);
            string playerName = Console.ReadLine();

            //NOTE: For learning and texting purposes, if the user types admin as a name, do this.
            if (playerName.ToLower() == "admin")
            {
                //NOTE: We are calling a local private method, 'ReadExceptions' which returns a list of 'ExceptionEntity' objects
                //      and we are referencing it in variable 'Exceptions'.
                List<ExceptionEntity> Exceptions = ReadExceptions();

                //NOTE: Here we are iterating through each properties in each ExceptionEntity object created, and printing it
                //      as a representation of each record in the 'Exceptions' table in the 'TwentyOneGame' database.
                foreach (var exception in Exceptions)
                {
                    Console.Write(exception.Id + " | ");
                    Console.Write(exception.ExceptionType + " | ");
                    Console.Write(exception.ExceptionMessage + " | ");
                    Console.Write(exception.TimeStamp + " | ");
                    Console.WriteLine();
                }
                Console.Read();
                return;
            }

            //NOTE: Here instead of reciving the information with out it being handled,
            //      eg. 'int bank = Convert.ToInt32(Console.ReadLine());', we created a
            //      validator that uses the 'int.TryParse' method wich returns a boolean
            //      based on whether the first string parameter is an int32. The method
            //      also includes an 'out parameter' of the int result in the case that 
            //      it is a valid int. This is a quick way to validate user input without
            //      having to use exception handling.
            bool validAnswer = false;
            int bank = 0;
            while (!validAnswer)
            {
                Console.WriteLine("And how much money did you bring today?");
                validAnswer = int.TryParse(Console.ReadLine(), out bank); 
                if (!validAnswer) Console.WriteLine("Please enter digits only, no decimals.");
            }
            
            Console.WriteLine("Hello, {0}. Would you like to join a game of 21 right now?", playerName);
            string answer = Console.ReadLine().ToLower();
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                //NOTE: When the user answers yes, it will create a new player.
                //      In the 'Player' class we will create a constructoe that will 
                //      create that player with how much money they are bringing to the game.
                Player player = new Player(playerName, bank);

                //NOTE: Here we are logging a player 'Guid' so we can uniquely identify that player,
                //      and the cards that player has dealt with.
                player.Id = Guid.NewGuid();
                using (StreamWriter file = new StreamWriter(@"C:\Users\jenem\Documents\StudentDrills_Utah\The-Tech-Academy-Basic-C-Sharp-Projects-master\Intro to C-Sharp\TwentyOne\log.txt", true))
                {
                    file.WriteLine(player.Id);
                }

                //NOTE: Here we are using Polymorphism, while creating a 'TwentyOneGame' as also its 
                //      parent class, 'Game' so that it exposes those overloaded operators we 
                //      made in the 'Player' class, which returns a 'Game' object, and to a  
                //      specified game object such as 'TwentyOneGame'.
                Game game = new TwentyOneGame();

                //NOTE: Here we are adding players into a game.
                game += player;

                player.isActivelyPlaying = true;

                //NOTE: 'isActivelyPlaying' and 'player.balabnce' are used as a way to track
                //      wether the player is playing the game or isn't/can't play the game.
                while (player.isActivelyPlaying && player.Balance > 0)
                {

                    //NOTE: This try-cath will catch any errors in the 'Play()' method.
                    try
                    {
                        game.Play();
                    }
                    //NOTE: This is an exception we created which inherits the integrated 'Exception' class.
                    //      It is a great way of 'throwing' a custom exception by when a condition is in
                    //      our program is met.
                    catch (FraudException ex)
                    {
                        //NOTE: In our 'TwentyOneGame' class, we 'throw new FraudException("Security! Kick this person out.");' 
                        //      If a player bet is lower then 0 (a negative number).
                        Console.WriteLine(ex.Message);

                        //NOTE: Here we are calling the method that we created outside of the main method which takes in an exception.
                        UpdateDbWithException(ex);

                        Console.ReadLine();
                        return;
                    }
                    //NOTE: This is the 'Exception' class that will catch any exception if not covered in the 
                    //      TwentyOneGame 'Play()' method.
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occured. Please contact your System Administrator.");

                        //NOTE: Here we are calling the method that we created outside of the main method which takes in an exception.
                        UpdateDbWithException(ex);

                        Console.ReadLine();
                        return;
                    }
                         
                }
                //NOTE: Outside of the while loop we will subtract the  player form the game.
                game -= player;
                Console.WriteLine("Thank You for playing!");
            }
            //NOTE: We won't do an else statement, if they say anything other than 'yes' 'yeah' 'y' & 'ya',
            //      then it will jump to this line.
            Console.WriteLine("Feel free to look around the casino. Bye for now.");
            Console.ReadLine();

            //NOTE: This is all that the main method is going to have.
        }

        //NOTE: This method is private because we only want this class (Program) to have access to it, it is 'static' because it is not
        //      meant for the calling object to be instantiated (we will never instantiate a 'new Program()'), and we don't want
        //      to return anything (it will simply update the database) thus we are using 'void'.
        //NOTE: Here this method takes in any of the exceptions because we are passing in an object 'ex' which is of type 'Exception',
        //      which happens to be C#'s base class for all exceptions. This is a great example of polymorphism.
        private static void UpdateDbWithException(Exception ex)
        {
            //NOTE: What we are doing below is ADO.NET which is part of the system library which deals with database connections.
            //NOTE: Later we will learn about framworks which are built on top of ADO.NET which will make things even easier.
            //NOTE: Right now we will use a 'Connection' string that will allow us to connect to the database. For that we must 
            //      go to the 'SQL Server Object Explorer' window, right click on our 'TwentyOneGame' database, and select
            //      'Properties'. The 'Properties' window will show up and under 'General', you will see a cell named 'Connection String'.
            //      The cell to the right of that 'Connection String' will show string:
            //
            //      "Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            //
            //      We added the '@' character in front of the screen so that it reads the string as is and ignores scape keys.

            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;
                                        Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                        TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";

            //NOTE: Below is our query string.
            //NOTE: Below we are going to use 'Parameterized Queries' which are used to avoid 'Injection Attacks'.
            //      Injection Attacks. Injection attacks can be caused by inserting user input directly into a query
            //      and the user misusing it deliverately for an attack on the system. 'Parameterized Queries' helps
            //      validate the data that is being entered preventing misuse.
            string queryString = @"INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) VALUES
                                    (@ExceptionType, @ExceptionMessage, @TimeStamp)";

            //NOTE: 'using' is a way of controlling 'unmanaged code' or 'unmanaged resources'.  When you are inside of a program in the
            //      .NET Framwork in the 'Common Language Runtime' and you go outsode of it, to get something else is risky. You are 
            //      openning up resources which could use memeory (unexpected shut downs). The 'Common Language Runtime' is worried about that.
            //NOTE: Whenever you open this connections you must always close them and you can achieve that by using keyword, 'using'.
            //NOTE: The 'using' statement we are using below is different from the 'using' statements above for namespaces. 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //NOTE: Here we are creating a 'command' object which takes the query and the connection strings.
                SqlCommand command = new SqlCommand(queryString, connection);

                //NOTE: Here we are adding the parameter placeholders and their data types to 'Command'.
                //NOTE: "@ExceptionType" is a placeholder for one of the parameters, and in 'SqlDbType.VarChar', we are naming its data
                //      type (VarChar) to protect the SQL code from SQL injections.
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar);
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar);
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime);


                //NOTE: Here we are adding the Values to the parameter

                //NOTE: Storing a string of the name of the data type of object 'ex'.
                command.Parameters["@ExceptionType"].Value = ex.GetType().ToString();

                //NOTE: Storing the message of the exception, 'ex' (which is  already of type string). 
                command.Parameters["@ExceptionMessage"].Value = ex.Message; 

                //NOTE: Storing the date and time when this line gets executed as a DateTime value.
                command.Parameters["@TimeStamp"].Value = DateTime.Now;

                connection.Open();

                //NOTE: A query would be when you are querying the database, like using a 'SELECT' statement. This  is a non-query because
                //      we are using an 'INSERT' statement.
                command.ExecuteNonQuery(); 

                connection.Close();
            }


        }
        private static List<ExceptionEntity> ReadExceptions()
        {
            string connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=TwentyOneGame;
                                        Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                        TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";
            //NOTE: Please be aware that in bigger applicaions you won't have a connection string like this, and
            //      You'll be using a method to call a piece of information from your configuration file that will 
            //      appoint you to that connection string. The reason for this is that if for whatever reason that string
            //      changes, you would have to change this string throught your entire program. (Just something to be aware of).

            //NOTE: This is our query string selects four entities in the 'Exceptions' table in the 'TwentyOneGame' Database.
            string queryString = @"Select Id, ExceptionType, ExceptionMessage, TimeStamp From Exceptions";


            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();

            //NOTE: Here we are instantiating an SqlConnection.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //NOTE: Here we are creating a 'command' object which takes the query and the connection strnigs.
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                //NOTE: After openning the connection to the database, creating an 'SqlDataReader' object by 
                //      calling the 'ExecuteReader' method (which sends the SqlCommand.CommandText to the SqlCommand.Connection
                //      and returns a SqlDataReader object) from the 'command' object we instantiated.
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    //NOTE: What 'reader.Read()' will do is, it loops through each record. It returns 'true' if there are more rows, otherwise 
                    //      it will return false.
                    //NOTE: So for each record, we will instantiate an 'ExceptionEntity' object (from the 'ExceptionEntity' that we created),
                    //      then we will user the 'reader' object to read and assign each entity (Id, ExceptionType, ExceptionMessage, TimeStamp) to
                    //      its corresponding 'ExceptionEntity' class property.
                    //NOTE: Basically we are mapping each database record to our object.
                    ExceptionEntity exception = new ExceptionEntity();

                    //NOTE: Since we are getting Sql back, we need to make sure to typecast each column-entity value to its the corresponding type
                    //      that our object properties are expecting.
                    exception.Id = Convert.ToInt32(reader["Id"]);
                    exception.ExceptionType = reader["ExceptionType"].ToString();
                    exception.ExceptionMessage = reader["ExceptionMessage"].ToString();
                    exception.TimeStamp = Convert.ToDateTime(reader["TimeStamp"]);
                    //NOTE: The only problem about using ADO.NET where we are typing the column names, there is no intellisense that will let you know
                    //      if it is typed in incorrectly, and the developer can only find out something is wrong until after running the program.

                    //NOTE: After assigning values to the current object, we begin to append each exception entity to our 'Exceptions' list.
                    Exceptions.Add(exception);
                }
                connection.Close();
                
            }
            //NOTE: Here we are returning the 'exceptions' object which is a list of 'ExceptionEntity' objects that will contain our entities
            //      from our databse as properties.
            return Exceptions;
        }

    }
}
