
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

public partial class app_ProductDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            BindDdGrnNo();
            BindDdProductId();
            CheckQuentity();
            //BindStore();
            BindAddItemsGridView();
            // txtGuarantyWarrantyPeriod.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtGuarantyPeriod.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtManufactureDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            BindCountry();
            BindManufactureCountry();
            BindSaveItemGridView();
        }
    }

    private void BindCountry()
    {
        SQLQuery.PopulateDropDownWithoutSelect("SELECT id, country, Flag, ShowOnBoard FROM Countries", ddlCountry, "id", "country");
        ddlCountry.Items.Insert(0, new ListItem("---Select---", "0"));

    }
    private void BindManufactureCountry()
    {
        SQLQuery.PopulateDropDownWithoutSelect("SELECT id, country, Flag, ShowOnBoard FROM Countries", ddlManufactureCountry, "id", "country");
        ddlManufactureCountry.Items.Insert(0, new ListItem("---Select---", "0"));

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

                //SQLQuery.ExecNonQry("UPDATE ProductDetails SET  WHERE GrnNO='" + ddGrnNO.SelectedValue + "' ");                
                BindSaveItemGridView();
                AddItemsGridView.DataSource = null;

                Notify("Successfully Saved...", "success", lblMsg);

            }
            else
            {
                if (SQLQuery.OparatePermission(lName, "Update") == "1")
                {
                    //RunQuery.SQLQuery.ExecNonQry(" Update ProductDetails SET StoreID= '" + ddStore.SelectedValue + "' WHERE GrnNO='" + lblId.Text + "' ");
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
            BindSaveItemGridView();
            BindAddItemsGridView();
        }
    }

    protected void SaveItemGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                int index = Convert.ToInt32(SaveItemGridView.SelectedIndex);
                Label lblEditId = SaveItemGridView.Rows[index].FindControl("lblGrnNO") as Label;
                lblId.Text = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable(" Select GrnNO FROM ProductDetails WHERE GrnNO='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {

                    ddGrnNO.SelectedValue = dtx["GrnNO"].ToString();
                }
                BindItemsGridViewForEdit();
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

    protected void SaveItemGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = SaveItemGridView.Rows[index].FindControl("lblGrnNO") as Label;
            RunQuery.SQLQuery.ExecNonQry(" Delete ProductDetails WHERE GrnNO='" + lblId.Text + "' ");
            BindSaveItemGridView();
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

    private void BindSaveItemGridView()
    {
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT DISTINCT GrnNO, GRNFrom.GRNInvoiceNo AS GRNInvoiceNo, Store.Name FROM ProductDetails  INNER JOIN GRNFrom ON ProductDetails.GrnNO = GRNFrom.IDGrnNO INNER JOIN Store ON ProductDetails.StoreID  = Store.StoreID");
        SaveItemGridView.DataSource = dt;
        SaveItemGridView.DataBind();
    }


    private void BindDdGrnNo()
    {
        SQLQuery.PopulateDropDown("Select IDGrnNO, GRNInvoiceNo from GRNFrom WHERE StoreID IN (SELECT StoreID FROM StoreAssign WHERE  (EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "')) AND SaveMode<>'Submitted' Order By IDGrnNO DESC", ddGrnNO, "IDGrnNO", "GRNInvoiceNo");
    }
    //private void BindStore()
    //{
    //    SQLQuery.PopulateDropDown("SELECT DISTINCT StoreID, Name FROM Store", ddStore, "StoreID", "Name");
    //}


    protected void ddGrnNO_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindDdProductId();
            BindAddItemsGridView();
            CheckQuentity();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }

    }


    private void BindDdProductId()
    {
        SQLQuery.PopulateDropDown("SELECT GRNProduct.ProductID AS ProductID, Product.Name AS ProductName FROM  GRNProduct INNER JOIN Product ON GRNProduct.ProductID = Product.ProductID WHERE GRNProduct.GrnFormID ='" + ddGrnNO.SelectedValue + "' AND Product.ProductType='1'", ddProductID, "ProductID", "ProductName");

    }

    private int CheckQuentity()
    {
        int qty = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(SUM(ReceiveProduct),0) AS QTY FROM GRNProduct WHERE ProductID ='" + ddProductID.SelectedValue + "' AND GRNProduct.GrnFormID = '" + ddGrnNO.SelectedValue + "'"));
        int qty1 = Convert.ToInt32(SQLQuery.ReturnString("SELECT IsNull(Count(ProductDetailsID),0) AS qty FROM ProductDetails  where GrnNO = '" + ddGrnNO.SelectedValue + "' AND ProductID = '" + ddProductID.SelectedValue + "'"));

        txtItemQuantity.Text = (qty - qty1).ToString();
        return (qty - qty1);
    }

    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAddItemsGridView();
        CheckQuentity();
        //txtItemQuantity.Text = SQLQuery.ReturnString("SELECT  ReceiveProduct FROM GRNProduct WHERE ProductID ='"+ddProductID.SelectedValue+ "' AND GRNInvoiceNo = '"+ddGrnNO.SelectedItem.Text+"'");
    }

    private void ClearControls()
    {
        txtProductConditionStatus.Text = "";
        txtProductStatus.Text = "";
        //txtManufactureDate.Text = "";
        txtPartNo.Text = "";
        txtSerialNo.Text = "";
        txtModelNo.Text = "";
        txtBrand.Text = "";
        //txtManufacturingCompany.Text = "";
        //txtWarrantyPeriod.Text = "";
        txtGuarantyPeriod.Text = "";

    }



    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        int totalItmes = CheckQuentity();
        //int totalItmes =Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(SUM(ReceiveProduct),0) FROM GRNProduct WHERE GRNInvoiceNo='" + ddGrnNO.SelectedItem.Text + "'"));

        int gridItmes = Convert.ToInt32(SQLQuery.ReturnString("SELECT ISNULL(COUNT(ProductDetailsID),0) FROM ProductDetails WHERE GrnNO='" + ddGrnNO.SelectedValue + "' AND ProductID='" + ddProductID.SelectedValue + "'"));

        {
            if (SQLQuery.OparatePermission(lName, "Insert") == "1")
            {
                if (btnAdd.Text == "ADD")
                {
                    if (ProductSerialCheck(ddGrnNO.SelectedValue, ddProductID.SelectedValue))
                    {
                        if (totalItmes > 0)
                        {
                            InsertToProductDetails();
                            ClearControls();
                            BindAddItemsGridView();
                            CheckQuentity();
                            Notify("Successfully Saved...", "success", lblMsg);
                        }
                        else
                        {
                            Notify("All Products has been added!", "warn", lblMsg);
                        }

                    }
                    else
                    {
                        Notify("This serial No is already exists!", "warn", lblMsg);
                    }
                }
                else
                {
                    UpdateProductDetails();
                    btnAdd.Text = "ADD";
                    ClearControls();
                    BindAddItemsGridView();
                    Notify("Successfully Updated...", "success", lblMsg);
                }



            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }
        }





    }

    private bool ProductSerialCheck(string grnId, string productId)
    {
        bool serialStatus = true;
        string productSerial = SQLQuery.ReturnString(@"SELECT SerialNo FROM ProductDetails WHERE GrnNO='" + grnId + "' AND ProductID='" + productId + "' AND SerialNo='" + txtSerialNo.Text + "' ");
        if (productSerial != "")
        {
            serialStatus = false;
        }
        return serialStatus;
    }

    private void InsertToProductDetails()
    {

        string lName = Page.User.Identity.Name.ToString();
        string insertQuery = "";
        string parameter = "";
        if (txtManufactureDate.Text != "")
        {
            insertQuery = "ManufactureDate,";
            parameter = "@ManufactureDate,";
        }

        string serialNo = "(Not Available)";
        if (txtSerialNo.Text != "")
        {
            serialNo = txtSerialNo.Text;
        }
        string query = "INSERT INTO ProductDetails (GrnNO, ProductID, ProductConditionStatus, ProductStatus, CountryOfOrigin, ManufacturingCompany, " + insertQuery + " PartNo, SerialNo, ModelNo,Brand, StoreID, EntryBy, EntryDate) VALUES (@GrnNO, @ProductID, 'Good', 'Available',@CountryOfOrigin, @ManufacturingCompany, " + parameter + " @PartNo, @SerialNo, @ModelNo,@Brand, @StoreID,  @EntryBy, @EntryDate)";


        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Parameters.AddWithValue("@GrnNO", ddGrnNO.SelectedValue);
        command.Parameters.AddWithValue("@ProductID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@CountryOfOrigin", ddlCountry.SelectedValue);
        command.Parameters.AddWithValue("@ManufacturingCompany", ddlManufactureCountry.SelectedValue);
        if (txtManufactureDate.Text != "")
        {
            command.Parameters.AddWithValue("@ManufactureDate", Convert.ToDateTime(txtManufactureDate.Text));

        }
        command.Parameters.AddWithValue("@PartNo", txtPartNo.Text);
        command.Parameters.AddWithValue("@SerialNo", serialNo);
        command.Parameters.AddWithValue("@ModelNo", txtModelNo.Text);
        command.Parameters.AddWithValue("@Brand", txtBrand.Text);
        command.Parameters.AddWithValue("@StoreID", SQLQuery.ReturnString("SELECT StoreID FROM GRNFrom WHERE(IDGrnNO = '" + ddGrnNO.SelectedValue + "')"));
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }
    private void UpdateProductDetails()
    {

        // string updateQuery = "";

        //if (txtManufactureDate.Text != "")
        //{
        // updateQuery = "ManufactureDate=@ManufactureDate,";

        //}
        string serialNo = "(Not Available)";
        if (txtSerialNo.Text != "")
        {
            serialNo = txtSerialNo.Text;
        }
        string lName = Page.User.Identity.Name.ToString();
        string query = @"UPDATE ProductDetails SET CountryOfOrigin=@CountryOfOrigin,ManufacturingCompany=@ManufacturingCompany, ManufactureDate=@ManufactureDate, PartNo=@PartNo, SerialNo=@SerialNo, ModelNo=@ModelNo,Brand=@Brand,  EntryBy=@EntryBy, EntryDate=@EntryDate WHERE ProductDetailsID = '" + idHiddenField.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@CountryOfOrigin", ddlCountry.SelectedValue);
        command.Parameters.AddWithValue("@ManufacturingCompany", ddlManufactureCountry.SelectedValue);
        if (txtManufactureDate.Text != "")
        {
            command.Parameters.AddWithValue("@ManufactureDate", Convert.ToDateTime(txtManufactureDate.Text));

        }
        else
        {
            command.Parameters.AddWithValue("@ManufactureDate", DBNull.Value);
        }
        command.Parameters.AddWithValue("@PartNo", txtPartNo.Text);
        command.Parameters.AddWithValue("@SerialNo", serialNo);
        command.Parameters.AddWithValue("@ModelNo", txtModelNo.Text);
        command.Parameters.AddWithValue("@Brand", txtBrand.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindItemsGridViewForEdit()
    {
        string query = @"SELECT ProductDetails.ProductDetailsID, CONVERT(varchar, ProductDetails.ManufactureDate, 103) AS ManufactureDate, ProductDetails.SerialNo, Product.Name AS ProductName, GRNFrom.GRNInvoiceNo, 
                         CountriesOfOrigin.country AS CountriesOfOrigin, ManufacturingCompany.country AS ManufacturingCompany
                         FROM ProductDetails INNER JOIN
                         Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                         GRNFrom ON ProductDetails.GrnNO = GRNFrom.IDGrnNO LEFT OUTER JOIN
                         Countries AS CountriesOfOrigin ON ProductDetails.CountryOfOrigin = CountriesOfOrigin.id LEFT OUTER JOIN
                         Countries AS ManufacturingCompany ON ProductDetails.ManufacturingCompany = ManufacturingCompany.id WHERE GrnNO = '" + lblId.Text + "'";


        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        AddItemsGridView.EmptyDataText = "No data added ...";
        AddItemsGridView.DataSource = command.ExecuteReader();
        AddItemsGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }

    private void BindAddItemsGridView()
    {
        string query = @"SELECT ProductDetails.ProductDetailsID, CONVERT(varchar, ProductDetails.ManufactureDate, 103) AS ManufactureDate, ProductDetails.SerialNo, ProductDetails.ModelNo, ProductDetails.Brand, ProductDetails.PartNo, 
                         Product.Name AS ProductName, GRNFrom.GRNInvoiceNo, CountriesOfOrigin.country AS CountriesOfOrigin, ManufacturingCompany.country AS ManufacturingCompany
                         FROM ProductDetails INNER JOIN
                         Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                         GRNFrom ON ProductDetails.GrnNO = GRNFrom.IDGrnNO LEFT OUTER JOIN
                         Countries AS CountriesOfOrigin ON ProductDetails.CountryOfOrigin = CountriesOfOrigin.id LEFT OUTER JOIN
                         Countries AS ManufacturingCompany ON ProductDetails.ManufacturingCompany = ManufacturingCompany.id WHERE GrnNO = '" + ddGrnNO.SelectedValue + "'";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        AddItemsGridView.EmptyDataText = "No data added ...";
        AddItemsGridView.DataSource = command.ExecuteReader();
        AddItemsGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }

    protected void AddItemsGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
        Label label = AddItemsGridView.Rows[index].FindControl("lblProductDetailsID") as Label;
        idHiddenField.Value = label.Text;
        string query = @"SELECT ProductDetailsID, GrnNO, ProductID, Brand,ProductConditionStatus, ProductStatus, CountryOfOrigin, ManufacturingCompany, convert(varchar, ProductDetails.ManufactureDate, 103) AS ManufactureDate, PartNo, SerialNo, ModelNo, IsGuaranty, GuarantyWarrantyPeriod, StoreID FROM ProductDetails WHERE ProductDetailsID = '" + idHiddenField.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            btnAdd.Text = "Update";

            ddProductID.SelectedValue = dataReader["ProductID"].ToString();
            ddlCountry.SelectedValue = dataReader["CountryOfOrigin"].ToString();
            ddlManufactureCountry.SelectedValue = dataReader["ManufacturingCompany"].ToString();
            if (dataReader["ManufactureDate"].ToString() != "")
            {
                txtManufactureDate.Text = Convert.ToDateTime(dataReader["ManufactureDate"]).ToString("dd/MM/yyyy");
            }
            else
            {
                txtManufactureDate.Text = "";
            }
            txtPartNo.Text = dataReader["PartNo"].ToString();
            if (dataReader["SerialNo"].ToString() == "(Not Available)")
            {
                txtSerialNo.Text = "";
            }
            else
            {
                txtSerialNo.Text = dataReader["SerialNo"].ToString();
            }

            txtModelNo.Text = dataReader["ModelNo"].ToString();
            txtBrand.Text = dataReader["Brand"].ToString();
            CheckQuentity();
            //txtGuarantyWarrantyPeriod.Text = Convert.ToDateTime(dataReader["GuarantyWarrantyPeriod"]).ToString("dd/MM/yyyy");
            //cbIsGuaranty.Checked = Convert.ToBoolean(dataReader["IsGuaranty"].ToString());


        }
        dataReader.Close();
        command.Connection.Close();
    }

    protected void AddItemsGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = AddItemsGridView.Rows[index].FindControl("lblProductDetailsID") as Label;
            SQLQuery.ExecNonQry(" Delete ProductDetails FROM ProductDetails WHERE ProductDetailsID='" + lblId.Text + "' ");
            BindAddItemsGridView();
            CheckQuentity();
            Notify("Successfully Deleted...", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }
}
