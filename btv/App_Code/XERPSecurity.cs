using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using RunQuery;

/// <summary>
/// Summary description for SendEmail
/// </summary>

namespace XERPSecurity
{
    public class XERPSecure
    {
        public static int CheckPermissionLevel(string user1)
        {
            string query = SQLQuery.ReturnString("Select UserLevel from logins where LoginUserName ='" + user1 + "'");
            return Convert.ToInt32(query);
        }
        public static int HideMainMenu(string user1, string deptId)
        {
            int toHide = 1;
            int permissionLevel = CheckPermissionLevel(user1);
            if (permissionLevel>2)// Normal User
            {
                string empDept = SQLQuery.ReturnString("Select DepartmentID from EmployeeInfo where EmployeeInfoID =(Select EmployeeInfoID from logins where LoginUserName ='" + user1 + "')");
                if (empDept == deptId)
                {
                    toHide = 0;
                    //menuPanelId.Visible = false;
                }
            }

            return toHide;
        }

        public static void ProcessEmail(string Sender, string Receiver, string Subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("noreply@tviexpress.biz", "TVI Express");

            message.To.Add(new MailAddress("ronyjob@gmail.com"));
            //message.To.Add(new MailAddress("recipient2@foo.bar.com"));
            //message.To.Add(new MailAddress("recipient3@foo.bar.com"));

            //message.CC.Add(new MailAddress("carboncopy@foo.bar.com"));
            message.Subject = "This is my new subject";
            message.Body = "This is the new content<br/>" + DateTime.Now.ToString();
            message.IsBodyHtml = true;

            using (SmtpClient client = new SmtpClient())
            {
                client.Send(message);
            }

        }

        public static bool CheckUserPermission(string PermissionKey, string PermValue)
        {
            int num = 0;
            bool flag = false;
            if ((HttpContext.Current.Session["FormConstant"] != null) && (HttpContext.Current.Session["FunctionDesc"] != null))
            {
                ArrayList list = (ArrayList)HttpContext.Current.Session["FormConstant"];
                ArrayList list2 = (ArrayList)HttpContext.Current.Session["FunctionDesc"];
                string str = "";
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ToString() == PermissionKey)
                    {
                        num = i;
                        str = list2[num].ToString();
                        break;
                    }
                    str = "GA";
                }
                if (str.Contains(PermValue))
                {
                    flag = true;
                }
            }
            return flag;
        }

    }
}