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
    public partial class RVReport : System.Web.UI.Page
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
            string rvId = Convert.ToString(Request.QueryString["RvId"]);
            if (rvId != "")
            {
                SqlCommand cmd7 = new SqlCommand(@"SELECT IDRvNo, RvInvoiceNo, DateOfRV, LocationID, LocationName, ProductReturnDivision, Store, StoreName, ReturnType, LvSIRVoucherNo, ProductReturnCause, FinYear, Comments, DocumentUrl, SaveMode, SubmitDate, 
                         WorkflowStatus, WorkflowApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, PreparedBy, Receivedby, ReceivedDate, ApprovedBy, ApprovedDate, LedgerWriterStoreBy, LedgerWriterStoreDate, ValueDeterminedby, 
                         ValueDeterminedDate, StoreKeeperBy, StoreKeeperDate, EntryBy, EntryDate, NamePreparedBy, DesigPreparedBy, SigPreparedBy, NameApprovedBy, DesigApprovedBy, SigApprovedBy, NameReceivedBy, DesigReceivedBy, 
                         SigReceivedBy, NameLWStoreBy, DesigLWStoreBy, SigLWStoreBy, NameVDeterminBy, DesigVDeterminBy, SigVDeterminBy, NameSKeeperBy, DesigSKeeperBy, SigSKeeperBy
                         FROM VwRV WHERE IDRvNo='" + rvId + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr7 = cmd7.ExecuteReader();
                XerpDataSet ds = new XerpDataSet();
                ds.Load(dr7, LoadOption.OverwriteChanges, ds.VwRV);
                cmd7.Connection.Close();

                SqlCommand cmd = new SqlCommand(@"SELECT RVProductID, RVNo, CategoryID, CategoryName, SubCategoryID, SubCategoryName, ProductID, ProductName, ProductStatus, ReturnQTY, ReturnQtyInWords, ApprovedQty, ApprovedQtyInWords, TotalPrice, 
                         ProductReceive, IssueDescription, UnitPrice, DepositalAccount, EntryBy, ReturnDate, PartNo, SerialNo, ModelNo
                         FROM VwRVProduct WHERE (RVNo='" + rvId + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ds.Load(dr, LoadOption.OverwriteChanges, ds.VwRVProduct);
                cmd7.Connection.Close();

                rpt.Load(Server.MapPath("CrptRVReport.rpt"));

                //string datefield = "As on :" + Convert.ToDateTime(dateFrom).ToString(" dd/MM/yyyy");
                //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");

                rpt.SetDataSource(ds);
                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM GRNFrom WHERE (IDGrnNO='" + rvId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "CrptRVReport.rpt");
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