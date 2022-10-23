
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
using System.Configuration;

public partial class app_Station : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
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


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        string status = "";
        if (rdIsStation.Checked)
        {
            status = "Station";
        }
        else
        {
            status = "Sub Station";
        }
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                   
                        string query = "INSERT INTO Location(Name, Address, Status) VALUES(@Name, @Address, @Status)";
                        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@Name", txtName.Text);
                        command.Parameters.AddWithValue("@Address", txtAddress.Text);
                        command.Parameters.AddWithValue("@Status", ddOffice.SelectedValue);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        command.Connection.Close();
                        command.Connection.Dispose();
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
                    //RunQuery.SQLQuery.ExecNonQry(" Update  Location SET Name= '" + txtName.Text + "',  Address= '" + txtAddress.Text + "',  Status= '" + txtStatus.Text + "' WHERE LocationID='" + lblId.Text + "' ");
                    string query = "Update  Location SET Name=@Name,Address=@Address,Status=@Status WHERE LocationID='" + lblId.Text + "'";
                    SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Name", txtName.Text);
                    command.Parameters.AddWithValue("@Address", txtAddress.Text);
                    command.Parameters.AddWithValue("@Status", ddOffice.SelectedValue);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();
                    command.Connection.Dispose();
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select LocationID, Name,Address, Status FROM Location WHERE LocationID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtName.Text = dtx["Name"].ToString();
                    txtAddress.Text = dtx["Address"].ToString();
                    ddOffice.SelectedValue = dtx["Status"].ToString();
                    //if (dtx["Status"].ToString()=="Station")
                    //{
                    //    rdIsStation.Checked = true;
                    //    rdIsSubStation.Checked = false;
                    //}
                    //else
                    //{
                    //    rdIsStation.Checked = false;
                    //    rdIsSubStation.Checked = true;
                    //}
                    //rdIsStation.Checked = Convert.ToBoolean(dtx["Status"].ToString());

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
                RunQuery.SQLQuery.ExecNonQry(" Delete Location WHERE LocationID='" + lblId.Text + "' ");
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
            if (ex.Message.Contains("FK_"))
            {
                Notify("You can't delete this data","error", lblMsg);
            }
            else
            {
                Notify("ERROR: "+ex, "error", lblMsg);
            }
            

        }
        
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {
        string query = "";
        if (!User.IsInRole("Super Admin"))
        {
            query = "Where LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM Location "+query+" Order by LocationID Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        txtName.Text = "";
        txtAddress.Text = "";
       

    }











    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        this.BindGrid();
    }
}
