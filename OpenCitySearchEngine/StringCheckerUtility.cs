using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenCitySearchEngine
{
  public static class StringCheckerUtility
  {
    public static bool IsNumeric(string s)
    {
      int output;
      return int.TryParse(s, out output);
    }

    public static bool IsAlphaNumeric(string s)
    {
      return Regex.IsMatch(s, @"^[a-zA-Z0-9]+$");
    }

    public static bool IsValidOperator(string s)
    {
      List<string> validOperators = new List<string>() { "(", ")", "|", "&" };

      return validOperators.Contains(s);
    }

    public static bool IsValidQueryExpression(string s)
    {
      return Regex.IsMatch(s, @"^[a-zA-Z0-9)(|&]+$");
    }
  }
}
