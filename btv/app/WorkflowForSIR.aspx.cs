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

public partial class app_WorkflowForSIR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string id = Request.QueryString["Id"];
                productGridview.EmptyDataText = "No data added ...";
                productGridview.DataSource = null;
                productGridview.DataBind();
                WorkFlowUserGridView.EmptyDataText = "No data added ...";
                WorkFlowUserGridView.DataSource = null;
                WorkFlowUserGridView.DataBind();

                if (id != null)
                {
                    string userId = EncryptDecrypt.DecryptString(id);
                    string voucherID = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + userId + "'");
                    BindWorkFlowItemsGridView(voucherID);
                    BindWorkFlowUserGridView(voucherID);
                    LoadData(voucherID);
                }

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

    private void BindWorkFlowItemsGridView(string voucherID)
    {
        string query = @"SELECT SIRProduct.SIRProductID, CASE ProductDetails.SerialNo WHEN '0' THEN Product.Name ELSE Product.Name + '-' + ProductDetails.SerialNo END AS ProductName, SIRProduct.QTYDelivered, SIRProduct.QTYNeed, SIRProduct.QTYAvailable, SIRProduct.DeliveredQTYTotalPrice, SIRProduct.UnitPrice, SIRProduct.ProductId FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductId INNER JOIN SIRProduct ON ProductDetails.ProductDetailsID = SIRProduct.ProductDetailsID WHERE IDSirNo = '" + voucherID + "'";
        //string query = @"SELECT SIRProduct.SIRProductID, Product.Name AS ProductName, SIRProduct.QTYDelivered, SIRProduct.QTYNeed,SIRProduct.QTYAvailable,SIRProduct.DeliveredQTYTotalPrice,SIRProduct.UnitPrice FROM SIRProduct INNER JOIN Product ON SIRProduct.ProductId = Product.ProductID WHERE IDSirNo = '" + voucherID + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        productGridview.EmptyDataText = "No data added ...";
        productGridview.DataSource = command.ExecuteReader();
        productGridview.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindWorkFlowUserGridView(string id)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string query = @"SELECT  Employee.Name AS EmployeeName, WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks, WorkFlowUser.Remark,
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'SIR') ORDER BY WorkFlowUser.Priority";
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
        string query = @"SELECT  IDSirNo, DateOfSir, SirVoucherNo, LocationID, FinYear, LoanToEmployee, Store, GivenDivision, GivenDivisionDate, ProductUseAim, HeadOfCost, Remarks, DocumentUrl, PreparedBy, SaveMode, SubmitDate, WorkflowStatus, 
                  ApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, EntryDate, EntryBy FROM SIRFrom WHERE IDSirNo='" + id + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtDateofSIR.Text = Convert.ToDateTime(dataReader["DateOfSir"]).ToString("dd/MM/yyyy");
            txtSirNumber.Text = dataReader["SirVoucherNo"].ToString();
            txtEmployee.Text = SQLQuery.ReturnString("SELECT Name FROM Employee WHERE EmployeeID='" + dataReader["LoanToEmployee"].ToString() + "'");
            txtStore.Text = SQLQuery.ReturnString("SELECT Name FROM Store WHERE StoreAssignID='" + dataReader["Store"].ToString() + "'");
            txtCauseofLoan.Text = dataReader["HeadOfCost"].ToString();
            txtProductPurpose.Text = dataReader["ProductUseAim"].ToString();
            lblRemarks.Text = dataReader["Remarks"].ToString();
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
                    string sirId = "";
                    if (userDetails.Rows.Count > 0)
                    {
                        empID = userDetails.Rows[0]["EmployeeID"].ToString();
                        empName = userDetails.Rows[0]["EmployeeName"].ToString();
                        userPpriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                        sirId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
                    }
                    if (userPpriority > 0)
                    {
                        nextUserPriority = userPpriority + 1;
                    }

                    DataTable nextUserDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                    FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE [Priority]='" + nextUserPriority + "' AND WorkFlowTypeID='" + sirId + "' AND WFU.WorkFlowType='SIR'");
                    string nextempName = "";
                    string nextUserId = "";
                    //string priorty = "0";
                    string nextTypeID = "";
                    string nextEmail = "";
                    string nextVoucherNo = "";
                    string escday = "0";
                    if (nextUserDetails.Rows.Count > 0)
                    {
                        nextUserId = nextUserDetails.Rows[0]["WorkFlowUserID"].ToString();
                        nextempName = nextUserDetails.Rows[0]["EmployeeName"].ToString();
                        nextTypeID = nextUserDetails.Rows[0]["WorkFlowTypeID"].ToString();
                        nextEmail = nextUserDetails.Rows[0]["Email"].ToString();
                        nextVoucherNo = nextUserDetails.Rows[0]["VoucherNo"].ToString();
                        escday = nextUserDetails.Rows[0]["EsclationDay"].ToString();
                        string emailBody = "Dear " + nextempName +
                                      ", <br><br>Approve workflow, check your notification .<br><br>";

                        emailBody += " <br><br>Regards, <br><br>Development Team.";

                        SQLQuery.SendEmail2(nextEmail, "btvstoremanagementsystem@gmail.com", "Workflow for #" + nextVoucherNo, emailBody);
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(escday));
                        SQLQuery.ExecNonQry("Update WorkFlowUser SET IsActive='1', EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "' Where WorkFlowUserID='" + nextUserId + "'");
                        SQLQuery.ExecNonQry("UPDATE SIRFrom SET CurrentWorkflowUser='" + nextempName + "' WHERE IDSirNo = '" + sirId + "'");
                    }
                    string status = "";
                    string updateQuery = "";
                    if (nextUserDetails.Rows.Count == 0)
                    {
                        status = "Approved";
                        updateQuery = "WorkflowStatus='" + status + "', WorkflowApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "',";
                        DataTable dt = SQLQuery.ReturnDataTable("SELECT SIRProductID, CategoryID, SubCategoryID, ProductId,ProductDetailsID, IDSirNo, SIRVoucherNo, ProductDescription, QTYNeed, QTYDelivered, ApprovedQty, QTYAvailable, UnitPrice, DeliveredQTYTotalPrice, EntryBy, DeliveredDate FROM SIRProduct Where IDSirNo='" + sirId + "'");
                        foreach (DataRow item in dt.Rows)
                        {
                            string lvNummber = item["SIRVoucherNo"].ToString();
                            string categoryID = item["CategoryID"].ToString();
                            string subCategoryID = item["SubCategoryID"].ToString();
                            string productID = item["ProductID"].ToString();
                            string productDetailsID = item["ProductDetailsID"].ToString();
                            string locationID = SQLQuery.GetLocationID(item["EntryBy"].ToString());
                            string centerId = SQLQuery.GetCenterId(item["EntryBy"].ToString());
                            string departmentId = SQLQuery.GetDepartmentSectionId(item["EntryBy"].ToString());
                            string storeID = SQLQuery.ReturnString("SELECT Store FROM SIRFrom WHERE IDSirNo='" + sirId + "'");
                            string qTYNeed = item["QTYNeed"].ToString();
                            Accounting.VoucherEntry.StockEntry(sirId, categoryID, subCategoryID, productID, productDetailsID, locationID, centerId, departmentId, storeID, "SIR", "0", "0", "", qTYNeed, lvNummber, qTYNeed, "", item["EntryBy"].ToString(), "1");

                            string productType = SQLQuery.GetProductType(productID);
                            if (productType == "Detail")
                            {
                                SQLQuery.ExecNonQry("UPDATE ProductDetails SET Status='0' WHERE ProductDetailsID='" + productDetailsID + "'");
                            }
                        }

                    }
                    if (userPpriority == 1)
                    {
                        updateQuery += "Issuedby='" + wUserId + "', IssuedDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 2)
                    {
                        updateQuery += "Requisitionby='" + wUserId + "',RequisitionDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 3)
                    {
                        updateQuery += "Documentedby='" + wUserId + "',DocumentedDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 4)
                    {
                        updateQuery += "HeadSectionby='" + wUserId + "',HeadSectionDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 5)
                    {
                        updateQuery += "Approvedby='" + wUserId + "',ApprovedDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 6)
                    {
                        updateQuery += "ValueDeterminedby='" + wUserId + "',ValueDeterminedDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 7)
                    {
                        updateQuery += "StoreKeeperby='" + wUserId + "',StoreKeeperDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "', PermissionStatus= 'Approved',TaskStatus= '0',IsActive ='0',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                    RunQuery.SQLQuery.ExecNonQry(" Update SIRFrom SET  " + updateQuery + " WHERE IDSirNo='" + sirId + "'");
                    BindWorkFlowUserGridView(sirId);
                    txtYourRemark.Text = "";
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
                string sirId = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
                DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                  Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE  WorkFlowTypeID='" + sirId + "' AND WorkFlowType='SIR'");
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


                //DataTable dtEntryBy = SQLQuery.ReturnDataTable(@"SELECT Employee.EmployeeID, Employee.Name, Employee.Email
                //                                    FROM Logins INNER JOIN Employee ON Logins.EmployeeInfoID = Employee.EmployeeID WHERE  (Logins.LoginUserName = '" + entryBy + "')");
                //foreach (DataRow item in dtEntryBy.Rows)
                //{
                //    string name = item["Name"].ToString();
                //    string email = item["Email"].ToString();
                //    string emailBody = "Dear " + name + ", <br><br>Workflow return from " + empName + ", please check your grn.<br><br>";
                //    emailBody += " <br><br>Regards, <br><br>Development Team.";

                //    SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow Return for #" + voucherNo, emailBody);
                //}

                SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "', PermissionStatus= 'Hold',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                SQLQuery.ExecNonQry(" Update SIRFrom SET WorkflowStatus='Hold', ReturnOrHoldUserID='" + wUserId + "', WorkflowApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "' WHERE IDSirNo='" + sirId + "'");
                BindWorkFlowUserGridView(sirId);
                txtYourRemark.Text = "";
                Notify("Work flow hold", "success", lblMsg);
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
            try
            {
                DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName,Employee.EmployeeID, WFU.EntryBy,Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE WorkFlowUserID='" + wUserId + "'");
                string empID = "";
                string empName = "";
                int userPriority = 0;
                string entryBy = "";
                string sirId = "";
                string voucherNo = "";
                if (userDetails.Rows.Count > 0)
                {
                    empID = userDetails.Rows[0]["EmployeeID"].ToString();
                    empName = userDetails.Rows[0]["EmployeeName"].ToString();
                    userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                    sirId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
                    entryBy = userDetails.Rows[0]["EntryBy"].ToString();
                    voucherNo = userDetails.Rows[0]["VoucherNo"].ToString();
                }


                DataTable dtEntryBy = SQLQuery.ReturnDataTable(@"SELECT Employee.EmployeeID, Employee.Name, Employee.Email
                                                    FROM Logins INNER JOIN Employee ON Logins.EmployeeInfoID = Employee.EmployeeID WHERE  (Logins.LoginUserName = '" + entryBy + "')");
                foreach (DataRow item in dtEntryBy.Rows)
                {
                    string name = item["Name"].ToString();
                    string email = item["Email"].ToString();
                    string emailBody = "Dear " + name + ", <br><br>Workflow return from " + empName + ", please check your grn.<br><br>";
                    emailBody += " <br><br>Regards, <br><br>Development Team.";

                    //SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow Return for #" + voucherNo, emailBody);
                }

                RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Return', IsActive= '0', UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                RunQuery.SQLQuery.ExecNonQry(" Update SIRFrom SET WorkflowStatus='Return', ReturnOrHoldUserID='" + wUserId + "' WHERE IDSirNo='" + sirId + "'");
                BindWorkFlowUserGridView(sirId);
                txtYourRemark.Text = "";
            }
            catch (Exception ex)
            {
                Notify("ERROR!: " + ex, "error", lblMsg);
            }
        }
    }
    protected void btnDecline_OnClick(object sender, EventArgs e)
    {
        string id = Request.QueryString["Id"];
        string wUserId = EncryptDecrypt.DecryptString(id);
        try
        {
            DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName,Employee.EmployeeID, WFU.EntryBy,Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE WorkFlowUserID='" + wUserId + "'");
            string empID = "";
            string empName = "";
            int userPriority = 0;
            string entryBy = "";
            string sirId = "";
            string voucherNo = "";
            if (userDetails.Rows.Count > 0)
            {
                empID = userDetails.Rows[0]["EmployeeID"].ToString();
                empName = userDetails.Rows[0]["EmployeeName"].ToString();
                userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                sirId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
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
            RunQuery.SQLQuery.ExecNonQry(" Update SIRFrom SET WorkflowStatus='Decline', ReturnOrHoldUserID='" + wUserId + "' WHERE IDSirNo='" + sirId + "'");
            BindWorkFlowUserGridView(sirId);
            txtYourRemark.Text = "";
        }
        catch (Exception ex)
        {
            Notify("ERROR!: " + ex, "error", lblMsg);
        }
    }

}