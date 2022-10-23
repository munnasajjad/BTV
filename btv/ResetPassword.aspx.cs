using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using System.Configuration.Provider;
using RunQuery;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class ResetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
    {
        SmtpClient mySmtpClient = new SmtpClient();
        mySmtpClient.EnableSsl = true;
        mySmtpClient.Send(e.Message);
        e.Cancel = true;

    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        // DropDownList ddlLocation = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddLocationID");
        TextBox txtUserName = (TextBox)PasswordRecovery1.FindControl("UserName");
        string lName = txtUserName.Text;
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
