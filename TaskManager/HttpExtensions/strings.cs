﻿using System.Security.Claims;

namespace TaskManager.HttpExtensions
{
    public static class strings
    {
        public static bool IsLetter(this char c)
        {
            return (c <= 'z' && c >= 'a') || (c <= 'Z' && c >= 'A');
        }
        public static bool IsUpper(this char c)
        {
            return (c <= 'Z' && c >= 'A');
        }

        public static bool IsLower(this char c)
        {
            return (c <= 'z' && c >= 'a');
        }

        public static bool IsDigit(this char c)
        {
            return (c <= '9' && c >= '0');
        }

        public static bool IsSpecial(this char c)
        {
            string SpecialCharacters = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            return (SpecialCharacters.Contains(c));
        }

        //public static string? GetByName(this IEnumerable<Claim> input, string name)
        //{
        //    return input.Where(c => c.Type == name).Select(c => c.Value).FirstOrDefault();
        //}
    }
}
