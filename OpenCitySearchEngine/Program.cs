using OpenCitySearchEngine.Interfaces;
using System;

namespace OpenCitySearchEngine
{
  class Program
  {
    static void Main(string[] args)
    {
      // What's a command-line program without a good intro message?? :)
      Console.WriteLine("Welcome to the OpenCity Search Engine 6000! You can now start storing documents and tokens...\n"); 

      bool continueProgramExecution = true;
      ICommandParser parser = new CommandParser();
      IDocumentTokenRepository repository = new DocumentTokenRepository();

      while (continueProgramExecution)
      {
        try
        {
          string input = Console.ReadLine();

          if (input.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
          {
            continueProgramExecution = false;
          }

          ICommand command = parser.DeriveCommand(input);

          string response = command.GetResponse(repository);

          Console.WriteLine($"{response}\n");
        }
        catch (Exception e)
        {
          Console.WriteLine($"{e.Message}\n");
        }
      }
    }
  }
}
