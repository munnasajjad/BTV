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
    public partial class LVRegister : System.Web.UI.Page
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

            string dtFrom = Convert.ToString(Request.QueryString["DateForm"]);
            string dtTo = Convert.ToString(Request.QueryString["DateTo"]);

            string storeId = Convert.ToString(Request.QueryString["StoreId"]);
            string lvId = Convert.ToString(Request.QueryString["VoucherId"]);

            string query = "";
            if (lvId != "0")
            {
                query += " AND (VwLV.IDLvNo = '" + lvId + "')";
            }
            if (storeId != "0")
            {
                query += " AND (VwLV.Store = '" + storeId + "')";
            }


            if (query != "")
            {
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add("LVProductID");
                dt.Columns.Add("LvInvoiceNo");
                
                dt.Columns.Add("DateofLv");

                dt.Columns.Add("CategoryID");
                dt.Columns.Add("SubCategoryID");
                dt.Columns.Add("ProductID");
                dt.Columns.Add("ProductName");

                dt.Columns.Add("QTYDelivered");
                dt.Columns.Add("DeliveredQtyInWords");
                dt.Columns.Add("Remarks");


                DataTable lvData = SQLQuery.ReturnDataTable(@"SELECT IDLvNo, LvInvoiceNo, DateofLv, Division, LoanType, LoanToEmployee, ResponsiblePerson, CauseOfLoan, LocationID, Store, FinYear, DocumentUrl, Remarks, Verifier, RequisitionBy, DeliveredBy, PreparedBy, 
                         PreparedDate, EntryBy, EntryDate, SaveMode, Issuedby, IssuedDate, Approvedby, ApprovedbyDate, ReceivedBy, ReceivedByDate, ProductReturnReceiverStore, ProductReturnReceiverStoreDate, SubmitDate, WorkflowStatus, 
                         WorkflowApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, SigForPreparedBy, NameOfPreparedBy, DesigOfPreparedBy, EmpForAppBy, DesigForAppBy, SigForApprovedby, EmpForIssuBy, DesigForIssuBy, 
                         SigForIssuedby, EmpReturnBy, DesigReturnBy, SigForProductReturnReceiverStore, EmpForReceivedBy, DesigReceivedBy, SigForReceivedBy FROM VwLV WHERE IDLvNo<>'0' " + query + " AND DateofLv >='" + dtFrom + "' AND DateofLv <='" + dtTo + "'");

                DataTableReader sirDr = lvData.CreateDataReader();
                XerpDataSet ds = new XerpDataSet();
                //ds.EnforceConstraints = false;
                ds.Load(sirDr, LoadOption.OverwriteChanges, ds.VwLV);


                foreach (DataRow lvRow in lvData.Rows)
                {
                    DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT  LVProductID, IDLVNo, LvInvoiceNo, DateofLv, CategoryID, Category, SubCategoryID, SubCategory, ProductID, ProductName, PartNo, SerialNo, ModelNo, Brand, QTYNeed, NeedQtyInWords, QTYDelivered, 
                         DeliveredQtyInWords, ApprovedQty, DeliveredDate, ReturnQTY, ProductCondition, EntryBy, EntryDate, Remarks
FROM            VwLVRegister WHERE IDLvNo='" + lvRow["IDLvNo"] + "'");

                    string pName = "";
                    string brand = String.Empty;
                    string serialNo = String.Empty;
                    string partNo = String.Empty;
                    foreach (DataRow drx in dtx.Rows)
                    {
                        string serialQuery = String.Empty;
                        if (drx["Brand"].ToString() == String.Empty)
                        {
                            serialQuery = " AND (ModelNo = '" + drx["ModelNo"] + "') ";
                        }
                        else
                        {
                            serialQuery = " AND (Brand = '" + drx["Brand"] + "') AND (ModelNo = '" + drx["ModelNo"] + "') ";
                        }
                        DataTable serialDt = RunQuery.SQLQuery.ReturnDataTable(@"SELECT GrnNO, ProductID, PartNo, SerialNo FROM ProductDetails WHERE (ProductID = '" + drx["ProductID"] + "') " + serialQuery + ""); //(GrnNO='" + sirId + "') AND
                        foreach (DataRow serialRow in serialDt.Rows)
                        {
                            if (serialRow["SerialNo"].ToString().Length > 1)
                            {
                                serialNo += serialRow["SerialNo"] + ", ";
                            }
                            else if (serialRow["PartNo"].ToString().Length > 1)
                            {
                                partNo += serialRow["PartNo"] + ", ";
                            }
                        }
                        if (serialNo.Length > 1)
                        {
                            serialNo = ", Serial: " + serialNo.Trim().TrimEnd(',');
                        }
                        else if (partNo.Length > 1)
                        {
                            partNo = ", PartNo: " + partNo.Trim().TrimEnd(',');
                        }
                        else
                        {
                            serialNo = String.Empty;
                            partNo = String.Empty;
                        }


                        if (drx["Brand"].ToString().Length > 0 && drx["ModelNo"].ToString().Length > 0)
                        {
                            pName = drx["ProductName"] + " Brand: " + drx["Brand"] + ", Model: " + drx["ModelNo"] + serialNo + partNo;
                        }
                        else if (drx["Brand"].ToString().Length > 1 && drx["ModelNo"].ToString().Length == 0)
                        {

                            pName = drx["ProductName"] + " Brand: " + drx["Brand"] + serialNo + partNo;
                        }
                        else if (drx["Brand"].ToString().Length == 0 && drx["ModelNo"].ToString().Length > 1)
                        {

                            pName = drx["ProductName"] + " Model: " + drx["ModelNo"] + serialNo + partNo;
                        }
                        else
                        {
                            pName = drx["ProductName"] + serialNo + partNo;
                        }
                        serialNo = String.Empty;
                        partNo = String.Empty;

                        dr = dt.NewRow();
                        dr["LVProductID"] = drx["LVProductID"].ToString();
                        dr["LvInvoiceNo"] = drx["LvInvoiceNo"].ToString();
                        
                        dr["DateofLv"] = drx["DateofLv"].ToString();

                        dr["CategoryID"] = drx["CategoryID"].ToString();
                        dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                        dr["ProductID"] = drx["ProductID"].ToString();
                        dr["ProductName"] = pName;

                        dr["QTYDelivered"] = drx["QTYDelivered"].ToString();
                        dr["DeliveredQtyInWords"] = drx["DeliveredQtyInWords"].ToString();
                        dr["Remarks"] = lvRow["Remarks"].ToString();
                        dt.Rows.Add(dr);
                    }

                }

                DataTableReader sirDetailsRow = dt.CreateDataReader();
                ds.EnforceConstraints = false;
                ds.Load(sirDetailsRow, LoadOption.OverwriteChanges, ds.VwLVRegister);

                rpt.Load(Server.MapPath("CrptLVRegister.rpt"));
                rpt.SetDataSource(ds);

                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                //rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM SIRFrom WHERE (IDLvNo='" + sirId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;

                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "LVRegister");
                rpt.Close();
                rpt.Dispose();
                CrystalReportViewer1.Dispose();

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