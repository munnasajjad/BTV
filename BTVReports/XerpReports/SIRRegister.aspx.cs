using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class SIRRegister : System.Web.UI.Page
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
            string sirId = Convert.ToString(Request.QueryString["VoucherId"]);


            string query = "";
            if (sirId != "0")
            {
                query += " AND (VwSirForm.IDSirNo = '" + sirId + "')";
            }
            if (storeId != "0")
            {
                query += " AND (VwSirForm.Store = '" + storeId + "')";
            }


            if (query != "")
            {
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add("SIRProductID");
                dt.Columns.Add("SIRVoucherNo");
                //dt.Columns.Add("LocationName");
                dt.Columns.Add("DateOfSir");

                dt.Columns.Add("CategoryID");
                dt.Columns.Add("SubCategoryID");
                dt.Columns.Add("ProductID");
                dt.Columns.Add("ProductName");

                dt.Columns.Add("QTYDelivered");
                dt.Columns.Add("DeliveredQtyInWords");
                dt.Columns.Add("Remarks");

               
                DataTable sirData = SQLQuery.ReturnDataTable(@"SELECT IDSirNo, DateOfSir, SirVoucherNo, LocationID, FinYear, LoanToEmployee, Store, GivenDivision, GivenDivisionDate, ProductUseAim, HeadOfCost, Remarks, DocumentUrl, PreparedBy, Issuedby, IssuedDate, 
                         Requisitionby, RequisitionDate, Documentedby, DocumentedDate, HeadSectionby, HeadSectionDate, Approvedby, ApprovedDate, ValueDeterminedby, ValueDeterminedDate, StoreKeeperby, StoreKeeperDate, SaveMode, 
                         SubmitDate, WorkflowStatus, WorkflowApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, EntryDate, EntryBy, NamePreparedBy, DesigPreparedBy, SigPreBy, NameReqBy, DesigReqBy, SigReqBy, NameAppBy, 
                         DesigAppBy, SigAppBy, NameIssuedBy, DesigIssuedBy, SigIssuedBy, NameDocBy, DesigDocBy, SigDocBy, NameHSectBy, DesigHSectBy, SigHSectBy, NameVDeterminBy, DesigVDeterminBy, SigVDeterminBy, 
                         NameSKeeperBy, DesigSKeeperBy, SigSKeeperBy, EmployeeName, Designation, Signature, NameOfRcvBy, DesigOfRcvrBy, SigForRcvBy, ReceiverDate FROM VwSirForm WHERE IDSirNo<>'0' " + query + " AND DateOfSir >='" + dtFrom + "' AND DateOfSir <='" + dtTo + "'");

                DataTableReader sirDr = sirData.CreateDataReader();
                XerpDataSet ds = new XerpDataSet();
                //ds.EnforceConstraints = false;
                ds.Load(sirDr, LoadOption.OverwriteChanges, ds.VwSirForm);
                 

                foreach (DataRow sirRow in sirData.Rows)
                {
                    DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT SIRProductID, DateOfSir, CategoryID, Category, SubCategoryID, SubCategory, ProductType, ProductId, ProductName, IDSirNo, SIRVoucherNo, ProductDescription, QTYNeed, NeedQtyInWords, QTYDelivered, 
                         DeliveredQtyInWords, ApprovedQty, AvailableQtyInWords, QTYAvailable, UnitPrice, DeliveredQTYTotalPrice, EntryBy, DeliveredDate, PartNo, SerialNo, ModelNo, Brand, Remarks
                    FROM VwSIRRegister WHERE IDSIRNo='" + sirRow["IDSirNo"] + "'");

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
                        dr["SIRProductID"] = drx["SIRProductID"].ToString();
                        dr["SIRVoucherNo"] = drx["SIRVoucherNo"].ToString();
                        //dr["LocationName"] = drx["LocationName"].ToString();
                        dr["DateOfSir"] = drx["DateOfSir"].ToString();

                        dr["CategoryID"] = drx["CategoryID"].ToString();
                        dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                        dr["ProductID"] = drx["ProductID"].ToString();
                        dr["ProductName"] = pName;

                        dr["QTYDelivered"] = drx["QTYDelivered"].ToString();
                        dr["DeliveredQtyInWords"] = drx["DeliveredQtyInWords"].ToString();
                        dr["Remarks"] = sirRow["Remarks"].ToString();
                        dt.Rows.Add(dr);
                    }

                }

                DataTableReader sirDetailsRow = dt.CreateDataReader();
                ds.EnforceConstraints = false;
                ds.Load(sirDetailsRow, LoadOption.OverwriteChanges, ds.VwSIRRegister);

                rpt.Load(Server.MapPath("CrptSirRegister.rpt"));
                rpt.SetDataSource(ds);

                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                //rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM SIRFrom WHERE (IDSirNo='" + sirId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;

                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "SirRegister");
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