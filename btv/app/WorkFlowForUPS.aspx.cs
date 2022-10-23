using RunQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkFlowForUPS : System.Web.UI.Page
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
        //string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks, 
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'UPS') ORDER BY WorkFlowUser.Priority";
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
        string query = @"SELECT dbo.UPS.UpsID, dbo.UPS.UPSVoucher, dbo.UPS.MainOfficeId, dbo.Location.Name AS MainOffice, dbo.UPS.Date, dbo.MainOfficeLocation.MainOfficeLocationName AS Location, dbo.UPS.Model, dbo.UPS.I1, dbo.UPS.I2, 
                        dbo.UPS.I3, dbo.UPS.U12, dbo.UPS.U13, dbo.UPS.U14, dbo.UPS.V1, dbo.UPS.V2, dbo.UPS.V3, dbo.UPS.[Load], dbo.UPS.Maintenance, dbo.Employee.Name AS ShiftInCharge, dbo.UPS.Remarks
                        FROM dbo.Location INNER JOIN dbo.UPS ON dbo.Location.LocationID = dbo.UPS.MainOfficeId INNER JOIN dbo.MainOfficeLocation ON dbo.UPS.Location = dbo.MainOfficeLocation.Id INNER JOIN
                        dbo.Employee ON dbo.UPS.ShiftInCharge = dbo.Employee.EmployeeID WHERE UpsID='" + id + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtDate.Text = Convert.ToDateTime(dataReader["Date"]).ToString("dd/MM/yyyy");
            txtUPSNumber.Text = dataReader["UPSVoucher"].ToString();
            txtMainOffice.Text = dataReader["MainOffice"].ToString();
            txtLocation.Text = dataReader["Location"].ToString();
            txtModel.Text = dataReader["Model"].ToString();
            txtI1.Text = dataReader["I1"].ToString();
            txtI2.Text = dataReader["I2"].ToString();
            txtI3.Text = dataReader["I3"].ToString();
            txtU12.Text = dataReader["U12"].ToString();
            txtU13.Text = dataReader["U13"].ToString();
            txtU14.Text = dataReader["U14"].ToString();
            txtV1.Text = dataReader["V1"].ToString();
            txtV2.Text = dataReader["V2"].ToString();
            txtV3.Text = dataReader["V3"].ToString();
            txtLoad.Text = dataReader["Load"].ToString();
            txtMaintenance.Text = dataReader["Maintenance"].ToString();
            txtShiftInCharge.Text = dataReader["ShiftInCharge"].ToString();
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
                    string upsId = "";
                    if (userDetails.Rows.Count > 0)
                    {
                        empID = userDetails.Rows[0]["EmployeeID"].ToString();
                        empName = userDetails.Rows[0]["EmployeeName"].ToString();
                        userPpriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                        upsId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
                    }
                    if (userPpriority > 0)
                    {
                        nextUserPriority = userPpriority + 1;
                    }

                    DataTable nextUserDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                  Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE [Priority]='" + nextUserPriority + "' AND WorkFlowTypeID='" + upsId + "' AND WFU.WorkFlowType='UPS'");
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
                        SQLQuery.ExecNonQry("UPDATE UPS SET CurrentWorkflowUser='" + nextEmpName + "' WHERE UPSId = '" + upsId + "'");
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
                    RunQuery.SQLQuery.ExecNonQry(" Update UPS SET  " + updateQuery + " WHERE UPSId='" + upsId + "'");
                    BindWorkFlowUserGridView(upsId);
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
                string lvId = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
                string status = SQLQuery.ReturnString("SELECT PermissionStatus From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
                if (status != "Hold")
                {
                    DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN
                  DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                  Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE  WorkFlowTypeID='" + lvId + "' AND WorkFlowType='UPS'");
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
                    SQLQuery.ExecNonQry(" Update UPS SET WorkflowStatus='Hold',ReturnOrHoldUserID='" + wUserId + "',WorkflowApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "' WHERE UPSId='" + lvId + "'");
                    BindWorkFlowUserGridView(lvId);
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
                    string lvId = "";
                    string voucherNo = "";
                    if (userDetails.Rows.Count > 0)
                    {
                        empID = userDetails.Rows[0]["EmployeeID"].ToString();
                        empName = userDetails.Rows[0]["EmployeeName"].ToString();
                        userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                        lvId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
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
                    RunQuery.SQLQuery.ExecNonQry(" Update UPS SET WorkflowStatus='Return', ReturnOrHoldUserID='" + wUserId + "' WHERE UPSId='" + lvId + "'");
                    BindWorkFlowUserGridView(lvId);
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
                string lvId = "";
                string voucherNo = "";
                if (userDetails.Rows.Count > 0)
                {
                    empID = userDetails.Rows[0]["EmployeeID"].ToString();
                    empName = userDetails.Rows[0]["EmployeeName"].ToString();
                    userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                    lvId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
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


                RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Decline', TaskStatus='0', IsActive= '0', UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                RunQuery.SQLQuery.ExecNonQry(" Update UPS SET WorkflowStatus='Decline', ReturnOrHoldUserID='" + wUserId + "' WHERE UPSId='" + lvId + "'");
                BindWorkFlowUserGridView(lvId);
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