using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class mDestAddress
    {
        public mDestAddress(string address_line_1, string address_line_2, string city, int? zip_code, string state, string country)
        {
            this.address_line_1 = address_line_1;
            this.address_line_2 = address_line_2;
            this.city = city;
            this.zip_code = zip_code;
            this.state = state;
            this.country = country;
        }

        public mDestAddress()
        {

        }

        public mDestAddress(String[] address_arr)
        {
            this.address_line_1 = address_arr[0];
            this.address_line_2 = address_arr[1];
            this.city = address_arr[2];
            this.zip_code = int.Parse(address_arr[3]);
            this.state = address_arr[4];
            this.country = address_arr[5];
        }

        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string city { get; set; }
        public Nullable<int> zip_code { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}