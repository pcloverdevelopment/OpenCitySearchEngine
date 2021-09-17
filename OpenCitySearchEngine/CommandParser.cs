using OpenCitySearchEngine.Interfaces;
using System;

namespace OpenCitySearchEngine
{
  public class CommandParser : ICommandParser
  {
    public ICommand DeriveCommand(string input)
    {
      if (string.IsNullOrWhiteSpace(input))
      {
        throw new Exception("Invalid command, please try again.");
      }
      
      string[] commandParts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

      bool isIndexCommand = commandParts[0].Equals("index");
      bool isQueryCommand = commandParts[0].Equals("query");

      if (!(isIndexCommand || isQueryCommand))
      {
        throw new Exception("Invalid command, please try again.");
      }

      ICommand command = null;

      if (isIndexCommand)
      {
        command = new IndexCommand(commandParts);
      }

      if (isQueryCommand)
      {
        command = new QueryCommand(commandParts);
      }

      return command;
    }
  }
}
