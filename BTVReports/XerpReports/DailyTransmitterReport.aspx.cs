using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class DailyTransmitterReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadGridData();
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }

        }
        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name.ToString();
            string prjID = "1";

            string type = Convert.ToString(Request.QueryString["type"]);
            string transmitterMachineId = Convert.ToString(Request.QueryString["transmitterMachineId"]);
            string transmitterName =
                SQLQuery.ReturnString(@"SELECT TransmitterName FROM Transmitters WHERE id='" + transmitterMachineId +
                                      "'");
            string stationId = Convert.ToString(Request.QueryString["stationId"]);
            string stationName =
                SQLQuery.ReturnString(@"SELECT Name FROM Location WHERE LocationID = '" + stationId + "'");
            string dateFrom = Convert.ToString(Request.QueryString["dateFrom"]);
            string dateTo = Convert.ToString(Request.QueryString["dateTo"]);

            string fromMonth = Convert.ToDateTime(dateFrom).ToString("MMMM");
            string toMonth = Convert.ToDateTime(dateTo).ToString("MMMM");
            string fromYear = Convert.ToDateTime(dateFrom).Year.ToString();
            string toYear = Convert.ToDateTime(dateTo).Year.ToString();
            //DataTable dt1 = BindItemGrid(dateFrom, dateTo, type);
            //DataTableReader dr2 = dt1.CreateDataReader();
            XerpDataSet dsx = new XerpDataSet();
            //dsx.Load(dr2, LoadOption.OverwriteChanges, dsx.Vw_DailyTransmitterLog);

            if (type == "thales")
            {
                //Sub report
                DataTable dataTable = SQLQuery.ReturnDataTable(@"SELECT Id, StationId, TransmitterMachineId, ChannelNumber, Date, TransmitterOutputPowerVideo, TransmitterOutputPowerAudio, ReflectedPowerVideo, ReflectedPowerAudio, ExciterInOperation, PumpInOperation, 
                         LiouidStaticPressure, PumpOutputPressure, LiquidTemperature, DehydratorLinePressure, PA1AGCLevel, PA1VSWR, PA1Temperature, PA1T1, PA1T2, PA1T3, PA1T4, PA1T5, PA1T6, PA2AGCLevel, PA2VSWR, PA2Temperature, 
                         PA2T1, PA2T2, PA2T3, PA2T4, PA2T5, PA2T6, PA3AGCLevel, PA3VSWR, PA3Temperature, PA3T1, PA3T2, PA3T3, PA3T4, PA3T5, PA3T6, PA4AGCLevel, PA4VSWR, PA4Temperature, PA4T1, PA4T2, PA4T3, PA4T4, PA4T5, PA4T6, 
                         PA5AGCLevel, PA5VSWR, PA5Temperature, PA5T1, PA5T2, PA5T3, PA5T4, PA5T5, PA5T6, PA6AGCLevel, PA6VSWR, PA6Temperature, PA6T1, PA6T2, PA6T3, PA6T4, PA6T5, PA6T6, PA7AGCLevel, PA7VSWR, PA7Temperature, 
                         PA7T1, PA7T2, PA7T3, PA7T4, PA7T5, PA7T6, PA8AGCLevel, PA8VSWR, PA8Temperature, PA8T1, PA8T2, PA8T3, PA8T4, PA8T5, PA8T6, Remarks, EntryBy, EntryDate, TransmitterName, Name
FROM            Vw_DailyTransmitterLog WHERE TransmitterMachineId='" + transmitterMachineId + "' AND StationId = '" + stationId + "' AND (Date >='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ) AND (Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "') ORDER BY Date ASC");
                DataTableReader dataReader = dataTable.CreateDataReader();
                //XerpDataSet dsxx = new XerpDataSet();
                dsx.Load(dataReader, LoadOption.OverwriteChanges, dsx.Vw_DailyTransmitterLog);

                rpt.Load(Server.MapPath("CrptDailyTranmitterReportThales.rpt"));
            }

            string datefield = "";
            if (fromMonth.Equals(toMonth) && fromYear.Equals(toYear))
            {
                datefield = fromMonth + " '" + fromYear;
            }
            else
            {
                datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            }
            //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT        sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");
            rpt.SetDataSource(dsx);
            //SQLQuery.LoadrptHeader(dsx, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@transmitterName", transmitterName);
            rpt.SetParameterValue("@stationName", stationName);
            
            rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "DailyTransmitterLogReport");
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();

        }
        private DataTable BindItemGrid(string dateFrom, string dateTo, string type)
        {
            DataSet ds = new DataSet();
            try
            {
                if (dateFrom != "")
                {
                    dateFrom = " AND PrdPlasticCont.Date>='" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "' ";
                }

                if (dateTo != "")
                {
                    dateTo = " AND PrdPlasticCont.Date<='" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "' ";
                }

                string query = "   where PrdPlasticCont.ActProduction>0 " + dateFrom + dateTo;
                string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

                if (type == "summery")
                {
                    query = @"SELECT        (SELECT        BrandName
                          FROM            Brands
                          WHERE        (BrandID = PrdPlasticCont.PackSize)) + ' ' +
                             (SELECT        ItemName
                               FROM            Products
                               WHERE        (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                             (SELECT        BrandName
                               FROM            CustomerBrands
                               WHERE        (BrandID = PrdPlasticCont.Brand)) AS ProductName,
                             (SELECT        DepartmentName
                               FROM            Colors
                               WHERE        (Departmentid = PrdPlasticCont.Color)) AS Color, SUM(PrdPlasticCont.FinalProduction) AS Qty, SUM(PrdPlasticCont.FinalKg) AS Weight, AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) 
                         AS CycleTime, SUM(PrdPlasticCont.WorkingHour + PrdPlasticCont.WorkingMin / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, SUM(PrdPlasticCont.FinalProduction) AS Actual, 
                         SUM(PrdPlasticCont.Rejection) AS wastage, SUM(PrdPlasticCont.FinalProduction) AS FinalPrdn, SUM(PrdPlasticCont.FinalKg) AS FinalKg, SUM(PrdPlasticCont.WasteWeight) AS wasteWt, 
                         Machines.MachineNo AS Machine, Machines.Description AS Brand
FROM            PrdPlasticCont INNER JOIN
                         Machines ON PrdPlasticCont.MachineNo = Machines.mid " + query + @"
GROUP BY PrdPlasticCont.MachineNo, PrdPlasticCont.Brand, PrdPlasticCont.PackSize, PrdPlasticCont.ItemID, PrdPlasticCont.Color, Machines.MachineNo, Machines.Description 
ORDER BY Machine  ";

                }
                else
                {
                    query = @"SELECT        (SELECT        Company
                          FROM            Party
                          WHERE        (PartyID = PrdPlasticCont.CustomerID)) AS Customer,
                             (SELECT        BrandName
                               FROM            Brands
                               WHERE        (BrandID = PrdPlasticCont.PackSize)) + ' ' +
                             (SELECT        ItemName
                               FROM            Products
                               WHERE        (ProductID = PrdPlasticCont.ItemID)) + ' ' +
                             (SELECT        BrandName
                               FROM            CustomerBrands
                               WHERE        (BrandID = PrdPlasticCont.Brand)) AS ProductName,
                             (SELECT        'Shift - ' + DepartmentName AS Expr1
                               FROM            Shifts
                               WHERE        (Departmentid = PrdPlasticCont.Shift)) AS Shifts,
                             (SELECT        DepartmentName
                               FROM            Colors
                               WHERE        (Departmentid = PrdPlasticCont.Color)) AS Color, SUM(PrdPlasticCont.FinalProduction) AS Qty, SUM(PrdPlasticCont.FinalKg) AS Weight, AVG(CONVERT(decimal, PrdPlasticCont.CycleTime)) 
                         AS CycleTime, SUM(PrdPlasticCont.WorkingHour + PrdPlasticCont.WorkingMin / 60) AS Running, SUM(PrdPlasticCont.CalcProduction) AS Projected, SUM(PrdPlasticCont.FinalProduction) AS Actual, 
                         SUM(PrdPlasticCont.Rejection) AS wastage, SUM(PrdPlasticCont.Rejection) * 100 / SUM(PrdPlasticCont.ActProduction) AS wastePercent, Machines_1.MachineNo AS Machine, Machines_1.Description AS Brand
    FROM PrdPlasticCont INNER JOIN
                             Machines AS Machines_1 ON PrdPlasticCont.MachineNo = Machines_1.mid  " + query + @"
    GROUP BY Machines_1.MachineNo, PrdPlasticCont.MachineNo, PrdPlasticCont.CustomerID, PrdPlasticCont.Brand, PrdPlasticCont.PackSize, PrdPlasticCont.ItemID, PrdPlasticCont.Shift, PrdPlasticCont.Color, Machines_1.Description
    ORDER BY Machines_1.MachineNo, PrdPlasticCont.Shift";

                }

                ds = SQLQuery.ReturnDataSet(query);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        ReportDocument rpt = new ReportDocument();


        protected void CrystalReportViewer1_OnUnload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected void Page_Unload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
        protected override void OnUnload(EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
    }
}