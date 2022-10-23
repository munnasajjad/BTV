
using DocumentFormat.OpenXml.Wordprocessing;
using RunQuery;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_DesignationWithEmployee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            BindGrid();
            BindMainOfficeID();
            BindFunctionalOfficeId();
            bindDDDepartmentSectionID();
            BindEmployee();
            bindDDDesignationID();
        }
    }
    private void BindMainOfficeID()
    {
        string query = "";
        if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        SQLQuery.PopulateDropDown("Select LocationID, Name from Location " + query, ddlMainOffice, "LocationID", "Name");
    }

    private void BindFunctionalOfficeId()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddlMainOffice.SelectedValue + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' ";
        }

        string strQuery = "Select CenterID, Name from Center " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddlFunctionalOffice, "CenterID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddlFunctionalOffice.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- all ---", "0"));
        }
    }

    private void bindDDDepartmentSectionID()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddlMainOffice.SelectedValue + "' AND CenterID = '" + ddlFunctionalOffice.SelectedValue + "' ";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID = '" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";
        }

        string strQuery = @"SELECT DepartmentSectionID, Name FROM DepartmentSection " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddlDepartment, "DepartmentSectionID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- all ---", "0"));
        }
    }

   
   
    private void bindDDDesignationID()
    {
        SQLQuery.PopulateDropDown("Select DesignationID, Name from Designation", ddlDesignation, "DesignationID", "Name");
    }
    private void BindEmployee()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID='" + ddlMainOffice.SelectedValue + "' AND CenterID='" + ddlFunctionalOffice.SelectedValue + "' AND DepartmentSectionID='" + ddlDepartment.SelectedValue + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID='" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "' ";
        }
        string strQuery = @"SELECT EmployeeID, Name FROM Employee " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddlEmployee, "EmployeeID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddlEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
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
                    string oldDesignationId = "0";
                    if (ddlType.SelectedValue=="Promotion")
                    {
                        string isExist = SQLQuery.ReturnString("SELECT DesignationID FROM Employee Where EmployeeID='"+ddlEmployee.SelectedValue+"'");
                        if (isExist!="")
                        {
                            oldDesignationId = isExist;
                        }
                        SQLQuery.ReturnString("UPDATE Employee SET DesignationID='"+ddlDesignation.SelectedValue+ "' WHERE EmployeeID='"+ddlEmployee.SelectedValue+"'");
                    }
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO DesignationWithEmployee (MainOfficeID, FunctionalOfficeID, DepartmentID,EmployeeID, OldDesignationID, DesignationID,Status,EntryFrom) VALUES ('" + ddlMainOffice.SelectedValue + "', '" + ddlFunctionalOffice.SelectedValue + "', '" + ddlDepartment.SelectedValue + "',  '" + ddlEmployee.SelectedValue + "','"+oldDesignationId+"', '" + ddlDesignation.SelectedValue + "','"+ddlStatus.SelectedValue+"','"+ddlType.SelectedValue+"')");
                    ClearControls();
                    Notify("Successfully Saved...", "success", lblMsg);
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
                    if (ddlType.SelectedValue == "Promotion")
                    {
                        SQLQuery.ReturnString("UPDATE Employee SET DesignationID='" + ddlDesignation.SelectedValue + "' WHERE EmployeeID='" + ddlEmployee.SelectedValue + "'");
                    }
                    RunQuery.SQLQuery.ExecNonQry(" Update  DesignationWithEmployee SET MainOfficeID= '" + ddlMainOffice.SelectedValue + "',  FunctionalOfficeID= '" + ddlFunctionalOffice.SelectedValue + "',  DepartmentID= '" + ddlDepartment.SelectedValue + "',Status='"+ddlStatus.SelectedValue+"'   EmployeeID= '" + ddlEmployee.SelectedValue + "',OldDesignationID='',  DesignationID= '" + ddlDesignation.SelectedValue + "',EntryFrom='"+ddlType.SelectedValue+"' WHERE Id='" + lblId.Text + "' ");
                    ClearControls();
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select Id, MainOfficeID,FunctionalOfficeID,DepartmentID,EmployeeID,DesignationID FROM DesignationWithEmployee WHERE Id='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    BindMainOfficeID()
;                    ddlMainOffice.SelectedValue = dtx["MainOfficeID"].ToString();
                    BindFunctionalOfficeId();
                    ddlFunctionalOffice.SelectedValue = dtx["FunctionalOfficeID"].ToString();
                    bindDDDepartmentSectionID();
                    ddlDepartment.SelectedValue = dtx["DepartmentID"].ToString();
                    BindEmployee();
                    ddlEmployee.SelectedValue = dtx["EmployeeID"].ToString();
                    bindDDDesignationID();
                    ddlDesignation.SelectedValue = dtx["DesignationID"].ToString();

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
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Delete") == "1")
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;

                string oldDesignation = SQLQuery.ReturnString("SELECT OldDesignationID DesignationWithEmployee WHERE Id='" + lblId.Text + "'");
                string employeeID = SQLQuery.ReturnString("SELECT EmployeeID DesignationWithEmployee WHERE Id='" + lblId.Text + "'");
              
               int affectedRow= SQLQuery.ExecNonQry(" Delete DesignationWithEmployee WHERE Id='" + lblId.Text + "' ");
                if (affectedRow>0)
                {
                    SQLQuery.ReturnString("UPDATE Employee SET DesignationID='" + oldDesignation+ "' WHERE EmployeeID='" + employeeID + "'");
                }
                BindGrid();
                Notify("Successfully Deleted...", "success", lblMsg);
            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("The DELETE statement conflicted with the REFERENCE constraint"))
            {
                Notify("You cannot delete this data, because this data use another table", "error", lblMsg);

            }

        }
        
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {
        string query = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            query = "AND DesignationWithEmployee.MainOfficeID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }

        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT DesignationWithEmployee.Id, DesignationWithEmployee.MainOfficeID,DesignationWithEmployee.EntryFrom, Location.Name AS MainOffice, DesignationWithEmployee.FunctionalOfficeID, Center.Name AS FunctionalOffice, DesignationWithEmployee.DepartmentID, 
                  DepartmentSection.Name AS Department, DesignationWithEmployee.EmployeeID, Employee.Name AS EmployeeName, DesignationWithEmployee.Status, DesignationWithEmployee.DesignationID, Designation.Name AS Designation,DesignationWithEmployee.EntryFrom, CONVERT(varchar, DesignationWithEmployee.EntryDate, 103) AS EntryDate FROM DesignationWithEmployee INNER JOIN Location ON DesignationWithEmployee.MainOfficeID = Location.LocationID INNER JOIN
                  Center ON DesignationWithEmployee.FunctionalOfficeID = Center.CenterID INNER JOIN DepartmentSection ON DesignationWithEmployee.DepartmentID = DepartmentSection.DepartmentSectionID INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID
                  WHERE (DesignationWithEmployee.EntryFrom <> 'Employee') "+query+"");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    private void ClearControls()
    {
        //txtMainOfficeID.Text = "";
        //txtFunctionalOfficeID.Text = "";
        //txtDepartmentID.Text = "";
        //txtStoreID.Text = "";
        //txtEmployeeID.Text = "";
        //txtDesignationID.Text = "";

    }

    protected void ddlMainOffice_SelectedIndexChanged(object sender, EventArgs e)
    {

        BindFunctionalOfficeId();
        bindDDDepartmentSectionID();
        BindEmployee();

    }

    protected void ddlFunctionalOffice_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindDDDepartmentSectionID();
        BindEmployee();
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {

       
        BindEmployee();
    }

    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindEmployee();
    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}
