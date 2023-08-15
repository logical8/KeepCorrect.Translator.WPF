using System;
using System.Linq;

namespace KeepCorrect.Translator.WPF.Extensions;

public static class StringExtensions
{
    public static bool IsText(this string text)
    {
        if (!text.IsValid(out var trimedText)) return false;
        return HasAnySpace(trimedText) && ContainsOnlyAscii(trimedText);
    }
    
    public static string GetText(this string text)
    {
        return text.Trim();
    }

    public static bool IsTextOrWord(this string text)
    {
        return IsText(text) || IsWord(text);
    }

    private static bool IsValid(this string text, out string trimedText)
    {
        trimedText = text.Trim();
        return trimedText.Length > 0;
    }

    private static bool ContainsOnlyAscii(string trimedText)
    {
        return trimedText.All(char.IsAscii);
    }

    private static bool HasAnySpace(string trimedText)
    {
        return trimedText.Any(ch => ch == ' ');
    }

    public static bool IsWord(this string text)
    {
        if (!text.IsValid(out var trimedText)) return false;
        return ContainsOnlyAscii(trimedText) && HasNoSpace(trimedText);
    }

    private static bool HasNoSpace(string text)
    {
        return text.All(ch => ch != ' ');
    }
}