
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Office : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            BindDdLocationId();
            BindDdCenterId();
            bindDDDepartmentSectionID();
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
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO Office (Name, Description, CenterID, DepartmentSectionID) VALUES ('" + txtName.Text.Replace("'", "''") + "', '" + txtDescription.Text + "', '" + ddCenterID.SelectedValue + "', '" + ddDepartmentSectionID.SelectedValue + "')    ");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  Office SET Name= '" + txtName.Text.Replace("'", "''") + "',  Description= '" + txtDescription.Text + "',  CenterID= '" + ddCenterID.SelectedValue + "',  DepartmentSectionID= '" + ddDepartmentSectionID.SelectedValue + "' WHERE OfficeID='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select OfficeID, Name,Description,CenterID,DepartmentSectionID FROM Office WHERE OfficeID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtName.Text = dtx["Name"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();
                    ddCenterID.SelectedValue = dtx["CenterID"].ToString();
                    ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete Office WHERE OfficeID='" + lblId.Text + "' ");
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
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT Office.OfficeID, Office.Name, Office.Description, Center.Name AS CenterName, DepartmentSection.Name AS DepartmentName FROM Office INNER JOIN Center ON Office.CenterID = Center.CenterID INNER JOIN DepartmentSection ON Office.DepartmentSectionID = DepartmentSection.DepartmentSectionID AND Center.CenterID = DepartmentSection.CenterID WHERE Center.CenterID = '" + ddCenterID.SelectedValue + "' AND DepartmentSection.DepartmentSectionID = '" + ddDepartmentSectionID.SelectedValue + "'");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    private void BindDdLocationId()
    {
        SQLQuery.PopulateDropDown("Select Name,LocationID from Location", ddLocationID, "LocationID", "Name");
    }
    protected void ddLocationID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdCenterId();
        bindDDDepartmentSectionID();
        BindGrid();
    }


    private void BindDdCenterId()
    {
        SQLQuery.PopulateDropDown("Select CenterID, Name from Center WHERE LocationID = '" + ddLocationID.SelectedValue + "'", ddCenterID, "CenterID", "Name");
    }


    protected void ddCenterID_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindDDDepartmentSectionID();
        BindGrid();

    }


    private void bindDDDepartmentSectionID()
    {
        SQLQuery.PopulateDropDown("Select DepartmentSectionID, Name from DepartmentSection WHERE CenterID = '" + ddCenterID.SelectedValue + "'", ddDepartmentSectionID, "DepartmentSectionID", "Name");
    }


    protected void ddDepartmentSectionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }



    private void ClearControls()
    {
        txtName.Text = "";
        txtDescription.Text = "";

    }










}
