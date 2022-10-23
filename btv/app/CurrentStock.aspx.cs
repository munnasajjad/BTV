using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_CurrentStock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProjectID"] = SQLQuery.ProjectID(User.Identity.Name);
        lblProject.Text = Session["ProjectID"].ToString();
        if (!IsPostBack)
        {
            //ddDept.DataBind();
            BindStore();
            txtDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            BindDdProduct();
            //PopulateDrpDown();
            //  ChangeCaptions();
        }

    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void ChangeCaptions()
    {
        //litCustomer.Text = SQLQuery.FindCaption("Customers", lblProject.Text);
        //litAgent.Text = SQLQuery.FindCaption("Agents", lblProject.Text);
        //lblName.Text = SQLQuery.FindCaption("Products", SQLQuery.ProjectID(User.Identity.Name));
        //lblPrdct.Text = SQLQuery.FindCaption("Products", SQLQuery.ProjectID(User.Identity.Name));
    }
    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        SQLQuery.PopulateDropDown("Select pid, ProductName from Products where (ProductGroup= '" + ddGroup.SelectedValue + "')  ORDER BY [ProductName]", ddProducts, "pid", "ProductName");
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

    private void BindDdProduct()
    {
        try
        {
            ddProducts.Items.Clear();
            ddProducts.Items.Insert(0, new ListItem("Select", "0"));
            string strQuery = "SELECT ProductID, Name FROM Product ORDER BY Name";
            SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddProducts, "ProductID", "Name");
            //if (ddProductID.Text == "")
            //{
            ddProducts.Items.Insert(0, new ListItem("---All---", "0"));
            //}
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg);
        }

    }
    private void PopulateDrpDown()
    {
        //SQLQuery.PopulateDropDown("SELECT sl, GroupName FROM [ProjectGroup]  Where IsActive='1' ORDER BY [GroupName]", ddGroup, "sl", "GroupName");
        //SQLQuery.PopulateDropDown("Select pid, ProductName from Products ORDER BY [ProductName]", ddProducts, "pid", "ProductName");
        //ddProducts.Items.Insert(0, new ListItem("--- All ---", "0"));

    }

    protected void OnClick(object sender, EventArgs e)
    {
        frame1.Visible = false;
        divGrid.Visible = true;
        string dt2 = Convert.ToDateTime(txtDate.Text).AddDays(1).ToString("yyyy-MM-dd");
        ////string deptId = ddDept.SelectedValue;
        //string grpId = ddGroup.SelectedValue;
        //string productId = ddProducts.SelectedValue;
        //string godownId = ddGodown.SelectedValue;
        //string urlx = "./" + "XerpReports/FormCurrentStockPosition.aspx?dateTo=" + dt2 + "&grp=" + grpId + "&product=" + productId + "&godown=" + godownId;
        //frame1.Attributes.Add("src", urlx);
        string query = "Stock.EntryDate<='" + dt2 + "' ";
        if (ddGroup.SelectedValue != "0")
        {
            query = query + "And Stock.ItemGroup='" + ddGroup.SelectedValue + "' ";
        }
        if (ddProducts.SelectedValue != "0")
        {
            query = query + "And Stock.ProductID='" + ddProducts.SelectedValue + "' ";
        }
        //if (godown != "0")
        //{
        //    query = query + "And Stock.WarehouseID='" + godown + "'";
        //}
        //search(dateTo);

        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT ProjectGroup.GroupName, Products.ProductName, ISNULL(SUM(Stock.InQuantity - Stock.OutQuantity),0) AS Balance, Warehouses.StoreName
                         FROM Stock INNER JOIN
                         Products ON Stock.ProductID = Products.pid INNER JOIN
                         ProjectGroup ON Products.ProductGroup = ProjectGroup.sl INNER JOIN
                         Warehouses ON Stock.WarehouseID = Warehouses.WID
                         where " + query + " And ItemType='main' GROUP BY ProjectGroup.GroupName, Products.ProductName, Warehouses.StoreName HAVING ISNULL(SUM(Stock.InQuantity - Stock.OutQuantity),0)<>0");
        GridView1.DataSource = dtx;
        GridView1.DataBind();
    }

    protected void ddGroup_OnDataBound(object sender, EventArgs e)
    {
        ddGroup.Items.Insert(0, new ListItem("--- All ---", "0"));
    }

    
    protected void OnPrintClick(object sender, EventArgs e)
    {
        frame1.Visible = true;
        divGrid.Visible = false;
        string urlx = "./GovReport/FrmCurrentStockPosition.aspx?grp=" + ddGroup.SelectedValue + "&&product=" + ddProducts.SelectedValue + "&&dateTo=" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "&&godown=" + ddStore.SelectedValue;
        frame1.Attributes.Add("src", urlx);
    }
}