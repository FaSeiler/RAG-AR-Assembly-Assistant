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

    public static string ReplaceUrlsWithLinks(TextMeshProUGUI input)
    {
        return urlRegex.Replace(input.text, match => {
            string url = match.Value.TrimEnd(')', '.', ',', '!', '?', ':', ';');
            input.gameObject.AddComponent<TextMeshProLinkClickDetector>();
            return $"<link=\"{url}\"><color=#00A2E8>{url}</color></link>";
        });
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
