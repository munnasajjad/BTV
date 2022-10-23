
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_DepartmentSection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            BindDdLocationId();
            BindDdCenterId();
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
        //string status = "";
        //if (rdIsDepartment.Checked)
        //{
        //    status = "Department";
        //}
        //else
        //{
        //    status = "Section";
        //}
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO DepartmentSection (Name, Description, CenterID,LocationID) VALUES (N'" + txtName.Text.Replace("'", "''") + "', N'" + txtDescription.Text + "', '" + ddCenterID.SelectedValue + "','" + ddLocationID.SelectedValue + "')    ");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  DepartmentSection SET Name= N'" + txtName.Text.Replace("'", "''") + "',  Description= N'" + txtDescription.Text + "',  CenterID= '" + ddCenterID.SelectedValue + "', LocationID= '" + ddLocationID.SelectedValue + "' WHERE DepartmentSectionID='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select DepartmentSectionID, Name,Description,CenterID,IsDepartment FROM DepartmentSection WHERE DepartmentSectionID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtName.Text = dtx["Name"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();
                    ddCenterID.SelectedValue = dtx["CenterID"].ToString();
                    //if (dtx["IsDepartment"].ToString() == "Department")
                    //{
                    //    rdIsDepartment.Checked = true;
                    //    rdIsSection.Checked = false;
                    //}
                    //else
                    //{
                    //    rdIsDepartment.Checked = false;
                    //    rdIsSection.Checked = true;
                    //}
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
                RunQuery.SQLQuery.ExecNonQry(" Delete DepartmentSection WHERE DepartmentSectionID='" + lblId.Text + "' ");
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
            Notify("ERROR:"+ex, "error", lblMsg);

        }
       
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT DepartmentSection.DepartmentSectionID, DepartmentSection.Name, DepartmentSection.IsDepartment, DepartmentSection.Description, Location.Name AS LocationName, Center.Name AS CenterName FROM DepartmentSection INNER JOIN Location ON DepartmentSection.LocationID = Location.LocationID INNER JOIN Center ON DepartmentSection.CenterID = Center.CenterID AND Location.LocationID = Center.LocationID WHERE DepartmentSection.LocationID = '" + ddLocationID.SelectedValue + "' AND DepartmentSection.CenterID = '" + ddCenterID.SelectedValue + "' Order by DepartmentSectionID Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    private void BindDdCenterId()
    {

        SQLQuery.PopulateDropDown("Select Name,CenterID from Center WHERE LocationID= '" + ddLocationID.SelectedValue + "'", ddCenterID, "CenterID", "Name");
    }
    private void BindDdLocationId()
    {

        string query = "";
        if (!User.IsInRole("Super Admin"))
        {
            query = "Where LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        SQLQuery.PopulateDropDown("Select Name,LocationID from Location "+query, ddLocationID, "LocationID", "Name");
    }


    protected void ddCenterID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }



    private void ClearControls()
    {
       
        txtName.Text = "";
        txtDescription.Text = "";

    }


    protected void ddLocationID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdCenterId();
        BindGrid();
    }
}
