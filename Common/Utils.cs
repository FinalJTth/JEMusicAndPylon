using System.Text.RegularExpressions;

namespace JEMusicAndPylon.Common
{
    public static class Utils
    {
        public static string[] SplitCamelCase(string source)
        {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }
    }
}
