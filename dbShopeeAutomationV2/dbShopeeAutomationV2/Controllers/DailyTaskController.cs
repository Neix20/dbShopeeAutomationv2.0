using dbShopeeAutomationV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class DailyTaskController : AdminController
    {

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public int numOfOrdersLeft(IEnumerable<TShopeeInvoice> invoiceList)
        {
            int inv_sta_id = dbStatusFunction.invoiceStatusID("Incomplete");
            int p_inv_sta_id = dbStatusFunction.invoiceStatusID("Packaging");

            return invoiceList.Where(it => it.invoice_status_id == inv_sta_id || it.invoice_status_id == p_inv_sta_id).ToList().Count;
        }

        // GET: DailyTask
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DailyTaskPartial()
        {
            int inv_sta_id = dbStatusFunction.invoiceStatusID("Incomplete");
            int p_inv_sta_id = dbStatusFunction.invoiceStatusID("Packaging");

            var model = db.TShopeeInvoices.AsEnumerable().Where(
                it =>
                {
                    if (it.invoice_completed_date != null)
                    {
                        DateTime tmp = (DateTime)it.invoice_completed_date;
                        return tmp.Date == DateTime.Now.Date;
                    }
                    return it.invoice_status_id == inv_sta_id || it.invoice_status_id == p_inv_sta_id;
                }
            );

            ViewData["num_of_orders_left"] = numOfOrdersLeft(model);

            return PartialView("_DailyTaskPartial", model);
        }

        [HttpPost]
        public ActionResult Show(TShopeeInvoice item)
        {
            IEnumerable<TShopeeInvoice> model;

            if (((DateTime)item.invoice_completed_date).Date == DateTime.Now.Date)
            {
                int inv_sta_id = dbStatusFunction.invoiceStatusID("Incomplete");
                int p_inv_sta_id = dbStatusFunction.invoiceStatusID("Packaging");

                model = db.TShopeeInvoices.AsEnumerable().Where(
                    it =>
                    {
                        if (it.invoice_completed_date != null)
                        {
                            DateTime tmp = (DateTime)it.invoice_completed_date;
                            return tmp.Date == DateTime.Now.Date;
                        }
                        return it.invoice_status_id == inv_sta_id || it.invoice_status_id == p_inv_sta_id;
                    }
                );

                ViewData["num_of_orders_left"] = numOfOrdersLeft(model);

                return PartialView("_DailyTaskPartial", model);
            }

            model = db.TShopeeInvoices.AsEnumerable().Where(
                it =>
                {
                    if (it.invoice_completed_date != null)
                    {
                        DateTime tmp = (DateTime)it.invoice_completed_date;
                        return tmp.Date == ((DateTime)item.invoice_completed_date).Date;
                    }
                    return false;

                }
            );

            ViewData["num_of_orders_left"] = numOfOrdersLeft(model);

            return PartialView("_DailyTaskPartial", model);
        }

        [HttpPost]
        public ActionResult GenerateSummary()
        {
            string[] invoice_id_str_arr = Request.Form.GetValues("invoice_id");

            if(invoice_id_str_arr != null)
            {
                int[] invoice_id_arr = invoice_id_str_arr.Select(x => int.Parse(x)).ToArray();

                // Create Empty Product Summary List
                Dictionary<int, ProductSummary> productSummaryDict = new Dictionary<int, ProductSummary>();

                // Packaging Invoice Status ID
                int p_inv_sta_id = dbStatusFunction.invoiceStatusID("Packaging");

                string username = User.Identity.Name;

                foreach (var invoice_id in invoice_id_arr)
                {
                    // 0. Update Invoice Status to Packaging
                    var invoice = db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == invoice_id);

                    // 1. Get Order ID
                    int order_id = (int)invoice.order_id;

                    // 2. Get List of Order Item
                    List<TShopeeOrderItem> orderItemList = db.TShopeeOrderItems.Where(it => it.order_id == order_id).ToList();

                    // 3. Loop Through List of Order Item
                    foreach (var order_item in orderItemList)
                    {
                        int order_item_id = order_item.order_item_id;

                        // 4. Get Product
                        var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == order_item.product_id);

                        int product_id = product.product_id;
                        int quantity = (int)order_item.quantity;

                        // 5. Check if Product exist in product Summary Dict
                        if (productSummaryDict.ContainsKey(product_id))
                        {
                            productSummaryDict[product_id].addQuantity(quantity);
                        }
                        else
                        {
                            // 6. Create New Product Summary Class
                            string product_name = product.name;
                            string product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product.product_brand_id).name;
                            string product_type = db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product.product_type_id).name;
                            string product_variety = db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == product.product_variety_id).name;

                            var productSummary = new ProductSummary(product_id, product_name, product_brand, product_type, product_variety, quantity);

                            productSummaryDict.Add(product_id, productSummary);
                        }
                    }

                    invoice.invoice_status_id = p_inv_sta_id;

                    dbStoredProcedure.invoiceUpdate(invoice.invoice_id, invoice.invoice_title, invoice.invoice_created_date, invoice.invoice_completed_date, invoice.invoice_details, invoice.shipping_fee, invoice.invoice_status_id, invoice.payment_method_id, invoice.order_id, invoice.customer_id, username);
                    db.SaveChanges();
                }

                // 7. Create List of productSummary
                IList<ProductSummary> model = new List<ProductSummary>();

                // 8. Loop Throught Dictionary to Populate List of ProductSummary
                foreach (var kvp in productSummaryDict)
                {
                    model.Add(kvp.Value);
                }

                return PartialView("_SummaryListingPartial", model);
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult PrintBill(int invoice_id)
        {
            // Stock Warehouse
            var stockWarehouse = db.TShopeeStockWarehouses.ToList().ElementAt(0);

            // Invoice
            var invoice = db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == invoice_id);

            // Customer
            var customer = db.TShopeeCustomers.FirstOrDefault(it => it.customer_id == invoice.customer_id);

            // Order
            var order = db.TShopeeOrders.FirstOrDefault(it => it.order_id == invoice.order_id);

            // Create Empty Product Summary List
            Dictionary<int, ProductSummary> productSummaryDict = new Dictionary<int, ProductSummary>();

            // 1. Get Order ID
            int order_id = (int)invoice.order_id;

            // 2. Get List of Order Item
            List<TShopeeOrderItem> orderItemList = db.TShopeeOrderItems.Where(it => it.order_id == order_id).ToList();

            // 3. Loop Through List of Order Item
            foreach (var order_item in orderItemList)
            {
                int order_item_id = order_item.order_item_id;

                // 4. Get Product
                var product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == order_item.product_id);

                int product_id = product.product_id;
                int quantity = (int)order_item.quantity;

                // 5. Check if Product exist in product Summary Dict
                if (productSummaryDict.ContainsKey(product_id))
                {
                    productSummaryDict[product_id].addQuantity(quantity);
                }
                else
                {
                    // 6. Create New Product Summary Class
                    string product_name = product.name;
                    string product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product.product_brand_id).name;
                    string product_type = db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product.product_type_id).name;
                    string product_variety = db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == product.product_variety_id).name;

                    var productSummary = new ProductSummary(product_id, product_name, product_brand, product_type, product_variety, product.sell_price, quantity);

                    productSummary.calculateSubTotal();

                    productSummaryDict.Add(product_id, productSummary);
                }
            }


            // 7. Create List of productSummary
            IList<ProductSummary> productSummaryList = new List<ProductSummary>();

            // 8. Loop Throught Dictionary to Populate List of ProductSummary
            foreach (var kvp in productSummaryDict)
            {
                productSummaryList.Add(kvp.Value);
            }

            // Total Price
            Decimal total_price = (Decimal) (order.total_price - invoice.shipping_fee);

            PackageListingSummary model = new PackageListingSummary(stockWarehouse, customer, invoice, productSummaryList, order.total_price, total_price);

            return PartialView("_PackageListingPartial", model);
        }


    }
}