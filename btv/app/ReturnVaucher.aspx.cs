
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
using DocumentFormat.OpenXml.Drawing.Charts;
using RunQuery;
using DataTable = System.Data.DataTable;

public partial class app_ReturnVaucher : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SQLQuery.IsUserActive(User.Identity.Name);
        this.Page.Form.Enctype = "multipart/form-data";
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtRV.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindStore();
            LaodLabelWithDropdwon();
            BindddlDesignation();
            BindddEmployee();
            BindDdProductId();
            BindddPriority();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            VisibleWorkflowDateAndDay();
            BindGrid();
            txtReturnVoucharNo.Text = GenerateVoucherNumber.GetRvNumber(Convert.ToDateTime(txtRV.Text), User.Identity.Name, ddlStore.SelectedValue);
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private void BindddPriority()
    {
        SQLQuery.PopulateDropDown("SELECT  SequenceId,SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'RV')", ddlPriority, "Priority", "SequenceBan");
    }
    private void BindddEmployee()
    {
        string query = @"SELECT DesignationWithEmployee.Id, Employee.Name + ', ' + Designation.Name AS Name
                  FROM DesignationWithEmployee INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE DesignationWithEmployee.DesignationID='" + ddlDesignation.SelectedValue + "' AND Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        SQLQuery.PopulateDropDown(query, ddEmployee, "Id", "Name");
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddEmployee();
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
                                string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRV.Text));
                                string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                string isExist = SQLQuery.ReturnString("SELECT RvInvoiceNo FROM ReturnVauchar WHERE RvInvoiceNo='" + txtReturnVoucharNo.Text.Trim() + "'  AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND Store='" + ddlStore.SelectedValue + "'");
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
                                    SaveRVData("Submitted");
                                    Notify("Successfully Saved...", "success", lblMsg);
                                }
                                else
                                {
                                    Notify("This " + txtReturnVoucharNo.Text + " RV Number already exist.", "warn", lblMsg);
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
                        UpdateRVData("Submitted");
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
            hdnRVVoucher.Value = "";
            hdnRVID.Value = "";
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";
            BindGrid();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            txtReturnVoucharNo.Text = GenerateVoucherNumber.GetRvNumber(Convert.ToDateTime(txtRV.Text), User.Identity.Name, ddlStore.SelectedValue);
        }
    }
    private bool VerifyPrioritySequence()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = true;
        int priorityCount = 1;
        DataTable priorityDataTable = new DataTable();
        if (String.Empty == hdnRVID.Value)
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnRVID.Value + "'  AND EntryBy = '" + lName + "' AND WorkFlowType = 'RV' ORDER BY Priority");
        }
        else
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnRVID.Value + "' AND WorkFlowType = 'RV' ORDER BY Priority");
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

    private void UpdateRVData(string saveMode)
    {
        SQLQuery.Empty2Zero(txtDeposit);
        string updateQuery = "";
        string returnUser = "";
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM ReturnVauchar WHERE IDRvNo='" + hdnRVID.Value + "'");
        if (workflowStatus == "Return" && saveMode == "Submitted")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM ReturnVauchar WHERE IDRvNo='" + hdnRVID.Value + "'");
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
                string fileName = file.Replace(file, "RV-" + Guid.NewGuid().GetHashCode() + tExt);
                if (hdnDocumentUrl.Value != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath(hdnDocumentUrl.Value));
                }

                document.SaveAs(Server.MapPath("./Uploads/RV/") + fileName);
                docUrl = "./Uploads/RV/" + fileName;
                updateQuery += "DocumentUrl=@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }

        SqlCommand command = new SqlCommand(@"Update  ReturnVauchar SET DateOfRV=@DateOfRV," + updateQuery + "ProductReturnDivision=@ProductReturnDivision,Comments=@Comments,Store=@Store,SaveMode=@SaveMode, FinYear=@FinYear WHERE IDRvNo='" + hdnRVID.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@RvInvoiceNo", txtReturnVoucharNo.Text);
        command.Parameters.AddWithValue("@DateOfRV", Convert.ToDateTime(txtRV.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRV.Text));
        command.Parameters.AddWithValue("@ProductReturnDivision", txtProductReturnDivision.Text);
        command.Parameters.AddWithValue("@ProductReturnCause", txtProductReturnCause.Text);
        command.Parameters.AddWithValue("@Store", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Comments", txtComments.Text);
        command.Parameters.Add("@SaveMode", SqlDbType.VarChar).Value = saveMode;
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
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnRVID.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnRVID.Value + "' AND WorkFlowType='RV'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnRVID.Value);
                    }
                }
            }
        }

    }
    private void SaveRVData(string saveMode)
    {
        SQLQuery.Empty2Zero(txtDeposit);
        string insertQuery = "";
        string parameter = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "Document-" + txtReturnVoucharNo.Text.Trim() + tExt);
                if (fileName != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath("./Uploads/RV/" + fileName));
                }
                document.SaveAs(Server.MapPath("./Uploads/RV/") + fileName);
                docUrl = "./Uploads/RV/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";
            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }
        }
        string date = Convert.ToDateTime(txtRV.Text).ToString("yyyy-MM-dd");
        if (saveMode == "Submitted")
        {
            insertQuery += "SubmitDate,";
            parameter += "@SubmitDate,";
        }
        string type = "SIR";
        if (rdLoan.Checked)
        {
            type = "LV";
        }
        SqlCommand command = new SqlCommand(@"INSERT INTO ReturnVauchar (" + insertQuery + @"RvInvoiceNo,ReturnType,LvSIRVoucherNo, DateOfRV, LocationID,FinYear, ProductReturnDivision,ProductReturnCause, Store, Comments,  SaveMode, EntryBy,PreparedBy )
                                            VALUES (" + parameter + "@RvInvoiceNo,@ReturnType,@LvSIRVoucherNo, @DateOfRV, @LocationID, @FinYear, @ProductReturnDivision,@ProductReturnCause, @Store,@Comments, @SaveMode, @EntryBy,@PreparedBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        string lName = Page.User.Identity.Name.ToString();

        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
        }
        if (saveMode == "Submitted")
        {
            command.Parameters.Add("@SubmitDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }
        command.Parameters.AddWithValue("@RvInvoiceNo", txtReturnVoucharNo.Text);
        command.Parameters.AddWithValue("@ReturnType", type);
        command.Parameters.AddWithValue("@LvSIRVoucherNo", ddSIR_LVID.SelectedValue);
        command.Parameters.AddWithValue("@DateOfRV", Convert.ToDateTime(txtRV.Text).ToString("yyyy-MM-dd"));
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRV.Text));
        command.Parameters.AddWithValue("@ProductReturnDivision", txtProductReturnDivision.Text);
        command.Parameters.AddWithValue("@ProductReturnCause", txtProductReturnCause.Text);
        command.Parameters.AddWithValue("@Store", ddlStore.SelectedValue);
        command.Parameters.AddWithValue("@Comments", txtComments.Text);
        command.Parameters.Add("@SaveMode", SqlDbType.VarChar).Value = saveMode;
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.Add("@PreparedBy", SqlDbType.Int).Value = SQLQuery.GetEmployeeID(lName);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

        ClearControls();
        Notify("Successfully Saved...", "success", lblMsg);

        string query = "";
        if (hdnRVID.Value == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }

        string rvId = SQLQuery.ReturnString("SELECT MAX(IDRvNo) AS rvId FROM ReturnVauchar WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE RVProduct SET RVNo='" + rvId + "',RVVoucherNo='" + txtReturnVoucharNo.Text + "'  WHERE RVNo = '" + hdnRVID.Value + "'" + query);
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + rvId + "' , VoucherNo='" + txtReturnVoucharNo.Text.Trim() + "'  WHERE WorkFlowTypeID = '" + hdnRVID.Value + "'  AND WorkFlowType='RV' AND EntryBy='" + lName + "' ");
        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + rvId + "' AND WorkFlowType='RV'";

            DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
            foreach (DataRow item in dtUser.Rows)
            {
                if (item["Priority"].ToString() == "1")
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), rvId);
                }
            }

        }

    }
    private void NotifyToEmployee(string employeeID, string lvNumber, string rvId)
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

            SQLQuery.ExecNonQry("UPDATE ReturnVauchar SET CurrentWorkflowUser='" + name + "' WHERE IDRvNo = '" + rvId + "'");
            SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + lvNumber, emailBody);

        }
    }
    //protected void btnSave_OnClick(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        string lName = Page.User.Identity.Name.ToString();
    //        if (btnSave.Text == "Save")
    //        {
    //            if (SQLQuery.OparatePermission(lName, "Insert") == "1")
    //            {
    //                string isExist = SQLQuery.ReturnString("SELECT RvInvoiceNo FROM ReturnVauchar WHERE RvInvoiceNo='" + txtReturnVoucharNo.Text.Trim() + "'");
    //                if (isExist == "")
    //                {
    //                    //RunQuery.SQLQuery.ExecNonQry(" INSERT INTO ReturnVauchar (RvInvoiceNo, DateOfRV,LocationID,CenterID,ProductReturnDivision,ProductReturnCause,Comments,ApprovedBy,StoreHouseLedgerWritter,EntryBy) " +
    //                    //                         "VALUES ('" + txtReturnVoucharNo.Text + "','" + Convert.ToDateTime(txtRV.Text).ToString("yyyy-MM-dd") + "', '" + ddlLocation.SelectedValue + "', '" + ddlCenterID.SelectedValue + "', '" + txtProductReturnDivision.Text + "','" + txtProductReturnCause.Text + "','" + txtComments.Text + "','" + ddApprovedBy.SelectedValue + "', '" + txtStoreHouseAccountLedgerWritter.Text + "','" + lName + "')");
    //                   // ClearControls();
    //                    //Notify("Successfully Saved...", "success", lblMsg);
    //                    string RvId = SQLQuery.ReturnString("SELECT MAX(IDRvNo) AS RvId FROM ReturnVauchar");
    //                    //SQLQuery.ExecNonQry("UPDATE RVProduct SET RVNo='" + RvId + "'  WHERE RVNo = '" + LvIdHiddenField.Value + "' AND EntryBy='" + lName + "' ");
    //                   // SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + RvId + "'  WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND EntryBy='" + lName + "' ");

    //                    //string sqlQuery = @"SELECT TOP (1) WFU.WorkFlowTypeID,WFU.WorkFlowUserID, RV.RvInvoiceNo, WFU.Priority, WFU.EsclationStartTime, WFU.EsclationEndTime, WFU.TaskStatus, WFU.IsActive, Employee_1.Name, Employee_1.Email
    //                    //FROM  WorkFlowUser AS WFU INNER JOIN
    //                    //ReturnVauchar AS RV ON WFU.WorkFlowTypeID = RV.IDRvNo INNER JOIN
    //                    //Employee AS Employee_1 ON WFU.EmployeeID = Employee_1.EmployeeID
    //                    //WHERE WFU.WorkFlowTypeID='" + RvId + "' AND (WFU.TaskStatus = '1') AND (WFU.IsActive = '0') ORDER BY WFU.Priority DESC";
    //                    //DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);

    //                    //foreach (DataRow item in dt.Rows)
    //                    //{
    //                    //    string name = item["Name"].ToString();
    //                    //    string email = item["Email"].ToString();
    //                    //    string RvInvoiceNo = item["RvInvoiceNo"].ToString();
    //                    //    string wfuId = item["WorkFlowUserID"].ToString();
    //                    //    string emailBody = "Dear " + name +
    //                    //                  ", <br><br>Approve workflow, check your notification .<br><br>";

    //                    //    emailBody += " <br><br>Regards, <br><br>Development Team.";

    //                    //    SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + RvInvoiceNo, emailBody);
    //                    //    SQLQuery.ExecNonQry("Update WorkFlowUser SET IsActive='1' Where WorkFlowUserID='" + wfuId + "' AND WorkFlowType = 'RV'");
    //                    //}
    //                }
    //                else
    //                {
    //                    Notify("Return voucher number already exist.", "error", lblMsg);
    //                }
    //            }
    //            else
    //            {
    //                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //            }
    //        }
    //        else
    //        {
    //            if (SQLQuery.OparatePermission(lName, "Update") == "1")
    //            {
    //                //RunQuery.SQLQuery.ExecNonQry("Update  ReturnVauchar SET DateOfRV= '" + Convert.ToDateTime(txtRV.Text).ToString() + "', RvInvoiceNo='" + txtReturnVoucharNo.Text.Trim() + "', LocationID= '" + ddlLocation.SelectedValue + "',  ProductReturnDivision= '" + txtProductReturnDivision.Text + "',  ProductReturnCause= '" + txtProductReturnCause.Text + "',   Comments= '" + txtComments.Text + "',    ApprovedBy= '" + txtApprovedBy.Text + "',   StoreHouseAccountLedgerWritter= '" + txtStoreHouseAccountLedgerWritter.Text + "'   WHERE IDRvNo='" + lblId.Text + "' ");

    //                //ClearControls();
    //                //btnSave.Text = "Save";
    //                //Notify("Successfully Updated...", "success", lblMsg);
    //            }
    //            else
    //            {
    //                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Notify(ex.ToString(), "error", lblMsg);
    //    }
    //    finally
    //    {
    //        hdnRVID.Value = "";
    //        BindGrid();
    //        BindAddItemsGridView();
    //        BindWorkFlowUserGridView();
    //        txtReturnVoucharNo.Text = GenerateVoucherNumber.GetRVNumber(Convert.ToDateTime(txtRV.Text), User.Identity.Name);
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
                Label lblEditId = GridView1.Rows[index].FindControl("Label1") as Label;
                Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
                Label lblRvVoucherNo = GridView1.Rows[index].FindControl("lblRVInvoiceNo") as Label;
                hdnRVID.Value = lblEditId.Text;
                lblId.Text = lblEditId.Text;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM ReturnVauchar WHERE IDRvNo='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM ReturnVauchar WHERE IDRvNo='" + lblEditId.Text + "'");
                hdnMode.Value = "EditMode";
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
                        Notify("This " + lblRvVoucherNo.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
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

    private void EditMode(string id)
    {
        DataTable dt = SQLQuery.ReturnDataTable(" Select IDRvNo, RvInvoiceNo,Store,ReturnType, LvSIRVoucherNo, DateOfRV,  ProductReturnDivision, ProductReturnCause, Comments, PreparedBy, ApprovedBy,DocumentUrl,  EntryBy, EntryDate FROM ReturnVauchar WHERE IDRvNo='" + id + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            txtRV.Text = Convert.ToDateTime(dtx["DateOfRV"]).ToString("dd/MM/yyyy");
            txtReturnVoucharNo.Text = dtx["RvInvoiceNo"].ToString();
            txtProductReturnDivision.Text = dtx["ProductReturnDivision"].ToString();
            txtProductReturnCause.Text = dtx["ProductReturnCause"].ToString();
            txtComments.Text = dtx["Comments"].ToString();
            ddlStore.SelectedValue = dtx["Store"].ToString();
            hdnDocumentUrl.Value = dtx["DocumentUrl"].ToString();
            if (dtx["ReturnType"].ToString() == "LV")
            {
                rdLoan.Checked = true;
                rdSir.Checked = false;
                rdSir.Enabled = false;
            }
            else
            {
                rdSir.Checked = true;
                rdLoan.Checked = false;
                rdLoan.Enabled = false;
            }

            LaodLabelWithDropdwon();

            ddSIR_LVID.SelectedValue = dtx["LvSIRVoucherNo"].ToString();
            BindDdProductId();
            ddSIR_LVID.Enabled = false;
            ddlStore.Enabled = false;
            txtReturnVoucharNo.Enabled = false;

        }
        BindAddItemsGridView();
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();
        btnSave.Text = "Submit";
        btnDraft.Text = "Update Draft";
        hdnMode.Value = "";
        Notify("Edit mode activated ...", "info", lblMsg);
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            Label lblRVInvoiceNo = GridView1.Rows[index].FindControl("lblRVInvoiceNo") as Label;
            Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
            string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM ReturnVauchar WHERE IDRvNo='" + lblId.Text + "'");
            string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM ReturnVauchar WHERE IDRvNo='" + lblId.Text + "'");
            if (Page.User.IsInRole("Super Admin"))
            {
                if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Pending"))
                {
                    DeleteSIRData(lblId.Text);
                }
            }
            else
            {
                if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                {
                    DeleteSIRData(lblId.Text);
                }
                else
                {
                    Notify("This " + lblRVInvoiceNo.Text + " already submitted. If you need to delete. please contact higher authority.", "warn", lblMsg);
                }
            }



        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }

    private void DeleteSIRData(string lblId)
    {
        string query = "Delete ReturnVauchar WHERE IDRvNo='" + lblId + "'";
        query += " Delete WorkFlowUser WHERE WorkFlowTypeID='" + lblId + "' AND WorkFlowType = 'RV'";
        query += " Delete RVProduct WHERE RVNo='" + lblId + "'";
        RunQuery.SQLQuery.ExecNonQry(query);
        BindGrid();
        BindAddItemsGridView();
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();
        Notify("Successfully Deleted...", "success", lblMsg);
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
            query = "AND ReturnVauchar.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT ReturnVauchar.IDRvNo, ReturnVauchar.RvInvoiceNo, ReturnVauchar.DateOfRV, ReturnVauchar.LocationID, ReturnVauchar.ProductReturnDivision, ReturnVauchar.Store, ReturnVauchar.ReturnType, ReturnVauchar.LvSIRVoucherNo, 
                  ReturnVauchar.ProductReturnCause, ReturnVauchar.FinYear, ReturnVauchar.Comments, ReturnVauchar.DocumentUrl, ReturnVauchar.SaveMode, ReturnVauchar.SubmitDate, ReturnVauchar.WorkflowStatus, ReturnVauchar.WorkflowApprovedDate, ReturnVauchar.ReturnOrHoldUserID, ReturnVauchar.CurrentWorkflowUser, ReturnVauchar.Receivedby, ReturnVauchar.ReceivedDate, ReturnVauchar.ApprovedBy,
                  ReturnVauchar.ApprovedDate, ReturnVauchar.LedgerWriterStoreBy, ReturnVauchar.LedgerWriterStoreDate, ReturnVauchar.ValueDeterminedby, ReturnVauchar.ValueDeterminedDate, ReturnVauchar.StoreKeeperBy, ReturnVauchar.StoreKeeperDate, ReturnVauchar.EntryBy, ReturnVauchar.EntryDate, '" + baseUrl + @"'+'RVReport.aspx?RvId=' + CONVERT(VARCHAR, ReturnVauchar.IDRvNo) AS RvId, Employee.Name AS PreparedBy
                    FROM ReturnVauchar INNER JOIN Employee ON ReturnVauchar.PreparedBy = Employee.EmployeeID WHERE ReturnVauchar.Store='" + ddlStore.SelectedValue + "'" + query + " Order by ReturnVauchar.EntryDate Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    private void BindStore(string query = "")
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

        SQLQuery.PopulateDropDownWithoutSelect(query, ddlStore, "StoreAssignID", "Name");
        if (ddlStore.SelectedValue == "")
        {
            ddlStore.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        //SQLQuery.PopulateDropDown("Select StoreAssignID, Name from Store", ddSaveToStore, "StoreAssignID", "Name");
    }

    private void BindDdProductId()
    {
        if (ddSIR_LVID.SelectedValue != "0")
        {
            if (rdLoan.Checked)
            {
                SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product.Name ELSE Product.Name +'-'+ ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID FROM Product INNER JOIN ProductDetails ON Product.ProductID = ProductDetails.ProductID INNER JOIN LVProduct ON ProductDetails.ProductDetailsID = LVProduct.ProductDetailsID WHERE(LVProduct.IDLVNo = '" + ddSIR_LVID.SelectedValue + "' AND LVProduct.StoreId='" + ddlStore.SelectedValue + "')", ddProductID, "ProductDetailsID", "ProductName");
            }
            else
            {
                SQLQuery.PopulateDropDown(@"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product.Name ELSE Product.Name +'-'+ ProductDetails.SerialNo END AS ProductName, ProductDetails.ProductDetailsID, SIRProduct.IDSirNo FROM Product INNER JOIN ProductDetails ON Product.ProductID = ProductDetails.ProductID INNER JOIN SIRProduct ON ProductDetails.ProductDetailsID = SIRProduct.ProductDetailsID WHERE (SIRProduct.IDSirNo = '" + ddSIR_LVID.SelectedValue + "' AND SIRProduct.StoreId='" + ddlStore.SelectedValue + "')", ddProductID, "ProductDetailsID", "ProductName");
            }
        }
        else
        {
            ddProductID.Items.Clear();
            ListItem lst = new ListItem("---Select---", "0");
            ddProductID.Items.Insert(ddProductID.Items.Count, lst);
        }

    }


    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetProductPeice();
        if (ddProductID.SelectedValue != "0")
        {
            txtReturnQuantity.Text = "1";
        }
        else
        {
            txtReturnQuantity.Text = "0";
        }
        //GridView1.DataBind();
    }

    private void GetProductPeice()
    {

    }

    private void ClearControls()
    {
        //txtRV.Text = "";
        ddSIR_LVID.Enabled = true;
        ddlStore.Enabled = true;
        txtReturnVoucharNo.Enabled = true;
        txtProductReturnDivision.Text = "";
        txtProductReturnCause.Text = "";
        txtIssueDetails.Text = "";
        txtReturnQuantity.Text = "0";
        txtProductStatus.Text = "";
        txtProductReceive.Text = "";
        txtUnitPrice.Text = "";
        txtTotalPrice.Text = "";
        txtDeposit.Text = "";
        txtComments.Text = "";
        LaodLabelWithDropdwon();

    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //bindCenter();
        BindGrid();
    }
    private void InsertProduct()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        int lvNo;
        if (hdnRVID.Value == "")
        {
            lvNo = 0;
        }
        else
        {
            lvNo = Convert.ToInt32(hdnRVID.Value);
        }
        string type = "SIR";
        if (rdLoan.Checked)
        {
            type = "LV";
        }

        if (btnDraft.Text.ToUpper() != "UPDATE DRAFT")
        {
            command = new SqlCommand(@"INSERT INTO RVProduct ( RVNo,RVVoucherNo,CategoryID,SubCategoryID,ProductID,ProductDetailsID,Type,VoucherID,IssueDescription,ReturnQTY,ReturnQtyInWords,ApprovedQty,ApprovedQtyInWords,ProductStatus,ProductReceive, UnitPrice, TotalPrice,DepositalAccount,EntryBy) 
                                       VALUES ('',@RVVoucherNo,@CategoryID,@SubCategoryID,@ProductID,@ProductDetailsID,@Type,@VoucherID,@IssueDescription,@ReturnQTY,@ReturnQtyInWords,@ApprovedQty,@ApprovedQtyInWords,@ProductStatus,@ProductReceive,@UnitPrice,@TotalPrice,@DepositalAccount ,@EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        else
        {
            command = new SqlCommand(@"INSERT INTO RVProduct ( RVNo,RVVoucherNo,CategoryID,SubCategoryID,ProductID,ProductDetailsID,Type,VoucherID,IssueDescription,ReturnQTY,ReturnQtyInWords,ApprovedQty,ApprovedQtyInWords,ProductStatus,ProductReceive, UnitPrice, TotalPrice,DepositalAccount,EntryBy) 
                                       VALUES (@RVNo,@RVVoucherNo,@CategoryID,@SubCategoryID,@ProductID,@ProductDetailsID,@Type,@VoucherID,@IssueDescription,@ReturnQTY,@ReturnQtyInWords,@ApprovedQty,@ApprovedQtyInWords,@ProductStatus,@ProductReceive,@UnitPrice,@TotalPrice,@DepositalAccount ,@EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        }
        command.Parameters.AddWithValue("@RVNo", lvNo);
        command.Parameters.AddWithValue("@RVVoucherNo", hdnRVVoucher.Value);
        command.Parameters.AddWithValue("@CategoryID", SQLQuery.ReturnString(@"SELECT Product.ProductCategoryID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')"));
        command.Parameters.AddWithValue("@SubCategoryID", SQLQuery.ReturnString(@"SELECT Product.ProductSubCategoryID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')"));
        command.Parameters.AddWithValue("@ProductID", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@Type", type);
        command.Parameters.AddWithValue("@VoucherID", ddSIR_LVID.SelectedValue);
        command.Parameters.AddWithValue("@IssueDescription", txtIssueDetails.Text);
        command.Parameters.AddWithValue("@ReturnQTY", Convert.ToInt32(txtReturnQuantity.Text));
        command.Parameters.AddWithValue("@ReturnQtyInWords", SQLQuery.Int2WordsBangla(txtReturnQuantity.Text.ToString()));
        command.Parameters.AddWithValue("@ApprovedQty", Convert.ToInt32(txtProductReceive.Text));
        command.Parameters.AddWithValue("@ApprovedQtyInWords", SQLQuery.Int2WordsBangla(txtProductReceive.Text.ToString()));
        command.Parameters.AddWithValue("@ProductStatus", txtProductStatus.Text);
        command.Parameters.AddWithValue("@ProductReceive", txtProductReceive.Text);
        command.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text);
        command.Parameters.AddWithValue("@TotalPrice", txtTotalPrice.Text);
        command.Parameters.AddWithValue("@DepositalAccount", txtDeposit.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);


        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }
    private void BindAddItemsGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string type = "SIR";
        string query = "";
        if (rdLoan.Checked)
        {
            type = "LV";
        }
        if (Page.User.IsInRole("Super Admin") && hdnRVID.Value != "")
        {
            query = @"AND RVProduct.RVNo = '" + hdnRVID.Value + "'";
        }
        else
        {
            query = @"AND RVProduct.RVNo = '" + hdnRVID.Value + "' AND RVProduct.EntryBy = '" + lName + "'";
        }
        string sql = @"SELECT Product.Name + '-' + ProductDetails.SerialNo AS ProductName, RVProduct.ProductID, RVProduct.ReturnQTY, RVProduct.ProductStatus, RVProduct.RVProductID, RVProduct.ProductReceive, RVProduct.UnitPrice, RVProduct.EntryBy, 
                  RVProduct.TotalPrice, RVProduct.RVNo FROM     ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                  RVProduct ON ProductDetails.ProductDetailsID = RVProduct.ProductDetailsID WHERE  RVProduct.Type='" + type + "'  AND RVProduct.VoucherID='" + ddSIR_LVID.SelectedValue + "'" + query + "";

        SqlCommand command = new SqlCommand(sql, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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

        string query = @"UPDATE RVProduct SET ProductStatus=@ProductStatus, ProductID=@ProductID,CategoryID=@CategoryID,SubCategoryID=@SubCategoryID, ProductDetailsID=@ProductDetailsID,IssueDescription=@IssueDescription, ReturnQTY=@ReturnQTY, ProductReceive=@ProductReceive, UnitPrice=@UnitPrice,TotalPrice=@TotalPrice,DepositalAccount=@DepositalAccount WHERE RVProductID = '" + hdnProductId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Parameters.AddWithValue("@CategoryID", SQLQuery.ReturnString(@"SELECT Product.ProductCategoryID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')"));
        command.Parameters.AddWithValue("@SubCategoryID", SQLQuery.ReturnString(@"SELECT Product.ProductSubCategoryID FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID WHERE  (ProductDetails.ProductDetailsID = '" + ddProductID.SelectedValue + "')"));
        command.Parameters.AddWithValue("@ProductID", SQLQuery.GetProductIDByDetailsID(ddProductID.SelectedValue));
        command.Parameters.AddWithValue("@ProductDetailsID", ddProductID.SelectedValue);
        command.Parameters.AddWithValue("@IssueDescription", txtIssueDetails.Text);
        command.Parameters.AddWithValue("@ProductStatus", txtProductStatus.Text);
        command.Parameters.AddWithValue("@ReturnQTY", txtReturnQuantity.Text);
        command.Parameters.AddWithValue("@ProductReceive", txtProductReceive.Text);
        command.Parameters.AddWithValue("@UnitPrice", txtUnitPrice.Text);
        command.Parameters.AddWithValue("@TotalPrice", txtTotalPrice.Text);
        command.Parameters.AddWithValue("@DepositalAccount", txtDeposit.Text);

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
            string type = "SIR";
            if (rdLoan.Checked)
            {
                type = "LV";
            }

            string isProductExists = SQLQuery.ReturnString("SELECT ProductDetailsID FROM RVProduct WHERE ProductDetailsID = '" + ddProductID.SelectedValue + "' AND Type='" + type + "' AND VoucherID='" + ddSIR_LVID.SelectedValue + "' AND RVNo = '" + hdnRVID.Value + "' AND EntryBy = '" + lName + "'");
            SQLQuery.Empty2Zero(txtUnitPrice);
            SQLQuery.Empty2Zero(txtDeposit);
            if (btnAdd.Text.ToUpper() == "ADD PRODUCT")
            {
                if (isProductExists != ddProductID.SelectedValue)
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        InsertProduct();
                        ClearProductControls();
                        Notify("Insert Successful", "info", lblMsg);
                        BindAddItemsGridView();
                        ddProductID.SelectedValue = "0";
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
                    ClearProductControls();
                    BindAddItemsGridView();
                    Notify("Update Successful", "info", lblMsg);

                    ddProductID.SelectedValue = "0";
                    btnAdd.Text = "ADD PRODUCT";
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
                                string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRV.Text));
                                string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                string isExist = SQLQuery.ReturnString("SELECT RvInvoiceNo FROM ReturnVauchar WHERE RvInvoiceNo='" + txtReturnVoucharNo.Text.Trim() + "'  AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND Store='" + ddlStore.SelectedValue + "'");
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
                                    SaveRVData("Drafted");
                                    Notify("Successfully Saved...", "success", lblMsg);

                                }
                                else
                                {
                                    Notify("This " + txtReturnVoucharNo.Text + " RV Number already exist.", "warn", lblMsg);

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

                        UpdateRVData("Drafted");
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
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";
            BindGrid();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            txtReturnVoucharNo.Text = GenerateVoucherNumber.GetRvNumber(Convert.ToDateTime(txtRV.Text), User.Identity.Name, ddlStore.SelectedValue);
        }
    }

    private void ClearProductControls()
    {
        txtReturnQuantity.Text = "0";
        txtIssueDetails.Text = txtProductReceive.Text = txtProductStatus.Text = txtUnitPrice.Text = txtTotalPrice.Text = txtDeposit.Text = "";
    }
    protected void AddItemsGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
            Label label = AddItemsGridView.Rows[index].FindControl("lblRVProductID") as Label;
            hdnProductId.Value = label.Text;
            string query = @"SELECT RVProductID, RVNo, ProductID, ProductStatus,ProductDetailsID, ReturnQTY, TotalPrice, ProductReceive, IssueDescription, UnitPrice, DepositalAccount, EntryBy FROM RVProduct WHERE RVProductID = '" + hdnProductId.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                btnAdd.Text = "Update";
                //bindDDCategoryID();
                //ddCategoryID.SelectedValue = dataReader["CategoryID"].ToString();
                //BindddProductSubCategory();
                //ddProductSubCategory.SelectedValue = dataReader["SubCategoryID"].ToString();
                BindDdProductId();
                ddProductID.SelectedValue = dataReader["ProductDetailsID"].ToString();
                txtProductStatus.Text = dataReader["ProductStatus"].ToString();
                txtReturnQuantity.Text = dataReader["ReturnQTY"].ToString();
                txtTotalPrice.Text = dataReader["TotalPrice"].ToString();
                txtProductReceive.Text = dataReader["ProductReceive"].ToString();
                txtIssueDetails.Text = dataReader["IssueDescription"].ToString();
                txtUnitPrice.Text = dataReader["UnitPrice"].ToString();
                txtDeposit.Text = dataReader["DepositalAccount"].ToString();

            }
            Notify("Edit mode activated ...", "info", lblMsg);
            //ddProductID.Enabled = false;
            dataReader.Close();
            command.Connection.Close();
        }
        catch (Exception ex)
        {

            Notify("ERROR!" + ex, "error", lblMsg);
        }

    }

    protected void AddItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = AddItemsGridView.Rows[index].FindControl("lblRVProductID") as Label;
        SQLQuery.ExecNonQry("Delete RVProduct FROM RVProduct WHERE RVProductID='" + lblId.Text + "' ");
        BindAddItemsGridView();
        Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
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

    private void BindWorkFlowUserGridView()
    {
        string lName = Page.User.Identity.Name.ToString();

        string query = "";
        if (User.IsInRole("Super Admin") && hdnRVID.Value != "")
        {
            query = "AND WorkFlowTypeID='" + hdnRVID.Value + "'";
        }
        else
        {
            query = "AND WorkFlowTypeID='" + hdnRVID.Value + "' AND EntryBy = '" + lName + "'";
        }
        string sql = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE WorkFlowUser.WorkFlowType = 'RV'" + query + " Order By Priority ASC";

        SqlCommand command = new SqlCommand(sql, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnRVID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'RV'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnRVID.Value + "' AND WorkFlowType = 'RV'");
            if (priorityDataRow["Priority"].ToString() == ddlPriority.SelectedValue)
            {
                priorityStatus = false;
            }

        }

        return priorityStatus;
    }
    private void InsertToWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();


        SqlCommand command;
        int typeId;
        if (hdnRVID.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hdnRVID.Value);
        }


        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,VoucherNo,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,@VoucherNo,'RV',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));


        command.Parameters.AddWithValue("@WorkFlowTypeID", typeId);
        command.Parameters.AddWithValue("@VoucherNo", hdnRVVoucher.Value);
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
        command.Parameters.AddWithValue("@VoucherNo", hdnRVVoucher.Value);
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
    private bool PriorityCheckForUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnRVID.Value + "' AND WorkFlowType = 'RV'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnRVID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'RV'");
                if (int.Parse(priority) > 0)
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
            string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hdnRVID.Value + "' AND WorkFlowType = 'RV' AND EntryBy = '" + lName + "'");
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

    private bool PrioritySequenceCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = false;
        int priority = int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(MAX(Priority),0) FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnRVID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'RV'"));
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
    private void VisibleWorkflowDateAndDay()
    {
        //string dayName = Convert.ToDateTime(txtEsclationStartTime.Text).ToString("dddd");
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnRVID.Value + "' AND WorkFlowType = 'RV' AND EntryBy = '" + User.Identity.Name + "'");

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
    private void LaodLabelWithDropdwon()
    {
        if (rdLoan.Checked)
        {
            lblSirLV.Text = "LV Number";
            LoadLvInvoice();
        }
        else
        {
            lblSirLV.Text = "SIR Number";
            LoadSirInvoice();
        }
    }
    private void LoadLvInvoice()
    {
        string sqlQuery = "";
        if (hdnMode.Value != "EditMode")
        {
            sqlQuery = "AND ReceivedStatus='Pending' AND IDLVNo NOT IN (SELECT LvSIRVoucherNo FROM ReturnVauchar WHERE ReturnType='LV')";
        }
        SQLQuery.PopulateDropDown("SELECT IDLVNo, LvInvoiceNo FROM  LoanVouchar WHERE  Store='" + ddlStore.SelectedValue + "' AND WorkflowStatus='Approved'" + sqlQuery + "", ddSIR_LVID, "IDLVNo", "LvInvoiceNo");
    }

    private void LoadSirInvoice()
    {
        string sqlQuery = "";
        if (hdnMode.Value != "EditMode")
        {
            sqlQuery = "AND ReceivedStatus='Pending' AND IDSirNo NOT IN (SELECT LvSIRVoucherNo FROM ReturnVauchar WHERE ReturnType='SIR')";
        }
        SQLQuery.PopulateDropDown("SELECT IDSirNo, SirVoucherNo FROM  SIRFrom WHERE Store='" + ddlStore.SelectedValue + "' AND WorkflowStatus='Approved' " + sqlQuery + "", ddSIR_LVID, "IDSirNo", "SirVoucherNo");
    }
    private void BindddlDesignation()
    {
        string query = @"SELECT DesignationID, Name, Description, RoleID, Priority FROM Designation";
        SQLQuery.PopulateDropDown(query, ddlDesignation, "DesignationID", "Name");
    }

    protected void ddlCenterID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
    protected void ddSIR_LVID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDdProductId();
        ClearProductControls();
        btnAdd.Text = "ADD PRODUCT";
        BindAddItemsGridView();
    }

    protected void rdLoan_CheckedChanged(object sender, EventArgs e)
    {
        LaodLabelWithDropdwon();
        BindDdProductId();
        ClearProductControls();
        btnAdd.Text = "ADD PRODUCT";
        BindAddItemsGridView();
    }

    protected void txtRV_TextChanged(object sender, EventArgs e)
    {
        txtReturnVoucharNo.Text = GenerateVoucherNumber.GetRvNumber(Convert.ToDateTime(txtRV.Text), User.Identity.Name, ddlStore.SelectedValue);
    }

    protected void txtReturnVoucharNo_TextChanged(object sender, EventArgs e)
    {
        if (txtReturnVoucharNo.Text.Length == 15)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtRV.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT RvInvoiceNo FROM ReturnVauchar WHERE RvInvoiceNo='" + txtReturnVoucharNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtReturnVoucharNo.Text + " RV Number already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("Return Voucher Number should be 15 characters", "warn", lblMsg);
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void ddlStore_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        LaodLabelWithDropdwon();
        BindDdProductId();
        BindGrid();
        txtReturnVoucharNo.Text = GenerateVoucherNumber.GetRvNumber(Convert.ToDateTime(txtRV.Text), User.Identity.Name, ddlStore.SelectedValue);

    }
}
