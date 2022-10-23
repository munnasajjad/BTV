﻿using RunQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkFlowForChiller : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string id = Request.QueryString["Id"];

                WorkFlowUserGridView.EmptyDataText = "No data added ...";
                WorkFlowUserGridView.DataSource = null;
                WorkFlowUserGridView.DataBind();

                if (id != null)
                {
                    string userId = EncryptDecrypt.DecryptString(id);
                    string voucherID = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + userId + "'");
                    //string voucherNumber = SQLQuery.ReturnString("SELECT VoucherNo From WorkFlowUser Where WorkFlowUserID='" + userId + "'");

                    BindWorkFlowUserGridView(voucherID);
                    LoadData(voucherID);
                }
                PermissionToAction();
            }
            catch (Exception ex)
            {
                Notify("ERROR!: " + ex, "error", lblMsg);
            }

        }
    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void PermissionToAction()
    {
        string id = Request.QueryString["Id"];
        string wUserId = EncryptDecrypt.DecryptString(id);
        string status = SQLQuery.ReturnString("SELECT PermissionStatus From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
        if (status == "")
        {
            btnApprove.Enabled = true;
            btnHold.Enabled = true;
            btnReturn.Enabled = true;
            btnDecline.Enabled = true;
        }
        else if (status == "Approved")
        {
            btnHold.Enabled = false;
            btnReturn.Enabled = false;
            btnDecline.Enabled = false;
        }
        else if (status == "Hold")
        {
            btnApprove.Enabled = false;
            btnReturn.Enabled = false;
            btnDecline.Enabled = false;
        }
        else if (status == "Return")
        {
            btnApprove.Enabled = false;
            btnHold.Enabled = false;
            btnDecline.Enabled = false;
        }
        else if (status == "Decline")
        {
            btnApprove.Enabled = false;
            btnHold.Enabled = false;
            btnReturn.Enabled = false;
        }
    }
    private void ReloadNotification()
    {
        Repeater Repeater = Page.Master.FindControl("Repeater3") as Repeater;
        Label lblPOrder = Page.Master.FindControl("lblPOrder") as Label;
        SQLQuery.GenerateNotification(Repeater, lblPOrder, User.Identity.Name);
    }

    private void BindWorkFlowUserGridView(string id)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + clId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks, 
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'CL') ORDER BY WorkFlowUser.Priority";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }


    private void LoadData(string id)
    {
        string query = @"SELECT C.CillerID, C.CLVoucher, C.Date, C.ReadingTaken, C.Time, C.ChillerMode, C.ActiveChilledWaterSetpoint, 
                         C.AverageLineCurrent, C.ActiveCurrentLimitSetpoint, C.EvapEnteringWaterTemperature, C.EvapLeavingWaterTemperature, C.EvapSatRfgtTemp, C.EvapApproachTemp, 
                         C.EvapWaterFlowSwitchStatus, C.ExpansionValvePosition, C.ExpansionValvePositionSteps, C.EvapRfgtLiquidlevel, C.CondEnteringWaterTemp, 
                         C.CondLeavingWaterTemp, C.CondSatRfgtTemp, C.CondRftgPressure, C.CondApproachTemp, C.CondWaterFlowSwtichSatatus, C.CompressorStarts, 
                         C.CompressorRuntime, C.SystemRfgtDiffPressure, C.OilPressure, C.CompressorRfgtDischargeTemp, C.RLA, C.Amps, C.VoltsABBCCA, C.Remarks, 
                         C.SaveMode, E.Name AS ShiftInCharge FROM Chiller AS C INNER JOIN Employee AS E ON C.ShiftIncharge = E.EmployeeID WHERE CillerID='" + id + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtDate.Text = Convert.ToDateTime(dataReader["Date"]).ToString("dd/MM/yyyy");
            txtCLNumber.Text = dataReader["CLVoucher"].ToString();
            txtReadingTakenBy.Text = dataReader["ReadingTaken"].ToString();
            txtShiftInCharge.Text = dataReader["ShiftIncharge"].ToString();
            txtTime.Text = dataReader["Time"].ToString();
            txtChillerMode.Text = dataReader["ChillerMode"].ToString();
            txtActiveChilledWaterSetpoint.Text = dataReader["ActiveChilledWaterSetpoint"].ToString();
            txtAverageLineCurrent.Text = dataReader["AverageLineCurrent"].ToString();
            txtActiveCurrentLimitSetpoint.Text = dataReader["ActiveCurrentLimitSetpoint"].ToString();
            txtEvapEnteringWaterTemperature.Text = dataReader["EvapEnteringWaterTemperature"].ToString();
            txtEvapLeavingWaterTemperature.Text = dataReader["EvapLeavingWaterTemperature"].ToString();
            txtEvapSatRfgtTemp.Text = dataReader["EvapSatRfgtTemp"].ToString();
            txtEvapApproachTemp.Text = dataReader["EvapApproachTemp"].ToString();
            txtEvapWaterFlowSwitchStatus.Text = dataReader["EvapWaterFlowSwitchStatus"].ToString();
            txtExpansionValvePosition.Text = dataReader["ExpansionValvePosition"].ToString();
            txtExpansionValvePositionSteps.Text = dataReader["ExpansionValvePositionSteps"].ToString();
            txtEvapRfgtLiquidlevel.Text = dataReader["EvapRfgtLiquidlevel"].ToString();
            txtCondEnteringWaterTemp.Text = dataReader["CondEnteringWaterTemp"].ToString();
            txtCondLeavingWaterTemp.Text = dataReader["CondLeavingWaterTemp"].ToString();
            txtCondSatRfgtTemp.Text = dataReader["CondSatRfgtTemp"].ToString();
            txtCondRftgPressure.Text = dataReader["CondRftgPressure"].ToString();
            txtCondApproachTemp.Text = dataReader["CondApproachTemp"].ToString();
            txtCondWaterFlowSwtichStatus.Text = dataReader["CondWaterFlowSwtichSatatus"].ToString();
            txtCompressorStarts.Text = dataReader["CompressorStarts"].ToString();
            txtCompressorRuntime.Text = dataReader["CompressorRuntime"].ToString();
            txtSystemRfgtDiffPressure.Text = dataReader["SystemRfgtDiffPressure"].ToString();
            txtOilPressure.Text = dataReader["OilPressure"].ToString();
            txtCompressorRfgtDischargeTemp.Text = dataReader["CompressorRfgtDischargeTemp"].ToString();
            txtRLAL1L2L3.Text = dataReader["RLA"].ToString();
            txtAmpsL1L2L3.Text = dataReader["Amps"].ToString();
            txtVoltsABBCCA.Text = dataReader["VoltsABBCCA"].ToString();
            txtRemarks.Text = dataReader["Remarks"].ToString();
        }
    }
    protected void btnApprove_OnClick(object sender, EventArgs e)
    {
        if (WorkFlowUserGridView.Rows.Count > 0)
        {
            string id = Request.QueryString["Id"];
            string wUserId = EncryptDecrypt.DecryptString(id);
            string statusPermission = SQLQuery.ReturnString("SELECT PermissionStatus From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
            if (statusPermission != "Approved")
            {
                try
                {
                    DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName,Employee.EmployeeID, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE WorkFlowUserID='" + wUserId + "'");
                    string empID = "";
                    string empName = "";
                    int userPpriority = 0;
                    int nextUserPriority = 0;
                    string esId = "";
                    if (userDetails.Rows.Count > 0)
                    {
                        empID = userDetails.Rows[0]["EmployeeID"].ToString();
                        empName = userDetails.Rows[0]["EmployeeName"].ToString();
                        userPpriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                        esId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
                    }
                    if (userPpriority > 0)
                    {
                        nextUserPriority = userPpriority + 1;
                    }

                    DataTable nextUserDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                  Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE [Priority]='" + nextUserPriority + "' AND WorkFlowTypeID='" + esId + "' AND WFU.WorkFlowType='CL'");
                    string nextEmpName = "";
                    string nextUserId = "";
                    //string priorty = "0";
                    string nextTypeID = "";
                    string nextEmail = "";
                    string nextVoucherNo = "";
                    string escday = "0";
                    if (nextUserDetails.Rows.Count > 0)
                    {
                        nextUserId = nextUserDetails.Rows[0]["WorkFlowUserID"].ToString();
                        nextEmpName = nextUserDetails.Rows[0]["EmployeeName"].ToString();
                        nextTypeID = nextUserDetails.Rows[0]["WorkFlowTypeID"].ToString();
                        nextEmail = nextUserDetails.Rows[0]["Email"].ToString();
                        nextVoucherNo = nextUserDetails.Rows[0]["VoucherNo"].ToString();
                        escday = nextUserDetails.Rows[0]["EsclationDay"].ToString();
                        string emailBody = "Dear " + nextEmpName +
                                      ", <br><br>Approve workflow, check your notification .<br><br>";

                        emailBody += " <br><br>Regards, <br><br>Development Team.";

                        SQLQuery.SendEmail2(nextEmail, "btvstoremanagementsystem@gmail.com", "Workflow for #" + nextVoucherNo, emailBody);
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(escday));
                        SQLQuery.ExecNonQry("Update WorkFlowUser SET IsActive='1', EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "' Where WorkFlowUserID='" + nextUserId + "'");
                        SQLQuery.ExecNonQry("UPDATE Chiller SET CurrentWorkflowUser='" + nextEmpName + "' WHERE CillerID = '" + esId + "'");
                    }
                    string status = "";
                    string updateQuery = "";
                    if (nextUserDetails.Rows.Count == 0)
                    {
                        status = "Approved";
                        updateQuery = "WorkflowStatus='" + status + "', WorkflowApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "',";

                    }
                    if (userPpriority == 1)
                    {
                        updateQuery += "Checkerby='" + wUserId + "', CheckerDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 2)
                    {
                        updateQuery += "Approvedby='" + wUserId + "',ApprovedDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }


                    RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "', PermissionStatus= 'Approved',TaskStatus= '0',IsActive ='0',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                    RunQuery.SQLQuery.ExecNonQry(" Update Chiller SET  " + updateQuery + " WHERE CillerID='" + esId + "'");
                    BindWorkFlowUserGridView(esId);
                    txtYourRemark.Text = "";
                    ReloadNotification();
                    PermissionToAction();
                }
                catch (Exception ex)
                {
                    Notify("ERROR!: " + ex, "error", lblMsg);
                }
            }
            else
            {
                Notify("You are already approved this workflow", "warning", lblMsg);
            }
        }
    }

    protected void btnHold_OnClick(object sender, EventArgs e)
    {
        if (WorkFlowUserGridView.Rows.Count > 0)
        {
            try
            {
                string id = Request.QueryString["Id"];
                string wUserId = EncryptDecrypt.DecryptString(id);
                string clId = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
                string status = SQLQuery.ReturnString("SELECT PermissionStatus From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
                if (status != "Hold")
                {
                    DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN
                  DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                  Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE  WorkFlowTypeID='" + clId + "' AND WorkFlowType='CL'");
                    string empName = "";
                    string email = "";
                    string voucherNo = "";
                    foreach (DataRow item in userDetails.Rows)
                    {
                        empName = item["EmployeeName"].ToString();
                        email = item["Email"].ToString();
                        voucherNo = item["VoucherNo"].ToString();
                        string emailBody = "Dear " + empName +
                                      ", <br><br>Workflow Hold<br><br>";

                        emailBody += " <br><br>Regards, <br><br>Development Team.";
                        SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow Hold For #" + voucherNo, emailBody);

                    }

                    SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "', PermissionStatus= 'Hold',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                    SQLQuery.ExecNonQry(" Update Chiller SET WorkflowStatus='Hold',ReturnOrHoldUserID='" + wUserId + "',WorkflowApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "' WHERE CillerID='" + clId + "'");
                    BindWorkFlowUserGridView(clId);
                    txtYourRemark.Text = "";
                    Notify("Work flow hold", "success", lblMsg);
                    PermissionToAction();
                    ReloadNotification();
                }
                else
                {
                    Notify("You are already hold this workflow", "warning", lblMsg);
                }
            }
            catch (Exception ex)
            {
                Notify("ERROR!: " + ex, "error", lblMsg);
            }
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (WorkFlowUserGridView.Rows.Count > 0)
        {
            string id = Request.QueryString["Id"];
            string wUserId = EncryptDecrypt.DecryptString(id);
            string status = SQLQuery.ReturnString("SELECT PermissionStatus From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
            if (status != "Return")
            {
                try
                {
                    DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName,Employee.EmployeeID, WFU.EntryBy,Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE WorkFlowUserID='" + wUserId + "'");
                    string empID = "";
                    string empName = "";
                    int userPriority = 0;
                    string entryBy = "";
                    string clId = "";
                    string voucherNo = "";
                    if (userDetails.Rows.Count > 0)
                    {
                        empID = userDetails.Rows[0]["EmployeeID"].ToString();
                        empName = userDetails.Rows[0]["EmployeeName"].ToString();
                        userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                        clId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
                        entryBy = userDetails.Rows[0]["EntryBy"].ToString();
                        voucherNo = userDetails.Rows[0]["VoucherNo"].ToString();
                    }


                    DataTable dtEntryBy = SQLQuery.ReturnDataTable(@"SELECT Employee.EmployeeID, Employee.Name, Employee.Email
                                                    FROM Logins INNER JOIN Employee ON Logins.EmployeeInfoID = Employee.EmployeeID WHERE  (Logins.LoginUserName = '" + entryBy + "')");
                    foreach (DataRow item in dtEntryBy.Rows)
                    {
                        string name = item["Name"].ToString();
                        string email = item["Email"].ToString();
                        string emailBody = "Dear " + name + ", <br><br>Workflow return from " + empName + ", please check your earth station.<br><br>";
                        emailBody += " <br><br>Regards, <br><br>Development Team.";

                        SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow Return for #" + voucherNo, emailBody);
                    }

                    RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Return', IsActive= '0', UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                    RunQuery.SQLQuery.ExecNonQry(" Update Chiller SET WorkflowStatus='Return', ReturnOrHoldUserID='" + wUserId + "' WHERE CillerID='" + clId + "'");
                    BindWorkFlowUserGridView(clId);
                    txtYourRemark.Text = "";
                    PermissionToAction();
                    ReloadNotification();
                }
                catch (Exception ex)
                {
                    Notify("ERROR!: " + ex, "error", lblMsg);
                }
            }
            else
            {
                Notify("You are already return this workflow", "warning", lblMsg);
            }
        }
    }
    protected void btnDecline_OnClick(object sender, EventArgs e)
    {
        string id = Request.QueryString["Id"];
        string wUserId = EncryptDecrypt.DecryptString(id);
        string status = SQLQuery.ReturnString("SELECT PermissionStatus From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
        if (status != "Decline")
        {
            try
            {
                DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName,Employee.EmployeeID, WFU.EntryBy,Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE WorkFlowUserID='" + wUserId + "'");
                string empID = "";
                string empName = "";
                int userPriority = 0;
                string entryBy = "";
                string clId = "";
                string voucherNo = "";
                if (userDetails.Rows.Count > 0)
                {
                    empID = userDetails.Rows[0]["EmployeeID"].ToString();
                    empName = userDetails.Rows[0]["EmployeeName"].ToString();
                    userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                    clId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
                    entryBy = userDetails.Rows[0]["EntryBy"].ToString();
                    voucherNo = userDetails.Rows[0]["VoucherNo"].ToString();
                }


                DataTable dtEntryBy = SQLQuery.ReturnDataTable(@"SELECT Employee.EmployeeID, Employee.Name, Employee.Email
                                                    FROM Logins INNER JOIN Employee ON Logins.EmployeeInfoID = Employee.EmployeeID WHERE  (Logins.LoginUserName = '" + entryBy + "')");
                foreach (DataRow item in dtEntryBy.Rows)
                {
                    string name = item["Name"].ToString();
                    string email = item["Email"].ToString();
                    string emailBody = "Dear " + name + ", <br><br>Workflow decline from " + empName + ", please check your grn.<br><br>";
                    emailBody += " <br><br>Regards, <br><br>Development Team.";
                    SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow Decline for #" + voucherNo, emailBody);
                }

                //DataTable allUserDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                //      FROM WorkFlowUser AS WFU INNER JOIN
                //      DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                //      Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE  WorkFlowTypeID='" + grnId + "' AND WorkFlowUserID<>'" + wUserId + "'");
                //string employeeName = "";
                //string emailAddress = "";
                //string grnVoucherNo = "";
                //foreach (DataRow item in allUserDetails.Rows)
                //{
                //    employeeName = item["EmployeeName"].ToString();
                //    emailAddress = item["Email"].ToString();
                //    voucherNo = item["VoucherNo"].ToString();
                //    string emailBody = "Dear " + employeeName +
                //                  ", <br><br>Workflow hold from " + empName + "<br><br>";

                //    emailBody += " <br><br>Regards, <br><br>Development Team.";
                //    SQLQuery.SendEmail2(emailAddress, "btvstoremanagementsystem@gmail.com", "Workflow Decline For #" + grnVoucherNo, emailBody);

                //}


                RunQuery.SQLQuery.ExecNonQry("Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Decline', TaskStatus='0', IsActive= '0', UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                RunQuery.SQLQuery.ExecNonQry("Update Chiller SET WorkflowStatus='Decline', ReturnOrHoldUserID='" + wUserId + "' WHERE CillerID='" + clId + "'");
                BindWorkFlowUserGridView(clId);
                txtYourRemark.Text = "";
                PermissionToAction();
                ReloadNotification();
            }
            catch (Exception ex)
            {
                Notify("ERROR!: " + ex, "error", lblMsg);
            }
        }
        else
        {
            Notify("You are already decline this workflow", "warning", lblMsg);
        }
    }

}