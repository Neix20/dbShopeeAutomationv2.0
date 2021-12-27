using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class mLocality
    {
        public mLocality(string value)
        {
            this.value = value;
        }

        public mLocality()
        {

        }

        public string value { get; set; }
    }
}