using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_Confirm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = Request.QueryString["ID"];
            string email= Request.QueryString["email"];

            if (id != null && email != null)
            {
                string isExist =
                    SQLQuery.ReturnString(
                        "Select Username from Users WHERE BranchName=(Select EmployeeID from Employee WHERE EmployeeID='" +
                        id + "' AND Email='" + email + "')");
                if (isExist != "")
                {
                    SQLQuery.ExecNonQry(@"UPDATE  aspnet_Membership SET IsApproved='true' WHERE Email='" + email + "'");
                }
            }

        }
    }

    public int LeftCount, RightCount = 0;


    //private void SaveToBoard(string strUserName, string upLine, string side, string trPin, string id)
    //{
    //    try
    //    {
    //        string dnLeaderId = upLine;
    //        string leaderId = upLine;
    //        string sqlQuery = "";

    //        SqlCommand cmd2V = new SqlCommand("SELECT BoardEntryID FROM Board where MemberID='" + leaderId + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
    //        cmd2V.Connection.Open();
    //        int bId = Convert.ToInt32(cmd2V.ExecuteScalar());
    //        cmd2V.Connection.Close();
    //        cmd2V.Connection.Dispose();

    //        //Insert Members Head to Board
    //        //sqlQuery += " " + "INSERT INTO Board (UpperMemberID, MemberID, MemberSide) VALUES ('" + leaderId + "', '" + strUserName + "', '" + side + "')";

    //        //Insert to Rank Table /////////////////////////////////////////////
    //        //sqlQuery += " " + "INSERT INTO Rank (MemberID, BronzeQty) VALUES ('" + strUserName + "', 0)";

    //        //Insert to Point /////////////////////////////////////////////
    //        //sqlQuery += " " + "INSERT INTO Points (MemberID) VALUES  ('" + strUserName + "')";

    //        //Insert to InvCount /////////////////////////////////////////////
    //        sqlQuery += " " + "INSERT INTO InvCount (MemberID) VALUES  ('" + strUserName + "')";

    //        //Insert to Update members /////////////////////////////////////////////
    //        sqlQuery += " " + "Update Members set Password=NULL,SecurityPin=NULL,IsActive='1' WHERE MemberID = '" + id + "' ";
    //        sqlQuery += " " + "Insert into CreatPnl(MemberId, PnlNum, PnlName) VALUES ('" + strUserName + "', '" + 1 + "', 'Main') ";

    //        //Insert to TrPassword /////////////////////////////////////////////
    //        //sqlQuery += " " + "INSERT INTO TrPassword (MemberID,Password) Values('" + strUserName + "','" + trPin + "')";
    //        SQLQuery.ExecNonQry(sqlQuery);
    //        message.InnerHtml = GetMessage("Congratulations! Your Novecx account has been activated.", "success");

    //    }
    //    catch (Exception er)
    //    {

    //        message.InnerHtml = GetMessage("Error: " + er, "danger");
    //    }

    //}
    //private string CreateUser(string memberId, string password)
    //{
    //    string msg = "";
    //    try
    //    {
    //        string email = memberId;
    //        string passwordQuestion = "What is your pass?";

    //        MembershipCreateStatus createStatus;
    //        MembershipUser newUser = Membership.CreateUser(memberId, password, email, passwordQuestion, password, true, out createStatus);
    //        switch (createStatus)
    //        {
    //            case MembershipCreateStatus.Success:
    //                msg = "";
    //                break;
    //            case MembershipCreateStatus.DuplicateUserName:
    //                msg = "There already exists a user with this username." + memberId;
    //                break;

    //            case MembershipCreateStatus.DuplicateEmail:
    //                msg = "There already exists a user with this email address." + memberId;
    //                break;
    //            case MembershipCreateStatus.InvalidEmail:
    //                msg = "There email address you provided in invalid." + memberId;
    //                break;
    //            case MembershipCreateStatus.InvalidAnswer:
    //                msg = "There security answer was invalid." + memberId;
    //                break;
    //            case MembershipCreateStatus.InvalidPassword:
    //                msg = "The password you provided is invalid. It must be seven characters long and have at least one non-alphanumeric character." + memberId;

    //                break;
    //            default:
    //                msg = "There was an unknown error; the user account was NOT created." + memberId;
    //                break;
    //        }
    //        if (msg == "")
    //        {
    //            Roles.AddUserToRole(memberId, "Members");
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    //CreateAccountResults.Text = CreateAccountResults.Text + "<br>" + msg;

    //    return msg;

    //}

    
    private string GetMessage(string message, string type)
    {
        return "<div class='alert alert-" + type + "'>" + message + "</div>";
    }
}