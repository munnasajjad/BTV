using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;


public partial class app_EmployeeTransfer : System.Web.UI.Page
{
    string deptSecId = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        //this.Page.Form.Enctype = "multipart/form-data";
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {

            deptSecId = SQLQuery.GetDepartmentSectionId(Page.User.Identity.Name);
            bindDDEmployee();
            bindDDDesignationID();
            bindDDLocationID();
            bindDDNewLocationID();
            BindDdCenterId();
            BindDdNewCenterId();
            bindDDDepartmentSectionID();
            bindDDNewDepartmentSectionID();
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

    private bool IsMobileExist(string mobile)
    {
        bool status = false;
        string isExist = SQLQuery.ReturnString(@"SELECT Mobile FROM Employee WHERE(Mobile = '" + mobile + "')");
        if (isExist == "")
        {
            status = true;
        }
        return status;
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

                    RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO EmployeeTransfer (EmployeeID, PreviousOfficeID, PreviousFunctionalOfficeID, PreviousDepartmentID, CurrentOfficeID, CurrentFunctionalOfficeID, CurrentDepartmentID, TransferDate, EntryBy) VALUES ('" + ddEmployee.SelectedValue + "','" + ddLocationID.SelectedValue + "', '" + ddCenterID.SelectedValue + "', '" + ddDepartmentSectionID.SelectedValue + "','" + ddNewLocationID.SelectedValue + "','" + ddNewCenterID.SelectedValue + "', '" + ddNewDepartment.SelectedValue + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', '" + lName + "')");
                    string empFKeyId = SQLQuery.ReturnString(@"SELECT ISNULL(MAX(EmployeeID),0) FROM Employee WHERE EmpId='" + ddEmployee.SelectedValue + "'");
                    SQLQuery.ExecNonQry("Update  Employee SET LocationID= '" + ddNewLocationID.SelectedValue + "', CenterID= '" + ddNewCenterID.SelectedValue + "',  DepartmentSectionID= '" + ddNewDepartment.SelectedValue + "' WHERE EmployeeID='" + empFKeyId + "' ");
                    string employeeTransferId = SQLQuery.ReturnString(@"SELECT ISNULL(MAX(employeeTransferID),0) FROM EmployeeTransfer WHERE EmployeeID='" + ddEmployee.SelectedValue + "'");
                    AddUpdateDesignation(int.Parse(empFKeyId), int.Parse(employeeTransferId));
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
                    RunQuery.SQLQuery.ExecNonQry("Update EmployeeTransfer SET EmployeeID='" + ddEmployee.SelectedValue + "', LocationID= '" + ddLocationID.SelectedValue + "', CenterID= '" + ddCenterID.SelectedValue + "',  DepartmentSectionID= '" + ddDepartmentSectionID.SelectedValue + "' WHERE EmployeeID='" + lblId.Text + "' ");
                    string empFKeyId = SQLQuery.ReturnString(@"SELECT ISNULL(MAX(EmployeeID),0) FROM Employee WHERE EmpId='" + ddEmployee.SelectedValue + "'");
                    SQLQuery.ExecNonQry("Update  Employee SET LocationID= '" + ddLocationID.SelectedValue + "', CenterID= '" + ddCenterID.SelectedValue + "',  DepartmentSectionID= '" + ddDepartmentSectionID.SelectedValue + "' WHERE EmployeeID='" + empFKeyId + "' ");
                    string employeeTransferId = SQLQuery.ReturnString(@"SELECT ISNULL(MAX(employeeTransferID),0) FROM EmployeeTransfer WHERE EmployeeID='" + ddEmployee.SelectedValue + "'");
                    AddUpdateDesignation(int.Parse(lblId.Text), Convert.ToInt32(employeeTransferId));
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

    private void AddUpdateDesignation(int empFKeyId, int employeeTransferId)
    {
        SqlCommand cmd2 = new SqlCommand();

        cmd2 = new SqlCommand("UPDATE DesignationWithEmployee SET MainOfficeID=@MainOfficeID, FunctionalOfficeID=@FunctionalOfficeID, DepartmentID=@DepartmentID,EmployeeID=@EmployeeID,EmployeeTransferID=@EmployeeTransferID, DesignationID=@DesignationID,Status=@Status WHERE (EmployeeID = '" + empFKeyId + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@MainOfficeID", ddNewLocationID.SelectedValue);
        cmd2.Parameters.AddWithValue("@FunctionalOfficeID", ddNewCenterID.SelectedValue);
        cmd2.Parameters.AddWithValue("@DepartmentID", ddNewDepartment.SelectedValue);
        cmd2.Parameters.AddWithValue("@EmployeeID", empFKeyId);
        cmd2.Parameters.AddWithValue("@EmployeeTransferID", employeeTransferId);
        cmd2.Parameters.AddWithValue("@DesignationID", ddDesignationID.SelectedValue);
        cmd2.Parameters.AddWithValue("@Status", "Active");
        cmd2.Connection.Open();
        int rowAffected = cmd2.ExecuteNonQuery();
        if (rowAffected > 0)
        {
            //SQLQuery.ExecNonQry("UPDATE CPFHistory SET Dept_Number='" + drpDepartment.SelectedValue + "' WHERE (Emp_Number = '" + empNumber + "')");
        }

        cmd2.Connection.Close();
    }

    protected void EditMode()
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Update") == "1")
        {
            DataTable dt = SQLQuery.ReturnDataTable(@"Select EmployeeID,EmpId, Name,DesignationID, Gender,BloodGroup,MaterialStatus,Mobile,TelephoneOffice,Email,EduQualifiactionID,DOB,FatherName,NID,Relagion,PresentAddress,PermanentAddress,DateOfJoining, LocationID, CenterID, DepartmentSectionID,StoreID,OfficeID FROM Employee WHERE EmpId='" + ddEmployee.SelectedValue + "'");
            foreach (DataRow dtx in dt.Rows)
            {
                ddEmployee.SelectedValue = dtx["EmpId"].ToString();
                txtName.Text = dtx["Name"].ToString();
                ddDesignationID.SelectedValue = dtx["DesignationID"].ToString();
                ddLocationID.SelectedValue = dtx["LocationID"].ToString();
                BindDdCenterId();
                ddCenterID.SelectedValue = dtx["CenterID"].ToString();
                bindDDDepartmentSectionID();
                ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();
                //ddOfficeID.SelectedValue = dtx["OfficeID"].ToString();


            }
            //btnSave.Text = "Update";
            Notify("Edit mode activated ...", "info", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation", "warn", lblMsg);
        }
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    string lName = Page.User.Identity.Name.ToString();
        //    if (SQLQuery.OparatePermission(lName, "Update") == "1")
        //    {
        //        int index = Convert.ToInt32(GridView1.SelectedIndex);
        //        Label lblEditId = GridView1.Rows[index].FindControl("Label1") as Label;
        //        lblId.Text = lblEditId.Text;
        //        DataTable dt = SQLQuery.ReturnDataTable(@"Select EmployeeID,EmpId, Name,DesignationID, Gender,BloodGroup,MaterialStatus,Mobile,TelephoneOffice,Email,EduQualifiactionID,DOB,FatherName,NID,Relagion,PresentAddress,PermanentAddress,DateOfJoining, LocationID, CenterID, DepartmentSectionID,StoreID,OfficeID FROM Employee WHERE EmployeeID='" + lblId.Text + "'");
        //        foreach (DataRow dtx in dt.Rows)
        //        {
        //            ddEmployee.SelectedValue = dtx["EmpId"].ToString();
        //            txtName.Text = dtx["Name"].ToString();
        //            ddDesignationID.SelectedValue = dtx["DesignationID"].ToString();
        //            ddLocationID.SelectedValue = dtx["LocationID"].ToString();
        //            BindDdCenterId();
        //            ddCenterID.SelectedValue = dtx["CenterID"].ToString();
        //            bindDDDepartmentSectionID();
        //            ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();
        //            //ddOfficeID.SelectedValue = dtx["OfficeID"].ToString();


        //        }
        //        btnSave.Text = "Update";
        //        Notify("Edit mode activated ...", "info", lblMsg);
        //    }
        //    else
        //    {
        //        Notify("You are not eligible to attempt this operation", "warn", lblMsg);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Notify(ex.ToString(), "error", lblMsg);
        //}
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
                RunQuery.SQLQuery.ExecNonQry("Delete Employee WHERE EmployeeID='" + lblId.Text + "' ");
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

            Notify(ex.ToString(), "error", lblMsg);
        }

    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {

        DataTable dt;
        string query = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            query = " AND (CurrentOfficeID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name.ToString()) + "' OR LocationID = '" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name.ToString()) + "')";
        }
        //else if (Page.User.IsInRole("Department Admin"))
        //{
        //    query = " WHERE Employee.LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND Employee.CenterID='" + RunQuery.SQLQuery.GetCenterID(User.Identity.Name) + "' ";
        //}
        string searchQry = txtSearch.Text.Trim().Replace("'", "''");

        if (txtSearch.Text != "")
        {
            query += " AND (Name like '%" + searchQry + "%')";
        }

        string sql =
            @"SELECT EmployeeID, EmpId, Name, LocationID, PreviousMainOffice, CenterID, PreviousFunctionalOffice, DepartmentSectionID, PreviousDepartment, CurrentOfficeID, CurrentMainOffice, CurrentFunctionalOfficeID, CurrentFunctionalOffice, CurrentDepartmentID, CurrentDepartment FROM VwEmployeeTransferHistory WHERE EmployeeID<>0 " + query + " ORDER BY Name";
        dt = SQLQuery.ReturnDataTable(sql);

        //else
        //{
        //    dt = SQLQuery.ReturnDataTable(@"SELECT Employee.EmployeeID, Employee.EmpId,Employee.Name, Employee.Mobile, Employee.Email, Employee.NID, Employee.DateOfJoining, EducationQualification.Name AS Education, Designation.Name AS Designation, Employee.Gender, 
        //          DepartmentSection.Name AS Department, Location.Name AS MainOffice, Center.Name AS FunctionalOffice
        //            FROM Employee INNER JOIN Designation ON Employee.DesignationID = Designation.DesignationID INNER JOIN
        //          EducationQualification ON Employee.EduQualifiactionID = EducationQualification.EduQualifiactionID INNER JOIN
        //          DepartmentSection ON Employee.DepartmentSectionID = DepartmentSection.DepartmentSectionID INNER JOIN
        //          Location ON Employee.LocationID = Location.LocationID INNER JOIN
        //          Center ON DepartmentSection.CenterID = Center.CenterID AND Location.LocationID = Center.LocationID WHERE DepartmentSection.DepartmentSectionID='" + deptSecId + "'");
        //}
        GridView1.DataSource = dt;
        GridView1.DataBind();

    }

    private void bindDDEmployee()
    {
        string query = "";
        if (Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' ";
        }
        SQLQuery.PopulateDropDown("SELECT DISTINCT dbo.Employee.EmpId, CONCAT(dbo.Employee.Name,'-',dbo.Designation.Name) AS Name FROM dbo.Employee INNER JOIN dbo.Designation ON dbo.Employee.DesignationID = dbo.Designation.DesignationID " + query + "  ORDER BY Name ASC", ddEmployee, "EmpId", "Name");
    }
    private void bindDDDesignationID()
    {
        SQLQuery.PopulateDropDown("Select DesignationID, Name from Designation", ddDesignationID, "DesignationID", "Name");
    }


    protected void ddDesignationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void ddEduQualifiactionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
    //private void bindDDLocationID()
    //{
    //    string query = "";
    //    if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
    //    {
    //        query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name)+"'";
    //    }
    //    else if (Page.User.IsInRole("Department Admin"))
    //    {
    //        query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' ";
    //    }
    //    SQLQuery.PopulateDropDown("Select LocationID, Name from Location "+query, ddLocationID, "LocationID", "Name");

    //}
    private void bindDDLocationID()
    {
        string query = "";
        if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        SQLQuery.PopulateDropDown("Select LocationID, Name from Location " + query, ddLocationID, "LocationID", "Name");
    }
    private void bindDDNewLocationID()
    {
        string query = "";
        //if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        //{
        //    query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "'";
        //}
        SQLQuery.PopulateDropDown("Select LocationID, Name from Location " + query, ddNewLocationID, "LocationID", "Name");
    }


    protected void ddLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdCenterId();
        bindDDDepartmentSectionID();
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
            ddCenterID.Items.Insert(0, new ListItem("--- Select ---", "0"));
        }
    }
    private void BindDdNewCenterId()
    {
        string query = "";
        //if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        //{
        query = " WHERE LocationID = '" + ddNewLocationID.SelectedValue + "'";
        //}
        //else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        //{
        //    query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' ";
        //}

        string strQuery = "Select CenterID, Name from Center " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddNewCenterID, "CenterID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddNewCenterID.Items.Insert(0, new ListItem("--- Select ---", "0"));
        }
    }

    protected void ddCenterID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDDDepartmentSectionID();
    }


    private void bindDDDepartmentSectionID()
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
    private void bindDDNewDepartmentSectionID()
    {
        string query = "";
        //if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        //{
        query = " WHERE LocationID = '" + ddNewLocationID.SelectedValue + "' AND CenterID = '" + ddNewCenterID.SelectedValue + "' ";
        //}
        //else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        //{
        //    query = " WHERE LocationID = '" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";
        //}
        string strQuery = @"SELECT DepartmentSectionID, Name FROM DepartmentSection " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddNewDepartment, "DepartmentSectionID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddNewDepartment.Items.Insert(0, new ListItem("--- all ---", "0"));
        }
    }



    protected void ddDepartmentSectionID_SelectedIndexChanged(object sender, EventArgs e)
    {

        GridView1.DataBind();
    }

    private void ClearControls()
    {
        txtName.Text = "";
        //txtGender.Text = "";
        //ddlBloodGroup.SelectedValue = "";
        //ddlMarriedStatus.SelectedValue = "";

    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void ddEmployee_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        EditMode();
    }

    protected void ddNewLocationID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdNewCenterId();
        bindDDNewDepartmentSectionID();
    }

    protected void ddNewCenterID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDDNewDepartmentSectionID();
    }
}
