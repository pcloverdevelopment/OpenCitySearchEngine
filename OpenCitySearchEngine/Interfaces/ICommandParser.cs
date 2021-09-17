namespace OpenCitySearchEngine.Interfaces
{
  interface ICommandParser
  {
    ICommand DeriveCommand(string command); 
  }
}
