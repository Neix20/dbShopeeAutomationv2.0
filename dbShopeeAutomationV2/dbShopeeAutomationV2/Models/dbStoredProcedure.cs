using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public static class dbStoredProcedure
    {
        static dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        // Details Section
        // Details Stored Procedure
        public static int detailInsert(string status, string remark, string created_by, string last_updated_by)
        {
            DateTime currentTime = DateTime.Now;
            return db.NSP_TShopeeDetail_Insert(status, remark, created_by, currentTime, last_updated_by, currentTime);
        }

        public static int detailUpdate(int detail_id, string status, string remark, string created_by, DateTime? created_date, string last_updated_by)
        {
            DateTime currentTime = DateTime.Now;
            return db.NSP_TShopeeDetail_Update(detail_id, status, remark, created_by, created_date, last_updated_by, currentTime);
        }

        public static int detailDelete(int detail_id)
        {
            return db.NSP_TShopeeDetail_Delete(detail_id);
        }

        // User Stored Procedure
        public static int userInsert(string username, string password, string email)
        {
            return db.NSP_TShopeeUser_Insert(username, password, email);
        }

        public static int userUpdate(int user_id, string username, string password, string email)
        {
            return db.NSP_TShopeeUser_Update(user_id, username, password, email);
        }

        public static int userDelete(int user_id)
        {
            return db.NSP_TShopeeUser_Delete(user_id);
        }

        // User Role Stored Procedure
        public static int userRoleInsert(string username, string role)
        {
            return db.NSP_TShopeeUserRole_Insert(username, role);
        }

        public static int userRoleUpdate(int user_role_id, string username, string role)
        {
            return db.NSP_TShopeeUserRole_Update(user_role_id, username, role);
        }

        public static int userRoleDelete(int user_role_id)
        {
            return db.NSP_TShopeeUserRole_Delete(user_role_id);
        }

        // Product Brand Stored Procedure
        public static int productBrandInsert(string name, string username)
        {
            string status = $"Product Brand: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductBrand_Insert(name, detail_id);
        }

        public static int productBrandUpdate(int product_brand_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product_brand_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Product Brand: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductBrand_Update(product_brand_id, name, detail_id);
        }

        public static int productBrandDelete(int product_brand_id)
        {
            int detail_id = (int)db.TShopeeProductBrands.FirstOrDefault(it => it.product_brand_id == product_brand_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductBrand_Delete(product_brand_id);
        }

        // Product Category
        public static int productCategoryInsert(string name, string username)
        {
            string status = $"Product Category: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductCategory_Insert(name, detail_id);
        }

        public static int productCategoryUpdate(int product_category_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == product_category_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Product Category: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductCategory_Update(product_category_id, name, detail_id);
        }

        public static int productCategoryDelete(int product_category_id)
        {
            int detail_id = (int)db.TShopeeProductCategories.FirstOrDefault(it => it.product_category_id == product_category_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductCategory_Delete(product_category_id);
        }

        // Product Model
        public static int productModelInsert(string name, string username)
        {
            string status = $"Product Model: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductModel_Insert(name, detail_id);
        }

        public static int productModelUpdate(int product_model_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductModels.FirstOrDefault(it => it.product_model_id == product_model_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Product Model: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductModel_Update(product_model_id, name, detail_id);
        }

        public static int productModelDelete(int product_model_id)
        {
            int detail_id = (int)db.TShopeeProductModels.FirstOrDefault(it => it.product_model_id == product_model_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductModel_Delete(product_model_id);
        }

        // Product Status
        public static int productStatusInsert(string name, string username)
        {
            string status = $"Product Status: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductStatus_Insert(name, detail_id);
        }

        public static int productStatusUpdate(int product_status_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductStatus.FirstOrDefault(it => it.product_status_id == product_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Product Status: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductStatus_Update(product_status_id, name, detail_id);
        }

        public static int productStatusDelete(int product_status_id)
        {
            int detail_id = (int)db.TShopeeProductStatus.FirstOrDefault(it => it.product_status_id == product_status_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductStatus_Delete(product_status_id);
        }

        // Product Types Stored Procedure
        public static int productTypeInsert(string name, string username)
        {
            string status = $"Product Type: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductType_Insert(name, detail_id);
        }

        public static int productTypeUpdate(int product_type_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product_type_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Product Type: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductType_Update(product_type_id, name, detail_id);
        }

        public static int productTypeDelete(int product_type_id)
        {
            int detail_id = (int)db.TShopeeProductTypes.FirstOrDefault(it => it.product_type_id == product_type_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductType_Delete(product_type_id);
        }

        // Product Variety Stored Procedure
        public static int productVarietyInsert(string name, string username)
        {
            string status = $"Product Variety: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductVariety_Insert(name, detail_id);
        }

        public static int productVarietyUpdate(int product_variety_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductVarieties.FirstOrDefault(it => it.product_variety_id == product_variety_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Product Variety: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
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
        public static int stockWarehouseInsert(string name, string email_address, string phone_number, string address_line_1, string address_line_2, string city, int? zip_code, string state, string country, string username)
        {
            string status = $"Stock Warehouse: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeStockWarehouse_Insert(name, email_address, phone_number, address_line_1, address_line_2, city, state, zip_code, country, detail_id);
        }

        public static int stockWarehouseUpdate(int stock_warehouse_id, string name, string email_address, string phone_number, string address_line_1, string address_line_2, string city, int? zip_code, string state, string country, string username)
        {
            int detail_id = (int)db.TShopeeStockWarehouses.FirstOrDefault(it => it.stock_warehouse_id == stock_warehouse_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Stock Warehouse: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

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
        public static int stockItemInsert(string name, string description, int quantity, int product_id, int warehouse_id, string username)
        {
            string status = $"Stock Item: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeStockItem_Insert(name, description, quantity, product_id, warehouse_id, detail_id);
        }

        public static int stockItemUpdate(int stock_item_id, string name, string description, int quantity, int product_id, int warehouse_id, string username)
        {
            int detail_id = (int)db.TShopeeStockItems.FirstOrDefault(it => it.stock_item_id == stock_item_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Stock Item: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

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
        public static int productInsert(
            string product_code, string name, 
            string description, string SKU, string SKU2, 
            Decimal? buy_price, Decimal? sell_price, 
            int? product_brand_id, int? product_model_id, int? product_category_id,
            int? product_type_id, int? product_variety_id, int? product_status_id, string username)
        {
            // Create New Detail
            string status = $"Product: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProduct_Insert(
                product_code, name, 
                description, SKU, SKU2, 
                buy_price, sell_price, 
                product_brand_id, product_model_id, product_category_id,
                product_type_id, product_variety_id, product_status_id, detail_id);
        }

        public static int productUpdate(
            int product_id, string product_code, string name, 
            string description, string SKU, string SKU2, 
            Decimal? buy_price, Decimal? sell_price,
            int? product_brand_id, int? product_model_id, int? product_category_id,
            int? product_type_id, int? product_variety_id, int? product_status_id, string username)
        {
            int detail_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Product: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProduct_Update(
                product_id, product_code, name, 
                description, SKU, SKU2, 
                buy_price, sell_price,
                product_brand_id, product_model_id, product_category_id,
                product_type_id, product_variety_id, product_status_id, detail_id);
        }

        public static int productDelete(int product_id)
        {
            int detail_id = (int)db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProduct_Delete(product_id);
        }

        // Product Component
        public static int productComponentInsert(int? master_product_id, int? sub_product_id, int? quantity, string username)
        {
            string master_product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == master_product_id).name;
            string sub_product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == sub_product_id).name;
            
            // Create New Detail
            string status = $"Master product: {master_product}, Sub Product: {sub_product}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductComponent_Insert(master_product_id, sub_product_id, quantity, detail_id);
        }

        public static int productComponentUpdate(int product_component_id, int? master_product_id, int? sub_product_id, int? quantity, string username)
        {
            string master_product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == master_product_id).name;
            string sub_product = db.TShopeeProducts.FirstOrDefault(it => it.product_id == sub_product_id).name;

            int detail_id = (int)db.TShopeeProductComponents.FirstOrDefault(it => it.product_component_id == product_component_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Master product: {master_product}, Sub Product: {sub_product}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductComponent_Update(product_component_id, master_product_id, sub_product_id, quantity, detail_id);
        }

        public static int productComponentDelete(int product_component_id)
        {
            int detail_id = (int)db.TShopeeProductComponents.FirstOrDefault(it => it.product_component_id == product_component_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductComponent_Delete(product_component_id);
        }

        // Production Status Stored Procedure
        public static int productionStatusInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Production Status: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductionStatus_Insert(name, detail_id);
        }

        public static int productionStatusUpdate(int production_status_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeProductionStatus.FirstOrDefault(it => it.production_status_id == production_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Production Status: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductionStatus_Update(production_status_id, name, detail_id);
        }

        public static int productionStatusDelete(int production_status_id)
        {
            int detail_id = (int)db.TShopeeProductionStatus.FirstOrDefault(it => it.production_status_id == production_status_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductionStatus_Delete(production_status_id);
        }

        // Production Stored Procedure
        public static int productionInsert(
            string title, string description, Decimal? total_usage, int? production_status_id, string username
        )
        {
            // Create New Detail
            string status = $"Production: {title}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProduction_Insert(title, description, total_usage, production_status_id, detail_id);
        }

        public static int productionUpdate(
            int production_id, string title, string description, Decimal? total_usage, int? production_status_id, string username
        )
        {
            int detail_id = (int)db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Production: {title}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProduction_Update(production_id, title, description, total_usage, production_status_id, detail_id);
        }

        public static int productionDelete(int production_id)
        {
            int detail_id = (int)db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProduction_Delete(production_id);
        }

        // Production Detail Stored Procedure
        public static int productionDetailInsert(
            string UOM, DateTime? manufactured_date, DateTime? expiry_date,
            Decimal? height, Decimal? width, Decimal? length, Decimal? quantity,
            int? cannot_be_used, int? can_be_used, int? production_id, int? product_id, string username
        )
        {
            string production_title = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).title;
            string product_sku = db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).SKU;

            // Create New Detail
            string status = $"Production: {production_title}, Production Detail: {product_sku}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductionDetail_Insert(
                UOM, manufactured_date, expiry_date,
                height, width, length,
                quantity, cannot_be_used, can_be_used,
                production_id, product_id, detail_id);
        }

        public static int productionDetailUpdate(
            int production_detail_id, string UOM, DateTime? manufactured_date, DateTime? expiry_date,
            Decimal? height, Decimal? width, Decimal? length, Decimal? quantity, 
            int? cannot_be_used, int? can_be_used, int? production_id, int? product_id, string username)
        {
            string production_title = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).title;
            string product_sku = db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).SKU;

            int detail_id = (int)db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == production_detail_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Production: {production_title}, Production Detail: {product_sku}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductionDetail_Update(
                production_detail_id, UOM, manufactured_date, expiry_date,
                height, width, length, quantity, 
                cannot_be_used, can_be_used, production_id, product_id, detail_id);
        }

        public static int productionDetailDelete(int production_detail_id)
        {
            int detail_id = (int)db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == production_detail_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductionDetail_Delete(production_detail_id);
        }

        // Supplier Stored Procedure
        public static int supplierInsert(string name, string code, string nation, string username)
        {
            // Create New Detail
            string status = $"Supplier: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeSupplier_Insert(name, code, nation, detail_id);
        }

        public static int supplierUpdate(int supplier_id, string name, string code, string nation, string username)
        {
            int detail_id = (int)db.TShopeeSuppliers.FirstOrDefault(it => it.supplier_id == supplier_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Supplier: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeSupplier_Update(supplier_id, name, code, nation, detail_id);
        }

        public static int supplierDelete(int supplier_id)
        {
            int detail_id = (int)db.TShopeeSuppliers.FirstOrDefault(it => it.supplier_id == supplier_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeSupplier_Delete(supplier_id);
        }

        // Supplier Shipment Stored Procedure
        public static int supplierShipmentInsert(
            DateTime? received_date, string supplier_tracking_id, string NTL_tracking_id, 
            Decimal? height, Decimal? width, Decimal? length, 
            int? supplier_id, int? product_id, string username)
        {
            // Create New Detail
            string status = $"Supplier Shipment: {supplier_tracking_id}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeSupplierShipment_Insert(
                received_date, supplier_tracking_id, NTL_tracking_id,
                height, width, length,
                supplier_id, product_id, detail_id
            );
        }

        public static int supplierShipmentUpdate(
            int supplier_shipment_id, DateTime? received_date, string supplier_tracking_id, string NTL_tracking_id,
            Decimal? height, Decimal? width, Decimal? length,
            int? supplier_id, int? product_id, string username)
        {
            int detail_id = (int)db.TShopeeSupplierShipments.FirstOrDefault(it => it.supplier_shipment_id == supplier_shipment_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Supplier Shipment: {supplier_tracking_id}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeSupplierShipment_Update(
                supplier_shipment_id, received_date, supplier_tracking_id, NTL_tracking_id,
                height, width, length,
                supplier_id, product_id, detail_id);
        }

        public static int supplierShipmentDelete(int supplier_shipment_id)
        {
            int detail_id = (int)db.TShopeeSupplierShipments.FirstOrDefault(it => it.supplier_shipment_id == supplier_shipment_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeSupplierShipment_Delete(supplier_shipment_id);
        }

        // Platform Stored Procedure
        public static int platformInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Platform: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeePlatform_Insert(name, detail_id);
        }

        public static int platformUpdate(int platform_id, string name, string username)
        {
            int detail_id = (int)db.TShopeePlatforms.FirstOrDefault(it => it.platform_id == platform_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Platform: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeePlatform_Update(platform_id, name, detail_id);
        }

        public static int platformDelete(int platform_id)
        {
            int detail_id = (int)db.TShopeePlatforms.FirstOrDefault(it => it.platform_id == platform_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeePlatform_Delete(platform_id);
        }

        // Payment Method Stored Procedure
        public static int paymentMethodInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Payment Method: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeePaymentMethod_Insert(name, detail_id);
        }

        public static int paymentMethodUpdate(int payment_method_id, string name, string username)
        {
            int detail_id = (int)db.TShopeePaymentMethods.FirstOrDefault(it => it.payment_method_id == payment_method_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Payment Method: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeePaymentMethod_Update(payment_method_id, name, detail_id);
        }

        public static int paymentMethodDelete(int payment_method_id)
        {
            int detail_id = (int)db.TShopeePaymentMethods.FirstOrDefault(it => it.payment_method_id == payment_method_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeePaymentMethod_Delete(payment_method_id);
        }

        // Order Item Status Stored Procedure
        public static int orderItemStatusInsert(string name, string description, int? rma, string username)
        {
            // Create New Detail
            string status = $"{name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeOrderItemStatus_Insert(name, description, rma, detail_id);
        }

        public static int orderItemStatusUpdate(int order_item_status_id, string name, string description, int? rma, string username)
        {
            int detail_id = (int)db.TShopeeOrderItemStatus.FirstOrDefault(it => it.order_item_status_id == order_item_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"{name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeOrderItemStatus_Update(order_item_status_id, name, description, rma, detail_id);
        }

        public static int orderItemStatusDelete(int order_item_status_id)
        {
            int detail_id = (int)db.TShopeeOrderItemStatus.FirstOrDefault(it => it.order_item_status_id == order_item_status_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeOrderItemStatus_Delete(order_item_status_id);
        }

        // Invoice Status Stored Procedure
        public static int invoiceStatusInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Invoice Status: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeInvoiceStatus_Insert(name, detail_id);
        }

        public static int invoiceStatusUpdate(int invoice_status_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeInvoiceStatus.FirstOrDefault(it => it.invoice_status_id == invoice_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Invoice Status: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeInvoiceStatus_Update(invoice_status_id, name, detail_id);
        }

        public static int invoiceStatusDelete(int invoice_status_id)
        {
            int detail_id = (int)db.TShopeeInvoiceStatus.FirstOrDefault(it => it.invoice_status_id == invoice_status_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeInvoiceStatus_Delete(invoice_status_id);
        }

        // Order Status Stored Procedure
        public static int orderStatusInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Order Status: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeOrderStatus_Insert(name, detail_id);
        }

        public static int orderStatusUpdate(int order_status_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeOrderStatus.FirstOrDefault(it => it.order_status_id == order_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Order Status: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeOrderStatus_Update(order_status_id, name, detail_id);
        }

        public static int orderStatusDelete(int order_status_id)
        {
            int detail_id = (int)db.TShopeeOrderStatus.FirstOrDefault(it => it.order_status_id == order_status_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeOrderStatus_Delete(order_status_id);
        }

        // Customer Stored Procedure
        public static int customerInsert(string first_name, string last_name, DateTime? dob, string email_address, string phone_number, string address_line_1, string address_line_2, string city, int? zip_code, string state, string country, int platform_id, string username)
        {
            // Create New Detail
            string status = $"Customer: {first_name} {last_name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeCustomer_Insert(first_name, last_name, dob, email_address, phone_number, address_line_1, address_line_2, city, state, zip_code, country, platform_id, detail_id);
        }

        public static int customerUpdate(int customer_id, string first_name, string last_name, DateTime? dob, string email_address, string phone_number, string address_line_1, string address_line_2, string city, int? zip_code, string state, string country, int platform_id, string username)
        {
            int detail_id = (int)db.TShopeeCustomers.FirstOrDefault(it => it.customer_id == customer_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Customer: {first_name} {last_name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeCustomer_Update(customer_id, first_name, last_name, dob, email_address, phone_number, address_line_1, address_line_2, city, state, zip_code, country, platform_id, detail_id);
        }

        public static int customerDelete(int customer_id)
        {
            int detail_id = (int)db.TShopeeCustomers.FirstOrDefault(it => it.customer_id == customer_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeCustomer_Delete(customer_id);
        }

        // Order Stored Procedure
        public static int orderInsert(string order_title, DateTime? order_placed_date, Decimal? total_price, int? order_status_id, string username)
        {
            // Create New Detail
            string status = $"Order: {order_title}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeOrder_Insert(order_title, order_placed_date, total_price, order_status_id, detail_id);
        }

        public static int orderUpdate(int order_id, string order_title, DateTime? order_placed_date, Decimal? total_price, int? order_status_id, string username)
        {
            int detail_id = (int)db.TShopeeOrders.FirstOrDefault(it => it.order_id == order_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Order: {order_title}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeOrder_Update(order_id, order_title, order_placed_date, total_price, order_status_id, detail_id);
        }

        public static int orderDelete(int order_id)
        {
            int detail_id = (int)db.TShopeeOrders.FirstOrDefault(it => it.order_id == order_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeOrder_Delete(order_id);
        }

        // Order Item Stored Procedure
        public static int orderItemInsert(int? quantity, Decimal? sub_total, Decimal? discount_fee, int? RMA_num, string RMA_issued_by, DateTime? RMA_issued_date, int? order_id, int? order_item_status_id, int? product_id, string username)
        {
            string product_sku = db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).SKU;

            // Create New Detail
            string status = $"Order ID: {order_id} Order Item: {product_sku}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            quantity = (quantity == null) ? 0 : quantity;
            sub_total = (sub_total == null) ? 0 : sub_total;
            discount_fee = (discount_fee == null) ? 0 : discount_fee;
            RMA_num = (RMA_num == null) ? 0 : RMA_num;
            RMA_issued_date = (RMA_issued_date == null) ? DateTime.Now : RMA_issued_date;

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeOrderItem_Insert(quantity, sub_total, discount_fee, RMA_num, RMA_issued_by, RMA_issued_date, order_id, order_item_status_id, product_id, detail_id);
        }

        public static int orderItemUpdate(int order_item_id, int? quantity, Decimal? sub_total, Decimal? discount_fee, int? RMA_num, string RMA_issued_by, DateTime? RMA_issued_date, int? order_id, int? order_item_status_id, int? product_id, string username)
        {
            string product_sku = db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).SKU;

            int detail_id = (int)db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == order_item_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Order ID: {order_id} Order Item: {product_sku}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            quantity = (quantity == null) ? 0 : quantity;
            sub_total = (sub_total == null) ? 0 : sub_total;
            discount_fee = (discount_fee == null) ? 0 : discount_fee;
            RMA_num = (RMA_num == null) ? 0 : RMA_num;
            RMA_issued_date = (RMA_issued_date == null) ? DateTime.Now : RMA_issued_date;

            return db.NSP_TShopeeOrderItem_Update(order_item_id, quantity, sub_total, discount_fee, RMA_num, RMA_issued_by, RMA_issued_date, order_id, order_item_status_id, product_id, detail_id);
        }

        public static int orderItemDelete(int order_item_id)
        {
            int detail_id = (int)db.TShopeeOrderItems.FirstOrDefault(it => it.order_item_id == order_item_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeOrderItem_Delete(order_item_id);
        }

        // Invoice Stored Procedure

        // Create Related Invoice Status
        // Delete Related Invoice Status 
        public static int invoiceInsert(string invoice_title, DateTime? invoice_created_date, DateTime? invoice_completed_date, string invoice_details, Decimal? shipping_fee, int? invoice_status_id, int? payment_method_id, int? order_id, int? customer_id, string username)
        {
            // Create New Detail
            string status = $"Invoice Name: {invoice_title}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeInvoice_Insert(invoice_title, invoice_created_date, invoice_completed_date, invoice_details, shipping_fee, invoice_status_id, payment_method_id, order_id, customer_id, detail_id);
        }

        public static int invoiceUpdate(int invoice_id, string invoice_title, DateTime? invoice_created_date, DateTime? invoice_completed_date, string invoice_details, Decimal? shipping_fee, int? invoice_status_id, int? payment_method_id, int? order_id, int? customer_id, string username)
        {
            int detail_id = (int)db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == invoice_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Invoice Name: {invoice_title}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeInvoice_Update(invoice_id, invoice_title, invoice_created_date, invoice_completed_date, invoice_details, shipping_fee, invoice_status_id, payment_method_id, order_id, customer_id, detail_id);
        }

        public static int invoiceDelete(int invoice_id)
        {
            int detail_id = (int)db.TShopeeInvoices.FirstOrDefault(it => it.invoice_id == invoice_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeInvoice_Delete(invoice_id);
        }

        // Shipment

        // Shipment Status Stored Procedure
        public static int shipmentStatusInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Shipment Status: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeShipmentStatus_Insert(name, detail_id);
        }

        public static int shipmentStatusUpdate(int shipment_status_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeShipmentStatus.FirstOrDefault(it => it.shipment_status_id == shipment_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Shipment Status: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeShipmentStatus_Update(shipment_status_id, name, detail_id);
        }

        public static int shipmentStatusDelete(int shipment_status_id)
        {
            int detail_id = (int)db.TShopeeShipmentStatus.FirstOrDefault(it => it.shipment_status_id == shipment_status_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeShipmentStatus_Delete(shipment_status_id);
        }

        // Carrier Stored Procedure
        public static int carrierInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Carrier: {name}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeCarrier_Insert(name, detail_id);
        }

        public static int carrierUpdate(int carrier_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeCarriers.FirstOrDefault(it => it.carrier_id == carrier_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Carrier: {name}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeCarrier_Update(carrier_id, name, detail_id);
        }

        public static int carrierDelete(int carrier_id)
        {
            int detail_id = (int)db.TShopeeCarriers.FirstOrDefault(it => it.carrier_id == carrier_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeCarrier_Delete(carrier_id);
        }

        // Shipment Stored Procedure
        public static int shipmentInsert(string start_location, string destination, string tracking_id, DateTime? created_date, DateTime? expected_date, DateTime? due_date, int? invoice_id, int? carrier_id, int? shipment_status_id, string username)
        {
            // Create New Detail
            string status = $"Shipment Tracking Code: {tracking_id}";
            string remark = "";
            detailInsert(status, remark, username, username);
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeShipment_Insert(start_location, destination, tracking_id, created_date, expected_date, due_date, invoice_id, carrier_id, shipment_status_id, detail_id);
        }

        public static int shipmentUpdate(int shipment_id, string start_location, string destination, string tracking_id, DateTime? created_date, DateTime? expected_date, DateTime? due_date, int? invoice_id, int? carrier_id, int? shipment_status_id, string username)
        {
            int detail_id = (int)db.TShopeeShipments.FirstOrDefault(it => it.shipment_id == shipment_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Shipment Tracking Code: {tracking_id}";
            detailUpdate(detail.detail_id, detail.status, detail.remark, detail.created_by, detail.created_date, username);
            db.SaveChanges();

            return db.NSP_TShopeeShipment_Update(shipment_id, start_location, destination, tracking_id, created_date, expected_date, due_date, invoice_id, carrier_id, shipment_status_id, detail_id);
        }

        public static int shipmentDelete(int shipment_id)
        {
            int detail_id = (int)db.TShopeeShipments.FirstOrDefault(it => it.shipment_id == shipment_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeShipment_Delete(shipment_id);
        }
    }
}