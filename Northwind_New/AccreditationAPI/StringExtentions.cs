using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeManagementAPI
{
    public static class StringExtentions
    {
        static string[] arabicNumbers = { "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩", "٠" },
                 persianNumbers = { "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹", "۰" },
                 englishNumbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

        public static string ToPersianString(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string[] arabicChars = { "ي", "ك", "‍", "دِ", "بِ", "زِ", "ذِ", "ِشِ", "ِسِ", "ى", "ة" },
                     persianChars = { "ی", "ک", "", "د", "ب", "ز", "ذ", "ش", "س", "ی", "ه" };

            for (var i = 0; i < arabicChars.Length; i++)
                input = input.Replace(arabicChars[i], persianChars[i]);

            return input.Trim();
        }

        public static string PersianToEnglishNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            for (var i = 0; i < englishNumbers.Length; i++)
                input = input.Replace(persianNumbers[i], englishNumbers[i]);

            return input.Trim();
        }

        public static string ArabicToPersianNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            for (var i = 0; i < arabicNumbers.Length; i++)
                input = input.Replace(arabicNumbers[i], persianNumbers[i]);

            return input.Trim();
        }

        public static string EnglishToPersianNumber(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            for (var i = 0; i < englishNumbers.Length; i++)
                input = input.Replace(englishNumbers[i], persianNumbers[i]);

            return input.Trim();
        }

        public static string NormalizedInput(this string input)
        {
            return !string.IsNullOrEmpty(input) ? input.Trim().ToLower() : string.Empty;
        }
    }
}
