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

public partial class app_WorkFlowStatusForChiller : System.Web.UI.Page
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
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'CL') ORDER BY WorkFlowUser.Priority";
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
}