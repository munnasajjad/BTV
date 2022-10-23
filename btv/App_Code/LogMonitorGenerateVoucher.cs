using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RunQuery;

/// <summary>
/// Summary description for LogMonitorGenerateVoucher
/// </summary>
public class LogMonitorGenerateVoucher
{
    public LogMonitorGenerateVoucher()
    {
        //
        // TODO: Add constructor logic here
        //
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
    public static string GetESVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(ESVoucher),0)+1 from EarthStation where MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "ES-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT ESVoucher from EarthStation WHERE ESVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(ESVoucher),0)+1+" + i + " from EarthStation WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "ES-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT ESVoucher from EarthStation WHERE ESVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetMRVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(MRVoucher),0)+1 from MeterReading where MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "MR-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT MRVoucher from MeterReading WHERE MRVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(MRVoucher),0)+1+" + i + " from MeterReading WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "MR-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT MRVoucher from MeterReading WHERE MRVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }

    public static string GetDMVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(DMVoucher),0)+1 from Dehumidifier WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "DM-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT DMVoucher from Dehumidifier WHERE DMVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(DMVoucher),0)+1+" + i + " from Dehumidifier WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "DM-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT DMVoucher from Dehumidifier WHERE DMVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetMNVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(MNVoucher),0)+1 from Maintenance WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "MN-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT MNVoucher from Maintenance WHERE MNVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(MNVoucher),0)+1+" + i + " from Maintenance WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "MN-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT MNVoucher from Maintenance WHERE MNVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetLBVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(LBVoucher),0)+1 from LiveBroadcasting WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "LB-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT LBVoucher from LiveBroadcasting WHERE LBVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(LBVoucher),0)+1+" + i + " from LiveBroadcasting WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "LB-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT LBVoucher from LiveBroadcasting WHERE LBVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetUPSVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(UPSVoucher),0)+1 from UPS WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "UPS-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT UPSVoucher from UPS WHERE UPSVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(UPSVoucher),0)+1+" + i + " from UPS WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "UPS-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT UPSVoucher from UPS WHERE UPSVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetACFVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(ACFVoucher),0)+1 from AirConditionFCU WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "ACF-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT ACFVoucher from AirConditionFCU WHERE ACFVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(ACFVoucher),0)+1+" + i + " from AirConditionFCU WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "ACF-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT ACFVoucher from AirConditionFCU WHERE ACFVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetCLVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(CLVoucher),0)+1 from Chiller WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "CL-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT CLVoucher from Chiller WHERE CLVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(CLVoucher),0)+1+" + i + " from Chiller WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "CL-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT CLVoucher from Chiller WHERE CLVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetSSVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(SSVoucher),0)+1 from SubStation WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "SS-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT SSVoucher from SubStation WHERE SSVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(SSVoucher),0)+1+" + i + " from SubStation WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "SS-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT SSVoucher from SubStation WHERE SSVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetAHVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(AHVoucher),0)+1 from AirHandlingUnit WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "AH-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT AHVoucher from AirHandlingUnit WHERE AHVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(AHVoucher),0)+1+" + i + " from AirHandlingUnit WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "AH-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT AHVoucher from AirHandlingUnit WHERE AHVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
    public static string GetDMNVoucherNumber(DateTime date, string uName, string departmentId)
    {
        string fYear = GetFinYear(date);
        string locationId = SQLQuery.GetLocationID(uName);
        string mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(DMNVoucher),0)+1 from DailyMaintenance WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        mid = "DMN-" + fYear + "-" + Make4Digit(mid);
        string isExist = SQLQuery.ReturnString("SELECT DMNVoucher from DailyMaintenance WHERE DMNVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        int i = 0;
        while (isExist != "")
        {
            i++;
            mid = SQLQuery.ReturnString("SELECT ISNULL(COUNT(DMNVoucher),0)+1+" + i + " from DailyMaintenance WHERE MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
            mid = "DMN-" + fYear + "-" + Make4Digit(mid);
            isExist = SQLQuery.ReturnString("SELECT DMNVoucher from DailyMaintenance WHERE DMNVoucher='" + mid + "' AND MainOfficeId='" + locationId + "' AND FinYear='" + fYear + "' AND DepartmentId='" + departmentId + "'");
        }
        return mid;
    }
}