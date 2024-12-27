using System.Text.RegularExpressions;
namespace Acrux.Dialoguing;

public static class DialogueSplitter
{
    private const string PATTERN =
        @"(?<!\b(?:Mr|Mrs|Ms|Dr|St|Jr|Sr|vs|etc|i\.e|e\.g))   # Avoid split after common abbreviations
			  (?<!\b[A-Z]\.)                                      # Avoid split after initials (e.g., 'J. K. Rowling')
			  (?<=\.{3}|\.|\?|!)                                  # Ensure the sentence ends with '...', '.', '?' or '!'
			  (?=\s+[a-zA-Z])                                     # Followed by whitespace and a letter";

    private readonly static Regex regex;

    static DialogueSplitter()
    {
        // Initialize the regex with IgnorePatternWhitespace to support comments and spacing in the pattern
        regex = new Regex(PATTERN, RegexOptions.IgnorePatternWhitespace);
    }

    public static string[] SplitIntoSentences(string dialogue)
    {
        return regex
            .Split(dialogue)
            .Select(sentence => sentence.Trim())
            .ToArray();
    }
}