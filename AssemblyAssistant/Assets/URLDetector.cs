using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class URLDetector : MonoBehaviour
{
    // Define a regex pattern for matching URLs
    private static readonly Regex urlRegex = new Regex(
        @"(http|https|ftp|ftps)://[^\s/$.?#].[^\s]*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex boldRegex = new Regex(
        @"\*\*(.*?)\*\*",
        RegexOptions.Compiled);

    /// <summary>
    /// This method formats the input TextMeshProUGUI component's text to include rich text formatting for URLs and bold text.
    /// IMPORTANT: The TexMeshProUGUI component must have a TextMeshProLinkClickDetector component attached to it.
    /// </summary>
    public static string FormatTextMeshProForRichTextFormat(string input)
    {
        if (input == null)
            return null;

        // Replace URLs with links
        input = urlRegex.Replace(input, match => {
            string url = match.Value.TrimEnd(')', '.', ',', '!', '?', ':', ';');
            return $"<link=\"{url}\"><color=#00A2E8>{url}</color></link>";
        });

        // Replace **text** with <b>text</b>
        input = boldRegex.Replace(input, match => {
            string content = match.Groups[1].Value;
            return $"<b>{content}</b>";
        });

        return input;
    }

    public static bool ContainsUrl(string input, out string url)
    {
        // Try to find a match in the input string
        Match match = urlRegex.Match(input);

        if (match.Success)
        {
            // If a match is found, set the output parameter and return true
            url = match.Value.TrimEnd(')', '.', ',', '!', '?', ':', ';');
            return true;
        }
        else
        {
            // If no match is found, set the output parameter to null and return false
            url = null;
            return false;
        }
    }
}
