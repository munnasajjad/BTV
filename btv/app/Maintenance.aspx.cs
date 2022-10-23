using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Maintenance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtMNNumber.Text = LogMonitorGenerateVoucher.GetMNVoucherNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));
            BindGrid();
            BindDesignation();
            BindEmployee();
            BindddPriority();
            BindWorkFlowUserGridView();
            BindShiftInCharge();
            BindMainOffice();
            BindDDLocationInMainOffice();
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
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
    //                RunQuery.SQLQuery.ExecNonQry("INSERT INTO Maintenance (SerialNo, Date, [User], ProblemPlace, ProblemType, SolutionProvidedBy, ShiftInCharge, Remarks,EntryBy) VALUES ('" + txtSerialNo.Text.Replace("'", "''") + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtUser.Text.Replace("'", "''") + "', '" + txtProblemPlace.Text.Replace("'", "''") + "', '" + txtProblemType.Text.Replace("'", "''") + "', '" + txtSolutionProvidedBy.Text.Replace("'", "''") + "', '" + txtShiftInCharge.Text.Replace("'", "''") + "', '" + txtRemarks.Text.Replace("'", "''") + "','" + lName + "')    ");
    //                ClearControls();
    //                Notify("Successfully Saved...", "success", lblMsg);
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
    //                RunQuery.SQLQuery.ExecNonQry(" Update  Maintenance SET SerialNo= '" + txtSerialNo.Text.Replace("'", "''") + "',  Date= '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',  [User]= '" + txtUser.Text.Replace("'", "''") + "',  ProblemPlace= '" + txtProblemPlace.Text.Replace("'", "''") + "',  ProblemType= '" + txtProblemType.Text.Replace("'", "''") + "',  SolutionProvidedBy= '" + txtSolutionProvidedBy.Text.Replace("'", "''") + "',  ShiftInCharge= '" + txtShiftInCharge.Text.Replace("'", "''") + "',  Remarks= '" + txtRemarks.Text.Replace("'", "''") + "' WHERE MaintenanceId='" + lblId.Text + "' ");
    //                ClearControls();
    //                btnSave.Text = "Save";
    //                Notify("Successfully Updated...", "success", lblMsg);
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
    //        BindGrid();
    //    }
    //}

    //protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string lName = Page.User.Identity.Name.ToString();
    //        if (SQLQuery.OparatePermission(lName, "Update") == "1")
    //        {
    //            int index = Convert.ToInt32(GridView1.SelectedIndex);
    //            Label lblEditId = GridView1.Rows[index].FindControl("Label1") as Label;
    //            lblId.Text = lblEditId.Text;
    //            DataTable dt = SQLQuery.ReturnDataTable(" Select MaintenanceId, SerialNo,Date,[User],ProblemPlace,ProblemType,SolutionProvidedBy,ShiftInCharge,Remarks FROM Maintenance WHERE MaintenanceId='" + lblId.Text + "'");
    //            foreach (DataRow dtx in dt.Rows)
    //            {
    //                txtSerialNo.Text = dtx["SerialNo"].ToString();
    //                txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
    //                txtUser.Text = dtx["User"].ToString();
    //                txtProblemPlace.Text = dtx["ProblemPlace"].ToString();
    //                txtProblemType.Text = dtx["ProblemType"].ToString();
    //                txtSolutionProvidedBy.Text = dtx["SolutionProvidedBy"].ToString();
    //                txtShiftInCharge.Text = dtx["ShiftInCharge"].ToString();
    //                txtRemarks.Text = dtx["Remarks"].ToString();

    //            }
    //            btnSave.Text = "Update";
    //            Notify("Edit mode activated ...", "info", lblMsg);
    //        }
    //        else
    //        {
    //            Notify("You are not eligible to attempt this operation", "warn", lblMsg);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Notify(ex.ToString(), "error", lblMsg);
    //    }
    //}

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ExecNonQry(" Delete Maintenance WHERE MaintenanceId='" + lblId.Text + "' ");
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
        //Response.Redirect("./Default.aspx");
        ClearControls();
    }

    //private void BindGrid()
    //{
    //    DataTable dt = SQLQuery.ReturnDataTable(@" SELECT MaintenanceId, SerialNo, CONVERT(varchar,Date, 103) AS Date, [User], ProblemPlace, ProblemType, SolutionProvidedBy, ShiftInCharge, Remarks FROM Maintenance");
    //    GridView1.DataSource = dt;
    //    GridView1.DataBind();
    //}



    private void ClearControls()
    {
        txtSerialNo.Text = "";
        txtDate.Text = "";
        txtUser.Text = "";
        /*txtProblemPlace.Text = ""*/
        ;
        txtProblemType.Text = "";
        txtSolutionProvidedBy.Text = "";
        //txtShiftInCharge.Text = "";
        txtRemarks.Text = "";

    }

    private void BindShiftInCharge()
    {
        string sqlquery = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            sqlquery = " AND VwShiftInCharge.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        string query = @"SELECT EmployeeId, Name FROM VwShiftInCharge WHERE VwShiftInCharge.EmployeeId<>0 " + sqlquery + "";
        SQLQuery.PopulateDropDown(query, ddShiftInCharge, "EmployeeId", "Name");
        if (ddShiftInCharge.Text == "")
        {
            ddShiftInCharge.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    private void EditMode(string id)
    {
        hiddenMNID.Value = id;
        DataTable dt = SQLQuery.ReturnDataTable("SELECT MaintenanceId, MNVoucher, MainOfficeId, SerialNo,Date,[User],ProblemPlace,ProblemType,SolutionProvidedBy,ShiftInCharge,Remarks FROM Maintenance WHERE MaintenanceId='" + lblId.Text + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            hiddenMNVoucher.Value = dtx["MNVoucher"].ToString();
            txtMNNumber.Text = dtx["MNVoucher"].ToString();
            txtSerialNo.Text = dtx["SerialNo"].ToString();
            txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
            txtUser.Text = dtx["User"].ToString();
            BindMainOffice();
            ddLocationID.SelectedValue = dtx["MainOfficeId"].ToString();
            BindDDLocationInMainOffice();
            ddLocationInMainOffice.SelectedValue = dtx["ProblemPlace"].ToString();
            txtProblemType.Text = dtx["ProblemType"].ToString();
            txtSolutionProvidedBy.Text = dtx["SolutionProvidedBy"].ToString();
            ddShiftInCharge.SelectedValue = dtx["ShiftInCharge"].ToString();
            txtRemarks.Text = dtx["Remarks"].ToString();

        }
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();
        btnSave.Text = "Submit";
        btnDraft.Text = "Update Draft";
        Notify("Edit mode activated ...", "info", lblMsg);
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
                Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
                Label labelMNVoucher = GridView1.Rows[index].FindControl("lblMNNumber") as Label;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM Maintenance WHERE MaintenanceId='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM Maintenance WHERE MaintenanceId='" + lblEditId.Text + "'");
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
                        Notify("This " + labelMNVoucher.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("This number" + labelMNVoucher.Text + " entry by user is " + lblEntryBy.Text + ". You are not authorize edit the voucher. If you need to any change please contact higher authority.", "warn", lblMsg);
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

    private void BindGrid()
    {
        string query = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            query = "WHERE DepartmentId='" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";
        }

        string sql = @"SELECT M.MaintenanceId, M.MNVoucher, M.MainOfficeId, M.FunctionalOfficeId, M.DepartmentId, M.FinYear, M.SerialNo, CONVERT(varchar, M.Date, 103) AS Date, M.[User], M.ProblemPlace, M.ProblemType, M.SolutionProvidedBy, E.Name AS ShiftInCharge, M.Remarks, M.PreparedDate, M.Checkerby, M.CheckerDate, M.Approvedby, M.ApprovedDate, M.SaveMode, M.SubmitDate, M.WorkflowStatus, M.WorkflowApprovedDate, M.CurrentWorkflowUser, M.ReturnOrHoldUserID, M.EntryBy 
FROM Maintenance AS M  INNER JOIN Employee AS E ON M.ShiftInCharge = E.EmployeeID " + query + "";
        DataTable dt = SQLQuery.ReturnDataTable(sql);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private void BindEmployee()
    {
        string sql = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            sql = "WHERE DesignationWithEmployee.DesignationID='" + ddlDesignation.SelectedValue +
                  "' AND Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) +
                  "' AND Employee.CenterID='" + SQLQuery.GetCenterId(User.Identity.Name) +
                  "' AND Employee.DepartmentSectionID='" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) +
                  "' AND Employee.EmployeeId<>'" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'";
        }

        string query = @"SELECT DesignationWithEmployee.Id, Employee.Name + ', ' + Designation.Name AS Name FROM DesignationWithEmployee INNER JOIN Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID " + sql + "";
        SQLQuery.PopulateDropDown(query, ddEmployee, "Id", "Name");
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindEmployee();
    }
    private bool PriorityCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenMNID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'MN'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenMNID.Value + "' AND WorkFlowType = 'MN'");
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
        if (hiddenMNID.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hiddenMNID.Value);
        }

        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,'MN',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Parameters.AddWithValue("@WorkFlowTypeID", typeId);
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
    private void BindWorkFlowUserGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string query = "";
        //if (hiddenMNID.Value == "")
        //{
        //    hiddenMNID.Value = "0";
        //}
        query = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE WorkFlowTypeID='" + hiddenMNID.Value + "' AND WorkFlowUser.WorkFlowType = 'MN' AND EntryBy = '" + lName + "' Order By Priority ASC";

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
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hiddenMNID.Value + "' AND WorkFlowType = 'MN' AND EntryBy = '" + User.Identity.Name + "'");

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
    private bool PriorityCheckForUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenMNID.Value + "' AND WorkFlowType = 'MN'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenMNID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'MN'");
                if (int.Parse(priority) > 0)
                {
                    priorityStatus = false;
                }
            }
        }
        return priorityStatus;
    }
    private void UpdateWorkFlowUser()
    {

        string lName = Page.User.Identity.Name.ToString();
        string query = @"UPDATE WorkFlowUser SET EmployeeID=@EmployeeID,DesignationId=@DesignationId,EsclationDay=@EsclationDay, Priority=@Priority, Remark=@Remark, EntryBy=@EntryBy, EntryDate=@EntryDate WHERE WorkFlowUserID = '" + hiddenWorkFlowUserID.Value + "'";
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
    private void BindDesignation()
    {
        string query = @"SELECT DesignationID, Name, Description, RoleID, Priority FROM Designation";
        SQLQuery.PopulateDropDown(query, ddlDesignation, "DesignationID", "Name");
    }
    protected void WorkFlowUserGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(WorkFlowUserGridView.SelectedIndex);
            Label label = WorkFlowUserGridView.Rows[index].FindControl("lblWorkFlowUserID") as Label;

            hiddenWorkFlowUserID.Value = label.Text;
            string query = @"SELECT WorkFlowUserID,DesignationId,EmployeeID, Priority, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, Remark FROM WorkFlowUser WHERE WorkFlowUserID = '" + hiddenWorkFlowUserID.Value + "'";
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
    private void BindddPriority()
    {
        SQLQuery.PopulateDropDown("SELECT SequenceId, SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'MN')", ddlPriority, "Priority", "SequenceBan");
    }
    protected void btnWorkFlowSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;
            string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hiddenMNID.Value + "' AND WorkFlowType = 'MN' AND EntryBy = '" + lName + "'");
            if (btnWorkFlowSave.Text.ToUpper() == "ADD USER")
            {
                if (isUserExists != ddEmployee.SelectedValue)
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
                    Notify("This employee is already added!", "warn", lblMsg);
                }
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

    private void NotifyToEmployee(string employeeID, string lvNumber, string mnId)
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

            SQLQuery.ExecNonQry("UPDATE Maintenance SET CurrentWorkflowUser='" + name + "' WHERE MaintenanceId = '" + mnId + "'");
            SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + lvNumber, emailBody);

        }
    }

    private void SaveData(string saveMode)
    {
        string lName = User.Identity.Name;
        string sqlColumn = "";
        string sqlValue = "";
        if (saveMode == "Submitted")
        {
            sqlColumn = "SubmitDate,";
            sqlValue = "'" + DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss tt") + "',";
        }
        //(" INSERT INTO Maintenance (Date, SuppliedBy, MeterNumber, ConsumptionTime, KVARH, MaximumDemand, UHICondition, [1C-Peak], [2C-Peak], ReadingTakenBy, Remarks,EntryBy) VALUES ('" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + ddlSupplied.SelectedValue + "', '" + txtMeterNumber.Text.Replace("'", "''") + "', '" + ddlConsumptionTime.SelectedValue + "', '" + txtKVARH.Text.Replace("'", "''") + "', '" + txtMaximumDemand.Text.Replace("'", "''") + "', '" + txtUHICondition.Text.Replace("'", "''") + "', '" + txt1CPeak.Text.Replace("'", "''") + "', '" + txt2CPeak.Text.Replace("'", "''") + "', '" + txtReadingTakenBy.Text.Replace("'", "''") + "', '" + txtRemarks.Text.Replace("'", "''") + "','" + lName + "')");
        SQLQuery.ExecNonQry("INSERT INTO Maintenance (" + sqlColumn + "SaveMode, Date, MNVoucher, MainOfficeId, FunctionalOfficeId, DepartmentId, FinYear, SerialNo, [User], ProblemPlace, ProblemType, SolutionProvidedBy, ShiftInCharge, Remarks, PreparedBy, EntryBy) VALUES (" + sqlValue + "'" + saveMode + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtMNNumber.Text + "','" + SQLQuery.GetLocationID(User.Identity.Name) + "','" + SQLQuery.GetCenterId(User.Identity.Name) + "','" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "','" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "','" + txtSerialNo.Text.Replace("'", "''") + "', '" + txtUser.Text.Replace("'", "''") + "', '" + ddLocationInMainOffice.SelectedValue + "', '" + txtProblemType.Text.Replace("'", "''") + "', '" + txtSolutionProvidedBy.Text.Replace("'", "''") + "', '" + ddShiftInCharge.SelectedValue + "', '" + txtRemarks.Text.Replace("'", "''") + "','" + SQLQuery.GetEmployeeID(User.Identity.Name) + "','" + User.Identity.Name + "')");
        string mnId = SQLQuery.ReturnString("SELECT MAX(MaintenanceId) AS mnId FROM Maintenance WHERE EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + mnId + "',VoucherNo='" + txtMNNumber.Text + "'  WHERE WorkFlowTypeID = '0' AND WorkFlowType='MN' AND EntryBy='" + lName + "' ");

        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                            PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + mnId + "' AND WorkFlowType='MN'";

            DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
            foreach (DataRow item in dtUser.Rows)
            {
                if (item["Priority"].ToString() == "1")
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), mnId);
                }
            }
        }
    }

    private void UpdateData(string saveMode)
    {
        string returnUser = "";
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM Maintenance WHERE MaintenanceId='" + hiddenMNID.Value + "'");
        if (workflowStatus == "Return")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM Maintenance WHERE MaintenanceId='" + hiddenMNID.Value + "'");
            workflowStatus = "Pending";
        }

        string sqlColumn = "";
        if (saveMode == "Submitted")
        {
            sqlColumn = ",SubmitDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
            // sqlValue = DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss tt");
        }
        RunQuery.SQLQuery.ExecNonQry("UPDATE Maintenance SET SerialNo= '" + txtSerialNo.Text.Replace("'", "''") + "',  Date= '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',  [User]= '" + txtUser.Text.Replace("'", "''") + "',  ProblemPlace= '" + ddLocationInMainOffice.SelectedValue + "',  ProblemType= '" + txtProblemType.Text.Replace("'", "''") + "',  SolutionProvidedBy= '" + txtSolutionProvidedBy.Text.Replace("'", "''") + "',  ShiftInCharge= '" + ddShiftInCharge.SelectedValue + "',  Remarks= '" + txtRemarks.Text.Replace("'", "''") + "', ShiftInCharge = '" + ddShiftInCharge.SelectedValue + "', WorkflowStatus='" + workflowStatus + "', SaveMode='" + saveMode + "'" + sqlColumn + " WHERE MaintenanceId='" + lblId.Text + "' ");
        if (saveMode == "Submitted")
        {
            if (returnUser != "")
            {
                string sqlQuery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                        PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowUserID='" + returnUser + "'";
                DataTable dtUser = SQLQuery.ReturnDataTable(sqlQuery);
                foreach (DataRow item in dtUser.Rows)
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "'");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hiddenMNID.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hiddenMNID.Value + "' AND WorkFlowType='MN'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hiddenMNID.Value);
                    }
                }
            }
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
                    SaveData("Drafted");
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtMNNumber.Text = LogMonitorGenerateVoucher.GetMNVoucherNumber(Convert.ToDateTime(txtDate.Text),
                        User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));

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
                if (SQLQuery.OparatePermission(lName, "UPDATE") == "1")
                {
                    UpdateData("Drafted");
                    hiddenMNID.Value = "";
                    hiddenWorkFlowUserID.Value = "";
                    hiddenMNVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtMNNumber.Text = LogMonitorGenerateVoucher.GetMNVoucherNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));

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
            BindGrid();
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
                    SaveData("Submitted");
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtMNNumber.Text = LogMonitorGenerateVoucher.GetMNVoucherNumber(Convert.ToDateTime(txtDate.Text),
                        User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));

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
                    UpdateData("Submitted");
                    hiddenMNID.Value = "";
                    hiddenWorkFlowUserID.Value = "";
                    hiddenMNVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtMNNumber.Text = LogMonitorGenerateVoucher.GetMNVoucherNumber(Convert.ToDateTime(txtDate.Text),
                        User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));

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
            BindGrid();
        }
    }
    private void BindMainOffice()
    {
        string query = "";
        if (!User.IsInRole("Super Admin"))
        {
            query = "Where LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        SQLQuery.PopulateDropDown("SELECT Name, LocationID from Location " + query, ddLocationID, "LocationID", "Name");
    }

    protected void ddLocationID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDDLocationInMainOffice();
        BindGrid();
    }
    private void BindDDLocationInMainOffice()
    {

        SQLQuery.PopulateDropDown("SELECT Id, MainOfficeLocationName FROM MainOfficeLocation WHERE MainOfficeId= '" + ddLocationID.SelectedValue + "'", ddLocationInMainOffice, "Id", "MainOfficeLocationName");
        if (ddLocationInMainOffice.Text == "")
        {
            ddLocationInMainOffice.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }
}
