using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dbShopeeAutomationV2.Models
{
    public static class dbStatusFunction
    {
        static dbShopeeAutomationV2Entities db = new dbShopeeAutomationV2Entities();

        public static int invoiceStatusID(string name)
        {
            var invoiceStatus = db.TShopeeInvoiceStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (invoiceStatus == null) ? -1 : invoiceStatus.invoice_status_id;
        }

        public static int orderStatusID(string name)
        {
            var orderStatus = db.TShopeeOrderStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (orderStatus == null) ? -1 : orderStatus.order_status_id;
        }

        public static int productBrandID(string name)
        {
            var productBrand = db.TShopeeProductBrands.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productBrand == null) ? -1 : productBrand.product_brand_id;
        }

        public static int productCategoryID(string name)
        {
            var productCategory = db.TShopeeProductCategories.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productCategory == null) ? -1 : productCategory.product_category_id;
        }

        public static int productModelID(string name)
        {
            var productModel = db.TShopeeProductModels.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productModel == null) ? -1 : productModel.product_model_id;
        }

        public static int productModelCodeID(string code)
        {
            var productModel = db.TShopeeProductModels.FirstOrDefault(it => it.code == code);
            return (productModel == null) ? -1 : productModel.product_model_id;
        }

        public static int productTypeID(string name)
        {
            var productType = db.TShopeeProductTypes.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productType == null) ? -1 : productType.product_type_id;
        }

        public static int productVarietyID(string name)
        {
            var productVariety = db.TShopeeProductVarieties.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productVariety == null) ? -1 : productVariety.product_variety_id;
        }

        public static int productVarietyCodeID(string code)
        {
            var productVariety = db.TShopeeProductVarieties.FirstOrDefault(it => it.code == code);
            return (productVariety == null) ? -1 : productVariety.product_variety_id;
        }

        public static int productStatusID(string name)
        {
            var productStatus = db.TShopeeProductStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productStatus == null) ? -1 : productStatus.product_status_id;
        }

        public static int productionID(string title)
        {
            var production = db.TShopeeProductions.FirstOrDefault(it => it.title.ToLower().Equals(title.ToLower()));
            return (production == null) ? -1 : production.production_id;
        }

        public static int productIdByCode(string code)
        {
            var product = db.TShopeeProducts.FirstOrDefault(it => it.product_code.Equals(code));
            return (product == null) ? -1 : product.product_id;
        }

        public static int productionStatusID(string name)
        {
            var productionStatus = db.TShopeeProductionStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (productionStatus == null) ? -1 : productionStatus.production_status_id;
        }

        public static int supplierID(string name)
        {
            var supplier = db.TShopeeSuppliers.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (supplier == null) ? -1 : supplier.supplier_id;
        }

        public static int shipmentStatusID(string name)
        {
            var shipmentStatus = db.TShopeeShipmentStatus.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (shipmentStatus == null) ? -1 : shipmentStatus.shipment_status_id;
        }

        public static int stockWarehouseID(string name)
        {
            var stockWarehouse = db.TShopeeStockWarehouses.FirstOrDefault(it => it.name.ToLower().Equals(name.ToLower()));
            return (stockWarehouse == null) ? -1 : stockWarehouse.stock_warehouse_id;
        }
    }
}