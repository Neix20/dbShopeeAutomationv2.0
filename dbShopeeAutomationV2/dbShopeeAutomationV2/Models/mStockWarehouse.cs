using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class mStockWarehouse
    {
        public mStockWarehouse(string name, string address)
        {
            this.name = name;
            this.address = address;
        }

        public mStockWarehouse()
        {

        }

        public string name { get; set; }
        public string address { get; set; }
    }
}