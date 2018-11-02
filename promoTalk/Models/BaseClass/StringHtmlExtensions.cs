﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace promoTalk.Models
{
    public static class StringHtmlExtensions
    { 
     public static string TruncateHtml(this string html, int maxCharacters, string trailingText)
    {
        if (string.IsNullOrEmpty(html))
            return html;
        // find the spot to truncate
        // count the text characters and ignore tags
        var textCount = 0;
        var charCount = 0;
        var ignore = false;
        foreach (char c in html)
        {
            charCount++;
            if (c == '<')
                ignore = true;
            else if (!ignore)
                textCount++;
            if (c == '>')
                ignore = false;
            // stop once we hit the limit
            if (textCount >= maxCharacters)
                break;
        }
        // Truncate the html and keep whole words only
        var trunc = new StringBuilder(html.TruncateWords(charCount));
        // keep track of open tags and close any tags left open
        var tags = new Stack();
        var matches = Regex.Matches(trunc.ToString(),@"<((?[^\s/>]+)|/(?[^\s>]+)).*?(?/)?\s*>",
            RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
        foreach (Match match in matches)
        {
            if (match.Success)
            {
                var tag = match.Groups["tag"].Value;
                var closeTag = match.Groups["closeTag"].Value;
                // push to stack if open tag and ignore it if it is self-closing, i.e. 

                if (!string.IsNullOrEmpty(tag) && string.IsNullOrEmpty(match.Groups["selfClose"].Value))
                    tags.Push(tag);
                // pop from stack if close tag
                else if (!string.IsNullOrEmpty(closeTag))
                {
                    // pop the tag to close it.. find the matching opening tag
                    // ignore any unclosed tags
                    while (tags.Pop() != closeTag && tags.Count > 0)
                    { }
                }
            }
        }
        if (html.Length > charCount)
            // add the trailing text
            trunc.Append(trailingText);
        // pop the rest off the stack to close remainder of tags
        while (tags.Count > 0)
        {
            trunc.Append("trunc.Append(tags.Pop())");

            trunc.Append('>');
        }
        return trunc.ToString();
    }
    ///
    /// Truncates a string containing HTML to a number of text characters, keeping whole words.
    /// The result contains HTML and any tags left open are closed.
   
    public static string TruncateHtml(this string html, int maxCharacters)
    {
        return html.TruncateHtml(maxCharacters, null);
    }
    ///
    /// Strips all HTML tags from a string
    
    public static string StripHtml(this string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;
        return Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
    }
    ///
    /// Truncates text to a number of characters
   
    public static string Truncate(this string text, int maxCharacters)
    {
        return text.Truncate(maxCharacters, null);
    }
 
    public static string Truncate(this string text, int maxCharacters, string trailingText)
    {
        if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
            return text;
        else
            return text.Substring(0, maxCharacters) + trailingText;
    }
 
    public static string TruncateWords(this string text, int maxCharacters)
    {
        return text.TruncateWords(maxCharacters, null);
    }
 
    public static string TruncateWords(this string text, int maxCharacters, string trailingText)
    {
        if (string.IsNullOrEmpty(text) || maxCharacters <= 0 || text.Length <= maxCharacters)
            return text;
        // trunctate the text, then remove the partial word at the end
        return Regex.Replace(text.Truncate(maxCharacters),
            @"\s+[^\s]+$", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Compiled) + trailingText;
    }
}
}