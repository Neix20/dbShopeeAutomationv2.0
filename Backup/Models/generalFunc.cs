using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public static class generalFunc
    {
        public static string trimStr(string str)
        {
            return str.Trim('"');
        }

        public static string Random10DigitCode(int length = 10)
        {
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < length; i++) str += $"{rand.Next(0, 10) + 1}";
            return str;
        }

        public static string GenEmail()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < rand.Next(1, 10); i++) str += $"{alphabet.ElementAt(rand.Next(0, 10) + 1)}";
            str += "@mail.com";
            return str;
        }

        public static string GenPhoneNum()
        {
            Random rand = new Random();
            string str = "016-";
            for (int i = 0; i < 3; i++) str += $"{rand.Next(0, 10) + 1}";
            str += " ";
            for (int i = 0; i < 4; i++) str += $"{rand.Next(0, 10) + 1}";
            return str;
        }

        public static string GenSKU(string str, string str2, string str3)
        {
            str = (str.Length < 3) ? "str1" : str;
            str2 = (str2.Length < 3) ? "str2" : str2;
            str3 = (str3.Length < 3) ? "str3" : str3;
            return $"{str.Substring(0, 3)}/{str2.Substring(0, 3)}_{str3.Substring(0, 3)}";
        }
    }
}