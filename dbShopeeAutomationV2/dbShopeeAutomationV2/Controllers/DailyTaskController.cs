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

            return invoiceList
                .Where(it => it.invoice_status_id == inv_sta_id || it.invoice_status_id == p_inv_sta_id)
                .ToList().Count;
        }

        // GET: DailyTask
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DailyTaskPartial()
        {
            // Return only Incomplete, Packaging and Orders with Completed Dates on Today
            var model = db.TShopeeInvoices.AsEnumerable().Where(it =>
            {
                if (it.invoice_completed_date == null)
                {
                    return true;
                }

                DateTime tmp = (DateTime)it.invoice_completed_date;
                return tmp.Date == DateTime.Now.Date;
            });

            ViewData["num_of_orders_left"] = numOfOrdersLeft(model);
            return PartialView("_DailyTaskPartial", model);
        }

        [HttpPost]
        public ActionResult Show(TShopeeInvoice item)
        {
            IEnumerable<TShopeeInvoice> model;

            DateTime inv_com_dt = (DateTime)item.invoice_completed_date;

            if (inv_com_dt.Date == DateTime.Now.Date)
            {
                // Return only Incomplete, Packaging and Orders with Completed Dates on Today
                model = db.TShopeeInvoices.AsEnumerable().Where(it =>
                {
                    if (it.invoice_completed_date == null)
                    {
                        return true;
                    }

                    DateTime tmp = (DateTime)it.invoice_completed_date;
                    return tmp.Date == DateTime.Now.Date;
                });

                ViewData["num_of_orders_left"] = numOfOrdersLeft(model);
                return PartialView("_DailyTaskPartial", model);
            }

            // Return only Specific Date that were completed on that day
            model = db.TShopeeInvoices.AsEnumerable().Where(it =>
            {
                if (it.invoice_completed_date == null)
                {
                    return false;
                }

                DateTime tmp = (DateTime)it.invoice_completed_date;
                return tmp.Date == inv_com_dt.Date.Date;
            });

            ViewData["num_of_orders_left"] = numOfOrdersLeft(model);

            return PartialView("_DailyTaskPartial", model);
        }

        [HttpPost]
        public ActionResult GenerateSummary()
        {
            string[] invoice_id_str_arr = Request.Form.GetValues("invoice_id");

            if (invoice_id_str_arr != null)
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
                    int order_id = (int) invoice.order_id;

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

                            // Product Brand
                            var product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product.product_brand_id);
                            string product_brand_str = product_brand.code;

                            // Product Model
                            var product_model = db.TShopeeProductModels.FirstOrDefault(it => it.product_model_id == product.product_model_id);
                            string product_model_str = product_model.code;

                            // Product Type
                            var product_type = db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product.product_type_id);
                            string product_type_str = product_type.code;

                            // Product Category
                            var product_category = db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == product.product_category_id);
                            string product_category_str = product_category.code;

                            var productSummary = new ProductSummary(
                                product_id, product_name,
                                product_brand_str, product_category_str, 
                                product_model_str, product_type_str, 
                                quantity);

                            productSummaryDict.Add(product_id, productSummary);
                        }
                    }

                    invoice.invoice_status_id = p_inv_sta_id;
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

                    // Product Brand
                    var product_brand = db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product.product_brand_id);
                    string product_brand_str = product_brand.code;

                    // Product Model
                    var product_model = db.TShopeeProductModels.FirstOrDefault(it => it.product_model_id == product.product_model_id);
                    string product_model_str = product_model.code;

                    // Product Type
                    var product_type = db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product.product_type_id);
                    string product_type_str = product_type.code;

                    // Product Category
                    var product_category = db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == product.product_category_id);
                    string product_category_str = product_category.code;

                    var productSummary = new ProductSummary(
                        product_id, product_name, 
                        product_brand_str, product_category_str, 
                        product_model_str, product_type_str, 
                        product.sell_price, quantity);

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
            Decimal total_price = (Decimal)(order.total_price + invoice.shipping_fee);

            // Check Shipment
            var shipment = db.TShopeeShipments.FirstOrDefault(it => it.invoice_id == invoice_id);

            mDestAddress destAddress = new mDestAddress();

            if (shipment != null)
            {
                string address = shipment.destination;
                address = (address == null) ? "" : address;

                String[] address_arr = generalFunc.FormatAddress(address);
                destAddress = new mDestAddress(address_arr);
            }
            else
            {
                destAddress = new mDestAddress(
                    customer.address_line_1, customer.address_line_2,
                    customer.city, customer.zip_code,
                    customer.state, customer.country);
            }

            PackageListingSummary model = new PackageListingSummary(
                stockWarehouse, customer, invoice, destAddress,
                productSummaryList, order.total_price, total_price);

            return PartialView("_PackageListingPartial", model);
        }


    }
}