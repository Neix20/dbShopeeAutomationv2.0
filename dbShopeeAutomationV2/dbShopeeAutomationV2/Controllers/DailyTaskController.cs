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
            int inv_sta_id = db.TShopeeInvoiceStatus.FirstOrDefault(it =>
                it.name.ToLower().Equals("Incomplete".ToLower())
            ).invoice_status_id;

            return invoiceList.Where(it => it.invoice_status_id == inv_sta_id).ToList().Count;
        }

        // GET: DailyTask
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DailyTaskPartial()
        {
            int inv_sta_id = db.TShopeeInvoiceStatus.FirstOrDefault(it =>
                it.name.ToLower().Equals("Incomplete".ToLower())
            ).invoice_status_id;

            var model = db.TShopeeInvoices.AsEnumerable().Where(
                it =>
                {
                    if (it.invoice_completed_date != null)
                    {
                        DateTime tmp = (DateTime)it.invoice_completed_date;
                        return it.invoice_status_id == inv_sta_id || tmp.Date == DateTime.Now.Date;
                    }
                    return it.invoice_status_id == inv_sta_id;
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
                int inv_sta_id = db.TShopeeInvoiceStatus.FirstOrDefault(it =>
                    it.name.ToLower().Equals("Incomplete".ToLower())
                ).invoice_status_id;

                model = db.TShopeeInvoices.AsEnumerable().Where(
                    it =>
                    {
                        if (it.invoice_completed_date != null)
                        {
                            DateTime tmp = (DateTime)it.invoice_completed_date;
                            return it.invoice_status_id == inv_sta_id || tmp.Date == DateTime.Now.Date;
                        }
                        return it.invoice_status_id == inv_sta_id;
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
        public ActionResult Update(FormCollection collection)
        {
            string username = User.Identity.Name;

            int invoice_id = int.Parse(generalFunc.trimStr(collection["invoice_id"]));
            int invoice_status_id = int.Parse(generalFunc.trimStr(collection["invoice_status_id"]));

            TShopeeInvoice item = db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == invoice_id);
            item.invoice_status_id = invoice_status_id;

            int c_inv_sta_id = db.TShopeeInvoiceStatus.FirstOrDefault(it =>
                it.name.ToLower().Equals("Complete".ToLower())
            ).invoice_status_id;

            if (item.invoice_status_id == c_inv_sta_id)
            {
                item.invoice_completed_date = DateTime.Now;
            }

            dbStoredProcedure.invoiceUpdate(item.invoice_id, item.invoice_title, item.invoice_created_date, item.invoice_completed_date, item.invoice_details, item.shipping_fee, item.invoice_status_id, item.payment_method_id, item.order_id, item.customer_id, username);
            db.SaveChanges();

            return RedirectToAction("Index", "DailyTask");
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

                foreach (var invoice_id in invoice_id_arr)
                {
                    // 1. Get Order ID
                    int order_id = (int)db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == invoice_id).order_id;

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

        public ActionResult PrintBill(int invoice_id)
        {
            var model = db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == invoice_id);

            String str = $"<h1>Invoice Code: {model.invoice_title}</h1>";

            return Content(str);
        }


    }
}