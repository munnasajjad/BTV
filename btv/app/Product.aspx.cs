
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Product : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            BindDdUnitId();
            ProductType();
            BindDdProductCategoryId();
            BindddProductSubCategory();
            BindGrid();
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void ProductType()
    {
        SQLQuery.PopulateDropDown("SELECT TypeId, Type FROM ProductType", ddProductType, "TypeId", "Type");
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
                    string isExist = SQLQuery.ReturnString("SELECT Name FROM Product WHERE Name='" + txtName.Text.Trim() + "' AND ProductCategoryID='"+ddProductCategoryID.SelectedValue+ "' AND ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "'");
                    if (isExist == "")
                    {
                        RunQuery.SQLQuery.ExecNonQry("INSERT INTO Product (Name, UnitID, ProductCategoryID, ProductSubCategoryID, Description, StockBookID, MinStockQty, MinOrderQTy, ProductType) VALUES (N'" + txtName.Text.Replace("'", "''") + "', '" + ddUnitID.SelectedValue + "', '" + ddProductCategoryID.SelectedValue + "', '" + ddProductSubCategory.SelectedValue + "', N'" + txtDescription.Text + "', '" + txtStockBookID.Text + "', '" + txtMinStockQty.Text + "', '" + txtMinOrderQTy.Text + "', '" + ddProductType.SelectedValue + "')    ");
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);
                    }
                    else
                    {
                        Notify("This product name already exists!", "warn", lblMsg);
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  Product SET Name= N'" + txtName.Text.Replace("'", "''") + "',  UnitID= '" + ddUnitID.SelectedValue + "',  ProductCategoryID= '" + ddProductCategoryID.SelectedValue + "',  ProductSubCategoryID= '" + txtProductSubCategoryID.Text + "',  Description= N'" + txtDescription.Text + "',  StockBookID= '" + txtStockBookID.Text + "',  MinStockQty= '" + txtMinStockQty.Text + "',  MinOrderQTy= '" + txtMinOrderQTy.Text + "',  ProductType= '" + ddProductType.SelectedValue + "' WHERE ProductID='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select ProductID, Name, UnitID, ProductCategoryID, ProductSubCategoryID, Description, StockBookID, MinStockQty, MinOrderQTy, ProductType FROM Product WHERE ProductID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtName.Text = dtx["Name"].ToString();
                    ddUnitID.SelectedValue = dtx["UnitID"].ToString();
                    ddProductCategoryID.SelectedValue = dtx["ProductCategoryID"].ToString();
                    txtProductSubCategoryID.Text = dtx["ProductSubCategoryID"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();
                    txtStockBookID.Text = dtx["StockBookID"].ToString();
                    txtMinStockQty.Text = dtx["MinStockQty"].ToString();
                    txtMinOrderQTy.Text = dtx["MinOrderQTy"].ToString();
                    ddProductType.SelectedValue = dtx["ProductType"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete Product WHERE ProductID='" + lblId.Text + "' ");
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
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT Product.ProductID, Product.Name,Product.Description, Unit.Name as UnitName , ProductCategory.Name as CategoryName, ProductSubCategory.Name as SubCategoryName, Product.MinOrderQTy FROM Product INNER JOIN Unit ON Product.UnitID = Unit.UnitID INNER JOIN ProductCategory ON Product.ProductCategoryID = ProductCategory.ProductCategoryID INNER JOIN ProductSubCategory ON Product.ProductSubCategoryID = ProductSubCategory.ProductSubCategoryID  WHERE Product.ProductCategoryID = '" + ddProductCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID = '" + ddProductSubCategory.SelectedValue + "' Order by ProductID Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    private void BindDdUnitId()
    {
        SQLQuery.PopulateDropDown("Select Name, UnitID from Unit", ddUnitID, "UnitID", "Name");
    }


    protected void ddUnitID_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }


    private void BindDdProductCategoryId()
    {
        SQLQuery.PopulateDropDown("Select Name,ProductCategoryID from ProductCategory", ddProductCategoryID, "ProductCategoryID", "Name");
    }
    private void BindddProductSubCategory()
    {
        SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddProductCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
    }


    protected void ddProductCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddProductSubCategory();
        BindGrid();
    }



    private void ClearControls()
    {
        txtName.Text = "";
        txtProductSubCategoryID.Text = "";
        txtDescription.Text = "";
        txtStockBookID.Text = "";
        txtMinStockQty.Text = "";
        txtMinOrderQTy.Text = "";

    }


    protected void ddProductSubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
}
