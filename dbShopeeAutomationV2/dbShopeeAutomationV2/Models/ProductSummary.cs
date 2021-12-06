using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class ProductSummary
    {
        public ProductSummary(int product_id, string name, string product_brand, string product_type, string product_variety, int quantity)
        {
            this.product_id = product_id;
            this.name = name;
            this.product_brand = product_brand;
            this.product_type = product_type;
            this.product_variety = product_variety;
            this.quantity = quantity;
        }

        public ProductSummary()
        {

        }

        public void addQuantity(int num)
        {
            quantity += num;
        }

        public int product_id { get; set; }
        public string name { get; set; }
        public string product_brand { get; set; }
        public string product_type { get; set; }
        public string product_variety { get; set; }
        public int quantity { get; set; }


    }
}