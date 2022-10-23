
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_ProductSubCategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            bindDDCategoryID();
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
                    string isExist = SQLQuery.ReturnString("SELECT Name FROM ProductSubCategory WHERE Name='" + txtName.Text.Trim() + "'");
                    if (isExist == "")
                    {
                        RunQuery.SQLQuery.ExecNonQry(" INSERT INTO ProductSubCategory (Name, Description, CategoryID) VALUES (N'" + txtName.Text + "', N'" + txtDescription.Text + "', '" + ddCategoryID.SelectedValue + "')    ");
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);
                    }
                    else
                    {
                        Notify("This product sub category name already exists!", "warn", lblMsg);
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  ProductSubCategory SET Name= N'" + txtName.Text + "',  Description= N'" + txtDescription.Text + "',  CategoryID= '" + ddCategoryID.SelectedValue + "' WHERE ProductSubCategoryID='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select ProductSubCategoryID, Name,Description,CategoryID FROM ProductSubCategory WHERE ProductSubCategoryID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtName.Text = dtx["Name"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();
                    ddCategoryID.SelectedValue = dtx["CategoryID"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete ProductSubCategory WHERE ProductSubCategoryID='" + lblId.Text + "' ");
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
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT ProductSubCategory.ProductSubCategoryID, ProductSubCategory.Name, ProductSubCategory.Description, ProductCategory.Name AS ProductCategoryName FROM ProductSubCategory INNER JOIN ProductCategory ON ProductSubCategory.CategoryID = ProductCategory.ProductCategoryID WHERE ProductSubCategory.CategoryID='" + ddCategoryID.SelectedValue + "' Order by ProductSubCategoryID Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    private void bindDDCategoryID()
    {
        SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
    }


    protected void ddCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }



    private void ClearControls()
    {
        txtName.Text = "";
        txtDescription.Text = "";

    }










}
