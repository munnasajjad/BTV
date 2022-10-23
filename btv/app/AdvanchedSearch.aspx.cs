using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_AdvanchedSearch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadCategoryID();
            LoadSubCategory();
            LoadProduct();
            LoadStore();
            LoadData();
        }
    }
    private DataTable GetStockRegister(string gdownId, string deptSectionId, string storeID, string productId)
    {
        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds;
        int recordcount = 0;
        int ic = 0;

        string query = "";
        if (deptSectionId == "0" && productId != "0")
        {
            query = "WHERE (Product.ProductID = @ProductId) AND (StockRegister.StoreID = @StoreID)";
        }
        else if (deptSectionId != "0" && productId == "0")
        {
            query = "WHERE (EntryType = 'Issued')";
        }
        else if (deptSectionId != "0" && productId != "0")
        {
            query = "WHERE (Product.ProductID = @ProductId)";
        }
        /*
        SqlCommand cmd2 = new SqlCommand(@"SELECT Date, Description AS Particulars, 0 AS OpeningBalance, InvoiceID, EntryType, ItemGroup, ProductID, ProductName, ItemType, Customer, DeptID, WarehouseID, InQuantity AS Received, 0 AS Total, OutQuantity AS Issued, Price, 0 AS Balance, Status, Remark as Remarks, ProjectId, EntryBy, EntryDate, ReceiveTo
            FROM Stock WHERE ((EntryType = 'Received') OR
                         (EntryType = 'Issued') AND (Status = 'Purchase') OR
                         (Status = 'TransferOut')) " + query + " AND Date>= @DateFrom AND Date <= @DateTo ORDER BY ProductName, Date, OrderBy", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));*/

        SqlCommand cmd2 = new SqlCommand(@"SELECT StockRegister.StockRegID, StockRegister.VoucherID, StockRegister.CatgoryId, StockRegister.SubCategoryId, 0 AS OpeningBalance, StockRegister.ProductID, Product.Name AS ProductName, StockRegister.LocationID, StockRegister.CenterID, 
                         StockRegister.DepartmentID, StockRegister.StoreID, StockRegister.EntryType, StockRegister.Date, StockRegister.PreviousStockIn, StockRegister.StockIn, StockRegister.StockInCashMemoChallanNo, StockRegister.Total, 0 AS Balance,
                         StockRegister.StockOutCashMemoChallanNo, StockRegister.SellQty Issued, StockRegister.Status, StockRegister.Remarks, StockRegister.EntryBy, StockRegister.Priority
                        FROM StockRegister INNER JOIN
                         Product ON StockRegister.ProductID = Product.ProductID " + query + "  ORDER BY StockRegister.Date, ProductName, StockRegister.Priority", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        cmd2.Parameters.Add("@StoreID", SqlDbType.Int).Value = storeID;
        cmd2.Parameters.Add("@ProductId", SqlDbType.VarChar).Value = productId;
        /*
        cmd2.Parameters.Add("@DeptID", SqlDbType.VarChar).Value = deptSectionId;*/
        //cmd2.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = Convert.ToDateTime(dtFrom).ToShortDateString();
        //cmd2.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = Convert.ToDateTime(dtTo).ToShortDateString();

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

        decimal preBal = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT  isnull(sum(StockIn),0) - isnull(sum(SellQty),0) FROM [StockRegister] WHERE ([ProductID] = '" + productId + "')  AND (StoreID='" + storeID + "')"));

        currBal = preBal;

        if (deptSectionId == "0")
        {
            dr1 = dt1.NewRow();
            //dr1["Date"] = Convert.ToDateTime(dtFrom).ToShortDateString();

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
                // date = Convert.ToDateTime(ds.Tables[0].Rows[ic]["Date"].ToString()).ToShortDateString();
                //description = ds.Tables[0].Rows[ic]["Particulars"].ToString();
                remarks = ds.Tables[0].Rows[ic]["Remarks"].ToString();
                debt = Convert.ToDecimal(ds.Tables[0].Rows[ic]["StockIn"].ToString());
                stockInCashMemoChallanNo = ds.Tables[0].Rows[ic]["StockInCashMemoChallanNo"].ToString();
                credit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Issued"].ToString());
                //price = Convert.ToDecimal(ds.Tables[0].Rows[ic]["Price"].ToString());
                stockOutCashMemoChallanNo = ds.Tables[0].Rows[ic]["StockOutCashMemoChallanNo"].ToString();

                dr1 = dt1.NewRow();
                //dr1["Date"] = date;
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

        gridSearch.DataSource = dt1;
        gridSearch.DataBind();

        return dt1;

    }
    private void LoadData()
    {
        string query = "";
        if (ddlCategory.SelectedValue != "0")
        {
            query += "AND (StockRegister.CatgoryId = '" + ddlCategory.SelectedValue + "')";
        }
        if (ddlSubCategory.SelectedValue != "0")
        {
            query += "AND (StockRegister.SubCategoryId = '" + ddlSubCategory.SelectedValue + "')";
        }
        if (ddlProduct.SelectedValue != "0")
        {
            query += "AND (StockRegister.ProductID = '" + ddlProduct.SelectedValue + "')";
        }
        if (ddlStore.SelectedValue != "0")
        {
            query += "AND (StockRegister.StoreID = '" + ddlStore.SelectedValue + "')";
        }
        if (txtSearch.Text!="")
        {
            query += "AND (Product.Name like N'%" + txtSearch.Text.Trim().Replace("'", "''") + "%')";
        }

        if (Page.User.IsInRole("User")|| Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin"))
        {
            query += "AND  StockRegister.StoreID IN(SELECT StoreID FROM StoreAssign WHERE  (EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        }
        
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT ISNULL(SUM(StockRegister.StockIn), 0) - ISNULL(SUM(StockRegister.SellQty), 0) AS QTY,  StockRegister.CatgoryId, StockRegister.SubCategoryId, StockRegister.ProductID, Product.Name AS ProductName, 
                  Store.Name, Location.Name AS LocationName, Unit.Name AS UnitName FROM StockRegister INNER JOIN Product ON StockRegister.ProductID = Product.ProductID INNER JOIN
                  Store ON StockRegister.StoreID = Store.StoreAssignID INNER JOIN Location ON StockRegister.LocationID = Location.LocationID INNER JOIN Unit ON Unit.UnitID = Product.UnitID WHERE StockRegister.StockRegID<>0" + query + @"GROUP BY StockRegister.CatgoryId, StockRegister.SubCategoryId, StockRegister.ProductID, Product.Name, Store.Name, Location.Name, Unit.Name");
        gridSearch.DataSource = dt;
        gridSearch.EmptyDataText = "No Data Found!";
        gridSearch.DataBind();
    }

    private void LoadProduct()
    {
        string query = "";
        if (ddlCategory.SelectedValue != "0")
        {
            query += " AND ProductCategoryID='" + ddlCategory.SelectedValue + "'";
        }
        if (ddlSubCategory.SelectedValue != "0")
        {
            query += " AND ProductSubCategoryID='" + ddlSubCategory.SelectedValue + "'";
        }
        SQLQuery.PopulateDropDownWithAll("SELECT  Name, ProductID FROM Product Where ProductID<>0" + query, ddlProduct, "ProductID", "Name");
        //SQLQuery.PopulateDropDownWithAll("SELECT  Product.Name+'-'+ProductDetails.SerialNo AS ProductName,ProductDetails.SerialNo FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID", ddProductID, "SerialNo", "ProductName");
    }
    private void LoadCategoryID()
    {
        SQLQuery.PopulateDropDownWithAll("Select ProductCategoryID, Name from ProductCategory", ddlCategory, "ProductCategoryID", "Name");
    }
    private void LoadSubCategory()
    {
        SQLQuery.PopulateDropDownWithAll("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddlCategory.SelectedValue + "'", ddlSubCategory, "ProductSubCategoryID", "Name");
    }
    private void LoadStore(string query = "")
    {
        if (Page.User.IsInRole("Super Admin")|| Page.User.IsInRole("Senior Store Officer"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store";
        }
        else if (Page.User.IsInRole("Admin"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store WHERE (CenterID = '" + SQLQuery.GetCenterId(User.Identity.Name) + "')";
        }
        else if (Page.User.IsInRole("Department Admin"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store WHERE (DepartmentSectionID = '" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "')";
        }
        else
        {
            query = @"SELECT Store.StoreAssignID, Store.Name
            FROM Store INNER JOIN StoreAssign ON Store.StoreAssignID = StoreAssign.StoreID
            WHERE (StoreAssign.EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "')";
        }

        SQLQuery.PopulateDropDownWithAll(query, ddlStore, "StoreAssignID", "Name");
        //if (ddlStore.SelectedValue == "")
        //{
        //    ddlStore.Items.Insert(0, new ListItem("---Select---", "0"));
        //}
        //SQLQuery.PopulateDropDown("Select StoreAssignID, Name from Store", ddSaveToStore, "StoreAssignID", "Name");
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSubCategory();
        LoadProduct();
        LoadData();
    }

    protected void ddlSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProduct();
        LoadData();
    }

    protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void gridSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string _id = gridSearch.DataKeys[e.Row.RowIndex].Value.ToString();
            string sQuery = @"SELECT ProductDetails.ProductDetailsID, ProductDetails.GrnNO, ProductDetails.ProductID, ProductDetails.ProductConditionStatus, ProductDetails.ProductStatus, ProductDetails.CountryOfOrigin, ProductDetails.ManufacturingCompany, 
                  ProductDetails.ManufactureDate, ProductDetails.PartNo, ProductDetails.SerialNo, ProductDetails.ModelNo, ProductDetails.IsGuaranty, ProductDetails.GuarantyWarrantyPeriod, ProductDetails.StoreID, ProductDetails.EntryBy, 
                  ProductDetails.EntryDate, Product.Name AS ProductName
                FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE ProductDetails.ProductID='" + _id + "'";
            GridView sc = (GridView)e.Row.FindControl("GridDetails");
            sc.DataSource = SQLQuery.ReturnDataTable(sQuery);
            sc.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void gridSearch_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridSearch.PageIndex = e.NewPageIndex;
        LoadData();
    }
}