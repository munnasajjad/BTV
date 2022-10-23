using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

namespace app
{
    public partial class app_LoanVoucher : System.Web.UI.Page
    {
        private SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            this.Page.Form.Enctype = "multipart/form-data";
            if (!IsPostBack)
            {
                LoadEmployee();
                BindDesignation();
                BindSaveToStore();
                BindEmployee();
                bindCategoryId();
                BindProductSubCategory();
                BindDdProductId();
                txtDateofLv.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtLoanVoucharNo.Text = GenerateVoucherNumber.GetLvNumber(Convert.ToDateTime(txtDateofLv.Text), User.Identity.Name.Trim(), ddSaveToStore.SelectedValue);
                BindPriority();
                BindWorkFlowUserGridView();
                BindAddItemsGridView();
                VisibleWorkflowDateAndDay();
                VisibleEmployeOrResponsible();
                BindSaveItemsGridView();
            }
        }
        private void BindDdProductId()
        {
            //SQLQuery.PopulateDropDown("SELECT  Product.Name+'-'+ProductDetails.SerialNo AS ProductName,ProductDetails.ProductDetailsID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddlStore.SelectedValue + "'", ddProductID, "ProductDetailsID", "ProductName");
            SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product.Name + '-' + ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID
                                FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductID Where Product.ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND Product.ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "' AND ProductDetails.StoreID='" + ddSaveToStore.SelectedValue + "' AND ProductDetails.Status='True'", ddProductID, "ProductDetailsID", "ProductName");
        }

        private void LoadEmployee()
        {
            SQLQuery.PopulateDropDown("Select EmployeeID, Name+' ('+ Mobile+')' AS Name from Employee WHERE EmployeeID NOT IN( SELECT EmployeeInfoID FROM Logins WHERE  (LoginUserName = '" + User.Identity.Name + "')) AND LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'", ddlEmployee, "EmployeeID", "Name");
        }
        private void BindPriority()
        {
            SQLQuery.PopulateDropDown("SELECT  SequenceId,SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'LV')", ddlPriority, "Priority", "SequenceBan");
        }

        private void BindDesignation()
        {
            string query = @"SELECT DesignationID, Name, Description, RoleID, Priority FROM Designation";
            SQLQuery.PopulateDropDown(query, ddlDesignation, "DesignationID", "Name");
        }
        protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEmployee();
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
        }
        private void BindEmployee()
        {
            string query = @"SELECT DesignationWithEmployee.Id, Employee.Name + ', ' + Designation.Name AS Name
                  FROM DesignationWithEmployee INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE DesignationWithEmployee.DesignationID='" + ddlDesignation.SelectedValue + "' AND Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
            SQLQuery.PopulateDropDown(query, ddEmployee, "Id", "Name");
        }


        private void Notify(string msg, string type, Label lblNotify)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
            //Types: success, info, warn, error
            lblNotify.Attributes.Add("class", "xerp_" + type);
            lblNotify.Text = msg;
        }
        private void SaveData(string saveMode)
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
                    //string fileName = file.Replace(file, "Document-" + txtLoanVoucharNo.Text.Trim() + tExt);
                    string fileName = file.Replace(file, "LV-" + Guid.NewGuid().GetHashCode() + tExt);
                    if (fileName != "")
                    {
                        SQLQuery.DeleteFile(Server.MapPath(fileName));
                    }
                    document.SaveAs(Server.MapPath("./Uploads/LV/") + fileName);
                    docUrl = "./Uploads/LV/" + fileName;
                    insertQuery = "DocumentUrl,";
                    parameter = "@DocumentUrl,";
                }
                catch (Exception ex)
                {
                    Notify("ERROR" + ex.ToString(), "error", lblMsg);
                }

            }

            string date = Convert.ToDateTime(txtDateofLv.Text).ToString("yyyy-MM-dd");

            if (saveMode == "Submitted")
            {
                insertQuery += "SubmitDate,";
                parameter += "@SubmitDate,";
            }
            string responsiblePerson = "";
            string employee = "";
            string gDivision = "";
            if (ddlLoanType.SelectedValue == "Employee")
            {
                responsiblePerson = "";
                gDivision = SQLQuery.GetLocationNameByEmpID(ddlEmployee.SelectedValue);
                employee = ddlEmployee.SelectedValue;
            }
            else
            {
                gDivision = txtDivision.Text;
                responsiblePerson = txtResponsiblePerson.Text;
                employee = "0";
            }
            SqlCommand command = new SqlCommand(@"INSERT INTO LoanVouchar (" + insertQuery + @" LvInvoiceNo, DateofLv, LoanType, LoanToEmployee,Division, ResponsiblePerson, CauseOfLoan, LocationID, Store, FinYear,Remarks,PreparedBy, EntryBy, EntryDate, SaveMode)
                                            VALUES (" + parameter + "@LvInvoiceNo,@DateofLv,@LoanType,@LoanToEmployee,@Division,@ResponsiblePerson,@CauseOfLoan,@LocationID,@StoreID,@FinYear,@Remarks,@PreparedBy, @EntryBy, @EntryDate,@SaveMode)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            string lName = Page.User.Identity.Name.ToString();
            command.Parameters.Add("@LvInvoiceNo", SqlDbType.VarChar).Value = txtLoanVoucharNo.Text.Trim();
            command.Parameters.Add("@DateofLv", SqlDbType.DateTime).Value = date;
            command.Parameters.Add("@LoanType", SqlDbType.VarChar).Value = ddlLoanType.SelectedValue;
            command.Parameters.Add("@LoanToEmployee", SqlDbType.Int).Value = employee;
            command.Parameters.Add("@Division", SqlDbType.NVarChar).Value = gDivision;
            command.Parameters.Add("@ResponsiblePerson", SqlDbType.VarChar).Value = responsiblePerson;
            command.Parameters.Add("@CauseOfLoan", SqlDbType.NVarChar).Value = txtCauseOfLoan.Text.Trim();
            command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
            command.Parameters.Add("@StoreID", SqlDbType.Int).Value = ddSaveToStore.SelectedValue;
            command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateofLv.Text));
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;
            command.Parameters.Add("@PreparedBy", SqlDbType.Int).Value = SQLQuery.GetEmployeeID(lName);
            command.Parameters.Add("@SaveMode", SqlDbType.VarChar).Value = saveMode;
            command.Parameters.Add("@EntryDate", SqlDbType.VarChar).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            if (document.HasFile)
            {
                command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
            }
            if (saveMode == "Submitted")
            {
                command.Parameters.Add("@SubmitDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
            }
            command.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();

            string query = "";
            if (hdnLvID.Value == "")
            {
                query = " AND EntryBy='" + lName + "'";
            }
            string lvId = SQLQuery.ReturnString("SELECT MAX(IDLvNo) AS lvId FROM LoanVouchar WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
            SQLQuery.ExecNonQry("UPDATE LVProduct SET LVVoucher='" + txtLoanVoucharNo.Text + "',IDLVNo='" + lvId + "'  WHERE IDLVNo='" + hdnLvID.Value + "'" + query);
            SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + lvId + "', VoucherNo='" + txtLoanVoucharNo.Text.Trim() + "'  WHERE WorkFlowTypeID = '0' AND WorkFlowType='LV' AND EntryBy='" + lName + "' ");

            if (saveMode == "Submitted")
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + lvId + "' AND WorkFlowType='LV'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), lvId);
                    }
                }

            }

        }
        private void NotifyToEmployee(string employeeID, string lvNumber, string lvId)
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

                SQLQuery.ExecNonQry("UPDATE LoanVouchar SET CurrentWorkflowUser='" + name + "' WHERE IDLvNo = '" + lvId + "'");
                SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + lvNumber, emailBody);

            }
        }
        private void UpdateData(string saveMode)
        {
            string updateQuery = "";
            string returnUser = "";
            string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM LoanVouchar WHERE IDLvNo='" + hdnLvID.Value + "'");
            if (workflowStatus == "Return" && saveMode == "Submitted")
            {
                returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM LoanVouchar WHERE IDLvNo='" + hdnLvID.Value + "'");
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
                    string fileName = file.Replace(file, "LV-" + Guid.NewGuid().GetHashCode() + tExt);
                    if (hdnDocumentUrl.Value != "")
                    {
                        SQLQuery.DeleteFile(Server.MapPath(hdnDocumentUrl.Value));
                    }
                    document.SaveAs(Server.MapPath("./Uploads/LV/") + fileName);
                    docUrl = "./Uploads/LV/" + fileName;
                    updateQuery += "DocumentUrl=@DocumentUrl,";

                }
                catch (Exception ex)
                {
                    Notify("ERROR" + ex.ToString(), "error", lblMsg);
                }

            }
            string employee = "";
            string responsiblePerson = "";
            string gDivision = "";
            if (ddlLoanType.SelectedValue == "Employee")
            {
                gDivision = SQLQuery.GetLocationNameByEmpID(ddlEmployee.SelectedValue);
                responsiblePerson = "";
                employee = ddlEmployee.SelectedValue;
            }
            else
            {
                gDivision = txtDivision.Text;
                responsiblePerson = txtResponsiblePerson.Text;
                employee = "0";
            }
            SqlCommand command = new SqlCommand("Update  LoanVouchar SET DateofLv=@DateofLv, " + updateQuery + "LoanType=@LoanType,LoanToEmployee=@LoanToEmployee,Division=@Division,ResponsiblePerson=@ResponsiblePerson,CauseOfLoan=@CauseOfLoan, Store= @Store, FinYear=@FinYear,  Remarks= @Remarks,  PreparedBy= @PreparedBy,SaveMode=@SaveMode,WorkflowStatus=@WorkflowStatus, EntryBy=@EntryBy WHERE IDLvNo='" + hdnLvID.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            string lName = Page.User.Identity.Name.ToString();
            command.Parameters.Add("@DateofLv", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateofLv.Text).ToString("yyyy-MM-dd");
            command.Parameters.Add("@LoanType", SqlDbType.VarChar).Value = ddlLoanType.SelectedValue;
            command.Parameters.Add("@LoanToEmployee", SqlDbType.Int).Value = employee;
            command.Parameters.Add("@Division", SqlDbType.NVarChar).Value = gDivision;
            command.Parameters.Add("@ResponsiblePerson", SqlDbType.VarChar).Value = responsiblePerson;
            command.Parameters.Add("@CauseOfLoan", SqlDbType.VarChar).Value = txtCauseOfLoan.Text.Trim();
            command.Parameters.Add("@Store", SqlDbType.Int).Value = ddSaveToStore.SelectedValue;
            command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateofLv.Text));
            command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;
            command.Parameters.Add("@PreparedBy", SqlDbType.Int).Value = SQLQuery.GetEmployeeID(lName);
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
            command.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
            command.Connection.Dispose();

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
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnLvID.Value);
                    }
                }
                else
                {
                    string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnLvID.Value + "' AND WorkFlowType='LV'";

                    DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                    foreach (DataRow item in dtUser.Rows)
                    {
                        if (item["Priority"].ToString() == "1")
                        {
                            DateTime startDateTime = DateTime.Now;
                            DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                            SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                            NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnLvID.Value);
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
                                    string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateofLv.Text));
                                    string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                    string isExist = SQLQuery.ReturnString("SELECT LvInvoiceNo FROM LoanVouchar WHERE LvInvoiceNo='" + txtLoanVoucharNo.Text.Trim() + "'  AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND Store='" + ddSaveToStore.SelectedValue + "'");
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
                                        Notify("This " + txtLoanVoucharNo.Text + " LV Number already exist.", "warn", lblMsg);
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
                            btnSave.Text = "Submit";
                            btnDraft.Text = "SAVE AS DRAFT";
                            ClearControls();
                            btnDraft.Enabled = true;
                            Notify("Successfully Submitted...", "success", lblMsg);
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
                hdnLvVoucher.Value = "";
                hdnLvID.Value = "";
                hdnProductId.Value = "";
                hdnWorkFlowUserId.Value = "";
                BindSaveItemsGridView();
                BindAddItemsGridView();
                BindWorkFlowUserGridView();
                txtLoanVoucharNo.Text = GenerateVoucherNumber.GetLvNumber(Convert.ToDateTime(txtDateofLv.Text), User.Identity.Name.Trim(), ddSaveToStore.SelectedValue);
            }
        }
        private void EditMode(string lblId)
        {
            hdnLvID.Value = lblId;

            DataTable dt = SQLQuery.ReturnDataTable(@"SELECT  IDLvNo, LvInvoiceNo, DateofLv, LoanType, LoanToEmployee, ResponsiblePerson, CauseOfLoan, LocationID, Store, FinYear, DocumentUrl, Remarks, Verifier, RequisitionBy, DeliveredBy, PreparedBy, PreparedDate, EntryBy, 
                    EntryDate, SaveMode, SubmitDate, WorkflowStatus,  ReturnOrHoldUserID, CurrentWorkflowUser FROM LoanVouchar WHERE IDLvNo='" + lblId + "'");
            foreach (DataRow dtx in dt.Rows)
            {
                hdnLvVoucher.Value = dtx["LvInvoiceNo"].ToString();
                txtLoanVoucharNo.Text = dtx["LvInvoiceNo"].ToString();
                txtDateofLv.Text = Convert.ToDateTime(dtx["DateofLv"]).ToString("dd/MM/yyyy");
                ddlLoanType.SelectedValue = dtx["LoanType"].ToString();
                hdnDocumentUrl.Value = dtx["DocumentUrl"].ToString();
                if (ddlLoanType.SelectedValue == "Employee")
                {
                    LoadEmployee();
                    ddlEmployee.SelectedValue = dtx["LoanToEmployee"].ToString();
                }
                else
                {
                    txtResponsiblePerson.Text = dtx["ResponsiblePerson"].ToString();
                }
                VisibleEmployeOrResponsible();
                txtCauseOfLoan.Text = dtx["CauseOfLoan"].ToString();
                ddSaveToStore.SelectedValue = dtx["Store"].ToString();
                txtRemarks.Text = dtx["Remarks"].ToString();
            }
            btnSave.Text = "Submit";
            btnDraft.Text = "Update Draft";
            Notify("Edit mode activated ...", "info", lblMsg);
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            VisibleWorkflowDateAndDay();
        }
        protected void SaveItemsGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string lName = Page.User.Identity.Name.ToString();
                if (SQLQuery.OparatePermission(lName, "Update") == "1")
                {
                    int index = Convert.ToInt32(SaveItemsGridView.SelectedIndex);
                    Label lblEditId = SaveItemsGridView.Rows[index].FindControl("lblIDLvNo") as Label;
                    Label lblLvInvoiceNo = SaveItemsGridView.Rows[index].FindControl("lblLvInvoiceNo") as Label;
                    Label lblEntryBy = SaveItemsGridView.Rows[index].FindControl("lblEntryBy") as Label;
                    string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM LoanVouchar WHERE IDLvNo='" + lblEditId.Text + "'");
                    string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM LoanVouchar WHERE IDLvNo='" + lblEditId.Text + "'");

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
                            Notify("This " + lblLvInvoiceNo.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
                        }
                    }
                    else
                    {
                        Notify("This voucher " + lblLvInvoiceNo.Text + " entry by user is " + lblEntryBy.Text + ". You are not authorize edit the voucher. If you need to any change please contact higher authority.", "warn", lblMsg);
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
        private void DeleteLVData(string lblId)
        {
            string query = @"Delete LoanVouchar WHERE IDLvNo='" + lblId + "'";
            query += " Delete LVProduct WHERE IDLVNo='" + lblId + "'";
            query += " Delete WorkFlowUser WHERE WorkFlowTypeID='" + lblId + "' AND WorkFlowType='LV'";
            SQLQuery.ExecNonQry(query);
            BindSaveItemsGridView();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            VisibleWorkflowDateAndDay();

            Notify("Successfully Deleted...", "success", lblMsg);
            txtLoanVoucharNo.Text = GenerateVoucherNumber.GetLvNumber(Convert.ToDateTime(txtDateofLv.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
        }
        protected void SaveItemsGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string lName = Page.User.Identity.Name.ToString();
                if (SQLQuery.OparatePermission(lName, "Delete") == "1")
                {
                    int index = Convert.ToInt32(e.RowIndex);
                    Label lblId = SaveItemsGridView.Rows[index].FindControl("lblIDLvNo") as Label;

                    Label lblLvInvoiceNo = SaveItemsGridView.Rows[index].FindControl("lblLvInvoiceNo") as Label;
                    Label lblEntryBy = SaveItemsGridView.Rows[index].FindControl("lblEntryBy") as Label;
                    string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM LoanVouchar WHERE IDLvNo='" + lblId.Text + "'");
                    string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM LoanVouchar WHERE IDLvNo='" + lblId.Text + "'");
                    if (Page.User.IsInRole("Super Admin"))
                    {
                        if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Pending"))
                        {
                            DeleteLVData(lblId.Text);
                        }

                    }
                    else
                    {
                        if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                        {
                            DeleteLVData(lblId.Text);
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
            catch (Exception ex)
            {
                Notify("ERROR:" + ex, "error", lblMsg);

            }

        }
        protected void btnClear_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("./Default.aspx");
        }

        private void BindSaveItemsGridView()
        {
            string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";
            string query = "";
            if (!Page.User.IsInRole("Super Admin"))
            {
                query = "AND LoanVouchar.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
            }
            DataTable dt = SQLQuery.ReturnDataTable(@"SELECT '" + baseUrl + "'+'LVReport.aspx?LvId=' + CONVERT(VARCHAR, LoanVouchar.IDLvNo) AS LvId, LoanVouchar.IDLvNo, LoanVouchar.LvInvoiceNo, CONVERT(varchar, LoanVouchar.DateofLv, 103) AS DateofLv, LoanVouchar.DocumentUrl, LoanVouchar.SaveMode, LoanVouchar.WorkflowStatus, LoanVouchar.CurrentWorkflowUser, LoanVouchar.Remarks, LoanVouchar.LoanType, LoanVouchar.EntryBy, Employee.Name AS PreparedBy,Store.Name As StoreName FROM LoanVouchar INNER JOIN Employee ON LoanVouchar.PreparedBy = Employee.EmployeeID INNER JOIN Store ON LoanVouchar.Store = Store.StoreAssignID WHERE  Store='" + ddSaveToStore.SelectedValue + "'" + query + " ORDER BY LoanVouchar.EntryDate DESC");
            SaveItemsGridView.DataSource = dt;
            SaveItemsGridView.DataBind();
        }
        protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsDetailProduct(ddProductID.SelectedValue);
        }
        private void IsDetailProduct(string id)
        {
            string isDetail = SQLQuery.ReturnString(@"SELECT Product.ProductType FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + id + "')");
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
            //txtDateofLv.Text = "";
            txtResponsiblePerson.Text = "";
            txtCauseOfLoan.Text = "";
            //txtDivision.Text=""; 
            //txtProductDescription.Text = "";
            txtQtyNeed.Text = "";
            txtQtyDelivered.Text = "";
            txtDeliveredDate.Text = "";
            txtQtyReturn.Text = "";
            txtProductCondition.Text = "";
            // txtVerifier.Text = "";
            txtRemarks.Text = "";
        }
        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            try
            {
                string lName = Page.User.Identity.Name.ToString();
                string isProductExists = SQLQuery.ReturnString("SELECT ProductDetailsID FROM LVProduct WHERE ProductDetailsID = '" + ddProductID.SelectedValue + "' AND IDLVNo = '" + hdnLvID.Value + "' AND EntryBy = '" + lName + "'");
                SQLQuery.Empty2Zero(txtQtyDelivered);
                if (btnAdd.Text.ToUpper() == "ADD PRODUCT")
                {
                    if (isProductExists != ddProductID.SelectedValue)
                    {
                        if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                        {
                            SQLQuery.Empty2Zero(txtQtyNeed);
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
                                InsertToLvProduct();
                                if (productType == "Detail")
                                {
                                    SQLQuery.UpdateProductStatus("LV", ddProductID.SelectedValue);
                                }
                                Notify("Insert Successful", "info", lblMsg);
                                BindAddItemsGridView();
                                ClearProductField();
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
                        Notify("Update Successful", "info", lblMsg);
                        ClearProductField();
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
        }
        private void ClearProductField()
        {
            //ddCategoryID.SelectedValue = "0";
            // ddProductSubCategory.SelectedValue = "0";
            ddProductID.SelectedValue = "0";
            txtQtyNeed.Text = "";
            txtProductCondition.Text = "";
            txtQtyDelivered.Text = "";
            txtDeliveredDate.Text = "";
        }
        private void InsertToLvProduct()
        {
            int lvNo;
            if (hdnLvID.Value == "")
            {
                lvNo = 0;
            }
            else
            {
                lvNo = Convert.ToInt32(hdnLvID.Value);
            }
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand command;
            if (btnDraft.Text.ToUpper() != "UPDATE DRAFT")
            {
                command = new SqlCommand(@"INSERT INTO LVProduct (IDLVNo, CategoryID, SubCategoryID, ProductID,StoreID,ProductDetailsID, QTYNeed, NeedQtyInWords, QTYDelivered, DeliveredQtyInWords, DeliveredDate, ProductCondition, EntryBy, EntryDate) 
                                       VALUES ('',@CategoryID,@SubCategoryID,@ProductID,@StoreID,@ProductDetailsID, @QTYNeed, @NeedQtyInWords, @QTYDelivered, @DeliveredQtyInWords, @DeliveredDate,@ProductCondition,@EntryBy,@EntryDate)", connection);
            }
            else
            {
                command = new SqlCommand(@"INSERT INTO LVProduct (IDLVNo,LVVoucher, CategoryID, SubCategoryID, ProductID,StoreID,ProductDetailsID, QTYNeed, NeedQtyInWords, QTYDelivered, DeliveredQtyInWords, DeliveredDate, ProductCondition, EntryBy, EntryDate) 
                                       VALUES (@IDLVNo,@LVVoucher,@CategoryID,@SubCategoryID,@ProductID,@StoreID,@ProductDetailsID, @QTYNeed, NeedQtyInWords, @QTYDelivered, @DeliveredQtyInWords, @DeliveredDate, @ProductCondition,@EntryBy,@EntryDate)", connection);


            }
            command.Parameters.AddWithValue("@IDLVNo", lvNo);
            command.Parameters.AddWithValue("@LVVoucher", hdnLvVoucher.Value);
            command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
            command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
            command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
            command.Parameters.AddWithValue("@ProductID", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
            command.Parameters.AddWithValue("@StoreID", ddSaveToStore.SelectedValue);
            command.Parameters.AddWithValue("@QTYNeed", Convert.ToInt32(txtQtyNeed.Text));
            command.Parameters.AddWithValue("@NeedQtyInWords", SQLQuery.Int2WordsBangla(txtQtyNeed.Text.ToString()));
            command.Parameters.AddWithValue("@QTYDelivered", Convert.ToInt32(txtQtyDelivered.Text));
            command.Parameters.AddWithValue("@DeliveredQtyInWords", SQLQuery.Int2WordsBangla(txtQtyDelivered.Text.ToString()));
            command.Parameters.AddWithValue("@DeliveredDate", Convert.ToDateTime(txtDeliveredDate.Text).ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@ProductCondition", txtProductCondition.Text);
            command.Parameters.AddWithValue("@EntryBy", lName);
            command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
        }
        private void UpdateLvProduct()
        {
            string lName = Page.User.Identity.Name.ToString();
            string query = @"UPDATE LVProduct SET  CategoryID=@CategoryID,SubCategoryID=@SubCategoryID,ProductID=@ProductID,StoreID=@StoreID, QTYNeed=@QTYNeed, NeedQtyInWords=@NeedQtyInWords, QTYDelivered=@QTYDelivered, DeliveredQtyInWords=@DeliveredQtyInWords, ProductCondition=@ProductCondition, DeliveredDate=@DeliveredDate  WHERE LVProductID = '" + hdnProductId.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
            command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
            command.Parameters.AddWithValue("@ProductID", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
            command.Parameters.AddWithValue("@StoreID", ddSaveToStore.SelectedValue);
            command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
            command.Parameters.AddWithValue("@QTYNeed", Convert.ToInt32(txtQtyNeed.Text));
            command.Parameters.AddWithValue("@NeedQtyInWords", SQLQuery.Int2WordsBangla(txtQtyNeed.Text.ToString()));
            command.Parameters.AddWithValue("@QTYDelivered", txtQtyDelivered.Text);
            command.Parameters.AddWithValue("@DeliveredQtyInWords", SQLQuery.Int2WordsBangla(txtQtyDelivered.Text.ToString()));
            command.Parameters.AddWithValue("@ProductCondition", txtProductCondition.Text);
            command.Parameters.AddWithValue("@DeliveredDate", Convert.ToDateTime(txtDeliveredDate.Text));
            //command.Parameters.AddWithValue("@EntryBy", lName);
            //command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();
            command.Connection.Dispose();
        }
        private void BindAddItemsGridView()
        {
            string lName = Page.User.Identity.Name.ToString();

            string query = "";
            if (Page.User.IsInRole("Super Admin") && hdnLvID.Value != "")
            {
                query = @"WHERE IDLVNo = '" + hdnLvID.Value + "'";
            }
            else
            {
                query = @"WHERE IDLVNo = '" + hdnLvID.Value + "' AND LVProduct.EntryBy = '" + lName + "'";
            }
            //string query = @"SELECT LVProductID, Product.Name AS ProductName, LVProduct.QTYNeed, LVProduct.QTYDelivered, convert(varchar, LVProduct.DeliveredDate, 103) AS DeliveredDate FROM LVProduct INNER JOIN Product ON LVProduct.ProductID = Product.ProductID WHERE IDLVNo = '" + hdnLvID.Value + "' AND EntryBy = '" + lName + "'";
            string sql = @"SELECT LVProduct.LVProductID, CASE ProductDetails.SerialNo WHEN '0' THEN Product .Name ELSE Product .Name + '-' + ProductDetails.SerialNo END AS ProductName, LVProduct.QTYNeed, LVProduct.QTYDelivered, CONVERT(varchar, LVProduct.DeliveredDate, 103) AS DeliveredDate, LVProduct.ProductID
                        FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN LVProduct ON ProductDetails.ProductDetailsID = LVProduct.ProductDetailsID " + query + "";

            SqlCommand command = new SqlCommand(sql, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            command.Connection.Open();
            AddItemsGridView.EmptyDataText = "No data added ...";
            AddItemsGridView.DataSource = command.ExecuteReader();
            AddItemsGridView.DataBind();
            command.Connection.Close();
            command.Connection.Dispose();
        }
        protected void AddItemsGridView_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
                Label label = AddItemsGridView.Rows[index].FindControl("lblLVProductID") as Label;
                hdnProductId.Value = label.Text;
                string query = @"SELECT LVProductID, ProductID,ProductDetailsID,CategoryID,SubCategoryID, QTYNeed, QTYDelivered, DeliveredDate,ProductCondition FROM LVProduct WHERE LVProductID = '" + hdnProductId.Value + "'";
                SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                command.Connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    btnAdd.Text = "Update";
                    bindCategoryId();
                    ddCategoryID.SelectedValue = dataReader["CategoryID"].ToString();
                    BindProductSubCategory();
                    ddProductSubCategory.SelectedValue = dataReader["SubCategoryID"].ToString();
                    BindDdProductId();
                    ddProductID.SelectedValue = dataReader["ProductDetailsID"].ToString();
                    IsDetailProduct(ddProductID.SelectedValue);
                    txtQtyNeed.Text = dataReader["QTYNeed"].ToString();
                    txtQtyDelivered.Text = dataReader["QTYDelivered"].ToString();
                    txtDeliveredDate.Text = Convert.ToDateTime(dataReader["DeliveredDate"]).ToString("dd/MM/yyyy");
                    txtProductCondition.Text = dataReader["ProductCondition"].ToString();
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
        protected void AddItemsGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string lName = Page.User.Identity.Name;
                //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
                //{
                int index = Convert.ToInt32(e.RowIndex);
                Label lblId = AddItemsGridView.Rows[index].FindControl("lblLVProductID") as Label;
                string productDetailsId = SQLQuery.ReturnString("SELECT ProductDetailsID FROM LVProduct WHERE(LVProductID = '" + lblId.Text + "')");
                SQLQuery.UpdateProductStatus("Available", productDetailsId);
                SQLQuery.ExecNonQry("Delete LVProduct FROM LVProduct WHERE LVProductID='" + lblId.Text + "' ");
                BindAddItemsGridView();
                Notify("Successfully Deleted...", "success", lblMsg);
                //}
                //else
                //{
                //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                //}
            }
            catch (Exception exception)
            {
                Notify("ERROR: " + exception, "error", lblMsg);
            }

        }


        private void VisibleWorkflowDateAndDay()
        {
            DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnLvID.Value + "' AND WorkFlowType = 'LV' AND EntryBy = '" + User.Identity.Name + "'");

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
        protected void btnWorkFlowSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                string lName = Page.User.Identity.Name;
                string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hdnLvID.Value + "' AND WorkFlowType = 'LV' AND EntryBy = '" + lName + "'");
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
                            Notify("Update Successful", "info", lblMsg);
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

        private void InsertToWorkFlowUser()
        {
            string lName = Page.User.Identity.Name.ToString();
            SqlCommand command;
            int typeId;
            if (hdnLvID.Value == "")
            {
                typeId = 0;
            }
            else
            {
                typeId = Convert.ToInt32(hdnLvID.Value);
            }
            command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,VoucherNo,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,@VoucherNo,'LV',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            command.Parameters.AddWithValue("@WorkFlowTypeID", typeId);
            command.Parameters.AddWithValue("@VoucherNo", hdnLvVoucher.Value);
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
            string query = @"UPDATE WorkFlowUser SET EmployeeID=@EmployeeID,VoucherNo=@VoucherNo,DesignationId=@DesignationId,EsclationDay=@EsclationDay, Priority=@Priority, Remark=@Remark, EntryBy=@EntryBy, EntryDate=@EntryDate WHERE WorkFlowUserID = '" + hdnWorkFlowUserId.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Parameters.AddWithValue("@DesignationId", ddlDesignation.SelectedValue);
            command.Parameters.AddWithValue("@VoucherNo", hdnLvVoucher.Value);
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

        private void BindWorkFlowUserGridView()
        {
            string lName = Page.User.Identity.Name.ToString();
            string query = "";
            if (User.IsInRole("Super Admin") && hdnLvID.Value != "")
            {
                query = "AND WorkFlowTypeID='" + hdnLvID.Value + "'";
            }
            else
            {
                query = "AND WorkFlowTypeID='" + hdnLvID.Value + "' AND EntryBy = '" + lName + "'";
            }

            string sql = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  WorkFlowUser.WorkFlowType = 'LV' " + query + " Order By Priority ASC";

            SqlCommand command = new SqlCommand(sql, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Connection.Open();
            WorkFlowUserGridView.EmptyDataText = "No data added ...";
            WorkFlowUserGridView.DataSource = command.ExecuteReader();
            WorkFlowUserGridView.DataBind();
            command.Connection.Close();
            command.Connection.Dispose();
        }

        private bool PrioritySequenceCheck()
        {
            string lName = Page.User.Identity.Name.ToString();
            bool sequentStatus = false;
            int priority = int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(MAX(Priority),0) FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnLvID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'LV'"));
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

        private bool PriorityCheck()
        {
            string lName = Page.User.Identity.Name.ToString();
            bool priorityStatus = true;
            DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnLvID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'LV'");

            foreach (DataRow priorityDataRow in priorityDataTable.Rows)
            {
                string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnLvID.Value + "' AND WorkFlowType = 'LV'");
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
            DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnLvID.Value + "' AND WorkFlowType = 'LV'");
            foreach (DataRow priorityDataRow in priorityDataTable.Rows)
            {
                if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
                {
                    string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnLvID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'LV'");
                    if (int.Parse(priority) > 0)
                    {
                        priorityStatus = false;
                    }
                }

            }
            return priorityStatus;
        }


        protected void WorkFlowUserGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
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
            catch (Exception ex)
            {

                Notify("ERROR" + ex, "error", lblMsg);
            }


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
                    BindDesignation();
                    ddlDesignation.SelectedValue = dataReader["DesignationId"].ToString();
                    BindEmployee();
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
        private void bindCategoryId()
        {
            SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
        }
        private void BindProductSubCategory()
        {
            SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
        }
        protected void ddCategoryID_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindProductSubCategory();
            BindDdProductId();

        }
        protected void ddProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDdProductId();
        }

        protected void txtDateofLv_TextChanged(object sender, EventArgs e)
        {
            txtLoanVoucharNo.Text = GenerateVoucherNumber.GetLvNumber(Convert.ToDateTime(txtDateofLv.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
        }

        protected void txtLoanVoucharNo_TextChanged(object sender, EventArgs e)
        {
            if (txtLoanVoucharNo.Text.Length == 15)
            {
                string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateofLv.Text));
                string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                string isExist = SQLQuery.ReturnString("SELECT LvInvoiceNo FROM LoanVouchar WHERE LvInvoiceNo='" + txtLoanVoucharNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
                if (isExist != "")
                {
                    Notify("This " + txtLoanVoucharNo.Text + " LV Number already exist.", "warn", lblMsg);
                }
            }
            else
            {
                Notify("Loan Voucher Number should be 15 characters", "warn", lblMsg);
            }
        }
        private void VisibleEmployeOrResponsible()
        {
            if (ddlLoanType.SelectedValue == "Employee")
            {
                trEmployee.Visible = true;
                trResponsible.Visible = false;
                trDivision.Visible = false;
            }
            else
            {
                trEmployee.Visible = false;
                trResponsible.Visible = true;
                trDivision.Visible = true;
            }
        }
        protected void ddlLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisibleEmployeOrResponsible();
        }

        protected void btnDraft_Click(object sender, EventArgs e)
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
                                        string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateofLv.Text));
                                        string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                        string isExist = SQLQuery.ReturnString("SELECT LvInvoiceNo FROM LoanVouchar WHERE LvInvoiceNo='" + txtLoanVoucharNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND Store='" + ddSaveToStore.SelectedValue + "'");
                                        if (isExist == "")
                                        {
                                            SaveData("Drafted");
                                            ClearControls();
                                            Notify("Successfully SAVE AS DRAFT...", "success", lblMsg);
                                        }
                                        else
                                        {
                                            Notify("This " + txtLoanVoucharNo.Text + " LV Number already exist.", "warn", lblMsg);
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
                hdnLvVoucher.Value = "";
                hdnLvID.Value = "";
                hdnProductId.Value = "";
                hdnWorkFlowUserId.Value = "";
                BindAddItemsGridView();
                BindWorkFlowUserGridView();
                BindSaveItemsGridView();
                VisibleWorkflowDateAndDay();
                txtLoanVoucharNo.Text = GenerateVoucherNumber.GetLvNumber(Convert.ToDateTime(txtDateofLv.Text), User.Identity.Name, ddSaveToStore.SelectedValue);
            }
        }

        private bool VerifyPrioritySequence()
        {
            string lName = Page.User.Identity.Name.Trim().ToString();
            bool sequentStatus = true;
            int priorityCount = 1;

            DataTable priorityDataTable = new DataTable();
            if (String.Empty == hdnLvID.Value)
            {
                priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnLvID.Value + "'  AND EntryBy = '" + lName + "' AND WorkFlowType = 'LV' ORDER BY Priority");
            }
            else
            {
                priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnLvID.Value + "' AND WorkFlowType = 'LV' ORDER BY Priority");
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
        protected void btnLnk_Click(object sender, EventArgs e)
        {
            Response.Redirect("Employee.aspx");
        }

        protected void ddSaveToStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtLoanVoucharNo.Text = GenerateVoucherNumber.GetLvNumber(Convert.ToDateTime(txtDateofLv.Text), User.Identity.Name.Trim(), ddSaveToStore.SelectedValue);
            BindDdProductId();
            BindSaveItemsGridView();

        }

        protected void SaveItemsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveItemsGridView.PageIndex = e.NewPageIndex;
            BindSaveItemsGridView();
        }
    }
}
