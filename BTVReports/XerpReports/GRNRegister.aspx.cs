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
    public partial class GRNRegister : System.Web.UI.Page
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
            string grnId = Convert.ToString(Request.QueryString["VoucherId"]);


            string query = "";
            if (grnId != "0")
            {
                query += " AND (VwGRMFrom.IDGrnNO = '" + grnId + "')";
            }
            if (storeId != "0")
            {
                query += " AND (VwGRMFrom.StoreID = '" + storeId + "')";
            }


            if (query != "")
            {
                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add("GRNProductID");
                dt.Columns.Add("GRNInvoiceNo");
                dt.Columns.Add("LocationName");
                dt.Columns.Add("DateOfGRN");

                dt.Columns.Add("CategoryID");
                dt.Columns.Add("SubCategoryID");
                dt.Columns.Add("ProductID");
                dt.Columns.Add("ProductName");

                dt.Columns.Add("ReceiveProduct");
                dt.Columns.Add("ReceiveQtyInWords");
                dt.Columns.Add("Remarks");


                DataTable grnData = SQLQuery.ReturnDataTable(@"SELECT IDGrnNO, GRNInvoiceNo, DateOfGRN, ReferenceID, Reference, Supplier, InvoiceNo, DateofInvoiceNo, PurchaseOrderNo, DateofPurchaseOrderNo, ProductSHReceiveDate, TotalAmount, Remarks, PreparedBy, 
                         PreparedDate, ProductInspectorApprovedBy, ApprovedDate, StoreDivLedgerWritter, StoreDivLedgerWritterDate, AccountDivLedgerWritter, AccountDivLedgerDate, EntryDate, EntryBy, SigForAcDivLedgerWritter, 
                         SigForPreparedBy, SigForPInspectorApprovedBy, SigForStoreDivLedgerWritter, StoreDivLWName, StoreDivLWDesig, ProInspecAppByName, ProInspecAppByDesig, AcDivLWName, AcDivLWDesig, PreparedByName, 
                         PreparedByDesig FROM VwGRMFrom WHERE IDGrnNO<>'0' " + query + " AND IDGrnNO IN(SELECT IDGrnNO FROM GRNFrom WHERE DateOfGRN >='" + dtFrom + "' AND DateOfGRN <='" + dtTo + "')");

                DataTableReader sirDr = grnData.CreateDataReader();
                XerpDataSet ds = new XerpDataSet();
                ds.EnforceConstraints = false;
                ds.Load(sirDr, LoadOption.OverwriteChanges, ds.VwGRMFrom);


                foreach (DataRow grnRow in grnData.Rows)
                {
                    DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT GRNProductID, DateOfGRN, GRNInvoiceNo, CategoryID, GrnFormID, SubCategoryID, ProductID, UnitName, CountryOfOrigin, ManufacturingCompany, Less, More, ReceiveProduct, ReceiveQtyInWords, RejectProduct, 
                         PriceLetterNo, CurrencyID, Currency, UnitPrice, TotalPrice, OtherCost, TotalCost, EntryBy, EntryDate, ProductName, ProductCategory, ProductSubCategory, Unit, ProductType, Remarks
FROM VwGRNRegister WHERE (GrnFormID='" + grnRow["IDGrnNO"] + "')");
                   
                    /*
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
                        dr["GRNProductID"] = drx["GRNProductID"].ToString();
                        dr["GRNInvoiceNo"] = drx["GRNInvoiceNo"].ToString();
                        dr["LocationName"] = drx["LocationName"].ToString();
                        dr["DateOfGRN"] = drx["DateOfGRN"].ToString();

                        dr["CategoryID"] = drx["CategoryID"].ToString();
                        dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                        dr["ProductID"] = drx["ProductID"].ToString();
                        dr["ProductName"] = pName;

                        dr["ReceiveProduct"] = drx["ReceiveProduct"].ToString();
                        dr["ReceiveQtyInWords"] = drx["ReceiveQtyInWords"].ToString();
                        dr["Remarks"] = sirRow["Remarks"].ToString();
                        dt.Rows.Add(dr);
                    }
                    */
                    foreach (DataRow drx in dtx.Rows)
                    {
                        //string productName = drx["ProductName"].ToString();
                        if (int.Parse(drx["ProductType"].ToString()) == 1)//Details items
                        {
                            DataTable pDetails = RunQuery.SQLQuery.ReturnDataTable(@"SELECT GrnNO, ProductID, Brand, ModelNo, COUNT(ProductDetailsID) AS ReceiveProduct
                            FROM ProductDetails WHERE (GrnNO='" + grnRow["IDGrnNO"] + "') AND (ProductID = '" + drx["ProductID"] + "') GROUP BY GrnNO, ProductID, Brand, ModelNo ORDER BY ProductID");
                            string pName = "";
                            string brand = String.Empty;
                            string serialNo = String.Empty;
                            string partNo = String.Empty;
                            foreach (DataRow dataRow in pDetails.Rows)
                            {
                                string serialQuery = String.Empty;
                                if (dataRow["Brand"].ToString() == String.Empty)
                                {
                                    serialQuery = " AND (ModelNo = '" + dataRow["ModelNo"] + "') ";
                                }
                                else
                                {
                                    serialQuery = " AND (Brand = '" + dataRow["Brand"] + "') AND (ModelNo = '" + dataRow["ModelNo"] + "') ";
                                }
                                DataTable serialDt = RunQuery.SQLQuery.ReturnDataTable(@"SELECT GrnNO, ProductID, PartNo, SerialNo FROM ProductDetails WHERE (GrnNO='" + grnId + "') AND (ProductID = '" + drx["ProductID"] + "') " + serialQuery + "");
                                foreach (DataRow serialRow in serialDt.Rows)
                                {
                                    if (serialRow["SerialNo"].ToString()!="(Not Available)")
                                    {
                                        serialNo += serialRow["SerialNo"] + ", ";
                                    }
                                    else if (serialRow["PartNo"].ToString().Length > 1)
                                    {
                                        partNo += serialRow["PartNo"] + ", ";
                                    }

                                }
                                if (serialNo.Length > 0)
                                {
                                    serialNo = ", Serial: " + serialNo.Trim().TrimEnd(',');
                                }
                                else if (partNo.Length>1)
                                {
                                    partNo = ", PartNo: " + partNo.Trim().TrimEnd(',');
                                }
                                else
                                {
                                    serialNo = String.Empty;
                                    partNo = String.Empty;
                                }

                                pName = drx["ProductName"] + " Brand: " + dataRow["Brand"] + ", Model: " + dataRow["ModelNo"] + serialNo;
                                serialNo = String.Empty;

                                dr = dt.NewRow();
                                dr["GRNProductID"] = drx["GRNProductID"].ToString();
                                dr["GRNInvoiceNo"] = drx["GRNInvoiceNo"].ToString();
                                dr["DateOfGRN"] = drx["DateOfGRN"].ToString();
                                dr["CategoryID"] = drx["CategoryID"].ToString();
                                dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                                dr["ProductID"] = drx["ProductID"].ToString();

                                //dr["UnitName"] = drx["UnitName"].ToString();
                                //dr["CountryOfOrigin"] = drx["CountryOfOrigin"].ToString();
                                //dr["ManufacturingCompany"] = drx["ManufacturingCompany"].ToString();
                                //dr["Less"] = drx["Less"].ToString();
                                //dr["More"] = drx["More"].ToString();

                                dr["ReceiveProduct"] = dataRow["ReceiveProduct"].ToString();
                                dr["ReceiveQtyInWords"] =
                                    SQLQuery.Int2WordsBangla(dataRow["ReceiveProduct"].ToString());
                                //dr["RejectProduct"] = drx["RejectProduct"].ToString();
                                //dr["PriceLetterNo"] = drx["PriceLetterNo"].ToString();
                                //dr["CurrencyID"] = drx["CurrencyID"].ToString();
                                //dr["Currency"] = drx["Currency"].ToString();
                                //dr["UnitPrice"] = drx["UnitPrice"].ToString();
                                //dr["TotalPrice"] = Convert.ToDecimal(dataRow["ReceiveProduct"]) * Convert.ToDecimal(drx["UnitPrice"]);
                                //dr["OtherCost"] = drx["OtherCost"].ToString();
                                //dr["TotalCost"] = drx["TotalCost"].ToString();
                                //dr["EntryBy"] = drx["EntryBy"].ToString();
                                dr["ProductName"] = pName.ToString();
                                //dr["ProductCategory"] = drx["ProductCategory"].ToString();
                                //dr["ProductSubCategory"] = drx["ProductSubCategory"].ToString();
                                //dr["Unit"] = drx["Unit"].ToString();
                                dr["Remarks"] = drx["Remarks"].ToString();
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            dr = dt.NewRow();
                            dr["GRNProductID"] = drx["GRNProductID"].ToString();
                            dr["GRNInvoiceNo"] = drx["GRNInvoiceNo"].ToString();
                            dr["DateOfGRN"] = drx["DateOfGRN"].ToString();
                            dr["CategoryID"] = drx["CategoryID"].ToString();
                            dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                            dr["ProductID"] = drx["ProductID"].ToString();
                            dr["ProductName"] = drx["ProductName"].ToString();

                            //dr["UnitName"] = drx["UnitName"].ToString();
                            //dr["CountryOfOrigin"] = drx["CountryOfOrigin"].ToString();
                            //dr["ManufacturingCompany"] = drx["ManufacturingCompany"].ToString();
                            //dr["Less"] = drx["Less"].ToString();
                            //dr["More"] = drx["More"].ToString();
                            dr["ReceiveProduct"] = drx["ReceiveProduct"].ToString();
                            dr["ReceiveQtyInWords"] = drx["ReceiveQtyInWords"].ToString();
                            //dr["RejectProduct"] = drx["RejectProduct"].ToString();
                            //dr["PriceLetterNo"] = drx["PriceLetterNo"].ToString();
                            //dr["CurrencyID"] = drx["CurrencyID"].ToString();
                            //dr["Currency"] = drx["Currency"].ToString();
                            //dr["UnitPrice"] = drx["UnitPrice"].ToString();

                            //dr["TotalPrice"] = drx["TotalPrice"].ToString();
                            //dr["OtherCost"] = drx["OtherCost"].ToString();
                            //dr["TotalCost"] = drx["TotalCost"].ToString();
                            //dr["EntryBy"] = drx["EntryBy"].ToString();

                            //dr["ProductCategory"] = drx["ProductCategory"].ToString();
                            //dr["ProductSubCategory"] = drx["ProductSubCategory"].ToString();
                            //dr["Unit"] = drx["Unit"].ToString();
                            dr["Remarks"] = drx["Remarks"].ToString();
                            
                            dt.Rows.Add(dr);
                        }

                    }

                }

                DataTableReader sirDetailsRow = dt.CreateDataReader();
                ds.EnforceConstraints = false;
                ds.Load(sirDetailsRow, LoadOption.OverwriteChanges, ds.VwGRNRegister);

                //rpt.Load(Server.MapPath("CrptGrnRegister.rpt"));
                rpt.Load(Server.MapPath("CrptGrnRegister.rpt"));
                rpt.SetDataSource(ds);

                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                
                
                //CrystalReportViewer1.ReportSource = rpt;

                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "GrnRegister");
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