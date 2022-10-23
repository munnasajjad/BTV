using RunQuery;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_EarthStation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtESNumber.Text = LogMonitorGenerateVoucher.GetESVoucherNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));
            BindDesignation();
            BindEmployee();
            BindddPriority();
            BindWorkFlowUserGridView();
            BindGrid();
            BindShiftInCharge();
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
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
        RunQuery.SQLQuery.ExecNonQry(" INSERT INTO EarthStation (" + sqlColumn + "SaveMode,Date,ESVoucher,MainOfficeId,FunctionalOfficeId,DepartmentId,FinYear, Time, HPA, OutputPower, ReflationPower, TWTA, HelixCurrent, RoomTemperature, Remarks, PreparedBy, EntryBy, ShiftInCharge) VALUES (" + sqlValue + "'" + saveMode + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtESNumber.Text + "','" + SQLQuery.GetLocationID(User.Identity.Name) + "','" + SQLQuery.GetCenterId(User.Identity.Name) + "','" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "','" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "','" + txtTime.Text.Replace("'", "''") + "','" + ddlHpa.SelectedValue + "', '" + txtOutputPower.Text.Replace("'", "''") + "', '" + txtReflationPower.Text.Replace("'", "''") + "', '" + txtTWTA.Text.Replace("'", "''") + "', '" + txtHelixCurrent.Text.Replace("'", "''") + "', '" + txtRoomTemperature.Text.Replace("'", "''") + "', '" + txtRemarks.Text.Replace("'", "''") + "','" + SQLQuery.GetEmployeeID(User.Identity.Name) + "','" + User.Identity.Name + "', '" + ddShiftInCharge.SelectedValue + "')");
        string esId = SQLQuery.ReturnString("SELECT MAX(EarthStationId) AS esId FROM EarthStation WHERE EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + esId + "',VoucherNo='" + txtESNumber.Text + "'  WHERE WorkFlowTypeID = '0' AND WorkFlowType='ES' AND EntryBy='" + lName + "' ");

        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                            PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + esId + "' AND WorkFlowType='ES'";

            DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
            foreach (DataRow item in dtUser.Rows)
            {
                if (item["Priority"].ToString() == "1")
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), esId);
                }
            }
        }
    }
    private void UpdateData(string saveMode)
    {
        string returnUser = "";
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM EarthStation WHERE EarthStationId='" + hdnESID.Value + "'");
        if (workflowStatus == "Return")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM EarthStation WHERE EarthStationId='" + hdnESID.Value + "'");
            workflowStatus = "Pending";
        }

        string sqlColumn = "";
        if (saveMode == "Submitted")
        {
            sqlColumn = ",SubmitDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
            // sqlValue = DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss tt");
        }
        RunQuery.SQLQuery.ExecNonQry("Update  EarthStation SET Date= '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',  Time= '" + txtTime.Text.Replace("'", "''") + "',  HPA= '" + ddlHpa.SelectedValue + "',  OutputPower= '" + txtOutputPower.Text.Replace("'", "''") + "',  ReflationPower= '" + txtReflationPower.Text.Replace("'", "''") + "',  TWTA= '" + txtTWTA.Text.Replace("'", "''") + "',  HelixCurrent= '" + txtHelixCurrent.Text.Replace("'", "''") + "',  RoomTemperature= '" + txtRoomTemperature.Text.Replace("'", "''") + "',  Remarks= '" + txtRemarks.Text.Replace("'", "''") + "', ShiftInCharge = '" + ddShiftInCharge.SelectedValue + "', WorkflowStatus='" + workflowStatus + "',SaveMode='" + saveMode + "'" + sqlColumn + " WHERE EarthStationId='" + lblId.Text + "' ");
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
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnESID.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnESID.Value + "' AND WorkFlowType='ES'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnESID.Value);
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
                    SaveData("Submitted");
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtESNumber.Text = LogMonitorGenerateVoucher.GetESVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
                    hdnESID.Value = "";
                    hdnWorkFlowUserId.Value = "";
                    hdnEsVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtESNumber.Text = LogMonitorGenerateVoucher.GetESVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
    private void NotifyToEmployee(string employeeID, string lvNumber, string esId)
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

            SQLQuery.ExecNonQry("UPDATE EarthStation SET CurrentWorkflowUser='" + name + "' WHERE EarthStationId = '" + esId + "'");
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
                lblId.Text = lblEditId.Text;
                Label lblEntryBy = GridView1.Rows[index].FindControl("lblEntryBy") as Label;
                Label lblEsNumber = GridView1.Rows[index].FindControl("lblEsNumber") as Label;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM EarthStation WHERE EarthStationId='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM EarthStation WHERE EarthStationId='" + lblEditId.Text + "'");
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
                        Notify("This " + lblEsNumber.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("This number" + lblEsNumber.Text + " entry by user is " + lblEntryBy.Text + ". You are not authorize edit the voucher. If you need to any change please contact higher authority.", "warn", lblMsg);
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
        hdnESID.Value = id;
        DataTable dt = SQLQuery.ReturnDataTable("Select EarthStationId,ESVoucher, Date,Time,HPA,OutputPower,ReflationPower,TWTA,HelixCurrent,RoomTemperature,Remarks,ShiftInCharge FROM EarthStation WHERE EarthStationId='" + id + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            hdnEsVoucher.Value = dtx["ESVoucher"].ToString();
            txtESNumber.Text = dtx["ESVoucher"].ToString();
            txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("yyyy-MM-dd");
            txtTime.Text = dtx["Time"].ToString();

            ddlHpa.SelectedValue = dtx["HPA"].ToString();
            txtOutputPower.Text = dtx["OutputPower"].ToString();
            txtReflationPower.Text = dtx["ReflationPower"].ToString();
            txtTWTA.Text = dtx["TWTA"].ToString();
            txtHelixCurrent.Text = dtx["HelixCurrent"].ToString();
            txtRoomTemperature.Text = dtx["RoomTemperature"].ToString();

            txtRemarks.Text = dtx["Remarks"].ToString();
            ddShiftInCharge.SelectedValue = dtx["ShiftInCharge"].ToString();

        }
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();
        btnSave.Text = "Submit";
        btnDraft.Text = "Update Draft";
        Notify("Edit mode activated ...", "info", lblMsg);
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
        RunQuery.SQLQuery.ExecNonQry(" Delete EarthStation WHERE EarthStationId='" + lblId.Text + "' ");
        SQLQuery.ExecNonQry(" Delete WorkFlowUser FROM WorkFlowUser WHERE WorkFlowTypeID='" + lblId.Text + "' ");
        BindGrid();
        Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        //Response.Redirect("./Default.aspx");
        ClearControls();
    }

    private void BindGrid()
    {
        string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";
        string query = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            query = "WHERE DepartmentId='" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";
        }

        string sql = @"SELECT ES.EarthStationId, ES.ESVoucher, ES.MainOfficeId, ES.FunctionalOfficeId, ES.DepartmentId, ES.FinYear, CONVERT(varchar,ES.Date, 103) AS Date, ES.Time, ES.HPA, ES.OutputPower, ES.ReflationPower, ES.TWTA, ES.HelixCurrent, ES.RoomTemperature, ES.Remarks, ES.PreparedBy, ES.PreaperedDate, ES.Checkerby, 
ES.CheckerDate, ES.Approvedby, ES.ApprovedDate, ES.SaveMode, ES.SubmitDate, ES.WorkflowStatus, ES.WorkflowApprovedDate, ES.CurrentWorkflowUser, ES.ReturnOrHoldUserID, ES.EntryBy, E.Name AS ShiftInCharge 
FROM EarthStation AS ES INNER JOIN Employee AS E ON ES.ShiftInCharge = E.EmployeeID " + query + "";
        DataTable dt = SQLQuery.ReturnDataTable(sql);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void btnWorkFlowSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;
            string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hdnESID.Value + "' AND WorkFlowType = 'ES' AND EntryBy = '" + lName + "'");
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
    private void BindddPriority()
    {
        SQLQuery.PopulateDropDown("SELECT  SequenceId,SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'ES')", ddlPriority, "Priority", "SequenceBan");
    }

    private void InsertToWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        int typeId;
        if (hdnESID.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hdnESID.Value);
        }

        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,'ES',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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

    private void BindWorkFlowUserGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string query = "";

        query = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE WorkFlowTypeID='" + hdnESID.Value + "' AND WorkFlowUser.WorkFlowType = 'ES' AND EntryBy = '" + lName + "' Order By Priority ASC";

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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnESID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'ES'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnESID.Value + "' AND WorkFlowType = 'ES'");
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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnESID.Value + "' AND WorkFlowType = 'ES'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnESID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'ES'");
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
    private void VisibleWorkflowDateAndDay()
    {
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnESID.Value + "' AND WorkFlowType = 'ES' AND EntryBy = '" + User.Identity.Name + "'");

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
    private void BindDesignation()
    {
        string query = @"SELECT DesignationID, Name, Description, RoleID, Priority FROM Designation";
        SQLQuery.PopulateDropDown(query, ddlDesignation, "DesignationID", "Name");
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindEmployee();
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

    private void ClearControls()
    {
        txtDate.Text = "";
        txtTime.Text = "";
        //txtLocation.Text = "";
        //txtHPA.Text = "";
        txtOutputPower.Text = "";
        txtReflationPower.Text = "";
        txtTWTA.Text = "";
        txtHelixCurrent.Text = "";
        txtRoomTemperature.Text = "";
        //////txtInCharge.Text = "";
        txtRemarks.Text = "";

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
                    txtESNumber.Text = LogMonitorGenerateVoucher.GetESVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
                    UpdateData("Drafted");
                    hdnESID.Value = "";
                    hdnWorkFlowUserId.Value = "";
                    hdnEsVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtESNumber.Text = LogMonitorGenerateVoucher.GetESVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
}
