using System;

namespace Rhyous.Odata.Filter.Tests.Helpers
{
    /// <summary>
    /// Normalizes whitespace characters in strings to handle culture-specific formatting differences.
    /// </summary>
    public static class StringSpaceNormalizer
    {
        /// <summary>
        /// Default characters to replace with regular space:
        /// - Non-breaking space (U+00A0)
        /// - Narrow No-Break Space (U+202F)
        /// </summary>
        private static readonly char[] DefaultSpaceCharacters = new[]
        {
            '\u00A0', // Non-breaking space
            '\u202F'  // Narrow No-Break Space
        };

        /// <summary>
        /// Normalizes whitespace characters in the input string by replacing specified characters with regular space (U+0020).
        /// Uses default space characters if none are specified.
        /// </summary>
        /// <param name="input">The string to normalize.</param>
        /// <param name="charactersToReplace">Optional characters to replace with regular space. If not provided, uses default space characters.</param>
        /// <returns>The normalized string with specified characters replaced by regular space.</returns>
        public static string Normalize(string input, params char[] charactersToReplace)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var charsToReplace = charactersToReplace != null && charactersToReplace.Length > 0
                ? charactersToReplace
                : DefaultSpaceCharacters;

            var result = input;
            foreach (var charToReplace in charsToReplace)
            {
                result = result.Replace(charToReplace, ' ');
            }

            return result;
        }
    }
}

