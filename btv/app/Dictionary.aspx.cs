using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
using System.Data;

public partial class app_Dictionary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtName.Focus();
            string lName = Page.User.Identity.Name;
            if (lName == "rony")
            {
                //pnlLable.Visible = true;
            }
            else
            {
                //pnlLable.Visible = false;
            }
        }

    }
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
            if (txtName.Text != "" && txtDescription.Text != "")
            {
                int isCont = 0;
                string lName = Page.User.Identity.Name.ToString();
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    string isExists = SQLQuery.ReturnString("Select top(1) id from Dictionary where english='" + txtName.Text.Trim() + "' And ProjectId='" + SQLQuery.ProjectID(lName) + "'");

                    if (isExists == "")
                    {
                        SQLQuery.ExecNonQry(@"INSERT INTO [dbo].[Dictionary] ([english] ,[bangla], [EntryBy], ProjectId) VALUES ('" +
                        txtName.Text.Trim() + "', N'" + txtDescription.Text.Trim() + "', '" + Page.User.Identity.Name.ToString() + "','" + SQLQuery.ProjectID(User.Identity.Name) + "')");
                        Notify("Dictionary updated!", "info", lblMsg);
                    }
                    else
                    {
                        SQLQuery.ExecNonQry("UPDATE Dictionary SET english='" + txtName.Text.Trim() + "', bangla=N'" + txtDescription.Text.Trim() + "', UpdateBy='" + User.Identity.Name + "', UpdateDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE id='" + isExists + "' ");
                        Notify("Dictionary updated!", "info", lblMsg);
                    }

                }
                else
                {
                    Notify("You are not eligible to attempt this operation", "warn", lblMsg);
                }

                GridView1.DataBind();
                txtName.Text = "";
                txtDescription.Text = "";
                txtName.Focus();
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Error : Name field is Missing !";
                //lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff6230");
            }

        }
        catch (Exception ex)
        {
            lblMsg.Text = "Unable to Save: " + ex.ToString();
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
                Label name = GridView1.Rows[index].FindControl("Label1") as Label;

                DataTable dt = SQLQuery.ReturnDataTable("Select TOP(50) id, english, bangla from Dictionary WHERE id='" + name.Text + "' ");
                foreach (DataRow dr in dt.Rows)
                {
                    Session["id"] = dr["id"].ToString();
                    txtName.Text = dr["english"].ToString();
                    txtDescription.Text = dr["bangla"].ToString();
                }

                btnSave.Text = "Update";
                lblMsg.Attributes.Add("class", "xerp_info");
                lblMsg.Text = "Edit mode activated ...";
            }
            else
            {
                Notify("You are not eligible to attempt this operation", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        Label lblName = GridView1.Rows[index].FindControl("Label1") as Label;
        Label Label5 = GridView1.Rows[index].FindControl("Label5") as Label;

        if (SQLQuery.OparatePermission(Page.User.Identity.Name.ToString(), "Delete") == "1")
        {
            SQLQuery.ExecNonQry("Delete Dictionary where [id] ='" + lblName.Text + "' ");
            Notify(Label5.Text + " deleted from dictionary", "warn", lblMsg);
        }
        else
        {
            //lblMsg.Attributes.Add("class", "xerp_info");
            //lblMsg.Text = "You are not eligible to attempt this operation";
            Notify("You are not eligible to attempt this operation", "warn", lblMsg);
        }

    }

    protected void Button2_OnClick(object sender, EventArgs e)
    {
        GridView1.DataSource = SQLQuery.ReturnDataTable(@"SELECT TOP(50) id, english, bangla, EntryBy, ProjectId, EntryDate, UpdateBy, UpdateDate
FROM            Dictionary WHERE english like '%" + txtName.Text.Trim() + "%' ");
        GridView1.DataBind();
    }
    protected void Search(object sender, EventArgs e)
    {
        GridView1.DataSource = SQLQuery.ReturnDataTable(@"SELECT TOP(50) id, english, bangla, EntryBy, ProjectId, EntryDate, UpdateBy, UpdateDate
FROM            Dictionary WHERE bangla like N'%" + txtDescription.Text.Trim() + "%' ");
        GridView1.DataBind();
    }
}
