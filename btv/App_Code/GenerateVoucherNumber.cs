using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using DocumentFormat.OpenXml.Bibliography;
using RunQuery;
/// <summary>
/// Summary description for GenerateGRNNumber
/// </summary>
public class GenerateVoucherNumber
{
    public GenerateVoucherNumber()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string GetFinYear(DateTime date)
    {
        if (date.Month > 6)//Running year
        {
            //startYear = date.Year;
            return date.ToString("yyyy") + "-" + date.AddYears(1).ToString("yy");
        }
        else//Prev financial year
        {
            //startYear = date.AddYears(-1).Year;
            return date.AddYears(-1).ToString("yyyy") + "-" + date.ToString("yy");
        }
    }
    private static string Make4Digit(string maxValue)
    {
        if (maxValue.Length < 2)
        {
            maxValue = "000" + maxValue;
        }
        else if (maxValue.Length < 3)
        {
            maxValue = "00" + maxValue;
        }
        else if (maxValue.Length < 4)
        {
            maxValue = "0" + maxValue;
        }
        return maxValue;
    }
    public static string GetOldGrnNumber(DateTime date, string uName,string storeId)
    {
        //string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(GRNInvoiceNo),0)+1 from GRNFrom where StoreID='"+storeId+"' AND Type='Old'");
        mid = "GRN-"+ Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select GRNInvoiceNo from GRNFrom WHERE GRNInvoiceNo='" + mid + "' AND StoreID='" + storeId + "' AND Type='Old'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(GRNInvoiceNo),0)+1+" + i + " from GRNFrom WHERE  StoreID='" + storeId + "' AND Type='Old'");
            mid = "GRN-"+Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select GRNInvoiceNo from GRNFrom WHERE GRNInvoiceNo='" + mid + "' AND StoreID='" + storeId + "' AND Type='Old'");
        }
        return mid;
    }
    public static string GetGrnNumber(DateTime date, string uName,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(GRNInvoiceNo),0)+1 from GRNFrom where  FinYear='" + fYear + "' AND StoreID='"+storeId+ "' AND Type='New'");
        mid = "GRN-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select GRNInvoiceNo from GRNFrom WHERE GRNInvoiceNo='" + mid + "' AND FinYear='" + fYear + "' AND StoreID='" + storeId + "' AND Type='New'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(GRNInvoiceNo),0)+1+" + i + " from GRNFrom WHERE FinYear='" + fYear + "' AND StoreID='" + storeId + "' AND Type='New'");
            mid = "GRN-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select GRNInvoiceNo from GRNFrom WHERE GRNInvoiceNo='" + mid + "'  AND FinYear='" + fYear + "' AND StoreID='" + storeId + "' AND Type='New'");
        }
        return mid;
    }
    public static string GetLvNumber(DateTime date, string uname,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uname);
        
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(LvInvoiceNo),0)+1 from LoanVouchar where  FinYear='" + fYear + "' AND Store='" + storeId + "'");
        mid= "LV-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select LvInvoiceNo from LoanVouchar WHERE LvInvoiceNo='" + mid + "'  AND FinYear='" + fYear + "' AND Store='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(LvInvoiceNo),0)+1+" + i + " from LoanVouchar WHERE FinYear='" + fYear + "' AND Store='" + storeId + "'");
            mid = "LV-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select LvInvoiceNo from LoanVouchar WHERE LvInvoiceNo='" + mid + "' AND  FinYear='" + fYear + "' AND Store='" + storeId + "'");
        }

        return mid;

    }
    public static string GetTvNumber(DateTime date, string uName,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(TransferVoucherNo),0)+1 from TransferVoucher where FinYear='" + fYear + "'AND FormStoreID='" + storeId + "'");
        mid = "TV-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select TransferVoucherNo from TransferVoucher WHERE TransferVoucherNo='" + mid + "' AND  FinYear='" + fYear + "' AND FormStoreID='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(TransferVoucherNo),0)+1+" + i + " from TransferVoucher WHERE FinYear='" + fYear + "' AND FormStoreID='" + storeId + "'");
            mid = "TV-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select TransferVoucherNo from TransferVoucher WHERE TransferVoucherNo='" + mid + "' AND FinYear='" + fYear + "' AND FormStoreID='" + storeId + "'");
        }

        return mid;

    }
    public static string GetSirNumber(DateTime date, string uname,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uname);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(SirVoucherNo),0)+1 from SIRFrom where FinYear='" + fYear + "' AND Store='" + storeId + "'");
        mid = "SIR-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select SirVoucherNo from SIRFrom WHERE SirVoucherNo='" + mid + "' AND FinYear='" + fYear + "' AND Store='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(SirVoucherNo),0)+1+" + i + " from SIRFrom WHERE  AND FinYear='" + fYear + "' AND Store='" + storeId + "'");
            mid = "SIR-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select SirVoucherNo from SIRFrom WHERE SirVoucherNo='" + mid + "' AND FinYear='" + fYear + "' AND Store='" + storeId + "'");
        }

        return mid;

    }
    public static string GetRvNumber(DateTime date, string uName,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(RvInvoiceNo),0)+1 from ReturnVauchar where LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND Store='"+storeId+"'");
        mid = "RV-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select RvInvoiceNo from ReturnVauchar WHERE RvInvoiceNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND Store='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(RvInvoiceNo),0)+1+" + i + " from ReturnVauchar WHERE LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND Store='" + storeId + "'");
            mid = "RV-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select RvInvoiceNo from ReturnVauchar WHERE RvInvoiceNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND Store='" + storeId + "'");
        }
        return mid;
    }
    public static string GetRepairNumber(DateTime date, string uName,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(RepairNo),0)+1 from ProductRepair where LocationId='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='"+storeId+"'");
        mid = "RP-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select RepairNo from ProductRepair WHERE RepairNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(RepairNo),0)+1+" + i + " from ProductRepair WHERE LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
            mid = "RP-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select RepairNo from ProductRepair WHERE RepairNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        }
        return mid;
    }
    public static string GetReceiveNumber(DateTime date, string uName,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(ReceiveNo),0)+1 from ProductReceived where LocationId='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        mid = "PR-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select ReceiveNo from ProductReceived WHERE ReceiveNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(ReceiveNo),0)+1+" + i + " from ProductReceived WHERE LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
            mid = "PR-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select ReceiveNo from ProductReceived WHERE ReceiveNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        }

        return mid;

    }
    public static string GetAuctionNumber(DateTime date, string uName,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(AuctionNo),0)+1 from AuctionList where LocationId='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        mid = "A-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select AuctionNo from AuctionList WHERE AuctionNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(AuctionNo),0)+1+" + i + " from AuctionList WHERE LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
            mid = "A-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select AuctionNo from AuctionList WHERE AuctionNo='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        }
        return mid;
    }
    public static string GetDeadNumber(DateTime date, string uName,string storeId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("Select ISNULL(COUNT(DeadNumber),0)+1 from DeadProductList where LocationId='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        mid = "D-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("Select DeadNumber from DeadProductList WHERE DeadNumber='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("Select ISNULL(COUNT(DeadNumber),0)+1+" + i + " from DeadProductList WHERE LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
            mid = "D-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("Select DeadNumber from DeadProductList WHERE DeadNumber='" + mid + "' AND LocationID='" + locationId + "' AND FinYear='" + fYear + "' AND StoreId='" + storeId + "'");
        }
        return mid;
    }
}