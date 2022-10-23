using RunQuery;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_Chiller : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtCLNumber.Text = LogMonitorGenerateVoucher.GetCLVoucherNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));
            BindGrid();
            BindDesignation();
            BindEmployee();
            BindddPriority();
            BindWorkFlowUserGridView();
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


    //protected void btnSave_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string lName = Page.User.Identity.Name.ToString();
    //        if (btnSave.Text == "Save")
    //        {
    //            if (SQLQuery.OparatePermission(lName, "Insert") == "1")
    //            {
    //                RunQuery.SQLQuery.ExecNonQry(" INSERT INTO Chiller (Date, ReadingTaken, ShiftIncharge, Time, ChillerMode, ActiveChilledWaterSetpoint, AverageLineCurrent, ActiveCurrentLimitSetpoint, EvapEnteringWaterTemperature, EvapLeavingWaterTemperature, EvapSatRfgtTemp, EvapApproachTemp, EvapWaterFlowSwitchStatus, ExpansionValvePosition, EvapRfgtLiquidlevel, CondEnteringWaterTemp, CondSatRfgtTemp, CondRftgPressure, CondApproachTemp, CondWaterFlowSwtichSatatus, CompressorStarts, CompressorRuntime, SystemRfgtDiffPressure, OilPressure, CompressorRfgtDischargeTemp, RLA, Amps, VoltsABBCCA, EntryBy) VALUES ('" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtReadingTaken.Text.Replace("'", "''") + "', '" + txtShiftIncharge.Text.Replace("'", "''") + "', '" + txtTime.Text.Replace("'", "''") + "', '" + ddlChillerMode.SelectedValue + "', '" + txtActiveChilledWaterSetpoint.Text.Replace("'", "''") + "', '" + txtAverageLineCurrent.Text.Replace("'", "''") + "', '" + txtActiveCurrentLimitSetpoint.Text.Replace("'", "''") + "', '" + txtEvapEnteringWaterTemperature.Text.Replace("'", "''") + "', '" + txtEvapLeavingWaterTemperature.Text.Replace("'", "''") + "', '" + txtEvapSatRfgtTemp.Text.Replace("'", "''") + "', '" + txtEvapApproachTemp.Text.Replace("'", "''") + "', '" + txtEvapWaterFlowSwitchStatus.Text.Replace("'", "''") + "', '" + txtExpansionValvePosition.Text.Replace("'", "''") + "', '" + txtEvapRfgtLiquidlevel.Text.Replace("'", "''") + "', '" + txtCondEnteringWaterTemp.Text.Replace("'", "''") + "', '" + txtCondSatRfgtTemp.Text.Replace("'", "''") + "', '" + txtCondRftgPressure.Text.Replace("'", "''") + "', '" + txtCondApproachTemp.Text.Replace("'", "''") + "', '" + txtCondWaterFlowSwtichSatatus.Text.Replace("'", "''") + "', '" + txtCompressorStarts.Text.Replace("'", "''") + "', '" + txtCompressorRuntime.Text.Replace("'", "''") + "', '" + txtSystemRfgtDiffPressure.Text.Replace("'", "''") + "', '" + txtOilPressure.Text.Replace("'", "''") + "', '" + txtCompressorRfgtDischargeTemp.Text.Replace("'", "''") + "', '" + txtRLA.Text.Replace("'", "''") + "', '" + txtAmps.Text.Replace("'", "''") + "', '" + txtVoltsABBCCA.Text.Replace("'", "''") + "', '" + User.Identity.Name + "')    ");
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
    //                RunQuery.SQLQuery.ExecNonQry(" Update  Chiller SET Date= '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',  ReadingTaken= '" + txtReadingTaken.Text.Replace("'", "''") + "',  ShiftIncharge= '" + txtShiftIncharge.Text.Replace("'", "''") + "',  Time= '" + txtTime.Text.Replace("'", "''") + "',  ChillerMode= '" +ddlChillerMode.SelectedValue + "',  ActiveChilledWaterSetpoint= '" + txtActiveChilledWaterSetpoint.Text.Replace("'", "''") + "',  AverageLineCurrent= '" + txtAverageLineCurrent.Text.Replace("'", "''") + "',  ActiveCurrentLimitSetpoint= '" + txtActiveCurrentLimitSetpoint.Text.Replace("'", "''") + "',  EvapEnteringWaterTemperature= '" + txtEvapEnteringWaterTemperature.Text.Replace("'", "''") + "',  EvapLeavingWaterTemperature= '" + txtEvapLeavingWaterTemperature.Text.Replace("'", "''") + "',  EvapSatRfgtTemp= '" + txtEvapSatRfgtTemp.Text.Replace("'", "''") + "',  EvapApproachTemp= '" + txtEvapApproachTemp.Text.Replace("'", "''") + "',  EvapWaterFlowSwitchStatus= '" + txtEvapWaterFlowSwitchStatus.Text.Replace("'", "''") + "',  ExpansionValvePosition= '" + txtExpansionValvePosition.Text.Replace("'", "''") + "',  EvapRfgtLiquidlevel= '" + txtEvapRfgtLiquidlevel.Text.Replace("'", "''") + "',  CondEnteringWaterTemp= '" + txtCondEnteringWaterTemp.Text.Replace("'", "''") + "',  CondSatRfgtTemp= '" + txtCondSatRfgtTemp.Text.Replace("'", "''") + "',  CondRftgPressure= '" + txtCondRftgPressure.Text.Replace("'", "''") + "',  CondApproachTemp= '" + txtCondApproachTemp.Text.Replace("'", "''") + "',  CondWaterFlowSwtichSatatus= '" + txtCondWaterFlowSwtichSatatus.Text.Replace("'", "''") + "',  CompressorStarts= '" + txtCompressorStarts.Text.Replace("'", "''") + "',  CompressorRuntime= '" + txtCompressorRuntime.Text.Replace("'", "''") + "',  SystemRfgtDiffPressure= '" + txtSystemRfgtDiffPressure.Text.Replace("'", "''") + "',  OilPressure= '" + txtOilPressure.Text.Replace("'", "''") + "',  CompressorRfgtDischargeTemp= '" + txtCompressorRfgtDischargeTemp.Text.Replace("'", "''") + "',  RLA= '" + txtRLA.Text.Replace("'", "''") + "',  Amps= '" + txtAmps.Text.Replace("'", "''") + "',  VoltsABBCCA= '" + txtVoltsABBCCA.Text.Replace("'", "''") + "' WHERE CillerID='" + lblId.Text + "' ");
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
    //            DataTable dt = SQLQuery.ReturnDataTable(" Select CillerID, Date,ReadingTaken,ShiftIncharge,Time,ChillerMode,ActiveChilledWaterSetpoint,AverageLineCurrent,ActiveCurrentLimitSetpoint,EvapEnteringWaterTemperature,EvapLeavingWaterTemperature,EvapSatRfgtTemp,EvapApproachTemp,EvapWaterFlowSwitchStatus,ExpansionValvePosition,EvapRfgtLiquidlevel,CondEnteringWaterTemp,CondSatRfgtTemp,CondRftgPressure,CondApproachTemp,CondWaterFlowSwtichSatatus,CompressorStarts,CompressorRuntime,SystemRfgtDiffPressure,OilPressure,CompressorRfgtDischargeTemp,RLA,Amps,VoltsABBCCA FROM Chiller WHERE CillerID='" + lblId.Text + "'");
    //            foreach (DataRow dtx in dt.Rows)
    //            {
    //                txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
    //                txtReadingTaken.Text = dtx["ReadingTaken"].ToString();
    //                txtShiftIncharge.Text = dtx["ShiftIncharge"].ToString();
    //                txtTime.Text = dtx["Time"].ToString();
    //                ddlChillerMode.SelectedValue= dtx["ChillerMode"].ToString();
    //                txtActiveChilledWaterSetpoint.Text = dtx["ActiveChilledWaterSetpoint"].ToString();
    //                txtAverageLineCurrent.Text = dtx["AverageLineCurrent"].ToString();
    //                txtActiveCurrentLimitSetpoint.Text = dtx["ActiveCurrentLimitSetpoint"].ToString();
    //                txtEvapEnteringWaterTemperature.Text = dtx["EvapEnteringWaterTemperature"].ToString();
    //                txtEvapLeavingWaterTemperature.Text = dtx["EvapLeavingWaterTemperature"].ToString();
    //                txtEvapSatRfgtTemp.Text = dtx["EvapSatRfgtTemp"].ToString();
    //                txtEvapApproachTemp.Text = dtx["EvapApproachTemp"].ToString();
    //                txtEvapWaterFlowSwitchStatus.Text = dtx["EvapWaterFlowSwitchStatus"].ToString();
    //                txtExpansionValvePosition.Text = dtx["ExpansionValvePosition"].ToString();
    //                txtEvapRfgtLiquidlevel.Text = dtx["EvapRfgtLiquidlevel"].ToString();
    //                txtCondEnteringWaterTemp.Text = dtx["CondEnteringWaterTemp"].ToString();
    //                txtCondSatRfgtTemp.Text = dtx["CondSatRfgtTemp"].ToString();
    //                txtCondRftgPressure.Text = dtx["CondRftgPressure"].ToString();
    //                txtCondApproachTemp.Text = dtx["CondApproachTemp"].ToString();
    //                txtCondWaterFlowSwtichSatatus.Text = dtx["CondWaterFlowSwtichSatatus"].ToString();
    //                txtCompressorStarts.Text = dtx["CompressorStarts"].ToString();
    //                txtCompressorRuntime.Text = dtx["CompressorRuntime"].ToString();
    //                txtSystemRfgtDiffPressure.Text = dtx["SystemRfgtDiffPressure"].ToString();
    //                txtOilPressure.Text = dtx["OilPressure"].ToString();
    //                txtCompressorRfgtDischargeTemp.Text = dtx["CompressorRfgtDischargeTemp"].ToString();
    //                txtRLA.Text = dtx["RLA"].ToString();
    //                txtAmps.Text = dtx["Amps"].ToString();
    //                txtVoltsABBCCA.Text = dtx["VoltsABBCCA"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete Chiller WHERE CillerID='" + lblId.Text + "' ");
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
    //    DataTable dt = SQLQuery.ReturnDataTable(" SELECT Convert(varchar, Date,103) AS Date,* FROM Chiller");
    //    GridView1.DataSource = dt;
    //    GridView1.DataBind();
    //}



    private void ClearControls()
    {
        txtDate.Text = "";
        txtReadingTaken.Text = "";
        //txtShiftIncharge.Text = "";
        txtTime.Text = "";
        // txtChillerMode.Text = "";
        txtActiveChilledWaterSetpoint.Text = "";
        txtAverageLineCurrent.Text = "";
        txtActiveCurrentLimitSetpoint.Text = "";
        txtEvapEnteringWaterTemperature.Text = "";
        txtEvapLeavingWaterTemperature.Text = "";
        txtEvapSatRfgtTemp.Text = "";
        txtEvapApproachTemp.Text = "";
        txtEvapWaterFlowSwitchStatus.Text = "";
        txtExpansionValvePosition.Text = "";
        txtEvapRfgtLiquidlevel.Text = "";
        txtCondEnteringWaterTemp.Text = "";
        txtCondSatRfgtTemp.Text = "";
        txtCondRftgPressure.Text = "";
        txtCondApproachTemp.Text = "";
        txtCondWaterFlowSwtichSatatus.Text = "";
        txtCompressorStarts.Text = "";
        txtCompressorRuntime.Text = "";
        txtSystemRfgtDiffPressure.Text = "";
        txtOilPressure.Text = "";
        txtCompressorRfgtDischargeTemp.Text = "";
        txtRLA.Text = "";
        txtAmps.Text = "";
        txtVoltsABBCCA.Text = "";

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
        hiddenCLID.Value = id;
        DataTable dt = SQLQuery.ReturnDataTable("Select CillerID, CLVoucher, Date,ReadingTaken,ShiftIncharge,Time,ChillerMode,ActiveChilledWaterSetpoint,AverageLineCurrent,ActiveCurrentLimitSetpoint,EvapEnteringWaterTemperature,EvapLeavingWaterTemperature,EvapSatRfgtTemp,EvapApproachTemp,EvapWaterFlowSwitchStatus,ExpansionValvePosition,ExpansionValvePositionSteps,EvapRfgtLiquidlevel,CondEnteringWaterTemp,CondLeavingWaterTemp,CondSatRfgtTemp,CondRftgPressure,CondApproachTemp,CondWaterFlowSwtichSatatus,CompressorStarts,CompressorRuntime,SystemRfgtDiffPressure,OilPressure,CompressorRfgtDischargeTemp,RLA,Amps,VoltsABBCCA FROM Chiller WHERE CillerID='" + lblId.Text + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            hiddenCLVoucher.Value = dtx["CLVoucher"].ToString();
            txtCLNumber.Text = dtx["CLVoucher"].ToString();
            txtDate.Text = Convert.ToDateTime(dtx["Date"]).ToString("dd/MM/yyyy");
            txtReadingTaken.Text = dtx["ReadingTaken"].ToString();
            ddShiftInCharge.SelectedValue = dtx["ShiftIncharge"].ToString();
            txtTime.Text = dtx["Time"].ToString();
            ddlChillerMode.SelectedValue = dtx["ChillerMode"].ToString();
            txtActiveChilledWaterSetpoint.Text = dtx["ActiveChilledWaterSetpoint"].ToString();
            txtAverageLineCurrent.Text = dtx["AverageLineCurrent"].ToString();
            txtActiveCurrentLimitSetpoint.Text = dtx["ActiveCurrentLimitSetpoint"].ToString();
            txtEvapEnteringWaterTemperature.Text = dtx["EvapEnteringWaterTemperature"].ToString();
            txtEvapLeavingWaterTemperature.Text = dtx["EvapLeavingWaterTemperature"].ToString();
            txtEvapSatRfgtTemp.Text = dtx["EvapSatRfgtTemp"].ToString();
            txtEvapApproachTemp.Text = dtx["EvapApproachTemp"].ToString();
            txtEvapWaterFlowSwitchStatus.Text = dtx["EvapWaterFlowSwitchStatus"].ToString();
            txtExpansionValvePosition.Text = dtx["ExpansionValvePosition"].ToString();
            txtExpansionValvePositionSteps.Text = dtx["ExpansionValvePositionSteps"].ToString();
            txtEvapRfgtLiquidlevel.Text = dtx["EvapRfgtLiquidlevel"].ToString();
            txtCondEnteringWaterTemp.Text = dtx["CondEnteringWaterTemp"].ToString();
            txtCondLeaveingWaterTemp.Text = dtx["CondLeavingWaterTemp"].ToString();
            txtCondSatRfgtTemp.Text = dtx["CondSatRfgtTemp"].ToString();
            txtCondRftgPressure.Text = dtx["CondRftgPressure"].ToString();
            txtCondApproachTemp.Text = dtx["CondApproachTemp"].ToString();
            txtCondWaterFlowSwtichSatatus.Text = dtx["CondWaterFlowSwtichSatatus"].ToString();
            txtCompressorStarts.Text = dtx["CompressorStarts"].ToString();
            txtCompressorRuntime.Text = dtx["CompressorRuntime"].ToString();
            txtSystemRfgtDiffPressure.Text = dtx["SystemRfgtDiffPressure"].ToString();
            txtOilPressure.Text = dtx["OilPressure"].ToString();
            txtCompressorRfgtDischargeTemp.Text = dtx["CompressorRfgtDischargeTemp"].ToString();
            txtRLA.Text = dtx["RLA"].ToString();
            txtAmps.Text = dtx["Amps"].ToString();
            txtVoltsABBCCA.Text = dtx["VoltsABBCCA"].ToString();

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
                Label labelCLVoucher = GridView1.Rows[index].FindControl("lblCLNumber") as Label;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM Chiller WHERE CillerID='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM Chiller WHERE CillerID='" + lblEditId.Text + "'");
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
                        Notify("This " + labelCLVoucher.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("This number" + labelCLVoucher.Text + " entry by user is " + lblEntryBy.Text + ". You are not authorize edit the voucher. If you need to any change please contact higher authority.", "warn", lblMsg);
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

        string sql = @"SELECT C.CillerID, C.CLVoucher, C.MainOfficeId, C.FunctionalOfficeId, C.DepartmentId, C.FinYear, C.Date, C.ReadingTaken, E.Name AS ShiftIncharge, C.Time, C.ChillerMode, C.ActiveChilledWaterSetpoint, C.AverageLineCurrent, C.ActiveCurrentLimitSetpoint, C.
EvapEnteringWaterTemperature, C.EvapLeavingWaterTemperature, C.EvapSatRfgtTemp, C.EvapApproachTemp, C.EvapWaterFlowSwitchStatus, C.ExpansionValvePosition, C.ExpansionValvePositionSteps, C.EvapRfgtLiquidlevel, C.CondEnteringWaterTemp, C.CondLeavingWaterTemp, C.
CondSatRfgtTemp, C.CondRftgPressure, C.CondApproachTemp, C.CondWaterFlowSwtichSatatus, C.CompressorStarts, C.CompressorRuntime, C.SystemRfgtDiffPressure, C.OilPressure, C.CompressorRfgtDischargeTemp, C.RLA, C.Amps, C.
VoltsABBCCA, C.PreparedBy, C.PreparedDate, C.Checkerby, C.CheckerDate, C.Approvedby, C.ApprovedDate, C.SaveMode, C.SubmitDate, C.WorkflowStatus, C.WorkflowApprovedDate, C.CurrentWorkflowUser, C.ReturnOrHoldUserID, C.Remarks, C.
EntryBy FROM Chiller AS C INNER JOIN Employee AS E ON C.ShiftInCharge = E.EmployeeID " + query + "";
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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenCLID.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'CL'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenCLID.Value + "' AND WorkFlowType = 'CL'");
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
        if (hiddenCLID.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hiddenCLID.Value);
        }

        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,'CL',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

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
        //if (hiddenCLID.Value == "")
        //{
        //    hiddenCLID.Value = "0";
        //}
        query = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE WorkFlowTypeID='" + hiddenCLID.Value + "' AND WorkFlowUser.WorkFlowType = 'CL' AND EntryBy = '" + lName + "' Order By Priority ASC";

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
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hiddenCLID.Value + "' AND WorkFlowType = 'CL' AND EntryBy = '" + User.Identity.Name + "'");

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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenCLID.Value + "' AND WorkFlowType = 'CL'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hiddenCLID.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'CL'");
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
        SQLQuery.PopulateDropDown("SELECT SequenceId, SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'CL')", ddlPriority, "Priority", "SequenceBan");
    }

    protected void btnWorkFlowSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;
            string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hiddenCLID.Value + "' AND WorkFlowType = 'CL' AND EntryBy = '" + lName + "'");
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

            SQLQuery.ExecNonQry("UPDATE Chiller SET CurrentWorkflowUser='" + name + "' WHERE CillerID = '" + mnId + "'");
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
        //(" INSERT INTO Chiller (Date, SuppliedBy, MeterNumber, ConsumptionTime, KVARH, MaximumDemand, UHICondition, [1C-Peak], [2C-Peak], ReadingTakenBy, Remarks,EntryBy) VALUES ('" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + ddlSupplied.SelectedValue + "', '" + txtMeterNumber.Text.Replace("'", "''") + "', '" + ddlConsumptionTime.SelectedValue + "', '" + txtKVARH.Text.Replace("'", "''") + "', '" + txtMaximumDemand.Text.Replace("'", "''") + "', '" + txtUHICondition.Text.Replace("'", "''") + "', '" + txt1CPeak.Text.Replace("'", "''") + "', '" + txt2CPeak.Text.Replace("'", "''") + "', '" + txtReadingTakenBy.Text.Replace("'", "''") + "', '" + txtRemarks.Text.Replace("'", "''") + "','" + lName + "')");
        SQLQuery.ExecNonQry("INSERT INTO Chiller (" + sqlColumn + "SaveMode, Date, CLVoucher, MainOfficeId, FunctionalOfficeId, DepartmentId, FinYear, ReadingTaken, ShiftIncharge, Time, ChillerMode, ActiveChilledWaterSetpoint, AverageLineCurrent, ActiveCurrentLimitSetpoint, EvapEnteringWaterTemperature, EvapLeavingWaterTemperature, EvapSatRfgtTemp, EvapApproachTemp, EvapWaterFlowSwitchStatus, ExpansionValvePosition, ExpansionValvePositionSteps, EvapRfgtLiquidlevel, CondEnteringWaterTemp, CondLeavingWaterTemp, CondSatRfgtTemp, CondRftgPressure, CondApproachTemp, CondWaterFlowSwtichSatatus, CompressorStarts, CompressorRuntime, SystemRfgtDiffPressure, OilPressure, CompressorRfgtDischargeTemp, RLA, Amps, VoltsABBCCA, PreparedBy, EntryBy) VALUES (" + sqlValue + "'" + saveMode + "','" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtCLNumber.Text + "','" + SQLQuery.GetLocationID(User.Identity.Name) + "','" + SQLQuery.GetCenterId(User.Identity.Name) + "','" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "','" + GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDate.Text)) + "','" + txtReadingTaken.Text.Replace("'", "''") + "', '" + ddShiftInCharge.SelectedValue + "', '" + txtTime.Text.Replace("'", "''") + "', '" + ddlChillerMode.SelectedValue + "', '" + txtActiveChilledWaterSetpoint.Text.Replace("'", "''") + "', '" + txtAverageLineCurrent.Text.Replace("'", "''") + "', '" + txtActiveCurrentLimitSetpoint.Text.Replace("'", "''") + "', '" + txtEvapEnteringWaterTemperature.Text.Replace("'", "''") + "', '" + txtEvapLeavingWaterTemperature.Text.Replace("'", "''") + "', '" + txtEvapSatRfgtTemp.Text.Replace("'", "''") + "', '" + txtEvapApproachTemp.Text.Replace("'", "''") + "', '" + txtEvapWaterFlowSwitchStatus.Text.Replace("'", "''") + "', '" + txtExpansionValvePosition.Text.Replace("'", "''") + "', '" + txtExpansionValvePositionSteps.Text.Replace("'", "''") + "', '" + txtEvapRfgtLiquidlevel.Text.Replace("'", "''") + "', '" + txtCondEnteringWaterTemp.Text.Replace("'", "''") + "', '" + txtCondLeaveingWaterTemp.Text.Replace("'", "''") + "', '" + txtCondSatRfgtTemp.Text.Replace("'", "''") + "', '" + txtCondRftgPressure.Text.Replace("'", "''") + "', '" + txtCondApproachTemp.Text.Replace("'", "''") + "', '" + txtCondWaterFlowSwtichSatatus.Text.Replace("'", "''") + "', '" + txtCompressorStarts.Text.Replace("'", "''") + "', '" + txtCompressorRuntime.Text.Replace("'", "''") + "', '" + txtSystemRfgtDiffPressure.Text.Replace("'", "''") + "', '" + txtOilPressure.Text.Replace("'", "''") + "', '" + txtCompressorRfgtDischargeTemp.Text.Replace("'", "''") + "', '" + txtRLA.Text.Replace("'", "''") + "', '" + txtAmps.Text.Replace("'", "''") + "', '" + txtVoltsABBCCA.Text.Replace("'", "''") + "','" + SQLQuery.GetEmployeeID(User.Identity.Name) + "','" + User.Identity.Name + "')");
        string mnId = SQLQuery.ReturnString("SELECT MAX(CillerID) AS mnId FROM Chiller WHERE EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + mnId + "',VoucherNo='" + txtCLNumber.Text + "'  WHERE WorkFlowTypeID = '0' AND WorkFlowType='CL' AND EntryBy='" + lName + "' ");

        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                            PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + mnId + "' AND WorkFlowType='CL'";

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
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM Chiller WHERE CillerID='" + hiddenCLID.Value + "'");
        if (workflowStatus == "Return")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM Chiller WHERE CillerID='" + hiddenCLID.Value + "'");
            workflowStatus = "Pending";
        }

        string sqlColumn = "";
        if (saveMode == "Submitted")
        {
            sqlColumn = ",SubmitDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
            // sqlValue = DateTime.Now.ToString("yyyy - MM - dd hh: mm:ss tt");
        }
        RunQuery.SQLQuery.ExecNonQry("UPDATE Chiller SET Date= '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "',  ReadingTaken= '" + txtReadingTaken.Text.Replace("'", "''") + "',  ShiftIncharge= '" + ddShiftInCharge.SelectedValue + "',  Time= '" + txtTime.Text.Replace("'", "''") + "',  ChillerMode= '" + ddlChillerMode.SelectedValue + "',  ActiveChilledWaterSetpoint= '" + txtActiveChilledWaterSetpoint.Text.Replace("'", "''") + "',  AverageLineCurrent= '" + txtAverageLineCurrent.Text.Replace("'", "''") + "',  ActiveCurrentLimitSetpoint= '" + txtActiveCurrentLimitSetpoint.Text.Replace("'", "''") + "',  EvapEnteringWaterTemperature= '" + txtEvapEnteringWaterTemperature.Text.Replace("'", "''") + "',  EvapLeavingWaterTemperature= '" + txtEvapLeavingWaterTemperature.Text.Replace("'", "''") + "',  EvapSatRfgtTemp= '" + txtEvapSatRfgtTemp.Text.Replace("'", "''") + "',  EvapApproachTemp= '" + txtEvapApproachTemp.Text.Replace("'", "''") + "',  EvapWaterFlowSwitchStatus= '" + txtEvapWaterFlowSwitchStatus.Text.Replace("'", "''") + "',  ExpansionValvePosition= '" + txtExpansionValvePosition.Text.Replace("'", "''") + "', ExpansionValvePositionSteps= '" + txtExpansionValvePositionSteps.Text.Replace("'", "''") + "',  EvapRfgtLiquidlevel= '" + txtEvapRfgtLiquidlevel.Text.Replace("'", "''") + "',  CondEnteringWaterTemp= '" + txtCondEnteringWaterTemp.Text.Replace("'", "''") + "', CondLeavingWaterTemp= '" + txtCondLeaveingWaterTemp.Text.Replace("'", "''") + "',  CondSatRfgtTemp= '" + txtCondSatRfgtTemp.Text.Replace("'", "''") + "',  CondRftgPressure= '" + txtCondRftgPressure.Text.Replace("'", "''") + "',  CondApproachTemp= '" + txtCondApproachTemp.Text.Replace("'", "''") + "',  CondWaterFlowSwtichSatatus= '" + txtCondWaterFlowSwtichSatatus.Text.Replace("'", "''") + "',  CompressorStarts= '" + txtCompressorStarts.Text.Replace("'", "''") + "',  CompressorRuntime= '" + txtCompressorRuntime.Text.Replace("'", "''") + "',  SystemRfgtDiffPressure= '" + txtSystemRfgtDiffPressure.Text.Replace("'", "''") + "',  OilPressure= '" + txtOilPressure.Text.Replace("'", "''") + "',  CompressorRfgtDischargeTemp= '" + txtCompressorRfgtDischargeTemp.Text.Replace("'", "''") + "',  RLA= '" + txtRLA.Text.Replace("'", "''") + "',  Amps= '" + txtAmps.Text.Replace("'", "''") + "',  VoltsABBCCA= '" + txtVoltsABBCCA.Text.Replace("'", "''") + "', ShiftInCharge = '" + ddShiftInCharge.SelectedValue + "', WorkflowStatus='" + workflowStatus + "',SaveMode='" + saveMode + "'" + sqlColumn + " WHERE CillerID='" + lblId.Text + "' ");
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
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hiddenCLID.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hiddenCLID.Value + "' AND WorkFlowType='CL'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + User.Identity.Name + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hiddenCLID.Value);
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
                    txtCLNumber.Text = LogMonitorGenerateVoucher.GetCLVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
                    hiddenCLID.Value = "";
                    hiddenWorkFlowUserID.Value = "";
                    hiddenCLVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtCLNumber.Text = LogMonitorGenerateVoucher.GetCLVoucherNumber(Convert.ToDateTime(txtDate.Text), User.Identity.Name, SQLQuery.GetDepartmentSectionId(User.Identity.Name));

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
                    txtCLNumber.Text = LogMonitorGenerateVoucher.GetCLVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
                    hiddenCLID.Value = "";
                    hiddenWorkFlowUserID.Value = "";
                    hiddenCLVoucher.Value = "";
                    BindGrid();
                    BindWorkFlowUserGridView();
                    txtCLNumber.Text = LogMonitorGenerateVoucher.GetCLVoucherNumber(Convert.ToDateTime(txtDate.Text),
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
