using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using RunQuery;
using XERPSecurity;
using Microsoft.AspNet.FriendlyUrls;
using DocumentFormat.OpenXml.Office.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.IO;
using System.Web.Security;

public partial class AdminCentral_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                bool isSubscribed = true;
                string lName = Page.User.Identity.Name.ToString();

                GenerateMainMenu();
                generatePageElements();
                Branding_Settings();
                string sessionID = SQLQuery.ProjectID(lName);
                Session["ProjectID"] = sessionID;
                lblProjectID.Text = sessionID;
                //GetNotification();
                SQLQuery.GenerateNotification(Repeater3, lblPOrder, Page.User.Identity.Name);
                //SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + Page.User.Identity.Name.ToString() + "'");
                //SQLQuery.CancelPendingRequest();
                if (string.IsNullOrEmpty(sessionID) || string.IsNullOrEmpty(lName))
                {
                    Session.Abandon();
                    Session.Contents.RemoveAll();
                    System.Web.Security.FormsAuthentication.SignOut();
                    Response.Redirect("../Login");
                }
                else
                {
                    imgPhoto.ImageUrl = SQLQuery.ReturnString("SELECT  '.'+PhotoURL FROM Photos WHERE PhotoID=(SELECT PhotoID FROM Employee WHERE EmployeeID = (SELECT EmployeeInfoID FROM  Logins WHERE(LoginUserName= '" + lName + "')))");

                    //Image2.ImageUrl = RunQuery.SQLQuery.ReturnString("Select '.'+PhotoURL from Photos WHERE PhotoID=(Select Photo from Employee where LoginID='" + lname + "')");
                    //LoadMenu();
                    LoadPermission();

                    //string branch = SQLQuery.ReturnString("Select ProjectName from Projects where VID=(Select ProjectID from Logins where LoginUserName='" +                            lName + "')");
                    string title = Page.Title;
                    if (string.IsNullOrEmpty(title))
                    {
                        //Page.Title = lName + " : " + branch;
                    }

                    //CheckSecurity();
                    //Verify Subscription
                    //DateTime expDt = Convert.ToDateTime(SQLQuery.ReturnString("Select TrialDate from Projects where VID='" +SQLQuery.ProjectID(lName) + "'"));
                    //int diff = (expDt - DateTime.Today).Days;

                    //if (diff < 0 && lName != "")
                    //{
                    //    Notify("Your subscription has been expired " + (diff * (-1)) + " days ago. Please renew.", "error", lblSubscription);

                    //    if (!Page.Request.Path.Contains("Default"))
                    //    {
                    //        if (lName != "rony")
                    //        {
                    //            Response.Redirect("Default", false);
                    //        }
                    //    }
                    //    isSubscribed = false;
                    //    SQLQuery.ExecNonQry("Update Projects set Status='Expired' where VID='" + sessionID + "'");
                    //}
                    //else if (diff < 7 && lName != "rony")
                    //{
                    //    Notify("Your subscription will expire within 0" + diff + " days", "warn", lblSubscription);
                    //}


                }
                ChangeCaptions();                
                string pageNames = Path.GetFileName(Request.Path);
                chkFormAccess(pageNames);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblSubscription);
            //Response.Redirect("../Login?msg=" + ex.ToString());
        }
        finally
        {
            string isSkiped =
                SQLQuery.ReturnString("Select IsSkiped FROM Employee Where LoginID='" + Page.User.Identity.Name + "'");
            if (isSkiped != "0")
            {
                FirstTimeOptions();
                string isShown = Request.QueryString["showvid"];
                if (isShown != "")
                {
                    VideoShow(isShown);
                }
            }
        }
    }

    private void chkFormAccess(string pageName)
    {     
        string[] role = Roles.GetRolesForUser(Page.User.Identity.Name);        
        string roleId = SQLQuery.ReturnString(@"SELECT LevelID FROM UserLevel WHERE LevelName='" + role[0] + "'");
        string isBlockEd2 = SQLQuery.ReturnString("SELECT IsBlocked FROM UserForms WHERE FormName IN(Select sl from MenuStructure where PageName='" + pageName.Trim() + "') AND RoleId='" + roleId + "'");
        if (isBlockEd2 == "0")
        {
            Response.Redirect("Default", true);
        }
    }

    private void GetNotification()
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
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') AND (WorkFlowUser.EsclationEndTime >= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'LV') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + Page.User.Identity.Name + "')))");

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
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') AND (WorkFlowUser.EsclationEndTime >= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'SIR') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + Page.User.Identity.Name + "')))");


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
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') AND (WorkFlowUser.EsclationEndTime >= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'RV') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + Page.User.Identity.Name + "')))");


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
                                            Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') AND (WorkFlowUser.EsclationEndTime >= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'GRN') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + Page.User.Identity.Name + "')))");


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
                         Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID WHERE  ((WorkFlowUser.EsclationStartTime <= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') AND (WorkFlowUser.EsclationEndTime >= '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "') OR PermissionStatus='HOLD') AND (WorkFlowUser.TaskStatus = '1') AND (WorkFlowUser.IsActive = '1') AND (WorkFlowUser.WorkFlowType = 'TV') AND (DesignationWithEmployee.EmployeeID = (SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + Page.User.Identity.Name + "')))");


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
        lblPOrder.Text = dt.Rows.Count.ToString();

        Repeater3.DataSource = dt;
        Repeater3.DataBind();
        //Repeater1.DataBind();
        //Repeater2.DataBind();
    }

    private bool IsNotify(DateTime escStartDate, DateTime escEndDate)
    {
        bool isShow = false;
        if (escStartDate <= escEndDate && escStartDate >= DateTime.Today)
        {
            // = SQLQuery.IsBooked(roomNo, chkDateFrom);
            //if (bookingDetailId != "")
            //{
            //    break;
            //}
            //escStartDate = escStartDate.AddDays(1);
            isShow = false;
        }
        return isShow;
    }


    private void GenerateMainMenu()
    {
        string urlPage = "";
        //foreach (string segment in HttpRequestExtensions.GetFriendlyUrlSegments(Request.FilePath)) Maisha Kalam Sunny
        //{
        //    urlPage = segment;
        //}
        string[] urlParts = Request.FilePath.Split('/');
        foreach (string word in urlParts)
        {
            if (!string.IsNullOrEmpty(word))
            {
                urlPage = word.Split('?')[0]; //to ignore query string
            }
        }


        string txt = "";
        string userRoleId = SQLQuery.ReturnString(@"SELECT UserLevel.LevelID, UserLevel.LevelName FROM aspnet_Users INNER JOIN
                         aspnet_UsersInRoles ON aspnet_Users.UserId = aspnet_UsersInRoles.UserId INNER JOIN
                         aspnet_Roles ON aspnet_UsersInRoles.RoleId = aspnet_Roles.RoleId INNER JOIN
                         UserLevel ON UserLevel.RoleId = aspnet_Roles.RoleId WHERE aspnet_Users.UserName='" + Page.User.Identity.Name + "'");

        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT sl, GroupName, DesplaySerial, Show, IconClass FROM MenuGroup Where Show='True' Order by DesplaySerial ");
        foreach (DataRow drx in dt.Rows)
        {
            string grp = drx["GroupName"].ToString();

            string isGranted = SQLQuery.ReturnString("Select IsBlocked from UserForms WHERE FormName='" + drx["GroupName"] + "' AND RoleID='" + userRoleId + "'  ");

            if (isGranted != "0")
            {
                //MenuGroup
                if (grp == "Dashboard")
                {
                    txt += "<li><a href='./'><span class='nav_icon computer_imac'></span>Dashboard</a></li>";
                }
                else
                {
                    string isExist = SQLQuery.ReturnString("SELECT  PageName   FROM MenuStructure WHERE PageName='" + urlPage + "' AND MenuGroup='" + grp + "'   ");
                    if (isExist != "")
                    {
                        txt += "<li class='expand'><a href='#' class='active'><span class='nav_icon " + drx["IconClass"] + "'></span>" + drx["GroupName"] + "<span class='up_down_arrow'>&nbsp;</span></a>";
                        txt += "<ul class='acitem'>";
                    }
                    else
                    {
                        txt += "<li><a href='#'><span class='nav_icon " + drx["IconClass"] + "'></span>" + drx["GroupName"] + "<span class='up_down_arrow'>&nbsp;</span></a>";
                        txt += "<ul class='acitem'>";
                    }
                }
                //Forms
                DataTable dt2 = SQLQuery.ReturnDataTable(@"SELECT sl, TableName, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority, EntryBy, EntryDate, ProjectId FROM MenuStructure Where MenuGroup='" + drx["GroupName"] + "' AND Show='True' Order by Priority");
                foreach (DataRow drx2 in dt2.Rows)
                {
                    string page = drx2["PageName"].ToString();

                    string isPageGranted = SQLQuery.ReturnString("Select IsBlocked from UserForms WHERE FormName='" + drx2["sl"] + "' AND RoleID='" + userRoleId + "'  ");

                    if (isPageGranted != "0")
                    {
                        if (page == urlPage)
                        {
                            txt += "<li class='xerp_curr'><a href='" + page + "'><span class='list-icon'>&nbsp;</span> " + drx2["FormName"] + "</a></li>";
                        }
                        else
                        {
                            txt += "<li><a href='" + page + "'><span class='list-icon'>&nbsp;</span> " + drx2["FormName"] + "</a></li>";
                        }
                    }
                }
                if (drx["GroupName"].ToString() != "Dashboard")
                {
                    txt += "</li></ul>";
                }
            }
        }
        ltrMainMenu.Text = txt;
    }

    /*
    private void GenerateMainMenu()
    {
        string urlPage = "";
        string[] filePath = Request.FilePath.Split('/');
        foreach (string segment in filePath)// HttpRequestExtensions.GetFriendlyUrlSegments(Request))
        {
            urlPage = segment;
        }
            string txt = "";
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT GroupName, DesplaySerial, Show FROM MenuGroup Where Show='True' Order by DesplaySerial ");
        foreach (DataRow drx in dt.Rows)
        {
            if (drx["GroupName"].ToString() == "Dashboard")
            {
                txt += "<li><a href='./'><span class='nav_icon computer_imac'></span>Dashboard</a></li>";
            }
            else
            {
                string isCurrent = SQLQuery.ReturnString("Select MenuGroup from MenuStructure WHERE MenuGroup='"+ drx["GroupName"] + "' AND PageName='" + urlPage + "' ");
                if (isCurrent == "")
                {
                    txt += "<li><a href='#'><span class='nav_icon cog_4'></span>" + drx["GroupName"] +
                           "<span class='up_down_arrow'>&nbsp;</span></a>";
                }
                else //Current menu
                {
                    txt += "<li class='expand'><a href='#' class='active'><span class='nav_icon cog_4'></span>" + drx["GroupName"] +
                           "<span class='up_down_arrow'>&nbsp;</span></a>";
                }
                txt += "<ul class='acitem'>";
            }
            
            DataTable dt2 = SQLQuery.ReturnDataTable(@"SELECT TableName, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority, EntryBy, EntryDate, ProjectId FROM MenuStructure Where MenuGroup='"+ drx["GroupName"] + "' AND Show='True' Order by Priority");
            foreach (DataRow drx2 in dt2.Rows)
            {
                if (drx2["PageName"].ToString() == urlPage)
                {
                    txt += "<li class='xerp_curr'><a href='" + drx2["PageName"] + "'><span class='list-icon'>&nbsp;</span> " + drx2["FormName"] + "</a></li>";
                }
                else
                {
                    txt += "<li><a href='" + drx2["PageName"] + "'><span class='list-icon'>&nbsp;</span> " + drx2["FormName"] + "</a></li>";
                }
            }
            if (drx["GroupName"].ToString() != "Dashboard")
            {
                txt += "</li></ul>";
            }

        }
        ltrMainMenu.Text = txt;
    }*/

    private void ChangeCaptions()
    {
        //litCustomer.Text = SQLQuery.FindCaption("Customers", lblProject.Text);
        //litAgent.Text = SQLQuery.FindCaption("Agents", lblProject.Text);
        //litPrdGrp.Text = SQLQuery.FindCaption("Products", lblProject.Text);
        //ltrProduct.Text = SQLQuery.FindCaption("Products", lblProject.Text);
        //ltrJob.Text = SQLQuery.FindCaption("Contract", SQLQuery.ProjectID(Page.User.Identity.Name))+" Info";
    }
    private void MonthlyProcess(string loginId, bool subscribed)
    {
        string projectId = SQLQuery.ProjectID(loginId);
        //string isPrjIdExist =SQLQuery.ReturnString("Select ProjectId from SMSBalance Where ProjectId='" + projectId + "'");
        //if (isPrjIdExist != "")
        //{
        string oneMonthAgo = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
        string desc = "Balance Added: " + DateTime.Now.ToString("MMyyyy");
        string isExist = SQLQuery.ReturnString("Select ISNULL(EntryDate,0) from SMSBalance WHERE Description='" + desc + "' AND ProjectId='" + projectId + "' AND EntryDate>='" + oneMonthAgo + "'");

        if (isExist == "" && subscribed)
        {
            int pkgQty = Convert.ToInt32(SQLQuery.ReturnString("Select SMSQouta from Projects where VID='" + projectId + "'"));
            string firstEntryDate = SQLQuery.ReturnString("Select convert(datetime,MIN(EntryDate),120) from SMSBalance WHERE ProjectId='" + projectId + "'");
            if (firstEntryDate == "")
            {
                firstEntryDate = DateTime.Now.ToString("yyyy-MM-dd");
            }

            DateTime chkDate = Convert.ToDateTime(firstEntryDate);

            int balance = 0; //Convert.ToInt32(SQLQuery.ReturnString("Select SUM(InQty)-SUM(OutQty) from SMSBalance WHERE ProjectId='" + projectId + "' AND EntryDate>='" + chkDate + "'AND EntryDate<'" + chkDate.AddMonths(1) + "'"));
            while (chkDate < DateTime.Today)
            {
                int usedQty = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(SUM(OutQty),0) from SMSBalance WHERE ProjectId='" + projectId + "' AND Type='Used' AND EntryDate>='" + chkDate.ToString("yyyy-MM-dd") + "'"));
                int monthQty = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(SUM(InQty),0) from SMSBalance WHERE ProjectId='" + projectId + "' AND Type='Monthly SMS' AND EntryDate>='" + chkDate.ToString("yyyy-MM-dd") + "'AND EntryDate<'" + chkDate.AddMonths(1).ToString("yyyy-MM-dd") + "'"));
                int purchQty = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(SUM(InQty),0) from SMSBalance WHERE ProjectId='" + projectId + "' AND Type='Purchase' AND EntryDate>='" + chkDate.ToString("yyyy-MM-dd") + "'AND EntryDate<'" + chkDate.AddMonths(1).ToString("yyyy-MM-dd") + "'"));

                //balance += inQty;
                if (purchQty > 0) //Purchased
                {
                    if (monthQty >= usedQty)
                    {
                        balance += purchQty;
                    }
                    else
                    {
                        balance += (purchQty - (usedQty - monthQty));
                    }
                }
                else if (usedQty > monthQty)
                {
                    balance -= (usedQty - monthQty);
                }

                chkDate = chkDate.AddMonths(1);
            }

            int outQty = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(SUM(InQty)-SUM(OutQty),0) from SMSBalance WHERE ProjectId='" + projectId + "'")) - balance;
            SQLQuery.ExecNonQry("Insert Into SMSBalance (ProjectId, Description, OutQty, Type, EntryBy)VALUES('" + projectId + "','Monthly SMS balance clearing','" + outQty + "','Unused SMS','" + loginId + "')");
            SQLQuery.ExecNonQry("Insert Into SMSBalance (ProjectId, Description, InQty, Type, EntryBy)VALUES('" + projectId + "','" + desc + "','" + pkgQty + "','Monthly SMS','" + loginId + "')");
            //}
        }

    }



    private void FirstTimeOptions()
    {
        string is1stTime = Convert.ToString(Session["firstTime"]);
        if (string.IsNullOrEmpty(is1stTime))
        {
            //pnl1stVideo.Visible = true;
            mask.Visible = false;

            Session["firstTime"] = "No";
        }
    }


    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void LinkButtonx_Click(object sender, EventArgs e)
    {
        try
        {
            // Set current datetime to last login datetime
            string lName = Page.User.Identity.Name.ToString();
            int timeDifference = Convert.ToInt32(SQLQuery.ReturnString("Select ServerTimeDiffMins from SettingsDateTime where sid='1'"));
            DateTime curTimeLocal = DateTime.Now.AddMinutes(timeDifference);


            SqlCommand cmdxx = new SqlCommand("Select CurrentLoginTime from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdxx.Connection.Open();
            DateTime lDate = Convert.ToDateTime(cmdxx.ExecuteScalar());
            cmdxx.Connection.Close();
            cmdxx.Connection.Dispose();

            DateTime outTime = DateTime.Now;
            TimeSpan timeDiff = outTime.Subtract(lDate);
            string stayTime = String.Format("{0}:{1}:{2}", timeDiff.Hours, timeDiff.Minutes, timeDiff.Seconds);

            //Updating Login Datetime
            SqlCommand cmd = new SqlCommand("update LoginHistory set OutTime=@OutTime, WorkingTimeHr=@wh WHERE LID=(Select IsNull(Max(LID),0) from LoginHistory WHERE MemberID =@LName)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Parameters.Add("@OutTime", SqlDbType.DateTime).Value = outTime;
            cmd.Parameters.Add("@wh", SqlDbType.VarChar).Value = stayTime;
            cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = lName;
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            cmd.Connection.Dispose();


            //Attendance Entry==================================================
            // string inTime = SQLQuery.ReturnString("Select InTime from Attendance where EmployeeID='" + lName + "' AND  Date='" +
            //                           curTimeLocal.ToString("yyyy-MM-dd") + "'");

            // TimeSpan diff = curTimeLocal - Convert.ToDateTime(inTime);
            // double hours = diff.TotalHours;
            // SQLQuery.ExecNonQry(@"UPDATE [Attendance] SET
            //[OutTime]='" + curTimeLocal.ToString("yyyy-MM-dd  HH:mm:ss") + "' , [WorkingTimeHr]='" + hours + "'   where EmployeeID='" + lName +
            //                     "' AND  Date='" + curTimeLocal.ToString("yyyy-MM-dd") + "'");

        }
        catch (Exception ex)
        {

        }

        Session.Abandon();
        Session.Contents.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("../Login");
    }

    private void CheckSecurity()
    {
        if (SQLQuery.AuthLevel(Page.User.Identity.Name) > 2)//Official Reserved forms
        {
            //ResetPassword.Attributes.Add("class", "hidden");//Reset Password

            //Li22.Attributes.Add("class", "hidden");//data backup
            //FormAuthorization(2);
        }
        else if (SQLQuery.AuthLevel(Page.User.Identity.Name) > 3)// Max Super User
        {
            //NewUser.Attributes.Add("class", "hidden");// Create Profile
            //Li22.Attributes.Add("class", "hidden");
            //FormAuthorization(3);
        }
        else
        {
            //Unlock.Visible = true;
            //ResetPassword.Visible = true;
        }
    }
    private void FormAuthorization(int maxLevelPermitted)
    {
        if (SQLQuery.AuthLevel(Page.User.Identity.Name) > maxLevelPermitted)
        {
            Response.Redirect("./Default?msg=unauthorized");
        }
    }

    private void generatePageElements()
    {
        //Get Branch/Centre Name
        string lName = Page.User.Identity.Name.ToString();
        lblUser.Text = lName;
        /*
        string DoctorComm = SQLQuery.ReturnString("Select DoctorComm from Projects where VID=(Select ProjectID from Logins where LoginUserName='" + lName + "')");
        if (DoctorComm == "0")
        {
            //ReferrerDoctors.Attributes.Add("class", "hidden");
            EmployeeSalaryReport.Attributes.Add("class", "hidden");
        }
        */
        SqlCommand cmdxxz = new SqlCommand("Select ISnull(count(vid),0) from BillingMaster where InvoiceDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxxz.Connection.Open();
        //lblPOrder.Text = Convert.ToString(cmdxxz.ExecuteScalar());
        cmdxxz.Connection.Close();
        cmdxxz.Connection.Dispose();

        SqlCommand cmdxxz1 = new SqlCommand("Select ISnull(count(MsgID),0) from Messaging where Receiver='" + lName + "' AND IsRead=0", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxxz1.Connection.Open();
        lblPendingMsg.Text = Convert.ToString(cmdxxz1.ExecuteScalar());
        cmdxxz1.Connection.Close();
        cmdxxz1.Connection.Dispose();

        // Set current datetime to last login datetime        
        SqlCommand cmdx = new SqlCommand("Select LastLoginDate from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdx.Connection.Open();
        string lDate = Convert.ToString(cmdx.ExecuteScalar());
        cmdx.Connection.Close();
        cmdx.Connection.Dispose();

        if (lDate == "")
        {
            lblLogedIn.Text = "Never Logged-in";
        }
        else
        {
            lblLogedIn.Text = lDate;
        }

        string type = SQLQuery.ReturnString("Select Type from Projects where VID=(Select ProjectID from Logins where LoginUserName='" + lName + "')");

        //if (type == "Lab")
        //{
        //    ltrCategories.Text = "Test Categories";
        //    ltrNames.Text = "Test Names";
        //}
        //else if (type == "Physiotherapy")
        //{
        //    ltrCategories.Text = "Service Categories";
        //    ltrNames.Text = "Service Names";
        //}
        //else
        //{
        //    ltrCategories.Text = "Service Categories";
        //    ltrNames.Text = "Service Names";
        //}
    }



    private void Branding_Settings()
    {
        try
        {
            string sid = SQLQuery.ReturnString("Select Developed from  Projects where VID='" + SQLQuery.ProjectID(Page.User.Identity.Name.ToString()) + "'");

            SqlCommand cmd = new SqlCommand("SELECT TOP (1) sid, DevelopedBy, ProviderAddress, LoginLogo, InnerLogo, SoftwareName, SoftwareMode, ProviderURL, TrialDate FROM settings_branding where sid='" + sid + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //string sid = dr[0].ToString();
                string provider = dr[1].ToString();
                //string addr = dr[2].ToString();
                //string logo = dr[3].ToString();
                string logo = dr[4].ToString();
                string sName = dr[5].ToString();

                string sMode = dr[6].ToString();
                string url = dr[7].ToString();
                string tDate = dr[8].ToString();

                ltrDeveloper.Text = "&copy;" + DateTime.Now.Year.ToString() + " <a href='" + url + "' target='_blank'>" + provider + "</a>";
                imgLogo.ImageUrl = "../branding/" + logo;
                imgLogo.AlternateText = sName + " by " + provider;

            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        catch (Exception ex)
        { }
    }

    private void LoadPermission()
    {

        //XERP User Security Model
        //------------------------
        //1. Super Admin: All access
        //2. Admin: CRUD, Only cant create admin account, Menu access assignment
        //3. Super User: CRUD except delete, no admin feature access
        //4. User: Can Write & read, no update/delete
        //5. Guest: only reports for specific department features
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            int permissionLevel = XERPSecure.CheckPermissionLevel(lName);
            if (permissionLevel > 3) // Normal User
            {
                //Admin.Attributes.Add("class", "hidden");

                //Meny Access Permission For Lower then admin accounts
                //---------------
                //2	Sales
                //3	Purchase
                //4	Store & Inventory
                //5	Production
                //6	Accounts
                //7	HRM & Payroll

                //XERPSecure.HideMainMenu(lName, "2", SalesMenuPanel);
                //XERPSecure.HideMainMenu(lName, "3", PurchaseMenuPanel);
                //XERPSecure.HideMainMenu(lName, "4", InventoryMenuPanel);
                //XERPSecure.HideMainMenu(lName, "5", ProductionMenuPanel);
                //XERPSecure.HideMainMenu(lName, "6", AccountsMenuPanel);
                if (XERPSecure.HideMainMenu(lName, "2") == 1)
                {
                    //salesMenu.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "3") == 1)
                {
                    //purchaseMenu.Attributes.Add("class", "hidden");
                }

                //if (XERPSecure.HideMainMenu(lName, "4") == 1)
                //{
                //    Inventory.Attributes.Add("class", "hidden");
                //}

                if (XERPSecure.HideMainMenu(lName, "5") == 1)
                {
                    //Production.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "6") == 1)
                {
                    //Accounts.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "7") == 1)
                {
                    //Payroll.Attributes.Add("class", "hidden");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }


    public void LoadMenu()
    {
        if (Page.Request.Path.Contains("Default"))
        {
            //dashboard.Attributes.Add("class", "start active");
        }
        /*
                else if (Page.Request.Path.Contains("ECTrainerType"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    //Trainer.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("ECTrainerType");
                }
                else if (Page.Request.Path.Contains("EC_FacultySetup"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    //cFaculties.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_FacultySetup");
                }
                else if (Page.Request.Path.Contains("Road_Zone"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    Zone.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Road_Zone");
                }
                else if (Page.Request.Path.Contains("Division_Road_Circle"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    DRC.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Division_Road_Circle");
                }
                else if (Page.Request.Path.Contains("Road_Circle"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    Circle.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Road_Circle");
                }
                else if (Page.Request.Path.Contains("EC_Designation"))
                {
                    li1.Attributes.Add("class", "expand");
                    Designation.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Designation");
                }
                //else if (Page.Request.Path.Contains(""))
                //{
                //    liSetUp.Attributes.Add("class", "expand");
                //    DataUpload.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("");
                //}
                //else if (Page.Request.Path.Contains("EC_EditAttendance"))
                //{
                //    liSetUp.Attributes.Add("class", "expand");
                //    EditAttn.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("EC_EditAttendance");
                //}
                //else if (Page.Request.Path.Contains("EC_UploadData"))
                //{
                //    liSetUp.Attributes.Add("class", "expand");
                //    DataUpload.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("EC_UploadData");
                //}
                //else if (Page.Request.Path.Contains("EC_AttendanceHistory"))
                //{
                //    liSetUp.Attributes.Add("class", "expand");
                //    AttnHistory.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("EC_AttendanceHistory");
                //}
                else if (Page.Request.Path.Contains("EmailTemplatesForCustomers"))
                {
                    Admin.Attributes.Add("class", "expand");
                    ServiceEmail.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EmailTemplatesForCustomers");
                }
                else if (Page.Request.Path.Contains("EC_TrainingCategories"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //TrainCat.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_TrainingCategories");
                }


                else if (Page.Request.Path.Contains("EC_RestoreProgram"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //RestoreProgram.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_RestoreProgram");
                }


                else if (Page.Request.Path.Contains("EC_Officer_Setup"))
                {
                    li1.Attributes.Add("class", "expand");
                    OfficeStaff.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Officer_Setup");
                }
                else if (Page.Request.Path.Contains("AssignBungalow"))
                {
                    li1.Attributes.Add("class", "expand");
                    Li2.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("AssignBungalow");
                }

                else if (Page.Request.Path.Contains("EC_Trainers"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    //TrainerSet.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Trainers");
                }

                else if (Page.Request.Path.Contains("EC_Trainer_App"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    //Tapproved.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Trainer_App");
                }

                else if (Page.Request.Path.Contains("EC_Participants_App"))
                {
                    liSetUp.Attributes.Add("class", "expand");
                    //Tapproved.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Participants_App");
                }

                //else if (Page.Request.Path.Contains("Billing"))
                //{
                //    liAccounts.Attributes.Add("class", "expand");
                //    string type = Convert.ToString(Request.QueryString["type"]);

                //    if (string.IsNullOrEmpty(type))
                //    {
                //        SalesEntry.Attributes.Add("class", "xerp_curr");
                //}
                //else
                //{
                //    SalesEdit.Attributes.Add("class", "xerp_curr");
                //}

                //chkFormAccess("Billing");
                //}

                else if (Page.Request.Path.Contains("SMSPurchase"))
                {
                    Admin.Attributes.Add("class", "expand");
                    SMSBal.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("SMSPurchase");
                }

                else if (Page.Request.Path.Contains("EC_ListbyBranches"))
                {
                    liAccounts.Attributes.Add("class", "expand");
                    ListByBranch.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_ListbyBranches");
                }
                else if (Page.Request.Path.Contains("EC_ProgramsbyParticipants"))
                {
                    liAccounts.Attributes.Add("class", "expand");
                    li8.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_ProgramsbyParticipants");
                }
                else if (Page.Request.Path.Contains("EC_RoomAllotmentByPrograms"))
                {
                    liAccounts.Attributes.Add("class", "expand");
                    li9.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_RoomAllotmentByPrograms");
                }

                else if (Page.Request.Path.Contains("EC_LetterTemplates"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //LTemplate.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_LetterTemplates");
                }
                else if (Page.Request.Path.Contains("Booking_App"))
                {
                    Sales.Attributes.Add("class", "expand");
                    BookApp.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Booking_App");
                }

                //else if (Page.Request.Path.Contains("Booking"))
                //{
                //    Sales.Attributes.Add("class", "expand");
                //    RoomAloocation.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("Booking");
                //}

                else if (Page.Request.Path.Contains("EC_Dormitory_Setup"))
                {
                    Sales.Attributes.Add("class", "expand");
                    BungalowSetup.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Rooms_Setup");
                }
                else if (Page.Request.Path.Contains("List_of_Bungalow"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //LOB.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("List_of_Bungalow");
                }
                else if (Page.Request.Path.Contains("RoomTypes"))
                {
                    Sales.Attributes.Add("class", "expand");
                    Li3.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("RoomTypes");
                }
                else if (Page.Request.Path.Contains("EC_Rooms_Setup"))
                {
                    Sales.Attributes.Add("class", "expand");
                    RoomSetup.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Rooms_Setup");
                }

                else if (Page.Request.Path.Contains("Billing"))
                {
                    Sales.Attributes.Add("class", "expand");
                    chkOut.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Rooms_Setup");
                }
                else if (Page.Request.Path.Contains("Booking-History"))
                {
                    Sales.Attributes.Add("class", "expand");
                    BookingHistory.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Booking-History");
                }
                else if (Page.Request.Path.Contains("EC_UploadParticipants"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //Li3.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_UploadParticipants");
                }
                else if (Page.Request.Path.Contains("Guest_Info"))
                {
                    Sales.Attributes.Add("class", "expand");
                    Li4.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Guest_Info");
                }
                else if (Page.Request.Path.Contains("EC_Assign_training_programs"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //Li5.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Assign_training_programs");
                }
                else if (Page.Request.Path.Contains("ECBatchAssign"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //BAss.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("ECBatchAssign");
                }
                else if (Page.Request.Path.Contains("EC_Print_Invitation_letter"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //Li6.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_Print_Invitation_letter");
                }
                else if (Page.Request.Path.Contains("EC_ParticipantAttendance"))
                {
                    Sales.Attributes.Add("class", "expand");
                    //Li7.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EC_ParticipantAttendance");
                }

                else if (Page.Request.Path.Contains("ResetPassword"))
                {
                    Admin.Attributes.Add("class", "expand");
                    ResetPassword.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("ResetPassword");
                }
                else if (Page.Request.Path.Contains("Unlock"))
                {
                    Admin.Attributes.Add("class", "expand");
                    Unlock.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Unlock");
                }
                else if (Page.Request.Path.Contains("EmailChange"))
                {
                    Admin.Attributes.Add("class", "expand");
                    EmailChange.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("EmailChange");
                }


                else if (Page.Request.Path.Contains("Company"))
                {
                    //Admin.Attributes.Add("class", "expand");
                    //Company.Attributes.Add("class", "xerp_curr");
                    //chkFormAccess("Company");
                }
                else if (Page.Request.Path.Contains("Terms-and-Conditions"))
                {
                    Admin.Attributes.Add("class", "expand");
                    termsConditions.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Payment");
                }
                else if (Page.Request.Path.Contains("SMS-Template"))
                {
                    Admin.Attributes.Add("class", "expand");
                    smsTemplate.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("SMS-Template");
                }
                else if (Page.Request.Path.Contains("Email-Templates"))
                {
                    Admin.Attributes.Add("class", "expand");
                    EmailTemplate.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Email-Templates");
                }
                //else if (Page.Request.Path.Contains("SMSGateway"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    SmsGateway.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("SMSGateway");
                //}
                //else if (Page.Request.Path.Contains("GeneralSettings"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    GSetting.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("GeneralSettings");
                //}
                //else if (Page.Request.Path.Contains("Settings-Forms"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    SettingsForms.Attributes.Add("class", "xerp_curr");
                //}
                else if (Page.Request.Path.Contains("Post-News"))
                {
                    Admin.Attributes.Add("class", "expand");
                    PostNews.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Post-News");
                }
                else if (Page.Request.Path.Contains("Activity-Log"))
                {
                    Admin.Attributes.Add("class", "expand");
                    activityLog.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Activity-Log");
                }
                //else if (Page.Request.Path.Contains("NewsEdit"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    NewsEdit.Attributes.Add("class", "xerp_curr");
                //}
                //else if (Page.Request.Path.Contains("All-News"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    AllNews.Attributes.Add("class", "xerp_curr");
                //}
                //else if (Page.Request.Path.Contains("Check-Msg"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    CheckMsg.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("Check-Msg");
                //}
                else if (Page.Request.Path.Contains("New-User"))
                {
                    Admin.Attributes.Add("class", "expand");
                    NewUser.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("New-User");
                }

                //else if (Page.Request.Path.Contains("Documents-Upload"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    DocumentsUpload.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("Documents-Upload");
                //}
                //else if (Page.Request.Path.Contains("Gateway-Settings"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    EmailSettings.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("Gateway-Settings");
                //}

                //else if (Page.Request.Path.Contains("GeneralSettings"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    GSetting.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("GeneralSettings");
                //}

                else if (Page.Request.Path.Contains("Backup"))
                {
                    Admin.Attributes.Add("class", "expand");
                    DatabaseBackups.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Backup");
                    FormAuthorization(2);
                }
                else if (Page.Request.Path.Contains("Control-User-Access"))
                {
                    Admin.Attributes.Add("class", "expand");
                    ControlUserAccess.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Control-User-Access");
                    FormAuthorization(2);
                }

                else if (Page.Request.Path.Contains("Notice-Board"))
                {
                    Admin.Attributes.Add("class", "expand");
                    PostNews.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Notice-Board");
                }
                //else if (Page.Request.Path.Contains("Post-Message"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    PostMessage.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("Post-Message");
                //}

                //else if (Page.Request.Path.Contains("Check-Msg"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    CheckMsg.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("Check-Msg");
                //}
                //else if (Page.Request.Path.Contains("Support"))
                //{
                //    Admin.Attributes.Add("class", "expand");
                //    Support.Attributes.Add("class", "xerp_curr");
                //    chkFormAccess("Support");
                //}
                else if (Page.Request.Path.Contains("New-User"))
                {
                    Admin.Attributes.Add("class", "expand");
                    NewUser.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("New-User");
                    FormAuthorization(3);
                }
                else if (Page.Request.Path.Contains("Profile"))
                {
                    Admin.Attributes.Add("class", "expand");
                    SettingsUserPermission.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Profile");
                }
                else if (Page.Request.Path.Contains("Unlock"))
                {
                    Admin.Attributes.Add("class", "expand");
                    Unlock.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Unlock");
                }
                else if (Page.Request.Path.Contains("FormUserLevelSecurity"))
                {
                    Admin.Attributes.Add("class", "expand");
                    FRMSecurity.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("FormUserLevelSecurity");
                }
                else if (Page.Request.Path.Contains("Permission-Level"))
                {
                    Admin.Attributes.Add("class", "expand");
                    UserPer.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Permission-Level");
                }
                else if (Page.Request.Path.Contains("MenuStructure"))
                {
                    Admin.Attributes.Add("class", "expand");
                    MenuSrtcr.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("MenuStructure");
                }
                else if (Page.Request.Path.Contains("Settings-User-Permission"))
                {
                    Admin.Attributes.Add("class", "expand");
                    MenuSecurity.Attributes.Add("class", "xerp_curr");
                    chkFormAccess("Settings-User-Permission");
                }



                string pid = SQLQuery.ProjectID(Page.User.Identity.Name);
                //chkPermissions();
                chkMenuAccessPerm();
                //PackageStatus(pid);
                chkAdonsPermission();
                GeneralSettings(pid);
        */
    }

    private void OnFeedBackClick()
    {
        string host = HttpContext.Current.Request.Url.AbsoluteUri;
        string urlx = "./" + "FeedBack?url=" + host;
        Response.Redirect(urlx);
    }

    private string isMainMenuBlocked(string formName, HtmlGenericControl ctrl)
    {
        string user = Page.User.Identity.Name;
        string isBlockEd2 = SQLQuery.ReturnString("Select IsBlocked from UserForms where UserID='" + user + "' AND  FormName='" + formName + "'AND ProjectId='" + SQLQuery.ProjectID(user) + "'");
        if (isBlockEd2 == "1")
        {
            ctrl.Attributes.Remove("class");
            ctrl.Attributes.Add("class", "hidden");
        }
        return isBlockEd2;
    }
    /*
    private void chkPermissions()
    {
        if (isBlocked("Marketing", Marketing) != "1")
        {
            isBlocked("Planning", sub1);
            isBlocked("Activities", sub2);
            isBlocked("Campaigns", sub3);
        }
        if (isBlocked("Sales", Sales) != "1")
        {
            isBlocked("Products", sub4);
            isBlocked("Customers", sub5);
            isBlocked("Planning", sub6);
        }
        if (isBlocked("Employee", EmployeeM) != "1")
        {
            isBlocked("Setup", sub7);
            isBlocked("Attendance", sub8);
            isBlocked("Work", sub9);
        }

        if (isBlocked("Store & Inventory", Inventory) != "1")
        {
            isBlocked("Warehouses", Span2);
            isBlocked("Purchase", Span1);
            isBlocked("Store Activities", Span3);
            isBlocked("Reports", sub14);
        }

        if (isBlocked("Accounts", liAccounts) != "1")
        {
            isBlocked("Setup", sub10);
            isBlocked("Data Entry", sub11);
            isBlocked("Report", sub12);
            //isBlocked("PRODUCTION4", CheckBox5);
        }
        if (isBlocked("Core Accounting", Accounts) != "1")
        {
            isBlocked("Setup", sub17);
            isBlocked("Voucher", sub18);
            isBlocked("Reports", sub19);
        }
        if (isBlocked("Maintenance", Admin) != "1")
        {
            isBlocked("Company", sub20);
            isBlocked("Notice & Messages", sub21);
            isBlocked("User & Security", sub22);
        }

        //////// Menu Items Check for hiding
        isMenuItemBlocked("Chart of Accounts", AccountsChartReport);
        isMenuItemBlocked("Income Statement", IncomeStatement);
        isMenuItemBlocked("Trial Balance", TrialBalance);
        isMenuItemBlocked("Balance Sheet", BalanceSheet);
        isMenuItemBlocked("Bank Book", BankBook);
        isMenuItemBlocked("Sub A/C Ledger", LedgerSub);
        isMenuItemBlocked("Control A/C Ledger", LedgerControl);
        isMenuItemBlocked("A/C Head Ledger", LedgerHead);
        isMenuItemBlocked("Trade Receivables", accTrRec);
        isMenuItemBlocked("Collection Summery", Li54);
        isMenuItemBlocked("Control A/C Summery", Li55);
        //isMenuItemBlocked("Control A/C Balance", Li55x);
        //isMenuItemBlocked("Profit by Month", Li56);
        isMenuItemBlocked("Cancelled Vouchers", CancelledVouchers);
        isMenuItemBlocked("Voucher by Date", ALLEntries);

    } */





    private string isAdonsBlocked(string columnName, HtmlGenericControl ctrl)
    {
        string user = Page.User.Identity.Name;
        string projectId = SQLQuery.ProjectID(user);
        string colmName = "";
        if (columnName == "Core Accounting")
        {
            colmName = "Accounting";
        }
        if (columnName == "Store & Inventory")
        {
            colmName = "Inventory";
        }
        string isBlockEd = SQLQuery.ReturnString("Select " + colmName + " from Projects where VID='" + projectId + "'");
        if (isBlockEd == "0")
        {
            ctrl.Attributes.Remove("class");
            ctrl.Attributes.Add("class", "hidden");
        }
        return isBlockEd;
    }

    private void chkAdonsPermission()
    {
        string lname = Page.User.Identity.Name.ToString();
        if (lname != "rony")
        {
            string isExists = SQLQuery.ReturnString("Select Accounting from Projects where VID='" + SQLQuery.ProjectID(Page.User.Identity.Name) + "'");
            if (isExists == "1")
            {
                //Adjustment.Attributes.Remove("class");
                //Adjustment.Attributes.Add("class", "hidden");
            }
        }

    }

    private void GeneralSettings(string pid)
    {
        string serviceType =
            SQLQuery.ReturnString("Select SalesServiceType FROM generalSettings where ProjectId='" +
                                  pid + "'");
        if (serviceType != "2")
        {

        }
        string barcode =
            SQLQuery.ReturnString("Select Barcode FROM generalSettings where ProjectId='" +
                                  pid + "'");


        string Agent = SQLQuery.ReturnString("Select Agent FROM generalSettings where ProjectId='" + pid + "'");
        if (Agent != "1")
        {
            //AgentsSuppliers.Attributes.Remove("class");
            //AgentsSuppliers.Attributes.Add("class", "hidden");
        }
    }

    //private void PackageStatus(string pid)
    //{
    //    string package= SQLQuery.ReturnString("Select Package from Projects where VID='" + pid + "'");
    //    string type= SQLQuery.ReturnString("Select Type from Projects where VID='" + pid + "'");
    //    if (package=="1")
    //    {

    //        if (type=="9")
    //        {

    //            CustomerRequirements.Attributes.Remove("class");
    //            CustomerRequirements.Attributes.Add("class", "hidden");
    //            PendingReq.Attributes.Remove("class");
    //            PendingReq.Attributes.Add("class", "hidden");
    //        }
    //    }
    //    //Job/ Contract/ Project/ Vehicle Based:
    //    string isContractBased = SQLQuery.ReturnString("Select IsContracts FROM TargetIndustries WHERE Id ='" + type + "'");
    //    if (isContractBased == "1") //Project/Job/Car/Contract based
    //    {
    //        DataTable dt = SQLQuery.ReturnDataTable("SELECT TOP (5) VID, InvoiceNo + '  <br>' + CustomerName AS  OrderDetail, 'Billing_Job?type=edit&&inv='+ InvoiceNo as link FROM BillingMaster Where ProjectID='" + pid + "' ORDER BY vid DESC");
    //        Repeater1.DataSource = dt;



    //        GOther.Visible = false;
    //    }
    //    else 
    //    {
    //       DataTable dt = SQLQuery.ReturnDataTable("SELECT TOP (5) VID, InvoiceNo + '  <br>' + CustomerName AS  OrderDetail, 'Billing?type=edit&&inv='+ InvoiceNo as link FROM BillingMaster Where ProjectID='" + pid + "' ORDER BY vid DESC");
    //        Repeater1.DataSource = dt;

    //    }
    //}




    /*
private void chkMenuAccessPerm()
{
    if (isMainMenuBlocked("Initial Setup", liSetUp) != "1")
    {
        //isMainMenuBlocked("A/C Setup", Span5);
        //isMainMenuBlocked("Others", Span6);
    }

    if (isMainMenuBlocked("Sales", Sales) != "1")
    {
        //isMainMenuBlocked("Products", sub4);
        //isMainMenuBlocked("Customers", sub5);
        //isMainMenuBlocked("Planning", sub6);
    }

    if (isMainMenuBlocked("Accounts", liAccounts) != "1")
    {
        //isMainMenuBlocked("Setup", sub10);
        isMainMenuBlocked("Data Entry", sub11);
        isMainMenuBlocked("Report", sub12);
        //isMainMenuBlocked("PRODUCTION4", CheckBox5);
    }

    if (isMainMenuBlocked("Maintenance", Admin) != "1")
    {
        //isMainMenuBlocked("Company", sub20);
        //isMainMenuBlocked("Notice & Messages", sub21);
        //isMainMenuBlocked("User & Security", sub22);
    }

    DataTable dt = SQLQuery.ReturnDataTable(@"SELECT sl, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Show, EntryBy, EntryDate
        FROM MenuStructure WHERE sl IN (Select MenuItemID FROM [FormAccessSecurity] where  UserID='" + Page.User.Identity.Name + "' AND ProjectId='" + SQLQuery.ProjectID(Page.User.Identity.Name) + "')");

    foreach (DataRow dr in dt.Rows)
    {
        string toBlock = dr["HTMLControlID"].ToString();
        HtmlGenericControl ctrl = FindControl(toBlock) as HtmlGenericControl;

        if (ctrl != null)
        {
            ctrl.Visible = false;
        }
    }

}*/

    protected void btnFeed_OnServerClick(object sender, EventArgs e)
    {
        OnFeedBackClick();
    }

    //protected void btnSkip_onclick(object sender, EventArgs e)
    //{
    //    string user = Page.User.Identity.Name;
    //    string maxId = SQLQuery.ReturnString(@"SELECT MAX(LID) FORM LoginHistory where MemberID='" + user + "'");
    //    SQLQuery.ExecNonQry("UPDATE LoginHistory SET IsSkiped='1' where LID='" + maxId + "' and IsSkiped='0'");
    //    pnl1stVideo.Visible = false;
    //}

    private void VideoShow(string isShow)
    {
        string empId = SQLQuery.ReturnString("Select sl from Employee where LoginID='" + Page.User.Identity.Name + "'");
        //SQLQuery.ExecNonQry("Update Employee Set IsSkiped='" + isShow + "' where sl='" + empId + "'");
    }
}

