using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_ResetPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
    }
    protected void btnSave0_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUid.Text != "")
            {
                string lName = Page.User.Identity.Name.ToString();
                //string prjId = SQLQuery.ProjectID(Page.User.Identity.Name);
                string isExsist = SQLQuery.ReturnString("select LoginUserName FROM  Logins where LoginUserName='" + txtUid.Text + "'");
                if (isExsist != "" || lName == "rony")
                {
                    MembershipUser membershipUser = Membership.GetUser(txtUid.Text); //.GetUser(false);
                    lblCurrEmail.Text = membershipUser.Email;
                }
                else
                {
                    lblCurrEmail.Text = "You Typed an Invalid ID";
                }

            }
        }
        catch (Exception ex)
        {
            lblCurrEmail.Text = "You Typed an Invalid ID";
        }

    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            string username = txtUid.Text;
            string password = txtPassword.Text;
            MembershipUser mu = Membership.GetUser(username);
            mu.ChangePassword(mu.ResetPassword(), password);
            txtPassword.Text = "";
            lblMsg.Text = "Password Reset Successfull.";
            MessageBox("Password Reset Successfull.");
        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR: " + ex.ToString();
            MessageBox("Password Reset Failed!");
        }
    }
} 
