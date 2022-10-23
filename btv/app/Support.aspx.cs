using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Support : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["ProjectID"] = SQLQuery.ProjectID(User.Identity.Name);
        lblProject.Text = Session["ProjectID"].ToString();
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            lblUser.Text = Page.User.Identity.Name.ToString();
            lblProjectID.Text = SQLQuery.ProjectID(lblUser.Text);

            ddUsers.DataBind();
            LoadMsgBody();
            GridView1.DataBind();

        }

    }

    private void LoadMsgBody()
    {
        DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT TOP (1)  MsgID, Sender, Receiver, Subject, BodyText, IsRead, IsFlag, EntryDate, ProjectID
FROM            Messaging WHERE (MsgID = '" + ddUsers.SelectedValue + "')");

        foreach (DataRow drx in dtx.Rows)
        {
            txtSubject.Text = drx["Subject"].ToString();
            txtMsgBody.Content = drx["BodyText"].ToString();
            txtReceiver.Text = drx["Sender"].ToString();
        }
    }


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtSubject.Text.Length < 1)
            {
                Notify("Please write a subject", "error", lblMsg);
            }
            else if (txtMsgBody.Content.Length < 5)
            {
                Notify("Please write bigger message body", "error", lblMsg);
            }
            else
            {
                string receiversID = SQLQuery.ProjectID(txtReceiver.Text);
                SQLQuery.ExecNonQry("INSERT INTO Messaging (Sender, Receiver, Subject, BodyText, ProjectID) " +
                                    "VALUES  ('" + User.Identity.Name + "', '" + receiversID + "', '" + txtSubject.Text + "', '" + txtMsgBody.Content + "', '" + lblProjectID.Text + "')");
                string max = SQLQuery.ReturnString("Select MAX(MsgID) from Messaging");

                string linkPath = "./Docs/Messaging/";
                if (FileUpload2.HasFile)
                {
                    SQLQuery.UploadFile(ddUsers.SelectedValue + " - " + txtSubject.Text, FileUpload2, Server.MapPath(".\\Docs\\Messaging\\"), Server.MapPath(linkPath), linkPath, Page.User.Identity.Name.ToString(), "Messaging");
                    SQLQuery.ExecNonQry("UPDATE Photos SET MsgID='" + max + "' where PhotoID=(Select MAX(PhotoID) from Photos)");
                    //SQLQuery.ExecNonQry("UPDATE Photos SET MsgID='" + max + "' where PhotoID=(Select MAX(PhotoID) from Photos)");
                }
                GridView1.DataBind();
                txtSubject.Text = "";
                txtMsgBody.Content = "";
                Notify("Message successfully send!", "success", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }

    }

    protected void ddUsers_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LoadMsgBody();
        GridView1.DataBind();
    }
}

