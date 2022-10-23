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
    public partial class TVReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGridData();
        }

        ReportDocument rpt = new ReportDocument();
        private void LoadGridData()
        {
            string lName = Page.User.Identity.Name.ToString();
            string prjId = "1";
            string rvId = Convert.ToString(Request.QueryString["TVID"]);
            if (rvId != "")
            {
                /*SqlCommand cmd7 = new SqlCommand(@"SELECT TvID, TransferVoucherNo, Date, FormStoreID, FromStore, RequsitionBy, ToStore, Url, LocationID, CenterID, DepartmentSectionID, ToStoreID, MainOfficeID, FinYear, Requirment, DocumentUrl, SaveMode, 
                         WorkflowStatus, ReturnOrHoldUserID, WorkflowApprovedDate, SubmitDate, CurrentWorkflowUser, Remarks, PreparedBy, PreparedDate, IssuedBy, IssuedByDate, ReceivedBy, ReceivedByDate, ApprovedBy, ApprovedByDate, 
                         EntryDate, EntryBy, NamePreperBy, DesigPreperBy, SigPreperBy, NameIssuedBy, DesigIssuedBy, SigIssuedBy, NameAppBy, DesigAppBy, SigAppBy, NameRcvBy, DesigRcvBy, SigRcvBy
                         FROM VwTV WHERE (TvID='" + rvId + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr7 = cmd7.ExecuteReader();
                XerpDataSet ds = new XerpDataSet();
                ds.Load(dr7, LoadOption.OverwriteChanges, ds.VwTV);
                cmd7.Connection.Close();*/

                DataTable dt1 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TvID, TransferVoucherNo, Date, FormStoreID, FromStore, RequsitionBy, ToStore, Url, LocationID, CenterID, DepartmentSectionID, ToStoreID, MainOfficeID, FinYear, Requirment, DocumentUrl, SaveMode, 
                         WorkflowStatus, ReturnOrHoldUserID, WorkflowApprovedDate, SubmitDate, CurrentWorkflowUser, Remarks, PreparedBy, PreparedDate, IssuedBy, IssuedByDate, ReceivedBy, ReceivedByDate, ApprovedBy, ApprovedByDate, 
                         EntryDate, EntryBy, NamePreperBy, DesigPreperBy, SigPreperBy, NameIssuedBy, DesigIssuedBy, SigIssuedBy, NameAppBy, DesigAppBy, SigAppBy, NameRcvBy, DesigRcvBy, SigRcvBy
                         FROM VwTV WHERE (TvID='" + rvId + "')");

                DataTableReader dr1 = dt1.CreateDataReader();
                XerpDataSet ds = new XerpDataSet();
                ds.Load(dr1, LoadOption.OverwriteChanges, ds.VwTV);

                /*SqlCommand cmd = new SqlCommand(@"SELECT TvProductID, TvVoucherID, ProductID, ProductName, TvQty, Unit, ProductCategory, ProductSubCategory
                FROM VwTVProduct WHERE (TvVoucherID='" + rvId + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ds.Load(dr, LoadOption.OverwriteChanges, ds.VwTVProduct);
                cmd7.Connection.Close();*/

                    DataTable dt2 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TvProductID, TvVoucherID, TVoucherNo, CategoryID, SubCategoryID, ProductID, ProductName, ProductCategory, ProductSubCategory, Unit, PartNo, SerialNo, ModelNo, TvQty, TvQtyInWords, EntryBy, EntryDate
                    FROM VwTVProduct WHERE (TvVoucherID='" + rvId + "')");
                DataTableReader dr2 = dt2.CreateDataReader();
                ds.Load(dr2, LoadOption.OverwriteChanges, ds.VwTVProduct);

                rpt.Load(Server.MapPath("CrptTVReport.rpt"));

                //string datefield = "As on :" + Convert.ToDateTime(dateFrom).ToString(" dd/MM/yyyy");
                //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");

                rpt.SetDataSource(ds);
                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM TransferVoucher WHERE (TvID='" + rvId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "CrptTVReport.rpt");
            }
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