using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class app_WorkFlow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            string lvId = Request.QueryString["LvId"];
            string lvNo = Request.QueryString["LvNo"];
            //ControlTaskStatus();
            //BindWorkFlowType();
            //BindWorkFlowList();
           
            BindWorkFlowItemsGridView(lvId);
            BindWorkFlowUserGridView(lvId);
            LoadData(lvId);           
        }
           

    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    //private string employeeId = SQLQuery.ReturnString("SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='"+ Page.User.Identity.Name + "'");
    //private void BindWorkFlowList()
    //{
    //    if (ddWorkflowtype.SelectedValue == "LV")
    //    {
    //        SQLQuery.PopulateDropDown(@"SELECT WFU.WorkFlowTypeID, LV.LvInvoiceNo
    //            FROM WorkFlowUser AS WFU INNER JOIN LoanVouchar AS LV ON WFU.WorkFlowTypeID = LV.IDLvNo WHERE EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') AND CONVERT(DATETIME, EsclationEndTime, 111) >='" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "' AND WFU.TaskStatus='1' AND WFU.WorkFlowType='" + ddWorkflowtype.SelectedValue + "'", ddWorkflowlist, "WorkFlowTypeID", "LvInvoiceNo");//AND WFU.UserRemarks==''
    //    }
    //    else if (ddWorkflowtype.SelectedValue == "RV")
    //    {

    //    }

    //}
    //private void BindWorkFlowType()
    //{
    //    SQLQuery.PopulateDropDown("SELECT DISTINCT WorkFlowType FROM WorkFlowUser", ddWorkflowtype, "WorkFlowType", "WorkFlowType");
    //}

    private void BindWorkFlowItemsGridView(string lvNo)
    {
        //string lName = Page.User.Identity.Name.ToString();

        string query = @"SELECT IDLVNo, Product.Name AS ProductName, LVProduct.QTYNeed, LVProduct.QTYDelivered, convert(varchar, LVProduct.DeliveredDate, 103) AS DeliveredDate FROM LVProduct INNER JOIN Product ON LVProduct.ProductID = Product.ProductID WHERE IDLVNo = '" + lvNo + "'";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Connection.Open();
        WorkFlowItemsGridView.EmptyDataText = "No data added ...";
        WorkFlowItemsGridView.DataSource = command.ExecuteReader();
        WorkFlowItemsGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindWorkFlowUserGridView(string lvId)
    {
        string lName = Page.User.Identity.Name.ToString();


        string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";


        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }

   
    private void LoadData(string lvNo)
    {
        string query = @"SELECT DateofLv, ResponsiblePerson, CauseOfLoan, LoanFromCenter, LoanFromStore, SaveToStore, Verifier, RequisitionBy, DeliveredBy FROM LoanVouchar WHERE IDLvNo = '" + lvNo + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {

            txtIssueDate.Text = Convert.ToDateTime(dataReader["DateofLv"]).ToString("dd-MM-yyyy");
            string centerFrom = dataReader["LoanFromCenter"].ToString();
            txtStoreFrom.Text = SQLQuery.ReturnString("Select Name from Center Where  CenterId='" + centerFrom + "'");
            string storeFrom = dataReader["LoanFromStore"].ToString();
            txtStoreReceiveTo.Text = SQLQuery.ReturnString("SELECT Name FROM Store WHERE CenterId='" + centerFrom + "' And StoreId='"+ storeFrom + "'");
            txtResponsiblePerson.Text = dataReader["ResponsiblePerson"].ToString();
            tbxRequirements.Text = dataReader["CauseOfLoan"].ToString();
        }
    }
    //private void ControlTaskStatus()
    //{
    //    bool taskStatus;
    //    DateTime today = DateTime.Now;
    //    string startDate = SQLQuery.ReturnString("SELECT EsclationStartTime FROM WorkFlowUser WHERE WorkFlowTypeID ='" + ddWorkflowlist.SelectedValue + "' AND EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') ");
    //    DateTime firstDay = Convert.ToDateTime(startDate);
    //    string endDate = SQLQuery.ReturnString("SELECT EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID ='" + ddWorkflowlist.SelectedValue + "' AND EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') ");
    //    DateTime lastDay = Convert.ToDateTime(endDate);
    //    if (today <= firstDay && today >= lastDay)
    //    {
    //        taskStatus = true;
    //    }
    //    else
    //    {
    //        taskStatus = false;
    //    }
    //    RunQuery.SQLQuery.ExecNonQry(" Update  WorkFlowUser SET TaskStatus= '" + taskStatus + "' WHERE WorkFlowTypeID='" + ddWorkflowlist.SelectedValue + "' AND EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') ");
    //}



    private void TakeDecision()
    {
        if (txtYourRemark.Text.Trim() != "")
        {
            string permissionStatus = "";
            if (btnApprove.Text == "Approve")
            {
                permissionStatus = "Approved";
            }
            else if (btnDecline.Text == "Decline")
            {
                permissionStatus = "Declined";
            }
            else if (btnHold.Text == "Decline")
            {
                permissionStatus = "Hold";
            }
            else
            {
                permissionStatus = "Hold";
            }
           // RunQuery.SQLQuery.ExecNonQry(" Update  WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now + "', PermissionStatus= '" + permissionStatus + "',TaskStatus= '0',  UserRemarks= '" + txtYourRemark.Text + "' WHERE WorkFlowTypeID='" + ddWorkflowlist.SelectedValue + "' AND EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') ");
        }

        else
        {
            Notify("Your remark feild can't be empty!", "warn", lblMsg);
        }
        
    }

    
    protected void btnApprove_OnClick(object sender, EventArgs e)
    {
        string lvNo = Request.QueryString["lvId"];
        try
        {
            string sqlQuery = @"SELECT TOP (1) WFU.WorkFlowTypeID,WFU.WorkFlowUserID, LV.LvInvoiceNo, WFU.Priority, WFU.EsclationStartTime, WFU.EsclationEndTime, WFU.TaskStatus, WFU.IsActive, Employee_1.Name, Employee_1.Email
                        FROM  WorkFlowUser AS WFU INNER JOIN
                        LoanVouchar AS LV ON WFU.WorkFlowTypeID = LV.IDLvNo INNER JOIN
                        Employee AS Employee_1 ON WFU.EmployeeID = Employee_1.EmployeeID
                        WHERE  (WFU.TaskStatus = '1') AND (WFU.IsActive = '0')
                        ORDER BY WFU.Priority DESC";
            DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);

            foreach (DataRow item in dt.Rows)
            {
                string name = item["Name"].ToString();
                string email = item["Email"].ToString();
                string LvInvoiceNo = item["LvInvoiceNo"].ToString();
                string wfuId= item["WorkFlowUserID"].ToString();
                string emailBody = "Dear " + name +
                              ", <br><br>Approve workflow, check your notification .<br><br>";                               
                
                emailBody +=  " <br><br>Regards, <br><br>Development Team.";

                SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #"+ LvInvoiceNo, emailBody);
                SQLQuery.ExecNonQry("Update WorkFlowUser SET IsActive='1' Where WorkFlowUserID='"+ wfuId + "'");
            }
            //Send Email:

            RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Approved',TaskStatus= '0',IsActive ='0',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowTypeID='" + lvNo + "' AND EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') ");
            RunQuery.SQLQuery.ExecNonQry(" Update LoanVouchar SET ApprovedDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', RequestStatus= '1' WHERE IDLvNo='"+ lvNo + "'");
            BindWorkFlowUserGridView(lvNo);
            txtYourRemark.Text = "";
        }
        catch (Exception ex)
        {

            
        }
         
    }

    protected void btnHold_OnClick(object sender, EventArgs e)
    {
        string lvNo = Request.QueryString["lvId"];
        try
        {
            RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Hold',TaskStatus= '0',IsActive ='0',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowTypeID='" + lvNo + "' AND EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') ");
            BindWorkFlowUserGridView(lvNo);
            txtYourRemark.Text = "";
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    protected void btnDecline_OnClick(object sender, EventArgs e)
    {
        string lvNo = Request.QueryString["lvId"];
        try
        {
            RunQuery.SQLQuery.ExecNonQry(" Update WorkFlowUser SET ApproveDeclineDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "', PermissionStatus= 'Declined',TaskStatus= '0',IsActive ='0',  UserRemarks= N'" + txtYourRemark.Text.Trim() + "' WHERE WorkFlowTypeID='" + lvNo + "' AND EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + Page.User.Identity.Name + "') ");
            BindWorkFlowUserGridView(lvNo);
            txtYourRemark.Text = "";
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}