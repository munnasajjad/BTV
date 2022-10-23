using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;


public partial class app_MainOfficeLocation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            BindDdLocationId();
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
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO MainOfficeLocation (MainOfficeId, MainOfficeLocationName, EntryBy) VALUES ('" + ddLocationID.SelectedValue + "', N'" + txtName.Text.Replace("'", "''") + "', '" + lName + "')    ");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  MainOfficeLocation SET MainOfficeId= '" + ddLocationID.SelectedValue + "',  MainOfficeLocationName= N'" + txtName.Text.Replace("'", "''") + "' WHERE Id='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select Id, MainOfficeId, MainOfficeLocationName FROM MainOfficeLocation WHERE Id='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    ddLocationID.SelectedValue = dtx["MainOfficeId"].ToString();
                    txtName.Text = dtx["MainOfficeLocationName"].ToString();
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
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ExecNonQry(" Delete MainOfficeLocation WHERE Id='" + lblId.Text + "' ");
            BindGrid();
            Notify("Successfully Deleted...", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
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
            query = "Where MainOfficeId='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT MainOfficeLocation.Id, MainOfficeLocation.MainOfficeId, Location.Name AS LocationName, MainOfficeLocation.MainOfficeLocationName
                        FROM MainOfficeLocation INNER JOIN Location ON MainOfficeLocation.MainOfficeId = Location.LocationID WHERE MainOfficeLocation.MainOfficeId ='" + ddLocationID.SelectedValue + "' ORDER BY Id DESC");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    private void BindDdLocationId()
    {
        string query = "";
        if (!User.IsInRole("Super Admin"))
        {
            query = "Where LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        SQLQuery.PopulateDropDown("Select Name,LocationID from Location " + query, ddLocationID, "LocationID", "Name");
    }
    protected void ddLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    private void ClearControls()
    {
        //ddLocationID.SelectedValue = "0";
        txtName.Text = "";
    }
}
