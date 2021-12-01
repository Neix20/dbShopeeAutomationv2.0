using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class TaskController : AdminController
    {
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public int numOfOrdersLeft(IEnumerable<TShopeeInvoice> invoiceList)
        {
            int inv_sta_id = db.TShopeeInvoiceStatus.FirstOrDefault(it =>
                it.name.ToLower().Equals("Incomplete".ToLower())
            ).invoice_status_id;

            return invoiceList.Where(it => it.invoice_status_id == inv_sta_id).ToList().Count;
        }

        [ValidateInput(false)]
        public ActionResult InvoiceTaskGridViewPartial()
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

            return PartialView("_InvoiceTaskGridViewPartial", model.ToList());
        }

        [HttpPost]
        public ActionResult InvoiceTaskGridViewShow(TShopeeInvoice item)
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

                return PartialView("_InvoiceTaskGridViewPartial", model.ToList());
            }

            model = db.TShopeeInvoices.AsEnumerable().Where(
                it =>
                {
                    if(it.invoice_completed_date != null)
                    {
                        DateTime tmp = (DateTime)it.invoice_completed_date;
                        return tmp.Date == ((DateTime)item.invoice_completed_date).Date;
                    }
                    return false;
                    
                }
            );

            ViewData["num_of_orders_left"] = numOfOrdersLeft(model);

            return PartialView("_InvoiceTaskGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceTaskGridViewPartialAddNew(TShopeeInvoice item)
        {
            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceTaskGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceTaskGridViewPartialUpdate(TShopeeInvoice item)
        {
            string username = User.Identity.Name;

            int i_inv_sta_id = (int) item.invoice_status_id;
            item = db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == item.invoice_id);
            item.invoice_status_id = i_inv_sta_id;

            int c_inv_sta_id = db.TShopeeInvoiceStatus.FirstOrDefault(it =>
                it.name.ToLower().Equals("Complete".ToLower())
            ).invoice_status_id;

            if (item.invoice_status_id == c_inv_sta_id)
            {
                item.invoice_completed_date = DateTime.Now;
            }

            dbStoredProcedure.invoiceUpdate(item.invoice_id, item.invoice_title, item.invoice_created_date, item.invoice_completed_date, item.invoice_details, item.shipping_fee, item.invoice_status_id, item.payment_method_id, item.order_id, item.customer_id, username);
            db.SaveChanges();

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

            return PartialView("_InvoiceTaskGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult InvoiceTaskGridViewPartialDelete(int invoice_id)
        {
            var model = db.TShopeeInvoices;
            return PartialView("_InvoiceTaskGridViewPartial", model.ToList());
        }
    }
}