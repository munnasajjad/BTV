using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class app_WorkflowStatusForUPS : System.Web.UI.Page
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

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void BindWorkFlowUserGridView(string id)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks,WorkFlowUser.Remark, 
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID
                  FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'UPS') ORDER BY WorkFlowUser.Priority";
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
}