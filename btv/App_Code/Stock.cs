using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using RunQuery;

namespace Stock
{
    public class Inventory
    {
        public static string GetProductName(string pid)
        {
            pid = SQLQuery.ReturnString("Select ItemName from Products where ProductID='" + pid + "'");
            return pid;
        }

        public static string StockEnabled()
        {
            string catID = SQLQuery.ReturnString("SELECT ShortDescription FROM Settings_Project WHERE SettingType ='Stock Link'");
            return catID;
        }

        public static string GetItemGroup(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
            string grpID = SQLQuery.ReturnString("SELECT GroupID FROM ItemSubGroup WHERE CategoryID ='" + subID + "'");
            return grpID;
        }
        public static string GetItemSubGroup(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            string subID = SQLQuery.ReturnString("SELECT CategoryID FROM ItemGrade WHERE GradeID ='" + grdID + "'");
            return subID;
        }
        public static string GetItemGrade(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            string grdID = SQLQuery.ReturnString("SELECT GradeID FROM Categories WHERE CategoryID ='" + catID + "'");
            return grdID;
        }

        public static string GetItemCategory(string productId)
        {
            string catID = SQLQuery.ReturnString("SELECT CategoryID FROM Products WHERE ProductID ='" + productId + "'");
            return catID;
        }

        public static string AvailableNonPrintedQty(string purpose, string itemType, string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where  Purpose='" + purpose + "' AND   ProductID='" + productId + "' AND ItemType='"+itemType+"' AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string AvailableNonPrintedWeight(string purpose, string itemType, string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock where  Purpose='" + purpose + "' AND   ProductID='" + productId + "' AND ItemType='" + itemType + "' AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string LastNonprintedPrice(string purpose, string itemType, string productId)
        {
            string catID = "0";
            try
            {
                 catID = SQLQuery.ReturnString("SELECT Top(1) Price FROM Stock where  Purpose='" + purpose +
                                          "' AND   ProductID='" + productId + "' AND ItemType='" + itemType +
                                          "' AND price>0 order by EntryID desc ");
            }
            catch (Exception xx) { }
            return catID;
        }
        public static string PlasticRawWeight(string purpose, string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock where  Purpose='" + purpose + "' AND   ProductID='" + productId + "' AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string QtyinStock(string purpose, string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where  Purpose='" + purpose + "' AND   ProductID='" + productId + "' AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string LastPlasticRawPrice(string purpose, string productId)
        {
            string catID = "0";
            try
            {
                catID = SQLQuery.ReturnString("SELECT TOP(1) Price FROM Stock where  Purpose='" + purpose + "' AND   ProductID='" + productId + "' AND price>0 order by EntryID desc ");
            }
            catch (Exception xx) { }
            return catID;
        }
        public static string NonUsableFixedAssestsQty(string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM  FixedAssets where  ProductID='" + productId + "' AND  WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string NonUsableQty(string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where  ProductID='" + productId + "' AND  WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string NonUsableWeight(string productId, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock where  ProductID='" + productId + "' AND WarehouseID='" + godownId + "' ");
            return catID;
        }

        public static decimal RawWeightQtyRatio(string purpose, string itemType, string productId, string godownId)
        {
            decimal avQty= Convert.ToDecimal(AvailableNonPrintedQty(purpose, itemType, productId, godownId));
            decimal avWeight= Convert.ToDecimal(AvailableNonPrintedWeight(purpose, itemType, productId, godownId));
            if (avQty>0)
            {
                return (avWeight / avQty);    
            }
            else
            {
                return 0;
            }
        }

        public static string AvailablePrintedQty(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId)
        {
            string catID = SQLQuery.ReturnString(
                        "SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock WHERE Purpose='" + purpose + "' AND  ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string AvailablePrintedWeight(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock WHERE  Purpose='" + purpose + "' AND   ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string PrintedWeight(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock WHERE  Purpose='" + purpose + "' AND   ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string AvailableInkWeight(string productId, string specification, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock where  ProductID='" + productId + "' AND Spec='"+specification+"' AND  WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string LastInkPrice(string productId, string specification)
        {
            string catID = "0";
            try
            {
                catID = SQLQuery.ReturnString("SELECT Top(1) Price FROM Stock where  ProductID='" + productId + "' AND Spec='" + specification + "' AND price>0 order by EntryID desc ");
            }
            catch (Exception xx) { }
            return catID;
        }
        public static string AvailableProcessedQty(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId)
        {
            string catID = SQLQuery.ReturnString(
                        "SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock WHERE Purpose='" + purpose + "' AND  ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' ");
            return catID;
        }
        public static string AvailableProcessedWeight(string purpose, string itemType, string productId, string customer, string brand, string packSize, string color, string godownId)
        {
            string catID = SQLQuery.ReturnString("SELECT ISNULL(sum(InWeight)-Sum(OutWeight),0) FROM Stock WHERE  Purpose='" + purpose + "' AND   ProductID='" +
                        productId + "' AND ItemType='" + itemType + "' AND Customer='" +
                        customer + "' AND  BrandID='" + brand + "' AND SizeId='" +
                        packSize + "'  AND Color='" +
                        color + "'  AND WarehouseID='" + godownId + "' ");
            return catID;
        }

        public static void SaveToStock(string purpose, string InvoiceID, string EntryType, string RefNo, string SizeID, string Customer, string BrandID, string color, string spec, string ProductID, string ProductName, string ItemType, string WarehouseID, string LocationID, string ItemGroup, decimal InQuantity, decimal OutQuantity, decimal unitPrice, decimal InWeight, decimal OutWeight, string ItemSerialNo, string Remark, string Status, string StockLocation, string EntryBy, string EntryDate)
        {
            if (InQuantity > 0 || OutQuantity > 0 || InWeight > 0 || OutWeight > 0)
            {
                //Item entry to stock
                SqlCommand cmd3 =new SqlCommand(
                        "INSERT INTO Stock ( Purpose, InvoiceID, EntryType, RefNo, SizeID, Customer, BrandID, Color, spec, ProductID, ProductName, ItemType, WarehouseID, LocationID, ItemGroup, InQuantity, OutQuantity, Price, InWeight, OutWeight, ItemSerialNo, Remark, Status, StockLocation, EntryBy, EntryDate)" +
                        " VALUES ('" + purpose + "', @InvoiceID, @EntryType, @RefNo, @SizeID, @Customer, @BrandID, @Color, '" + spec +
                        "', @ProductID, @ProductName, @ItemType, @WarehouseID, @LocationID, @ItemGroup, @InQuantity, @OutQuantity, " + unitPrice + ", @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @StockLocation, @EntryBy, '" + EntryDate + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd3.Parameters.AddWithValue("@EntryType", EntryType);
                cmd3.Parameters.AddWithValue("@RefNo", RefNo);
                cmd3.Parameters.AddWithValue("@SizeID", SizeID);
                cmd3.Parameters.AddWithValue("@Customer", Customer);
                cmd3.Parameters.AddWithValue("@BrandID", BrandID);
                cmd3.Parameters.AddWithValue("@Color", color);
                cmd3.Parameters.AddWithValue("@ProductID", ProductID);

                cmd3.Parameters.AddWithValue("@ProductName", ProductName);
                cmd3.Parameters.AddWithValue("@ItemType", ItemType);
                cmd3.Parameters.AddWithValue("@WarehouseID", WarehouseID);
                cmd3.Parameters.AddWithValue("@LocationID", LocationID);
                cmd3.Parameters.AddWithValue("@ItemGroup", ItemGroup);

                cmd3.Parameters.AddWithValue("@InQuantity", Convert.ToDecimal(InQuantity));
                cmd3.Parameters.AddWithValue("@OutQuantity", Convert.ToDecimal(OutQuantity));
                cmd3.Parameters.AddWithValue("@InWeight", Convert.ToDecimal(InWeight));
                cmd3.Parameters.AddWithValue("@OutWeight", Convert.ToDecimal(OutWeight));

                cmd3.Parameters.AddWithValue("@ItemSerialNo", ItemSerialNo);
                cmd3.Parameters.AddWithValue("@Remark", Remark);
                cmd3.Parameters.AddWithValue("@Status", Status);
                cmd3.Parameters.AddWithValue("@StockLocation", StockLocation);
                cmd3.Parameters.AddWithValue("@EntryBy", EntryBy);

                cmd3.Connection.Open();
                cmd3.ExecuteNonQuery();
                cmd3.Connection.Close();
            }


        }

        public static void SaveToStock(string purpose, string InvoiceID, string EntryType, string RefNo, string SizeID, string Customer, string BrandID, string color, string spec, string ProductID, string ProductName, string ItemType, string WarehouseID, string LocationID, string ItemGroup, decimal InQuantity, decimal OutQuantity, decimal unitPrice, decimal InWeight, decimal OutWeight, string ItemSerialNo, string Remark, string Status, string StockLocation, string EntryBy, string EntryDate, string Description)
        {
            if (InQuantity > 0 || OutQuantity > 0 || InWeight > 0 || OutWeight > 0)
            {
                //Item entry to stock
                SqlCommand cmd3 = new SqlCommand(
                        "INSERT INTO Stock ( Purpose, InvoiceID, EntryType, RefNo, SizeID, Customer, BrandID, Color, spec, ProductID, ProductName, ItemType, WarehouseID, LocationID, ItemGroup, InQuantity, OutQuantity, Price, InWeight, OutWeight, ItemSerialNo, Remark, Status, StockLocation, EntryBy, EntryDate, Description)" +
                        " VALUES ('" + purpose + "', @InvoiceID, @EntryType, @RefNo, @SizeID, @Customer, @BrandID, @Color, '" + spec +
                        "', @ProductID, @ProductName, @ItemType, @WarehouseID, @LocationID, @ItemGroup, @InQuantity, @OutQuantity, " + unitPrice + ", @InWeight, @OutWeight, @ItemSerialNo, @Remark, @Status, @StockLocation, @EntryBy, '" + EntryDate + "', '" + Description + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

                cmd3.Parameters.AddWithValue("@InvoiceID", InvoiceID);
                cmd3.Parameters.AddWithValue("@EntryType", EntryType);
                cmd3.Parameters.AddWithValue("@RefNo", RefNo);
                cmd3.Parameters.AddWithValue("@SizeID", SizeID);
                cmd3.Parameters.AddWithValue("@Customer", Customer);
                cmd3.Parameters.AddWithValue("@BrandID", BrandID);
                cmd3.Parameters.AddWithValue("@Color", color);
                cmd3.Parameters.AddWithValue("@ProductID", ProductID);

                cmd3.Parameters.AddWithValue("@ProductName", ProductName);
                cmd3.Parameters.AddWithValue("@ItemType", ItemType);
                cmd3.Parameters.AddWithValue("@WarehouseID", WarehouseID);
                cmd3.Parameters.AddWithValue("@LocationID", LocationID);
                cmd3.Parameters.AddWithValue("@ItemGroup", ItemGroup);

                cmd3.Parameters.AddWithValue("@InQuantity", Convert.ToDecimal(InQuantity));
                cmd3.Parameters.AddWithValue("@OutQuantity", Convert.ToDecimal(OutQuantity));
                cmd3.Parameters.AddWithValue("@InWeight", Convert.ToDecimal(InWeight));
                cmd3.Parameters.AddWithValue("@OutWeight", Convert.ToDecimal(OutWeight));

                cmd3.Parameters.AddWithValue("@ItemSerialNo", ItemSerialNo);
                cmd3.Parameters.AddWithValue("@Remark", Remark);
                cmd3.Parameters.AddWithValue("@Status", Status);
                cmd3.Parameters.AddWithValue("@StockLocation", StockLocation);
                cmd3.Parameters.AddWithValue("@EntryBy", EntryBy);

                cmd3.Connection.Open();
                cmd3.ExecuteNonQuery();
                cmd3.Connection.Close();
            }


        }
    }
}