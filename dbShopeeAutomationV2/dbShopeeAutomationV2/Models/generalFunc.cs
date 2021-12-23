﻿using System;
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

        public static string removeWhitespace(string str)
        {
            return str.Replace(" ", string.Empty);
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

        public static int getMonthInt(string str)
        {
            List<String> monthList = new List<String>()
            {
                "Jan", "Feb", "Mar", "Apr",
                "May", "Jun", "Jul", "Aug",
                "Sep", "Oct", "Nov", "Dec"
            };

            return (str.Length < 3) ? -1 : monthList.IndexOf(str.Substring(0, 3));
        }

        public static string genDateString(int year, int month, int day)
        {
            string y = (year < 1000) ? $"20{year}" : $"{year}";
            string m = (month < 10) ? $"0{month}" : $"{month}";
            string d = (day < 10) ? $"0{day}" : $"{day}";
            return $"{y}-{m}-{d}";
        }

        public static string parseReceivedDate(string received_date)
        {
            string[] r_dt_arr = received_date.Split(' ');
            int year = int.Parse(r_dt_arr[0]);
            int month = getMonthInt(r_dt_arr[1]);
            int day = int.Parse(r_dt_arr[2]);
            return genDateString(year, month, day);
        }

        public static Decimal[] parseRollSize(string roll_size)
        {
            Decimal[] arr = new Decimal[3];

            // arr[0] => Height, arr[1] => Width, arr[2] => Length
            arr[0] = 0;

            string[] tmp_arr = roll_size.Split(new[] { " x " }, StringSplitOptions.None);

            // Remove Last Character 'm'
            tmp_arr[0] = tmp_arr[0].Substring(0, tmp_arr[0].Length - 1);
            tmp_arr[1] = tmp_arr[1].Substring(0, tmp_arr[1].Length - 1);

            arr[1] = Decimal.Parse(tmp_arr[0]);
            arr[2] = Decimal.Parse(tmp_arr[1]);

            return arr;
        }

        public static string FormatNum(int num, int zLen)
        {
            string num_str = $"{num}";
            return new string('0', zLen - num_str.Length) + num_str;
        }


        public static string GenProductionCode(int last_production_id)
        {
            int zLen = 5;
            return $"JS{FormatNum(last_production_id, zLen)}";
        }
    }
}