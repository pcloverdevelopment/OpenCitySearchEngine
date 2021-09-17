using OpenCitySearchEngine.Interfaces;
using System;
using System.Collections.Generic;

namespace OpenCitySearchEngine
{
  internal class IndexCommand : ICommand
  {
    private int _documentId;
    private List<string> _tokens;

    public IndexCommand(string[] commandParts)
    {
      ValidateAndSetCommandParts(commandParts);
    }

    public string GetResponse(IDocumentTokenRepository repository)
    {
      repository.IndexDocument(_documentId, _tokens);

      return $"index ok {_documentId}";
    }

    private void ValidateAndSetCommandParts(string[] commandParts)
    {
      if (commandParts.Length < 3)
      {
        throw new ArgumentException("index error due to incomplete command, please try again.");
      }

      if (!StringCheckerUtility.IsNumeric(commandParts[1]))
      {
        throw new ArgumentException("index error due to invalid document id, please try again with a numeric value.");
      }

      List<string> tokens = new List<string>();

      for (int tokenIndexer = 2;  tokenIndexer < commandParts.Length; tokenIndexer++)
      {
        if (!StringCheckerUtility.IsAlphaNumeric(commandParts[tokenIndexer]))
        {
          throw new ArgumentException($"index error due to invalid token: ({commandParts[tokenIndexer]}), please try again with only alphanumeric tokens.");
        }

        tokens.Add(commandParts[tokenIndexer]);
      }

      _documentId = int.Parse(commandParts[1]);
      _tokens = tokens;
    }
  }
}
