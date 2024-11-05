using System.Text.RegularExpressions;
using TextDataProcessing.Models;

namespace TextDataProcessing.Helpers
{
    public static class ExpenseParser
    {
        public static ExpenseData ParseExpenseData(string inputText)
        {
            if (!AreTagsBalanced(inputText))
            {
                // Reject the message if tags are unbalanced
                return null;
            }
            var expenseData = new ExpenseData();
            try
            {
                // Match and extract values using regular expressions
                var costCentreMatch = Regex.Match(inputText, @"<cost_centre>(.*?)</cost_centre>", RegexOptions.IgnoreCase);
                var totalMatch = Regex.Match(inputText, @"<total>(.*?)</total>", RegexOptions.IgnoreCase);
                var paymentMethodMatch = Regex.Match(inputText, @"<payment_method>(.*?)</payment_method>", RegexOptions.IgnoreCase);
                var vendorMatch = Regex.Match(inputText, @"<vendor>(.*?)</vendor>", RegexOptions.IgnoreCase);
                var descriptionMatch = Regex.Match(inputText, @"<description>(.*?)</description>", RegexOptions.IgnoreCase);
                var dateMatch = Regex.Match(inputText, @"<date>(.*?)</date>", RegexOptions.IgnoreCase);

                // Cost Centre (defaults to "UNKNOWN" if not found)
                expenseData.CostCentre = costCentreMatch.Success ? costCentreMatch.Groups[1].Value : "UNKNOWN";

                // Total (required field)
                if (!totalMatch.Success || !decimal.TryParse(totalMatch.Groups[1].Value, out decimal total))
                {
                    return null; // Fail if total is missing or invalid
                }
                expenseData.Total = total;

                // Payment Method
                expenseData.PaymentMethod = paymentMethodMatch.Success ? paymentMethodMatch.Groups[1].Value : null;

                // Vendor
                expenseData.Vendor = vendorMatch.Success ? vendorMatch.Groups[1].Value : null;

                // Description
                expenseData.Description = descriptionMatch.Success ? descriptionMatch.Groups[1].Value : null;

                // Date (optional, default to current date if not found)
                expenseData.Date = dateMatch.Success ? DateTime.Parse(dateMatch.Groups[1].Value) : DateTime.Now;

                // Calculate tax and total excluding tax
                const decimal taxRate = 0.10M; // Assume 10% tax rate
                expenseData.SalesTax = expenseData.Total * taxRate;
                expenseData.TotalExcludingTax = expenseData.Total - expenseData.SalesTax;
            }
            catch (Exception)
            {
                return null; // Return null if parsing fails
            }

            return expenseData;
        }
        /*private static bool AreTagsBalanced(string input)
        {
            // Check if there are unbalanced opening and closing tags
            var regex = new Regex(@"<(?<tag>[^/<>]+)>");
            var matches = regex.Matches(input);
            var openTags = new Stack<string>();

            foreach (Match match in matches)
            {
                var tag = match.Groups["tag"].Value;

                // Check for closing tags
                if (input.Contains($"</{tag}>"))
                {
                    openTags.Push(tag);
                    input = input.Replace($"</{tag}>", ""); // Remove the processed closing tag to avoid duplicate checks
                }
                else if (tag.StartsWith("/"))
                {
                    if (openTags.Count == 0 || openTags.Pop() != tag.Substring(1))
                    {
                        return false; // Mismatched or unbalanced tag found
                    }
                }
                else
                {
                    openTags.Push(tag);
                }
            }

            return openTags.Count == 0; // Ensure all opened tags are closed
        }
        */

        private static bool AreTagsBalanced(string inputText)
        {
            var tagStack = new Stack<string>();
            var regex = new Regex(@"<(/?)(\w+)>");
            var matches = regex.Matches(inputText);

            foreach (Match match in matches)
            {
                var isClosingTag = match.Groups[1].Value == "/";
                var tagName = match.Groups[2].Value;

                if (isClosingTag)
                {
                    // If it's a closing tag, ensure it matches the last opened tag
                    if (tagStack.Count == 0 || tagStack.Pop() != tagName)
                    {
                        return false; // Unmatched closing tag found
                    }
                }
                else
                {
                    // It's an opening tag, push it onto the stack
                    tagStack.Push(tagName);
                }
            }

            // Check if any required tag is missing
            if (!Regex.IsMatch(inputText, @"<total>.*?</total>", RegexOptions.IgnoreCase))
            {
                return false; // Missing <total> tag
            }

            // Return true if the stack is empty (all tags are balanced)
            return tagStack.Count == 0;
        }

    }


}
