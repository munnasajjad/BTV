using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oxford.XerpReports
{
    public partial class LVReport : System.Web.UI.Page
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
            string lvId = Convert.ToString(Request.QueryString["LVId"]);
            if (lvId != "")
            {
                /*DataTable dt1 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT IDLvNo, LvInvoiceNo, DateofLv, ResponsiblePerson, CauseOfLoan, Station, StationName, LoanFromCenter, CenterName, LoanFromStore, LoanFromStoreName, SaveToStore, Remarks, Verifier, RequisitionBy, 
                         DeliveredBy, EntryBy, EntryDate, RequestStatus, ApprovedStatus, ApprovedDate, DeliveredStatus FROM VwLV Where IDLvNo='" + lvId + "'");*/
                  DataTable dt1 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT IDLvNo, LvInvoiceNo, DateofLv, Division, LoanType, LoanToEmployee, ResponsiblePerson, CauseOfLoan, LocationID, Store, FinYear, DocumentUrl, Remarks, Verifier, RequisitionBy, DeliveredBy, PreparedBy, 
                         PreparedDate, EntryBy, EntryDate, SaveMode, Issuedby, IssuedDate, Approvedby, ApprovedbyDate, ReceivedBy, ReceivedByDate, ProductReturnReceiverStore, ProductReturnReceiverStoreDate, SubmitDate, WorkflowStatus, 
                         WorkflowApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, SigForPreparedBy, NameOfPreparedBy, DesigOfPreparedBy, EmpForAppBy, DesigForAppBy, SigForApprovedby, EmpForIssuBy, DesigForIssuBy, 
                         SigForIssuedby, EmpReturnBy, DesigReturnBy, SigForProductReturnReceiverStore, EmpForReceivedBy, DesigReceivedBy, SigForReceivedBy FROM VwLV Where IDLvNo='" + lvId + "'");

              
                DataTableReader dr1 = dt1.CreateDataReader();
                XerpDataSet ds = new XerpDataSet();
                ds.Load(dr1, LoadOption.OverwriteChanges, ds.VwLV);

                DataTable dt2 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT IDLVNo, CategoryID, Category, SubCategoryID, SubCategory, ProductID, ProductName, SerialNo, ModelNo, QTYNeed, NeedQtyInWords, QTYDelivered, DeliveredQtyInWords, ApprovedQty, DeliveredDate, ReturnQTY, 
                ProductCondition, EntryBy, EntryDate, LVProductID FROM VwLvProduct WHERE IDLVNo='" + lvId+"'");
                DataTableReader dr2 = dt2.CreateDataReader();
                ds.Load(dr2, LoadOption.OverwriteChanges, ds.VwLvProduct);

                rpt.Load(Server.MapPath("CrptLVReport.rpt"));

                //string datefield = "As on :" + Convert.ToDateTime(dateFrom).ToString(" dd/MM/yyyy");
                //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");

                rpt.SetDataSource(ds);
                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                //rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM GRNFrom WHERE (IDGrnNO='" + rvId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "CrptLVReport.rpt");
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