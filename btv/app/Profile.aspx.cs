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

public partial class app_Profile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        btnSave.Attributes.Add("onclick"," this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            LoadProfile();
            if (User.Identity.Name.ToLower() == "demo")
            {
                ChangePassword1.Enabled = false;
            }
        }
    }

    private void LoadProfile()
    {
        txtCode.Text = User.Identity.Name.ToString();
        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) EmployeeID, Name, DesignationID,(SELECT Name FROM Designation WHERE DesignationID=Employee.DesignationID) AS Designation, Gender, BloodGroup, MaterialStatus, Mobile, TelephoneOffice, Email, EduQualifiactionID, DOB, FatherName, NID, Relagion, PresentAddress, PermanentAddress, DateOfJoining, 
                         OfficeID, DepartmentSectionID, PhotoID, (Select '.'+PhotoURL from Photos where PhotoID=Employee.PhotoID) as Img
                          FROM Employee WHERE EmployeeID=(SELECT EmployeeInfoID FROM  Logins WHERE (LoginUserName= '" + txtCode.Text + "')) ");//AND ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'

        foreach (DataRow drx in dtx.Rows)
        {
            txtName.Text= drx["Name"].ToString();
            txtAddress.Text = drx["PresentAddress"].ToString();
            txtMobile.Text = drx["Mobile"].ToString();
            MakeReadOnly(txtEmail, drx["email"].ToString());
            MakeReadOnly(txtDesignation, drx["Designation"].ToString());
            //MakeReadOnly(txtDOB, Convert.ToDateTime(drx["DOB"].ToString()).ToString("dd/MM/yyyy"));

            if (drx["DOB"].ToString() != "")
            {
                DateTime dt = Convert.ToDateTime(drx["DOB"].ToString());
                txtDOB.Text = dt.ToString("dd/MM/yyyy");
                if (dt < DateTime.Now.AddYears(-15))
                {
                    txtDOB.ReadOnly = true;
                }
            }

            if (drx["PhotoID"].ToString() != "")
            {
                imgPhoto.ImageUrl = drx["Img"].ToString();
                if (drx["PhotoID"].ToString() != "1")
                {
                    //FileUpload2.Visible = false;
                }
            }
        }

    }

    private void MakeReadOnly(TextBox txtBx, string val)
    {
        txtBx.Text = val;
        if (val.Length > 1)
        {
            txtBx.ReadOnly = true;
        }
    }


    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');",
            true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = User.Identity.Name.ToString().ToLower();
            string dt = "1901-01-01";
            if (txtDOB.Text != "")
            {
                dt = Convert.ToDateTime(txtDOB.Text).ToString("yyyy-MM-dd");
            }

            SQLQuery.ExecNonQry("Update Employee Set Name='" + txtName.Text + "', PresentAddress='" + txtAddress.Text +
                                "', DOB='" + dt + "', Mobile='" + txtMobile.Text + "', email='" +
                                txtEmail.Text + "' WHERE EmployeeID=(SELECT EmployeeInfoID FROM  Logins WHERE (LoginUserName='" + lName + "'))");

            //string linkPath = "./Uploads/Photos/";
            if (FileUpload2.HasFile)
            {
                //string photoURL = SQLQuery.UploadPhoto(txtName.Text, FileUpload2, Server.MapPath("..\\Uploads\\Photos\\"), Server.MapPath(linkPath), linkPath, Page.User.Identity.Name.ToString(), "Users", 60, 60);
                string photoURL = RunQuery.SQLQuery.UploadImage(txtName.Text, FileUpload2, Server.MapPath("..\\Uploads\\Photos\\"), Server.MapPath("../Uploads/Photos/"), Page.User.Identity.Name.ToString(), "User");
                RunQuery.SQLQuery.ExecNonQry("UPDATE Employee SET PhotoID='" + photoURL + "' WHERE EmployeeID=(SELECT EmployeeInfoID FROM  Logins WHERE (LoginUserName='" + lName + "'))");
            }

            LoadProfile();
            Notify("Info Updated...", "success", lblMsg);


        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void ChangePasswordPushButton_Click(object sender, EventArgs e)
    {
        string lName = User.Identity.Name;
        string isActive = SQLQuery.ReturnString("SELECT IsActive FROM Users WHERE Username = '" + lName + "'");
        if (isActive == "False")
        {
            //Updating Login Datetime
            SqlCommand cmd = new SqlCommand("update Users set IsActive=@IsActive WHERE Username =@LName",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = "True";
            cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = lName;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        
    }
}
