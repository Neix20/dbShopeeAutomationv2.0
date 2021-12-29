using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class ProductSummary
    {
        public ProductSummary()
        {
            this.sub_total = 0;
        }

        public ProductSummary(int product_id, string name, string product_brand, string product_category, string product_model, string product_type, decimal? product_price, int quantity)
        {
            this.product_id = product_id;
            this.name = name;
            this.product_brand = product_brand;
            this.product_category = product_category;
            this.product_model = product_model;
            this.product_type = product_type;
            this.product_price = product_price;
            this.quantity = quantity;
        }

        public ProductSummary(int product_id, string name, string product_brand, string product_category, string product_model, string product_type, int quantity)
        {
            this.product_id = product_id;
            this.name = name;
            this.product_brand = product_brand;
            this.product_category = product_category;
            this.product_model = product_model;
            this.product_type = product_type;
            this.quantity = quantity;
        }

        public void addQuantity(int num)
        {
            this.quantity += num;
        }

        public void calculateSubTotal()
        {
            this.sub_total = this.quantity * this.product_price;
        }

        public int product_id { get; set; }
        public string name { get; set; }
        public string product_brand { get; set; }
        public string product_category { get; set; }
        public string product_model { get; set; }
        public string product_type { get; set; }
        public Decimal? product_price { get; set; }
        public int quantity { get; set; }
        public Decimal? sub_total { get; set; }


    }
}