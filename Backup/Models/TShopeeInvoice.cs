//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace dbShopeeAutomationV2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TShopeeInvoice
    {
        public int invoice_id { get; set; }
        public string invoice_title { get; set; }
        public Nullable<System.DateTime> invoice_created_date { get; set; }
        public Nullable<System.DateTime> invoice_completed_date { get; set; }
        public string invoice_details { get; set; }
        public Nullable<decimal> shipping_fee { get; set; }
        public Nullable<int> invoice_status_id { get; set; }
        public Nullable<int> payment_method_id { get; set; }
        public Nullable<int> order_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> detail_id { get; set; }
    }
}
