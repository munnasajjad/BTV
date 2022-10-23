using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class ChillerReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name.ToString();

            //bool isPdf = Convert.ToBoolean(Request.QueryString["IsPdf"]);
            //bool isExcel = Convert.ToBoolean(Request.QueryString["IsExcel"]);
            //bool isWord = Convert.ToBoolean(Request.QueryString["IsWord"]);


            string dateFrom = Convert.ToString(Request.QueryString["DateForm"]);
            string dateTo = Convert.ToString(Request.QueryString["DateTo"]);

            string mainOfficeId = Convert.ToString(Request.QueryString["MainOfficeId"]);
            string strQuery = String.Empty;
            if (mainOfficeId != "0")
            {
                strQuery = " AND (MainOfficeId='" + mainOfficeId + "')";
            }


            SqlCommand cmd = new SqlCommand(@"SELECT MainOfficeId, Date, CLVoucher, ReadingTaken, Time, ChillerMode, ActiveChilledWaterSetpoint, AverageLineCurrent, ActiveCurrentLimitSetpoint, EvapEnteringWaterTemperature, EvapLeavingWaterTemperature, 
                         EvapSatRfgtTemp, EvapApproachTemp, EvapWaterFlowSwitchStatus, ExpansionValvePosition, ExpansionValvePositionSteps, EvapRfgtLiquidlevel, CondEnteringWaterTemp, CondLeavingWaterTemp, CondSatRfgtTemp, 
                         CondRftgPressure, CondApproachTemp, CondWaterFlowSwtichSatatus, CompressorStarts, CompressorRuntime, SystemRfgtDiffPressure, OilPressure, CompressorRfgtDischargeTemp, RLA, Amps, VoltsABBCCA, ShiftIncharge, 
                         ShiftInChargeName, Remarks FROM VwChiller WHERE (Date >= '" + Convert.ToDateTime(dateFrom).ToString("yyyy-MM-dd") + "') AND (Date <= '" + Convert.ToDateTime(dateTo).ToString("yyyy-MM-dd") + "') " + strQuery + " ORDER BY Date, MainOfficeId", new SqlConnection(ConfigurationManager.ConnectionStrings["Connection_String"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader dr7 = cmd.ExecuteReader();
            XerpDataSet ds = new XerpDataSet();
            ds.Load(dr7, LoadOption.OverwriteChanges, ds.VwChiller);
            cmd.Connection.Close();

            rpt.Load(Server.MapPath("CrptChiller.rpt"));

            string datefield = "From " + Convert.ToDateTime(dateFrom).ToString("dd/MM/yyyy") + " to " + Convert.ToDateTime(dateTo).ToString("dd/MM/yyyy");
            rpt.SetDataSource(ds);
            string mainOfficeName = "";
            if (mainOfficeId == "0")
            {
                mainOfficeName = "All Stations";
            }
            else
            {
                mainOfficeName = SQLQuery.ReturnString("SELECT Name FROM Location WHERE LocationID = '" + mainOfficeId + "'");
            }

            //SQLQuery.LoadrptHeader(ds, rpt);
            rpt.SetParameterValue("@date", datefield);
            rpt.SetParameterValue("@mainOfficeName", mainOfficeName);
            //CrystalReportViewer1.ReportSource = rpt;
            rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "ChillerReport");


            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();


        }
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