
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Designation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            bindDDRoleID();
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
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO Designation (Name, Description, RoleID, Priority) VALUES (N'" + txtName.Text + "', N'" + txtDescription.Text + "', '" + ddRoleID.SelectedValue + "', '" + txtPriority.Text + "')    ");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  Designation SET Name= N'" + txtName.Text + "',  Description= N'" + txtDescription.Text + "',  RoleID= '" + ddRoleID.SelectedValue + "',  Priority= '" + txtPriority.Text + "' WHERE DesignationID='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select DesignationID, Name,Description,RoleID,Priority FROM Designation WHERE DesignationID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtName.Text = dtx["Name"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();
                    ddRoleID.SelectedValue = dtx["RoleID"].ToString();
                    txtPriority.Text = dtx["Priority"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete Designation WHERE DesignationID='" + lblId.Text + "' ");
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
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM Designation");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    private void bindDDRoleID()
    {
        SQLQuery.PopulateDropDown("Select RoleID, Title from Role", ddRoleID, "RoleID", "Title");
    }


    protected void ddRoleID_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        txtName.Text = "";
        txtDescription.Text = "";
        txtPriority.Text = "";

    }

    protected void GridView1_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}
