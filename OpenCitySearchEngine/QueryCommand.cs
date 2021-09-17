using OpenCitySearchEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenCitySearchEngine
{
  public class QueryCommand : ICommand
  {
    private List<string> _queryExpression;
    private readonly Stack<HashSet<int>> _tokenValueStack;
    private readonly Stack<string> _operatorStack;

    public QueryCommand(string[] commandParts)
    {
      ValidateCommandParts(commandParts);
      _tokenValueStack = new Stack<HashSet<int>>();
      _operatorStack = new Stack<string>();
    }

    // Algorithm Source: https://www.geeksforgeeks.org/expression-evaluation/
    public string GetResponse(IDocumentTokenRepository repository)
    {
      HashSet<int> documentIds = new HashSet<int>();
      int indexer = 0;

      while (indexer < _queryExpression.Count)
      {
        string queryElement = _queryExpression[indexer++];

        if (StringCheckerUtility.IsAlphaNumeric(queryElement))
        {
          _tokenValueStack.Push(repository.GetDocumentsByTokenQuery(queryElement));
        }
        else if (queryElement.Equals("("))
        {
          _operatorStack.Push(queryElement);
        }
        else if (queryElement.Equals(")"))
        {
          while (!_operatorStack.Peek().Equals("("))
          {
            PerformOperationOnValueStack();
          }

          _operatorStack.Pop();
        }
        else
        {
          while (_operatorStack.Count > 0 && !(_operatorStack.Peek().Equals("(") || _operatorStack.Peek().Equals(")")))
          {
            PerformOperationOnValueStack();
          }

          _operatorStack.Push(queryElement);
        }
      }

      while (_operatorStack.Count > 0)
      {
        PerformOperationOnValueStack();
      }

      string queryResults = string.Join(" ", _tokenValueStack.Pop());
      
      return $"query results {queryResults}";
    }

    private void ValidateCommandParts(string[] commandParts)
    {
      if (commandParts.Length < 2)
      {
        throw new Exception("query error due to incomplete command, please try again.");
      }

      StringBuilder queryExpressionSb = new StringBuilder();

      for (int tokenIndexer = 1; tokenIndexer < commandParts.Length; tokenIndexer++)
      {
        queryExpressionSb.Append(commandParts[tokenIndexer]);
      }

      string queryExpression = queryExpressionSb.ToString();

      bool isValidQueryExpression = StringCheckerUtility.IsValidQueryExpression(queryExpression)
        && (!queryExpression.StartsWith("&") || !queryExpression.StartsWith("|"));

      if (!isValidQueryExpression)
      {
        throw new ArgumentException("query error due to invalid query expression, please try again.");
      }

      _queryExpression = Regex.Split(queryExpression, @"([()|&])").Where(s => !string.IsNullOrEmpty(s)).ToList();
    }

    private void PerformOperationOnValueStack()
    {
      string queryOperator = _operatorStack.Pop();
      HashSet<int> tokenValueOne = _tokenValueStack.Pop();
      HashSet<int> tokenValueTwo = _tokenValueStack.Pop();

      if (queryOperator.Equals("|"))
      {
        _tokenValueStack.Push(PerformUnion(tokenValueOne, tokenValueTwo));
      }
      else
      {
        _tokenValueStack.Push(PerformIntersection(tokenValueOne, tokenValueTwo));
      }
    }
    
    private HashSet<int> PerformUnion(HashSet<int> tokenValueOne, HashSet<int> tokenValueTwo)
    {
      HashSet<int> result = tokenValueOne;

      result.Union(tokenValueTwo);

      return result;
    }

    private HashSet<int> PerformIntersection(HashSet<int> tokenValueOne, HashSet<int> tokenValueTwo)
    {
      HashSet<int> result = tokenValueOne;

      result.Intersect(tokenValueTwo);

      return result;
    }
  }

}
