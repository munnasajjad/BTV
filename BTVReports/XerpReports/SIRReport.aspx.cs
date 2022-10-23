using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Oxford.XerpReports
{
    public partial class SIRReport : System.Web.UI.Page
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
            string SirId = Convert.ToString(Request.QueryString["SIRId"]);
            if (SirId != "")
            {
                DataTable dt1 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT   IDSirNo, DateOfSir, SirVoucherNo, LocationID, FinYear, LoanToEmployee, Store, GivenDivision, GivenDivisionDate, ProductUseAim, HeadOfCost, Remarks, DocumentUrl, PreparedBy, Issuedby, IssuedDate, 
                         Requisitionby, RequisitionDate, Documentedby, DocumentedDate, HeadSectionby, HeadSectionDate, Approvedby, ApprovedDate, ValueDeterminedby, ValueDeterminedDate, StoreKeeperby, StoreKeeperDate, SaveMode, 
                         SubmitDate, WorkflowStatus, WorkflowApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, EntryDate, EntryBy, NamePreparedBy, DesigPreparedBy, SigPreBy, NameReqBy, DesigReqBy, SigReqBy, NameAppBy, 
                         DesigAppBy, SigAppBy, NameIssuedBy, DesigIssuedBy, SigIssuedBy, NameDocBy, DesigDocBy, SigDocBy, NameHSectBy, DesigHSectBy, SigHSectBy, NameVDeterminBy, DesigVDeterminBy, SigVDeterminBy, 
                         NameSKeeperBy, DesigSKeeperBy, SigSKeeperBy, EmployeeName, Designation, Signature, NameOfRcvBy, DesigOfRcvrBy, SigForRcvBy, ReceiverDate FROM VwSirForm WHERE IDSirNo='" + SirId + "'");


                DataTableReader dr1 = dt1.CreateDataReader();
                XerpDataSet ds = new XerpDataSet();
                ds.Load(dr1, LoadOption.OverwriteChanges, ds.VwSirForm);

                DataTable dt2 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT SIRProductID, CategoryID, Category, SubCategoryID, SubCategory, ProductId, ProductName, IDSirNo, SIRVoucherNo, ProductDescription, QTYNeed, NeedQtyInWords, QTYDelivered, DeliveredQtyInWords, 
                         ApprovedQty, AvailableQtyInWords, QTYAvailable, UnitPrice, DeliveredQTYTotalPrice, EntryBy, DeliveredDate, PartNo, SerialNo, ModelNo
                FROM VwSirProduct WHERE IDSIRNo='" + SirId + "'");
                DataTableReader dr2 = dt2.CreateDataReader();
                ds.Load(dr2, LoadOption.OverwriteChanges, ds.VwSirProduct);

                rpt.Load(Server.MapPath("CrptSirReport.rpt"));

                //string datefield = "As on :" + Convert.ToDateTime(dateFrom).ToString(" dd/MM/yyyy");
                //string rptName = SQLQuery.ReturnString("SELECT [ControlAccountsName] FROM [ControlAccount] WHERE ControlAccountsID='" + item + "'");

                //DataSet ds = RunQuery.SQLQuery.ReturnDataSet("SELECT sl, StudentsID, StudentName, FathersName, Class, Roll, Section, Session, imgUrl FROM tmpIdCard ");

                rpt.SetDataSource(ds);
                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                //rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM GRNFrom WHERE (IDGrnNO='" + rvId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "CrptSirReport.rpt");
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