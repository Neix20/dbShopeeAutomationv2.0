using dbShopeeAutomationV2.Models;
using DevExpress.Web.Mvc;
using IronXL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace dbShopeeAutomationV2.Controllers
{
    public class PlatformController : AdminController
    {
        // GET: Platform
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public void customerInsert(string customer_name, string customer_phone_number, int platform_id, string username)
        {
            string name = customer_name;
            name = (name == "") ? "first_name last_name" : name;
            int name_count = name.Split(' ').Length - 1;
            name = (name_count < 1) ? $"{name} " : name;

            string[] name_arr = name.Split(new[] { " " }, StringSplitOptions.None);

            string first_name = name_arr[0];
            string last_name = name_arr[1];

            DateTime dob = DateTime.Now;

            string email_address = generalFunc.GenEmail();

            string phone_number = customer_phone_number;

            string address = "";

            int address_count = address.Split(new[] { ", " }, StringSplitOptions.None).Length - 1;
            address = (address_count != 5) ? "address line 1, address line 2, city, 00000, state, country" : address;

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            dbStoredProcedure.customerInsert(first_name, last_name, dob, email_address, phone_number, address_line_1, address_line_2, city, zip_code, state, country, platform_id, username);
            db.SaveChanges();
        }

        public void readExcelFile(string fileName, int platform_id, string username)
        {
            WorkBook wb = WorkBook.Load(fileName);
            WorkSheet ws = wb.GetWorkSheet("Shopee Report");

            // Get Only Rows without headers
            ws.Rows.ToList().GetRange(1, ws.RowCount - 1).ForEach(row =>
            {
                var tmp_arr = row.ToArray();

                // Create Customer / Buyer
                string customer_name = (string)tmp_arr[5].Value;
                string customer_phone_numer = (string)tmp_arr[6].Value;
                string first_name = customer_name.Split(new[] { " " }, StringSplitOptions.None)[0];
                var customer = db.TShopeeCustomers.FirstOrDefault(it => it.first_name.ToLower().Equals(first_name.ToLower()));
                int customer_id = 0;
                if (customer != null)
                {
                    customer_id = customer.customer_id;
                }
                else
                {
                    customerInsert(customer_name, customer_phone_numer, platform_id, username);
                    customer_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeCustomer') AS INT)").FirstOrDefault() + 1;
                }

                // Create Carrier
                string carrier_name = (string)tmp_arr[14].Value;
                var carrier = db.TShopeeCarriers.FirstOrDefault(it => it.name.ToLower().Equals(carrier_name.ToLower()));
                int carrier_id = 0;
                if (carrier != null)
                {
                    carrier_id = carrier.carrier_id;
                }
                else
                {
                    dbStoredProcedure.carrierInsert(carrier_name, username);
                    db.SaveChanges();
                    carrier_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeCarrier') AS INT)").FirstOrDefault() + 1;
                }

                // Create Order Status
                string order_status = (string)tmp_arr[1].Value;
                var orderStatus = db.TShopeeOrderStatus.FirstOrDefault(it => it.name.ToLower().Equals(order_status.ToLower()));
                int orderStatusId = 0;
                if (orderStatus != null)
                {
                    orderStatusId = orderStatus.order_status_id;
                }
                else
                {
                    dbStoredProcedure.orderStatusInsert(order_status, username);
                    db.SaveChanges();
                    orderStatusId = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeOrderStatus') AS INT)").FirstOrDefault() + 1;
                }

                // Create Payment Method
                string payment_method = (string)tmp_arr[2].Value;
                var paymentMethod = db.TShopeePaymentMethods.FirstOrDefault(it => it.name.ToLower().Equals(payment_method.ToLower()));
                int paymentMethodId = 0;
                if (paymentMethod != null)
                {
                    paymentMethodId = paymentMethod.payment_method_id;
                }
                else
                {
                    dbStoredProcedure.paymentMethodInsert(payment_method, username);
                    db.SaveChanges();
                    paymentMethodId = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeePaymentMethod') AS INT)").FirstOrDefault() + 1;
                }

                // Create Order

                // Create Order Item

                // Create Invoice

                // Create Shipment



            });
        }

        [ValidateInput(false)]
        public ActionResult PlatformGridViewPartial()
        {
            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialAddNew(TShopeePlatform item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "platform_name" : item.name;

            // Prevent Duplicate
            var platform = db.TShopeePlatforms.FirstOrDefault(it => it.name.ToLower().Equals(item.name.ToLower()));
            int platform_id = 0;
            if (platform != null)
            {
                platform_id = platform.platform_id;
            }
            else
            {
                dbStoredProcedure.platformInsert(item.name, username);
                db.SaveChanges();
                platform_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeePlatform') AS INT)").FirstOrDefault();
            }


            // File Upload / Read Excel
            var file = Request.Files["fileUpload"];

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/Uploads")}\\{file.FileName}";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);
                file.SaveAs(file_path);

                readExcelFile(file_path, platform_id, username);
            }

            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialUpdate(TShopeePlatform item)
        {
            string username = User.Identity.Name;

            item.name = (item.name == null) ? "platform_name" : item.name;

            dbStoredProcedure.platformUpdate(item.platform_id, item.name, username);
            db.SaveChanges();

            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult PlatformGridViewPartialDelete(int platform_id)
        {
            dbStoredProcedure.platformDelete(platform_id);
            db.SaveChanges();

            var model = db.TShopeePlatforms;
            return PartialView("_PlatformGridViewPartial", model.ToList());
        }
    }
}