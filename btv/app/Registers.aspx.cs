using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Bcpg.OpenPgp;
using RunQuery;

public partial class app_Registers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Now.ToString(1 + "/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            GetVoucherType();
            BindStore();
            BindddVoucherNumber();
            LoadLabelWithDropdown();
            //BindGridView();

        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void GetVoucherType()
    {
        try
        {
            ddType.Items.Clear();
            string strQuery = "SELECT WorkTypeId, WorkTypeName FROM WorkType ORDER BY WorkTypeId";
            SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddType, "WorkTypeName", "WorkTypeName");
            if (ddType.Text == "")
            {
                ddType.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg);
        }

    }

    private void BindStore(string query = "")
    {
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store";
        }

        else
        {
            query = @"SELECT Store.StoreAssignID, Store.Name
            FROM Store INNER JOIN StoreAssign ON Store.StoreAssignID = StoreAssign.StoreID
            WHERE (StoreAssign.EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "')";
        }

        SQLQuery.PopulateDropDownWithoutSelect(query, ddStore, "StoreAssignID", "Name");
        if (ddStore.Text == "")
        {
            ddStore.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    private void BindddVoucherNumber()
    {
        try
        {
            ddVoucherNumber.Items.Clear();
            string strQuery = String.Empty;
            if (rdGrn.Checked)
            {
                strQuery = "SELECT IDGrnNO VoucherId, GRNInvoiceNo VoucherNumber FROM GRNFrom WHERE StoreID='" + ddStore.SelectedValue + "' ORDER BY GRNInvoiceNo";
            }
            else if (rdSir.Checked)
            {
                strQuery = "SELECT IDSirNo VoucherId, SirVoucherNo VoucherNumber FROM SIRFrom WHERE Store='" + ddStore.SelectedValue + "' ORDER BY SirVoucherNo";
            }
            else if (rdLoan.Checked)
            {
                strQuery = "SELECT IDLvNo VoucherId, LvInvoiceNo VoucherNumber FROM LoanVouchar WHERE Store='" + ddStore.SelectedValue + "' ORDER BY LvInvoiceNo";
            }
            else //RV
            {
                strQuery = "SELECT IDRvNo VoucherId, RvInvoiceNo VoucherNumber FROM ReturnVauchar WHERE Store='" + ddStore.SelectedValue + "' ORDER BY RvInvoiceNo";
            }


            SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddVoucherNumber, "VoucherId", "VoucherNumber");
            ddVoucherNumber.Items.Insert(0, new ListItem("---All---", "0"));
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg);
        }

    }


    private DataTable GetStockRegister(string gdownId, string deptSectionId, string storeID, string productId, string dtFrom, string dtTo)
    {
        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds;
        int recordcount = 0;
        int ic = 0;

        string query = "";
        if (productId != "0")
        {
            //query += " AND (Product.ProductID = @ProductId)";
        }
        if (storeID != "0")
        {
            query += " AND (StockRegister.StoreID = @StoreID)";
        }

        SqlCommand cmd2 = new SqlCommand(@"SELECT StockRegister.StockRegID, StockRegister.VoucherID, StockRegister.CatgoryId, StockRegister.SubCategoryId, 0 AS OpeningBalance, StockRegister.ProductID, Product.Name AS ProductName, StockRegister.LocationID, StockRegister.CenterID, 
                         StockRegister.DepartmentID, StockRegister.StoreID, StockRegister.EntryType, StockRegister.Date, StockRegister.PreviousStockIn, StockRegister.StockIn, StockRegister.StockInCashMemoChallanNo, StockRegister.Total, 0 AS Balance,
                         StockRegister.StockOutCashMemoChallanNo, StockRegister.SellQty Issued, StockRegister.Status, StockRegister.Remarks, StockRegister.EntryBy, StockRegister.Priority
                         FROM StockRegister INNER JOIN Product ON StockRegister.ProductID = Product.ProductID WHERE StockRegister.StockRegID<>0 " + query + " AND Date >=@DateFrom AND Date <=@DateTo ORDER BY StockRegister.Date, ProductName, StockRegister.Priority", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        cmd2.Parameters.Add("@StoreID", SqlDbType.Int).Value = storeID;
        cmd2.Parameters.Add("@ProductId", SqlDbType.VarChar).Value = productId;
        /*cmd2.Parameters.Add("@DeptID", SqlDbType.VarChar).Value = deptSectionId;*/
        cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
        cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).ToShortDateString();

        da = new SqlDataAdapter(cmd2);
        ds = new DataSet("Board");

        cmd2.Connection.Open();
        da.Fill(ds, "Board");

        dr = cmd2.ExecuteReader();
        recordcount = ds.Tables[0].Rows.Count;

        DataTable dt1 = new DataTable();
        DataRow dr1 = null;
        dt1.Columns.Add(new DataColumn("Date", typeof(string)));
        //dt1.Columns.Add(new DataColumn("Particulars", typeof(string)));
        dt1.Columns.Add(new DataColumn("OpeningBalance", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("StockIn", typeof(decimal)));

        dt1.Columns.Add(new DataColumn("StockInCashMemoChallanNo", typeof(string)));
        dt1.Columns.Add(new DataColumn("Total", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Issued", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("StockOutCashMemoChallanNo", typeof(string)));
        //dt1.Columns.Add(new DataColumn("Price", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Remarks", typeof(string)));
        cmd2.Connection.Close();

        decimal debt = 0; string stockInCashMemoChallanNo = ""; string stockOutCashMemoChallanNo = ""; decimal credit = 0; decimal currBal = 0; decimal total = 0; decimal opening = 0; decimal price = 0;
        string date; string description; string remarks = "";

        decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(StockIn),0) - isnull(sum(SellQty),0) FROM [StockRegister] WHERE ([ProductID] = '" + productId + "') AND (Date < '" + Convert.ToDateTime(dtFrom).ToString("yyyy-MM-dd") + "') AND (StoreID='" + storeID + "')"));

        currBal = preBal;

        if (deptSectionId == "0")
        {
            dr1 = dt1.NewRow();
            dr1["Date"] = Convert.ToDateTime(dtFrom).ToShortDateString();

            //dr1["Particulars"] = "Openning Balance";
            dr1["OpeningBalance"] = 0;
            dr1["StockIn"] = 0;
            dr1["StockInCashMemoChallanNo"] = "";
            dr1["Total"] = 0;
            dr1["Issued"] = 0;
            dr1["StockOutCashMemoChallanNo"] = "";
            //dr1["Price"] = 0;
            dr1["Balance"] = string.Format("{0:N0}", currBal);
            dr1["Remarks"] = remarks;
            dt1.Rows.Add(dr1);
        }


        if (recordcount > 0)
        {
            do
            {
                date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["Date"].ToString()).ToShortDateString();
                //description = ds.Tables[0].Rows[ic]["Particulars"].ToString();
                remarks = ds.Tables[0].Rows[ic]["Remarks"].ToString();
                debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["StockIn"].ToString());
                stockInCashMemoChallanNo = ds.Tables[0].Rows[ic]["StockInCashMemoChallanNo"].ToString();
                credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Issued"].ToString());
                //price = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Price"].ToString());
                stockOutCashMemoChallanNo = ds.Tables[0].Rows[ic]["StockOutCashMemoChallanNo"].ToString();

                dr1 = dt1.NewRow();
                dr1["Date"] = date;
                dr1["OpeningBalance"] = currBal;
                //dr1["Particulars"] = description;
                dr1["StockIn"] = debt;
                dr1["StockInCashMemoChallanNo"] = stockInCashMemoChallanNo;
                dr1["Total"] = currBal + debt;
                dr1["Issued"] = credit;
                dr1["StockOutCashMemoChallanNo"] = stockOutCashMemoChallanNo;
                //dr1["Price"] = price;
                currBal = +currBal + debt - credit;
                dr1["Balance"] = string.Format("{0:N0}", currBal);
                dr1["Remarks"] = remarks;
                dt1.Rows.Add(dr1);
                ic++;

            } while (ic < recordcount);
        }


        GridView1.DataSource = dt1;
        GridView1.DataBind();

        return dt1;

    }
    protected void ddStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddVoucherNumber();
        //BindGridView();
    }

    protected void ddVoucherNumber_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindGridView();
    }

    protected void txtDateFrom_TextChanged(object sender, EventArgs e)
    {
        //BindGridView();
    }

    protected void txtDateTo_TextChanged(object sender, EventArgs e)
    {
        //BindGridView();
    }

    private void BindGridView()
    {
        string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");
        GetStockRegister("0", "0", ddStore.SelectedValue, ddVoucherNumber.SelectedValue, dateFrom, dateTo);
    }


    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGridView();
    }
    protected void rdLoan_CheckedChanged(object sender, EventArgs e)
    {
        //ProductGridView();
        LoadLabelWithDropdown();
        BindddVoucherNumber();
    }

    private void LoadLabelWithDropdown()
    {
        if (rdGrn.Checked)
        {
            lblSirLVRV.Text = "GRN Number";
        }
        else if (rdSir.Checked)
        {
            lblSirLVRV.Text = "SIR Number";
        }
        else if (rdLoan.Checked)
        {
            lblSirLVRV.Text = "LV Number";
            // LoadLvInvoice();
        }
        else
        {
            lblSirLVRV.Text = "RV Number";
            //LoadSirInovice();
            //LoadSirReceivedBy();
        }
    }
    protected void btnSearch_OnClick(object sender, EventArgs e)
    {


        try
        {
            string str = String.Empty;
            string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";

            string dateForm = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");
            string urlx = "StoreId=" + ddStore.SelectedValue + "&VoucherId=" + ddVoucherNumber.SelectedValue + "&DateForm=" + dateForm + "&DateTo=" + dateTo;
            if (lblSirLVRV.Text == "GRN Number")
            {
                str = baseUrl + "GRNRegister.aspx?" + urlx;
            }
            else if (lblSirLVRV.Text == "SIR Number")
            {  
                str = baseUrl + "SIRRegister.aspx?" + urlx;
            }
            else if (lblSirLVRV.Text == "LV Number")
            {   
                str = baseUrl + "LVRegister.aspx?" + urlx;
            }
            else //RV
            {
                str = baseUrl + "RVRegister.aspx?" + urlx;
            }


            Response.Redirect(str);
            //Page.ClientScript.RegisterStartupScript(base.GetType(), "Open Window", "window.open('" + str + "' , 'no', 'height=680, width= 1024, top=300, left=500, location=1, scrollbars=1, resizable=1');", true);
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg);
        }
    }
}