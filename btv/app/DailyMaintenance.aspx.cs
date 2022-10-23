using RunQuery;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class app_DailyMaintenance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            LoadEmployee();
            BindItemGrid();
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDMNumber.Text = LogMonitorGenerateVoucher.GetDMNVoucherNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));
            BindGrid();
            BindDesignation();
            BindEmployee();
            BindddPriority();
            BindWorkFlowUserGridView();
            BindShiftInCharge();
            BindDdLocation();
            BindDDLocationInMainOffice();
        }
    }
    private void LoadEmployee()
    {
        SQLQuery.PopulateDropDown("Select EmployeeID,Name from Employee", ddlShiftIncharge, "EmployeeID", "Name");
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
    //                RunQuery.SQLQuery.ExecNonQry(" INSERT INTO DailyMaintenance (NameoftheBrand, CapacityofSplit, CoolingArea, PresentCondition,Remarks, EntryBy) VALUES ('" + txtNameoftheBrand.Text.Replace("'", "''") + "', '" + txtCapacityofSplit.Text.Replace("'", "''") + "', '" + txtCoolingArea.Text.Replace("'", "''") + "', '" + txtPresentCondition.Text.Replace("'", "''") + "','"+txtRemarks.Text.Replace("'","''")+"' ,'" + User.Identity.Name + "')    ");
    //                string maxId = SQLQuery.ReturnString(@"SELECT MAX(DailyMaintenanceID) AS DailyMaintenanceID FROM DailyMaintenance WHERE(EntryBy = '" + lName + "') ");
    //                SQLQuery.ExecNonQry("Update DailyMaintenanceDetails SET DailyMaintenanceID='" + maxId + "' WHERE DailyMaintenanceID='0' AND EntryBy='" + lName + "'");
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
    //                RunQuery.SQLQuery.ExecNonQry(" Update  DailyMaintenance SET NameoftheBrand= '" + txtNameoftheBrand.Text.Replace("'", "''") + "',  CapacityofSplit= '" + txtCapacityofSplit.Text.Replace("'", "''") + "',  CoolingArea= '" + txtCoolingArea.Text.Replace("'", "''") + "',  PresentCondition= '" + txtPresentCondition.Text.Replace("'", "''") + "',Remarks='"+ txtRemarks.Text.Replace("'", "''") + "' WHERE DailyMaintenanceID='" + lblId.Text + "' ");
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
    //        lblId.Text = "";
    //        BindItemGrid();
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
    //            DataTable dt = SQLQuery.ReturnDataTable(" Select DailyMaintenanceID, Remarks,NameoftheBrand,CapacityofSplit,CoolingArea,PresentCondition FROM DailyMaintenance WHERE DailyMaintenanceID='" + lblId.Text + "'");
    //            foreach (DataRow dtx in dt.Rows)
    //            {
    //                txtNameoftheBrand.Text = dtx["NameoftheBrand"].ToString();
    //                txtCapacityofSplit.Text = dtx["CapacityofSplit"].ToString();
    //                txtCoolingArea.Text = dtx["CoolingArea"].ToString();
    //                txtPresentCondition.Text = dtx["PresentCondition"].ToString();
    //                txtRemarks.Text = dtx["Remarks"].ToString();
    //            }
    //            BindItemGrid();
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
            string query = "Delete DailyMaintenance WHERE DailyMaintenanceID='" + lblId.Text + "'";
            query += " Delete DailyMaintenanceDetails WHERE DailyMaintenanceID='" + lblId.Text + "'";
            RunQuery.SQLQuery.ExecNonQry(query);
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
    //    DataTable dt = SQLQuery.ReturnDataTable(" SELECT Convert(varchar,EntryDate,103) as EntryDate,* FROM DailyMaintenance");
    //    GridView1.DataSource = dt;
    //    GridView1.DataBind();
    //}



    private void ClearControls()
    {
        txtNameoftheBrand.Text = "";
        txtCapacityofSplit.Text = "";
        BindDdLocation();
        BindDDLocationInMainOffice();
        txtPresentCondition.Text = "";
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnAdd.Text == "ADD")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    int lvNo;
                    if (lblId.Text == "")
                    {
                        lvNo = 0;
                    }
                    else
                    {
                        lvNo = Convert.ToInt32(lblId.Text);
                    }

                    RunQuery.SQLQuery.ExecNonQry("INSERT INTO DailyMaintenanceDetails (DailyMaintenanceID, WorkType,  Sign,Date, EntryBY) VALUES ('" + lvNo + "', '" + ddlWorkType.SelectedValue + "', '" + ddlShiftIncharge.SelectedValue + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + User.Identity.Name + "')");
                    //ClearControls();
                    //Notify("Successfully Saved...", "success", lblMsg);

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
                    RunQuery.SQLQuery.ExecNonQry(" Update  DailyMaintenanceDetails SET WorkType= '" + ddlWorkType.SelectedValue + "',  Sign= '" + ddlShiftIncharge.SelectedValue + "', Date= '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' WHERE DailyMaintenanceDetailsID='" + lblItemId.Text + "' ");
                    //ClearControls();

                    btnAdd.Text = "ADD";
                    //Notify("Successfully Updated...", "success", lblMsg);
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

            BindItemGrid();
        }
    }

    private void BindItemGrid()
    {
        DataTable dt = SQLQuery.ReturnDataTable(@" SELECT DailyMaintenanceDetails.DailyMaintenanceDetailsID, DailyMaintenanceDetails.DailyMaintenanceID, DailyMaintenanceDetails.WorkType, DailyMaintenanceDetails.Sign, CONVERT(varchar, DailyMaintenanceDetails.Date, 103) AS Date, DailyMaintenanceDetails.EntryBy,Employee.Name
        FROM DailyMaintenanceDetails INNER JOIN Employee ON DailyMaintenanceDetails.Sign = Employee.EmployeeID Where DailyMaintenanceID='" + lblId.Text + "' AND EntryBy='" + User.Identity.Name + "'");
        AddItemsGridView.DataSource = dt;
        AddItemsGridView.EmptyDataText = "No Records Found!";
        AddItemsGridView.DataBind();
    }

    protected void AddItemsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Delete") == "1")
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblId = AddItemsGridView.Rows[index].FindControl("Label1") as Label;
                RunQuery.SQLQuery.ExecNonQry(" Delete DailyMaintenanceDetails WHERE DailyMaintenanceDetailsID='" + lblId.Text + "' ");
                lblId.Text = "";
                BindItemGrid();
                Notify("Successfully Deleted...", "success", lblMsg);
            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify("ERROR:" + ex, "warn", lblMsg);
        }

    }

    protected void AddItemsGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
                Label lblEditId = AddItemsGridView.Rows[index].FindControl("Label1") as Label;
                lblItemId.Text = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable(" SELECT DailyMaintenanceDetailsID, DailyMaintenanceID, WorkType, Date, Sign, EntryBY FROM DailyMaintenanceDetails WHERE DailyMaintenanceDetailsID='" + lblEditId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
                    ddlWorkType.SelectedValue = dtx["WorkType"].ToString();
                    ddlShiftIncharge.SelectedValue = dtx["Sign"].ToString();

                }
                btnAdd.Text = "Update";
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string _id = GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
            string sQuery = @"SELECT DailyMaintenanceDetails.DailyMaintenanceDetailsID, DailyMaintenanceDetails.DailyMaintenanceID, DailyMaintenanceDetails.WorkType, DailyMaintenanceDetails.Sign, CONVERT(varchar, DailyMaintenanceDetails.Date, 103) AS Date, DailyMaintenanceDetails.EntryBy,Employee.Name
        FROM DailyMaintenanceDetails INNER JOIN Employee ON DailyMaintenanceDetails.Sign = Employee.EmployeeID  WHERE DailyMaintenanceID='" + _id + "'";
            GridView SC = (GridView)e.Row.FindControl("GridWorkDetails");
            SC.DataSource = SQLQuery.ReturnDataTable(sQuery);
            SC.DataBind();
        }
    }
    private void EditMode(string id)
    {
        hiddenDMNID.Value = id;
        DataTable dt = SQLQuery.ReturnDataTable(" Select DailyMaintenanceID, DMNVoucher, MainOfficeId, Remarks, NameoftheBrand,CapacityofSplit,CoolingArea,PresentCondition,ShiftInCharge FROM DailyMaintenance WHERE DailyMaintenanceID='" + lblId.Text + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            hiddenDMNVoucher.Value = dtx["DMNVoucher"].ToString();
            txtDMNumber.Text = dtx["DMNVoucher"].ToString();
            txtNameoftheBrand.Text = dtx["NameoftheBrand"].ToString();
            txtCapacityofSplit.Text = dtx["CapacityofSplit"].ToString();
            BindDdLocation();
            ddLocationID.SelectedValue = dtx["MainOfficeId"].ToString();
            BindDDLocationInMainOffice();
            ddLocationInMainOffice.SelectedValue = dtx["CoolingArea"].ToString();
            txtPresentCondition.Text = dtx["PresentCondition"].ToString();
            txtRemarks.Text = dtx["Remarks"].ToString();
            ddShiftInCharge.SelectedValue = dtx["ShiftInCharge"].ToString();
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
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM DailyMaintenance WHERE DailyMaintenanceID='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM DailyMaintenance WHERE DailyMaintenanceID='" + lblEditId.Text + "'");
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

        string sql = @"SELECT D.DailyMaintenanceID, D.DMNVoucher, D.MainOfficeId, D.FunctionalOfficeId, D.DepartmentId, D.FinYear, D.NameoftheBrand, D.CapacityofSplit, D.CoolingArea, D.PresentCondition, D.PreparedBy, D.PreparedDate, D.Checkerby, D.CheckerDate, 
D.Approvedby, D.ApprovedDate, D.SaveMode, D.SubmitDate, D.WorkflowStatus, D.WorkflowApprovedDate, D.CurrentWorkflowUser, D.ReturnOrHoldUserID, D.Remarks, D.EntryDate, D.EntryBy, E.Name AS ShiftInCharge FROM DailyMaintenance AS D INNER JOIN Employee AS E ON D.ShiftInCharge = E.EmployeeID " + query + "";
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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenDMNID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'DMN'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenDMNID.Value + "' AND WorkFlowType = 'DMN'");
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
        if (hiddenDMNID.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hiddenDMNID.Value);
        }

        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,'DMN',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        //if (hiddenDMNID.Value == "")
        //{
        //    hiddenDMNID.Value = "0";
        //}
        query = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE WorkFlowTypeID='" + hiddenDMNID.Value + "' AND WorkFlowUser.WorkFlowType = 'DMN' AND EntryBy = '" + lName + "' Order By Priority ASC";

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
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hiddenDMNID.Value + "' AND WorkFlowType = 'DMN' AND EntryBy = '" + User.Identity.Name + "'");

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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenDMNID.Value + "' AND WorkFlowType = 'DMN'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenDMNID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'DMN'");
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
        SQLQuery.PopulateDropDown("SELECT SequenceId, SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'DMN')", ddlPriority, "Priority", "SequenceBan");
    }
    protected void btnWorkFlowSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;
            string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hiddenDMNID.Value + "' AND WorkFlowType = 'DMN' AND EntryBy = '" + lName + "'");
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

            SQLQuery.ExecNonQry("UPDATE DailyMaintenance SET CurrentWorkflowUser='" + name + "' WHERE DailyMaintenanceID = '" + mnId + "'");
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
        //(" INSERT INTO DailyMaintenance (Date, SuppliedBy, MeterNumber, ConsumptionTime, KVARH, MaximumDemand, UHICondition, [1C-Peak], [2C-Peak], ReadingTakenBy, Remarks,EntryBy) VALUES ('" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + ddlSupplied.SelectedValue + "', '" + txtMeterNumber.Text.Replace("'", "''") + "', '" + ddlConsumptionTime.SelectedValue + "', '" + txtKVARH.Text.Replace("'", "''") + "', '" + txtMaximumDemand.Text.Replace("'", "''") + "', '" + txtUHICondition.Text.Replace("'", "''") + "', '" + txt1CPeak.Text.Replace("'", "''") + "', '" + txt2CPeak.Text.Replace("'", "''") + "', '" + txtReadingTakenBy.Text.Replace("'", "''") + "', '" + txtRemarks.Text.Replace("'", "''") + "','" + lName + "')");
        SQLQuery.ExecNonQry("INSERT INTO DailyMaintenance (" + sqlColumn + "SaveMode, Date, DMNVoucher, MainOfficeId, FunctionalOfficeId, DepartmentId, FinYear, NameoftheBrand, CapacityofSplit, CoolingArea, PresentCondition, Remarks, PreparedBy, EntryBy, ShiftInCharge) VALUES (" + sqlValue + "'" + saveMode + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtDMNumber.Text + "','" + SQLQuery.GetLocationID(User.Identity.Name) + "','" + SQLQuery.GetCenterId(User.Identity.Name) + "','" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "','" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "','" + txtNameoftheBrand.Text.Replace("'", "''") + "', '" + txtCapacityofSplit.Text.Replace("'", "''") + "', '" + ddLocationInMainOffice.SelectedValue + "', '" + txtPresentCondition.Text.Replace("'", "''") + "', '" + txtRemarks.Text.Replace("'", "''") + "','" + SQLQuery.GetEmployeeID(User.Identity.Name) + "','" + User.Identity.Name + "','" + ddShiftInCharge.SelectedValue + "')");
        string maxId = SQLQuery.ReturnString(@"SELECT MAX(DailyMaintenanceID) AS DailyMaintenanceID FROM DailyMaintenance WHERE(EntryBy = '" + lName + "') ");
        SQLQuery.ExecNonQry("Update DailyMaintenanceDetails SET DailyMaintenanceID='" + maxId + "' WHERE DailyMaintenanceID='0' AND EntryBy='" + lName + "'");
        string mnId = SQLQuery.ReturnString("SELECT MAX(DailyMaintenanceID) AS mnId FROM DailyMaintenance WHERE EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + mnId + "',VoucherNo='" + txtDMNumber.Text + "'  WHERE WorkFlowTypeID = '0' AND WorkFlowType='DMN' AND EntryBy='" + lName + "' ");

        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                            PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + mnId + "' AND WorkFlowType='DMN'";

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
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM DailyMaintenance WHERE DailyMaintenanceID='" + hiddenDMNID.Value + "'");
        if (workflowStatus == "Return")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM DailyMaintenance WHERE DailyMaintenanceID='" + hiddenDMNID.Value + "'");
            workflowStatus = "Pending";
        }

        string sqlColumn = "";
        if (saveMode == "Submitted")
        {
            sqlColumn = ",SubmitDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
            // sqlValue = DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss tt");
        }
        RunQuery.SQLQuery.ExecNonQry("UPDATE DailyMaintenance SET NameoftheBrand= '" + txtNameoftheBrand.Text.Replace("'", "''") + "',  CapacityofSplit= '" + txtCapacityofSplit.Text.Replace("'", "''") + "',  CoolingArea= '" + ddLocationInMainOffice.SelectedValue + "',  PresentCondition= '" + txtPresentCondition.Text.Replace("'", "''") + "',Remarks='" + txtRemarks.Text.Replace("'", "''") + "', ShiftInCharge = '" + ddShiftInCharge.SelectedValue + "', WorkflowStatus='" + workflowStatus + "', SaveMode='" + saveMode + "'" + sqlColumn + " WHERE DailyMaintenanceID='" + lblId.Text + "' ");
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
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hiddenDMNID.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hiddenDMNID.Value + "' AND WorkFlowType='DMN'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hiddenDMNID.Value);
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
                    txtDMNumber.Text = LogMonitorGenerateVoucher.GetDMNVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
                    hiddenDMNID.Value = "";
                    hiddenWorkFlowUserID.Value = "";
                    hiddenDMNVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtDMNumber.Text = LogMonitorGenerateVoucher.GetDMNVoucherNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));

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
                    txtDMNumber.Text = LogMonitorGenerateVoucher.GetDMNVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
                    hiddenDMNID.Value = "";
                    hiddenWorkFlowUserID.Value = "";
                    hiddenDMNVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtDMNumber.Text = LogMonitorGenerateVoucher.GetDMNVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
    private void BindDdLocation()
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
