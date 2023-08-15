using System.Linq;

namespace KeepCorrect.Translator.WPF.Extensions;

public static class StringExtensions
{
    public static bool IsText(this string text)
    {
        return text.Trim().Any(ch => ch == ' ');
    }
}