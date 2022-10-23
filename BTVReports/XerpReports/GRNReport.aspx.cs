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
    public partial class GRNReport : System.Web.UI.Page
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
            string grnId = Convert.ToString(Request.QueryString["GrnId"]);
            if (grnId != "")
            {
                SqlCommand cmd7 = new SqlCommand(@"SELECT TOP (5) IDGrnNO, GRNInvoiceNo, DateOfGRN, ReferenceID, Reference, Supplier, InvoiceNo, DateofInvoiceNo, PurchaseOrderNo, DateofPurchaseOrderNo, ProductSHReceiveDate, TotalAmount, Remarks, PreparedBy, 
                         PreparedDate, ProductInspectorApprovedBy, ApprovedDate, StoreDivLedgerWritter, StoreDivLedgerWritterDate, AccountDivLedgerWritter, AccountDivLedgerDate, EntryDate, EntryBy, SigForAcDivLedgerWritter, 
                         SigForPreparedBy, SigForPInspectorApprovedBy, SigForStoreDivLedgerWritter, StoreDivLWName, StoreDivLWDesig, ProInspecAppByName, ProInspecAppByDesig, AcDivLWName, AcDivLWDesig, PreparedByName, 
                         PreparedByDesig FROM VwGRMFrom WHERE IDGrnNO='" + grnId + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd7.Connection.Open();
                SqlDataReader dr7 = cmd7.ExecuteReader();
                XerpDataSet ds = new XerpDataSet();
                ds.EnforceConstraints = false;
                ds.Load(dr7, LoadOption.OverwriteChanges, ds.VwGRMFrom);
                cmd7.Connection.Close();
                
                /*SqlCommand cmd = new SqlCommand(@"SELECT GRNProductID, GRNInvoiceNo, CategoryID, SubCategoryID, ProductID, UnitName, CountryOfOrigin, ManufacturingCompany, Less, More, ReceiveProduct, ReceiveQtyInWords, RejectProduct, PriceLetterNo, CurrencyID, Currency, UnitPrice, 
                TotalPrice, OtherCost, TotalCost, EntryBy, EntryDate, ProductName, ProductCategory, ProductSubCategory, Unit, ProductType
                FROM VwGRNProduct WHERE (GrnFormID='" + grnId + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["XERP_Cnn_String"].ConnectionString));
                cmd.Connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                ds.Load(dr, LoadOption.OverwriteChanges, ds.VwGRNProduct);
                cmd7.Connection.Close();*/

                DataTable dt = new DataTable();
                DataRow dr = null;

                dt.Columns.Add("GRNProductID");
                dt.Columns.Add("GRNInvoiceNo");
                dt.Columns.Add("CategoryID");
                dt.Columns.Add("SubCategoryID");
                dt.Columns.Add("ProductID");

                dt.Columns.Add("UnitName");
                dt.Columns.Add("CountryOfOrigin");
                dt.Columns.Add("ManufacturingCompany");
                dt.Columns.Add("Less");
                dt.Columns.Add("More");
                dt.Columns.Add("ReceiveProduct");
                dt.Columns.Add("ReceiveQtyInWords");
                dt.Columns.Add("RejectProduct");
                dt.Columns.Add("PriceLetterNo");
                dt.Columns.Add("CurrencyID");
                dt.Columns.Add("Currency");
                dt.Columns.Add("UnitPrice");

                dt.Columns.Add("TotalPrice");
                dt.Columns.Add("OtherCost");
                dt.Columns.Add("TotalCost");
                dt.Columns.Add("EntryBy");
                dt.Columns.Add("ProductName");
                dt.Columns.Add("ProductCategory");
                dt.Columns.Add("ProductSubCategory");
                dt.Columns.Add("Unit");
                dt.Columns.Add("ProductType");

                DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT GRNProductID, GRNInvoiceNo, CategoryID, SubCategoryID, ProductID, UnitName, CountryOfOrigin, ManufacturingCompany, Less, More, ReceiveProduct, ReceiveQtyInWords, RejectProduct, PriceLetterNo, CurrencyID, Currency, UnitPrice, 
                TotalPrice, OtherCost, TotalCost, EntryBy, EntryDate, ProductName, ProductCategory, ProductSubCategory, Unit, ProductType FROM VwGRNProduct WHERE (GrnFormID='" + grnId + "')");

                foreach (DataRow drx in dtx.Rows)
                {
                    //string productName = drx["ProductName"].ToString();
                    if (int.Parse(drx["ProductType"].ToString()) == 1)//Details Items (true)
                    {
                        DataTable pDetails = RunQuery.SQLQuery.ReturnDataTable(@"SELECT GrnNO, ProductID, Brand, ModelNo, COUNT(ProductDetailsID) AS ReceiveProduct
                            FROM ProductDetails WHERE (GrnNO='" + grnId + "') AND (ProductID = '" + drx["ProductID"] + "') GROUP BY GrnNO, ProductID, Brand, ModelNo ORDER BY ProductID");
                        string pName = drx["ProductName"].ToString();
                        string brand = String.Empty;
                        string serialNo = String.Empty;
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
                            DataTable serialDt = RunQuery.SQLQuery.ReturnDataTable(@"SELECT GrnNO, ProductID, SerialNo FROM ProductDetails WHERE (GrnNO='" + grnId + "') AND (ProductID = '" + drx["ProductID"] + "') " + serialQuery + "");
                            foreach (DataRow serialRow in serialDt.Rows)
                            {
                                serialNo += serialRow["SerialNo"] + ", ";
                            }
                            if (serialNo.Length > 0)
                            {
                                serialNo = ", Serial: " + serialNo.Trim().TrimEnd(',');
                            }
                            else
                            {
                                serialNo = String.Empty;
                            }

                            pName = drx["ProductName"] + " Brand: " + dataRow["Brand"] + ", Model: " + dataRow["ModelNo"] + serialNo;
                            serialNo = String.Empty;

                            dr = dt.NewRow();
                            dr["GRNProductID"] = drx["GRNProductID"].ToString();
                            dr["GRNInvoiceNo"] = drx["GRNInvoiceNo"].ToString();
                            dr["CategoryID"] = drx["CategoryID"].ToString();
                            dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                            dr["ProductID"] = drx["ProductID"].ToString();

                            dr["UnitName"] = drx["UnitName"].ToString();
                            dr["CountryOfOrigin"] = drx["CountryOfOrigin"].ToString();
                            dr["ManufacturingCompany"] = drx["ManufacturingCompany"].ToString();
                            dr["Less"] = drx["Less"].ToString();
                            dr["More"] = drx["More"].ToString();

                            dr["ReceiveProduct"] = dataRow["ReceiveProduct"].ToString();
                            dr["ReceiveQtyInWords"] =
                                SQLQuery.Int2WordsBangla(dataRow["ReceiveProduct"].ToString());
                            dr["RejectProduct"] = drx["RejectProduct"].ToString();
                            dr["PriceLetterNo"] = drx["PriceLetterNo"].ToString();
                            dr["CurrencyID"] = drx["CurrencyID"].ToString();
                            dr["Currency"] = drx["Currency"].ToString();
                            dr["UnitPrice"] = drx["UnitPrice"].ToString();
                            dr["TotalPrice"] = Convert.ToDecimal(dataRow["ReceiveProduct"]) * Convert.ToDecimal(drx["UnitPrice"]);
                            dr["OtherCost"] = drx["OtherCost"].ToString();
                            dr["TotalCost"] = drx["TotalCost"].ToString();
                            dr["EntryBy"] = drx["EntryBy"].ToString();
                            dr["ProductName"] = pName.ToString();
                            dr["ProductCategory"] = drx["ProductCategory"].ToString();
                            dr["ProductSubCategory"] = drx["ProductSubCategory"].ToString();
                            dr["Unit"] = drx["Unit"].ToString();
                            dr["ProductType"] = drx["ProductType"].ToString();
                            dt.Rows.Add(dr);

                        }


                    }
                    else
                    {
                        dr = dt.NewRow();
                        dr["GRNProductID"] = drx["GRNProductID"].ToString();
                        dr["GRNInvoiceNo"] = drx["GRNInvoiceNo"].ToString();
                        dr["CategoryID"] = drx["CategoryID"].ToString();
                        dr["SubCategoryID"] = drx["SubCategoryID"].ToString();
                        dr["ProductID"] = drx["ProductID"].ToString();

                        dr["UnitName"] = drx["UnitName"].ToString();
                        dr["CountryOfOrigin"] = drx["CountryOfOrigin"].ToString();
                        dr["ManufacturingCompany"] = drx["ManufacturingCompany"].ToString();
                        dr["Less"] = drx["Less"].ToString();
                        dr["More"] = drx["More"].ToString();
                        dr["ReceiveProduct"] = drx["ReceiveProduct"].ToString();
                        dr["ReceiveQtyInWords"] = drx["ReceiveQtyInWords"].ToString();
                        dr["RejectProduct"] = drx["RejectProduct"].ToString();
                        dr["PriceLetterNo"] = drx["PriceLetterNo"].ToString();
                        dr["CurrencyID"] = drx["CurrencyID"].ToString();
                        dr["Currency"] = drx["Currency"].ToString();
                        dr["UnitPrice"] = drx["UnitPrice"].ToString();

                        dr["TotalPrice"] = drx["TotalPrice"].ToString();
                        dr["OtherCost"] = drx["OtherCost"].ToString();
                        dr["TotalCost"] = drx["TotalCost"].ToString();
                        dr["EntryBy"] = drx["EntryBy"].ToString();
                        dr["ProductName"] = drx["ProductName"].ToString();
                        dr["ProductCategory"] = drx["ProductCategory"].ToString();
                        dr["ProductSubCategory"] = drx["ProductSubCategory"].ToString();
                        dr["Unit"] = drx["Unit"].ToString();
                        dr["ProductType"] = drx["ProductType"].ToString();
                        dt.Rows.Add(dr);
                    }

                }




                DataTableReader drn = dt.CreateDataReader();
                ds.EnforceConstraints = false;
                ds.Load(drn, LoadOption.OverwriteChanges, ds.VwGRNProduct);

                rpt.Load(Server.MapPath("CrptGRNReport.rpt"));
                rpt.SetDataSource(ds);
                //SQLQuery.LoadrptHeader(dsx, rpt);
                //rpt.SetParameterValue("@date", datefield);
                //rpt.SetParameterValue("@rptName", rptName);
                rpt.SetParameterValue("@remarks", SQLQuery.ReturnString("SELECT Remarks FROM GRNFrom WHERE (IDGrnNO='" + grnId + "')"));
                //CrystalReportViewer1.ReportSource = rpt;
                rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, false, "CrptGRNReport.rpt");
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