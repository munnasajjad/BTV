
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_DeadProductList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Enctype = "multipart/form-data";
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            BindStore();
            BindDdCategoryId();
            BindddProductSubCategory();
            bindDDProductID();
            BindAddItemsGridView();
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDeadNo.Text = GenerateVoucherNumber.GetDeadNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name,ddlStore.SelectedValue);
            BindGrid();
        }
    }
    private void BindDdCategoryId()
    {
        SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
    }
    private void BindddProductSubCategory()
    {
        SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
    }
    protected void ddCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddProductSubCategory();
        bindDDProductID();
        //txtUnitPrice.Text = GetPrice();
    }
    protected void ddProductSubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        bindDDProductID();
        //txtUnitPrice.Text = GetPrice();
    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void SaveDeadData()
    {
        string insertQuery = "";
        string parameter = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtDeadNo.Text.Trim() + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/Dead/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/Dead/") + fileName);
                docUrl = "./Uploads/Dead/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";
            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }

        SqlCommand command = new SqlCommand(@"INSERT INTO DeadProductList (" + insertQuery + @"Date, DeadNumber,ProductCondition, LocationID, FinYear,  StoreId, Remarks,  EntryBy)
                                            VALUES (" + parameter + "@Date, @DeadNumber,@ProductCondition, @LocationID, @FinYear, @StoreId, @Remarks,  @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        string lName = Page.User.Identity.Name.ToString();
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
        }
        command.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@ProductCondition", txtProductCondition.Text);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@DeadNumber", txtDeadNo.Text);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        string query = "";
        if (hdnProductId.Value == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }
        ClearControls();
        Notify("Successfully Saved...", "success", lblMsg);
        string deadProductID = SQLQuery.ReturnString("SELECT MAX(DeadProductID) AS DeadProductID FROM DeadProductList WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE DeadProductDetails SET DeadProductID='" + deadProductID + "'  WHERE DeadProductID = '" + hdnDeadId.Value + "'" + query);
        DataTable dt = SQLQuery.ReturnDataTable("SELECT DeadProductDetailsID, DeadProductID, CategoryID, SubCategoryID, ProductDetailsID, ProductID, StoreId,  EntryDate,Qty, EntryBy FROM DeadProductDetails Where DeadProductID='" + deadProductID + "'");
        foreach (DataRow item in dt.Rows)
        {
            string deadNummber = SQLQuery.ReturnString("SELECT DeadNumber FROM DeadProductList Where DeadProductID='" + deadProductID + "'");
            string categoryID = item["CategoryID"].ToString();
            string subCategoryID = item["SubCategoryID"].ToString();
            string productID = item["ProductID"].ToString();
            string productDetailsID = item["ProductDetailsID"].ToString();
            string locationID = SQLQuery.GetLocationID(item["EntryBy"].ToString());
            string centerId = SQLQuery.GetCenterId(item["EntryBy"].ToString());
            string departmentId = SQLQuery.GetDepartmentSectionId(item["EntryBy"].ToString());
            string storeID = item["StoreId"].ToString();
            string qty = item["Qty"].ToString();
            string productType = SQLQuery.GetProductType(productID);
            if (productType == "Detail")
            {
                SQLQuery.ExecNonQry("UPDATE ProductDetails SET Status='0' WHERE ProductDetailsID='" + productDetailsID + "'");
            }
            Accounting.VoucherEntry.StockEntry(deadProductID, categoryID, subCategoryID, productID, productDetailsID, locationID, centerId, departmentId, storeID, "Dead", "0", "0", "", qty, deadNummber, qty, "", item["EntryBy"].ToString(), "1");
        }


    }
    private void UpdateDeadData()
    {
        string updateQuery = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtDeadNo.Text + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/Dead/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/Dead/") + fileName);
                docUrl = "./Uploads/Dead/" + fileName;
                updateQuery += "DocumentUrl=@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }
        SqlCommand command = new SqlCommand(@"Update  DeadProductList SET Date=@Date,ProductCondition=@ProductCondition, DeadNumber=@DeadNumber, LocationID=@LocationID, FinYear=@FinYear,  StoreId=@StoreId," + updateQuery + "Remarks=@Remarks WHERE DeadProductID='" + hdnDeadId.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@ProductCondition", txtProductCondition.Text);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@DeadNumber", txtDeadNo.Text);
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.NVarChar).Value = docUrl;
        }
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

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
                    if (document.HasFile)
                    {
                        string tExt = Path.GetExtension(document.FileName);
                        if (tExt != ".pdf")
                        {
                            Notify("Please upload the pdf file!", "warning", lblMsg);
                            return;
                        }
                    }
                    SaveDeadData();
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
                    // RunQuery.SQLQuery.ExecNonQry(" Update  DeadProductList SET  Date= '" + txtDate.Text + "',  Remarks= '" + txtRemarks.Text + "',  ProductCondition= '" + txtProductCondition.Text + "' WHERE DeadProductID='" + lblId.Text + "' ");
                    if (document.HasFile)
                    {
                        string tExt = Path.GetExtension(document.FileName);
                        if (tExt != ".pdf")
                        {
                            Notify("Please upload the pdf file!", "warning", lblMsg);
                            return;
                        }
                    }
                    UpdateDeadData();
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
            hdnDeadId.Value = "";
            BindAddItemsGridView();
            BindGrid();
            txtDeadNo.Text = GenerateVoucherNumber.GetDeadNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name,ddlStore.SelectedValue);

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
                hdnDeadId.Value = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable(" Select DeadProductID,StoreId,DeadNumber, Date, Remarks, ProductCondition, EntryBy FROM DeadProductList WHERE DeadProductID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
                    txtRemarks.Text = dtx["Remarks"].ToString();
                    txtProductCondition.Text = dtx["ProductCondition"].ToString();
                    txtDeadNo.Text= dtx["DeadNumber"].ToString();
                    BindStore();
                    ddlStore.SelectedValue= dtx["StoreId"].ToString();

                }
                BindAddItemsGridView();
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
            RunQuery.SQLQuery.ExecNonQry(" Delete DeadProductList WHERE DeadProductID='" + lblId.Text + "' ");
            RunQuery.SQLQuery.ExecNonQry(" Delete DeadProductDetails WHERE DeadProductID='" + lblId.Text + "' ");
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
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = "And DeadProductList.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND DeadProductList.StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        }
        string sqlQuery = @"SELECT DeadProductList.DeadProductID, DeadProductList.DeadNumber, DeadProductList.FinYear, DeadProductList.DocumentUrl,CONVERT(varchar, DeadProductList.Date, 103) AS Date,  DeadProductList.Remarks, DeadProductList.ProductCondition, 
                  DeadProductList.EntryDate, DeadProductList.EntryBy, DeadProductList.LocationId, DeadProductList.StoreId, Store.Name AS StoreName, Location.Name AS MainOffice
FROM     Location INNER JOIN DeadProductList ON Location.LocationID = DeadProductList.LocationId INNER JOIN Store ON DeadProductList.StoreId = Store.StoreAssignID Where DeadProductList.StoreId='" + ddlStore.SelectedValue + "'" + query + " Order by DeadProductList.EntryDate Desc";
        DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);

        //DataTable dt = SQLQuery.ReturnDataTable(" SELECT DeadProductID,  CONVERT(varchar, Date, 103) AS Date, Remarks, ProductCondition, EntryBy FROM DeadProductList");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    private void bindDDProductID()
    {
        //SQLQuery.PopulateDropDown("SELECT  Product.Name+'-'+ProductDetails.SerialNo AS ProductName,ProductDetails.ProductDetailsID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddlStore.SelectedValue + "'", ddProductID, "ProductDetailsID", "ProductName");
        SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID
                                FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddlStore.SelectedValue + "' AND ProductDetails.Status='True'", ddProductID, "ProductDetailsID", "ProductName");
    }
    
    private void IsDetails(string id)
    {
        string isDetail = SQLQuery.ReturnString(@"SELECT Product.ProductType FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + id + "')");
        if (isDetail == "1")
        {
            txtQty.Text = "1";
            txtQty.Enabled = false;

        }
        else
        {
            txtQty.Text = "";
            txtQty.Enabled = true;

        }
    }
    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {
        IsDetails(ddProductID.SelectedValue);
    }
    private void BindStore(string query = "")
    {
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Senior Store Officer"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store";
        }
        else if (Page.User.IsInRole("Admin"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store WHERE (CenterID = '" + SQLQuery.GetCenterId(User.Identity.Name) + "')";
        }
        else
        {
            query = @"SELECT Store.StoreAssignID, Store.Name
            FROM Store INNER JOIN StoreAssign ON Store.StoreAssignID = StoreAssign.StoreID
            WHERE (StoreAssign.EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "')";
        }

        SQLQuery.PopulateDropDownWithoutSelect(query, ddlStore, "StoreAssignID", "Name");
        if (ddlStore.Text == "")
        {
            ddlStore.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string isProductExists = SQLQuery.ReturnString("SELECT ProductDetailsID FROM DeadProductDetails WHERE ProductDetailsID = '" + ddProductID.SelectedValue + "' AND DeadProductID='" + hdnDeadId.Value + "' AND EntryBy = '" + lName + "'");
            if (btnAdd.Text == "ADD")
            {
                if (isProductExists != ddProductID.SelectedValue)
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        string productID = SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue);
                        string productType = SQLQuery.GetProductType(productID);
                        if (productType == "Non-Detail")
                        {
                            int availableQty = SQLQuery.GetAvailableQty(ddlStore.SelectedValue, productID);

                            if (!(int.Parse(txtQty.Text) <= availableQty))
                            {
                                Notify("This item is stock out", "warn", lblMsg);
                                return;
                            }
                        }

                        string productStatus = "";
                        if (SQLQuery.IsAvailableProduct(ddProductID.SelectedValue, ddlStore.SelectedValue, out productStatus))
                        {
                            InsertToProduct();
                            if (productType == "Detail")
                            {
                                SQLQuery.UpdateProductStatus("Dead", ddProductID.SelectedValue);
                            }
                            ddProductID.SelectedValue = "0";
                            txtQty.Text = "";
                            txtQty.Enabled = true;
                            ddProductID.SelectedValue = "0";
                            Notify("Insert Successful", "info", lblMsg);
                            BindAddItemsGridView();
                        }
                        else
                        {
                            Notify("This product already added to " + productStatus + " voucher", "warn", lblMsg);
                        }

                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("This Product is already added!", "warn", lblMsg);
                }

            }
            else
            {
                if (SQLQuery.OparatePermission(lName, "Update") == "1")
                {
                    UpdateProduct();
                    ddCategoryID.SelectedValue = "0";
                    ddProductSubCategory.SelectedValue = "0";
                    ddProductID.SelectedValue = "0";
                    txtQty.Enabled = true;
                    txtQty.Text = "";
                    btnAdd.Text = "ADD";
                    Notify("Update Successful", "info", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }


            }
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "error", lblMsg);
        }
        finally
        {
            BindAddItemsGridView();
            BindGrid();
        }
    }

    protected void AddItemsGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
            Label label = AddItemsGridView.Rows[index].FindControl("lblDeadProductDetailsID") as Label;
            hdnProductId.Value = label.Text;
            string query = @"SELECT DeadProductDetailsID, DeadProductID, CategoryID, SubCategoryID,ProductDetailsID, EntryBy, EntryDate,Qty FROM DeadProductDetails WHERE DeadProductDetailsID = '" + hdnProductId.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                btnAdd.Text = "Update";
                BindDdCategoryId();
                ddCategoryID.SelectedValue = dataReader["CategoryID"].ToString();
                BindddProductSubCategory();
                ddProductSubCategory.SelectedValue = dataReader["SubCategoryID"].ToString();
                bindDDProductID();
                ddProductID.SelectedValue = dataReader["ProductDetailsID"].ToString();
                txtQty.Text = dataReader["Qty"].ToString();
                string isDetail = SQLQuery.ReturnString(@"SELECT Product.ProductType FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')");
                if (isDetail == "1")
                {
                    //txtQty.Text = "1";
                    txtQty.Enabled = false;
                }
                else
                {
                    //txtQty.Text = "";
                    txtQty.Enabled = true;
                }
            }
            Notify("Edit mode activated ...", "info", lblMsg);
            //ddProductID.Enabled = false;
            dataReader.Close();
            command.Connection.Close();
        }
        catch (Exception ex)
        {
            Notify("ERROR:" + ex, "error", lblMsg);

        }

    }

    protected void AddItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = AddItemsGridView.Rows[index].FindControl("lblDeadProductDetailsID") as Label;
            string productDetailsId = SQLQuery.ReturnString("SELECT ProductDetailsID FROM DeadProductDetails WHERE(DeadProductDetailsID = '" + lblId.Text + "')");
            SQLQuery.UpdateProductStatus("Available", productDetailsId);
            SQLQuery.ExecNonQry(" Delete FROM DeadProductDetails WHERE DeadProductDetailsID='" + lblId.Text + "' ");
            BindAddItemsGridView();
            Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
    }
    private void InsertToProduct()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        int lvNo;
        if (hdnDeadId.Value == "")
        {
            lvNo = 0;
        }
        else
        {
            lvNo = Convert.ToInt32(hdnDeadId.Value);
        }

        command = new SqlCommand(@"INSERT INTO DeadProductDetails (DeadProductID, CategoryID, SubCategoryID,ProductDetailsID,ProductId,StoreId,Qty, EntryBy) 
                                       VALUES (@DeadProductID,@CategoryID, @SubCategoryID,@ProductDetailsID, @ProductId,@StoreId, @Qty, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@DeadProductID", lvNo);
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@ProductId", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Qty", txtQty.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }
    private void BindAddItemsGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string query = @"SELECT  DeadProductDetails.DeadProductDetailsID, DeadProductDetails.DeadProductID, DeadProductDetails.CategoryID, DeadProductDetails.SubCategoryID, DeadProductDetails.ProductDetailsID, DeadProductDetails.ProductID, 
                  DeadProductDetails.StoreId, DeadProductDetails.Qty, DeadProductDetails.EntryBy, DeadProductDetails.EntryDate, CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName FROM DeadProductDetails INNER JOIN ProductDetails ON DeadProductDetails.ProductDetailsID = ProductDetails.ProductDetailsID INNER JOIN
                  Product ON ProductDetails.ProductID = Product.ProductID WHERE DeadProductDetails.DeadProductID = '" + hdnDeadId.Value + "' AND DeadProductDetails.EntryBy = '" + lName + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        AddItemsGridView.EmptyDataText = "No data added ...";
        AddItemsGridView.DataSource = command.ExecuteReader();
        AddItemsGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void UpdateProduct()
    {
        // string lName = Page.User.Identity.Name.ToString();
        string query = @"UPDATE DeadProductDetails SET CategoryID=@CategoryID,SubCategoryID=@SubCategoryID, ProductDetailsID=@ProductDetailsID,  ProductId=@ProductId,StoreId=@StoreId,Qty=@Qty WHERE DeadProductDetailsID = '" + hdnProductId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@ProductId", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Qty", txtQty.Text);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }

    private void ClearControls()
    {
        txtRemarks.Text = "";
        txtProductCondition.Text = "";
    }
    protected void txtDeadNo_TextChanged(object sender, EventArgs e)
    {
        if (txtDeadNo.Text.Length == 14)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT DeadNumber FROM DeadProductList WHERE DeadNumber='" + txtDeadNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtDeadNo.Text + " Dead already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("Dead Number should be 14 characters", "warn", lblMsg);
        }
    }
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        txtDeadNo.Text = GenerateVoucherNumber.GetDeadNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name,ddlStore.SelectedValue);
    }
    protected void ddlStore_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        txtDeadNo.Text = GenerateVoucherNumber.GetDeadNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, ddlStore.SelectedValue);
    }
}
