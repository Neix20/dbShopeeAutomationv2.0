using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public static class dbStoredProcedure
    {
        // Memory Optimization:
        // 1. Do not use TShopeeDetail item as parameters, as it creates a new object each time TShopeeDetail is created.
        // Replace TShopeeDetail item using its parameters (u will know wat I mean, future me)

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
        public static int stockItemInsert(string name, string description, int quantity, int product_id, int warehouse_id, string username)
        {
            string status = $"Stock Item: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeStockItem_Insert(name, description, quantity, product_id, warehouse_id, detail_id);
        }

        public static int stockItemUpdate(int stock_item_id, string name, string description, int quantity, int product_id, int warehouse_id, string username)
        {
            int detail_id = (int)db.TShopeeStockItems.FirstOrDefault(it => it.stock_item_id == stock_item_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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
        public static int productionDetailInsert(string UOM, DateTime? manufactured_date, DateTime? expiry_date, int? quantity, int product_id, int? production_id, string username)
        {
            string production_title = db.TShopeeProductions.FirstOrDefault(it => it.production_id == production_id).title;
            string product_sku = db.TShopeeProducts.FirstOrDefault(it => it.product_id == product_id).SKU;

            // Create New Detail
            string status = $"Production: {production_title}, Production Detail: {product_sku}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeProductionDetail_Insert(UOM, manufactured_date, expiry_date, quantity, production_id, product_id, detail_id);
        }

        public static int productionDetailUpdate(int production_detail_id, string UOM, DateTime? manufactured_date, DateTime? expiry_date, int? quantity, int product_id, int? production_id, string username)
        {
            int detail_id = (int)db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == production_detail_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeProductionDetail_Update(production_detail_id, UOM, manufactured_date, expiry_date, quantity, production_id, product_id, detail_id);
        }

        public static int productionDetailDelete(int production_detail_id)
        {
            int detail_id = (int)db.TShopeeProductionDetails.FirstOrDefault(it => it.production_detail_id == production_detail_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeProductionDetail_Delete(production_detail_id);
        }

        // Platform Stored Procedure
        public static int platformInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Platform: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeePlatform_Insert(name, detail_id);
        }

        public static int platformUpdate(int platform_id, string name, string username)
        {
            int detail_id = (int) db.TShopeePlatforms.FirstOrDefault(it => it.platform_id == platform_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeePaymentMethod_Insert(name, detail_id);
        }

        public static int paymentMethodUpdate(int payment_method_id, string name, string username)
        {
            int detail_id = (int)db.TShopeePaymentMethods.FirstOrDefault(it => it.payment_method_id == payment_method_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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
            string status = $"Order Item Status: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeOrderItemStatus_Insert(name, description, rma, detail_id);
        }

        public static int orderItemStatusUpdate(int order_item_status_id, string name, string description, int? rma, string username)
        {
            int detail_id = (int)db.TShopeeOrderItemStatus.FirstOrDefault(it => it.order_item_status_id == order_item_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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
        public static int invoiceStatusInsert(string name, string description, string username)
        {
            // Create New Detail
            string status = $"Invoice Status: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeInvoiceStatus_Insert(name, description, detail_id);
        }

        public static int invoiceStatusUpdate(int invoice_status_id, string name, string description, string username)
        {
            int detail_id = (int) db.TShopeeInvoiceStatus.FirstOrDefault(it => it.invoice_status_id == invoice_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeInvoiceStatus_Update(invoice_status_id, name, description, detail_id);
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
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeOrderStatus_Insert(name, detail_id);
        }

        public static int orderStatusUpdate(int order_status_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeOrderStatus.FirstOrDefault(it => it.order_status_id == order_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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

        // Shipment Status Stored Procedure
        public static int shipmentStatusInsert(string name, string username)
        {
            // Create New Detail
            string status = $"Shipment Status: {name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeShipmentStatus_Insert(name, detail_id);
        }

        public static int shipmentStatusUpdate(int shipment_status_id, string name, string username)
        {
            int detail_id = (int) db.TShopeeShipmentStatus.FirstOrDefault(it => it.shipment_status_id == shipment_status_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeCarrier_Insert(name, detail_id);
        }

        public static int carrierUpdate(int carrier_id, string name, string username)
        {
            int detail_id = (int)db.TShopeeCarriers.FirstOrDefault(it => it.carrier_id == carrier_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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

        // Customer Stored Procedure
        public static int customerInsert(string full_name, DateTime? dob, string email_address, string phone_number, string address, int platform_id, string username)
        {
            string[] name_arr = full_name.Split(new[] { " " }, StringSplitOptions.None);
            string first_name = name_arr[0];
            string last_name = name_arr[1];

            // Split Address
            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            // Create New Detail
            string status = $"Customer: {full_name}";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeCustomer_Insert(first_name, last_name, dob, email_address, phone_number, address_line_1, address_line_2, city, state, zip_code, country, platform_id, detail_id);
        }

        public static int customerUpdate(int customer_id,string full_name, DateTime? dob, string email_address, string phone_number, string address, int platform_id, string username)
        {
            string[] name_arr = full_name.Split(new[] { " " }, StringSplitOptions.None);
            string first_name = name_arr[0];
            string last_name = name_arr[1];

            // Split Address
            string[] address_arr = address.Split(new[] { ", " }, StringSplitOptions.None);

            string address_line_1 = address_arr[0];
            string address_line_2 = address_arr[1];
            string city = address_arr[2];
            int zip_code = int.Parse(address_arr[3]);
            string state = address_arr[4];
            string country = address_arr[5];

            int detail_id = (int)db.TShopeeCustomers.FirstOrDefault(it => it.customer_id == customer_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detailUpdate(detail, username);
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
        public static int orderInsert(DateTime? order_placed_date, Decimal? total_price, string order_status, string username)
        {
            // Create Order Status if It doesn't Exist
            var order_status_list = db.TShopeeOrderStatus.Select(x => x.name.ToLower()).ToList();
            if (!order_status_list.Contains(order_status.ToLower()))
            {
                orderStatusInsert(order_status, username);
                db.SaveChanges();
            }

            // Create New Detail
            string status = "";
            string remark = "";
            detailInsert(new TShopeeDetail(status, remark, username, username));
            db.SaveChanges();

            int detail_id = db.Database.SqlQuery<int>("SELECT CAST(IDENT_CURRENT('TShopeeDetail') AS INT)").FirstOrDefault();
            return db.NSP_TShopeeOrder_Insert(order_placed_date, total_price, order_status, detail_id);
        }

        public static int orderUpdate(int order_id, DateTime? order_placed_date, Decimal? total_price, string order_status, string username)
        {
            // Create Order Status if It doesn't Exist
            var order_status_list = db.TShopeeOrderStatus.Select(x => x.name.ToLower()).ToList();
            if (!order_status_list.Contains(order_status.ToLower()))
            {
                orderStatusInsert(order_status, username);
                db.SaveChanges();
            }

            int detail_id = (int)db.TShopeeOrders.FirstOrDefault(it => it.order_id == order_id).detail_id;
            TShopeeDetail detail = db.TShopeeDetails.FirstOrDefault(it => it.detail_id == detail_id);
            detail.status = $"Order ID: {order_id}";
            detailUpdate(detail, username);
            db.SaveChanges();

            return db.NSP_TShopeeOrder_Update(order_id, order_placed_date, total_price, order_status, detail_id);
        }

        public static int orderDelete(int order_id) {
            int detail_id = (int)db.TShopeeOrders.FirstOrDefault(it => it.order_id == order_id).detail_id;
            detailDelete(detail_id);
            db.SaveChanges();

            return db.NSP_TShopeeOrder_Delete(order_id);
        }

        // Order Item Stored Procedure

        // Invoice Stored Procedure

        // Shipment Stored Procedure
    }
}