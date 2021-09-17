namespace OpenCitySearchEngine.Interfaces
{
  public interface ICommand
  {
    public string GetResponse(IDocumentTokenRepository repository);
  }
}
