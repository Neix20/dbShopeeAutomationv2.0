using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class mCustomer
    {
        public mCustomer(int customer_id, string name)
        {
            this.customer_id = customer_id;
            this.name = name;
        }

        public mCustomer()
        {

        }

        public int customer_id { get; set; }
        public string name { get; set; }
    }
}