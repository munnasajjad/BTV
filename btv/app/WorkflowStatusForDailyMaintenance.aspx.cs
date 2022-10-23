using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class app_WorkflowStatusForDailyMaintenance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                string id = Request.QueryString["Id"];
                workInfoGridView.EmptyDataText = "No data added ...";
                workInfoGridView.DataSource = null;
                workInfoGridView.DataBind();
                WorkFlowUserGridView.EmptyDataText = "No data added ...";
                WorkFlowUserGridView.DataSource = null;
                WorkFlowUserGridView.DataBind();

                if (id != null)
                {
                    BindWorkFlowItemsGridView(id);
                    // string userId = EncryptDecrypt.DecryptString(id);
                    //string voucherID = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + id + "'");
                    //string voucherNumber = SQLQuery.ReturnString("SELECT VoucherNo From WorkFlowUser Where WorkFlowUserID='" + userId + "'");

                    BindWorkFlowUserGridView(id);
                    LoadData(id);
                }
                //PermissionToAction();

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
        //string lName = Page.User.Identity.Name.ToString();

        string query = @"SELECT dbo.DailyMaintenanceDetails.DailyMaintenanceDetailsID, dbo.DailyMaintenanceDetails.DailyMaintenanceID, dbo.DailyMaintenanceDetails.WorkType, dbo.DailyMaintenanceDetails.Date, dbo.Employee.Name AS Sign
FROM dbo.DailyMaintenanceDetails INNER JOIN dbo.Employee ON dbo.DailyMaintenanceDetails.Sign = dbo.Employee.EmployeeID WHERE dbo.DailyMaintenanceDetails.DailyMaintenanceID = '" + voucherID + "'";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Connection.Open();
        workInfoGridView.EmptyDataText = "No data added ...";
        workInfoGridView.DataSource = command.ExecuteReader();
        workInfoGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindWorkFlowUserGridView(string id)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks,WorkFlowUser.Remark, 
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID
                  FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'AH') ORDER BY WorkFlowUser.Priority";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void LoadData(string id)
    {
        string query = @"SELECT dbo.DailyMaintenance.DailyMaintenanceID, dbo.DailyMaintenance.DMNVoucher, dbo.DailyMaintenance.MainOfficeId, dbo.Location.Name AS MainOffice, dbo.DailyMaintenance.Date, dbo.DailyMaintenance.NameoftheBrand, 
dbo.DailyMaintenance.CapacityofSplit, dbo.MainOfficeLocation.MainOfficeLocationName AS CoolingArea, dbo.DailyMaintenance.PresentCondition, dbo.Employee.Name AS ShiftInCharge, dbo.DailyMaintenance.Remarks
FROM dbo.DailyMaintenance INNER JOIN dbo.Location ON dbo.DailyMaintenance.MainOfficeId = dbo.Location.LocationID INNER JOIN
dbo.MainOfficeLocation ON dbo.DailyMaintenance.CoolingArea = dbo.MainOfficeLocation.Id INNER JOIN dbo.Employee ON dbo.DailyMaintenance.ShiftInCharge = dbo.Employee.EmployeeID WHERE  (DailyMaintenanceID = '" + id + "')";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtDate.Text = Convert.ToDateTime(dataReader["Date"]).ToString("dd/MM/yyyy");
            txtDMNNo.Text = dataReader["DMNVoucher"].ToString();
            txtNameoftheBrand.Text = dataReader["NameoftheBrand"].ToString();
            txtCapacityofSplit.Text = dataReader["CapacityofSplit"].ToString();
            txtMainOffice.Text = dataReader["MainOffice"].ToString();
            txtCoolingArea.Text = dataReader["CoolingArea"].ToString();
            txtPresentCondition.Text = dataReader["PresentCondition"].ToString();
            txtRemarks.Text = dataReader["Remarks"].ToString();
            txtShiftInCharge.Text = dataReader["ShiftInCharge"].ToString();
        }
    }
}