using System.Collections.Generic;

namespace OpenCitySearchEngine.Interfaces
{
  public interface IDocumentTokenRepository
  {
    public void IndexDocument(int documentId, List<string> tokens);
    public HashSet<int> GetDocumentsByTokenQuery(string token);
  }
}
