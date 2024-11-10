using System;
using System.Text.RegularExpressions;

namespace DialogW
{
	public static class SentenceSplitter
	{
		private const string PATTERN =
			@"(?<!\b(?:Mr|Mrs|Ms|Dr|St|Jr|Sr|vs|etc|i\.e|e\.g))   # Avoid split after common abbreviations
			  (?<!\b[A-Z]\.)                                      # Avoid split after initials (e.g., 'J. K. Rowling')
			  (?<=\.|\?|!)                                        # Ensure the sentence ends with '.', '?' or '!'
			  (?=\s+[A-Z])                                        # Followed by whitespace and a capital letter";

		private readonly static Regex regex;

		static SentenceSplitter()
		{
			// Initialize the regex with IgnorePatternWhitespace to support comments and spacing in the pattern
			regex = new Regex(PATTERN, RegexOptions.IgnorePatternWhitespace);
		}

		public static string[] SplitIntoSentences(string paragraph)
		{
			return regex
				.Split(paragraph)
				.Select(sentence => sentence.Trim())
				.ToArray();
		}
	}
}