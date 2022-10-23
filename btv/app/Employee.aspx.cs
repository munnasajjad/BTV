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

public partial class app_Employee : System.Web.UI.Page
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
            bindDDDesignationID();
            bindDDEduQualifiactionID();
            txtDOB.Text = DateTime.Now.AddYears(-18).ToString("dd/MM/yyyy");
            txtDateOfJoining.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindDDLocationID();
            BindDdCenterId();
            bindDDOfficeID();
            bindDDDepartmentSectionID();
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
                    if (IsMobileExist(txtMobile.Text))
                    {
                        RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO Employee (EmpId,Name, DesignationID, Gender, BloodGroup, MaterialStatus, Mobile, TelephoneOffice, Email, EduQualifiactionID, DOB, FatherName, NID, Relagion, PresentAddress, PermanentAddress, DateOfJoining, LocationID, CenterID, DepartmentSectionID)
                    VALUES ('" + txtEmpId.Text + "','" + txtName.Text + "', '" + ddDesignationID.SelectedValue + "', '" + ddlGender.SelectedValue + "', '" + ddlBloodGroup.SelectedValue + "', '" + ddlMarriedStatus.SelectedValue + "', '" + txtMobile.Text + "', '" + txtTelephoneOffice.Text + "', '" + txtEmail.Text + "', '" + ddEduQualifiactionID.SelectedValue + "', '" + Convert.ToDateTime(txtDOB.Text).ToString("yyyy-MM-dd") + "', '" + txtFatherName.Text + "', '" + txtNID.Text + "', '" + ddRelagion.SelectedValue + "', '" + txtPresentAddress.Text + "', '" + txtPermanentAddress.Text + "', '" + Convert.ToDateTime(txtDateOfJoining.Text).ToString("yyyy-MM-dd") + "', '" + ddLocationID.SelectedValue + "','" + ddCenterID.SelectedValue + "','" + ddDepartmentSectionID.SelectedValue + "')    ");
                        string empFKeyId = SQLQuery.ReturnString(@"SELECT ISNULL(MAX(EmployeeID),0) FROM Employee WHERE EmpId='" + txtEmpId.Text + "'");
                        AddUpdateDesignation(int.Parse(empFKeyId), txtEmpId.Text);
                        UpdateSignature(empFKeyId);
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);
                    }
                    else
                    {
                        Notify("This " + txtMobile + "mobile number is already exist.", "warn", lblMsg);
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
                    RunQuery.SQLQuery.ExecNonQry("Update  Employee SET EmpId='" + txtEmpId.Text + "', Name= '" + txtName.Text + "',  DesignationID= '" + ddDesignationID.SelectedValue + "',  Gender= '" + ddlGender.SelectedValue + "',  BloodGroup= '" + ddlBloodGroup.SelectedValue + "',  MaterialStatus= '" + ddlMarriedStatus.SelectedValue + "',  Mobile= '" + txtMobile.Text + "',  TelephoneOffice= '" + txtTelephoneOffice.Text + "',  Email= '" + txtEmail.Text + "',  EduQualifiactionID= '" + ddEduQualifiactionID.SelectedValue + "',  DOB= '" + Convert.ToDateTime(txtDOB.Text).ToString("yyyy-MM-dd") + "',  FatherName= '" + txtFatherName.Text + "',  NID= '" + txtNID.Text + "',  Relagion= '" + ddRelagion.SelectedValue + "',  PresentAddress= '" + txtPresentAddress.Text + "',  PermanentAddress= '" + txtPermanentAddress.Text + "',  DateOfJoining= '" + Convert.ToDateTime(txtDateOfJoining.Text).ToString("yyyy-MM-dd") + "',  LocationID= '" + ddLocationID.SelectedValue + "', CenterID= '" + ddCenterID.SelectedValue + "',  DepartmentSectionID= '" + ddDepartmentSectionID.SelectedValue + "' WHERE EmployeeID='" + lblId.Text + "' ");
                    AddUpdateDesignation(int.Parse(lblId.Text), txtEmpId.Text);
                    UpdateSignature(lblId.Text);
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

    private void AddUpdateDesignation(int empFKeyId, string empNumber)
    {
        SqlCommand cmd2 = new SqlCommand();
        string isExist = SQLQuery.ReturnString(@"SELECT DesignationID FROM DesignationWithEmployee WHERE EmployeeID='" + empFKeyId + "' AND EntryFrom='First Joining'");
        if (isExist == "")
        {
            cmd2 = new SqlCommand("INSERT INTO DesignationWithEmployee (MainOfficeID, FunctionalOfficeID, DepartmentID, EmployeeID, DesignationID, Status, EntryFrom) VALUES(@MainOfficeID, @FunctionalOfficeID, @DepartmentID, @EmployeeID, @DesignationID, @Status, 'First Joining')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        else
        {
            cmd2 = new SqlCommand("UPDATE DesignationWithEmployee SET MainOfficeID=@MainOfficeID, FunctionalOfficeID=@FunctionalOfficeID, DepartmentID=@DepartmentID,EmployeeID=@EmployeeID, DesignationID=@DesignationID,Status=@Status WHERE (EmployeeID = '" + empFKeyId + "') AND EntryFrom='Employee'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        cmd2.Parameters.AddWithValue("@MainOfficeID", ddLocationID.SelectedValue);
        cmd2.Parameters.AddWithValue("@FunctionalOfficeID", ddCenterID.SelectedValue);
        cmd2.Parameters.AddWithValue("@DepartmentID", ddDepartmentSectionID.SelectedValue);
        cmd2.Parameters.AddWithValue("@EmployeeID", empFKeyId);
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
                DataTable dt = SQLQuery.ReturnDataTable(@"Select EmployeeID,EmpId, Name,DesignationID, Gender,BloodGroup,MaterialStatus,Mobile,TelephoneOffice,Email,EduQualifiactionID,DOB,FatherName,NID,Relagion,PresentAddress,PermanentAddress,DateOfJoining, LocationID, CenterID, DepartmentSectionID,StoreID,OfficeID FROM Employee WHERE EmployeeID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtEmpId.Text = dtx["EmpId"].ToString();
                    txtName.Text = dtx["Name"].ToString();
                    ddDesignationID.SelectedValue = dtx["DesignationID"].ToString();
                    ddlGender.SelectedValue = dtx["Gender"].ToString();
                    ddlBloodGroup.SelectedValue = dtx["BloodGroup"].ToString();
                    ddlMarriedStatus.SelectedValue = dtx["MaterialStatus"].ToString();
                    txtMobile.Text = dtx["Mobile"].ToString();
                    txtTelephoneOffice.Text = dtx["TelephoneOffice"].ToString();
                    txtEmail.Text = dtx["Email"].ToString();
                    ddEduQualifiactionID.SelectedValue = dtx["EduQualifiactionID"].ToString();
                    txtDOB.Text = dtx["DOB"].ToString();
                    txtFatherName.Text = dtx["FatherName"].ToString();
                    txtNID.Text = dtx["NID"].ToString();
                    ddRelagion.SelectedValue = dtx["Relagion"].ToString();
                    txtPresentAddress.Text = dtx["PresentAddress"].ToString();
                    txtPermanentAddress.Text = dtx["PermanentAddress"].ToString();
                    txtDateOfJoining.Text = dtx["DateOfJoining"].ToString();

                    ddLocationID.SelectedValue = dtx["LocationID"].ToString();
                    BindDdCenterId();
                    ddCenterID.SelectedValue = dtx["CenterID"].ToString();
                    bindDDDepartmentSectionID();
                    ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();
                    //ddOfficeID.SelectedValue = dtx["OfficeID"].ToString();


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
            query = " AND Employee.LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        //else if (Page.User.IsInRole("Department Admin"))
        //{
        //    query = " WHERE Employee.LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND Employee.CenterID='" + RunQuery.SQLQuery.GetCenterID(User.Identity.Name) + "' ";
        //}
        string searchQry = txtSearch.Text.Trim().Replace("'", "''");

        if (txtSearch.Text != "")
        {
            query += " AND (Employee.Name like '%" + searchQry + "%' OR Employee.Mobile like '%" + searchQry +
                       "%' OR Employee.Email like '%" + searchQry + "%')";
        }


        dt = SQLQuery.ReturnDataTable(@"SELECT Employee.EmployeeID, Employee.EmpId, Employee.Name, Employee.Mobile, Employee.Email, Employee.NID, Employee.DateOfJoining, EducationQualification.Name AS Education, Designation.Name AS Designation, Employee.Gender, 
                  DepartmentSection.Name AS Department, Location.Name AS MainOffice, Center.Name AS FunctionalOffice
FROM Employee INNER JOIN
                  Designation ON Employee.DesignationID = Designation.DesignationID INNER JOIN
                  EducationQualification ON Employee.EduQualifiactionID = EducationQualification.EduQualifiactionID INNER JOIN
                  Location ON Employee.LocationID = Location.LocationID LEFT OUTER JOIN
                  Center ON Employee.CenterID = Center.CenterID LEFT OUTER JOIN
                  DepartmentSection ON Employee.DepartmentSectionID = DepartmentSection.DepartmentSectionID WHERE Employee.EmployeeID<>0" + query + " ORDER BY Designation.Priority");

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


    private void bindDDDesignationID()
    {
        SQLQuery.PopulateDropDown("Select DesignationID, Name from Designation", ddDesignationID, "DesignationID", "Name");
    }


    protected void ddDesignationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    private void bindDDEduQualifiactionID()
    {
        SQLQuery.PopulateDropDown("Select EduQualifiactionID, Name from EducationQualification", ddEduQualifiactionID, "EduQualifiactionID", "Name");
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



    protected void ddCenterID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDDDepartmentSectionID();
    }

    private void bindDDOfficeID()
    {
        SQLQuery.PopulateDropDownWithAll("Select OfficeID, Name from Office", ddOfficeID, "OfficeID", "Name");
    }

    protected void ddOfficeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
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
        txtMobile.Text = "";
        txtTelephoneOffice.Text = "";
        txtEmail.Text = "";
        txtDOB.Text = "";
        txtFatherName.Text = "";
        txtNID.Text = "";
        txtPresentAddress.Text = "";
        txtPermanentAddress.Text = "";
        txtDateOfJoining.Text = "";

    }
    private void UpdateSignature(string empFKeyId)
    {
        string lName = Page.User.Identity.Name.ToString();
        string linkPath = "./Docs/Employee/Signature";
        if (FileUpload1.HasFile)
        {
            string photoUrl = SQLQuery.UploadImage(txtName.Text, FileUpload1, Server.MapPath(".\\Docs\\Employee\\Signature\\"), Server.MapPath(linkPath), linkPath, lName, "Signature");
            SQLQuery.ExecNonQry("UPDATE Employee SET SignatureID='" + photoUrl + "' WHERE EmployeeID='" + empFKeyId + "'");
            string path = SQLQuery.ReturnString("SELECT PhotoURL FROM Photos WHERE PhotoID=(SELECT SignatureID FROM Employee WHERE EmployeeID='" + empFKeyId + "')");
            FileStream fs = new FileStream(Server.MapPath(path), System.IO.FileMode.Open, System.IO.FileAccess.Read);
            byte[] image = new byte[fs.Length];
            fs.Read(image, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            SqlCommand cmd7 = new SqlCommand("UPDATE Employee SET Signature=@Photo WHERE EmployeeID='" + empFKeyId + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Parameters.Add("@Photo", SqlDbType.Image).Value = image;
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();
            cmd7.Connection.Dispose();
        }
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

    protected void txtMobile_OnTextChanged(object sender, EventArgs e)
    {
        if (!IsMobileExist(txtMobile.Text))
        {
            Notify("This " + txtMobile.Text + " mobile number is already exist.", "warn", lblMsg);
        }
    }
}
