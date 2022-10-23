
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_StockRegister : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            GetVoucherType();
            BindStore();
            BindddProduct();
            BindGridView();

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
        if (Page.User.IsInRole("Super Admin"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store";
        }
        else if (Page.User.IsInRole("Admin"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store WHERE (CenterID = '" + SQLQuery.GetCenterId(User.Identity.Name) + "')";
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

    private void BindddProduct()
    {
        try
        {
            ddProductID.Items.Clear();
            ddProductID.Items.Insert(0, new ListItem("Select", "0"));
            string strQuery = "SELECT ProductID, Name FROM Product ORDER BY Name";
            SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddProductID, "ProductID", "Name");
            //if (ddProductID.Text == "")
            //{
                ddProductID.Items.Insert(0, new ListItem("---All---", "0"));
            //}
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg);
        }

    }

    private void BindGrid()
    {
        string query = "";
        if (ddType.SelectedValue != "0" && ddType.SelectedValue == "GRN" && ddProductID.SelectedValue != "0")
        {
            query = " AND EntryType='" + ddType.SelectedValue + "' AND ProductID='" + ddProductID.SelectedValue + "'";
        }
        else
        {
            query = " AND EntryType='" + ddType.SelectedValue + "'";
        }

        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT StockRegID, VoucherID, CatgoryId, SubCategoryId, ProductID, LocationID, CenterID, DepartmentID, StoreID, EntryType, Date, PreviousStockIn, StockIn, StockInCashMemoChallanNo, Total, 
                         StockOutCashMemoChallanNo, SellQty, Status, Remarks, EntryBy, Priority
            FROM StockRegister  ORDER BY Priority");
        GridView1.DataSource = dt;
        GridView1.DataBind();
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
            query += " AND (Product.ProductID = @ProductId)";
        }
        if (storeID!= "0")
        {
            query += " AND (StockRegister.StoreID = @StoreID)";
        }
        //if (deptSectionId != "0" && productId == "0")
        //{
        //    query += " AND (EntryType = 'Issued')";
        //}
       
        /*
        SqlCommand cmd2 = new SqlCommand(@"SELECT Date, Description AS Particulars, 0 AS OpeningBalance, InvoiceID, EntryType, ItemGroup, ProductID, ProductName, ItemType, Customer, DeptID, WarehouseID, InQuantity AS Received, 0 AS Total, OutQuantity AS Issued, Price, 0 AS Balance, Status, Remark as Remarks, ProjectId, EntryBy, EntryDate, ReceiveTo
            FROM Stock WHERE ((EntryType = 'Received') OR
                         (EntryType = 'Issued') AND (Status = 'Purchase') OR
                         (Status = 'TransferOut')) " + query + " AND Date>= @DateFrom AND Date <= @DateTo ORDER BY ProductName, Date, OrderBy", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));*/

        SqlCommand cmd2 = new SqlCommand(@"SELECT StockRegister.StockRegID, StockRegister.VoucherID, StockRegister.CatgoryId, StockRegister.SubCategoryId, 0 AS OpeningBalance, StockRegister.ProductID, Product.Name AS ProductName, StockRegister.LocationID, StockRegister.CenterID, 
                         StockRegister.DepartmentID, StockRegister.StoreID, StockRegister.EntryType, StockRegister.Date, StockRegister.PreviousStockIn, StockRegister.StockIn, StockRegister.StockInCashMemoChallanNo, StockRegister.Total, 0 AS Balance,
                         StockRegister.StockOutCashMemoChallanNo, StockRegister.SellQty Issued, StockRegister.Status, StockRegister.Remarks, StockRegister.EntryBy, StockRegister.Priority,  Unit.Name AS UnitName
                         FROM StockRegister INNER JOIN Product ON StockRegister.ProductID = Product.ProductID INNER JOIN Unit ON Product.UnitID = Unit.UnitID WHERE StockRegister.StockRegID<>0 " + query + " AND Date >=@DateFrom AND Date <=@DateTo ORDER BY StockRegister.Date, ProductName, StockRegister.Priority", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


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
        dt1.Columns.Add(new DataColumn("UnitName", typeof(string)));
        dt1.Columns.Add(new DataColumn("Balance", typeof(decimal)));
        dt1.Columns.Add(new DataColumn("Remarks", typeof(string)));
        cmd2.Connection.Close();

        decimal debt = 0; string stockInCashMemoChallanNo = ""; string stockOutCashMemoChallanNo = ""; decimal credit = 0; decimal currBal = 0; decimal total = 0; decimal opening = 0; decimal price = 0;
        string date; string description;
        string remarks = "";string unit="";

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
            dr1["UnitName"] = unit;
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
                unit = ds.Tables[0].Rows[ic]["UnitName"].ToString();
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
                dr1["UnitName"] = unit;
                currBal = +currBal + debt - credit;
                dr1["Balance"] = string.Format("{0:N0}", currBal);
                dr1["Remarks"] = remarks;
                dt1.Rows.Add(dr1);
                ic++;

            } while (ic < recordcount);
        }

        /*
        //get closing balance
        currBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(VoucherCR),0) - isnull(sum(VoucherDR),0) FROM [VoucherDetails] WHERE ([AccountsHeadID] = '" + gdownId + "') and   EntryDate < '" + Convert.ToDateTime(dtTo).AddDays(+1).ToString("yyyy-MM-dd") + "'  and ISApproved='A'"));

        dr1 = dt1.NewRow();
        dr1["Date"] = Convert.ToDateTime(dtTo).ToShortDateString();
        dr1["Particulars"] = "Closing Balance";
        dr1["Received"] = 0;
        dr1["Total"] = 0;
        dr1["Issued"] = 0;
        dr1["Price"] = 0;
        dr1["Balance"] = string.Format("{0:N2}", (currBal));
        dr1["Remarks"] = remarks;
        dt1.Rows.Add(dr1);
        */

        GridView1.DataSource = dt1;
        GridView1.DataBind();

        return dt1;

    }
    protected void ddStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGridView();
    }

    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGridView();
    }

    protected void txtDateFrom_TextChanged(object sender, EventArgs e)
    {
        BindGridView();
    }

    protected void txtDateTo_TextChanged(object sender, EventArgs e)
    {
        BindGridView();
    }

    private void BindGridView()
    {
        string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");
        GetStockRegister("0", "0", ddStore.SelectedValue, ddProductID.SelectedValue, dateFrom, dateTo);
    }


    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGridView();
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        BindGridView();
    }
}
