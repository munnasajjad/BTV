using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
using System.Configuration;
using System.Data;

public partial class app_CreateUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string query = "SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID > (Select UserLevel from logins where LoginUserName ='" + Page.User.Identity.Name.ToString() + "') ORDER BY [LevelID] ASC";
            //txtCurrentPosition.Text = SQLQuery.ReturnString("");   LevelID,    
            SQLQuery.PopulateDropDown(query, ddLevelX, "LevelID", "LevelName");
            DropDownList ddLevel = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLevel");
            SQLQuery.PopulateDropDown(query, ddLevel, "LevelID", "LevelName");
            bindDDLocationID();
            BindDdCenterId();
            bindDDDepartmentSectionID();
            BindEmployee();
            BindData();
        }
    }
    private void BindData()
    {
        string query = "";
        string centerName = SQLQuery.GetCenterName(SQLQuery.GetCenterId(User.Identity.Name));
        string departmentName = SQLQuery.GetDepartmentNameById(SQLQuery.GetDepartmentSectionId(User.Identity.Name));
        if (!Page.User.IsInRole("Super Admin"))
        {
            query = " AND Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        if(centerName!="Not Applicable")
        {
            query+= "AND Employee.CenterID='" + SQLQuery.GetCenterId(User.Identity.Name) + "'";

        }
        if (departmentName != "Not Applicable")
        {
            query += "AND Employee.DepartmentSectionID='" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";

        }
        string sql = @"SELECT Logins.LID, Logins.LoginUserName, Logins.EmployeeInfoID,
                      (SELECT LevelName FROM UserLevel WHERE(LevelID = Logins.UserLevel)) AS UserLevel, Employee.Name AS EmpName, Center.Name AS CenterName, DepartmentSection.Name AS DptName, Location.Name AS LocationName
                        FROM Logins INNER JOIN Employee ON Logins.EmployeeInfoID = Employee.EmployeeID INNER JOIN
                  Location ON Employee.LocationID = Location.LocationID INNER JOIN
                  Center ON Employee.CenterID = Center.CenterID INNER JOIN DepartmentSection ON Employee.DepartmentSectionID = DepartmentSection.DepartmentSectionID
                    WHERE(Logins.LoginUserName <> 'rony') "+query+"";
        DataTable dt = SQLQuery.ReturnDataTable(sql);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    private void bindDDLocationID()
    {
        DropDownList ddlLocation = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLocationID");
        string query = "";
        if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }

        SQLQuery.PopulateDropDown("Select LocationID, Name from Location " + query, ddlLocation, "LocationID", "Name");
    }

    //private void bindDDLocationID()
    //{
    //    string query = "";
    //    if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
    //    {
    //        query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' ";
    //    }

    //    DropDownList ddlLocation = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLocationID");

    //    SQLQuery.PopulateDropDown("Select LocationID, Name from Location "+ query, ddlLocation, "LocationID", "Name");
    //}
    private void BindDdCenterId()
    {
        DropDownList ddlLocation = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLocationID");
        DropDownList ddlCenter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCenterID");

        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddlLocation.SelectedValue + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' ";
        }

        string strQuery = "Select CenterID, Name from Center " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddlCenter, "CenterID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddlCenter.Items.Insert(0, new ListItem("--- all ---", "0"));
        }
    }
    private void bindDDDepartmentSectionID()
    {
        DropDownList ddlLocation = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLocationID");
        DropDownList ddlCenter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCenterID");
        DropDownList ddDepartment = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCounter");
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddlLocation.SelectedValue + "' AND CenterID = '" + ddlCenter.SelectedValue + "' ";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = " WHERE LocationID = '" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";
        }

        string strQuery = @"SELECT DepartmentSectionID, Name FROM DepartmentSection " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddDepartment, "DepartmentSectionID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddDepartment.Items.Insert(0, new ListItem("--- all ---", "0"));
        }
    }
    private void BindEmployee()
    {
        DropDownList ddlDepartment = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCounter");
        DropDownList ddlCenter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCenterID");
        DropDownList ddlLocation = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLocationID");
        DropDownList ddEmployee = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID='" + ddlLocation.SelectedValue + "' AND CenterID='" + ddlCenter.SelectedValue + "' AND DepartmentSectionID='" + ddlDepartment.SelectedValue + "'";
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

    //private void BindEmployee()
    //{
        
    //    DropDownList ddCounter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCounter");
    //    DropDownList ddlCenter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCenterID");
    //    DropDownList ddlLocation = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLocationID");
    //    DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
    //    SQLQuery.PopulateDropDown("SELECT [Name], EmployeeID FROM [Employee] WHERE DepartmentSectionID='"+ ddCounter.SelectedValue + "' AND LocationID='" + ddlLocation.SelectedValue + "' AND CenterID = '" + ddlCenter.SelectedValue + "' AND EmployeeID NOT IN (Select EmployeeInfoID from Logins) ORDER BY [Name]", ddEmploye, "EmployeeID", "Name");
    //}
    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {
        DropDownList ddLevel = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLevel");
        DropDownList ddCounter = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddCounter");
        DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        TextBox txtUser = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
        TextBox Email = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");

        Roles.AddUserToRole(CreateUserWizard1.UserName, ddLevel.SelectedItem.Text);
        //string isUserExists = SQLQuery.ReturnString("SELECT BranchName AS UserId FROM Users WHERE BranchName = '" + ddEmploye.SelectedValue + "'");
        //if(isUserExists != ddEmploye.SelectedValue)
        //{
        SQLQuery.ExecNonQry(@"INSERT INTO Users (Username, BranchName, ProjectId, LevelId)
                 VALUES ('" + txtUser.Text + "','" + ddEmploye.SelectedValue + "', '1', '" + ddLevel.SelectedValue + "')");

        SqlCommand cmd2c = new SqlCommand("INSERT INTO Logins (LoginUserName, EmployeeInfoID, UserLevel)" +
                               "VALUES ('" + txtUser.Text + "','" + ddEmploye.SelectedValue + "', '" + ddLevel.SelectedValue + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd2c.Connection.Open();
        cmd2c.ExecuteNonQuery();
        cmd2c.Connection.Close();

        //SQLQuery.ExecNonQry(@"UPDATE aspnet_Membership SET IsApproved='false' WHERE Email='"+Email.Text+"'");

        //Send Email:
        string emailBody = "Dear " + ddEmploye.SelectedItem.Text +
                           ", <br><br>your account has been registered in our system.<br><br>" +
                            ", <br><br>please confirm your email by click on the bellow link.<br><br>";
        string link = "http://" + Request.Url.Authority + Page.ResolveUrl("~/app/Confirm.aspx?ID=" + ddEmploye.SelectedValue + "&&email=" + Email.Text);
        emailBody += link + " <br><br>Regards, <br><br>Development Team.";

        SQLQuery.SendEmail2(Email.Text, "btvstoremanagementsystem@gmail.com", "Please confirm your email", emailBody);

        lblMsg.Text = "Please tell user to click the verify link send to his email ";
        ddEmploye.DataBind();
        GridView1.DataBind();
        //}
        //else
        //{
        //    lblMsg.Text = "This employee is already exists ...";
        //    return;
        //}
    }
    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
            lblid.Text = PID.Text;

            Panel1.Visible = true;
            pnlRegister.Visible = false;

            lblMsg.Attributes.Add("class", "isa_info");
            lblMsg.Text = "Edit mode activated ...";

            EditMode();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "isa_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT LoginUserName, EmployeeInfoID, UserLevel FROM [Logins] WHERE LID='" + lblid.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtUserX.Text = dr[0].ToString();
            string empid = dr[1].ToString();

            // string eid = SQLQuery.ReturnString("SELECT EmployeeID FROM [Employee] WHERE EmployeeID='" + dr[1].ToString() + "'");
            if (empid != "")
            {
                ddCounterX.DataBind();
                ddCounterX.SelectedValue = SQLQuery.ReturnString("SELECT DepartmentSectionID FROM [Employee] WHERE EmployeeID='" + dr[1].ToString() + "'");
                ddEmployeX.DataBind();
                ddEmployeX.SelectedValue = empid;


            }

            //eid = ; //SQLQuery.ReturnString("SELECT UserOparateLevel FROM [Counters] WHERE UserOparateLevel='" + dr[2].ToString() + "'");
            //if (eid != "")
            //{            
            ddLevelX.SelectedValue = dr[2].ToString();
            //}

        }
        cmd7.Connection.Close();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            string uName = SQLQuery.ReturnString("Select LoginUserName from Logins where lid='" + lblItemCode.Text + "' ");
            SQLQuery.ExecNonQry(@"SELECT UserId, RoleId into #temp
                                FROM  aspnet_UsersInRoles
                                WHERE UserId= (Select UserId from aspnet_Users where LoweredUserName='" + uName.ToLower() + "' )" +
                                @"DELETE FROM dbo.aspnet_Membership WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_UsersInRoles WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_Profile WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_Users WHERE UserId IN (Select UserId from #temp)");

            SQLQuery.ExecNonQry("DELETE Users WHERE Username='" + uName.ToLower() + "' ");
            SQLQuery.ExecNonQry("DELETE Logins WHERE LID=" + lblItemCode.Text);


            GridView1.DataBind();
            lblMsg.Attributes.Add("class", "isa_warning");
            lblMsg.Text = "Entry removed successfully ...";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "isa_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("CreateUser");
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        SQLQuery.ExecNonQry("Update Users set BranchName='" + ddEmployeX.SelectedValue + "', LevelId='" + ddLevelX.SelectedValue + "' where Username='" + txtUserX.Text + "' ");
        SQLQuery.ExecNonQry("Update Logins set EmployeeInfoID='" + ddEmployeX.SelectedValue + "', UserLevel='" + ddLevelX.SelectedValue + "' where LID='" + lblid.Text + "' ");

        SQLQuery.ExecNonQry("Update aspnet_UsersInRoles SET RoleId=(SELECT RoleId FROM UserLevel WHERE LevelID='" + ddLevelX.SelectedValue + "') WHERE UserId=(SELECT UserId FROM aspnet_Users WHERE LoweredUserName='" + txtUserX.Text + "')");
        lblMsg.Text = "Login info Updated Successfully";

        Panel1.Visible = false;
        pnlRegister.Visible = true;
    }

    protected void ddCounter_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        
        BindEmployee();
    }

    protected void ddCenterID_SelectedIndexChanged(object sender, EventArgs e)
    {

        
        bindDDDepartmentSectionID();
       
        BindEmployee();
    }

    protected void ddLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        BindDdCenterId();
        bindDDDepartmentSectionID();
       
        BindEmployee();
    }

    //protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    BindEmployee();
    //}



    protected void ddEmploye_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList dEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("dEmploye");
        //TextBox txtemail = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Email");

        //txtemail.Text = SQLQuery.ReturnString("SELECT Email Employee Where EmployeeID='" + dEmploye.SelectedValue + "'");

    }
}
