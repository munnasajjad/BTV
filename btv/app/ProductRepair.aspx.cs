
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

public partial class AppProductRepair : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Enctype = "multipart/form-data";
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            BindStore();
            BindDdCategoryId();
            BindddProductSubCategory();
            BindDdProductId();
           
            txtRepairDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtRepairNo.Text = GenerateVoucherNumber.GetRepairNumber(Convert.ToDateTime(txtRepairDate.Text), User.Identity.Name, ddStore.SelectedValue);
            BindAddItemsGridView();
            BindGrid();
        }
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

        SQLQuery.PopulateDropDownWithoutSelect(query, ddStore, "StoreAssignID", "Name");
        if (ddStore.Text == "")
        {
            ddStore.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
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
        BindDdProductId();
    }
    protected void ddProductSubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdProductId();
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
                    else
                    {
                        Notify("Upload file is mandatory!", "warning", lblMsg);
                    }
                    SaveRepair();
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
                    if (document.HasFile)
                    {
                        string tExt = Path.GetExtension(document.FileName);
                        if (tExt != ".pdf")
                        {
                            Notify("Please upload the pdf file!", "warning", lblMsg);
                            return;
                        }
                    }
                    UpdateRepair();
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
            hdnRpID.Value = "";
            BindAddItemsGridView();
            BindGrid();
            txtRepairNo.Text = GenerateVoucherNumber.GetRepairNumber(Convert.ToDateTime(txtRepairDate.Text), User.Identity.Name, ddStore.SelectedValue);
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
                hdnRpID.Value = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable(" Select ProductRepairID, RepairNo, FinYear, LocationId, StoreId, RepairDate, Supplier, CauseOfRepair, DocumentUrl, EntryDate, Remarks, EntryBy FROM ProductRepair WHERE ProductRepairID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {

                    txtRepairNo.Text = dtx["RepairNo"].ToString();
                    txtRepairDate.Text = Convert.ToDateTime(dtx["RepairDate"].ToString()).ToString("dd/MM/yyyy");
                    txtSupplier.Text = dtx["Supplier"].ToString();
                    txtCauseOfRepair.Text = dtx["CauseOfRepair"].ToString();
                    //txtReceivedDate.Text = dtx["ReceivedDate"].ToString();
                    txtRemarks.Text = dtx["Remarks"].ToString();

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
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Delete") == "1")
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
                RunQuery.SQLQuery.ExecNonQry(" Delete ProductRepair WHERE ProductRepairID='" + lblId.Text + "' ");
                RunQuery.SQLQuery.ExecNonQry(" Delete ProductRepairDetails WHERE ProductRepairID='" + lblId.Text + "' ");
                BindAddItemsGridView();
                BindGrid();
                Notify("Successfully Deleted...", "success", lblMsg);
                txtRepairNo.Text = GenerateVoucherNumber.GetRepairNumber(Convert.ToDateTime(txtRepairDate.Text), User.Identity.Name, ddStore.SelectedValue);
            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "warn", lblMsg);
        }

    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {
        string query = "";
        if (!Page.User.IsInRole("Super Admin") || !Page.User.IsInRole("Senior Store Officer"))
        {
            query = "And ProductRepair.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND ProductRepair.StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        }

        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT ProductRepair.ProductRepairID, ProductRepair.LocationId, ProductRepair.StoreId, ProductRepair.DocumentUrl, CONVERT(varchar, ProductRepair.EntryDate, 103) AS EntryDate, CONVERT(varchar, ProductRepair.RepairDate, 103) 
                  AS RepairDate, ProductRepair.Remarks, ProductRepair.Supplier, ProductRepair.CauseOfRepair, ProductRepair.EntryBy, Location.Name AS LocationName, Store.Name AS StoreName, Store.StoreAssignID
                    FROM ProductRepair INNER JOIN Location ON ProductRepair.LocationId = Location.LocationID INNER JOIN
                  Store ON ProductRepair.StoreId = Store.StoreAssignID WHERE ProductRepair.StoreId='" + ddStore.SelectedValue + "'" + query + " Order by ProductRepair.EntryDate Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private void BindDdProductId()
    {
        SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID
                                FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddStore.SelectedValue + "' AND ProductDetails.Status='True'", ddProductID, "ProductDetailsID", "ProductName");
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

    private void ClearControls()
    {
        txtCauseOfRepair.Text = "";
        txtSupplier.Text = "";
        txtRemarks.Text = "";
    }
    private void UpdateRepair()
    {
        string updateQuery = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtRepairNo.Text + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/RP/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/RP/") + fileName);
                docUrl = "./Uploads/RP/" + fileName;
                updateQuery += "DocumentUrl=@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }
        SqlCommand command = new SqlCommand(@"Update  ProductRepair SET RepairDate=@RepairDate, RepairNo=@RepairNo, LocationID=@LocationID, FinYear=@FinYear,  StoreId=@StoreId,Supplier=@Supplier,CauseOfRepair=@CauseOfRepair," + updateQuery + "Remarks=@Remarks WHERE ProductRepairID='" + hdnRpID.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@RepairDate", Convert.ToDateTime(txtRepairDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRepairDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@StoreId", ddStore.SelectedValue);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@RepairNo", txtRepairNo.Text);
        command.Parameters.AddWithValue("@Supplier", txtSupplier.Text);
        command.Parameters.AddWithValue("@CauseOfRepair", txtCauseOfRepair.Text);
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.NVarChar).Value = docUrl;
        }
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

    }
    private void InsertToProduct()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        int lvNo;
        lvNo = hdnRpID.Value == "" ? 0 : Convert.ToInt32(hdnRpID.Value);

        command = new SqlCommand(@"INSERT INTO ProductRepairDetails (ProductRepairID, CategoryID, SubCategoryID, ProductID,ProductDetailsID,StoreId, Qty, EntryBy) 
                                       VALUES (@ProductRepairID, @CategoryID, @SubCategoryID, @ProductID,@ProductDetailsID, @StoreId,@Qty,@EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@ProductRepairID", lvNo);
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@ProductId", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@StoreId", ddStore.SelectedValue);
        command.Parameters.AddWithValue("@Qty", txtQty.Text);
        command.Parameters.AddWithValue("@CauseOfRepair", txtCauseOfRepair.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }
    private void BindAddItemsGridView()
    {
        string lName = Page.User.Identity.Name.ToString();

        string query = @"SELECT ProductRepairDetails.ProductRepairDetailsID, ProductRepairDetails.ProductID, ProductRepairDetails.Qty, CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductRepairDetails.ProductRepairID
                    FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                  ProductRepairDetails ON ProductDetails.ProductDetailsID = ProductRepairDetails.ProductDetailsID WHERE ProductRepairDetails.ProductRepairID = '" + hdnRpID.Value + "' AND ProductRepairDetails.EntryBy = '" + lName + "'";

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

        string query = @"UPDATE ProductRepairDetails SET CategoryID=@CategoryID,SubCategoryID=@SubCategoryID, ProductId=@ProductID, StoreId=@StoreId,Qty=@Qty WHERE ProductRepairDetailsID = '" + hdnProductRpId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@StoreId", ddStore.SelectedValue);
        // command.Parameters.AddWithValue("@Supplier", txtSupplier.Text);
        command.Parameters.AddWithValue("@Qty", txtQty.Text);
        //command.Parameters.AddWithValue("@CauseOfRepair", txtCauseOfRepair.Text);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string isProductExists = SQLQuery.ReturnString("SELECT ProductDetailsID FROM ProductRepairDetails WHERE ProductDetailsID = '" + ddProductID.SelectedValue + "' AND ProductRepairID='" + hdnRpID.Value + "' AND EntryBy = '" + lName + "'");

            if (btnAdd.Text == "ADD")
            {
                if (isProductExists != ddProductID.SelectedValue)
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        string productId = SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue);
                        string productType = SQLQuery.GetProductType(productId);
                        if (productType == "Non-Detail")
                        {
                            int availableQty = SQLQuery.GetAvailableQty(ddStore.SelectedValue, productId);

                            if (!(int.Parse(txtQty.Text) <= availableQty))
                            {
                                Notify("This item is stock out", "warn", lblMsg);
                                return;
                            }
                        }

                        string productStatus = "";
                        if (SQLQuery.IsAvailableProduct(ddProductID.SelectedValue, ddStore.SelectedValue, out productStatus))
                        {
                            InsertToProduct();
                            if (productType == "Detail")
                            {
                                SQLQuery.UpdateProductStatus("PR", ddProductID.SelectedValue);
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
                    ddProductID.SelectedValue = "0";
                    txtQty.Text = "";
                    txtQty.Enabled = true;
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
    private void SaveRepair()
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
                string fileName = file.Replace(file, "Document-" + txtRepairNo.Text.Trim() + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/RP/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/RP/") + fileName);
                docUrl = "./Uploads/RP/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";
            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }
        }
        SqlCommand command = new SqlCommand(@"INSERT INTO ProductRepair (" + insertQuery + @"RepairDate, RepairNo, LocationID, FinYear,  StoreId,Supplier,CauseOfRepair,  Remarks,  EntryBy)
                                            VALUES (" + parameter + "@RepairDate, @RepairNo, @LocationID, @FinYear, @StoreId,@Supplier,@CauseOfRepair,  @Remarks,  @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        string lName = Page.User.Identity.Name.ToString();
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
        }
        command.Parameters.AddWithValue("@RepairDate", Convert.ToDateTime(txtRepairDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRepairDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@StoreId", ddStore.SelectedValue);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@RepairNo", txtRepairNo.Text);
        command.Parameters.AddWithValue("@Supplier", txtSupplier.Text);
        command.Parameters.AddWithValue("@CauseOfRepair", txtCauseOfRepair.Text);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

        ClearControls();
        Notify("Successfully Saved...", "success", lblMsg);

        string query = "";
        if (hdnProductRpId.Value == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }
        string productRepairId = SQLQuery.ReturnString("SELECT MAX(ProductRepairID) AS ProductRepairID FROM ProductRepair WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE ProductRepairDetails SET ProductRepairID='" + productRepairId + "'  WHERE ProductRepairID = '" + hdnRpID.Value + "'" + query);
        DataTable dt = SQLQuery.ReturnDataTable("SELECT  ProductRepairDetailsID, ProductRepairID, CategoryID, SubCategoryID, ProductID,ProductDetailsID, StoreId, Qty, EntryDate, EntryBy FROM ProductRepairDetails Where ProductRepairID='" + productRepairId + "'");
        foreach (DataRow item in dt.Rows)
        {
            string rpNummber = SQLQuery.ReturnString("SELECT RepairNo FROM ProductRepair Where ProductRepairID='" + productRepairId + "'");
            string categoryId = item["CategoryID"].ToString();
            string subCategoryId = item["SubCategoryID"].ToString();
            string productId = item["ProductID"].ToString();
            string productDetailsId = item["ProductDetailsID"].ToString();
            string locationId = SQLQuery.GetLocationID(item["EntryBy"].ToString());
            string centerId = SQLQuery.GetCenterId(item["EntryBy"].ToString());
            string departmentId = SQLQuery.GetDepartmentSectionId(item["EntryBy"].ToString());
            string storeId = item["StoreId"].ToString();
            string qty = item["Qty"].ToString();
            Accounting.VoucherEntry.StockEntry(productRepairId, categoryId, subCategoryId, productId, productDetailsId, locationId, centerId, departmentId, storeId, "RP", "0", "0", "", qty, rpNummber, qty, "", item["EntryBy"].ToString(), "1");
            string productType = SQLQuery.GetProductType(productId);
            if (productType == "Detail")
            {
                SQLQuery.ExecNonQry("UPDATE ProductDetails SET Status='0' WHERE ProductDetailsID='" + productDetailsId + "'");
            }
        }
    }
    protected void AddItemsGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
        Label label = AddItemsGridView.Rows[index].FindControl("lblProductRepairDetailsID") as Label;
        hdnProductRpId.Value = label.Text;
        string query = @"SELECT ProductRepairDetailsID,Qty, ProductRepairID, CategoryID, SubCategoryID, ProductID,ProductDetailsID, StoreId, EntryBy FROM ProductRepairDetails WHERE ProductRepairDetailsID = '" + hdnProductRpId.Value + "'";
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
            BindDdProductId();
            ddProductID.SelectedValue = dataReader["ProductDetailsID"].ToString();
            txtQty.Text = dataReader["Qty"].ToString();

            string isDetail = SQLQuery.ReturnString(@"SELECT Product.ProductType FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')");
            txtQty.Enabled = isDetail != "1";
        }
        Notify("Edit mode activated ...", "info", lblMsg);
        dataReader.Close();
        command.Connection.Close();
    }
    protected void AddItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = AddItemsGridView.Rows[index].FindControl("lblProductRepairDetailsID") as Label;
        string productDetailsId = SQLQuery.ReturnString("SELECT ProductDetailsID FROM ProductRepairDetails WHERE(ProductRepairDetailsID = '" + lblId.Text + "')");
        SQLQuery.UpdateProductStatus("Available", productDetailsId);
        SQLQuery.ExecNonQry(" Delete FROM ProductRepairDetails WHERE ProductRepairDetailsID='" + lblId.Text + "' ");
        BindAddItemsGridView();
        Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
    }

    protected void ddStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRepairNo.Text = GenerateVoucherNumber.GetRepairNumber(Convert.ToDateTime(txtRepairDate.Text), User.Identity.Name, ddStore.SelectedValue);
        BindGrid();
    }

    protected void txtRepairDate_TextChanged(object sender, EventArgs e)
    {
        txtRepairNo.Text = GenerateVoucherNumber.GetRepairNumber(Convert.ToDateTime(txtRepairDate.Text), User.Identity.Name,ddStore.SelectedValue);
    }
    protected void txtRepairNo_TextChanged(object sender, EventArgs e)
    {
        if (txtRepairNo.Text.Length == 15)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRepairDate.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT RepairNo FROM ProductRepair WHERE RepairNo='" + txtRepairNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtRepairNo.Text + " Number already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("Repair Number should be 15 characters", "warn", lblMsg);
        }
    }
}
