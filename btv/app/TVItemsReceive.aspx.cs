using RunQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class app_TVItemsReceive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Enctype = "multipart/form-data";
        //Page.Form.Attributes.Add("enctype", "multipart/form-data");//file upload
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            //txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtTransferVoucherNo.Text = GenerateTvVoucherNo();
            //txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTVNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name);
            BindddTVID();

            BindGrid();
            //bindDDFromStoreID();
            //bindDDCategoryID();
            //BindddProductSubCategory();
            //BindDdProductId();
            //bindDDLocationID();
            //BindDdCenterId();
            //bindDDDepartmentSectionID();
            //bindDDToStoreID();

            LoadData(ddTVID.SelectedValue);
            BindAddItemsGridView();

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

    private void BindddTVID(string query = "")
    {
        //if (Page.User.IsInRole("Super Admin"))
        //{
        //    query = @"SELECT StoreAssignID, Name FROM Store";
        //}
        //else if (Page.User.IsInRole("Admin"))
        //{
        //    query = @"SELECT StoreAssignID, Name FROM Store WHERE (CenterID = '" + SQLQuery.GetCenterID(User.Identity.Name) + "')";
        //}
        //else
        //{
        query = @"SELECT TV.TvID, TV.TransferVoucherNo +' '+ Store.Name Name, TV.FormStoreID, TV.ToStoreID, TV.ApprovedByDate
            FROM TransferVoucher AS TV INNER JOIN Store ON TV.FormStoreID = Store.StoreAssignID
            WHERE (TV.WorkflowStatus = 'Approved') AND (TV.ReceivedStatus='Pending') AND TV.MainOfficeID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND ToStoreID IN (SELECT StoreID FROM StoreAssign WHERE  (EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "')) ";
        //}

        SQLQuery.PopulateDropDownWithoutSelect(query, ddTVID, "TvID", "Name");
        if (ddTVID.Text == "")
        {
            ddTVID.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    //private void bindDDFromStoreID(string query = "")
    //{
    //    if (Page.User.IsInRole("Super Admin"))
    //    {
    //        query = @"SELECT StoreAssignID, Name FROM Store";
    //    }
    //    else if (Page.User.IsInRole("Admin"))
    //    {
    //        query = @"SELECT StoreAssignID, Name FROM Store WHERE (CenterID = '" + SQLQuery.GetCenterID(User.Identity.Name) + "')";
    //    }
    //    else
    //    {
    //        query = @"SELECT Store.StoreAssignID, Store.Name
    //        FROM Store INNER JOIN StoreAssign ON Store.StoreAssignID = StoreAssign.StoreID
    //        WHERE (StoreAssign.EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "')";
    //    }

    //    SQLQuery.PopulateDropDownWithoutSelect(query, ddFromStoreID, "StoreAssignID", "Name");
    //    if (ddFromStoreID.Text == "")
    //    {
    //        ddFromStoreID.Items.Insert(0, new ListItem("---Select---", "0"));
    //    }

    //}

    //private void bindDDCategoryID()
    //{
    //    SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
    //}
    //private void BindddProductSubCategory()
    //{
    //    SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
    //}

    //private void BindDdProductId()
    //{
    //    SQLQuery.PopulateDropDown("Select Name,ProductID from Product Where ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "'", ddProductID, "ProductID", "Name");
    //}
    //protected void btnAdd_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string lName = Page.User.Identity.Name.ToString();
    //        string isProductExists = SQLQuery.ReturnString("SELECT ProductID FROM TvProduct WHERE ProductID = '" + ddProductID.SelectedValue + "' AND TvVoucherID = '" + tvIdHiddenField.Value + "' AND EntryBy = '" + lName + "'");
    //        if (btnAdd.Text == "ADD")
    //        {
    //            if (isProductExists != ddProductID.SelectedValue)
    //            {
    //                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
    //                {
    //                    InsertToLvProduct();
    //                    Notify("Insert Successful", "info", lblMsg);
    //                    BindAddItemsGridView();
    //                }
    //                else
    //                {
    //                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //                }
    //            }
    //            else
    //            {
    //                Notify("This Product is already added!", "warn", lblMsg);
    //            }

    //        }
    //        else
    //        {
    //            if (SQLQuery.OparatePermission(lName, "Update") == "1")
    //            {
    //                UpdateLvProduct();
    //                BindAddItemsGridView();
    //                Notify("Update Successful", "info", lblMsg);
    //            }
    //            else
    //            {
    //                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //            }


    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Notify("ERROR: " + ex, "error", lblMsg);
    //    }
    //    finally
    //    {

    //        ddProductID.SelectedValue = "0";
    //        ddCategoryID.SelectedValue = "0";
    //        ddProductSubCategory.SelectedValue = "0";
    //        txtQtyNeed.Text = "";

    //    }


    //}
    //private void InsertToLvProduct()
    //{
    //    string lName = Page.User.Identity.Name.ToString();
    //    SqlCommand command;
    //    int lvNo;
    //    if (tvIdHiddenField.Value == "")
    //    {
    //        lvNo = 0;
    //    }
    //    else
    //    {
    //        lvNo = Convert.ToInt32(tvIdHiddenField.Value);
    //    }

    //    command = new SqlCommand(@"INSERT INTO TvProduct (TvVoucherID, CategoryID, SubCategoryID, ProductID, TvQty, EntryBy) 
    //                                   VALUES (@TvVoucherID, @CategoryID, @SubCategoryID, @ProductID, @TvQty, @EntryBy )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //    command.Parameters.AddWithValue("@TvVoucherID", lvNo);
    //    command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
    //    command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
    //    command.Parameters.AddWithValue("@ProductID", ddProductID.SelectedValue);
    //    command.Parameters.AddWithValue("@TvQty", Convert.ToInt32(txtQtyNeed.Text));
    //    command.Parameters.AddWithValue("@EntryBy", lName);
    //    command.Connection.Open();
    //    command.ExecuteNonQuery();
    //    command.Connection.Close();
    //}

    //private void UpdateLvProduct()
    //{
    //    string lName = Page.User.Identity.Name.ToString();

    //    string query = @"UPDATE TvProduct SET CategoryID=@CategoryID,SubCategoryID=@SubCategoryID,TvQty=@TvQty, EntryBy=@EntryBy WHERE TvProductID = '" + idHiddenField.Value + "'";
    //    SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

    //    command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
    //    command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
    //    command.Parameters.AddWithValue("@TvQty", txtQtyNeed.Text);
    //    command.Parameters.AddWithValue("@EntryBy", lName);
    //    command.Connection.Open();
    //    command.ExecuteNonQuery();
    //    command.Connection.Close();
    //    command.Connection.Dispose();
    //}
    private void SaveData(string saveMode)
    {
        //string docName = "";
        //if (document.HasFile)
        //{

        //    string tExt = Path.GetExtension(document.FileName);

        //    try
        //    {
        //        string file = Path.GetFileName(document.FileName);
        //        string fileName = file.Replace(file, "Document-" + txtTransferVoucherNo.Text + "." + tExt);
        //        document.SaveAs(Server.MapPath("./Uploads/TV/") + fileName);
        //        docName = "Uploads/TV/" + fileName;

        //    }
        //    catch (Exception ex)
        //    {
        //        Notify("ERROR" + ex.ToString(), "error", lblMsg);
        //    }

        //}

        //command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfGRN.Text));
        RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO TransferVoucher (TransferVoucherNo, Date, FormStoreID, RequsitionBy,FinYear,MainOfficeID, LocationID, CenterID, DepartmentSectionID, ToStoreID, ReceivedBy, ReceivedByDate, Requirment, DocumentUrl, SaveMode, Remarks, EntryBy) 
                 VALUES (N'" + txtTransferVoucherNo.Text.Replace("'", "''") + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + hdnFormStore.Value + "', N'" + txtRequisitionBy.Text.Replace("'", "''") + "','" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "', '" + SQLQuery.GetLocationID(User.Identity.Name) + "','" + hdnMainOffice.Value + "', '" + hdnCenter.Value + "', '" + hdnDepartment.Value + "', '" + hdnToStore.Value + "', '" + ddReceivedBy.SelectedValue + "', '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "', '" + txtRequirement.Text.Replace("'", "''") + "', 'URl', '" + saveMode + "',  '" + txtRemarks.Text.Replace("'", "''") + "', '" + User.Identity.Name + "')    ");

        string tvId = SQLQuery.ReturnString("SELECT MAX(TvID) AS lvId FROM TransferVoucher");
        SQLQuery.ExecNonQry("UPDATE TvProduct SET TvVoucherID='" + tvId + "', TVoucherNo='" + txtTransferVoucherNo.Text.Replace("'", "''") + "'  WHERE TvVoucherID = '" + tvIdHiddenField.Value + "' AND EntryBy='" + User.Identity.Name + "' ");

        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + tvId + "', VoucherNo='" + txtTransferVoucherNo.Text.Replace("'", "''") + "'  WHERE WorkFlowTypeID = '0' AND EntryBy='" + User.Identity.Name + "' ");
        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + tvId + "'";

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

            SQLQuery.ExecNonQry("UPDATE GRNFrom SET CurrentWorkflowUser='" + name + "' WHERE IDGrnNO = '" + tvId + "'");
            SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + grnNumber, emailBody);

        }
    }
    private void UpdateData(string saveMode)
    {
        string updateQuery = "";
        string returnUser = "";
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM TransferVoucher WHERE TvID='" + lblTvID.Text + "'");
        if (workflowStatus == "Return" && saveMode == "Submitted")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM TransferVoucher WHERE TvID='" + lblTvID.Text + "'");
            workflowStatus = "Pending";
        }
        //string submitDate = "";
        if (saveMode == "Submitted")
        {
            updateQuery = "SubmitDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "',";
        }

        //string parameter = "";
        //string docUrl = "";
        //if (document.HasFile)
        //{
        //    string tExt = Path.GetExtension(document.FileName);
        //    try
        //    {
        //        string file = Path.GetFileName(document.FileName);
        //        string fileName = file.Replace(file, "Document-" + txtTransferVoucherNo.Text + "." + tExt);
        //        if (fileName != "")
        //        {
        //            SQLQuery.DeleteFile(Server.MapPath("./Uploads/TV/" + fileName));
        //        }
        //        document.SaveAs(Server.MapPath("./Uploads/TV/") + fileName);
        //        docUrl = "./Uploads/TV/" + fileName;
        //        updateQuery += "DocumentUrl='" + docUrl + "',";

        //    }
        //    catch (Exception ex)
        //    {
        //        Notify("ERROR" + ex.ToString(), "error", lblMsg);
        //    }

        //}

        RunQuery.SQLQuery.ExecNonQry(@"Update TransferVoucher SET TransferVoucherNo=N'" + txtTransferVoucherNo.Text.Replace("'", "''") + "',FinYear='" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "' , Date='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',  FormStoreID='" + hdnFormStore.Value + "',  RequsitionBy= '" + txtRequisitionBy.Text.Replace("'", "''") + "', LocationID= '" + hdnMainOffice.Value + "', CenterID= '" + hdnCenter.Value + "', DepartmentSectionID= '" + hdnDepartment.Value + "',ToStoreID= '" + hdnToStore.Value + "', ReceivedBy= '" + ddReceivedBy.SelectedValue + "',  Requirment= '" + txtRequirement.Text.Replace("'", "''") + "', SaveMode='" + saveMode + "', WorkflowStatus='" + workflowStatus + "', " + updateQuery + " Remarks= '" + txtRemarks.Text.Replace("'", "''") + "' WHERE TvID='" + lblTvID.Text + "' ");


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
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), lblTvID.Text);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + lblTvID.Text + "'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), lblTvID.Text);
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
            if (btnSave.Text.ToUpper() == "RECEIVED")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {

                    //SaveData("Submitted");
                    DataTable dt = SQLQuery.ReturnDataTable("SELECT TvProductID, TvVoucherID, TVoucherNo, CategoryID, SubCategoryID, ProductID, ProductDetailsID, TvQty, EntryBy, EntryDate FROM TvProduct WHERE TvVoucherID='" + ddTVID.SelectedValue + "'");
                    foreach (DataRow item in dt.Rows)
                    {
                        string tvNo = item["TVoucherNo"].ToString();
                        string categoryID = item["CategoryID"].ToString();
                        string subCategoryID = item["SubCategoryID"].ToString();
                        string productID = item["ProductID"].ToString();
                        string productDetailsID = item["ProductDetailsID"].ToString();
                        string locationID = hdnMainOffice.Value;
                        string centerId = hdnCenter.Value;
                        string departmentId = hdnDepartment.Value;
                        string storeID = hdnToStore.Value;
                        string tvQty = item["TvQty"].ToString();
                        string tvRemarks = "";
                        Accounting.VoucherEntry.StockEntry(ddTVID.SelectedValue, categoryID, subCategoryID, productID, productDetailsID, locationID, centerId, departmentId, storeID, "TV", "0", tvQty, tvNo, tvQty, "", "0", tvRemarks, item["EntryBy"].ToString(), "1");
                        string productType = SQLQuery.GetProductType(productID);
                        if (productType == "Detail")
                        {
                            SQLQuery.ExecNonQry("UPDATE ProductDetails SET Status='1', StoreID='" + storeID + "',ProductStatus='Available'  WHERE ProductDetailsID='" + productDetailsID + "'");
                        }
                        SQLQuery.ExecNonQry("UPDATE TransferVoucher SET ReceivedStatus='Received' WHERE TvID = '" + ddTVID.SelectedValue + "'");
                    }
                    BindddTVID();
                    LoadData(ddTVID.SelectedValue);
                    BindGrid();
                    ClearControls();
                    Notify("Successfully Received...", "success", lblMsg);

                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
            //else
            //{
            //    if (SQLQuery.OparatePermission(lName, "Update") == "1")
            //    {
            //        //UpdateData("Submitted");
            //        ClearControls();
            //        btnSave.Text = "Received";
            //        //btnDraft.Text = "Saved as Draft";
            //        Notify("Successfully Updated...", "success", lblMsg);
            //        //btnDraft.Enabled = true;
            //    }
            //    else
            //    {
            //        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            //    }
            //}
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }

    }
    //protected void btnDraft_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string lName = Page.User.Identity.Name.ToString();
    //        if (txtRemarks.Text != "")
    //        {
    //            if (btnDraft.Text.ToUpper() == "SAVED AS DRAFT")
    //            {
    //                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
    //                {
    //                    string isExist = SQLQuery.ReturnString("SELECT TransferVoucherNo FROM TransferVoucher WHERE TransferVoucherNo='" + txtTransferVoucherNo.Text.Trim().Replace("'", "''") + "'");
    //                    if (isExist == "")
    //                    {
    //                        SaveData("Drafted");
    //                        ClearControls();
    //                        Notify("Successfully Saved as Draft...", "success", lblMsg);
    //                    }
    //                    else
    //                    {
    //                        Notify("TV Number already exist.", "error", lblMsg);
    //                    }
    //                }
    //                else
    //                {
    //                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //                }
    //            }
    //            else
    //            {
    //                if (SQLQuery.OparatePermission(lName, "Update") == "1")
    //                {
    //                    UpdateData("Drafted");
    //                    ClearControls();
    //                    btnDraft.Text = "Saved as Draft";
    //                    Notify("Successfully Updated as Draft...", "success", lblMsg);
    //                    btnSave.Enabled = true;
    //                }
    //                else
    //                {
    //                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Notify("Remarks field can't be empty!", "warn", lblMsg);
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Notify(ex.ToString(), "error", lblMsg);
    //    }
    //    finally
    //    {
    //        tvIdHiddenField.Value = "";
    //        BindGrid();
    //        BindAddItemsGridView();
    //        //BindGrnGridView();
    //        txtTransferVoucherNo.Text = GenerateVoucherNumber.GetTVNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name);
    //    }
    //}
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
                lblTvID.Text = lblEditId.Text;
                tvIdHiddenField.Value = lblEditId.Text;

                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM TransferVoucher WHERE TvID='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM TransferVoucher WHERE TvID='" + lblEditId.Text + "'");
                if (Page.User.IsInRole("Super Admin"))
                {
                    if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Approved"))
                    {
                        //EditMode(lblEditId.Text);
                        //btnDraft.Enabled = true;
                        //btnSave.Enabled = true;
                    }
                }
                else if (Page.User.Identity.Name == lblEntryBy.Text)
                {
                    if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                    {
                        //EditMode(lblEditId.Text);
                        //btnDraft.Enabled = true;
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
    private void BindddReceivedBy()
    {
        SQLQuery.PopulateDropDown("SELECT E.EmployeeID, CONCAT(E.Name,'-',D.Name) AS Name FROM Employee AS E INNER JOIN Designation AS D ON E.DesignationID = D.DesignationID WHERE LocationID = '" + hdnMainOffice.Value + "'", ddReceivedBy, "EmployeeID", "Name");
    }
    private void LoadData(string id)
    {
        DataTable dt = SQLQuery.ReturnDataTable(@"Select TvID, TransferVoucherNo,Date, FormStoreID,MainOfficeID, RequsitionBy, LocationID, CenterID, DepartmentSectionID, ToStoreID, ReceivedBy, Requirment,DocumentUrl, SaveMode,WorkflowStatus,Remarks FROM TransferVoucher WHERE TvID='" + id + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            txtTransferVoucherNo.Text = dtx["TransferVoucherNo"].ToString();
            txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
            // bindDDFromStoreID();
            txtForm.Text = SQLQuery.ReturnString("SELECT Name FROM Location WHERE LocationID='" + dtx["LocationID"].ToString() + "'") + ", " + SQLQuery.ReturnString("SELECT Name FROM     Store WHERE StoreAssignID='" + dtx["FormStoreID"].ToString() + "'");
            hdnFormStore.Value = dtx["FormStoreID"].ToString();
            txtRequisitionBy.Text = dtx["RequsitionBy"].ToString();
            // bindDDLocationID();
            txtMainOffice.Text = SQLQuery.ReturnString("SELECT   Name FROM     Location WHERE LocationID='" + dtx["MainOfficeID"].ToString() + "'");
            hdnMainOffice.Value = dtx["MainOfficeID"].ToString();
            BindddReceivedBy();
            ddReceivedBy.SelectedValue = dtx["ReceivedBy"].ToString();
            // BindDdCenterId();
            txtCenter.Text = SQLQuery.ReturnString("SELECT Name FROM Center WHERE CenterID = '" + dtx["CenterID"].ToString() + "'");
            hdnCenter.Value = dtx["CenterID"].ToString();
            // bindDDDepartmentSectionID();
            txtDepartment.Text = SQLQuery.ReturnString("SELECT Name FROM DepartmentSection WHERE DepartmentSectionID='" + dtx["DepartmentSectionID"].ToString() + "'");
            hdnDepartment.Value = dtx["DepartmentSectionID"].ToString();
            //bindDDToStoreID();
            txtToStore.Text = SQLQuery.ReturnString("SELECT Name FROM     Store WHERE StoreAssignID='" + dtx["ToStoreID"].ToString() + "'");
            hdnToStore.Value = dtx["ToStoreID"].ToString();
            txtRequirement.Text = dtx["Requirment"].ToString();
            //txtDocumentUrl.Text = dtx["DocumentUrl"].ToString();
            //txtStoreID.Text = dtx["StoreID"].ToString();

            txtRemarks.Text = dtx["Remarks"].ToString();

        }
        BindAddItemsGridView();
        //btnSave.Text = "Submit";
        //btnDraft.Text = "Update Draft";
        //Notify("Edit mode activated ...", "info", lblMsg);
    }
    //private void EditMode(string id)
    //{
    //    DataTable dt = SQLQuery.ReturnDataTable(@"Select TvID, TransferVoucherNo,Date, FormStoreID, RequsitionBy, LocationID, CenterID, DepartmentSectionID, ToStoreID, Requirment,DocumentUrl, SaveMode,WorkflowStatus,Remarks FROM TransferVoucher WHERE TvID='" + id + "'");
    //    foreach (DataRow dtx in dt.Rows)
    //    {
    //        txtTransferVoucherNo.Text = dtx["TransferVoucherNo"].ToString();
    //        txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
    //        bindDDFromStoreID();
    //        ddFromStoreID.SelectedValue = dtx["FormStoreID"].ToString();
    //        txtRequisitionBy.Text = dtx["RequsitionBy"].ToString();
    //        bindDDLocationID();
    //        ddLocationID.SelectedValue = dtx["LocationID"].ToString();
    //        BindDdCenterId();
    //        ddCenterID.SelectedValue = dtx["CenterID"].ToString();
    //        bindDDDepartmentSectionID();
    //        ddDepartmentSectionID.SelectedValue = dtx["DepartmentSectionID"].ToString();

    //        bindDDToStoreID();
    //        ddToStoreID.SelectedValue = dtx["ToStoreID"].ToString();
    //        txtRequirement.Text = dtx["Requirment"].ToString();
    //        //txtDocumentUrl.Text = dtx["DocumentUrl"].ToString();
    //        //txtStoreID.Text = dtx["StoreID"].ToString();

    //        txtRemarks.Text = dtx["Remarks"].ToString();

    //    }
    //    BindAddItemsGridView();
    //    btnSave.Text = "Received";
    //   // btnDraft.Text = "Update Draft";
    //    Notify("Edit mode activated ...", "info", lblMsg);
    //}

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblTvID = GridView1.Rows[index].FindControl("LblTvID") as Label;
            RunQuery.SQLQuery.ExecNonQry(" Delete TransferVoucher WHERE TvID='" + lblTvID.Text + "' ");
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
        string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";
        string query = "";
        if (!Page.User.IsInRole("Super Admin") || !Page.User.IsInRole("Senior Store Officer"))
        {
            query = "WHERE LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        }
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT TransferVoucher.TvID, TransferVoucher.TransferVoucherNo, CONVERT(VARCHAR, TransferVoucher.Date, 103) AS Date, TransferVoucher.FormStoreID, FStore.Name AS FromStore, TransferVoucher.RequsitionBy, 
                         TransferVoucher.ToStoreID, TStore.Name AS ToStore, Employee.Name AS ReceivedBy, TransferVoucher.Requirment, TransferVoucher.DocumentUrl, TransferVoucher.SaveMode, TransferVoucher.WorkflowStatus, TransferVoucher.Remarks, 
                         TransferVoucher.EntryDate, TransferVoucher.EntryBy, '" + baseUrl + "' +'TVReport.aspx?TVID=' + CONVERT(VARCHAR, TransferVoucher.TvID) AS Url FROM TransferVoucher INNER JOIN Store AS FStore ON TransferVoucher.FormStoreID = FStore.StoreAssignID INNER JOIN Store AS TStore ON TransferVoucher.ToStoreID = TStore.StoreAssignID INNER JOIN Employee ON TransferVoucher.ReceivedBy = Employee.EmployeeID Where (TransferVoucher.ReceivedStatus='Received') AND TransferVoucher.MainOfficeID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND  ToStoreID IN (SELECT StoreID FROM StoreAssign WHERE  (EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    //protected void ddCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindddProductSubCategory();
    //    BindDdProductId();

    //}

    //protected void ddProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDdProductId();
    //}
    //protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //}


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
        string Query = "";
        if (Page.User.IsInRole("Super Admin"))
        {
            Query = @"WHERE TvVoucherID = '" + ddTVID.SelectedValue + "'";
        }
        else
        {
            Query = @"WHERE TvVoucherID = '" + ddTVID.SelectedValue + "'";// AND EntryBy = '" + Page.User.Identity.Name.ToString() + "'";
        }
        string query = @"SELECT TvProduct.TvProductID, Product.Name, TvProduct.ProductID, TvProduct.TvQty, TvProduct.EntryBy
                    FROM TvProduct INNER JOIN ProductSubCategory ON TvProduct.SubCategoryID = ProductSubCategory.ProductSubCategoryID INNER JOIN
                  ProductCategory ON TvProduct.CategoryID = ProductCategory.ProductCategoryID INNER JOIN
                  Product ON TvProduct.ProductID = Product.ProductID " + Query + "";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Connection.Open();
        AddItemsGridView.EmptyDataText = "No data added ...";
        AddItemsGridView.DataSource = command.ExecuteReader();
        AddItemsGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }






    //protected void AddItemsGridView_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
    //        Label label = AddItemsGridView.Rows[index].FindControl("lblTvProductID") as Label;
    //        idHiddenField.Value = label.Text;
    //        string query = @"SELECT TvProductID, TvVoucherID, CategoryID, SubCategoryID, ProductID, TvQty, EntryBy, EntryDate
    //            FROM TvProduct WHERE TvProductID = '" + idHiddenField.Value + "'";
    //        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //        command.Connection.Open();
    //        SqlDataReader dataReader = command.ExecuteReader();
    //        if (dataReader.Read())
    //        {
    //            btnAdd.Text = "Update";
    //            bindDDCategoryID();
    //            ddCategoryID.SelectedValue = dataReader["CategoryID"].ToString();
    //            BindddProductSubCategory();
    //            ddProductSubCategory.SelectedValue = dataReader["SubCategoryID"].ToString();
    //            BindDdProductId();
    //            ddProductID.SelectedValue = dataReader["ProductID"].ToString();
    //            txtQtyNeed.Text = dataReader["TvQty"].ToString();

    //        }
    //        Notify("Edit mode activated ...", "info", lblMsg);
    //        //ddProductID.Enabled = false;
    //        dataReader.Close();
    //        command.Connection.Close();
    //    }
    //    catch (Exception ex)
    //    {

    //        Notify("ERROR: " + ex, "error", lblMsg);
    //    }

    //}

    //protected void AddItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    string lName = Page.User.Identity.Name;
    //    if (SQLQuery.OparatePermission(lName, "Delete") == "1")
    //    {
    //        int index = Convert.ToInt32(e.RowIndex);
    //        Label lblTvID = AddItemsGridView.Rows[index].FindControl("lblTvProductID") as Label;
    //        SQLQuery.ExecNonQry("Delete TvProduct FROM TvProduct WHERE TvProductID='" + lblTvID.Text + "' ");
    //        BindAddItemsGridView();
    //        Notify("Successfully Deleted...", "success", lblMsg);
    //    }
    //    else
    //    {
    //        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //    }
    //}

    //private void bindDDLocationID()
    //{
    //    SQLQuery.PopulateDropDown("SELECT LocationID, Name FROM Location", ddLocationID, "LocationID", "Name");
    //}


    //protected void ddLocationID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDdCenterId();
    //    bindDDDepartmentSectionID();

    //}
    //private void BindDdCenterId()
    //{
    //    string strQuery = @"SELECT CenterID, Name FROM Center WHERE (LocationID = '" + ddLocationID.SelectedValue + "')";
    //    SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddCenterID, "CenterID", "Name");
    //    if (ddCenterID.Text == "")
    //    {
    //        ddCenterID.Items.Insert(0, new ListItem("---Select---", "0"));
    //    }

    //}



    //protected void ddCenterID_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    bindDDDepartmentSectionID();
    //    bindDDToStoreID();
    //}
    //private void bindDDDepartmentSectionID()
    //{
    //    string strQuery = @"SELECT DepartmentSectionID, Name FROM DepartmentSection WHERE (LocationID = '" + ddLocationID.SelectedValue + "') AND (CenterID = '" + ddCenterID.SelectedValue + "')";
    //    SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddDepartmentSectionID, "DepartmentSectionID", "Name");
    //    if (ddDepartmentSectionID.Text == "")
    //    {
    //        ddDepartmentSectionID.Items.Insert(0, new ListItem("---Select---", "0"));
    //    }

    //}
    //Data load from Store table
    //private void bindDDToStoreID()
    //{
    //    string strQuery = @"SELECT StoreAssignID, StoreID, Name, Description, LocationID, CenterID, DepartmentSectionID
    //        FROM Store WHERE (DepartmentSectionID = '" + ddDepartmentSectionID.SelectedValue + "')";
    //    SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddToStoreID, "StoreAssignID", "Name");
    //    if (ddToStoreID.Text == "")
    //    {
    //        ddToStoreID.Items.Insert(0, new ListItem("---Select---", "0"));
    //    }

    //}

    //protected void ddDepartmentSectionID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    bindDDToStoreID();
    //}


    protected void ddTVID_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData(ddTVID.SelectedValue);
    }
}
