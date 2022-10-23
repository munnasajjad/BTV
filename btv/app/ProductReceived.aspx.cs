
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

public partial class app_ProductReceived : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Enctype = "multipart/form-data";
        // btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
           
            BindStore();
            bindProductRepair();
            txtReceiveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtReceiveNo.Text = GenerateVoucherNumber.GetReceiveNumber(Convert.ToDateTime(txtReceiveDate.Text), User.Identity.Name,ddlStore.SelectedValue);
            BindProductGridView();
            BindGrid();
        }
    }
    private void BindProductGridView()
    {
        string lName = Page.User.Identity.Name.ToString();

        string query = @"SELECT ProductRepairDetails.ProductRepairDetailsID, ProductRepairDetails.Qty, ProductRepairDetails.ProductID, CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductRepairDetails.ProductRepairID, ProductRepairDetails.ProductDetailsID
FROM     ProductDetails INNER JOIN                  Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                  ProductRepairDetails ON ProductDetails.ProductDetailsID = ProductRepairDetails.ProductDetailsID WHERE ProductRepairDetails.ProductRepairID = '" + ddRepair.SelectedValue + "'";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Connection.Open();
        productGrid.EmptyDataText = "No data added ...";
        productGrid.DataSource = command.ExecuteReader();
        productGrid.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void bindProductRepair()
    {
        if (hdnRpID.Value=="")
        {
            SQLQuery.PopulateDropDown("Select ProductRepairID,RepairNo from ProductRepair Where LocationId='" + SQLQuery.GetLocationID(User.Identity.Name) + "' And StoreId='" + ddlStore.SelectedValue + "' AND Status='Pending'", ddRepair, "ProductRepairID", "RepairNo");
        }
        else
        {
            SQLQuery.PopulateDropDown("Select ProductRepairID,RepairNo from ProductRepair Where LocationId='" + SQLQuery.GetLocationID(User.Identity.Name) + "' And StoreId='" + ddlStore.SelectedValue + "'", ddRepair, "ProductRepairID", "RepairNo");
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
                    //RunQuery.SQLQuery.ExecNonQry(" INSERT INTO ProductReceived (ReapirId, Receivedby, Date, Remarks, EntryBy) VALUES ('" + ddRepair.SelectedValue + "', '" + txtReceivedby.Text.Replace("'", "''") + "', '" + Convert.ToDateTime(txtReceiveDate.Text.Replace("'", "''")).ToString("yyyy-MM-dd") + "', '" + txtRemarks.Text.Replace("'", "''") + "', '" + User.Identity.Name + "')    ");
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
                    SaveReceived();
                    bindProductRepair();
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
                    UpdateReceived();
                   // RunQuery.SQLQuery.ExecNonQry(" Update  ProductReceived SET ReapirId= '" + ddRepair.SelectedValue + "',  Receivedby= '" + txtReceivedby.Text.Replace("'", "''") + "',  Date= '" + Convert.ToDateTime(txtReceiveDate.Text.Replace("'", "''")).ToString("yyyy-MM-dd") + "',  Remarks= '" + txtRemarks.Text.Replace("'", "''") + "' WHERE ProductReceiveID='" + lblId.Text + "' ");
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
            BindProductGridView();
            BindGrid();
        }
    }
    private void UpdateReceived()
    {
        string updateQuery = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtReceiveNo.Text + tExt);
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
        
        

        SqlCommand command = new SqlCommand(@"Update  ProductReceived SET ReapirId=@ReapirId,Date=@Date, ReceiveNo=@ReceiveNo, LocationID=@LocationID, FinYear=@FinYear,Receivedby=@Receivedby,  StoreId=@StoreId," + updateQuery + "Remarks=@Remarks WHERE ProductReceiveID='" + hdnRpID.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@ReapirId", ddRepair.SelectedValue);
        command.Parameters.AddWithValue("@ReceiveNo", txtReceiveNo.Text);
        command.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtReceiveDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtReceiveDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@Receivedby", txtReceivedby.Text);
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue) ;
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
     
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.NVarChar).Value = docUrl;
        }
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

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
                DataTable dt = SQLQuery.ReturnDataTable(" Select ProductReceiveID, ReapirId,Receivedby,Date,Remarks FROM ProductReceived WHERE ProductReceiveID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    bindProductRepair();
                    ddRepair.SelectedValue= dtx["ReapirId"].ToString();
                    ddRepair.Enabled = false;
                    BindProductGridView();
                    txtReceivedby.Text = dtx["Receivedby"].ToString();
                    txtReceiveDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
                    txtRemarks.Text = dtx["Remarks"].ToString();

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
    private void SaveReceived()
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
                string fileName = file.Replace(file, "Document-" + txtReceiveNo.Text.Trim() + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/RC/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/RC/") + fileName);
                docUrl = "./Uploads/RC/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";
            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }

        SqlCommand command = new SqlCommand(@"INSERT INTO ProductReceived (" + insertQuery + @"ReapirId,Date, ReceiveNo, LocationID, FinYear,Receivedby,  StoreId,  Remarks,  EntryBy)
                                            VALUES (" + parameter + "@ReapirId,@Date, @ReceiveNo, @LocationID, @FinYear, @Receivedby,@StoreId,  @Remarks,  @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        string lName = Page.User.Identity.Name.ToString();

        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
        }
        command.Parameters.AddWithValue("@ReapirId", ddRepair.SelectedValue);
        command.Parameters.AddWithValue("@ReceiveNo", txtReceiveNo.Text);
        command.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtReceiveDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtReceiveDate.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@Receivedby", txtReceivedby.Text);
        command.Parameters.AddWithValue("@StoreId", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

       

        //string query = "";
        //if (hdnRpID.Value == "")
        //{
        //    query = " AND EntryBy='" + lName + "'";
        //}

       // string productRepairID = SQLQuery.ReturnString("SELECT MAX(ProductReceiveID) AS ProductReceiveID FROM ProductReceived WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE ProductRepair SET Status='Received'  WHERE ProductRepairID = '" + ddRepair.SelectedValue + "'");

        DataTable dt = SQLQuery.ReturnDataTable("SELECT  ProductRepairDetailsID, ProductRepairID, CategoryID, SubCategoryID, ProductID,ProductDetailsID, StoreId, Qty, EntryDate, EntryBy FROM ProductRepairDetails Where ProductRepairID='" + ddRepair.SelectedValue + "'");
        foreach (DataRow item in dt.Rows)
        {
            string rpNummber = SQLQuery.ReturnString("SELECT RepairNo FROM ProductRepair Where ProductRepairID='" + ddRepair.SelectedValue + "'");
            string categoryID = item["CategoryID"].ToString();
            string subCategoryID = item["SubCategoryID"].ToString();
            string productID = item["ProductID"].ToString();
            string productDetailsID = item["ProductDetailsID"].ToString();
            string locationID = SQLQuery.GetLocationID(item["EntryBy"].ToString());
            string centerId = SQLQuery.GetCenterId(item["EntryBy"].ToString());
            string departmentId = SQLQuery.GetDepartmentSectionId(item["EntryBy"].ToString());
            string storeID = item["StoreId"].ToString();
            string qty = item["Qty"].ToString();
            Accounting.VoucherEntry.StockEntry(ddRepair.SelectedValue, categoryID, subCategoryID, productID, productDetailsID, locationID, centerId, departmentId, storeID, "PR", "0", qty, rpNummber, qty, "", "0", "", item["EntryBy"].ToString(), "1");
            string productType = SQLQuery.GetProductType(productID);
            if (productType == "Detail")
            {
                SQLQuery.ExecNonQry("UPDATE ProductDetails SET Status='1',ProductStatus='Available' WHERE ProductDetailsID='" + productDetailsID + "'");
            }

            //Accounting.VoucherEntry.StockEntry(rvId, categoryID, subCategoryID, productID, productDetailsID, locationID, centerId, departmentId, storeID, "PR", "0", returnQTY, lvNummber, returnQTY, "", "0", "", item["EntryBy"].ToString(), "1");
        }
        

    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ExecNonQry(" Delete ProductReceived WHERE ProductReceiveID='" + lblId.Text + "' ");
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
            query = "And LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        }
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT convert(varchar,Date,103) AS Date,* FROM ProductReceived WHERE StoreId='" + ddlStore.SelectedValue + "'" + query + " Order by EntryDate Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        ddRepair.SelectedValue = "0";
        txtReceivedby.Text = "";
        //txtReceiveDate.Text = "";
        txtRemarks.Text = "";
        //txtReceiveNo.Text = "";
        txtReceiveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtReceiveNo.Text = GenerateVoucherNumber.GetReceiveNumber(Convert.ToDateTime(txtReceiveDate.Text), User.Identity.Name,ddlStore.SelectedValue);
    }
    protected void ddRepair_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindProductGridView();
    }
    protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtReceiveNo.Text = GenerateVoucherNumber.GetReceiveNumber(Convert.ToDateTime(txtReceiveDate.Text), User.Identity.Name, ddlStore.SelectedValue);
        bindProductRepair();
        BindGrid();
    }
    protected void txtReceiveDate_TextChanged(object sender, EventArgs e)
    {
        txtReceiveNo.Text = GenerateVoucherNumber.GetReceiveNumber(Convert.ToDateTime(txtReceiveDate.Text), User.Identity.Name,ddlStore.SelectedValue);
    }
    protected void txtReceiveNo_TextChanged(object sender, EventArgs e)
    {
        if (txtReceiveNo.Text.Length == 14)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtReceiveDate.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT ReceiveNo FROM ProductReceived WHERE ReceiveNo='" + txtReceiveNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtReceiveNo.Text + " Number already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("Receive Number should be 14 characters", "warn", lblMsg);
        }
    }
}
