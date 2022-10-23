using System;
using System.Collections.Generic;
using System.Data;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
public partial class app_Gateway_Settings : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // LoadElements();
            MailSettigs();
        }
    }

    //Test Emails delivery
    protected void Button1_OnClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT  id, GatewayName, Link, UserName, Password, Port, EnableSSL, MonthlyLimit, MonthlySendQty, TotalSendQty, IsActive
                                                                FROM EmailGateways WHERE (IsActive = '1') AND ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'");

            foreach (DataRow drx in dtx.Rows)
            {
                string val = SQLQuery.SendMarketingEmail(drx["UserName"].ToString(), txtEmail.Text, "Test mail using gateway: " + drx["GatewayName"], "Test Success at: " + DateTime.Now, drx["Link"].ToString(), drx["Port"].ToString(), drx["Password"].ToString(), Convert.ToBoolean(drx["EnableSSL"].ToString()));
                lblMsg.Text += "<br/> " + drx["GatewayName"] + ": " + val + " at " + DateTime.Now;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text += "<br/>" + ex.ToString();
        }
    }

    protected void Button2_OnClick(object sender, EventArgs e)
    {
        if (btnNew.Text == "Add New")
        {
            NewGateway.Visible = true;
            btnNew.Text = "Adding New";
        }
        else
        {
            NewGateway.Visible = false;
            btnNew.Text = "Add New";
        }
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string gatewayName = "";
            string link = "", port = "", enableSSL = "", monthlyLimit = "", monthlySendQty = "", totalSendQty = "", isActive = "", priorityRatio = "", sendingPriority = "";
            if (ddMailSelect.SelectedValue == "1")
            {
                gatewayName = txtUser.Text;
                link = "smtp.gmail.com";
                monthlyLimit = "18000";
                monthlySendQty = "0";
                port = "587";
                enableSSL = "True";
                totalSendQty = "0";
                isActive = "1";
                priorityRatio = "2";
                sendingPriority = "78";
            }
            else if (ddMailSelect.SelectedValue == "2")
            {
                gatewayName = txtUser.Text;
                link = "smtp.mail.yahoo.com";
                monthlyLimit = "20000";
                monthlySendQty = "0";
                port = "587";
                enableSSL = "True";
                totalSendQty = "0";
                isActive = "1";
                priorityRatio = "5";
                sendingPriority = "56";

            }
            else if (ddMailSelect.SelectedValue == "3")
            {
                gatewayName = txtUser.Text;
                link = "smtp-mail.outlook.com";
                monthlyLimit = "9000";
                monthlySendQty = "0";
                port = "25";
                enableSSL = "True";
                totalSendQty = "0";
                isActive = "1";
                priorityRatio = "4";
                sendingPriority = "14";
            }
            else if (ddMailSelect.SelectedValue == "4")
            {
                gatewayName = txtUser.Text;
                link = txtLink.Text;
                monthlyLimit = txtMonth.Text;
                monthlySendQty = TxtMonthSend.Text;
                port = txtPort.Text;
                enableSSL = ddSSL.SelectedItem.Text;
                totalSendQty = txttotal.Text;
                isActive = ddIsActive.SelectedValue;
                priorityRatio = txtRatio.Text;
                sendingPriority = txtPriority.Text;
            }

            SQLQuery.ExecNonQry("Insert into EmailGateways(GatewayName, Link, UserName, Password, Port, EnableSSL, MonthlyLimit, MonthID, MonthlySendQty, TotalSendQty, IsActive, PriorityRatio, SendingPriority, ProjectId) " +
                                    "values('" + gatewayName + "','" + link + "','" + txtUser.Text + "','" + txtPass.Text + "','" + port + "','" + enableSSL + "','" + monthlyLimit + "','" + Convert.ToDateTime(txtMonthId.Text).ToString("yyyy-MM-dd") + "','" + monthlySendQty + "','" + totalSendQty + "','" + isActive + "','" + priorityRatio + "','" + sendingPriority + "','" + SQLQuery.ProjectID(User.Identity.Name) + "')");
            Clear();
            lblMsg.Attributes.Add("class", "xerp_success");
            lblMsg.Text = "Successfully Saved Gateway Informations";

        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = ex.ToString();
        }

    }

    private void Clear()
    {
        txtGatewayName.Text = "";
        TxtMonthSend.Text = "";
        txtEmail.Text = "";
        txtLink.Text = "";
        txtMonth.Text = "";
        txtMonthId.Text = "";
        txtPass.Text = "";
        txtPort.Text = "";
        txtPriority.Text = "";
        txtRatio.Text = "";
        txtUser.Text = "";
        txttotal.Text = "";
    }

    protected void ddMailSelect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            MailSettigs();
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
        }
    }

    private void MailSettigs()
    {
        if (ddMailSelect.SelectedValue == "4")
        {
            pnlMailDetails.Visible = true;
        }
        else
        {
            pnlMailDetails.Visible = false;
        }
        if (ddMailSelect.SelectedValue == "1")
        {
            GmailIns.Visible = true;
        }
        else
        {
            GmailIns.Visible = false;
        }
        if (ddMailSelect.SelectedValue == "2")
        {
            YahoolIns.Visible = true;
        }
        else
        {
            YahoolIns.Visible = false;
        }
    }
}

