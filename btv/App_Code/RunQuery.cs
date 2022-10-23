//using ReportDocument = CrystalDecisions.CrystalReports.Engine.ReportDocument;
using Newtonsoft.Json;
//using CrystalDecisions.ReportAppServer.ReportDefModel;

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for RunQuery
/// </summary>
///

namespace RunQuery
{
    public class SQLQuery
    {
        public static string HostURI()
        {
            return ConfigurationManager.AppSettings["HostURI"];
        }
        public static void Empty2Zero(TextBox textBox)
        {
            if (textBox.Text == "")
            {
                textBox.Text = "0";
            }
        }
        public static void GenerateNotification(Repeater repeater, Label label, string uName)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("VoucherNo", typeof(string));
            dt.Columns.Add("WorkFlowUserID", typeof(string));
            dt.Columns.Add("Url", typeof(string));

            string fromName = "";
            #region DataTable for LV voucher
            DataTable dtLV = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                                            WorkFlowUser.WorkFlowTypeID,LoanVouchar.SubmitDate, LoanVouchar.LvInvoiceNo, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name
                                            FROM WorkFlowUser INNER JOIN LoanVouchar ON WorkFlowUser.WorkFlowTypeID = LoanVouchar.IDLvNo INNER JOIN
                                            DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'LV') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");

            foreach (DataRow item in dtLV.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());

                fromName = "WorkFlowForLV";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            #region DataTable for SIR voucher
            DataTable dtSIR = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                                            WorkFlowUser.WorkFlowTypeID,SIRFrom.SubmitDate, SIRFrom.SirVoucherNo, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name
                                            FROM WorkFlowUser INNER JOIN SIRFrom ON WorkFlowUser.WorkFlowTypeID = SIRFrom.IDSirNo INNER JOIN
                                            DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'SIR') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtSIR.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());

                fromName = "WorkflowForSIR";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            #region DataTable for RV voucher
            DataTable dtRV = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                                            WorkFlowUser.WorkFlowTypeID,ReturnVauchar.SubmitDate, ReturnVauchar.RvInvoiceNo, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name
                                            FROM WorkFlowUser INNER JOIN ReturnVauchar ON WorkFlowUser.WorkFlowTypeID = ReturnVauchar.IDRvNo INNER JOIN
                                            DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'RV') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtRV.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());

                fromName = "WorkflowForRV";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            #region DataTable for GRN voucher
            DataTable dtGrn = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                                            WorkFlowUser.WorkFlowTypeID,GRNFrom.SubmitDate, GRNFrom.GRNInvoiceNo, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name
                                            FROM WorkFlowUser INNER JOIN GRNFrom ON WorkFlowUser.WorkFlowTypeID = GRNFrom.IDGrnNO INNER JOIN
                                            DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')  OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'GRN') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtGrn.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());

                fromName = "WorkFlowForGrn";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
                
            }
            #endregion
            #region DataTable for TV voucher

            DataTable dtTV = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, TransferVoucher.SubmitDate, TransferVoucher.TransferVoucherNo, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name
                         FROM WorkFlowUser INNER JOIN
                         TransferVoucher ON WorkFlowUser.WorkFlowTypeID = TransferVoucher.TvID INNER JOIN
                         DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'TV') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtTV.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());

                fromName = "WorkFlowFortv";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion

            #region DataTable for ES voucher
            DataTable dtES = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, EarthStation.SubmitDate, EarthStation.ESVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN EarthStation ON WorkFlowUser.WorkFlowTypeID = EarthStation.EarthStationId INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'ES') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtES.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkflowForEarthStation";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion

            #region DataTable for MR voucher
            DataTable dtMR = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, MeterReading.SubmitDate, MeterReading.MRVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN MeterReading ON WorkFlowUser.WorkFlowTypeID = MeterReading.MeterReadingId INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'MR') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtMR.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForMeterReading";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion

            #region DataTable for DM voucher
            DataTable dtDM = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, Dehumidifier.SubmitDate, Dehumidifier.DMVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN Dehumidifier ON WorkFlowUser.WorkFlowTypeID = Dehumidifier.DehumidifierId INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'DM') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtDM.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForDehumidifier";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for MN voucher
            DataTable dtMN = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, Maintenance.SubmitDate, Maintenance.MNVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN Maintenance ON WorkFlowUser.WorkFlowTypeID = Maintenance.MaintenanceId INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'MN') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtMN.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowStatusForMaintenance";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for LB voucher
            DataTable dtLB = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, LiveBroadcasting.SubmitDate, LiveBroadcasting.LBVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN LiveBroadcasting ON WorkFlowUser.WorkFlowTypeID = LiveBroadcasting.LiveBroadcastingId INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'LB') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtLB.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForLiveBroadcasting";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for UPS voucher
            DataTable dtUPS = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, UPS.SubmitDate, UPS.UPSVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN UPS ON WorkFlowUser.WorkFlowTypeID = UPS.UpsID INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'UPS') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtUPS.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForUPS";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for ACF voucher
            DataTable dtACF = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, AirConditionFCU.SubmitDate, AirConditionFCU.ACFVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN AirConditionFCU ON WorkFlowUser.WorkFlowTypeID = AirConditionFCU.AirConditionFcuId INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'ACF') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtACF.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForAirConditionFCU";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for CL voucher
            DataTable dtCL = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, Chiller.SubmitDate, Chiller.CLVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN Chiller ON WorkFlowUser.WorkFlowTypeID = Chiller.CillerID INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'CL') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtCL.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForChiller";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for SS voucher
            DataTable dtSS = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, SubStation.SubmitDate, SubStation.SSVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN SubStation ON WorkFlowUser.WorkFlowTypeID = SubStation.SubStationID INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'SS') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtSS.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForSubStation";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for AH voucher
            DataTable dtAH = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, AirHandlingUnit.SubmitDate, AirHandlingUnit.AHVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN AirHandlingUnit ON WorkFlowUser.WorkFlowTypeID = AirHandlingUnit.AirHandlingUnitID INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'AH') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtAH.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForAirHandlingUnit";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion
            
            #region DataTable for DMN voucher
            DataTable dtDMN = SQLQuery.ReturnDataTable(@"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.WorkFlowType, WorkFlowUser.VoucherNo, WorkFlowUser.EsclationStartTime, WorkFlowUser.EsclationEndTime, WorkFlowUser.TaskStatus, WorkFlowUser.IsActive, 
                         WorkFlowUser.WorkFlowTypeID, DailyMaintenance.SubmitDate, DailyMaintenance.DMNVoucher, WorkFlowUser.EsclationDay, WorkFlowUser.Priority, Employee.EmployeeID, Employee.Name FROM WorkFlowUser INNER JOIN DailyMaintenance ON WorkFlowUser.WorkFlowTypeID = DailyMaintenance.DailyMaintenanceID INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'DMN') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + uName + "')))");


            foreach (DataRow item in dtDMN.Rows)
            {
                string userId = EncryptDecrypt.EncryptString(item["WorkFlowUserID"].ToString());
                fromName = "WorkFlowForDailyMaintenance";
                dr = dt.NewRow();
                dr["VoucherNo"] = item["VoucherNo"] + ", " + Convert.ToDateTime(item["SubmitDate"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                dr["WorkFlowUserID"] = item["WorkFlowUserID"];
                dr["Url"] = fromName + "?Id=" + userId;
                dt.Rows.Add(dr);
            }
            #endregion

            label.Text = dt.Rows.Count.ToString();

            repeater.DataSource = dt;
            repeater.DataBind();

        }
        public static void IsUserActive(string lName)
        {
            string isActive = SQLQuery.ReturnString("SELECT IsActive FROM Users WHERE Username = '" + lName + "'");
            if (isActive == "False")
            {
                int loginCount = int.Parse(SQLQuery.ReturnString("SELECT LoginCount FROM Users WHERE Username = '" + lName + "'"));
                if (loginCount > 3)
                {
                    ResponseHelper.Redirect("Profile.aspx","","");
                }
            }
        }
        public static void DeleteFile(string path)
        {
            try
            {
                if (path != null || path != string.Empty)
                {
                    if ((File.Exists(path)))
                    {
                        File.Delete(path);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public static string SmsGatway(string recieverNumber, string templateName)
        {
            try
            {
                string smsBody = templateName;
                string reqText1 = "https://license.extreme.com.bd/api/sms?user=munna&pswd=123456&rcvr=" + recieverNumber + "&msg=" + smsBody;
                reqText1 = Convert.ToString(WebRequest.Create(reqText1).GetResponse());
                return "success";
            }
            catch (Exception e)
            {
                return "failed";
            }

        }


        public static string GetActualImage(string path)
        {
            var actualServerPath = HttpContext.Current.Server.MapPath(path);
            return actualServerPath = File.Exists(actualServerPath) ? path : "~/Images/no-image.jpg";
        }

       
       
        public static string Unicode2Eng(string banglaNumber)
        {
            string englishDigits = ".0123456789";
            string banglaDigits = ".\u09E6\u09E7\u09E8\u09E9\u09EA\u09EB\u09EC\u09ED\u09EE\u09EF\u09F0";
            return Translate(banglaNumber, banglaDigits, englishDigits);
        }
        public static string Eng2Unicode(string engNumber)
        {
            string englishDigits = ".0123456789";
            string banglaDigits = ".\u09E6\u09E7\u09E8\u09E9\u09EA\u09EB\u09EC\u09ED\u09EE\u09EF\u09F0";

            return Translate(engNumber, englishDigits, banglaDigits);

        }
        static string Translate(string source, string fromStr, string toStr)
        {
            string result = "";
            foreach (var sourceChar in source)
            {
                if (sourceChar == '-' || sourceChar == '/' || sourceChar == '.' || sourceChar == ' ')
                {
                    result += sourceChar;
                }
                else
                {
                    int pos = fromStr.IndexOf(sourceChar);
                    if ((pos >= 0) && (pos < toStr.Length))
                    {
                        result += toStr[pos];
                    }
                }
            }

            return result;

        }
        public static string ReturnString(string query)
        {
            try
            {
                string result = "";
                SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Connection.Open();
                result = Convert.ToString(cmd.ExecuteScalar());
                cmd.Connection.Close();
                cmd.Connection.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(query + ".<br/> ERROR: " + ex.ToString());
                return "";// query + ".<br/><br/> ERROR: " + ex.ToString();
            }
        }
        public static void Notify(Page page, string msg, string type, Label lblNotify)
        {
            //ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
            //Types: success, info, warn, error
            lblNotify.Attributes.Add("class", "xerp_" + type);
            lblNotify.Text = msg;
        }
        public static int ExecNonQry(string query)
        {
            SqlCommand cmd7 = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd7.Connection.Open();
            int affectedRow = cmd7.ExecuteNonQuery();
            cmd7.Connection.Close();
            cmd7.Connection.Dispose();
            return affectedRow;

        }
        public static string ProjectID(string userId)
        {
            return "1";// ReturnString("SELECT ProjectID FROM Employee WHERE LoginID ='" + userId + "'");
        }
        public static string GetDesignationWithEmployeeID(string userId)
        {
            return ReturnString(@"SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + userId + "'");
        }
        public static string GetEmployeeID(string userId)
        {
            return ReturnString(@"SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + userId + "'");
        }
        public static string GetLocationID(string userId)
        {
            return ReturnString(@"SELECT LocationID FROM Employee WHERE EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + userId + "' )");
        }
        public static string GetLocationIdBySirNo(string idSirNo)
        {
            return ReturnString(@"SELECT LocationID FROM Employee WHERE (EmployeeID=(SELECT LoanToEmployee FROM SIRFrom WHERE IDSirNo='" + idSirNo + "'))");
        }
        public static string GetLocationName(string locationID)
        {
            return ReturnString(@"SELECT Name FROM Location WHERE  LocationID='"+locationID+"'");
        }

        public static string GetLocationNameByEmpID(string employeId)
        {
            return ReturnString(@"SELECT Name FROM Location WHERE  LocationID = (SELECT LocationID FROM Employee WHERE EmployeeID='" + employeId+"')");
        }
        public static string GetProductIDByDetailsID(string detailsId)
        {
            return ReturnString(@"SELECT ProductID FROM ProductDetails WHERE ProductDetailsID='" + detailsId + "'");
        }
        public static string GetCenterId(string userId)
        {
            return ReturnString(@"SELECT CenterID FROM Employee WHERE EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + userId + "' )");
        }
        public static string GetCenterName(string centerID)
        {
            return ReturnString(@"SELECT Name FROM Center WHERE  CenterID='" + centerID + "'");
        }

        public static string GetDepartmentSectionId(string userId)
        {
            return ReturnString(@"SELECT DepartmentSectionID FROM Employee WHERE EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + userId + "' )");
        }
        public static string GetDepartmentNameById(string dptId)
        {
            return ReturnString(@"SELECT Name FROM DepartmentSection WHERE  (DepartmentSectionID = '"+dptId+ "')");
        }
        public static string GetStoreAdminId(string userId)
        {
            return ReturnString(@"SELECT DepartmentSectionID FROM Employee WHERE EmployeeID=(SELECT EmployeeInfoID FROM Logins WHERE LoginUserName='" + userId + "' )");
        }

        public static void UpdateProductStatus(string status, string productDetailsId)
        {
            ExecNonQry("Update ProductDetails SET ProductStatus='" + status + "' WHERE ProductDetailsID='" +
                       productDetailsId + "'");
        } 
        public static string GetProductType(string productId)
        {
            return ReturnString(@"SELECT ProductType.Type FROM Product INNER JOIN ProductType ON Product.ProductType = ProductType.TypeId WHERE  (Product.ProductID = '"+productId+"')");
        }
        public static int GetAvailableQty(string storeId,string productId, int sumQty=0)
        {
            sumQty = int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(SUM(QTYNeed),0) AS Qty FROM SIRProduct WHERE  (ProductId = '" + productId+"') AND (StoreID = '"+storeId+"')"));
            sumQty += int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(SUM(ReturnQTY),0) AS Qty FROM LVProduct WHERE StoreID = '" + storeId + "' AND ProductID = '"+productId+"'"));
            sumQty += int.Parse(SQLQuery.ReturnString(@"SELECT  ISNULL(SUM(TvQty),0) AS Qty FROM TvProduct WHERE StoreID='" + storeId + "' AND ProductID='" + productId + "'"));
            sumQty += int.Parse(SQLQuery.ReturnString(@"SELECT  ISNULL(SUM(Qty),0) AS Qty FROM ProductRepairDetails WHERE StoreID='" + storeId + "' AND ProductID='" + productId + "'"));
            sumQty += int.Parse(SQLQuery.ReturnString(@"SELECT  ISNULL(SUM(Qty),0) AS Qty FROM AuctionDetails WHERE StoreID='" + storeId + "' AND ProductID='" + productId + "'"));
            sumQty += int.Parse(SQLQuery.ReturnString(@"SELECT  ISNULL(SUM(Qty),0) AS Qty FROM DeadProductDetails WHERE StoreID='" + storeId + "' AND ProductID='" + productId + "'"));
            int allStock = int.Parse(ReturnString(@"SELECT ISNULL(SUM(StockIn), 0) AS Available FROM StockRegister WHERE  (StoreID = '" + storeId + "') AND (ProductID = '" + productId + "')"));
            return allStock - sumQty;
        }
        public static bool IsAvailableProduct(string productDetailsId, string storeId,out string productStatus)
        {
            bool status = true;
            productStatus = "";
            string sirProduct = SQLQuery.ReturnString(@"SELECT  ProductDetails.ProductStatus FROM SIRProduct INNER JOIN ProductDetails ON SIRProduct.ProductDetailsID = ProductDetails.ProductDetailsID  WHERE (SIRProduct.ProductDetailsID = '" + productDetailsId + "') AND SIRProduct.StoreID='" + storeId + "'");
            if (sirProduct == "SIR")
            {
                productStatus = sirProduct;
                status = false;
            }
            string tvProduct = SQLQuery.ReturnString(@"SELECT ProductDetails.ProductStatus FROM TvProduct INNER JOIN ProductDetails ON TvProduct.ProductDetailsID = ProductDetails.ProductDetailsID  WHERE (TvProduct.ProductDetailsID = '" + productDetailsId + "') AND TvProduct.StoreID='" + storeId + "'");
            if (tvProduct == "TV")
            {
                productStatus = tvProduct;
                status = false;
            }
            string lVProduct = SQLQuery.ReturnString(@"SELECT ProductDetails.ProductStatus FROM lVProduct INNER JOIN ProductDetails ON lVProduct.ProductDetailsID = ProductDetails.ProductDetailsID  WHERE (lVProduct.ProductDetailsID = '" + productDetailsId + "') AND lVProduct.StoreID='" + storeId + "'");
            if (lVProduct == "LV")
            {
                productStatus = lVProduct;
                status = false;
            }
            string pRProduct = SQLQuery.ReturnString(@"SELECT ProductDetails.ProductStatus FROM ProductRepairDetails INNER JOIN ProductDetails ON ProductRepairDetails.ProductDetailsID = ProductDetails.ProductDetailsID  WHERE (ProductRepairDetails.ProductDetailsID = '" + productDetailsId + "') AND ProductRepairDetails.StoreID='" + storeId + "'");
            if (pRProduct == "PR")
            {
                productStatus = "Product Repair";
                status = false;
            }
            string alProduct = SQLQuery.ReturnString(@"SELECT ProductDetails.ProductStatus FROM AuctionDetails INNER JOIN ProductDetails ON AuctionDetails.ProductDetailsID = ProductDetails.ProductDetailsID  WHERE (AuctionDetails.ProductDetailsID = '" + productDetailsId + "') AND AuctionDetails.StoreID='" + storeId + "'");
            if (alProduct == "Auction")
            {
                productStatus = "Auction List";
                status = false;
            }
            string deadProduct = SQLQuery.ReturnString(@"SELECT ProductDetails.ProductStatus FROM DeadProductDetails INNER JOIN ProductDetails ON DeadProductDetails.ProductDetailsID = ProductDetails.ProductDetailsID  WHERE (DeadProductDetails.ProductDetailsID = '" + productDetailsId + "') AND DeadProductDetails.StoreID='" + storeId + "'");
            if (deadProduct == "Dead")
            {
                productStatus = "Dead Product List";
                status = false;
            }
            return status;
        }
        public static string ContactHead(string pid)
        {
            return ReturnString("Select Name from ExpenseTypes WHERE SystemTypeID='4' AND ProjectId='" + pid + "'");
        }
        //Job/ Contract/ Project/ Vehicle Based:
        public static string ShowContracts(string pid)
        {
            return ReturnString("Select IsContracts FROM TargetIndustries WHERE Id =(Select Type from Projects where VID='" + pid + "')");
        }
        public static string FindCaption(string name, string prjId)
        {
            string caption = ReturnString("SELECT " + name + " FROM tblLabels WHERE PrjctID ='" + prjId + "'");
            if (caption == "")
            {
                caption = name;
            }
            return caption;
        }
        public static string PhotoURL(string photoId)
        {
            return ReturnString("SELECT PhotoURL FROM Photos WHERE PhotoID ='" + photoId + "'");
        }

        public static string RandomNumber(int length)
        {
            Random _random = new Random();
            int i = 0;
            while (i.ToString().Length < length)
            {
                i += _random.Next();
            }
            if (i.ToString().Length > length)
            {
                i = Convert.ToInt32(i.ToString().Substring(0, length));
            }

            return i.ToString();
        }

        public static string OparatePermission(string userName, string operationName)
        {
            //string EmpId = ReturnString("Select sl from Employee where LoginID='" + userName + "'");
            string RoleId = ReturnString(@"SELECT Logins.UserLevel FROM Employee INNER JOIN Logins ON Employee.EmployeeID = Logins.EmployeeInfoID WHERE (Logins.LoginUserName='" + userName + "')");
            string optName = "";
            if (operationName == "Insert")
            {
                optName = "CanInsert";
            }
            else if (operationName == "Delete")
            {
                optName = "CanDelete";
            }
            else if (operationName == "Update")
            {
                optName = "CanUpdate";
            }
            string isPermitted = ReturnString("SELECT " + optName + " FROM UserOparateLevel WHERE EmpID='" + RoleId + "'");
            if (userName == "rony" || userName == "btv")
            {
                isPermitted = "1";
            }
            return isPermitted;
        }

        public static string EmpID(string userId)
        {
            return ReturnString("SELECT sl FROM Employee WHERE LoginID ='" + userId + "'");
        }
        public static int AuthLevel(string userId)
        {
            if (userId.ToLower() == "rony")
            {
                return 0;
            }
            else if (userId != "")
            {
                string isAuthorized = ReturnString("Select LevelId FROM  Users WHERE Username='" + userId + "'");
                return Convert.ToInt32(isAuthorized);
            }
            else
            {
                return 10;
            }
        }
        public static int CanSave(string userId)
        {
            string isAuthorized = SQLQuery.ReturnString("Select CanInsert FROM UserLevel WHERE LevelID='" + AuthLevel(userId) + "'");
            return Convert.ToInt32(isAuthorized);
        }
        public static int CanEdit(string userId)
        {
            string isAuthorized = SQLQuery.ReturnString("Select CanUpdate FROM UserLevel WHERE LevelID='" + AuthLevel(userId) + "'");
            return Convert.ToInt32(isAuthorized);
        }
        public static int CanDelete(string userId)
        {
            string isAuthorized = SQLQuery.ReturnString("Select CanDelete FROM UserLevel WHERE LevelID='" + AuthLevel(userId) + "'");
            return Convert.ToInt32(isAuthorized);
        }

        public static string SearchMethod(string userId)
        {
            return ReturnString("Select SearchMethod from Logins where LoginUserName='" + userId + "'");
        }

        public static void PopulateGridView(GridView ItemGrid, string query)
        {
            SqlCommand cmd = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            ItemGrid.EmptyDataText = "No items to view...";
            ItemGrid.DataSource = cmd.ExecuteReader();
            ItemGrid.DataBind();
            cmd.Connection.Close();

        }
        //public static string Make2Digits(string InvNo)
        //{
        //    if (InvNo.Length < 2)
        //    {
        //        InvNo = "0" + InvNo;
        //    }
        //    return InvNo;
        //}

        public static void PopulateDropDown(string query, DropDownList ddGenerate, string value, string text)
        {
            ddGenerate.Items.Clear();
            ListItem lst = new ListItem("---Select---", "0");
            ddGenerate.Items.Insert(ddGenerate.Items.Count, lst);
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader grouplist = cmd.ExecuteReader();
            ddGenerate.DataSource = grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        public static void PopulateDropDownWithoutSelect(string query, DropDownList ddGenerate, string value, string text)
        {
            ddGenerate.Items.Clear();
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader grouplist = cmd.ExecuteReader();
            ddGenerate.DataSource = grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        public static void PopulateDropDownWithAll(string query, DropDownList ddGenerate, string value, string text)
        {
            ddGenerate.Items.Clear();
            ListItem lst = new ListItem("--- all ---", "0");
            ddGenerate.Items.Insert(ddGenerate.Items.Count, lst);
            SqlCommand cmd = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Grouplist = cmd.ExecuteReader();
            ddGenerate.DataSource = Grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        public static void PopulateListView(string query, ListBox ddGenerate, string value, string text)
        {
            SqlCommand cmd = new SqlCommand(query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            SqlDataReader Grouplist = cmd.ExecuteReader();
            ddGenerate.DataSource = Grouplist;
            ddGenerate.DataValueField = value;
            ddGenerate.DataTextField = text;
            ddGenerate.DataBind();

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }

        //Convert Gridview to datatable
        //DataTable dt = GetDataTable(GridView1);
        public static DataTable DataTableFromGrid(GridView dtg)
        {
            DataTable dt = new DataTable();
            if (dtg.HeaderRow != null)
            {
                for (int i = 0; i < dtg.HeaderRow.Cells.Count; i++)
                {
                    dt.Columns.Add(dtg.HeaderRow.Cells[i].Text);
                }
            }
            foreach (GridViewRow row in dtg.Rows)
            {
                DataRow dr;
                dr = dt.NewRow();

                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dr[i] = row.Cells[i].Text.Replace(" ", "");
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //Sql query to dataset
        public static DataSet ReturnDataSet(String Query)
        {
            DataSet ds = new DataSet();
            String connStr = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand objCommand = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(objCommand);
                da.Fill(ds);
                da.Dispose();
            }
            return ds;
        }

        public static DataTable ReturnDataTable(String Query)
        {
            SqlCommand cmd2y = new SqlCommand(Query,
                new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2y.Connection.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd2y);
            DataSet ds = new DataSet("Board");
            da.Fill(ds, "Board");
            cmd2y.Connection.Close();

            DataTable citydt = ds.Tables["Board"];
            return citydt;
        }

        public static string GenerateInvoiceNo()
        {
            string yr = DateTime.Now.Year.ToString();
            DateTime countForYear = Convert.ToDateTime("01/01/" + yr + " 00:00:00");

            SqlCommand cmd =
                new SqlCommand(
                    "Select CONVERT(varchar, (ISNULL (COUNT(SalesId),0)+ 1 )) from SalesEntry where EntryDate>=@EntryDate",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.AddWithValue("@EntryDate", countForYear);
            cmd.Connection.Open();
            int counter = Convert.ToInt32(cmd.ExecuteScalar());

            cmd.Connection.Close();
            cmd.Connection.Dispose();

            string InvNo = Make2Digits(DateTime.Now.Day.ToString()) + Make2Digits(DateTime.Now.Month.ToString()) +
                           yr.Substring(2, 2) + "-" + Make2Digits(counter.ToString());
            string isExist = RunQuery.SQLQuery.ReturnString("Select InvNo from SalesEntry where InvNo='" + InvNo + "'");
            while (isExist != "")
            {
                counter++;
                InvNo = Make2Digits(DateTime.Now.Day.ToString()) + Make2Digits(DateTime.Now.Month.ToString()) +
                        yr.Substring(2, 2) + "-" + Make2Digits(counter.ToString());
                isExist = RunQuery.SQLQuery.ReturnString("Select InvNo from SalesEntry where InvNo='" + InvNo + "'");
            }

            return InvNo;

        }

        public static string Make2Digits(string InvNo)
        {
            if (InvNo.Length < 2)
            {
                InvNo = "0" + InvNo;
            }
            return InvNo;
        }
        public static string Make2Decimal(string InvNo)
        {
            InvNo = Convert.ToDecimal(InvNo).ToString("F");
            //InvNo = Convert.ToDecimal(InvNo).ToString("F");
            return InvNo;
        }
        public static string GetIPAddress()
        {
            HttpContext context = HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = context.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ipAddress;
        }
        public static void ActivityLog(string FormName, string Activity, string Description, string UserId, string IpAddress)
        {
            ExecNonQry("INSERT INTO ActivityLog (FormName, Activity, Description, UserId,IpAddress) VALUES ('" + FormName + "', '" +
                       Activity + "', '" + Description + "', '" + UserId + "','" + IpAddress + "')");
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string Translate2Bangla(string input)
        {
            input = input.Trim();
            string bangla = "";
            string isExist = SQLQuery.ReturnString("Select bangla from Dictionary where english='" + input + "' ");
            if (isExist != "")
            {
                bangla = isExist;
            }
            else
            {
                string[] words = input.Split(' ');
                foreach (string word in words)
                {
                    isExist = SQLQuery.ReturnString("Select bangla from Dictionary where english='" + word + "' ");
                    if (isExist != "")
                    {
                        bangla += isExist + " ";
                    }
                    else
                    {
                        bangla += word + " ";
                    }
                }
            }

            return bangla;
        }

        public static string LangPref(string loginId)
        {
            return SQLQuery.ReturnString("Select BranchName from Users where Username='" + loginId + "' ");
        }

        public static string UploadImage(string description, FileUpload FileUpload1, string filePath, string savePath, string linkPath, string entryBy, string photoType)
        {
            string pid = ReturnString("Select ISNULL(MAX(PhotoID),0)+1 from Photos");
            string tExt = Path.GetFileName(FileUpload1.PostedFile.ContentType);
            string fileName = RemoveSpecialCharacters(description.Trim().Replace(" ", "-")) + "." + pid + "." + tExt;
            ExecNonQry("Insert into Photos (Description, PhotoURL, EntryBy, PhotoType) VALUES ('" + description +
                       "','" + linkPath + fileName + "','" + entryBy + "','" + photoType + "')");
            pid = ReturnString("Select ISNULL(MAX(PhotoID),0) from Photos");

            string strFullPath = filePath + fileName;
            if (File.Exists(strFullPath))
            {
                File.Delete(strFullPath);
            }
            var file = FileUpload1.PostedFile.InputStream;
            System.Drawing.Image img = System.Drawing.Image.FromStream(file, false, false);
            img.Save(savePath + fileName);

            return pid;
        }


        //Upload Photo
        public static string UploadPhoto(string description, FileUpload FileUpload1, string filePath, string savePath, string linkPath, string entryBy, string photoType, int width, int height)
        {
            string pid = ReturnString("Select ISNULL(MAX(PhotoID),0)+1 from Photos");
            string tExt = Path.GetFileName(FileUpload1.PostedFile.ContentType);
            string fileName = RemoveSpecialCharacters(description.Trim().Replace(" ", "-")) + "." + pid + "." + tExt;
            ExecNonQry("Insert into Photos (Description, PhotoURL, EntryBy, PhotoType) VALUES ('" + description +
                       "','" + linkPath + fileName + "','" + entryBy + "','" + photoType + "')");
            pid = ReturnString("Select ISNULL(MAX(PhotoID),0) from Photos");

            string strFullPath = filePath + fileName;
            if (File.Exists(strFullPath))
            {
                File.Delete(strFullPath);
            }
            var file = FileUpload1.PostedFile.InputStream;
            System.Drawing.Image img = System.Drawing.Image.FromStream(file, false, false);
            Size size = new Size(width, height);
            img = resizeImage(img, size);

            //if (tExt == "jpeg")
            //{
            //    ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            //    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            //    EncoderParameters myEncoderParameters = new EncoderParameters(1);
            //    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
            //    myEncoderParameters.Param[0] = myEncoderParameter;
            //    img.Save(savePath, jgpEncoder, myEncoderParameters);
            //}
            //else
            //{
            img.Save(savePath + fileName);
            //}
            return pid;
        }

        //Image Resizing
        public static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, System.Drawing.Size size)
        {
            return (System.Drawing.Image)(new System.Drawing.Bitmap(imgToResize, size));
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        //End of Resize Image

        public static DataTable GetSmsTemplate(string templateName)
        {
            return ReturnDataTable("SELECT ID, TemplateName, Template, Variables, Subject FROM SMSTemplate Where TemplateName='" + templateName + "'");
        }
        public static DataTable GetEmailTemplate(string templateName)
        {
            return ReturnDataTable("SELECT ID, TemplateName, Template, Variables, Subject FROM EmailTemplate Where TemplateName='" + templateName + "'");
        }
        public static string ToAgeString(DateTime dob)
        {
            DateTime dt = DateTime.Today;

            int days = dt.Day - dob.Day;
            if (days < 0)
            {
                dt = dt.AddMonths(-1);
                days += DateTime.DaysInMonth(dt.Year, dt.Month);
            }

            int months = dt.Month - dob.Month;
            if (months < 0)
            {
                dt = dt.AddYears(-1);
                months += 12;
            }

            int years = dt.Year - dob.Year;

            return string.Format("{0} year{1}, {2} month{3} and {4} day{5}",
                                 years, (years == 1) ? "" : "s",
                                 months, (months == 1) ? "" : "s",
                                 days, (days == 1) ? "" : "s");
        }
        public static string LocalDateFormat(string s)
        {
            try
            {
                if (s != "")
                {
                    s = Convert.ToDateTime(s).ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {

            }
            return s;
        }
        public static string ReturnInvNo(string tableName, string colName, string colEval)
        {
            SqlCommand cmd = new SqlCommand("Select ISNULL(MAX(" + colName + "),0)+1001 from " + tableName, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string invNo = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            string isExist = ReturnString("Select " + colEval + " from " + tableName + " where (" + colEval + " like '%" + invNo + "%')");
            while (isExist != "")
            {
                invNo = Convert.ToString(Convert.ToInt32(invNo) + 1);
                isExist = ReturnString("Select " + colEval + " from " + tableName + " where (" + colEval + " like '%" + invNo + "%')");
            }

            return invNo;
        }

        public static string DecimalToWords(string s)
        {
            string taka = "0"; string paisa = "0";
            string[] words = s.Split('.');
            foreach (string word in words)
            {
                if (taka.Length <= 5)
                {
                    taka = NumberToWords(Convert.ToInt32(word));
                }
                else
                {
                    paisa = NumberToWords(Convert.ToInt32(word));
                }
            }

            return " Taka " + taka + " and " + paisa + " Paisa Only.";//+" Taka Only.";
        }

        public static string GetImage(string photoID)
        {
            string absoluteUrl = SQLQuery.ReturnString("Select SettingValue from Settings where id=1");
            string url = absoluteUrl + SQLQuery.ReturnString("Select PhotoURL from Photos where PhotoID='" + photoID + "'");
            if (url == "")
            {
                url = absoluteUrl + "Uploads/Image_coming_soon.png";
            }
            return url;
        }
        public static string GenerateCustomerQS(string CustName, string custId)
        {
            string qs = Text2URL(CustName);
            string isExist = ReturnString("Select QueryStringURL from Customer where QueryStringURL='" + qs + "' AND sl<>'" + custId + "'");
            int i = 0;
            while (isExist != "")
            {
                i++;
                isExist = SQLQuery.ReturnString("Select QueryStringURL from Customer where QueryStringURL='" + (qs + i) + "'");
            }
            if (i > 0)
            {
                qs = qs + i;
            }
            return qs;
        }

        public static string UploadImage(string description, FileUpload FileUpload1, string filePath, string savePath, string entryBy, string photoType)
        {
            string pid = ReturnString("Select ISNULL(MAX(PhotoID),0)+1 from Photos");
            FileInfo fi = new FileInfo(FileUpload1.PostedFile.FileName);
            string tExt = fi.Extension;
            string fileName = RemoveSpecialCharacters(description.Trim().Replace(" ", "-")) + "." + pid + "." + tExt;
            ExecNonQry("Insert into Photos (Description, PhotoURL, EntryBy, PhotoType) VALUES (N'" + description + "','./Uploads/Photos/" + fileName + "','" + entryBy + "','" + photoType + "')");
            pid = ReturnString("Select ISNULL(MAX(PhotoID),0) from Photos");

            string strFullPath = filePath + fileName;

            if (File.Exists(strFullPath))
            {
                File.Delete(strFullPath);
            }
            var file = FileUpload1.PostedFile.InputStream;
            System.Drawing.Image img = System.Drawing.Image.FromStream(file, false, false);
            img.Save(savePath + fileName);

            return pid;
        }

        public static string UploadFile(string description, FileUpload FileUpload1, string filePath, string savePath, string linkPath, string entryBy, string photoType)
        {
            string pid = ReturnString("Select ISNULL(MAX(PhotoID),0)+1 from Photos");
            FileInfo fi = new FileInfo(FileUpload1.PostedFile.FileName);
            string tExt = fi.Extension;
            string fileName = RemoveSpecialCharacters(description.Trim().Replace(" ", "-")) + "." + pid + "." + tExt;
            ExecNonQry("Insert into Photos (Description, PhotoURL, EntryBy, PhotoType) VALUES ('" + description +
                       "','" + linkPath + fileName + "','" + entryBy + "','" + photoType + "')");
            pid = ReturnString("Select ISNULL(MAX(PhotoID),0) from Photos");

            string strFullPath = filePath + fileName;

            if (File.Exists(strFullPath))
            {
                File.Delete(strFullPath);
            }
            FileUpload1.PostedFile.SaveAs(savePath + fileName);
            return pid;
        }
        public static string RemoveSpecialCharacters(string str)
        {
            str = str.Trim().Replace(" ", "-");
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Replace("--", "-");
        }
        public static string Text2URL(string str)
        {
            str = str.Trim().ToLower().Replace(" ", "-").Replace(".", "");
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().ToLower().Replace("--", "-");
        }
        public static string ReturnInvNo(string tableName, string colName, string prefix, string colEval, string ProjectID)
        {
            SqlCommand cmd = new SqlCommand("Select ISNULL(COUNT(" + colName + "),0)+1001 from " + tableName + " where ProjectId = '" + ProjectID + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            string invNo = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            string isExist = ReturnString("Select " + colEval + " from " + tableName + " where ProjectId = '" + ProjectID + "' AND (" + colEval + " = '" + prefix + invNo + "')");
            while (isExist != "")
            {
                invNo = Convert.ToString(Convert.ToInt32(invNo) + 1);
                isExist = ReturnString("Select " + colEval + " from " + tableName + " where ProjectId = '" + ProjectID + "' AND (" + colEval + " = '" + prefix + invNo + "')");
            }

            return prefix + invNo;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0) { return "zero"; }
            if (number < 0) { return "minus " + NumberToWords(Math.Abs(number)); }
            string words = "";
            if ((number / 10000000) > 0) { words += NumberToWords(number / 10000000) + " Crore "; number %= 10000000; }
            if ((number / 100000) > 0) { words += NumberToWords(number / 100000) + " Lac "; number %= 100000; }
            if ((number / 1000) > 0) { words += NumberToWords(number / 1000) + " Thousand "; number %= 1000; }
            if ((number / 100) > 0) { words += NumberToWords(number / 100) + " Hundred "; number %= 100; }
            if (number > 0)
            {
                if (words != "") { words += "and "; }
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "seventy", "Eighty", "Ninety" };
                if (number < 20) { words += unitsMap[number]; }
                else { words += tensMap[number / 10]; if ((number % 10) > 0) { words += "-" + unitsMap[number % 10]; } }
            }
            return words;//+" Taka Only.";
        }

        public static string separateColor(string line)
        {
            string s = ""; string t = "";
            int i = 1;

            string[] words = line.Split(' ');
            foreach (string word in words)
            {
                if (i == 1)
                {
                    s += "<span style='color: #FF6600;'>" + word + " </span>";
                }
                else
                {
                    s += word + " ";
                }

                i++;
            }
            return s;
        }
        public static string Int2WordsBangla(string quantity)
        {
            string[] amt = Convert.ToString(quantity).Split('.');
            string qty = amt[0];

            return qty = AmountInWordsBangla(Convert.ToDouble(qty));
        }

        public static string Decimal2WordsBangla(string amount)
        {
            string[] amt = Convert.ToString(amount).Split('.');
            string taka = amt[0];
            string poisa = amt[1];

            taka = AmountInWordsBangla(Convert.ToDouble(taka));
            if (poisa.Length == 1)
            {
                poisa = AmountInWordsBangla(Convert.ToDouble(poisa + "0"));
            }
            else if (poisa.Length == 2)
            {
                poisa = AmountInWordsBangla(Convert.ToDouble(poisa));
            }
            else if (poisa.Length > 2)
            {
                poisa = poisa.Substring(0, 2);
            }

            if (poisa.Length == 0 || poisa.Length == 1)
            {
                //return taka + "টাকা ";
                return taka + "মাত্র।";
            }
            else
            {
                return taka + "টাকা " + poisa + "পয়সা ";
            }
        }

        private static string AmountInWordsBangla(double amount)
        {
            int num = (int)amount;
            if (num == 0)
            {
                return " ";
            }
            if ((num > 0) && (num <= 0x63))
            {
                string[] strArray = new string[] {
                "এক", "দুই", "তিন", "চার", "পাঁচ", "ছয়", "সাত", "আট", "নয়", "দশ", "এগার", "বারো", "তের", "চৌদ্দ", "পনের", "ষোল",
                "সতের", "আঠার", "ঊনিশ", "বিশ", "একুশ", "বাইস", "তেইশ", "চব্বিশ", "পঁচিশ", "ছাব্বিশ", "সাতাশ", "আঠাশ", "ঊনত্রিশ", "ত্রিশ", "একত্রিস", "বত্রিশ",
                "তেত্রিশ", "চৌত্রিশ", "পঁয়ত্রিশ", "ছত্রিশ", "সাঁইত্রিশ", "আটত্রিশ", "ঊনচল্লিশ", "চল্লিশ", "একচল্লিশ", "বিয়াল্লিশ", "তেতাল্লিশ", "চুয়াল্লিশ", "পয়তাল্লিশ", "ছেচল্লিশ", "সাতচল্লিশ", "আটচল্লিশ",
                "উনপঞ্চাশ", "পঞ্চাশ", "একান্ন", "বায়ান্ন", "তিপ্পান্ন", "চুয়ান্ন", "পঞ্চান্ন", "ছাপ্পান্ন", "সাতান্ন", "আটান্ন", "উনষাট", "ষাট", "একষট্টি", "বাষট্টি", "তেষট্টি", "চৌষট্টি",
                "পয়ষট্টি", "ছিষট্টি", " সাতষট্টি", "আটষট্টি", "ঊনসত্তর ", "সত্তর", "একাত্তর ", "বাহাত্তর", "তেহাত্তর", "চুয়াত্তর", "পঁচাত্তর", "ছিয়াত্তর", "সাতাত্তর", "আটাত্তর", "ঊনাশি", "আশি",
                "একাশি", "বিরাশি", "তিরাশি", "চুরাশি", "পঁচাশি", "ছিয়াশি", "সাতাশি", "আটাশি", "উননব্বই", "নব্বই", "একানব্বই", "বিরানব্বই", "তিরানব্বই", "চুরানব্বই", "পঁচানব্বই ", "ছিয়ানব্বই ",
                "সাতানব্বই", "আটানব্বই", "নিরানব্বই"
            };
                return (strArray[num - 1] + " ");
            }
            //if ((num >= 100) && (num <= 0xc7))
            //{
            //    return (AmountInWordsBangla((double)(num / 100)) + "এক শত " + AmountInWordsBangla((double)(num % 100)) + "");
            //}
            if ((num >= 100) && (num <= 0x3e7))
            {
                return (AmountInWordsBangla((double)(num / 100)) + "শত " + AmountInWordsBangla((double)(num % 100)) + "");
            }
            if ((num >= 0x3e8) && (num <= 0x7cf))
            {
                return ("এক হাজার " + AmountInWordsBangla((double)(num % 0x3e8)) + "");
            }
            if ((num >= 0x3e8) && (num <= 0x1869f))
            {
                return (AmountInWordsBangla((double)(num / 0x3e8)) + "হাজার " + AmountInWordsBangla((double)(num % 0x3e8)) + "");
            }
            if ((num >= 0x186a0) && (num <= 0x30d3f))
            {
                return ("এক লক্ষ " + AmountInWordsBangla((double)(num % 0x186a0)) + "");
            }
            if ((num >= 0x186a0) && (num <= 0x98967f))
            {
                return (AmountInWordsBangla((double)(num / 0x186a0)) + "লক্ষ " + AmountInWordsBangla((double)(num % 0x186a0)) + "");
            }
            if ((num >= 0x989680) && (num <= 0x1312cff))
            {
                return ("এক কোটি " + AmountInWordsBangla((double)(num % 0x989680)) + "");
            }
            return (AmountInWordsBangla((double)(num / 0x989680)) + "কোটি " + AmountInWordsBangla((double)(num % 0x989680)) + "");
        }

        public static void Subscribe(string QueryType, string Name, string Company, string Email, string Phone, string message, string MemberType, string NewsLetter, string ProductInterested, string ProductID, string ipAddress)
        {
            ExecNonQry(@"INSERT INTO Subscribers (QueryType, Name, Company, Email, Phone, Message, MemberType, NewsLetter, ProductInterested, ProductID, IPAddress) 
                            VALUES ('" + QueryType + "','" + Name + "', '" + Company + "', '" + Email + "','" + Phone + "','" + message + "','" + MemberType + "','" + NewsLetter + "','" + ProductInterested + "','" + ProductID + "','" + ipAddress + "')");
        }
        /*
        public static IRestResponse SendEmail(string receiver, string subject, string body)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", "key-9b59792d89239f9e5a8d5fc223495a1e");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "extreme.com.bd", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Extreme Solutions <marketing@extreme.com.bd>");
            request.AddParameter("to", receiver);// you can use hundreds of "to" field!
            //request.AddParameter("cc", "baz@example.com");
            //request.AddParameter("bcc", "bar@example.com");
            request.AddParameter("subject", subject);
            //request.AddParameter("text", "Testing some Mailgun awesomness!");
            request.AddParameter("html", body);
            //request.AddFile("attachment", Path.Combine("files", "test.jpg"));
            //request.AddFile("attachment", Path.Combine("files", "test.txt"));
            request.Method = Method.POST;
            return client.Execute(request);
        }*/
        public static string SendEmail2(string receiverEmail, string replyTo, string subject, String body)
        {
            //try
            //{
            //    //create the mail message
            //    MailMessage mail = new MailMessage();
            //    mail.From = new MailAddress("news@extreme-office.com", "Extreme OFFICE");
            //    mail.To.Add(receiverEmail);
            //    mail.Subject = subject;

            //    mail.Body = body;
            //    mail.IsBodyHtml = true;

            //    //send the message
            //    SmtpClient smtp = new SmtpClient("smtp.postmarkapp.com", 587);
            //    smtp.EnableSsl = true;
            //    //to authenticate we set the username and password properites on the SmtpClient
            //    smtp.Credentials = new NetworkCredential("26ddd9f7-322e-426a-96ed-de60aa9f707d", "26ddd9f7-322e-426a-96ed-de60aa9f707d");
            //    smtp.Send(mail);
            //    return "Success" + DateTime.Now;
            //}
            //catch (Exception ex)
            //{
            //    return ex.ToString();
            //}
            try
            {
                //create the mail message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("btvstoremanagementsystem@gmail.com", "BTV");
                mail.To.Add(receiverEmail);
                mail.Subject = subject;

                mail.Body = body;
                mail.IsBodyHtml = true;

                //send the message
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                //to authenticate we set the username and password properites on the SmtpClient
                smtp.Credentials = new NetworkCredential("btvstoremanagementsystem@gmail.com", "btvStore1964");
                smtp.Send(mail);
                return "Success" + DateTime.Now;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            ////Test Line
            //return "Success" + DateTime.Now;

        }
        public static string SendJoiningEmail(string userId, string password, string name, string prjID)
        {
            string confirmID = SQLQuery.ReturnString("Select UserId from    aspnet_Membership where LoweredEmail='" + userId + "'");
            string company = SQLQuery.ReturnString("Select  ProjectName FROM Projects WHERE VID='" + prjID + "'");
            string msg = "Thank you for registering at Library of Ministry of Expatriates' Welfare & Overseas Employment. We appreciate your decision and assure our best services at all times." +
                "<br><br>Before using the system you need to confirm your email address by clicking below link:<br/><br/><a href='http://mewlms.os.com.bd/customer-registration?confirmID=" + confirmID + @"'>http://mewlms.os.com.bd/customer-registration.aspx?confirmID=" + confirmID + @"</a><br/>" +
            "<br><br>After confirming by clicking the link, you will be able to login using the info you have provided during registration process and check your business status at any time.<br/><br/>If you have forgotten and wish to reset your login password at any time, you can do so by accessing the 'Forgot Password' link on member login page.";

            msg += "<br><br>If you have any questions or need any help, please feel free to e-mail us at <a href='mailto:info@os.com.bd' target='_blank'>info@os.com.bd</a> or call our help-desk number: +88 01000 000000<br><br>";
            msg += "<br><br>Regards,<br><br>MIS Team<br>Ministry of Expatriates' Welfare & Overseas Employment<br><br>";

            string subject = "Your Library Membership Account is almost ready!";
            msg = GetMailBodyFromTemplate("Please verify your email Address", "https://app.extreme-office.com/Uploads/Email-Verification-Banner-F.jpg", msg, "http://mewlms.os.com.bd/customer-registration.aspx?confirmID=" + confirmID, "Verify Me!");
            SendEmail("info@os.com.bd", userId, "xservice.team@gmail.com", subject, msg);
            return msg;
        }
        public static string SendAllertEmail(string userId, string password, string name, string prjID, string subscription)
        {

            string company = SQLQuery.ReturnString("Select  ProjectName FROM Projects WHERE VID='" + prjID + "'");
            string PkgId = SQLQuery.ReturnString("Select  Package FROM Projects WHERE VID='" + prjID + "'");
            string package = SQLQuery.ReturnString("Select Package from Packages where Id='" + PkgId + "'");
            string email = SQLQuery.ReturnString("Select  EntryBy FROM Projects WHERE VID='" + prjID + "'");
            string Phone = SQLQuery.ReturnString("Select  phone FROM Employee WHERE ProjectId='" + prjID + "' and email='" + email + "'");
            string duration = SQLQuery.ReturnString("Select  Duration FROM SubsDuration WHERE Id='" + subscription + "'");
            string msg = "Hi,<br><br>A company named " + company.ToUpper() + " , Email: " + email + " and Phone: " + Phone + " has been registered in Business " +
                         package + " for " + duration +
                         "<br><br>After confirming  Customer will be able to login using the info you have provided during registration process and check your business status at any time.";


            msg = GenerateMailBody(msg, prjID);
            SendEmail("support@xtremebd.com", userId, "xservice.team@gmail.com", "Welcome to Extreme Solutions!", msg);
            return msg;
        }

        //public static string IsRoomChecked(string roomNo, DateTime dt, string btnText, string id)
        //{
        //    string pId = ReturnString("Select Id from ECRoomAllocation where RoomNo='" + roomNo + "' And CheckIn<='" + dt.ToString("yyyy-MM-dd") + "' And CheckOut>='" + dt.ToString("yyyy-MM-dd") + "'");
        //    if (btnText == "Update" && id != "")
        //    {
        //        pId = ReturnString("Select Id from ECRoomAllocation where RoomNo='" + roomNo + "' And CheckIn<='" + dt.ToString("yyyy-MM-dd") + "' And CheckOut>='" + dt.ToString("yyyy-MM-dd") + "' and id<>'" + id + "'");
        //    }
        //    return pId;
        //}

        public static string GenerateMailBody(string mainText, string prjId)
        {
            string company = SQLQuery.ReturnString("Select  ProjectName FROM Projects WHERE VID='" + prjId + "'");
            string address = HttpUtility.HtmlEncode(SQLQuery.ReturnString("Select  ReportHeader FROM Projects WHERE VID='" + prjId + "'"));

            string body = "<div style='font-family: Calibri,Candara,Arial, sans-serif;'>";
            body += "<table border='0' cellspacing='0' cellpadding='0' width='100%'>";

            body += "<tr>";
            body += "<td width='50%' valign='top'><h2><img src='http://app.extreme-office.com/branding/xerp-logo.png' /></h2></td>";
            body += "<td width='50%'><p align='right'>";
            body += address;
            body += "</p></td>";
            body += "</tr>";

            body += "<tr>";
            body += "<td colspan='2' valign='top'>" + mainText + "</td>";
            body += "</tr>";
            body += "</table><hr/>";
            return body;
        }
        public static string SMSSSLGateway(string mobile, string smsBody)
        {


            //Procedure one  
            string sid = "RHDMASK";
            string user = "RHD";
            string pass = "68w>7D05";
            string resp = "";

            string phone = "+8801753719719";
            string sms_url = "http://sms.sslwireless.com/pushapi/dynamic/server.php?";

            string myParameters = "user=" + user + "&pass=" + pass + "&msisdn=" + mobile + "&sms=" + System.Web.HttpUtility.UrlEncode(smsBody) + "&csmsid=" + "1234567890" + "&sid=" + sid;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sms_url + myParameters);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) using (Stream stream = response.GetResponseStream()) using (StreamReader reader = new StreamReader(stream))
            {
                resp = reader.ReadToEnd();
            }
            return resp;

        }

        public static void SendSMS(string mob, string msg, string type, string lName)
        {
            try
            {
                string prjId = ProjectID(lName);
                //int balance = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(SUM(InQty)-SUM(OutQty),0) from SMSBalance WHERE ProjectId='" + prjId + "'"));
                mob = VerifyMobileNumber(mob);

                if (msg.Length > 150)
                {
                    msg = msg.Substring(0, 149);
                }

                if (mob != "")
                {
                    string reqText = "https://license.extreme.com.bd/api/sms?user=munna&pswd=123456&rcvr=" + mob + "&msg=" + msg;
                    WebRequest.Create(reqText).GetResponse();


                    //DataTable dtx = ReturnDataTable(@"SELECT TOP (1) pid, GatewayName, HostIP, SenderName, UserID, Password FROM SMSGateway WHERE (IsActive = '1')");

                    //foreach (DataRow drx in dtx.Rows)
                    //{
                    //    string password = drx["Password"].ToString();
                    //    string reqText = drx["HostIP"].ToString();
                    //    string sender = drx["SenderName"].ToString();
                    //    string userID = drx["UserID"].ToString();

                    //    reqText = reqText.Replace("[user]", userID).Replace("[pswd]", password).Replace("[rcvr]", mob).Replace("[msg]", msg);
                    //    WebRequest.Create(reqText).GetResponse();

                    //    ExecNonQry("Insert Into SMSBalance (ProjectId, Description, OutQty, Type, EntryBy)VALUES('" + prjId + "','Auto Bill to "+ mob + ". Message: "+msg+"', '1','Used','" + lName + "')");
                    //}
                }
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
            }
        }
        public static string VerifyMobileNumber(string mobileNo)
        {
            mobileNo = mobileNo.Replace(" ", "").Trim();

            if (mobileNo.StartsWith("+88"))
            {
                mobileNo = mobileNo.Replace("+88", "");
            }

            if (!mobileNo.StartsWith("88"))
            {
                mobileNo = "88" + mobileNo;
            }

            if (mobileNo.Length != 13 || !mobileNo.StartsWith("8801"))
            {
                mobileNo = "";
            }

            return mobileNo;
        }
        public static void GetReminders(string prjID)
        {
            try
            {
                DataTable dtx = SQLQuery.ReturnDataTable(
                    @"SELECT    sl, CustomerID, Status, Detail, EntryDate, EntryBy, RemindDate, IsRemind, EmailSend FROM Leads WHERE RemindDate<'" +
                    DateTime.Now.ToString("yyyy-MM-dd hh:mm tt") + "' AND (EmailSend = '1') AND IsRemind=1");

                foreach (DataRow drx in dtx.Rows)
                {
                    string msg = "Below task was scheduled to remind on " +
                                 Convert.ToDateTime(drx["RemindDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt") +
                                 " for taking actions";
                    msg += "<br/> <br/>Customer Name: <b>" +
                           SQLQuery.ReturnString("Select Name from Directory where sl='" + drx["CustomerID"].ToString() +
                                                 "'") + "</b>";
                    msg += "<br/>Lead Description: <b>" + drx["Detail"].ToString() + "</b>";
                    msg += "<br/>Entry by: " + drx["EntryBy"].ToString();
                    msg += "<br/>Entry date: " +
                           Convert.ToDateTime(drx["EntryDate"].ToString()).ToString("dd/MM/yyyy hh:mm tt");
                    msg += "<br/>Task Status: " + drx["Status"].ToString();

                    msg = GenerateMailBody(msg, prjID);
                    SendEmail("support@xtremebd.com", "info@extreme.com.bd", "xservice.team@gmail.com", drx["Detail"].ToString(), msg);

                    DateTime dt = Convert.ToDateTime(drx["RemindDate"].ToString());
                    SQLQuery.ExecNonQry(
                        "Insert into Tasks (taskFor, TaskName, TaskDetails, DeadLine, Priority, Status, EntryBy)" +
                        " VALUES ('" + drx["EntryBy"].ToString() + "', 'Sales Lead', '" + drx["Detail"].ToString() +
                        "', '" + dt + "', 'High', 'Pending', '" + drx["EntryBy"].ToString() + "')");

                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                SQLQuery.ExecNonQry("Update Leads SET IsRemind=0, EmailSend=2 WHERE  RemindDate<'" +
                                    DateTime.Now.ToString("yyyy-MM-dd hh:mm tt") + "'");
            }
        }

        public static string GetMailBodyFromTemplate(string h1Subject, string bannerURL, string bodyText, string actionLink, string actionText)
        {
            if (bannerURL == "")
            {
                bannerURL = "https://app.extreme-office.com/Uploads/ExtremeBG.png";
            }

            string body = @"
<table width='602' cellpadding='0' cellspacing='0' border='0' align='center'>
	<tbody><tr>
		<td colspan='5'><img src='https://app.extreme-office.com/Uploads/ExtremeLogo.png' width='124' alt='XO'/> <br/> </td>
	</tr>
	<tr>
		<td bgcolor='#ccc' colspan='5'><img src='https://app.extreme-office.com/Uploads/dot.gif' width='1' height='20'></td>
	</tr>
	<tr>
		<td bgcolor='#ccc'><img src='https://app.extreme-office.com/Uploads/dot.gif' width='1' height='1'></td>
		<td><img src='https://app.extreme-office.com/Uploads/dot.gif' width='30' height='1'></td>
		<td>
			<table width='540' cellpadding='0' cellspacing='0' border='0' align='center'>
				<tbody><tr>
					<td align='center'><img src='https://app.extreme-office.com/Uploads/dot.gif' width='1' height='1'>
					<h1 style='font-family:Helvetica;font-size:1.5em;font-weight:bold'>" + h1Subject + @"</h1></td>
				</tr>
				
				<tr valign='top'>
					<td>
						<table width='540' cellpadding='0' cellspacing='0' border='0' align='center'>
							<tbody><tr>
								<td>
								<img src='" + bannerURL + @"' width='550' alt='Extreme OFFICE banner'>								
								<img src='https://app.extreme-office.com/Uploads/dot.gif' width='1' height='20' alt='''><br>
                                <p style='font-family:Helvetica;font-size:1em;line-height:1.4em;text-align:justify;'>
								" + bodyText + @"</p>								
								<p align='center' style='margin:30px 0'>
								<a href='" + actionLink + @"' target='_blank' data-saferedirecturl='" + actionLink + @"' style='font-family: tahoma;font-weight: 600;display: inline-block;padding: 8px 22px;font-size:18px;cursor: pointer;text-align: center;text-decoration: none;outline: none;color: #fff;background-image: linear-gradient(#33de3a, #448e16);border: 3px solid #24c72a;border-radius: 12px;-webkit-box-shadow: 0px 3px 11px 0px rgba(0,0,0,0.75);-moz-box-shadow: 0px 3px 11px 0px rgba(0,0,0,0.75);box-shadow: 0px 3px 11px 0px rgba(0,0,0,0.75);'>" + actionText + @"</a>
								</p>
							</td></tr>
						</tbody></table>
					</td>
				</tr>
			</tbody></table>
		</td>
		<td><img src='https://app.extreme-office.com/Uploads/dot.gif' width='30' height='1'></td>
		<td bgcolor='#ccc'><img src='https://app.extreme-office.com/Uploads/dot.gif' width='1' height='1'></td>
	</tr>
	<tr>
		<td bgcolor='#ccc' colspan='5'><img src='https://app.extreme-office.com/Uploads/dot.gif' width='1' height='20'></td>
	</tr>
	<tr>
		<td colspan='5' align='center'><img src='https://app.extreme-office.com/Uploads/dot.gif' width='1' height='5'><br>
<p style='color:#59596b;font-family:Helvetica;font-size:.9em'><i>If you are not designated for this email or wish to unsubscribe, please send an e-mail to <a href='mailto:info@os.com.bd' target='_blank'>info@os.com.bd</a></i><br></p>
<p style='color:#80bd01;font-family:Helvetica;font-size:.7em'><br>
		This message was sent from <b>Extreme Solutions</b>. <br> Wali Mansion (4th Floor), 600 Sheikh Mujib Road, Pathantooly, Chittagong 4100
</p></td>
	</tr>
</tbody></table>
";
            return body;
        }
        public static void SendEmail(string senderEmail, string receiverEmail, string replyTo, string subject, String body)
        {
            try
            {
                /*
                MailMessage o = new MailMessage("TEAM EXTREME <support@xtremebd.com>", receiverEmail, subject, body);
                o.ReplyTo = new MailAddress(replyTo);
                o.IsBodyHtml = true;
                NetworkCredential netCred = new NetworkCredential("support@xtremebd.com", "jDvm63*4");
                SmtpClient smtpobj = new SmtpClient("mail.xtremebd.com", 25);
                smtpobj.EnableSsl = false;
                smtpobj.Credentials = netCred;
                smtpobj.Send(o);*/

                SendEmail2(receiverEmail, replyTo, subject, body);
            }
            catch (Exception ex)
            {

            }
        }

        public static void MonthReset()
        {


        }

        public static void GetMailingList()
        {
            string qty = SQLQuery.ReturnString("Select SettingValue from Settings where SettingName='Email Sending Frequency'");
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(
                @"SELECT TOP(" + qty + @") EmailCampDetail.id, EmailCampDetail.EmailAddress as email, Directory.Name, Directory.ContactPerson, EmailCampDetail.TemplateID, Directory.Designation, EmailTemplate.EmailSubject, EmailTemplate.id AS EmailTemplateID, EmailCampDetail.CampaignID
                FROM EmailCampDetail INNER JOIN Directory ON EmailCampDetail.DirectoryID = Directory.sl INNER JOIN EmailTemplate ON EmailCampDetail.TemplateID = EmailTemplate.TargetIndustry WHERE (EmailCampDetail.IsReceived = '0') order by EmailCampDetail.id");

            foreach (DataRow drx in dtx.Rows)
            {
                DataTable dtx2 = RunQuery.SQLQuery.ReturnDataTable(@"SELECT Top(1) id, GatewayName, Link, UserName, Password, Port, EnableSSL, MonthlyLimit, MonthID, MonthlySendQty, TotalSendQty, SendingPriority
                                                                FROM EmailGateways WHERE (IsActive = '1') AND SendingPriority IN (Select MIN(SendingPriority) from EmailGateways WHERE (IsActive = '1')) ");
                foreach (DataRow drx2 in dtx2.Rows)
                {
                    string emailBody = SQLQuery.ReturnString("SELECT SUBSTRING((SELECT left(('' + PageContent),3900) FROM BodyContent where  ContentType='Email Template' AND SectionTitle='" + drx["TemplateID"] + "' ORDER BY ContentSerial FOR XML PATH('')),2,200000) AS CSV");
                    emailBody += "<br/><br/><br/><center><div style='text-align:center; font-size:9px;'>This email was sent you, as you have been subscribed to receive email from EXTREME SOLUTIONS.<br/>To unsubscribe from the mailing list, please <a href='http://extreme.com.bd/connect/unsubscribe/?email=" + drx["email"] + "'>click here</a></div></center>";
                    string isSuccess = SQLQuery.SendMarketingEmail(drx2["UserName"].ToString(), drx["email"].ToString(), drx["EmailSubject"].ToString(), HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(emailBody)), drx2["Link"].ToString(), drx2["Port"].ToString(), drx2["Password"].ToString(), Convert.ToBoolean(drx2["EnableSSL"].ToString()));

                    string newID = DateTime.Now.ToString("yyyy-MM-01");
                    string oldID = drx2["MonthID"].ToString();
                    string monthCounter = "MonthlySendQty+1";
                    if (oldID != newID)
                    {
                        monthCounter = "0";
                    }
                    SQLQuery.ExecNonQry("UPDATE EmailGateways SET MonthlySendQty=" + monthCounter + ", MonthID='" + newID + "', TotalSendQty=TotalSendQty+1, SendingPriority=(PriorityRatio*" + (Convert.ToDecimal(drx2["MonthlySendQty"].ToString()) + 1) + ") WHERE ID=" + drx2["id"]);

                    if (isSuccess == "success")
                    {
                        SQLQuery.ExecNonQry("UPDATE EmailCampDetail SET IsReceived=1, ReceiveDate='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "', GateWayID='" + drx2["id"] + "' WHERE ID=" + drx["id"]);
                    }
                }
            }
        }
        public static string SendMarketingEmail(string loginID, string receiverEmail, string subject, String body, string server, string port, string pswd, bool enableSSL)
        {
            try
            {
                if (loginID.Contains("gmail"))
                {
                    return SendSSLGmail(loginID, receiverEmail, subject, body, server, port, pswd);
                }
                else
                {
                    //create the mail message
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("info@os.com.bd", "Ministry of Expatriates' Welfare & Overseas Employment");
                    mail.To.Add(receiverEmail);
                    mail.ReplyToList.Add("info@os.com.bd");
                    mail.Subject = subject;

                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    //send the message
                    SmtpClient smtp = new SmtpClient(server, Convert.ToInt32(port));
                    smtp.EnableSsl = enableSSL;
                    //to authenticate we set the username and password properites on the SmtpClient
                    smtp.Credentials = new NetworkCredential(loginID, pswd);
                    smtp.Send(mail);
                    return "success";

                }
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.ToString();
            }
        }

        public static string GetCaptions(string type, string projectId)
        {
            if (type == "Vendors")
            {

            }
            else
            if (type == "Customers")
            {

            }
            else
            if (type == "Contract")
            {

            }
            else
            if (type == "Agent")
            {

            }
            else
            if (type == "Products")
            {

            }
            return FindCaption(type, projectId);
        }

        public static string SendSSLGmail(string loginID, string receiverEmail, string subject, String body, string server, string port, string pswd)
        {
            try
            {
                using (MailMessage mm = new MailMessage(loginID, receiverEmail))
                {
                    mm.From = new MailAddress("marketing@xtremebd.com", "Extreme Solutions");
                    mm.To.Add(receiverEmail);
                    mm.ReplyToList.Add("xservice.team@gmail.com");
                    mm.Subject = subject;
                    mm.Body = body;
                    //if (fuAttachment.HasFile)
                    //{
                    //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                    //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                    //}
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = server;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(loginID, pswd);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = Convert.ToInt32(port);
                    smtp.Send(mm);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return "ERROR: " + ex.ToString();
            }
        }

        public static string FormatBDNumber(string q)
        {
            var bdCulture = CultureInfo.CreateSpecificCulture("bn-BD");
            return string.Format(bdCulture, "{0:N2}", Convert.ToDouble(q));
        }
        public static string FormatBDNumber(decimal q)
        {
            var bdCulture = CultureInfo.CreateSpecificCulture("bn-BD");
            return string.Format(bdCulture, "{0:N2}", Convert.ToDouble(q));
        }
        //public static void LoadrptHeader(XerpDataSet ds, CrystalDecisions.CrystalReports.Engine.ReportDocument rpt, string ProjectId)
        //{
        //    DataTable dtx2 = SQLQuery.ReturnDataTable(@"SELECT TOP (1) [VID], [ProjectName], [ReportHeader], [ProjectId], [Logo], [Photo]  FROM [Projects] Where [VID]='" + ProjectId + "'");

        //    DataTableReader drx = dtx2.CreateDataReader();
        //    ds.Load(drx, LoadOption.OverwriteChanges, ds.Company);
        //    rpt.Subreports["CrptHeader.rpt"].SetDataSource((DataTable)ds.Company);
        //}

        public static string IsBooked(string roomNo, DateTime dt)
        {
            return ReturnString("Select Id from BookingDetail where RoomId='" + roomNo + "' And ChkInDate<='" + dt.ToString("yyyy-MM-dd") + "' And ChkOutDate>='" + dt.ToString("yyyy-MM-dd") + "' AND IsAprove='A'");
        }

        public static string IsBooked4Update(string roomNo, DateTime dt, string btnText, string id)
        {
            string pId = ReturnString("Select Id from BookingDetail where RoomId='" + roomNo + "' And ChkInDate<='" + dt.ToString("yyyy-MM-dd") + "' And ChkOutDate>='" + dt.ToString("yyyy-MM-dd") + "'");
            if (btnText == "Update" && id != "")
            {
                pId = ReturnString("Select Id from BookingDetail where RoomId='" + roomNo + "' And ChkInDate<='" + dt.ToString("yyyy-MM-dd") + "' And ChkOutDate>='" + dt.ToString("yyyy-MM-dd") + "' and id<>'" + id + "'");
            }
            return pId;
        }

        public static void InsertSettings(string type, string pid, string startTime, string mnthTarget, string user, string industry)
        {
            SQLQuery.ExecNonQry("INSERT INTO Settings (SettingName, SettingValue, ProjectId) VALUES ('Start Time','" + startTime + "','" + pid + "')");
            SQLQuery.ExecNonQry("INSERT INTO Settings (SettingName, SettingValue, ProjectId) VALUES ('Monthly Sales Target','" + mnthTarget + "','" + pid + "')");
            SQLQuery.ExecNonQry("INSERT INTO Settings (SettingName, SettingValue, ProjectId) VALUES ('Email Sending Frequency','1','" + pid + "')");

            if (type == "Insert")//only first time
            {
                SQLQuery.ExecNonQry("INSERT INTO Departments (DepartmentName, EntryBy, ProjectId) VALUES ('Management','" + user + "','" + pid + "')");
                SQLQuery.ExecNonQry("INSERT INTO ExpenseTypes (Name, EntryBy, ProjectId, SystemTypeID) VALUES ('Salary','" + user + "','" + pid + "','1')");
                SQLQuery.ExecNonQry("INSERT INTO ExpenseTypes (Name, EntryBy, ProjectId, SystemTypeID) VALUES ('Agent Commission','" + user + "','" + pid + "','2')");
                SQLQuery.ExecNonQry("INSERT INTO ExpenseTypes (Name, EntryBy, ProjectId, SystemTypeID) VALUES ('Supplier Payment','" + user + "','" + pid + "','3')");
                if (industry == "1")
                {
                    SQLQuery.ExecNonQry("INSERT INTO ExpenseTypes (Name, EntryBy, ProjectId, SystemTypeID) VALUES ('Car / Vehicle','" + user + "','" + pid + "','4')");
                }
                else
                {
                    SQLQuery.ExecNonQry("INSERT INTO ExpenseTypes (Name, EntryBy, ProjectId, SystemTypeID) VALUES ('Contract Job','" + user + "','" + pid + "','4')");
                }
            }
        }



        public static T GetJsonRequest<T>(string requestUrl)
        {
            try
            {
                WebRequest apiRequest = WebRequest.Create(requestUrl);
                HttpWebResponse apiResponse = (HttpWebResponse)apiRequest.GetResponse();

                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    string jsonOutput;
                    using (StreamReader sr = new StreamReader(apiResponse.GetResponseStream()))
                        jsonOutput = sr.ReadToEnd();

                    var jsResult = JsonConvert.DeserializeObject<T>(jsonOutput);

                    if (jsResult != null)
                        return jsResult;
                    else
                        return default(T);
                }
                else
                {
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                // Log error here.

                return default(T);
            }
        }

        public static DataTable ChkPermission(string lName)
        {
            try
            {
                string query = "SELECT distinct  [Id], [Bname] FROM [ECDormitorySetup]  Order by Bname ";

                string level = SQLQuery.ReturnString("Select PermissionLevel from EmployeePermission where EmpId=(SELECT ID FROM ECOfficers WHERE Email='" + lName + "')");
                if (level != "" && lName != "rony")
                {
                    if (level == "All Zone")
                    {
                        query = "SELECT distinct  [Id], [Bname] FROM [ECDormitorySetup]  Order by Bname ";
                    }
                    else if (level == "All Circle")
                    {
                        string zoneId = ReturnString("Select ZoneId from EmployeePermission where EmpId=(SELECT ID FROM ECOfficers WHERE Email='" + lName + "')");
                        query = "SELECT distinct  [Id], [Bname] FROM [ECDormitorySetup] WHERE RoadZone ='" + zoneId + "' Order by Bname ";
                    }
                    else if (level == "All Division")
                    {
                        string circleId = ReturnString("Select CircleId from EmployeePermission where EmpId=(SELECT ID FROM ECOfficers WHERE Email='" + lName + "')");
                        query = "SELECT distinct  [Id], [Bname] FROM [ECDormitorySetup] WHERE RoadCircle='" + circleId + "' Order by Bname ";
                    }
                    else if (level == "All Bungalow")
                    {
                        string divisionId = ReturnString("Select DivisionId from EmployeePermission where EmpId=(SELECT ID FROM ECOfficers WHERE Email='" + lName + "')");
                        query = "SELECT distinct  [Id], [Bname] FROM [ECDormitorySetup] WHERE Division ='" + divisionId + "' Order by Bname ";
                    }
                    else
                    {
                        string bid = ReturnString("Select BungalowId from EmployeePermission where EmpId=(SELECT ID FROM ECOfficers WHERE Email='" + lName + "')");
                        query = "SELECT distinct  [Id], [Bname] FROM [ECDormitorySetup] WHERE ID='" + bid + "' Order by Bname ";
                    }
                }

                return ReturnDataTable(query);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }

    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }

    }


}