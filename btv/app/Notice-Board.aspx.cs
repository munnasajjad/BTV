using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Notice_Board : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        //txtDept.Text = "Drinks3";
        Session["ProjectID"] = SQLQuery.ProjectID(User.Identity.Name);
        lblProject.Text = Session["ProjectID"].ToString();
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            if (txtFullNews.Text != "")
            {
                if (btnSave.Text == "Save")
                {
                    ExecuteInsert();
                    Notify("Info Saved...", "success", lblMsg);
                }
                else
                {
                    SQLQuery.ExecNonQry("Update NewsUpdates set FullNews='" + txtFullNews.Text + "' where MsgID='" + lblId.Text + "'");

                    txtFullNews.Text = "";
                    btnSave.Text = "Save";
                    Notify("Info Updated...", "success", lblMsg);
                }
                GridView1.DataBind();
            }
            else
            {
                lblMsg.Text = "Please Select Test Name";
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }
    private void ExecuteInsert()
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            int prjId = Convert.ToInt32(SQLQuery.ProjectID(User.Identity.Name));
            cmd.Connection.Close();

            //string itemExist = SQLQuery.ReturnString("SELECT OrderNo FROM LighterQuantification WHERE OrderNo ='" + ddOrderNo.SelectedValue + "'");

            //if (itemExist != ddOrderNo.SelectedValue)
            //{
            SqlCommand cmd2 = new SqlCommand("INSERT INTO NewsUpdates (FullNews, ProjectID, EntryBy, Msgfor) VALUES (@FullNews, @ProjectID, @EntryBy, 'News')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            cmd2.Parameters.AddWithValue("@FullNews", txtFullNews.Text);
            cmd2.Parameters.AddWithValue("@ProjectID", prjId);
            cmd2.Parameters.AddWithValue("@EntryBy", lName);

            cmd2.Connection.Open();
            cmd2.ExecuteNonQuery();
            cmd2.Connection.Close();
            txtFullNews.Text = "";
            lblMsg.Text = "New Item Group Added Successfully";
            lblMsg.Attributes.Add("class", "xerp_success");
            //}
            //else
            //{
            //    lblMsg.Text = "ERROR: Info already exist!";
            //    lblMsg.Attributes.Add("class", "xerp_error");
            //}
        }
        catch (Exception ex)
        {
            lblMsg.Text = "ERROR: " + ex.ToString();
            lblMsg.Attributes.Add("class", "xerp_error");
        }
        finally
        {
            GridView1.DataBind();
        }

    }

    protected void btnClear_Click1(object sender, EventArgs e)
    {
        txtFullNews.Text = "";
        btnSave.Text = "Save";
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        //DropDownList1.SelectedValue = lblItemName.Text;
        EditMode(lblItemName.Text);

        lblMsg.Attributes.Add("class", "xerp_info");
        lblMsg.Text = "Edit mode activated ...";
    }

    private void EditMode(string id)
    {
        SqlCommand cmd7 = new SqlCommand("SELECT  FullNews FROM [NewsUpdates] WHERE MsgID='" + id + "' AND ProjrctId='" + SQLQuery.ProjectID(User.Identity.Name) + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            //EditField.Attributes.Add("class", "form-group");
            btnSave.Text = "Update";
            lblId.Text = id;

            txtFullNews.Text = dr[0].ToString();

            //ddCategory.SelectedValue = dr[2].ToString();
        }
        cmd7.Connection.Close();
    }

    protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            SqlCommand cmd7 = new SqlCommand("DELETE NewsUpdates WHERE MsgID=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();

            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Entry deleted successfully ...";

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
}  
