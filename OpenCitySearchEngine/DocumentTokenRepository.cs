using OpenCitySearchEngine.Interfaces;
using System;
using System.Collections.Generic;

namespace OpenCitySearchEngine
{
  public class DocumentTokenRepository : IDocumentTokenRepository
  {
    private readonly Dictionary<string, HashSet<int>> _tokenToDocumentDictionary;
    private readonly Dictionary<int, HashSet<string>> _documentToTokenDictionary;

    public DocumentTokenRepository()
    {
      _tokenToDocumentDictionary = new Dictionary<string, HashSet<int>>();
      _documentToTokenDictionary = new Dictionary<int, HashSet<string>>();
    }

    public HashSet<int> GetDocumentsByTokenQuery(string token)
    {

      HashSet<int> documentIds;
      
      bool hasDocsForToken = _tokenToDocumentDictionary.TryGetValue(token, out documentIds);

      if (!hasDocsForToken)
      {
        throw new Exception($"query error due to {token} not found in the index, please try again.");
      }

      return documentIds;
    }

    public void IndexDocument(int documentId, List<string> tokens)
    {
      if (IsDocumentAlreadyIndexed(documentId))
      {
        HashSet<string> oldTokenToDocumentAssociations = _documentToTokenDictionary.GetValueOrDefault(documentId);

        RemoveTokenToDocumentIndexInfo(documentId, oldTokenToDocumentAssociations);
        RemoveDocumentToTokenIndexInfo(documentId);
      }

      AddTokenToDocumentIndexInfo(documentId, tokens);
      AddDocumentToTokenIndexInfo(documentId, tokens);
    }

    private bool IsDocumentAlreadyIndexed(int documentId)
    {
      return _documentToTokenDictionary.ContainsKey(documentId);
    }

    private void RemoveTokenToDocumentIndexInfo(int documentId, HashSet<string> tokens)
    {
      HashSet<int> documentSet;

      foreach(string token in tokens)
      {
        documentSet = _tokenToDocumentDictionary.GetValueOrDefault(token);

        if (documentSet != null)
        {
          documentSet.Remove(documentId);
        }
      }
    }

    private void AddTokenToDocumentIndexInfo(int documentId, List<string> tokens)
    {
      HashSet<int> documentSet;

      foreach(string token in tokens)
      {
        bool hasToken = _tokenToDocumentDictionary.ContainsKey(token);

        if (hasToken)
        {
          documentSet = _tokenToDocumentDictionary.GetValueOrDefault(token);
        }
        else
        {
          documentSet = new HashSet<int>();
          _tokenToDocumentDictionary.Add(token, documentSet);
        }

        documentSet.Add(documentId);
      }
    }

    private void RemoveDocumentToTokenIndexInfo(int documentId)
    {
      _documentToTokenDictionary.Remove(documentId);
    }

    private void AddDocumentToTokenIndexInfo(int documentId, List<string> tokens)
    {
      HashSet<string> tokenSet = new HashSet<string>(tokens);
      _documentToTokenDictionary.Add(documentId, tokenSet);
    }
  }
}
