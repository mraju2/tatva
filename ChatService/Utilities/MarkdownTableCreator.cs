using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatService.Utilities
{
    public class MarkdownTableCreator
    {
        public string CreateMarkdownTable(List<Dictionary<string, object>> results)
        {
            if (!results.Any()) return "No results found.";

            var headers = results[0].Keys.ToList();
            var sb = new StringBuilder();

            // Calculate maximum width for each column including header
            var columnWidths = new Dictionary<string, int>();
            foreach (var header in headers)
            {
                var maxWidth = header.Length;
                foreach (var row in results)
                {
                    var value = row[header]?.ToString() ?? "NULL";
                    maxWidth = Math.Max(maxWidth, value.Length);
                }
                columnWidths[header] = maxWidth;
            }

            // Determine column alignment based on data type
            var columnAlignments = new Dictionary<string, string>();
            foreach (var header in headers)
            {
                var firstNonNullValue = results
                    .Select(r => r[header])
                    .FirstOrDefault(v => v != null);

                // Default left alignment
                var alignment = ":-";  // left align

                if (firstNonNullValue != null)
                {
                    if (firstNonNullValue is int or long or double or decimal or float)
                    {
                        alignment = "-:";  // right align for numbers
                    }
                    else if (firstNonNullValue is DateTime)
                    {
                        alignment = ":-:"; // center align for dates
                    }
                }

                columnAlignments[header] = alignment;
            }

            // Create header row
            sb.Append('|');
            foreach (var header in headers)
            {
                var paddedHeader = header.PadRight(columnWidths[header]);
                sb.Append($" {paddedHeader} |");
            }
            sb.AppendLine();

            // Create separator row with alignment
            sb.Append('|');
            foreach (var header in headers)
            {
                var width = columnWidths[header];
                var alignment = columnAlignments[header];

                // Create separator based on alignment and width
                var separator = new string('-', width);
                switch (alignment)
                {
                    case ":-":  // left align
                        sb.Append($" :{separator} |");
                        break;
                    case "-:":  // right align
                        sb.Append($" {separator}: |");
                        break;
                    case ":-:": // center align
                        sb.Append($" :{separator}: |");
                        break;
                }
            }
            sb.AppendLine();

            // Create data rows
            foreach (var row in results)
            {
                sb.Append('|');
                foreach (var header in headers)
                {
                    var value = FormatValue(row[header]);
                    var width = columnWidths[header];
                    var alignment = columnAlignments[header];

                    // Pad based on alignment
                    string paddedValue = alignment switch
                    {
                        "-:" => value.PadLeft(width),    // right align
                        ":-:" => value.PadCenter(width), // center align
                        _ => value.PadRight(width)       // left align (default)
                    };

                    sb.Append($" {paddedValue} |");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private string FormatValue(object value)
        {
            if (value == null) return "NULL";

            return value switch
            {
                DateTime dateTime => dateTime.ToString("yyyy-MM-dd"),
                decimal decimalValue => decimalValue.ToString("N2"),
                double doubleValue => doubleValue.ToString("N2"),
                float floatValue => floatValue.ToString("N2"),
                bool boolValue => boolValue ? "Yes" : "No",
                _ => value.ToString()
            };
        }
    }

    // Extension method for center alignment
    public static class StringExtensions
    {
        public static string PadCenter(this string value, int width)
        {
            if (string.IsNullOrEmpty(value)) return new string(' ', width);
            int spaces = width - value.Length;
            int padLeft = spaces / 2 + value.Length;
            return value.PadLeft(padLeft).PadRight(width);
        }
    }

}
