using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public static class dbStatusFunction
    {
        static dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public static int invoiceStatusID(string name)
        {
            var invoiceStatus = db.TShopeeInvoiceStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (invoiceStatus == null) ? -1 : invoiceStatus.invoice_status_id;
        }
    }
}