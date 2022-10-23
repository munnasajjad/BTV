
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

public partial class app_AuctionList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Enctype = "multipart/form-data";
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            BindStore();
            bindDDCategoryID();
            BindddProductSubCategory();
            bindDDProductID();
            txtAuctionDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtAuctionNo.Text = GenerateVoucherNumber.GetAuctionNumber(Convert.ToDateTime(txtAuctionDate.Text), User.Identity.Name, ddlStore.SelectedValue);
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

        SQLQuery.PopulateDropDownWithoutSelect(query, ddlStore, "StoreAssignID", "Name");
        if (ddlStore.Text == "")
        {
            ddlStore.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }
    private void bindDDProductID()
    {
        SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID
                                FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddlStore.SelectedValue + "' AND ProductDetails.Status='True'", ddProductID, "ProductDetailsID", "ProductName");
    }


    private void bindDDCategoryID()
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
        txtUnitPrice.Text = GetPrice();
    }
    protected void ddProductSubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        bindDDProductID();
        txtUnitPrice.Text = GetPrice();
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
                    if (document.HasFile)
                    {
                        string tExt = Path.GetExtension(document.FileName);
                        if (tExt != ".pdf")
                        {
                            Notify("Please upload the pdf file!", "warning", lblMsg);
                            return;
                        }
                    }
                    SaveAuction();
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
                    UpdateAuction();
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
            hdnAuctionID.Value = "";
            BindAddItemsGridView();
            BindGrid();
            txtAuctionNo.Text = GenerateVoucherNumber.GetAuctionNumber(Convert.ToDateTime(txtAuctionDate.Text), User.Identity.Name, ddlStore.SelectedValue);

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
                //lblId.Text = lblEditId.Text;
                hdnAuctionID.Value = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable(" SELECT AuctionID, AuctionNo, LocationId, StoreId, FinYear, DocumentUrl, AuctionDate, Remark, EntryDate, EntryBy FROM AuctionList WHERE AuctionID='" + hdnAuctionID.Value + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtAuctionDate.Text = Convert.ToDateTime(dtx["AuctionDate"]).ToString("dd/MM/yyyy");
                    txtRemark.Text = dtx["Remark"].ToString();
                    txtAuctionNo.Text = dtx["AuctionNo"].ToString();
                    BindStore();
                    ddlStore.SelectedValue = dtx["StoreId"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete AuctionList WHERE AuctionID='" + lblId.Text + "' ");
            RunQuery.SQLQuery.ExecNonQry(" Delete AuctionDetails WHERE AuctionDetailsID='" + lblId.Text + "' "); BindGrid();
            BindAddItemsGridView();
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
            query = "And AuctionList.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND AuctionList.StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        }
        string sqlQuery = @"SELECT AuctionList.AuctionID, AuctionList.AuctionNo, AuctionList.LocationId, AuctionList.StoreId, AuctionList.FinYear, AuctionList.DocumentUrl, CONVERT(varchar, AuctionList.AuctionDate, 103) AS Date, AuctionList.Remark, 
                  AuctionList.EntryDate, AuctionList.EntryBy, Store.Name AS StoreName, Location.Name AS MainOfficeName
                FROM AuctionList INNER JOIN Store ON AuctionList.StoreId = Store.StoreAssignID INNER JOIN
                  Location ON AuctionList.LocationId = Location.LocationID Where AuctionList.StoreId='" + ddlStore.SelectedValue + "'" + query + " Order by AuctionList.EntryDate Desc";
        DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    //private void bindDDProductID()
    //{
    //    SQLQuery.PopulateDropDown("Select ProductID from Product", ddProductID, "ProductID", "ProductID");
    //}
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
    private string GetPrice()
    {
        string price = "";
        if (ddProductID.SelectedValue != "0")
        {
            price = SQLQuery.ReturnString(@"SELECT  GRNProduct.UnitPrice FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                  GRNProduct ON Product.ProductID = GRNProduct.ProductID Where ProductDetails.ProductDetailsID='" + ddProductID.SelectedValue + "'");
        }

        return price;
    }
    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtUnitPrice.Text = GetPrice();
        IsDetails(ddProductID.SelectedValue);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            //string lName = Page.User.Identity.Name.ToString();
            //string isProductExists = SQLQuery.ReturnString("SELECT SerialNo FROM AuctionDetails WHERE SerialNo = '" + ddProductID.SelectedValue + "'");

            string lName = Page.User.Identity.Name.ToString();
            string isProductExists = SQLQuery.ReturnString("SELECT ProductDetailsID FROM AuctionDetails WHERE ProductDetailsID = '" + ddProductID.SelectedValue + "' AND AuctionID='" + hdnAuctionID.Value + "' AND EntryBy = '" + lName + "'");
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
                            int availableQty = SQLQuery.GetAvailableQty(ddlStore.SelectedValue, productId);

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
                                SQLQuery.UpdateProductStatus("Auction", ddProductID.SelectedValue);
                            }
                            ddProductID.SelectedValue = "0";
                            txtQty.Text = "";
                            txtQty.Enabled = true;
                            ddProductID.SelectedValue = "0";
                            txtUnitPrice.Text = "";
                            txtAuctionValue.Text = "";
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
                    //ddCategoryID.SelectedValue = "0";
                    //ddProductSubCategory.SelectedValue = "0";
                    ddProductID.SelectedValue = "0";
                    txtUnitPrice.Text = "";
                    txtQty.Text = "";
                    txtAuctionValue.Text = "";
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

    protected void AddItemsGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
            Label label = AddItemsGridView.Rows[index].FindControl("lblAuctionDetailsID") as Label;
            hdnProductId.Value = label.Text;
            string query = @"SELECT AuctionDetailsID, AuctionID,Qty, CategoryID, SubCategoryID, ProductDetailsID, ProductID, StoreId, Price, AuctionValue, EntryDate, EntryBy FROM AuctionDetails WHERE AuctionDetailsID = '" + hdnProductId.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                btnAdd.Text = "Update";
                bindDDCategoryID();
                ddCategoryID.SelectedValue = dataReader["CategoryID"].ToString();
                BindddProductSubCategory();
                ddProductSubCategory.SelectedValue = dataReader["SubCategoryID"].ToString();
                bindDDProductID();
                ddProductID.SelectedValue = dataReader["ProductDetailsID"].ToString();
                txtUnitPrice.Text = dataReader["Price"].ToString();
                txtQty.Text = dataReader["Qty"].ToString();
                txtAuctionValue.Text = dataReader["AuctionValue"].ToString();
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
        Label lblId = AddItemsGridView.Rows[index].FindControl("lblAuctionDetailsID") as Label;
        string productDetailsId = SQLQuery.ReturnString("SELECT ProductDetailsID FROM AuctionDetails WHERE(AuctionDetailsID = '" + lblId.Text + "')");
        SQLQuery.UpdateProductStatus("Available", productDetailsId);
        SQLQuery.ExecNonQry(" Delete FROM AuctionDetails WHERE AuctionDetailsID='" + lblId.Text + "' ");
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
        if (hdnAuctionID.Value == "")
        {
            lvNo = 0;
        }
        else
        {
            lvNo = Convert.ToInt32(hdnAuctionID.Value);
        }

        command = new SqlCommand(@"INSERT INTO AuctionDetails (AuctionID, CategoryID, SubCategoryID, ProductID,ProductDetailsID,AuctionValue,Price,StoreId, Qty, EntryBy) 
                                       VALUES (@AuctionID, @CategoryID, @SubCategoryID, @ProductID,@ProductDetailsID,@AuctionValue,@Price, @StoreId,@Qty,@EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@AuctionID", lvNo);
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@ProductId", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@AuctionValue", txtAuctionValue.Text);
        command.Parameters.AddWithValue("@Price", txtUnitPrice.Text);
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
        string query = @"SELECT AuctionDetails.AuctionDetailsID, AuctionDetails.ProductID,AuctionDetails.Price,AuctionDetails.AuctionValue, AuctionDetails.Qty, CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, AuctionDetails.AuctionID
                    FROM AuctionDetails INNER JOIN Product ON AuctionDetails.ProductID = Product.ProductID INNER JOIN
                  ProductDetails ON ProductDetails.ProductDetailsID = AuctionDetails.ProductDetailsID WHERE AuctionDetails.AuctionID = '" + hdnAuctionID.Value + "' AND AuctionDetails.EntryBy = '" + lName + "'";
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
        string query = @"UPDATE AuctionDetails SET CategoryID=@CategoryID,SubCategoryID=@SubCategoryID, ProductDetailsID=@ProductDetailsID,  AuctionValue=@AuctionValue,StoreId=@StoreId,Price=@Price,Qty=@Qty WHERE AuctionDetailsID = '" + hdnProductId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@ProductId", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@AuctionValue", txtAuctionValue.Text);
        command.Parameters.AddWithValue("@Price", txtUnitPrice.Text);
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Qty", txtQty.Text);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void ClearControls()
    {
        //txtQty.Text = "";
        txtUnitPrice.Text = "";
        //txtTotalPrice.Text = "";
        //txtDate.Text = "";
        txtRemark.Text = "";

    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAuctionNo.Text = GenerateVoucherNumber.GetAuctionNumber(Convert.ToDateTime(txtAuctionDate.Text), User.Identity.Name, ddlStore.SelectedValue);
        BindGrid();
    }

    protected void txtAuctionDate_TextChanged(object sender, EventArgs e)
    {
        txtAuctionNo.Text = GenerateVoucherNumber.GetAuctionNumber(Convert.ToDateTime(txtAuctionDate.Text), User.Identity.Name, ddlStore.SelectedValue);
    }
    private void SaveAuction()
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
                string fileName = file.Replace(file, "Document-" + txtAuctionNo.Text.Trim() + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/Auction/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/Auction/") + fileName);
                docUrl = "./Uploads/Auction/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";
            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }

        SqlCommand command = new SqlCommand(@"INSERT INTO AuctionList (" + insertQuery + @"AuctionDate, AuctionNo, LocationID, FinYear,  StoreId, Remark,  EntryBy)
                                            VALUES (" + parameter + "@AuctionDate, @AuctionNo, @LocationID, @FinYear, @StoreId, @Remark,  @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        string lName = Page.User.Identity.Name.ToString();
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
        }
        command.Parameters.AddWithValue("@AuctionDate", Convert.ToDateTime(txtAuctionDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtAuctionDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Remark", txtRemark.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@AuctionNo", txtAuctionNo.Text);
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
        string auctionID = SQLQuery.ReturnString("SELECT MAX(AuctionID) AS AuctionID FROM AuctionList WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE AuctionDetails SET AuctionID='" + auctionID + "'  WHERE AuctionID = '" + hdnAuctionID.Value + "'" + query);
        DataTable dt = SQLQuery.ReturnDataTable("SELECT AuctionDetailsID, AuctionID, CategoryID, SubCategoryID, ProductDetailsID, ProductID, StoreId, Price, AuctionValue, EntryDate,Qty, EntryBy FROM AuctionDetails Where AuctionID='" + auctionID + "'");
        foreach (DataRow item in dt.Rows)
        {
            string auctionNummber = SQLQuery.ReturnString("SELECT AuctionNo FROM AuctionList Where AuctionID='" + auctionID + "'");
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
            Accounting.VoucherEntry.StockEntry(auctionID, categoryID, subCategoryID, productID, productDetailsID, locationID, centerId, departmentId, storeID, "Auction", "0", "0", "", qty, auctionNummber, qty, "", item["EntryBy"].ToString(), "1");

        }


    }
    private void UpdateAuction()
    {
        string updateQuery = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtAuctionNo.Text + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/Auction/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/Auction/") + fileName);
                docUrl = "./Uploads/Auction/" + fileName;
                updateQuery += "DocumentUrl=@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }
        SqlCommand command = new SqlCommand(@"Update  AuctionList SET AuctionDate=@AuctionDate, AuctionNo=@AuctionNo, LocationID=@LocationID, FinYear=@FinYear,  StoreId=@StoreId," + updateQuery + "Remark=@Remark WHERE AuctionID='" + hdnAuctionID.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@AuctionDate", Convert.ToDateTime(txtAuctionDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtAuctionDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Remark", txtRemark.Text);
        command.Parameters.AddWithValue("@AuctionNo", txtAuctionNo.Text);
        //command.Parameters.AddWithValue("@Supplier", txtSupplier.Text);
        //command.Parameters.AddWithValue("@CauseOfRepair", txtCauseOfRepair.Text);
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.NVarChar).Value = docUrl;
        }
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

    }
    protected void txtAuctionNo_TextChanged(object sender, EventArgs e)
    {
        if (txtAuctionNo.Text.Length == 14)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtAuctionDate.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT AuctionNo FROM AuctionList WHERE AuctionNo='" + txtAuctionNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtAuctionNo.Text + " Number already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("Auction Number should be 14 characters", "warn", lblMsg);
        }
    }
}
