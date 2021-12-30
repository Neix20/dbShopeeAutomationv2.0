using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public class PackageListingSummary
    {

        public PackageListingSummary()
        {

        }

        public PackageListingSummary(TShopeeStockWarehouse stockWarehouse, TShopeeCustomer customer, TShopeeInvoice invoice, mDestAddress destAddress, IList<ProductSummary> productSummaryList, decimal? sub_total, decimal? total_price)
        {
            this.stockWarehouse = stockWarehouse;
            this.customer = customer;
            this.invoice = invoice;
            this.destAddress = destAddress;
            this.productSummaryList = productSummaryList;
            this.sub_total = sub_total;
            this.total_price = total_price;
        }

        // Stock Warehouse
        public TShopeeStockWarehouse stockWarehouse { get; set; }

        // Customer
        public TShopeeCustomer customer { get; set; }

        // Invoice
        public TShopeeInvoice invoice { get; set; }

        // Destination Address
        public mDestAddress destAddress { get; set; }

        // List of Product Summary
        public IList<ProductSummary> productSummaryList { get; set; }

        // Sub Total Price
        public Decimal? sub_total { get; set; }

        // Total Price
        public Decimal? total_price { get; set; }


    }
}