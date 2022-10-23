
using RunQuery;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AppSirForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Enctype = "multipart/form-data";
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            LoadEmployee();
            BindSaveToStore();

            BindddlDesignation();
            BindddEmployee();
            BindddPriority();
            BindDdCategoryId();
            BindddProductSubCategory();
            BindDdProductId();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            txtDateOfSir.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtSIRVoucharNo.Text = GenerateVoucherNumber.GetSirNumber(Convert.ToDateTime(txtDateOfSir.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
            BindGrid();
            VisibleWorkflowDateAndDay();
        }
    }
    private void BindSaveToStore(string query = "")
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

        SQLQuery.PopulateDropDownWithoutSelect(query, ddSaveToStore, "StoreAssignID", "Name");
        if (ddSaveToStore.SelectedValue == "")
        {
            ddSaveToStore.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        //SQLQuery.PopulateDropDown("Select StoreAssignID, Name from Store", ddSaveToStore, "StoreAssignID", "Name");
    }
    protected void btnLnk_Click(object sender, EventArgs e)
    {
        Response.Redirect("Employee.aspx");
    }
    private void BindDdCategoryId()
    {
        SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
    }
    private void BindddProductSubCategory()
    {
        SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
    }
    private void BindddPriority()
    {
        SQLQuery.PopulateDropDown("SELECT  SequenceId,SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'SIR')", ddlPriority, "Priority", "SequenceBan");
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

                                string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfSir.Text));
                                string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                string isExist = SQLQuery.ReturnString("SELECT SirVoucherNo FROM SIRFrom WHERE SirVoucherNo='" + txtSIRVoucharNo.Text.Trim() + "'  AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND Store='" + ddSaveToStore.SelectedValue + "'");
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

                                    SaveSir("Submitted");
                                    Notify("Successfully Saved...", "success", lblMsg);
                                }
                                else
                                {
                                    Notify("This " + txtSIRVoucharNo.Text + " SIR Number already exist.", "warn", lblMsg);
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
                        UpdateSir("Submitted");
                        ClearControls();
                        btnSave.Text = "Save";
                        Notify("Successfully Updated...", "success", lblMsg);
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
            hdnSIRVoucher.Value = "";
            hdnSIRID.Value = "";
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";
            BindGrid();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            txtSIRVoucharNo.Text = GenerateVoucherNumber.GetSirNumber(Convert.ToDateTime(txtDateOfSir.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
        }
    }

    private bool VerifyPrioritySequence()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = true;
        int priorityCount = 1;
        DataTable priorityDataTable = new DataTable();
        if (String.Empty == hdnSIRID.Value)
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "' AND EntryBy = '" + User.Identity.Name.Trim() + "' AND WorkFlowType = 'SIR' ORDER BY Priority");
        }
        else
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "' AND WorkFlowType = 'SIR' ORDER BY Priority");
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

    private void UpdateSir(string saveMode)
    {
        string updateQuery = "";
        string returnUser = "";
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM SIRFrom WHERE IDSirNo='" + hdnSIRID.Value + "'");
        if (workflowStatus == "Return" && saveMode == "Submitted")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM SIRFrom WHERE IDSirNo='" + hdnSIRID.Value + "'");
            workflowStatus = "Pending";
        }

        //string submitDate = "";
        if (saveMode == "Submitted")
        {
            updateQuery = "SubmitDate=@SubmitDate,";
        }

        //string parameter = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                // string fileName = file.Replace(file, "SIR-" + txtSIRVoucharNo.Text + tExt);
                string fileName = file.Replace(file, "SIR-" + Guid.NewGuid().GetHashCode() + tExt);
                if (hdnDocumentUrl.Value != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath(hdnDocumentUrl.Value));
                }
                document.SaveAs(Server.MapPath("./Uploads/SIR/") + fileName);
                docUrl = "./Uploads/SIR/" + fileName;
                updateQuery += "DocumentUrl=@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }

        SqlCommand command = new SqlCommand(@"Update SIRFrom SET DateOfSir=@DateOfSir,SirVoucherNo=@SirVoucherNo,Store=@Store," + updateQuery + "GivenDivision=@GivenDivision,WorkflowStatus=@WorkflowStatus,GivenDivisionDate=@GivenDivisionDate,SaveMode=@SaveMode,LoanToEmployee=@LoanToEmployee, FinYear=@FinYear, ProductUseAim=@ProductUseAim, HeadOfCost=@HeadOfCost,  Remarks=@Remarks WHERE IDSirNo='" + hdnSIRID.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@DateOfSir", Convert.ToDateTime(txtDateOfSir.Text).ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@SirVoucherNo", txtSIRVoucharNo.Text);
        command.Parameters.AddWithValue("@Store", ddSaveToStore.SelectedValue);
        command.Parameters.AddWithValue("@LoanToEmployee", ddlEmployee.SelectedValue);
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfSir.Text));
        command.Parameters.AddWithValue("@GivenDivision", SQLQuery.GetLocationNameByEmpID(ddlEmployee.SelectedValue));
        command.Parameters.AddWithValue("@GivenDivisionDate", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@ProductUseAim", txtProductUseAim.Text);
        command.Parameters.AddWithValue("@HeadOfCost", txtHeadOfCost.Text);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.Add("@SaveMode", SqlDbType.VarChar).Value = saveMode;
        command.Parameters.Add("@WorkflowStatus", SqlDbType.VarChar).Value = workflowStatus;
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.NVarChar).Value = docUrl;
        }
        if (saveMode == "Submitted")
        {
            command.Parameters.Add("@SubmitDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

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
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnSIRID.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnSIRID.Value + "' AND WorkFlowType='SIR'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnSIRID.Value);
                    }
                }
            }
        }

    }
    private void LoadEmployee()
    {
        string query = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            query = " WHERE LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND EmployeeID NOT IN( SELECT EmployeeInfoID FROM Logins WHERE  (LoginUserName = '" + User.Identity.Name + "'))";
            //query = "WHERE LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        }

        //SQLQuery.PopulateDropDown("Select EmployeeID, Name+' ('+ Mobile+')' AS Name from Employee WHERE EmployeeID NOT IN( SELECT EmployeeInfoID FROM Logins WHERE  (LoginUserName = '" + User.Identity.Name + "')) AND LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'", ddlEmployee, "EmployeeID", "Name");
        SQLQuery.PopulateDropDown("Select EmployeeID, Name+' ('+ Mobile+')' AS Name from Employee" + query + "", ddlEmployee, "EmployeeID", "Name");
    }
    private void SaveSir(string saveMode)
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
                //string fileName = file.Replace(file, "Document-" + txtSIRVoucharNo.Text.Trim() + tExt);
                string fileName = file.Replace(file, "SIR-" + Guid.NewGuid().GetHashCode() + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath(fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/SIR/") + fileName);
                docUrl = "./Uploads/SIR/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";
            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }

        string date = Convert.ToDateTime(txtDateOfSir.Text).ToString("yyyy-MM-dd");

        if (saveMode == "Submitted")
        {
            insertQuery += "SubmitDate,";
            parameter += "@SubmitDate,";
        }

        SqlCommand command = new SqlCommand(@"INSERT INTO SIRFrom (" + insertQuery + @"DateOfSir, SirVoucherNo, LocationID, FinYear, LoanToEmployee, Store, GivenDivision, GivenDivisionDate, ProductUseAim, HeadOfCost, Remarks, SaveMode, EntryBy,PreparedBy )
                                            VALUES (" + parameter + "@DateOfSir, @SirVoucherNo, @LocationID, @FinYear, @LoanToEmployee, @Store, @GivenDivision, @GivenDivisionDate, @ProductUseAim, @HeadOfCost, @Remarks, @SaveMode, @EntryBy,@PreparedBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        string lName = Page.User.Identity.Name.ToString();

        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
        }
        if (saveMode == "Submitted")
        {
            command.Parameters.Add("@SubmitDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }
        command.Parameters.AddWithValue("@DateOfSir", Convert.ToDateTime(txtDateOfSir.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfSir.Text));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.AddWithValue("@SirVoucherNo", txtSIRVoucharNo.Text);
        command.Parameters.AddWithValue("@LoanToEmployee", ddlEmployee.SelectedValue);
        command.Parameters.AddWithValue("@Store", ddSaveToStore.SelectedValue);
        command.Parameters.AddWithValue("@GivenDivision", SQLQuery.GetLocationNameByEmpID(ddlEmployee.SelectedValue));
        command.Parameters.AddWithValue("@GivenDivisionDate", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@ProductUseAim", txtProductUseAim.Text);
        command.Parameters.AddWithValue("@HeadOfCost", txtHeadOfCost.Text);
        command.Parameters.AddWithValue("@Remarks", txtRemarks.Text);
        command.Parameters.Add("@SaveMode", SqlDbType.VarChar).Value = saveMode;
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.Add("@PreparedBy", SqlDbType.Int).Value = SQLQuery.GetEmployeeID(lName);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

        ClearControls();
        Notify("Successfully Saved...", "success", lblMsg);

        string query = "";
        if (hdnSIRID.Value == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }

        string sirId = SQLQuery.ReturnString("SELECT MAX(IDSirNo) AS SirId FROM SIRFrom WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE SIRProduct SET IDSirNo='" + sirId + "',SIRVoucherNo='" + txtSIRVoucharNo.Text + "'  WHERE IDSirNo = '" + hdnSIRID.Value + "'" + query);
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + sirId + "' , VoucherNo='" + txtSIRVoucharNo.Text.Trim() + "'  WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "'  AND WorkFlowType='SIR' AND EntryBy='" + lName + "' ");
        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + sirId + "' AND WorkFlowType='SIR'";

            DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
            foreach (DataRow item in dtUser.Rows)
            {
                if (item["Priority"].ToString() == "1")
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), sirId);
                }
            }

        }


    }
    private void NotifyToEmployee(string employeeId, string lvNumber, string sirId)
    {
        string sqlQuery = @"SELECT DesignationWithEmployee.Id, Employee.EmployeeID, Employee.Name, Employee.Email
                        FROM DesignationWithEmployee INNER JOIN Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID
                        WHERE (DesignationWithEmployee.Id = '" + employeeId + "')";
        DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);

        foreach (DataRow item in dt.Rows)
        {
            string name = item["Name"].ToString();
            string email = item["Email"].ToString();
            string emailBody = "Dear " + name +
                          ", <br><br>Approve workflow, check your notification .<br><br>";

            emailBody += " <br><br>Regards, <br><br>Development Team.";

            SQLQuery.ExecNonQry("UPDATE SIRFrom SET CurrentWorkflowUser='" + name + "' WHERE IDSirNo = '" + sirId + "'");
            SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + lvNumber, emailBody);

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
                Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
                Label lblSirVoucherNo = GridView1.Rows[index].FindControl("lblSirVoucherNo") as Label;
                hdnSIRID.Value = lblEditId.Text;
                lblId.Text = lblEditId.Text;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM SIRFrom WHERE IDSirNo='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM SIRFrom WHERE IDSirNo='" + lblEditId.Text + "'");

                if (Page.User.IsInRole("Super Admin"))
                {
                    if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Approved"))
                    {
                        EditMode(lblEditId.Text);
                        btnDraft.Enabled = false;
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
                        Notify("This " + lblSirVoucherNo.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
                    }
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

    private void EditMode(string lblEditId)
    {
        DataTable dt = SQLQuery.ReturnDataTable("Select IDSirNo,SirVoucherNo,DateOfSir,LoanToEmployee,DocumentUrl,GivenDivision,GivenDivisionDate,ProductUseAim,HeadOfCost,Remarks FROM SIRFrom WHERE IDSirNo='" + lblEditId + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            txtDateOfSir.Text = Convert.ToDateTime(dtx["DateOfSir"]).ToString("dd/MM/yyyy");
            txtSIRVoucharNo.Text = dtx["SirVoucherNo"].ToString();
            //txtGivenDivision.Text = dtx["GivenDivision"].ToString();
            //txtGivenDivisionDate.Text = Convert.ToDateTime(dtx["GivenDivisionDate"]).ToString("dd/MM/yyyy");
            ddlEmployee.SelectedValue = dtx["LoanToEmployee"].ToString();
            txtProductUseAim.Text = dtx["ProductUseAim"].ToString();
            txtHeadOfCost.Text = dtx["HeadOfCost"].ToString();
            hdnDocumentUrl.Value = dtx["DocumentUrl"].ToString();
            txtRemarks.Text = dtx["Remarks"].ToString();
        }

        BindAddItemsGridView();
        BindWorkFlowUserGridView();
        //btnSave.Text = "Update Submit";
        btnDraft.Text = "Update Draft";
        Notify("Edit mode activated ...", "info", lblMsg);
    }
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            Label lblSirInvoiceNo = GridView1.Rows[index].FindControl("lblSirVoucherNo") as Label;
            Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
            string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM SIRFrom WHERE IDSirNo='" + lblId.Text + "'");
            string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM SIRFrom WHERE IDSirNo='" + lblId.Text + "'");
            if (Page.User.IsInRole("Super Admin"))
            {
                if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Pending"))
                {
                    DeleteSirData(lblId.Text);
                }

            }
            else
            {
                if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                {
                    DeleteSirData(lblId.Text);
                }
                else
                {
                    Notify("This " + lblSirInvoiceNo.Text + " already submitted. If you need to delete. please contact higher authority.", "warn", lblMsg);
                }
            }
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }

    private void DeleteSirData(string lblId)
    {
        string query = "Delete SIRFrom WHERE IDSirNo='" + lblId + "'";
        query += " Delete WorkFlowUser WHERE WorkFlowTypeID='" + lblId + "' AND WorkFlowType='SIR' ";
        query += " Delete SIRProduct WHERE SIRProductID='" + lblId + "'";
        RunQuery.SQLQuery.ExecNonQry(query);
        BindGrid();
        BindAddItemsGridView();
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();

        Notify("Successfully Deleted...", "success", lblMsg);
        txtSIRVoucharNo.Text = GenerateVoucherNumber.GetSirNumber(Convert.ToDateTime(txtDateOfSir.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
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
            query = "AND SIRFrom.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' ";
        }
        //else if (Page.User.IsInRole("Super Admin"))
        //{
        //    query = "Where SIRFrom.Store='" + ddSaveToStore.SelectedValue + "'";
        //}
        DataTable dt = SQLQuery.ReturnDataTable(@" SELECT '" + baseUrl + "'+'SIRReport.aspx?SIRId=' + CONVERT(VARCHAR, SIRFrom.IDSirNo) AS SirId, CONVERT(varchar, SIRFrom.GivenDivisionDate, 103) AS GivenDivisionDate, SIRFrom.IDSirNo, SIRFrom.DateOfSir, SIRFrom.SirVoucherNo, SIRFrom.LocationID, SIRFrom.FinYear, SIRFrom.LoanToEmployee, SIRFrom.Store, SIRFrom.GivenDivision, SIRFrom.GivenDivisionDate AS GivenDivisionDate, SIRFrom.ProductUseAim, SIRFrom.HeadOfCost, SIRFrom.Remarks, SIRFrom.DocumentUrl, SIRFrom.SaveMode, SIRFrom.SubmitDate, SIRFrom.WorkflowStatus, SIRFrom.ApprovedDate, SIRFrom.ReturnOrHoldUserID, SIRFrom.CurrentWorkflowUser, SIRFrom.EntryDate, SIRFrom.EntryBy, Employee.Name AS PreparedBy FROM SIRFrom INNER JOIN Employee ON SIRFrom.PreparedBy = Employee.EmployeeID WHERE  SIRFrom.Store ='" + ddSaveToStore.SelectedValue + "'" + query + "ORDER BY SIRFrom.EntryDate DESC");
        //DataTable dt = SQLQuery.ReturnDataTable(@" SELECT (SELECT SettingValue FROM Settings WHERE(id = '1')) + 'XerpReports/SIRReport.aspx?SIRId=' + CONVERT(VARCHAR, SIRFrom.IDSirNo) AS SirId, CONVERT(varchar, SIRFrom.GivenDivisionDate, 103) AS GivenDivisionDate, SIRFrom.IDSirNo, SIRFrom.DateOfSir, SIRFrom.SirVoucherNo, SIRFrom.LocationID, SIRFrom.FinYear, SIRFrom.LoanToEmployee, SIRFrom.Store, SIRFrom.GivenDivision, SIRFrom.GivenDivisionDate AS GivenDivisionDate, SIRFrom.ProductUseAim, SIRFrom.HeadOfCost, SIRFrom.Remarks, SIRFrom.DocumentUrl, SIRFrom.SaveMode, SIRFrom.SubmitDate, SIRFrom.WorkflowStatus, SIRFrom.ApprovedDate, SIRFrom.ReturnOrHoldUserID, SIRFrom.CurrentWorkflowUser, SIRFrom.EntryDate, SIRFrom.EntryBy, Employee.Name AS PreparedBy FROM SIRFrom INNER JOIN Employee ON SIRFrom.PreparedBy = Employee.EmployeeID " + query + "ORDER BY SIRFrom.EntryDate DESC");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
    private void BindDdProductId()
    {
        SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID
                                FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddSaveToStore.SelectedValue + "' AND ProductDetails.Status='True'", ddProductID, "ProductDetailsID", "ProductName");
    }


    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {
        string isDetail = SQLQuery.ReturnString(@"SELECT Product.ProductType FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')");
        if (isDetail == "1")
        {
            txtQtyNeed.Text = "1";
            txtQtyDelivered.Text = "1";
            txtQtyNeed.Enabled = false;
            txtQtyDelivered.Enabled = false;
        }
        else
        {
            txtQtyNeed.Text = "";
            txtQtyDelivered.Text = "";
            txtQtyNeed.Enabled = true;
            txtQtyDelivered.Enabled = true;
        }
    }

    private void ClearControls()
    {
        //txtDateOfSir.Text = "";
        //txtGivenDivision.Text = "";
        //txtGivenDivisionDate.Text = "";
        txtProductUseAim.Text = "";
        txtHeadOfCost.Text = "";
        //txtProductDescription.Text = "";
        txtQtyNeed.Text = "";
        txtQtyDelivered.Text = "0";
        txtQtyAvailable.Text = "";
        txtUnitPrice.Text = "";
        txtDeliveredQtyTotalPrice.Text = "";
        txtRemarks.Text = "";

    }



    private void InsertSirProduct()
    {
        string lName = Page.User.Identity.Name.ToString();
        int sirNo;
        sirNo = hdnSIRID.Value == "" ? 0 : Convert.ToInt32(hdnSIRID.Value);

        SqlCommand command;
        if (btnDraft.Text.ToUpper() != "UPDATE DRAFT")
        {
            command = new SqlCommand(@"INSERT INTO SIRProduct (IDSirNo,SIRVoucherNo,CategoryID,SubCategoryID,ProductId,StoreID,ProductDetailsID,QTYNeed,NeedQtyInWords,QTYDelivered,DeliveredQtyInWords,QTYAvailable,AvailableQtyInWords,UnitPrice, DeliveredQTYTotalPrice,EntryBy) 
                                       VALUES ('',@SIRVoucherNo,@CategoryID,@SubCategoryID,@ProductId,@StoreID,@ProductDetailsID,@QTYNeed,@NeedQtyInWords,@QTYDelivered,@DeliveredQtyInWords,@QTYAvailable,@AvailableQtyInWords,@UnitPrice,@DeliveredQTYTotalPrice,@EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        else
        {
            command = new SqlCommand(@"INSERT INTO SIRProduct (IDSirNo,SIRVoucherNo,CategoryID,SubCategoryID,ProductId,StoreID,ProductDetailsID,QTYNeed,NeedQtyInWords,QTYDelivered,DeliveredQtyInWords,QTYAvailable,AvailableQtyInWords,UnitPrice, DeliveredQTYTotalPrice,EntryBy) 
                                       VALUES (@IDSirNo,@SIRVoucherNo,@CategoryID,@SubCategoryID,@ProductId,@StoreID,@ProductDetailsID,@QTYNeed,@NeedQtyInWords,@QTYDelivered,@DeliveredQtyInWords,@QTYAvailable,@AvailableQtyInWords,@UnitPrice,@DeliveredQTYTotalPrice,@EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }

        command.Parameters.AddWithValue("@IDSirNo", sirNo);
        command.Parameters.AddWithValue("@SIRVoucherNo", hdnSIRVoucher.Value);
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductId", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@StoreID", ddSaveToStore.SelectedValue);
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@QTYNeed", txtQtyNeed.Text);
        command.Parameters.AddWithValue("@NeedQtyInWords", SQLQuery.Int2WordsBangla(txtQtyNeed.Text.ToString()));
        command.Parameters.AddWithValue("@QTYDelivered", txtQtyDelivered.Text);
        command.Parameters.AddWithValue("@DeliveredQtyInWords", SQLQuery.Int2WordsBangla(txtQtyDelivered.Text.ToString()));
        command.Parameters.AddWithValue("@QTYAvailable", txtQtyAvailable.Text);
        command.Parameters.AddWithValue("@AvailableQtyInWords", SQLQuery.Int2WordsBangla(txtQtyAvailable.Text.ToString()));
        command.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text);
        command.Parameters.AddWithValue("@DeliveredQTYTotalPrice", txtDeliveredQtyTotalPrice.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }

    private void BindAddItemsGridView()
    {
        string lName = Page.User.Identity.Name.ToString();

        string sql = "";
        if (Page.User.IsInRole("Super Admin") && hdnSIRID.Value != "")
        {
            sql = @"WHERE IDSirNo = '" + hdnSIRID.Value + "'";
        }
        else
        {
            sql = @"WHERE IDSirNo = '" + hdnSIRID.Value + "' AND SIRProduct.EntryBy = '" + lName + "'";
        }
        string query = @"SELECT SIRProduct.SIRProductID,CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, SIRProduct.QTYDelivered, SIRProduct.QTYNeed, SIRProduct.QTYAvailable, SIRProduct.DeliveredQTYTotalPrice, SIRProduct.UnitPrice, 
                  SIRProduct.ProductId FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN SIRProduct ON ProductDetails.ProductDetailsID = SIRProduct.ProductDetailsID " + sql + "";

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
            Label label = AddItemsGridView.Rows[index].FindControl("lblSirProductID") as Label;
            hdnProductId.Value = label.Text;
            string query = @"SELECT SIRProductID,ProductID, ProductDetailsID,CategoryID,SubCategoryID,IDSirNo, ProductDescription, QTYNeed, QTYDelivered, QTYAvailable, UnitPrice, DeliveredQTYTotalPrice FROM     SIRProduct WHERE SIRProductID = '" + hdnProductId.Value + "'";
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
                txtQtyNeed.Text = dataReader["QTYNeed"].ToString();
                txtQtyDelivered.Text = dataReader["QTYDelivered"].ToString();
                txtQtyAvailable.Text = dataReader["QTYAvailable"].ToString();
                txtDeliveredQtyTotalPrice.Text = dataReader["DeliveredQTYTotalPrice"].ToString();
                txtUnitPrice.Text = dataReader["UnitPrice"].ToString();

            }
            Notify("Edit mode activated ...", "info", lblMsg);
            //ddProductID.Enabled = false;
            dataReader.Close();
            command.Connection.Close();
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "error", lblMsg);
        }

    }

    protected void AddItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = AddItemsGridView.Rows[index].FindControl("lblSirProductID") as Label;
        string productDetailsId = SQLQuery.ReturnString("SELECT ProductDetailsID FROM SIRProduct WHERE(SIRProductID = '" + lblId.Text + "')");
        SQLQuery.UpdateProductStatus("Available", productDetailsId);
        SQLQuery.ExecNonQry("Delete FROM SIRProduct WHERE SIRProductID='" + lblId.Text + "' ");
        BindAddItemsGridView();
        Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
    }
    private void BindddlDesignation()
    {
        string query = @"SELECT DesignationID, Name, Description, RoleID, Priority FROM Designation";
        SQLQuery.PopulateDropDown(query, ddlDesignation, "DesignationID", "Name");
    }
    private void BindddEmployee()
    {
        //string sqlquery = "";
        //if (!Page.User.IsInRole("Super Admin"))
        //{
        //    sqlquery = "WHERE Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
        //}

        string query = @"SELECT DesignationWithEmployee.Id, Employee.Name + ', ' + Designation.Name AS Name
                  FROM DesignationWithEmployee INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE DesignationWithEmployee.DesignationID='" + ddlDesignation.SelectedValue + "' AND Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        SQLQuery.PopulateDropDown(query, ddEmployee, "Id", "Name");
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
                txtWorkflowRemarks.Text = dataReader["Remark"].ToString();
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
    private void UpdateSirProduct()
    {
        // string lName = Page.User.Identity.Name.ToString();

        string query = @"UPDATE SIRProduct SET CategoryID=@CategoryID,SubCategoryID=@SubCategoryID,ProductDetailsID=@ProductDetailsID, ProductId=@ProductID,StoreID=@StoreID, QTYNeed=@QTYNeed, NeedQtyInWords=@NeedQtyInWords, QTYDelivered=@QTYDelivered, DeliveredQtyInWords=@DeliveredQtyInWords, QTYAvailable=@QTYAvailable, AvailableQtyInWords=@AvailableQtyInWords, UnitPrice=@UnitPrice,DeliveredQTYTotalPrice =@TotalPrice WHERE SIRProductID = '" + hdnProductId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductID", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@StoreID", ddSaveToStore.SelectedValue);
        command.Parameters.AddWithValue("@QTYNeed", txtQtyNeed.Text);
        command.Parameters.AddWithValue("@NeedQtyInWords", SQLQuery.Int2WordsBangla(txtQtyNeed.Text.ToString()));
        command.Parameters.AddWithValue("@QTYDelivered", txtQtyDelivered.Text);
        command.Parameters.AddWithValue("@DeliveredQtyInWords", SQLQuery.Int2WordsBangla(txtQtyDelivered.Text.ToString()));
        command.Parameters.AddWithValue("@QTYAvailable", txtQtyAvailable.Text);
        command.Parameters.AddWithValue("@AvailableQtyInWords", SQLQuery.Int2WordsBangla(txtQtyAvailable.Text.ToString()));
        command.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text);
        command.Parameters.AddWithValue("@TotalPrice", txtDeliveredQtyTotalPrice.Text);

        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            SQLQuery.Empty2Zero(txtQtyAvailable);
            string lName = Page.User.Identity.Name.ToString();
            string isProductExists = SQLQuery.ReturnString("SELECT ProductDetailsID FROM SIRProduct WHERE ProductDetailsID = '" + ddProductID.SelectedValue + "' AND IDSirNo = '" + hdnSIRID.Value + "' AND EntryBy = '" + lName + "'");
            //SQLQuery.Empty2Zero(txtQtyDelivered);
            if (btnAdd.Text.ToUpper() == "ADD PRODUCT")
            {
                if (isProductExists != ddProductID.SelectedValue)
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        string productId = SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue);
                        string productType = SQLQuery.GetProductType(productId);
                        if (productType == "Non-Detail")
                        {
                            int availableQty = SQLQuery.GetAvailableQty(ddSaveToStore.SelectedValue, productId);

                            if (!(int.Parse(txtQtyNeed.Text) <= availableQty))
                            {
                                Notify("This item is stock out", "warn", lblMsg);
                                return;
                            }
                        }
                        string productStatus = "";
                        if (SQLQuery.IsAvailableProduct(ddProductID.SelectedValue, ddSaveToStore.SelectedValue, out productStatus))
                        {
                            InsertSirProduct();
                            if (productType == "Detail")
                            {
                                SQLQuery.UpdateProductStatus("SIR", ddProductID.SelectedValue);
                            }
                            txtQtyNeed.Text = "0";
                            txtQtyDelivered.Text = "0";
                            txtQtyAvailable.Text = "0";
                            txtUnitPrice.Text = "0";
                            txtDeliveredQtyTotalPrice.Text = "0";
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
                    UpdateSirProduct();
                    txtQtyNeed.Text = "0";
                    txtQtyDelivered.Text = "0";
                    txtQtyAvailable.Text = "0";
                    txtUnitPrice.Text = "0";
                    txtDeliveredQtyTotalPrice.Text = "0";
                    ddProductID.SelectedValue = "0";
                    btnAdd.Text = "ADD PRODUCT";
                    BindAddItemsGridView();
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

    }
    private void InsertToWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();


        SqlCommand command;
        int typeId;
        if (hdnSIRID.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hdnSIRID.Value);
        }


        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,VoucherNo,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,@VoucherNo,'SIR',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        command.Parameters.AddWithValue("@WorkFlowTypeID", typeId);
        command.Parameters.AddWithValue("@VoucherNo", hdnSIRVoucher.Value);
        command.Parameters.AddWithValue("@DesignationId", ddlDesignation.SelectedValue);
        command.Parameters.AddWithValue("@EmployeeID", ddEmployee.SelectedValue);
        command.Parameters.AddWithValue("@Priority", ddlPriority.SelectedValue);
        command.Parameters.AddWithValue("@EsclationDay", txtEsclationDay.Text);
        command.Parameters.AddWithValue("@Remark", txtWorkflowRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();


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
        command.Parameters.AddWithValue("@Remark", txtWorkflowRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddEmployee();
    }
    private void BindWorkFlowUserGridView()
    {
        string lName = Page.User.Identity.Name.ToString();


        string sql = "";
        if (User.IsInRole("Super Admin") && hdnSIRID.Value != "")
        {
            sql = "AND WorkFlowTypeID='" + hdnSIRID.Value + "'";
        }
        else
        {
            sql = "AND WorkFlowTypeID='" + hdnSIRID.Value + "' AND EntryBy = '" + lName + "'";
        }

        string query = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  WorkFlowUser.WorkFlowType = 'SIR' " + sql + " Order By Priority ASC";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private bool PriorityCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority, EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'SIR'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "' AND WorkFlowType = 'SIR'");
            if (priorityDataRow["Priority"].ToString() == ddlPriority.SelectedValue)
            {
                priorityStatus = false;
            }

        }

        return priorityStatus;
    }

    private bool PrioritySequenceCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = false;
        int priority = int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(MAX(Priority),0) FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'SIR'"));
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


    private bool PriorityCheckForUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUserID, Priority, EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "' AND WorkFlowType = 'SIR'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnSIRID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'SIR'");
                int priorityCount = int.Parse(SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowUserID <> '" + hdnWorkFlowUserId.Value + "' AND WorkFlowTypeID = '" + hdnSIRID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND WorkFlowType = 'SIR'"));
                if (int.Parse(priority) > 0 || priorityCount > 0)
                {
                    priorityStatus = false;

                }
            }

        }
        return priorityStatus;
    }




    protected void btnWorkFlowSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;
            string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hdnSIRID.Value + "' AND WorkFlowType = 'SIR' AND EntryBy = '" + lName + "'");
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
                        txtWorkflowRemarks.Text = "";
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
                        txtWorkflowRemarks.Text = "";
                        Notify("Update Successful", "success", lblMsg);
                    }
                    else
                    {
                        Notify("Already you have assigned this priority", "warn", lblMsg);
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
            Notify("ERROR" + ex, "warn", lblMsg);
        }

    }
    private void VisibleWorkflowDateAndDay()
    {
        //string dayName = Convert.ToDateTime(txtEsclationStartTime.Text).ToString("dddd");
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnSIRID.Value + "' AND WorkFlowType = 'SIR' AND EntryBy = '" + User.Identity.Name + "'");

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
    protected void btnDraft_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
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
                                string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfSir.Text));
                                string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                string isExist = SQLQuery.ReturnString("SELECT SirVoucherNo FROM SIRFrom WHERE SirVoucherNo='" + txtSIRVoucharNo.Text.Trim() + "'  AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND Store='" + ddSaveToStore.SelectedValue + "'");
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
                                    SaveSir("Drafted");
                                    Notify("Successfully Saved...", "success", lblMsg);

                                }
                                else
                                {
                                    Notify("This " + txtSIRVoucharNo.Text + " SIR Number already exist.", "warn", lblMsg);
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
                        UpdateSir("Drafted");
                        ClearControls();
                        btnDraft.Text = "SAVE AS DRAFT";
                        Notify("Successfully Updated...", "success", lblMsg);
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
            hdnSIRVoucher.Value = "";
            hdnSIRID.Value = "";
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";
            BindGrid();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            txtSIRVoucharNo.Text = GenerateVoucherNumber.GetSirNumber(Convert.ToDateTime(txtDateOfSir.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
        }
    }

    protected void txtDateOfSir_TextChanged(object sender, EventArgs e)
    {
        txtSIRVoucharNo.Text = GenerateVoucherNumber.GetSirNumber(Convert.ToDateTime(txtDateOfSir.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
    }

    protected void txtSIRVoucharNo_TextChanged(object sender, EventArgs e)
    {
        if (txtSIRVoucharNo.Text.Length == 16)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfSir.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT SirVoucherNo FROM SIRFrom WHERE SirVoucherNo='" + txtSIRVoucharNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtSIRVoucharNo.Text + " SIR Number already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("SIR Number should be 16 characters", "warn", lblMsg);
        }
    }

    protected void ddSaveToStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSIRVoucharNo.Text = GenerateVoucherNumber.GetSirNumber(Convert.ToDateTime(txtDateOfSir.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
        BindDdProductId();
        BindGrid();
    }



    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }
}
