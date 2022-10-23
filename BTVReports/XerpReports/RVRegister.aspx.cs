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
    public partial class RVRegister : System.Web.UI.Page
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
                query += " AND (VwRV.IDRvNo = '" + sirId + "')";
            }
            if (storeId != "0")
            {
                query += " AND (VwRV.Store = '" + storeId + "')";
            }


            if (query != "")
            {
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add("RVProductID");
                dt.Columns.Add("RVVoucherNo");
                dt.Columns.Add("LocationName");
                dt.Columns.Add("DateOfRV");

                dt.Columns.Add("CategoryID");
                dt.Columns.Add("SubCategoryID");
                dt.Columns.Add("ProductID");
                dt.Columns.Add("ProductName");

                dt.Columns.Add("ApprovedQty");
                dt.Columns.Add("ApprovedQtyInWords");
                dt.Columns.Add("Remarks");


                DataTable rvData = SQLQuery.ReturnDataTable(@"SELECT IDRvNo, RvInvoiceNo, DateOfRV, LocationID, LocationName, ProductReturnDivision, Store, StoreName, ReturnType, LvSIRVoucherNo, ProductReturnCause, FinYear, Comments, DocumentUrl, SaveMode, SubmitDate, 
                         WorkflowStatus, WorkflowApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, PreparedBy, Receivedby, ReceivedDate, ApprovedBy, ApprovedDate, LedgerWriterStoreBy, LedgerWriterStoreDate, ValueDeterminedby, 
                         ValueDeterminedDate, StoreKeeperBy, StoreKeeperDate, EntryBy, EntryDate, NamePreparedBy, DesigPreparedBy, SigPreparedBy, NameApprovedBy, DesigApprovedBy, SigApprovedBy, NameReceivedBy, DesigReceivedBy, 
                         SigReceivedBy, NameLWStoreBy, DesigLWStoreBy, SigLWStoreBy, NameVDeterminBy, DesigVDeterminBy, SigVDeterminBy, NameSKeeperBy, DesigSKeeperBy, SigSKeeperBy
                         FROM VwRV WHERE IDRvNo<>'0' " + query + " AND DateOfRV >='" + dtFrom + "' AND DateOfRV <='" + dtTo + "'");

                DataTableReader sirDr = rvData.CreateDataReader();
                XerpDataSet ds = new XerpDataSet();
                //ds.EnforceConstraints = false;
                ds.Load(sirDr, LoadOption.OverwriteChanges, ds.VwRV);


                foreach (DataRow rvRow in rvData.Rows)
                {
                    DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT RVProductID, DateOfRV, RVNo, RVVoucherNo, RVVoucherNoCategoryID, CategoryName, SubCategoryID, SubCategoryName, ProductID, ProductName, ProductStatus, ReturnQTY, ReturnQtyInWords, ApprovedQty, ApprovedQtyInWords, TotalPrice, 
                         ProductReceive, IssueDescription, UnitPrice, DepositalAccount, EntryBy, ReturnDate, PartNo, SerialNo, ModelNo, Brand, Remarks
                    FROM VwRVRegister WHERE RVNo='" + rvRow["IDRvNo"] + "'");

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
                        dr["RVProductID"] = drx["RVProductID"].ToString();
                        dr["RVVoucherNo"] = drx["RVVoucherNo"].ToString();
                        dr["LocationName"] = drx["LocationName"].ToString();
                        dr["DateOfRV"] = drx["DateOfRV"].ToString();

                        dr["CategoryID"] = drx["CategoryID"].ToString();
                        dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                        dr["ProductID"] = drx["ProductID"].ToString();
                        dr["ProductName"] = pName;

                        dr["ApprovedQty"] = drx["ApprovedQty"].ToString();
                        dr["ApprovedQtyInWords"] = drx["ApprovedQtyInWords"].ToString();
                        dr["Remarks"] = rvRow["Remarks"].ToString();
                        dt.Rows.Add(dr);
                    }

                }

                DataTableReader sirDetailsRow = dt.CreateDataReader();
                ds.EnforceConstraints = false;
                ds.Load(sirDetailsRow, LoadOption.OverwriteChanges, ds.VwRVRegister);

                rpt.Load(Server.MapPath("CrptRVRegister.rpt"));
                rpt.SetDataSource(ds);

                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                //rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM SIRFrom WHERE (IDRvNo='" + sirId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;

                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "RVRegister");
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