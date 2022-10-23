
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

public partial class app_TransferVoucher : System.Web.UI.Page
{
    private SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {

        this.Page.Form.Enctype = "multipart/form-data";
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);

            bindDDFromStoreID();
            bindDDCategoryID();
            BindddProductSubCategory();
            BindDdProductId();
            bindDDLocationID();
            BindDdCenterId();
            bindDDDepartmentSectionID();
            bindDDToStoreID();
            BindddlDesignation();
            BindddEmployee();
            BindddReceivedBy();
            BindddPriority();
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTvNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, ddFromStoreID.SelectedValue);
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            VisibleWorkflowDateAndDay();
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
    //public string GenerateTvVoucherNo()
    //{

    //    SqlCommand command = new SqlCommand("SELECT CONVERT(VARCHAR, (ISNULL (COUNT(TvID),0)+1001 )) FROM TransferVoucher", connection);
    //    command.Connection.Open();
    //    string tvNo = "";
    //    tvNo = "TV-" + Convert.ToString(command.ExecuteScalar());
    //    command.Connection.Close();
    //    command.Connection.Dispose();
    //    return tvNo;

    //}
    private void bindDDFromStoreID(string query = "")
    {
        if (Page.User.IsInRole("Super Admin"))
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

        SQLQuery.PopulateDropDownWithoutSelect(query, ddFromStoreID, "StoreAssignID", "Name");
        if (ddFromStoreID.Text == "")
        {
            ddFromStoreID.Items.Insert(0, new ListItem("---Select---", "0"));
        }

    }
    private void bindDDCategoryID()
    {
        SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
    }
    private void BindddProductSubCategory()
    {
        SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
    }
    private void BindDdProductId()
    {
        //SQLQuery.PopulateDropDown("SELECT  Product.Name+'-'+ProductDetails.SerialNo AS ProductName,ProductDetails.ProductDetailsID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddlStore.SelectedValue + "'", ddProductID, "ProductDetailsID", "ProductName");
        SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID
                                FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddFromStoreID.SelectedValue + "' AND ProductDetails.Status='True'", ddProductID, "ProductDetailsID", "ProductName");
    }
    //private void BindDdProductId()
    //{
    //    SQLQuery.PopulateDropDown("SELECT  Product.Name+'-'+ProductDetails.SerialNo AS ProductName,ProductDetails.ProductDetailsID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddFromStoreID.SelectedValue + "'", ddProductID, "ProductDetailsID", "ProductName");
    //    //SQLQuery.PopulateDropDown("Select Name,ProductID from Product Where ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "'", ddProductID, "ProductID", "Name");
    //}
    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            string isProductExists = SQLQuery.ReturnString("SELECT ProductDetailsID FROM TvProduct WHERE ProductDetailsID = '" + ddProductID.SelectedValue + "' AND TvVoucherID = '" + hdnTVId.Value + "' AND EntryBy = '" + lName + "'");
            if (btnAdd.Text.ToUpper() == "ADD PRODUCT")
            {
                if (isProductExists != ddProductID.SelectedValue)
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        string productID = SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue);
                        // int avalableQty = SQLQuery.GetAvailableQty(ddFromStoreID.SelectedValue, productID);

                        string productType = SQLQuery.GetProductType(productID);
                        if (productType == "Non-Detail")
                        {
                            int availableQty = SQLQuery.GetAvailableQty(ddFromStoreID.SelectedValue, productID);
                            if (!(int.Parse(txtQtyNeed.Text) <= availableQty))
                            {
                                Notify("This item is stock out", "warn", lblMsg);
                                return;
                            }
                        }
                        string productStatus = "";
                        if (SQLQuery.IsAvailableProduct(ddProductID.SelectedValue, ddFromStoreID.SelectedValue, out productStatus))
                        {
                            InsertToLvProduct();
                            if (productType == "Detail")
                            {
                                SQLQuery.UpdateProductStatus("TV", ddProductID.SelectedValue);
                            }
                            Notify("Product added successfully.", "info", lblMsg);
                            txtQtyNeed.Enabled = true;
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
                    UpdateLvProduct();
                    BindAddItemsGridView();
                    btnAdd.Text = "ADD PRODUCT";
                    Notify("Product update successfully.", "info", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }


            }

        }
        catch (Exception ex)
        {
            Notify("ERROR: " + ex, "error", lblMsg);
        }
        finally
        {

            ddProductID.SelectedValue = "0";
            //ddCategoryID.SelectedValue = "0";
            //ddProductSubCategory.SelectedValue = "0";
            txtQtyNeed.Text = "";

        }


    }
    private void InsertToLvProduct()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        int lvNo;
        if (hdnTVId.Value == "")
        {
            lvNo = 0;
        }
        else
        {
            lvNo = Convert.ToInt32(hdnTVId.Value);
        }
        command = new SqlCommand(@"INSERT INTO TvProduct (TvVoucherID,TVoucherNo, CategoryID, SubCategoryID, ProductID,StoreID, ProductDetailsID, TvQty, TvQtyInWords, EntryBy) 
                                       VALUES (@TvVoucherID,@TVoucherNo, @CategoryID, @SubCategoryID, @ProductID,@StoreID, @ProductDetailsID, @TvQty, @TvQtyInWords, @EntryBy )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Parameters.AddWithValue("@TvVoucherID", lvNo);
        command.Parameters.AddWithValue("@TVoucherNo", hdnTvVoucherNo.Value);
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@StoreID", ddFromStoreID.SelectedValue);
        command.Parameters.AddWithValue("@ProductID", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@TvQty", Convert.ToInt32(txtQtyNeed.Text));
        command.Parameters.AddWithValue("@TvQtyInWords", SQLQuery.Int2WordsBangla(txtQtyNeed.Text.ToString()));
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }

    private void UpdateLvProduct()
    {
        string lName = Page.User.Identity.Name.ToString();

        string query = @"UPDATE TvProduct SET CategoryID=@CategoryID,SubCategoryID=@SubCategoryID,StoreId=@StoreId,ProductID=@ProductID,ProductDetailsID=@ProductDetailsID,TvQty=@TvQty, TvQtyInWords=@TvQtyInWords, EntryBy=@EntryBy WHERE TvProductID = '" + hdnProductId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductID", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@StoreID", ddFromStoreID.SelectedValue);
        command.Parameters.AddWithValue("@TvQty", txtQtyNeed.Text);
        command.Parameters.AddWithValue("@TvQtyInWords", SQLQuery.Int2WordsBangla(txtQtyNeed.Text.ToString()));
        command.Parameters.AddWithValue("@EntryBy", lName);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void SaveData(string saveMode)
    {
        string insertQuery = "";
        string parameter = "";
        string docUrl = "";
        // string docName = "";
        if (document.HasFile)
        {

            string tExt = Path.GetExtension(document.FileName);

            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtTransferVoucherNo.Text.Trim() + tExt);
                document.SaveAs(Server.MapPath("./Uploads/TV/") + fileName);
                docUrl = "./Uploads/TV/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }
        if (saveMode == "Submitted")
        {
            insertQuery += "SubmitDate,";
            parameter += "@SubmitDate,";
        }
        string lName = Page.User.Identity.Name.ToString();

        SqlCommand command = new SqlCommand(@"INSERT INTO TransferVoucher (" + insertQuery + @" TransferVoucherNo, Date, FormStoreID, RequsitionBy,FinYear,MainOfficeID, LocationID, CenterID, DepartmentSectionID, ToStoreID, ReceivedBy, Requirment,SaveMode, Remarks, EntryBy,PreparedBy)
                                            VALUES (" + parameter + "@TransferVoucherNo,@Date,@FormStoreID,@RequsitionBy,@FinYear,@MainOfficeID,@LocationID,@CenterID,@DepartmentSectionID,@ToStoreID, @ReceivedBy, @Requirment,@SaveMode,@Remarks,@EntryBy,@PreparedBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        if (document.HasFile)
        {
            command.Parameters.AddWithValue("@DocumentUrl", docUrl);
        }
        if (saveMode == "Submitted")
        {
            command.Parameters.AddWithValue("@SubmitDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
        }
        command.Parameters.AddWithValue("@TransferVoucherNo", txtTransferVoucherNo.Text);
        command.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@FormStoreID", ddFromStoreID.SelectedValue);
        command.Parameters.AddWithValue("@RequsitionBy", txtRequisitionBy.Text);
        command.Parameters.AddWithValue("@FinYear", GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)));
        command.Parameters.AddWithValue("@LocationID", SQLQuery.GetLocationID(User.Identity.Name));
        command.Parameters.AddWithValue("@MainOfficeID", ddLocationID.SelectedValue);
        command.Parameters.AddWithValue("@CenterID", ddCenterID.SelectedValue);
        command.Parameters.AddWithValue("@DepartmentSectionID", ddDepartmentSectionID.SelectedValue);
        command.Parameters.AddWithValue("@ToStoreID", ddToStoreID.SelectedValue);
        command.Parameters.AddWithValue("@ReceivedBy", ddReceivedBy.SelectedValue);
        command.Parameters.AddWithValue("@Requirment", txtRequirement.Text);
        command.Parameters.AddWithValue("@SaveMode", saveMode);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@PreparedBy", SQLQuery.GetEmployeeID(lName));

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        //RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO TransferVoucher (TransferVoucherNo, Date, FormStoreID, RequsitionBy,FinYear,MainOfficeID, LocationID, CenterID, DepartmentSectionID, ToStoreID, Requirment, DocumentUrl, SaveMode, Remarks, EntryBy) 
        //         VALUES (N'" + txtTransferVoucherNo.Text.Replace("'", "''") + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + ddFromStoreID.SelectedValue + "', N'" + txtRequisitionBy.Text.Replace("'", "''") + "','" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "', '" + SQLQuery.GetLocationID(User.Identity.Name) + "','" + ddLocationID.SelectedValue + "', '" + ddCenterID.SelectedValue + "', '" + ddDepartmentSectionID.SelectedValue + "', '" + ddToStoreID.SelectedValue + "', '" + txtRequirement.Text.Replace("'", "''") + "', 'URl', '" + saveMode + "',  '" + txtRemarks.Text.Replace("'", "''") + "', '" + User.Identity.Name + "')    ");

        string query = "";
        if (hdnTVId.Value == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }
        string tvId = SQLQuery.ReturnString("SELECT MAX(TvID) AS lvId FROM TransferVoucher WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE TvProduct SET TvVoucherID='" + tvId + "', TVoucherNo='" + txtTransferVoucherNo.Text.Replace("'", "''") + "'  WHERE TvVoucherID = '" + hdnTVId.Value + "'" + query);
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + tvId + "', VoucherNo='" + txtTransferVoucherNo.Text.Replace("'", "''") + "'  WHERE WorkFlowTypeID = '0' AND   WorkFlowType='TV' AND EntryBy='" + User.Identity.Name + "' ");
        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + tvId + "' AND WorkFlowType = 'TV'";

            DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
            foreach (DataRow item in dtUser.Rows)
            {
                if (item["Priority"].ToString() == "1")
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), tvId);
                }
            }

        }
    }
    private void NotifyToEmployee(string employeeID, string grnNumber, string tvId)
    {
        string sqlQuery = @"SELECT DesignationWithEmployee.Id, Employee.EmployeeID, Employee.Name, Employee.Email
                        FROM DesignationWithEmployee INNER JOIN Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID
                        WHERE (DesignationWithEmployee.Id = '" + employeeID + "')";
        DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);

        foreach (DataRow item in dt.Rows)
        {
            string name = item["Name"].ToString();
            string email = item["Email"].ToString();
            string emailBody = "Dear " + name +
                          ", <br><br>Approve workflow, check your notification .<br><br>";

            emailBody += " <br><br>Regards, <br><br>Development Team.";

            SQLQuery.ExecNonQry("UPDATE TransferVoucher SET CurrentWorkflowUser='" + name + "' WHERE TvID = '" + tvId + "'");
            SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + grnNumber, emailBody);

        }
    }
    private void UpdateData(string saveMode)
    {
        string updateQuery = "";
        string returnUser = "";
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM TransferVoucher WHERE TvID='" + hdnTVId.Value + "'");
        if (workflowStatus == "Return" && saveMode == "Submitted")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM TransferVoucher WHERE TvID='" + hdnTVId.Value + "'");
            workflowStatus = "Pending";
        }

        if (saveMode == "Submitted")
        {
            updateQuery = "SubmitDate=@SubmitDate,";
        }

        string lName = Page.User.Identity.Name.ToString();
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtTransferVoucherNo.Text + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/TV/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/TV/") + fileName);
                docUrl = "./Uploads/TV/" + fileName;
                updateQuery += "DocumentUrl=@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }

        SqlCommand command = new SqlCommand(@"UPDATE TransferVoucher SET TransferVoucherNo=@TransferVoucherNo, Date=@Date," + updateQuery + " FormStoreID=@FormStoreID, RequsitionBy=@RequsitionBy,FinYear=@FinYear,MainOfficeID=@MainOfficeID, LocationID=@LocationID, CenterID=@CenterID, DepartmentSectionID=@DepartmentSectionID, ToStoreID=@ToStoreID, ReceivedBy=@ReceivedBy, Requirment=@Requirment,SaveMode=@SaveMode,WorkflowStatus=@WorkflowStatus, Remarks=@Remarks, EntryBy=@EntryBy WHERE TvID='" + hdnTVId.Value + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@TransferVoucherNo", txtTransferVoucherNo.Text);
        if (document.HasFile)
        {
            command.Parameters.AddWithValue("@DocumentUrl", docUrl);
        }
        if (saveMode == "Submitted")
        {
            command.Parameters.AddWithValue("@SubmitDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));
        }
        command.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@FormStoreID", ddFromStoreID.SelectedValue);
        command.Parameters.AddWithValue("@RequsitionBy", txtRequisitionBy.Text);
        command.Parameters.AddWithValue("@FinYear", GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)));
        command.Parameters.AddWithValue("@LocationID", SQLQuery.GetLocationID(User.Identity.Name));
        command.Parameters.AddWithValue("@MainOfficeID", ddLocationID.SelectedValue);
        command.Parameters.AddWithValue("@CenterID", ddCenterID.SelectedValue);
        command.Parameters.AddWithValue("@DepartmentSectionID", ddDepartmentSectionID.SelectedValue);
        command.Parameters.AddWithValue("@ToStoreID", ddToStoreID.SelectedValue);
        command.Parameters.AddWithValue("@ReceivedBy", ddReceivedBy.SelectedValue);
        command.Parameters.AddWithValue("@Requirment", txtRequirement.Text);
        command.Parameters.AddWithValue("@SaveMode", saveMode);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.AddWithValue("@WorkflowStatus", workflowStatus);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();

        //RunQuery.SQLQuery.ExecNonQry(@"Update TransferVoucher SET TransferVoucherNo=N'" + txtTransferVoucherNo.Text.Replace("'", "''") + "',FinYear='" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "' , Date='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',  FormStoreID='" + ddFromStoreID.SelectedValue + "',  RequsitionBy= '" + txtRequisitionBy.Text.Replace("'", "''") + "', LocationID= '" + ddLocationID.SelectedValue + "', CenterID= '" + ddCenterID.SelectedValue + "', DepartmentSectionID= '" + ddDepartmentSectionID.SelectedValue + "',ToStoreID= '" + ddToStoreID.SelectedValue + "',  Requirment= '" + txtRequirement.Text.Replace("'", "''") + "', SaveMode='" + saveMode + "', WorkflowStatus='" + workflowStatus + "', " + updateQuery + " Remarks= '" + txtRemarks.Text.Replace("'", "''") + "' WHERE TvID='" + lblTvID.Text + "' ");


        if (saveMode == "Submitted")
        {
            if (returnUser != "")
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowUserID='" + returnUser + "'";
                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "'");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnTVId.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnTVId.Value + "' AND WorkFlowType='TV'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnTVId.Value);
                    }
                }
            }
        }
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text.ToUpper() == "SUBMIT" && btnDraft.Text.ToUpper() == "SAVE AS DRAFT")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    if (AddItemsGridView.Rows.Count > 0)
                    {
                        if (WorkFlowUserGridView.Rows.Count > 0)
                        {
                            if (VerifyPrioritySequence())
                            {
                                string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text));
                                string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                string isExist = SQLQuery.ReturnString("SELECT TransferVoucherNo FROM TransferVoucher WHERE TransferVoucherNo='" + txtTransferVoucherNo.Text.Trim() + "'  AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND FormStoreID='" + ddFromStoreID.SelectedValue + "'");
                                if (isExist == "")
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
                                    SaveData("Submitted");
                                    ClearControls();
                                    Notify("Successfully Submitted...", "success", lblMsg);
                                }
                                else
                                {
                                    Notify("Transfer Voucher Number already exist.", "error", lblMsg);
                                }
                            }
                            else
                            {
                                Notify("Please check workflow priority sequence!", "warn", lblMsg);
                            }

                        }
                        else
                        {
                            Notify("Please add workflow details", "warn", lblMsg);
                        }
                    }
                    else
                    {
                        Notify("Please add product details", "warn", lblMsg);
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
                    if (VerifyPrioritySequence())
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
                        UpdateData("Submitted");
                        ClearControls();
                        btnSave.Text = "Submit";
                        btnDraft.Text = "SAVE AS DRAFT";
                        Notify("Successfully Submitted...", "success", lblMsg);
                        btnDraft.Enabled = true;
                    }
                    else
                    {
                        Notify("Please check workflow priority sequence!", "warn", lblMsg);
                    }

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
            hdnTvVoucherNo.Value = "";
            hdnTVId.Value = "";
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";

            BindGrid();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTvNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, ddFromStoreID.SelectedValue);
        }
    }
    protected void btnDraft_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (txtRemarks.Text != "")
            {
                if (btnDraft.Text.ToUpper() == "SAVE AS DRAFT")
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        if (AddItemsGridView.Rows.Count > 0)
                        {
                            if (WorkFlowUserGridView.Rows.Count > 0)
                            {
                                if (VerifyPrioritySequence())
                                {
                                    string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text));
                                    string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                    string isExist = SQLQuery.ReturnString("SELECT TransferVoucherNo FROM TransferVoucher WHERE TransferVoucherNo='" + txtTransferVoucherNo.Text.Trim() + "'  AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND FormStoreID='" + ddFromStoreID.SelectedValue + "'");
                                    if (isExist == "")
                                    {
                                        SaveData("Drafted");
                                        ClearControls();
                                        Notify("Successfully SAVE AS DRAFT...", "success", lblMsg);
                                    }
                                    else
                                    {
                                        Notify("This " + txtTransferVoucherNo.Text + " TV Number already exist.", "warn", lblMsg);
                                    }
                                }
                                else
                                {
                                    Notify("Please check workflow priority sequence!", "warn", lblMsg);
                                }

                            }
                            else
                            {
                                Notify("Please add workflow details", "warn", lblMsg);
                            }

                        }
                        else
                        {
                            Notify("Please add product details", "warn", lblMsg);
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
                        if (VerifyPrioritySequence())
                        {
                            UpdateData("Drafted");
                            ClearControls();
                            btnSave.Text = "Submit";
                            btnDraft.Text = "SAVE AS DRAFT";
                            Notify("Successfully Updated as Draft...", "success", lblMsg);
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            Notify("Please check workflow priority sequence!", "warn", lblMsg);
                        }

                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
            }
            else
            {
                Notify("Remarks field can't be empty!", "warn", lblMsg);
            }

        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            hdnTvVoucherNo.Value = "";
            hdnTVId.Value = "";
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";
            BindGrid();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTvNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, ddFromStoreID.SelectedValue);
        }
    }

    private bool VerifyPrioritySequence()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = true;
        int priorityCount = 1;

        DataTable priorityDataTable = new DataTable();
        if (String.Empty == hdnTVId.Value)
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnTVId.Value + "' AND EntryBy = '" + lName + "'  AND WorkFlowType = 'TV' ORDER BY Priority");
        }
        else
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnTVId.Value + "' AND WorkFlowType = 'TV' ORDER BY Priority");
        }

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (priorityCount != int.Parse(priorityDataRow["Priority"].ToString()))
            {
                sequentStatus = false;
            }
            priorityCount++;
        }

        return sequentStatus;
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblEditId = GridView1.Rows[index].FindControl("LblTvID") as Label;
                Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
                Label lblTransferVoucher = GridView1.Rows[index].FindControl("lblTransferVoucherNo") as Label;
                hdnTvVoucherNo.Value = lblTransferVoucher.Text;
                hdnTVId.Value = lblEditId.Text;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM TransferVoucher WHERE TvID='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM TransferVoucher WHERE TvID='" + lblEditId.Text + "'");
                if (Page.User.IsInRole("Super Admin"))
                {
                    if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Approved"))
                    {
                        EditMode(lblEditId.Text);
                        btnDraft.Enabled = true;
                        btnSave.Enabled = true;
                    }

                }
                else if (Page.User.Identity.Name == lblEntryBy.Text)
                {
                    if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                    {
                        EditMode(lblEditId.Text);
                        btnDraft.Enabled = true;
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        Notify("This " + lblTransferVoucher.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("This voucher " + lblTransferVoucher.Text + " entry by user is " + lblEntryBy.Text + ". You are not authorize edit the voucher. If you need to any change please contact higher authority.", "warn", lblMsg);
                }

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
    private void EditMode(string id)
    {
        DataTable dt = SQLQuery.ReturnDataTable(@"Select TvID, TransferVoucherNo,Date, FormStoreID, RequsitionBy, MainOfficeID, CenterID, DepartmentSectionID, ToStoreID, ReceivedBy, Requirment,DocumentUrl, SaveMode,WorkflowStatus,Remarks FROM TransferVoucher WHERE TvID='" + id + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            txtTransferVoucherNo.Text = dtx["TransferVoucherNo"].ToString();
            txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
            bindDDFromStoreID();
            ddFromStoreID.SelectedValue = dtx["FormStoreID"].ToString();
            txtRequisitionBy.Text = dtx["RequsitionBy"].ToString();
            bindDDLocationID();
            ddLocationID.SelectedValue = dtx["MainOfficeID"].ToString();
            BindDdCenterId();
            ddCenterID.SelectedValue = dtx["CenterID"].ToString();
            bindDDDepartmentSectionID();
            ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();

            bindDDToStoreID();
            ddToStoreID.SelectedValue = dtx["ToStoreID"].ToString();
            BindddReceivedBy();
            ddReceivedBy.SelectedValue = dtx["ReceivedBy"].ToString();
            txtRequirement.Text = dtx["Requirment"].ToString();
            //txtDocumentUrl.Text = dtx["DocumentUrl"].ToString();
            //txtStoreID.Text = dtx["StoreID"].ToString();

            txtRemarks.Text = dtx["Remarks"].ToString();

        }
        BindAddItemsGridView();
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();
        btnSave.Text = "Submit";
        btnDraft.Text = "Update Draft";
        Notify("Edit mode activated ...", "info", lblMsg);
    }
    private void DeleteTVData(string lblId)
    {
        string query = @"Delete TransferVoucher WHERE TvID = '" + lblId + "'";
        query += " Delete TvProduct WHERE TvVoucherID='" + lblId + "'";
        query += " Delete WorkFlowUser WHERE WorkFlowTypeID='" + lblId + "' AND WorkFlowType='TV'";
        SQLQuery.ExecNonQry(query);
        BindGrid();
        BindAddItemsGridView();
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();

        Notify("Successfully Deleted...", "success", lblMsg);
        txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTvNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, ddFromStoreID.SelectedValue);
    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("LblTvID") as Label;

            Label lblLvInvoiceNo = GridView1.Rows[index].FindControl("lblLvInvoiceNo") as Label;
            Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
            string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM TransferVoucher WHERE TvID='" + lblId.Text + "'");
            string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM TransferVoucher WHERE TvID='" + lblId.Text + "'");
            if (Page.User.IsInRole("Super Admin"))
            {
                if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Pending"))
                {
                    DeleteTVData(lblId.Text);
                }

            }
            else
            {
                if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                {
                    DeleteTVData(lblId.Text);
                }
                else
                {
                    Notify("This " + lblLvInvoiceNo.Text + " already submitted. If you need to delete. please contact higher authority.", "warn", lblMsg);
                }
            }

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
        string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";
        string query = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            //query = "WHERE TransferVoucher.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND TransferVoucher.FormStoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
            query = "AND TransferVoucher.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' ";
        }
        //else if (Page.User.IsInRole("Super Admin"))
        //{
        //    query = "Where TransferVoucher.FormStoreID='" + ddFromStoreID.SelectedValue + "'";
        //}

        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT TransferVoucher.TvID, TransferVoucher.TransferVoucherNo, CONVERT(VARCHAR, TransferVoucher.Date, 103) AS Date, TransferVoucher.FormStoreID, FStore.Name AS FromStore, TransferVoucher.RequsitionBy, 
                         TransferVoucher.ToStoreID, TStore.Name AS ToStore, Employee.Name AS ReceivedBy, TransferVoucher.Requirment, TransferVoucher.DocumentUrl, TransferVoucher.SaveMode, TransferVoucher.WorkflowStatus, TransferVoucher.Remarks, 
                         TransferVoucher.EntryDate, TransferVoucher.EntryBy, '" + baseUrl + @"' +'TVReport.aspx?TVID=' + CONVERT(VARCHAR, TransferVoucher.TvID) AS Url
                            FROM TransferVoucher INNER JOIN Store AS FStore ON TransferVoucher.FormStoreID = FStore.StoreAssignID INNER JOIN
                         Store AS TStore ON TransferVoucher.ToStoreID = TStore.StoreAssignID INNER JOIN 
						 Employee ON TransferVoucher.ReceivedBy = Employee.EmployeeID WHERE TransferVoucher.FormStoreID ='" + ddFromStoreID.SelectedValue + "'" + query + " ORDER BY TransferVoucher.EntryDate DESC");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    protected void ddCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddProductSubCategory();
        BindDdProductId();

    }

    protected void ddProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdProductId();
    }
    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {

        string isDetail = SQLQuery.ReturnString(@"SELECT Product.ProductType FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')");
        if (isDetail == "1")
        {
            txtQtyNeed.Text = "1";
            // txtQtyDelivered.Text = "1";
            txtQtyNeed.Enabled = false;
            // txtQtyDelivered.Enabled = false;
        }
        else
        {
            txtQtyNeed.Text = "";
            //txtQtyDelivered.Text = "";
            txtQtyNeed.Enabled = true;
            //txtQtyDelivered.Enabled = true;
        }
    }


    private void ClearControls()
    {
        txtTransferVoucherNo.Text = "";
        // txtDate.Text = "";
        //txtForm.Text = "";
        txtRequisitionBy.Text = "";
        //txtTo.Text = "";
        txtRequirement.Text = "";
        //txtDocumentUrl.Text = "";
        //txtStoreID.Text = "";

        txtRemarks.Text = "";

    }




    private void BindAddItemsGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string sql = "";
        if (Page.User.IsInRole("Super Admin") && hdnTVId.Value != "")
        {
            sql = @"WHERE TvProduct.TvVoucherID = '" + hdnTVId.Value + "' ";
        }
        else
        {
            sql = @"WHERE TvProduct.TvVoucherID = '" + hdnTVId.Value + "' AND TvProduct.EntryBy = '" + lName + "'";
        }
        string query = @"SELECT TvProduct.TvProductID, CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product.Name + '-' + ProductDetails.SerialNo END AS Name,   TvProduct.ProductID,TvProduct.TvQty
                        FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN TvProduct ON ProductDetails.ProductDetailsID = TvProduct.ProductDetailsID " + sql + "";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Connection.Open();
        AddItemsGridView.EmptyDataText = "No data added ...";
        AddItemsGridView.DataSource = command.ExecuteReader();
        AddItemsGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }

    protected void AddItemsGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
            Label label = AddItemsGridView.Rows[index].FindControl("lblTvProductID") as Label;
            hdnProductId.Value = label.Text;
            string query = @"SELECT TvProductID, TvVoucherID, CategoryID, SubCategoryID, ProductID, ProductDetailsID, TvQty, EntryBy, EntryDate
                FROM TvProduct WHERE TvProductID = '" + hdnProductId.Value + "'";
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
                BindDdProductId();
                ddProductID.SelectedValue = dataReader["ProductDetailsID"].ToString();
                txtQtyNeed.Text = dataReader["TvQty"].ToString();

            }
            Notify("Edit mode activated ...", "info", lblMsg);
            //ddProductID.Enabled = false;
            dataReader.Close();
            command.Connection.Close();
        }
        catch (Exception ex)
        {

            Notify("ERROR: " + ex, "error", lblMsg);
        }

    }

    protected void AddItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
        int index = Convert.ToInt32(e.RowIndex);
        Label lblTvID = AddItemsGridView.Rows[index].FindControl("lblTvProductID") as Label;
        string productDetailsId = SQLQuery.ReturnString("SELECT ProductDetailsID FROM TvProduct WHERE(TvProductID = '" + lblTvID.Text + "')");
        SQLQuery.UpdateProductStatus("Available", productDetailsId);
        SQLQuery.ExecNonQry("Delete TvProduct FROM TvProduct WHERE TvProductID='" + lblTvID.Text + "' ");
        BindAddItemsGridView();
        Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
    }

    private void bindDDLocationID()
    {
        SQLQuery.PopulateDropDown("SELECT LocationID, Name FROM Location", ddLocationID, "LocationID", "Name");
    }
    private void BindddReceivedBy()
    {
        SQLQuery.PopulateDropDown("SELECT E.EmployeeID, CONCAT(E.Name,'-',D.Name) AS Name FROM Employee AS E INNER JOIN Designation AS D ON E.DesignationID = D.DesignationID WHERE LocationID = '" + ddLocationID.SelectedValue + "'", ddReceivedBy, "EmployeeID", "Name");
    }
    protected void ddLocationID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdCenterId();
        bindDDDepartmentSectionID();
        BindddReceivedBy();
    }
    private void BindDdCenterId()
    {
        string strQuery = @"SELECT CenterID, Name FROM Center WHERE (LocationID = '" + ddLocationID.SelectedValue + "')";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddCenterID, "CenterID", "Name");
        if (ddCenterID.Text == "")
        {
            ddCenterID.Items.Insert(0, new ListItem("---Select---", "0"));
        }

    }
    protected void ddCenterID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindDDDepartmentSectionID();
        bindDDToStoreID();
    }
    private void bindDDDepartmentSectionID()
    {
        string strQuery = @"SELECT DepartmentSectionID, Name FROM DepartmentSection WHERE (LocationID = '" + ddLocationID.SelectedValue + "') AND (CenterID = '" + ddCenterID.SelectedValue + "')";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddDepartmentSectionID, "DepartmentSectionID", "Name");
        if (ddDepartmentSectionID.Text == "")
        {
            ddDepartmentSectionID.Items.Insert(0, new ListItem("---Select---", "0"));
        }

    }
    //Data load from Store table
    private void bindDDToStoreID()
    {
        string strQuery = @"SELECT StoreAssignID, StoreID, Name, Description, LocationID, CenterID, DepartmentSectionID
            FROM Store WHERE (DepartmentSectionID = '" + ddDepartmentSectionID.SelectedValue + "')";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddToStoreID, "StoreAssignID", "Name");
        if (ddToStoreID.Text == "")
        {
            ddToStoreID.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }
    protected void ddDepartmentSectionID_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindDDToStoreID();
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddEmployee();
    }
    private void BindddlDesignation()
    {
        string query = @"SELECT DesignationID, Name, Description, RoleID, Priority FROM Designation";
        SQLQuery.PopulateDropDown(query, ddlDesignation, "DesignationID", "Name");
    }
    private void BindddEmployee()
    {
        string query = @"SELECT DesignationWithEmployee.Id, Employee.Name + ', ' + Designation.Name AS Name
                  FROM DesignationWithEmployee INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE DesignationWithEmployee.DesignationID='" + ddlDesignation.SelectedValue + "' AND Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        SQLQuery.PopulateDropDown(query, ddEmployee, "Id", "Name");
    }
    private bool PriorityCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnTVId.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'TV'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnTVId.Value + "' AND WorkFlowType = 'TV'");
            if (priorityDataRow["Priority"].ToString() == ddlPriority.SelectedValue)
            {
                priorityStatus = false;
            }
        }
        return priorityStatus;
    }
    private bool PriorityCheckForUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnTVId.Value + "' AND WorkFlowType = 'TV'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {

            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnTVId.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'TV'");
                if (int.Parse(priority) > 0)
                {
                    priorityStatus = false;
                }

                else
                {
                    priorityStatus = true;
                }
            }
        }
        return priorityStatus;
    }
    private void InsertToWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        int typeId;
        if (hdnTVId.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hdnTVId.Value);
        }
        command = new SqlCommand(@"INSERT INTO WorkFlowUser (WorkFlowTypeID,VoucherNo,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,@VoucherNo,'TV',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@WorkFlowTypeID", typeId);
        command.Parameters.AddWithValue("@VoucherNo", hdnTvVoucherNo.Value);
        command.Parameters.AddWithValue("@EmployeeID", ddEmployee.SelectedValue);
        command.Parameters.AddWithValue("@DesignationId", ddlDesignation.SelectedValue);
        command.Parameters.AddWithValue("@Priority", ddlPriority.SelectedValue);
        command.Parameters.AddWithValue("@EsclationDay", txtEsclationDay.Text);
        command.Parameters.AddWithValue("@Remark", txtRemark.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

        txtRemark.Text = "";
    }
    private void UpdateWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();

        string query = @"UPDATE WorkFlowUser SET EmployeeID=@EmployeeID,DesignationId=@DesignationId,EsclationDay=@EsclationDay, Priority=@Priority, Remark=@Remark, EntryBy=@EntryBy, EntryDate=@EntryDate WHERE WorkFlowUserID = '" + hdnWorkFlowUserId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@DesignationId", ddlDesignation.SelectedValue);
        command.Parameters.AddWithValue("@EmployeeID", ddEmployee.SelectedValue);
        command.Parameters.AddWithValue("@EsclationDay", txtEsclationDay.Text);
        command.Parameters.AddWithValue("@Priority", ddlPriority.SelectedValue);
        command.Parameters.AddWithValue("@Remark", txtRemark.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }

    protected void WorkFlowUserGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(WorkFlowUserGridView.SelectedIndex);
            Label label = WorkFlowUserGridView.Rows[index].FindControl("lblWorkFlowUserID") as Label;

            hdnWorkFlowUserId.Value = label.Text;
            string query = @"SELECT WorkFlowUserID,DesignationId,EmployeeID, Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, Remark FROM WorkFlowUser WHERE WorkFlowUserID = '" + hdnWorkFlowUserId.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                btnWorkFlowSave.Text = "Update";
                BindddlDesignation();
                ddlDesignation.SelectedValue = dataReader["DesignationId"].ToString();
                BindddEmployee();
                ddEmployee.SelectedValue = dataReader["EmployeeID"].ToString();
                ddlPriority.SelectedValue = dataReader["Priority"].ToString();
                txtRemark.Text = dataReader["Remark"].ToString();
            }
            Notify("Edit mode activated ...", "info", lblMsg);
            dataReader.Close();
            command.Connection.Close();
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "info", lblMsg);
        }

    }
    protected void WorkFlowUserGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = WorkFlowUserGridView.Rows[index].FindControl("lblWorkFlowUserID") as Label;
        SQLQuery.ExecNonQry(" Delete WorkFlowUser FROM WorkFlowUser WHERE WorkFlowUserID='" + lblId.Text + "' ");
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();
        Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
    }

    protected void btnWorkFlowSave_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name;

        string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hdnTVId.Value + "' AND WorkFlowType = 'TV' AND EntryBy = '" + lName + "'");
        if (btnWorkFlowSave.Text.ToUpper() == "ADD USER")
        {
            //if (isUserExists != ddEmployee.SelectedValue)
            //{
            if (PrioritySequenceCheck())
            {
                if (PriorityCheck())
                {
                    InsertToWorkFlowUser();
                    BindWorkFlowUserGridView();
                    VisibleWorkflowDateAndDay();
                    Notify("Insert Successful", "info", lblMsg);
                }
                else
                {
                    Notify("Already you have assigned this priority!", "warn", lblMsg);
                }
            }
            else
            {
                Notify("Please check workflow priority sequence!", "warn", lblMsg);
            }

            //}
            //else
            //{
            //    Notify("This employee is already added!", "warn", lblMsg);
            //}
        }
        else
        {
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                if (PriorityCheckForUpdate())
                {
                    UpdateWorkFlowUser();
                    BindWorkFlowUserGridView();
                    VisibleWorkflowDateAndDay();
                    btnWorkFlowSave.Text = "ADD USER";
                    txtRemark.Text = "";
                    Notify("Update Successful", "info", lblMsg);
                }
                else
                {
                    Notify("Already you have assigned this priority or date!", "warn", lblMsg);
                }
            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }

        }

    }
    private bool PrioritySequenceCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = false;
        int priority = int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(MAX(Priority),0) FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnTVId.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'TV'"));
        if (priority > 0)
        {
            if ((priority + 1) == int.Parse(ddlPriority.SelectedValue))
            {
                sequentStatus = true;
            }
        }
        else
        {
            if ("1" == ddlPriority.SelectedValue)
            {
                sequentStatus = true;
            }
        }

        return sequentStatus;
    }
    private void BindWorkFlowUserGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string query = "";


        string sql = "";
        if (User.IsInRole("Super Admin") && hdnTVId.Value != "")
        {
            sql = "AND WorkFlowTypeID='" + hdnTVId.Value + "'";
        }
        else
        {
            sql = "AND WorkFlowTypeID='" + hdnTVId.Value + "' AND EntryBy = '" + lName + "'";
        }
        query = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  WorkFlowUser.WorkFlowType = 'TV' " + sql + "Order By Priority ASC";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }

    private void VisibleWorkflowDateAndDay()
    {
        //string dayName = Convert.ToDateTime(txtEsclationStartTime.Text).ToString("dddd");
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnTVId.Value + "' AND WorkFlowType = 'TV' AND EntryBy = '" + User.Identity.Name + "'");

        if (dt.Rows.Count > 0)
        {
            txtEsclationDay.Text = dt.Rows[0]["EsclationDay"].ToString();
            txtEsclationDay.Enabled = false;

        }
        else
        {
            txtEsclationDay.Enabled = true;
            txtEsclationDay.Text = "1";
        }

    }
    private void BindddPriority()
    {
        SQLQuery.PopulateDropDown("SELECT SequenceId,SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'TV')", ddlPriority, "Priority", "SequenceBan");
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTvNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, ddFromStoreID.SelectedValue);
    }

    protected void txtTransferVoucherNo_TextChanged(object sender, EventArgs e)
    {
        if (txtTransferVoucherNo.Text.Length == 15)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT TransferVoucherNo FROM TransferVoucher WHERE TransferVoucherNo='" + txtTransferVoucherNo.Text.Trim() + "' AND MainOfficeID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtTransferVoucherNo.Text + " TV Voucher Number already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("TV Number should be 15 characters", "warn", lblMsg);
        }

    }

    protected void ddFromStoreID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTvNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, ddFromStoreID.SelectedValue);
        BindDdProductId();

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}
