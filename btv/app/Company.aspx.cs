using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Company : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            ddDeveloped.DataBind();
            string lName = User.Identity.Name.ToString().ToLower();

            if (lName == "rony")
            {
                btnSave.Text = "Save";//Only I can insert
                pnlExpDate.Visible = true;
            }
            else
            {
                EditMode(SQLQuery.ProjectID(lName));
            }

            //ddDeveloped.DataBind();
            //ddDeveloped.SelectedValue = SQLQuery.ReturnString("Select Developed from Projects where VID='" + SQLQuery.ProjectID(lName) + "'");
            GridView1.DataBind();

            if (SQLQuery.AuthLevel(lName) == 1)
            {
                GridView1.Visible = true;//

            }

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


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = User.Identity.Name.ToString().ToLower();
            string showHide = "0";
            if (rbShow.Checked)
            {
                showHide = "1";
            }
            var acc = 0;
            var inv = 0;
            var proll = 0;
            if (ddPackage.SelectedValue == "1" || ddPackage.SelectedValue == "2")
            {
                acc = chkAcc.Checked ? 1 : 0;
                inv = chkInventory.Checked ? 1 : 0;
                proll = chkPayroll.Checked ? 1 : 0;
            }
            else if (ddPackage.SelectedValue == "3")
            {
                inv = 1;
            }
            else
            {
                inv = 1;
                acc = 1;
            }

            string dt = Convert.ToDateTime(txtExpDate.Text).ToString("yyyy-MM-dd");
            if (btnSave.Text == "Save")
            {

                SQLQuery.ExecNonQry(@"Insert into Projects (Type, ProjectName, ShowHeader, ReportHeader, NTamount, VAT, TrialDate, DoctorComm, Developed, EntryBy,ProjectId, UserLimit, Accounting, Inventory, Payroll,Package,SMSQouta,IsActive) 
                VALUES ('" + ddIndustry.SelectedValue + "', N'" + txtName.Text + "', '" + showHide + "',N'" + txtAddress.Content + "', '" + ddNT.SelectedValue + "', '" + txtVAT.Text + "', '" + dt + "', '" + ddDocComm.SelectedValue + "', '" +
            ddDeveloped.SelectedValue + "', '" + lName + "','" + SQLQuery.ProjectID(User.Identity.Name) + "', '" + txtLimitQuota.Text + "', '" + acc + "', '" + inv + "', '" + proll + "','" + ddPackage.SelectedValue + "','" + txtSmsQouta.Text + "','" + ddIsActive.SelectedValue + "')   ");
                lblEid.Text = SQLQuery.ReturnString("Select MAX(VID) from Projects");
                SQLQuery.InsertSettings("Insert", lblEid.Text, txtOfcSTime.Text, txtMnthSTarget.Text, User.Identity.Name, ddIndustry.SelectedValue);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Cl", "$('input[type=text]').val('');", true);
                Notify("New Info Added", "success", lblMsg);
            }
            else
            {
                //string lName = Page.User.Identity.Name.ToString();
                if (SQLQuery.OparatePermission(lName, "Update") == "1")
                {
                    if (pnlExpDate.Visible == true)
                    {
                        SQLQuery.ExecNonQry("Update Projects Set Type='" + ddIndustry.SelectedValue + "',  ProjectName=N'" + txtName.Text + "', ShowHeader='" + showHide + "', ReportHeader=N'" + txtAddress.Content + "', NTamount='" + ddNT.SelectedValue + "', VAT='" + txtVAT.Text + "', TrialDate='" + dt + "' , DoctorComm='" + ddDocComm.SelectedValue + "', Developed='" + ddDeveloped.SelectedValue + "', UserLimit='" + txtLimitQuota.Text + "', Accounting='" + acc + "', Inventory='" + inv + "', Payroll='" + proll + "', Package='" + ddPackage.SelectedValue + "', SMSQouta='" + txtSmsQouta.Text + "', IsActive='" + ddIsActive.SelectedValue + "'  Where VID='" + lblEid.Text + "' ");

                    }
                    else
                    {
                        SQLQuery.ExecNonQry("Update Projects Set Type='" + ddIndustry.SelectedValue + "',  ProjectName=N'" + txtName.Text + "', ShowHeader='" + showHide + "', ReportHeader=N'" + txtAddress.Content + "', NTamount='" + ddNT.SelectedValue + "', VAT='" + txtVAT.Text + "', DoctorComm='" + ddDocComm.SelectedValue + "' Where VID='" + lblEid.Text + "' ");

                    }

                    SQLQuery.ExecNonQry("Delete Settings WHERE ProjectId='" + lblEid.Text + "'");
                    SQLQuery.InsertSettings("Update", lblEid.Text, txtOfcSTime.Text, txtMnthSTarget.Text, User.Identity.Name, ddIndustry.SelectedValue);

                    if (lName == "rony")
                    {
                        btnSave.Text = "Save";
                    }
                    Notify("Info Updated...", "success", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation", "warn", lblMsg);
                }
            }

            GridView1.DataBind();

            string linkPath = "../Uploads/Photos/";
            if (FileUpload2.HasFile)
            {
                string photoURL = SQLQuery.UploadImage(txtName.Text, FileUpload2, Server.MapPath("..\\Uploads\\Photos\\"), Server.MapPath(linkPath), linkPath, Page.User.Identity.Name.ToString(), "Company");
                SQLQuery.ExecNonQry("UPDATE Projects SET Logo='" + photoURL + "' where (VID='" + lblEid.Text + "')");
                imgLogo.ImageUrl = SQLQuery.ReturnString("Select PhotoURL from Photos where PhotoID='" + photoURL + "'");
            }

            string projId =
               SQLQuery.ReturnString("Select VID from Projects where ProjectName='" + txtName.Text +
                                     "' And ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "' And Type='" +
                                     ddIndustry.SelectedValue + "'");
            string isDataExists =
                SQLQuery.ReturnString("Select GroupName FROM AccountGroup Where ProjectID='" + projId + "'");
            if (isDataExists == "")
            {
                if (chkAcc.Checked)
                {
                    string script = File.ReadAllText(Server.MapPath(".\\CoreAccBasicsScript.sql"));
                    SQLQuery.ExecNonQry(script);
                    SQLQuery.ExecNonQry("Update AccountGroup Set ProjectID='" + projId + "' where ProjectID='0'");
                    SQLQuery.ExecNonQry("Update Accounts Set ProjectID='" + projId + "' where ProjectID=''");
                    SQLQuery.ExecNonQry("Update ControlAccount Set ProjectID='" + projId + "' where ProjectID=''");
                    SQLQuery.ExecNonQry("Update HeadSetup Set ProjectID='" + projId + "' where ProjectID=''");
                }

            }

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }


    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            //string isExist = SQLQuery.ReturnString("Select TOP(1) DeliveryLocation FROM Sales WHERE DeliveryLocation='" + lblItemCode.Text + "'");

            //if (isExist == "")
            //{
            SQLQuery.ExecNonQry("DELETE Projects WHERE VID='" + lblItemCode.Text + "'");
            Notify("Delete command executed successfully.", "warn", lblMsg);
            //}
            //else
            //{
            //    Notify("ERROR: The location has existing sales record!", "error", lblMsg);
            //}

            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = Convert.ToInt32(GridView1.SelectedIndex);
        Label lblItemName = GridView1.Rows[index].FindControl("Label1") as Label;
        lblEid.Text = lblItemName.Text;
        EditMode(lblEid.Text);
        btnSave.Text = "Update";
        btnclr.Visible = true;
    }

    private void EditMode(string prjId)
    {
        prjId = "1";
        DataTable dtx = SQLQuery.ReturnDataTable(@"SELECT TOP (1) VID, Type, ProjectName, ShowHeader, ReportHeader, NTamount, VAT, Logo, 
TrialDate, DoctorComm, Developed, EntryBy, EntryDate, ProjectId, UserLimit, Accounting, Inventory, Payroll, Package, SMSQouta, IsActive FROM Projects WHERE VID='" + prjId + "'");
        foreach (DataRow drx in dtx.Rows)
        {
            lblEid.Text = prjId;
            ddIndustry.SelectedValue = drx["Type"].ToString();
            txtName.Text = drx["ProjectName"].ToString();
            string showHide = drx["ShowHeader"].ToString();
            rbShow.Checked = false; rbHide.Checked = true;
            if (showHide == "1")
            {
                rbHide.Checked = false;
                rbShow.Checked = true;
            }

            txtAddress.Content = drx["ReportHeader"].ToString();
            imgLogo.ImageUrl = SQLQuery.ReturnString("Select PhotoURL from Photos where PhotoID='" + drx["Logo"].ToString() + "'");
            ddNT.SelectedValue = drx["NTamount"].ToString();
            ddDocComm.SelectedValue = drx["DoctorComm"].ToString();
            ddDeveloped.SelectedValue = drx["Developed"].ToString();
            txtVAT.Text = drx["VAT"].ToString();
            txtLimitQuota.Text = drx["UserLimit"].ToString();
            txtMnthSTarget.Text = SQLQuery.ReturnString("SELECT SettingValue FROM Settings WHERE  SettingName='Monthly Sales Target' AND ProjectID='" + prjId + "'");

            chkAcc.Checked = false;
            chkInventory.Checked = false;
            chkPayroll.Checked = false;
            if (drx["Accounting"].ToString() == "1")
            {
                chkAcc.Checked = true;
            }
            if (drx["Inventory"].ToString() == "1")
            {
                chkInventory.Checked = true;
            }
            if (drx["Payroll"].ToString() == "1")
            {
                chkPayroll.Checked = true;
            }


            DateTime expDt = Convert.ToDateTime(drx["TrialDate"].ToString());
            txtExpDate.Text = expDt.ToString("dd/MM/yyyy");
            int diff = (expDt - DateTime.Today).Days;

            if (diff < 0)
            {
                Notify("Your subscription has been expired " + (diff * (-1)) + " days ago. Please renew.", "error", lblSubscription);
            }
            else if (diff < 7)
            {
                Notify("Your subscription will expire within 0" + diff + " days", "warn", lblSubscription);
            }
            else
            {
                Notify("Your subscription will expire within " + diff + " days", "success", lblSubscription);
            }

            ddPackage.SelectedValue = drx["Package"].ToString();
            txtSmsQouta.Text = drx["SMSQouta"].ToString();
            string activeState = drx["IsActive"].ToString();
            if (activeState == "1")
            {
                ddIsActive.SelectedValue = "1";
            }
            else
            {
                ddIsActive.SelectedValue = "0";
            }

        }
    }

    protected void ddDeveloped_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void ddIndustry_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

    protected void btnclr_OnClick(object sender, EventArgs e)
    {
        string msg = "";
        string connectString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
        // Retrieve the DataSource property.    
        string dbName = builder.InitialCatalog;
        string prjId = lblEid.Text;

        if (prjId != "1")
        {
            SQLQuery.ExecNonQry(
                   @"SELECT UserId, RoleId
                                into #temp
                                FROM  aspnet_UsersInRoles
                                WHERE UserId in (Select UserId from aspnet_Users where LoweredUserName IN (Select LoginID from Employee where ProjectId='" + prjId + "'))" +
                   @"DELETE FROM dbo.aspnet_Membership WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_UsersInRoles WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_Profile WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE UserId IN (Select UserId from #temp)
                                DELETE FROM dbo.aspnet_Users WHERE UserId IN (Select UserId from #temp)");
            //SQLQuery.ExecNonQry("Delete Logins where ProjectID='"+ prjId + "'");
            //SQLQuery.ExecNonQry("Delete Users where ProjectId='" + prjId + "'");
            //SQLQuery.ExecNonQry("Delete Employee where ProjectId='" + prjId + "'");
            DataTable dt = new DataTable();
            dt =
                SQLQuery.ReturnDataTable(
                    "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '" +
                    dbName + "'");
            foreach (DataRow dtx in dt.Rows)
            {
                try
                {
                    string tableName = dtx["TABLE_NAME"].ToString();
                    if (!tableName.StartsWith("aspnet_"))
                    {
                        SQLQuery.ExecNonQry("Delete " + tableName + " Where ProjectId='" + prjId + "'");
                    }
                }
                catch (Exception ex)
                {
                    msg += "</br>" + ex.Message;
                }
            }

            SQLQuery.ExecNonQry("Delete Projects where vid='" + prjId + "'");


            Notify("Company Deleted successfully.", "success", lblMsg);
            GridView1.DataBind();
        }
    }

    protected void ddPackage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddPackage.SelectedValue == "1" || ddPackage.SelectedValue == "2")
        {
            chkInventory.Checked = false;
            chkAcc.Checked = false;
        }
        else if (ddPackage.SelectedValue == "3")
        {
            chkInventory.Checked = true;
            chkAcc.Checked = false;
        }
        else
        {
            chkInventory.Checked = true;
            chkAcc.Checked = true;
        }
    }
}
