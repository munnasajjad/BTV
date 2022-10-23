using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkflowForGRN : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                string id = Request.QueryString["Id"];
                productAddGridView.EmptyDataText = "No data added ...";
                productAddGridView.DataSource = null;
                productAddGridView.DataBind();
                WorkFlowUserGridView.EmptyDataText = "No data added ...";
                WorkFlowUserGridView.DataSource = null;
                WorkFlowUserGridView.DataBind();

                if (id != null)
                {
                    string userId = EncryptDecrypt.DecryptString(id);
                    //string userId = id;
                    string voucherID = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + userId + "'");
                    //string voucherNumber = SQLQuery.ReturnString("SELECT VoucherNo From WorkFlowUser Where WorkFlowUserID='" + userId + "'");
                    BindWorkFlowItemsGridView(voucherID);
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
    private void ReloadNotification()
    {
        Repeater Repeater = Page.Master.FindControl("Repeater3") as Repeater;
        Label lblPOrder = Page.Master.FindControl("lblPOrder") as Label;
        SQLQuery.GenerateNotification(Repeater, lblPOrder, User.Identity.Name);
    }
    private void BindWorkFlowItemsGridView(string voucherID)
    {
        //string lName = Page.User.Identity.Name.ToString();

        string query = @"SELECT GRNProduct.GRNProductID, GRNProduct.GRNInvoiceNo, GRNProduct.CountryOfOrigin, GRNProduct.ManufacturingCompany, GRNProduct.Less, GRNProduct.More, GRNProduct.ReceiveProduct AS ReceiveProduct , 
                         GRNProduct.RejectProduct, GRNProduct.PriceLetterNo, GRNProduct.UnitPrice AS UnitPrice, GRNProduct.TotalPrice, GRNProduct.OtherCost, GRNProduct.TotalCost AS TotalCost, GRNProduct.EntryBy, GRNProduct.EntryDate, 
                         Product.Name AS ProductName FROM GRNProduct INNER JOIN
                         Product ON GRNProduct.ProductID = Product.ProductID WHERE GRNProduct.GrnFormID = '" + voucherID + "'";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Connection.Open();
        productAddGridView.EmptyDataText = "No data added ...";
        productAddGridView.DataSource = command.ExecuteReader();
        productAddGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindWorkFlowUserGridView(string id)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks,WorkFlowUser.Remark, 
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID
FROM     WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN

                  WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID
WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'GRN') ORDER BY WorkFlowUser.Priority";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }


    public void LoadData(string id)
    {
        string query = @"SELECT IDGrnNO, GRNInvoiceNo, DateOfGRN, StoreID, ReferenceID, Supplier, InvoiceNo, DateofInvoiceNo, PurchaseOrderNo, DateofPurchaseOrderNo, ProductSHReceiveDate, TotalAmount, Remarks, PreparedBy, PreparedDate, 
                  ProductInspectorApprovedBy, ApprovedDate, StoreDivLedgerWritter, AccountDivLedgerWritter, AccountDivLedgerDate, SaveMode, WorkflowStatus, EntryDate, EntryBy FROM     GRNFrom WHERE  (IDGrnNO = '" + id + "')";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtGrnDate.Text = Convert.ToDateTime(dataReader["DateOfGRN"]).ToString("dd/MM/yyyy");
            txtGrnNo.Text = dataReader["GRNInvoiceNo"].ToString();
            txtInvoiceNo.Text = dataReader["InvoiceNo"].ToString();
            txtReference.Text = SQLQuery.ReturnString("SELECT Name FROM Reference WHERE ReferenceID='" + dataReader["ReferenceID"].ToString() + "'");
            txtSupplier.Text = dataReader["Supplier"].ToString();
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
                    string grnId = "";
                    if (userDetails.Rows.Count > 0)
                    {
                        empID = userDetails.Rows[0]["EmployeeID"].ToString();
                        empName = userDetails.Rows[0]["EmployeeName"].ToString();
                        userPpriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                        grnId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
                    }
                    if (userPpriority > 0)
                    {
                        nextUserPriority = userPpriority + 1;
                    }

                    DataTable nextUserDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN
                  DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                  Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE [Priority]='" + nextUserPriority + "' AND WorkFlowTypeID='" + grnId + "' AND WFU.WorkFlowType='GRN'");
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
                        SQLQuery.ExecNonQry("UPDATE GRNFrom SET CurrentWorkflowUser='" + nextempName + "' WHERE IDGrnNO = '" + grnId + "'");
                    }
                    string status = "";
                    string updateQuery = "";
                    if (nextUserDetails.Rows.Count == 0)
                    {
                        status = "Approved";
                        updateQuery = "WorkflowStatus='" + status + "', WorkflowApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "',";
                        DataTable dt = SQLQuery.ReturnDataTable("SELECT GRNProductID, GRNInvoiceNo, CategoryID, SubCategoryID, ProductID, ReceiveProduct, EntryBy FROM GRNProduct Where GrnFormID='" + grnId + "'");
                        foreach (DataRow item in dt.Rows)
                        {
                            string grnNo = item["GRNInvoiceNo"].ToString();
                            string categoryID = item["CategoryID"].ToString();
                            string subCategoryID = item["SubCategoryID"].ToString();
                            string productID = item["ProductID"].ToString();
                            string locationID = SQLQuery.GetLocationID(item["EntryBy"].ToString());
                            string centerId = SQLQuery.GetCenterId(item["EntryBy"].ToString());
                            string departmentId = SQLQuery.GetDepartmentSectionId(item["EntryBy"].ToString());
                            string storeID = SQLQuery.ReturnString("SELECT StoreID FROM GRNFrom WHERE IDGrnNO='" + grnId + "'");
                            string receiveProduct = item["ReceiveProduct"].ToString();
                            Accounting.VoucherEntry.StockEntry(grnId, categoryID, subCategoryID, productID, "0", locationID, centerId, departmentId, storeID, "GRN", "0", receiveProduct, grnNo, receiveProduct, "", "0", "", item["EntryBy"].ToString(), "0");
                            SQLQuery.ExecNonQry("UPDATE ProductDetails SET Status='1' WHERE ProductID='" + productID + "'");

                            //Non-Detail Product Insert Into ProductDetails Table (for a single time)
                            string productType = SQLQuery.ReturnString(@"SELECT ProductType FROM Product WHERE (ProductID = '" + productID + "')");
                            if (productType == "2")
                            {
                                string productExist = SQLQuery.ReturnString(@"SELECT COUNT(ProductID) FROM ProductDetails WHERE ProductID='" + productID + "'");
                                if (int.Parse(productExist) == 0)
                                {
                                    Accounting.VoucherEntry.ProductDetailsEntry(grnId, productID, "0", "0", "0", "0", "0", storeID, item["EntryBy"].ToString());
                                }

                            }

                        }

                    }
                    if (userPpriority == 1)
                    {
                        updateQuery += "StoreDivLedgerWritter='" + wUserId + "', StoreDivLedgerWritterDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 2)
                    {
                        updateQuery += "ProductInspectorApprovedBy='" + wUserId + "',ApprovedDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }
                    else if (userPpriority == 3)
                    {
                        updateQuery += "AccountDivLedgerWritter='" + wUserId + "',AccountDivLedgerDate='" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "'";
                    }

                    RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "', PermissionStatus= 'Approved',TaskStatus= '0',IsActive ='0',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                    RunQuery.SQLQuery.ExecNonQry(" Update GRNFrom SET  " + updateQuery + " WHERE IDGrnNO='" + grnId + "'");
                    BindWorkFlowUserGridView(grnId);
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
    private void PermissionToAction()
    {
        string id = Request.QueryString["Id"];
        string wUserId = EncryptDecrypt.DecryptString(id);
        //string wUserId = id;
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
            btnApprove.Enabled = false;
            btnHold.Enabled = false;
            btnReturn.Enabled = false;
            btnDecline.Enabled = false;
        }
        else if (status == "Hold")
        {
            btnHold.Enabled = false;
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
            btnDecline.Enabled = false;

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
                string grnId = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
                string status = SQLQuery.ReturnString("SELECT PermissionStatus From WorkFlowUser Where WorkFlowUserID='" + wUserId + "'");
                if (status != "Hold")
                {
                    DataTable userDetails = SQLQuery.ReturnDataTable(@"SELECT  DWE.EmployeeID, WFU.WorkFlowUserID,WFU.EsclationDay,WFU.VoucherNo,WFU.WorkFlowTypeID, Employee.Name AS EmployeeName, Employee.Email, WFU.Priority, WFU.WorkFlowTypeID
                  FROM WorkFlowUser AS WFU INNER JOIN
                  DesignationWithEmployee AS DWE ON WFU.EmployeeID = DWE.Id INNER JOIN
                  Employee ON DWE.EmployeeID = Employee.EmployeeID WHERE  WorkFlowTypeID='" + grnId + "' AND WorkFlowType='GRN'");
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
                    SQLQuery.ExecNonQry(" Update GRNFrom SET WorkflowStatus='Hold', ReturnOrHoldUserID='" + wUserId + "', WorkflowApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "' WHERE IDGrnNO='" + grnId + "'");
                    BindWorkFlowUserGridView(grnId);
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
                    string grnId = "";
                    string voucherNo = "";
                    if (userDetails.Rows.Count > 0)
                    {
                        empID = userDetails.Rows[0]["EmployeeID"].ToString();
                        empName = userDetails.Rows[0]["EmployeeName"].ToString();
                        userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                        grnId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
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

                        SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow Return for #" + voucherNo, emailBody);
                    }

                    RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Return', IsActive= '0', UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowUserID='" + wUserId + "'");
                    RunQuery.SQLQuery.ExecNonQry(" Update GRNFrom SET WorkflowStatus='Return', ReturnOrHoldUserID='" + wUserId + "' WHERE IDGrnNO='" + grnId + "'");
                    BindWorkFlowUserGridView(grnId);
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
                string grnId = "";
                string voucherNo = "";
                if (userDetails.Rows.Count > 0)
                {
                    empID = userDetails.Rows[0]["EmployeeID"].ToString();
                    empName = userDetails.Rows[0]["EmployeeName"].ToString();
                    userPriority = int.Parse(userDetails.Rows[0]["Priority"].ToString());
                    grnId = userDetails.Rows[0]["WorkFlowTypeID"].ToString();
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
                RunQuery.SQLQuery.ExecNonQry(" Update GRNFrom SET WorkflowStatus='Decline', ReturnOrHoldUserID='" + wUserId + "' WHERE IDGrnNO='" + grnId + "'");
                BindWorkFlowUserGridView(grnId);
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
    decimal total = 0;
    protected void productAddGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalCost"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[4].Text = "Total:";
            e.Row.Cells[5].Text = Convert.ToString(total);
            total = 0;
        }
    }
}