using IronXL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using dbShopeeAutomationV2.Models;

namespace dbShopeeAutomationV2.Controllers
{
    public class ProductExcelController : Controller
    {
        // GET: ProductExcel
        public ActionResult Index()
        {
            return View();
        }

        dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public string readExcelFile(string fileName, string username)
        {
            string str = "";
            // Read 'Supplier Info' Worksheet
            str += getSupplierInfo(fileName, username);
            //str += getRawMaterialTrackingInfo(fileName, username);


            return $"<table>{str}</table>";
        }

        // 1.
        public string getSupplierInfo(string fileName, string username)
        {
            string str = "";

            WorkBook wb = WorkBook.Load(fileName);
            WorkSheet ws = wb.GetWorkSheet("Supplier Info");

            // Get Only Rows without headers
            foreach(var row in ws.Rows.ToList().GetRange(1, ws.RowCount - 1))
            {
                var tmp_arr = row.ToArray();

                string supplier_name = (string)tmp_arr[1].Value;
                string supplier_nation = (string)tmp_arr[2].Value;
                string supplier_code = (string)tmp_arr[3].Value;

                if (supplier_name == "" || supplier_name.Equals("Supplier")) continue;

                dbStoredProcedure.supplierInsert(supplier_name, supplier_code, supplier_nation, username);
            }
            db.SaveChanges();

            return str;
        }

        // 2.
        //public string getRawMaterialTrackingInfo(string fileName, string username)
        //{
        //    string str = "";

        //    WorkBook wb = WorkBook.Load(fileName);
        //    WorkSheet ws = wb.GetWorkSheet("Raw Material Tracking Info");

        //    //// Get Only Rows without headers
        //    //foreach (var row in ws.Rows.ToList().GetRange(5, ws.RowCount - 1))
        //    //{
        //    //    var tmp_arr = row.ToArray();

        //    //    string supplier_name = (string)tmp_arr[1].Value;
        //    //    string supplier_nation = (string)tmp_arr[2].Value;
        //    //    string supplier_code = (string)tmp_arr[3].Value;

        //    //    if (supplier_name == "" || supplier_name.Equals("Supplier")) continue;

        //    //    dbStoredProcedure.supplierInsert(supplier_name, supplier_code, supplier_nation, username);
        //    //}
        //    //db.SaveChanges();

        //    str += String.Join("", ws.Rows.ToList().Select(x => $"<tr>{String.Join("", x.ToList().Select(y => $"<td>{y}</td>").ToArray())}<tr>"));
        //    return str;
        //}

        [HttpPost]
        public ActionResult uploadFile()
        {
            string username = User.Identity.Name;

            var file = Request.Files["fileUpload"];

            if (file != null && file.ContentLength > 0)
            {
                string file_path = $"{Server.MapPath("~/Content/Uploads")}\\{file.FileName}";

                if (!Directory.Exists(file_path)) Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));

                // If File Exist, delete existing file
                if (System.IO.File.Exists(file_path)) System.IO.File.Delete(file_path);
                file.SaveAs(file_path);

                readExcelFile(file_path, username);

                return Content($"File {file.FileName} Uploaded Successfully!");
            }

            return Content($"Error! File {file.FileName} was not uploaded successfully...");
        }
    }
}