using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Unlock : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            lblUser.Text = Page.User.Identity.Name.ToString();
            lblProjectID.Text = SQLQuery.ProjectID(lblUser.Text);

            SQLQuery.PopulateDropDown(@"SELECT        aspnet_Users.UserName FROM            aspnet_Membership INNER JOIN
                         aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId WHERE        (aspnet_Membership.IsLockedOut = 'true') order by [UserName]", ddUsers, "UserName", "UserName");
            //checkUser();
        }
    }
    //Messagebox For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void checkUser()
    {
        MembershipUser membershipUser = Membership.GetUser(ddUsers.SelectedValue);      //.GetUser(false);
        try
        {
            if (ddUsers.SelectedValue != "")
            {
                bool IsApproved = membershipUser.IsApproved;
                bool IsLocked = membershipUser.IsLockedOut;
                if (IsApproved == true && IsLocked == true)
                {
                    //RadioButton2.Checked = false;
                    //RadioButton1.Checked = true;
                    btnSave.Text = "Unblock";
                    Notify("This user is blocked", "warn", lblStatus);

                }
                else
                {
                    //RadioButton1.Checked = false;
                    //RadioButton2.Checked = true;
                    btnSave.Text = "Block";
                    Notify("This user is not blocked", "info", lblStatus);
                }
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblStatus);
        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (RadioButton1.Checked == true)
            {
                SqlCommand cmd3 = new SqlCommand("update aspnet_Membership set failedpasswordattemptcount=0", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd3.Connection.Open();
                cmd3.ExecuteNonQuery();
                cmd3.Connection.Close();

                MembershipUser membershipUser = Membership.GetUser(ddUsers.SelectedValue); // Use (false); instead of (txtUid.Text) to get current user.
                membershipUser.UnlockUser();

                Notify("User Successfully Unblocked", "success", lblMsg);
            }
            else if (RadioButton2.Checked == true)
            {
                MembershipUser membershipUser = Membership.GetUser(ddUsers.SelectedValue); // Use (false); instead of (txtUid.Text) to get current user.
                LockUser(membershipUser);
                Notify("User is blocked now", "warn", lblMsg);
            }
            else
            {
                //SqlCommand cmd3x = new SqlCommand("update aspnet_Membership set islockedout='false', failedpasswordattemptcount=0", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                //cmd3x.Connection.Open();
                //cmd3x.ExecuteNonQuery();
                //cmd3x.Connection.Close();
                //MessageBox("All Locked Users Successfully Unlocked");
            }
            checkUser();
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblStatus);
        }
        finally
        {
            GridView1.DataBind();
        }
    }
    public static bool LockUser(MembershipUser user)
    {
        try
        {
            for (int i = 0; i < Membership.MaxInvalidPasswordAttempts; i++)
                Membership.ValidateUser(user.UserName, "thisisandummypasswordonlytolocktheuser");

            return user.IsLockedOut;
        }
        catch (Exception)
        {
            throw;
        }

    }

    protected void ddUsers_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        checkUser();
    }

    protected void RadioButton2_CheckedChanged(object sender, EventArgs e)
    {
        if (RadioButton2.Checked)
        {
            SQLQuery.PopulateDropDown(@"SELECT        aspnet_Users.UserName FROM            aspnet_Membership INNER JOIN
                         aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId WHERE        (aspnet_Membership.IsLockedOut = 'false') order by [UserName]", ddUsers, "UserName", "UserName");
        }
        else
        {
            SQLQuery.PopulateDropDown(@"SELECT        aspnet_Users.UserName FROM            aspnet_Membership INNER JOIN
                         aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId WHERE        (aspnet_Membership.IsLockedOut = 'true') order by [UserName]", ddUsers, "UserName", "UserName");
        }
        checkUser();
    }
}

