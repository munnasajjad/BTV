using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Center : System.Web.UI.Page
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
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO Center (LocationID, Name, Address, Status,ContactNo) VALUES ('" + ddLocationID.SelectedValue + "', N'" + txtName.Text.Replace("'", "''") + "', N'" + txtAdress.Text + "', '1',N'" + txtContactNo.Text + "')    ");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  Center SET LocationID= '" + ddLocationID.SelectedValue + "',  Name= N'" + txtName.Text.Replace("'", "''") + "',  Address= N'" + txtAdress.Text + "',  ContactNo= N'" + txtContactNo.Text + "' WHERE CenterID='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select CenterID, LocationID,Name,Address,ContactNo FROM Center WHERE CenterID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    ddLocationID.SelectedValue = dtx["LocationID"].ToString();
                    txtName.Text = dtx["Name"].ToString();
                    txtAdress.Text = dtx["Address"].ToString();
                    txtContactNo.Text = dtx["ContactNo"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete Center WHERE CenterID='" + lblId.Text + "' ");
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
            query = "Where LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT Center.CenterID, Center.LocationID, Location.Name AS LocationName, Center.Name, Center.Address, Center.ContactNo
                        FROM Center INNER JOIN Location ON Center.LocationID = Location.LocationID WHERE Center.LocationID ='" + ddLocationID.SelectedValue + "' Order by CenterID Desc");
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
        txtAdress.Text = "";
        txtStatus.Text = "";
        txtContactNo.Text = "";

    }
}
