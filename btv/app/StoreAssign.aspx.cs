
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_StoreAssign : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            BindDdLocationId();
            BindDdCenterId();
            BindDdDepartmentSectionId();
            BindStore();
            BindEmployee();
            BindGrid();
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    if (IsAssign())
                    {
                        RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO StoreAssign (LocationID, CenterID, DepartmentSectionID, StoreID, EmployeeID) 
                        VALUES ('" + ddLocationID.SelectedValue + "', '" + ddCenterID.SelectedValue + "', '" + ddDepartmentSectionID.SelectedValue + "', '" + ddStoreID.SelectedValue + "', '" + ddEmployee.SelectedValue + "')");

                        Notify("Successfully Saved...", "success", lblMsg);

                    }
                    else
                    {
                        Notify("This store already assigned", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
            else
            {
                if (SQLQuery.OparatePermission(lName, "Update") == "1")
                {
                    RunQuery.SQLQuery.ExecNonQry(@"Update  StoreAssign SET LocationID= '" + ddLocationID.SelectedValue + "',  CenterID= '" + ddCenterID.SelectedValue + "',  DepartmentSectionID= '" + ddDepartmentSectionID.SelectedValue + "',  StoreID= '" + ddStoreID.SelectedValue + "',  EmployeeID='" + ddEmployee.SelectedValue + "' WHERE StoreAssignID='" + lblId.Text + "' ");
                    btnSave.Text = "Save";
                    Notify("Successfully Updated...", "success", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            BindGrid();
        }
    }

    private bool IsAssign()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool assignStatus = true;
        string count = SQLQuery.ReturnString("SELECT ISNULL(COUNT(StoreAssignID),0) FROM StoreAssign WHERE LocationID='" + ddLocationID.SelectedValue + "' AND CenterID='" + ddCenterID.SelectedValue + "' AND DepartmentSectionID='" + ddDepartmentSectionID.SelectedValue + "' AND EmployeeID= '" + ddEmployee.SelectedValue + "' AND StoreID='" + ddStoreID.SelectedValue + "' ");
        if (int.Parse(count) > 0)
        {
            assignStatus = false;
        }
        return assignStatus;
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblEditId = GridView1.Rows[index].FindControl("Label1") as Label;
                lblId.Text = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable(@"Select StoreAssignID, LocationID,CenterID, DepartmentSectionID, StoreID, EmployeeID FROM StoreAssign WHERE StoreAssignID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    ddLocationID.SelectedValue = dtx["LocationID"].ToString();
                    BindDdCenterId();
                    ddCenterID.SelectedValue = dtx["CenterID"].ToString();
                    BindDdDepartmentSectionId();
                    ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();
                    BindStore();
                    ddStoreID.SelectedValue = dtx["StoreID"].ToString();
                    BindEmployee();
                    ddEmployee.SelectedValue = dtx["EmployeeID"].ToString();


                }
                btnSave.Text = "Update";
                Notify("Edit mode activated ...", "info", lblMsg);
            }
            else
            {
                Notify("You are not eligible to attempt this operation", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ExecNonQry(@"Delete StoreAssign WHERE StoreAssignID='" + lblId.Text + "' ");
            BindGrid();
            Notify("Successfully Deleted...", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {
        /*DataTable dt = SQLQuery.ReturnDataTable(@"SELECT StoreAssign.StoreAssignID, StoreAssign.LocationID, Location.Name AS Location, StoreAssign.CenterID, Center.Name AS Center, StoreAssign.DepartmentSectionID, DepartmentSection.Name AS DepartmentSection, 
                         StoreAssign.StoreID, Store.Name AS StoreName, StoreAssign.EmployeeID, Employee.Name AS EmployeeName
                         FROM StoreAssign INNER JOIN
                         Location ON StoreAssign.LocationID = Location.LocationID INNER JOIN
                         Center ON StoreAssign.CenterID = Center.CenterID INNER JOIN
                         Employee ON StoreAssign.EmployeeID = Employee.EmployeeID INNER JOIN
                         Store ON StoreAssign.StoreID = Store.StoreID INNER JOIN
                         DepartmentSection ON Center.CenterID = DepartmentSection.CenterID AND StoreAssign.DepartmentSectionID = DepartmentSection.DepartmentSectionID");*/
        
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT StoreAssign.StoreAssignID, StoreAssign.LocationID, Location.Name AS Location, StoreAssign.CenterID, Center.Name AS Center, StoreAssign.DepartmentSectionID, DepartmentSection.Name AS DepartmentSection, 
                         StoreAssign.StoreID, Store.Name AS StoreName, StoreAssign.EmployeeID, Employee.Name AS EmployeeName
                         FROM StoreAssign INNER JOIN
                         Location ON StoreAssign.LocationID = Location.LocationID INNER JOIN
                         Center ON StoreAssign.CenterID = Center.CenterID INNER JOIN
                         Employee ON StoreAssign.EmployeeID = Employee.EmployeeID INNER JOIN
                         Store ON StoreAssign.StoreID = Store.StoreAssignID INNER JOIN
                         DepartmentSection ON StoreAssign.DepartmentSectionID = DepartmentSection.DepartmentSectionID WHERE Employee.EmployeeID='" + ddEmployee.SelectedValue + "'");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    private void BindDdLocationId()
    {
        string query = "";
        if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        SQLQuery.PopulateDropDown("Select LocationID, Name from Location " + query, ddLocationID, "LocationID", "Name");
    }


    protected void ddLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdCenterId();
        BindDdDepartmentSectionId();
        BindStore();
        BindEmployee();
        //GridView1.DataBind();
    }


    private void BindDdCenterId()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' ";
        }

        string strQuery = "Select CenterID, Name from Center " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddCenterID, "CenterID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddCenterID.Items.Insert(0, new ListItem("--- all ---", "0"));
        }
    }

    private void BindDdDepartmentSectionId()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddLocationID.SelectedValue + "' AND CenterID = '" + ddCenterID.SelectedValue + "' ";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID = '" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";
        }

        string strQuery = @"SELECT DepartmentSectionID, Name FROM DepartmentSection " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddDepartmentSectionID, "DepartmentSectionID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddDepartmentSectionID.Items.Insert(0, new ListItem("--- all ---", "0"));
        }
    }

    protected void ddCenterID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdDepartmentSectionId();
        BindStore();
        BindEmployee();
        //GridView1.DataBind();
    }

    private void BindStore()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID='" + ddLocationID.SelectedValue + "' AND CenterID='" + ddCenterID.SelectedValue + "' AND DepartmentSectionID='" + ddDepartmentSectionID.SelectedValue + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID='" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "' ";
        }
        SQLQuery.PopulateDropDown("SELECT StoreAssignID, Name FROM Store " + query + " ", ddStoreID, "StoreAssignID", "Name");
    }

    private void BindEmployee()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID='" + ddLocationID.SelectedValue + "' AND CenterID='" + ddCenterID.SelectedValue + "' AND DepartmentSectionID='" + ddDepartmentSectionID.SelectedValue + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID='" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "' ";
        }
        string strQuery = @"SELECT EmployeeID, Name FROM Employee " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddEmployee, "EmployeeID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddEmployee.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    protected void ddDepartmentSectionID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindStore();
        BindEmployee();
        //GridView1.DataBind();
    }


    protected void ddEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}
