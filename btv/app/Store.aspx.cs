
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Store : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            BindStore();
            bindDDLocationID();
            BindDdCenterId();
            bindDDDepartmentSectionID();
            bindDDOfficeID();
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
            string description = SQLQuery.ReturnString(@"Select Description FROM Store where StoreID='" + ddStore.SelectedValue + "'");
            
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {

                    if (txtName.Text == "")
                    {
                        RunQuery.SQLQuery.ExecNonQry("INSERT INTO Store (StoreID, Name, Description, LocationID,CenterID ,DepartmentSectionID, OfficeID) VALUES ('" + ddStore.SelectedValue + "','" + ddStore.SelectedItem.Text + "', N'" + description + "', '" + ddLocationID.SelectedValue + "','" + ddCenterID.SelectedValue + "', '" + ddDepartmentSectionID.SelectedValue + "', '" + ddOfficeID.SelectedValue + "')    ");
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);

                    }
                    else
                    {
                        string previousStoreID = SQLQuery.ReturnString(@"Select TOP (1) StoreAssignID FROM Store ORDER BY StoreAssignID DESC");
                        if (previousStoreID == "")
                        {
                            previousStoreID = Convert.ToString(0);
                        }
                        var storeID = Convert.ToInt32(previousStoreID) + Convert.ToInt32(1);
                        RunQuery.SQLQuery.ExecNonQry(" INSERT INTO Store (StoreID, Name, Description, LocationID,CenterID ,DepartmentSectionID, OfficeID) VALUES ('" + storeID + "',N'" + txtName.Text.Replace("'", "''") + "', N'" + txtDescription.Text + "', '" + ddLocationID.SelectedValue + "','" + ddCenterID.SelectedValue + "', '" + ddDepartmentSectionID.SelectedValue + "', '" + ddOfficeID.SelectedValue + "')    ");
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);
                    }

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
                   
                        RunQuery.SQLQuery.ExecNonQry(" Update  Store SET Name= '" + txtName.Text.Replace("'", "''") + "',  Description= '" + txtDescription.Text + "',  LocationID= '" + ddLocationID.SelectedValue + "', CenterID= '" + ddCenterID.SelectedValue + "', DepartmentSectionID= '" + ddDepartmentSectionID.SelectedValue + "',  OfficeID= '" + ddOfficeID.SelectedValue + "' WHERE StoreAssignID='" + lblId.Text + "' ");
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
            BindStore();
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
                DataTable dt = SQLQuery.ReturnDataTable("Select StoreAssignID, StoreID, Name, Description, LocationID, CenterID,DepartmentSectionID,OfficeID FROM Store WHERE StoreAssignID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    //ddStore.SelectedValue = dtx["StoreID"].ToString();
                    txtName.Text = dtx["Name"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();
                    ddLocationID.SelectedValue = dtx["LocationID"].ToString();
                    ddCenterID.SelectedValue = dtx["CenterID"].ToString();
                    ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();
                    ddOfficeID.SelectedValue = dtx["OfficeID"].ToString();

                }
                btnSave.Text = "Update";
                Notify("Edit mode activated ...", "info", lblMsg);
                btnStore.Visible = false;
                pnlNewStore.Visible = true;
                litStoreName.Text = "Edit Store";
                ddStore.Visible = false;


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
            RunQuery.SQLQuery.ExecNonQry(" Delete Store WHERE StoreAssignID='" + lblId.Text + "' ");
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
        //DataTable dt = SQLQuery.ReturnDataTable("SELECT Store.StoreAssignID,Store.StoreID, Store.Name, Store.Description, Location.Name AS LocationName, Center.Name AS CenterName, DepartmentSection.Name AS DepartmentName, Office.Name AS OfficeName FROM Store INNER JOIN Location ON Store.LocationID = Location.LocationID INNER JOIN Center ON Store.CenterID = Center.CenterID INNER JOIN DepartmentSection ON Store.DepartmentSectionID = DepartmentSection.DepartmentSectionID INNER JOIN Office ON Store.OfficeID = Office.OfficeID WHERE Store.LocationID = '" + ddLocationID.SelectedValue+ "' AND Store.CenterID = '"+ddCenterID.SelectedValue+"'");
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT Store.StoreAssignID, Store.StoreID, Store.Name, Store.Description, Location.Name AS LocationName, Center.Name AS CenterName, DepartmentSection.Name AS DepartmentName
                         FROM Store INNER JOIN
                         Location ON Store.LocationID = Location.LocationID INNER JOIN
                         Center ON Store.CenterID = Center.CenterID INNER JOIN
                         DepartmentSection ON Store.DepartmentSectionID = DepartmentSection.DepartmentSectionID WHERE Store.LocationID = '" + ddLocationID.SelectedValue+ "' AND Store.CenterID = '"+ddCenterID.SelectedValue+ "' Order by StoreAssignID Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void bindDDLocationID()
    {
        string query = "";
        if (!User.IsInRole("Super Admin"))
        {
            query = "Where LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        SQLQuery.PopulateDropDown("Select LocationID, Name from Location "+query, ddLocationID, "LocationID", "Name");
        BindDdCenterId();
    }
    private void BindStore()
    {
        SQLQuery.PopulateDropDown("SELECT DISTINCT StoreID, Name FROM Store", ddStore, "StoreID", "Name");
    }
    private void BindDdCenterId()
    {
        SQLQuery.PopulateDropDown("Select CenterID, Name from Center WHERE LocationID = '"+ddLocationID.SelectedValue+"'", ddCenterID, "CenterID", "Name");
        bindDDDepartmentSectionID();
    }


    protected void ddLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        BindDdCenterId();
        bindDDDepartmentSectionID();
        bindDDOfficeID();
       BindGrid();
    }


    private void bindDDDepartmentSectionID()
    {
        SQLQuery.PopulateDropDown("Select DepartmentSectionID, Name from DepartmentSection WHERE CenterID = '"+ddCenterID.SelectedValue+"'", ddDepartmentSectionID, "DepartmentSectionID", "Name");
        bindDDOfficeID();
    }


    protected void ddDepartmentSectionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        bindDDOfficeID();
        BindGrid();
    }


    private void bindDDOfficeID()
    {
        SQLQuery.PopulateDropDown("Select OfficeID, Name from Office WHERE DepartmentSectionID = '"+ddDepartmentSectionID.SelectedValue+"'", ddOfficeID, "OfficeID", "Name");
    }


    protected void ddOfficeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }



    private void ClearControls()
    {
        txtName.Text = "";
        txtDescription.Text = "";



    }


    //protected void linkBtnStore_OnClick(object sender, EventArgs e)
    //{
    //    if (linkBtnStore.Text == "Add New Store")
    //    {
    //        linkBtnStore.Text = "Existing Store";

    //        ddStore.Visible = false;
    //        litStoreName.Visible = false;
    //        txtName.Visible = true;
    //        txtDescription.Visible = true;
    //        txtName.Focus();

    //        pnlNewStore.Visible = true;
    //    }
    //    else
    //    {
    //        linkBtnStore.Text = "Add New Store";
    //        ddStore.Visible = true;
    //        litStoreName.Visible = true;
    //        //BindStore();
    //        txtName.Visible = false;
    //        txtDescription.Visible = false;
    //        ddStore.Focus();
    //        pnlNewStore.Visible = false;
    //    }
    //}

    protected void ddCenterID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        
        bindDDDepartmentSectionID();
        bindDDOfficeID();
        BindGrid();
    }

    protected void btnStore_OnClick(object sender, EventArgs e)
    {
        if (btnStore.Text == "Add New Store")
        {
            btnStore.Text = "Existing Store";

            ddStore.Visible = false;
            litStoreName.Visible = false;
            txtName.Visible = true;
            txtDescription.Visible = true;
            txtName.Focus();

            pnlNewStore.Visible = true;
        }
        else
        {
            btnStore.Text = "Add New Store";
            ddStore.Visible = true;
            litStoreName.Visible = true;
            //BindStore();
            txtName.Visible = false;
            txtDescription.Visible = false;
            ddStore.Focus();
            pnlNewStore.Visible = false;
        }
    }
}
