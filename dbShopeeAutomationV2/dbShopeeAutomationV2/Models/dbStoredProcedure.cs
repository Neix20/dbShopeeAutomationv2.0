using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public static class dbStoredProcedure
    {
        static dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // Details Stored Procedure
        public static int detailInsert(string status, string remark, string created_by, DateTime created_date, string last_updated_by, DateTime last_updated_date)
        {
            return db.NSP_TShopeeDetail_Insert(status, remark, created_by, created_date, last_updated_by, last_updated_date);
        }

        public static int detailInsert(TShopeeDetail item)
        {
            DateTime currentTime = DateTime.Now;
            return db.NSP_TShopeeDetail_Insert(item.status, item.remark, item.created_by, currentTime, item.last_updated_by, currentTime);
        }

        public static int detailUpdate(TShopeeDetail item, string username)
        {
            DateTime currentTime = DateTime.Now;
            return db.NSP_TShopeeDetail_Update(item.detail_id ,item.status, item.remark, item.created_by, item.created_date, username, currentTime);
        }

        public static int detailDelete(int detail_id)
        {
            return db.NSP_TShopeeDetail_Delete(detail_id);
        }

        // Product Brand Stored Procedure
        public static int productBrandInsert(string name, string username)
        {
            string status = $"Product Brand {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();

            return db.NSP_TShopeeProductBrand_Insert(name, detail_id);
        }

        public static int productBrandUpdate(int product_brand_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product_brand_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductBrand_Update(product_brand_id, name, detail_id);
        }

        public static int productBrandDelete(int product_brand_id)
        {
            int detail_id = (int) db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product_brand_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductBrand_Delete(product_brand_id);
        }

        // Product Types Stored Procedure
        public static int productTypeInsert(string name, string username)
        {
            string status = $"Product Type {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductType_Insert(name, detail_id);
        }

        public static int productTypeUpdate(int product_type_id, string name, string username)
        {
            int detail_id = (int) db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product_type_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductType_Update(product_type_id, name, detail_id);
        }

        public static int productTypeDelete(int product_type_id)
        {
            int detail_id = (int) db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product_type_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductType_Delete(product_type_id);
        }

        // Product Variety Stored Procedure
        public static int productVarietyInsert(string name, string username)
        {
            string status = $"Product Variety {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductVariety_Insert(name, detail_id);
        }

        public static int productVarietyUpdate(int product_variety_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == product_variety_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductVariety_Update(product_variety_id, name, detail_id);
        }

        public static int productVarietyDelete(int product_variety_id)
        {
            int detail_id = (int)db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == product_variety_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductVariety_Delete(product_variety_id);
        }
    }
}