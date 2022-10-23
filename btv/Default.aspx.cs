using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
using System.Web.Security;

public partial class _Default : System.Web.UI.Page
{
    public string status;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.Redirect("./login");
            /*
            txtCIDate.Text = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd") + " to " + DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
            txtCOdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtJoinDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            CalendarExtender4.StartDate = DateTime.Today.AddDays(1);
            CalendarExtender4.EndDate = DateTime.Today.AddDays(60);

            ddRzone.DataBind();
            ddRC.DataBind();
            ddDivision.DataBind();
            ddBunalow.DataBind();
            ddName.DataBind();
            pnlStep2.Visible = false;
            //SQLQuery.CancelPendingRequest();
            FormsAuthentication.SignOut();
            //Session["UserName"] = "";
            Session.Abandon();
            //string empId = SQLQuery.ReturnString("SELECT Id FROM BookingMaster WHERE  (Status = 'P') AND (EntryDate <= '"+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"') ");
            */
            //SQLQuery.ExecNonQry("DELETE FROM BookingMaster WHERE  (Status = 'P') AND (EntryDate<='" + DateTime.Now.AddHours(-24) + "')");

            //try
            //{
            //    if (User.Identity.Name != "" && User.Identity.Name != "rony")
            //    {
            //        ddName.SelectedValue = SQLQuery.ReturnString("Select sl from Customer WHERE LoginID='" + User.Identity.Name + "'");
            //        ddName.Enabled = false;
            //    }
            //    LoadInfo();
            //}
            //catch (Exception ex)
            //{
            //    ddName.Enabled = true;
            //}

        }
    }
}
