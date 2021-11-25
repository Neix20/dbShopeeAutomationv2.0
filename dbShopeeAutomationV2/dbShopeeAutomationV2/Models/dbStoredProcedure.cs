﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public static class dbStoredProcedure
    {
        static dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // Details Stored Procedure
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
            string status = $"Product Brand: {name}";
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
            string status = $"Product Type: {name}";
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
            string status = $"Product Variety: {name}";
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

        // Stock Warehouse Stored Procedure
        public static int stockWarehouseInsert(string name, string email_address, string phone_number, string address, string username)
        {
            string status = $"Stock Warehouse: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];
            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeStockWarehouse_Insert(name, email_address, phone_number, address_line_1, address_line_2, city, state, zip_code, country, detail_id);
        }

        public static int stockWarehouseUpdate(int stock_warehouse_id, string name, string email_address, string phone_number, string address, string username)
        {
            int detail_id = (int)db.TShopeeStockWarehouses.FirstOrDefault(it => it.stock_warehouse_id == stock_warehouse_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            return db.NSP_TShopeeStockWarehouse_Update(stock_warehouse_id, name, email_address, phone_number, address_line_1, address_line_2, city, state, zip_code, country, detail_id);
        }

        public static int stockWarehouseDelete(int stock_warehouse_id)
        {
            int detail_id = (int)db.TShopeeStockWarehouses.FirstOrDefault(it => it.stock_warehouse_id == stock_warehouse_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeStockWarehouse_Delete(stock_warehouse_id);
        }

        // Stock Item Stored Procedure
        public static int stockItemInsert(string name, string description, int quantity, string product_sku, string warehouse_title, string username)
        {
            string status = $"Stock Item: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            int product_id = (int) db.TShopeeProducts.FirstOrDefault(it => it.SKU.Equals(product_sku)).product_id;
            int warehouse_id = (int) db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.Equals(warehouse_title)).stock_warehouse_id;

            return db.NSP_TShopeeStockItem_Insert(name, description, quantity, product_id, warehouse_id, detail_id);
        }

        public static int stockItemUpdate(int stock_item_id, string name, string description, int quantity, string product_sku, string warehouse_title, string username)
        {
            int detail_id = (int)db.TShopeeStockItems.FirstOrDefault(it => it.stock_item_id == stock_item_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            int product_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.SKU.Equals(product_sku)).product_id;
            int warehouse_id = (int)db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.Equals(warehouse_title)).stock_warehouse_id;

            return db.NSP_TShopeeStockItem_Update(stock_item_id, name, description, quantity, product_id, warehouse_id, detail_id);
        }

        public static int stockItemDelete(int stock_item_id)
        {
            int detail_id = (int)db.TShopeeStockItems.FirstOrDefault(it => it.stock_item_id == stock_item_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeStockItem_Delete(stock_item_id);
        }

        // Product Stored Procedure
        public static int productInsert(string product_code, string name, string description, string SKU, string SKU2, Decimal? buy_price, Decimal? sell_price, string product_brand, string product_type, string product_variety, string username)
        {
            // Check if Product Brand Exist
            var product_brand_list = db.TShopeeProductBrands.Select(x => x.name.ToLower()).ToList();
            if (!product_brand_list.Contains(product_brand.ToLower()))
            {
                productBrandInsert(product_brand, username);
                db.SaveChanges();
            }

            // Check if Product Type Exist
            var product_type_list = db.TShopeeProductTypes.Select(x => x.name.ToLower()).ToList();
            if (!product_type_list.Contains(product_type.ToLower()))
            {
                productTypeInsert(product_type, username);
                db.SaveChanges();
            }

            // Check if Product Variety Exist
            var product_variety_list = db.TShopeeProductVarieties.Select(x => x.name.ToLower()).ToList();
            if (!product_variety_list.Contains(product_variety.ToLower()))
            {
                productVarietyInsert(product_variety, username);
                db.SaveChanges();
            }

            // Create New Detail
            string status = $"Product: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProduct_Insert(product_code, name, description, SKU, SKU2, buy_price, sell_price, product_brand, product_type, product_variety, detail_id);
        }

        public static int productUpdate(int product_id, string product_code, string name, string description, string SKU, string SKU2, Decimal? buy_price, Decimal? sell_price, string product_brand, string product_type, string product_variety, string username)
        {
            // Check if Product Brand Exist
            var product_brand_list = db.TShopeeProductBrands.Select(x => x.name.ToLower()).ToList();
            if (!product_brand_list.Contains(product_brand.ToLower()))
            {
                productBrandInsert(product_brand, username);
                db.SaveChanges();
            }

            // Check if Product Type Exist
            var product_type_list = db.TShopeeProductTypes.Select(x => x.name.ToLower()).ToList();
            if (!product_type_list.Contains(product_type.ToLower()))
            {
                productTypeInsert(product_type, username);
                db.SaveChanges();
            }

            // Check if Product Variety Exist
            var product_variety_list = db.TShopeeProductVarieties.Select(x => x.name.ToLower()).ToList();
            if (!product_variety_list.Contains(product_variety.ToLower()))
            {
                productVarietyInsert(product_variety, username);
                db.SaveChanges();
            }

            int detail_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeProduct_Update(product_id, product_code, name, description, SKU, SKU2, buy_price, sell_price, product_brand, product_type, product_variety, detail_id);
        }

        public static int productDelete(int product_id)
        {
            int detail_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProduct_Delete(product_id);
        }

        // Production Stored Procedure
        public static int productionInsert(string title, string description, string production_status, string username)
        {
            // Create New Detail
            string status = $"Production: {title}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProduction_Insert(title, description, production_status, detail_id);
        }

        public static int productionUpdate(int production_id, string title, string description, string production_status, string username)
        {
            int detail_id = (int)db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeProduction_Update(production_id, title, description, production_status, detail_id);
        }

        public static int productionDelete(int production_id)
        {
            int detail_id = (int)db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProduction_Delete(production_id);
        }

        // Production Detail Stored Procedure
    }
}