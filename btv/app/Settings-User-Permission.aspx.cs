using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;


public partial class app_Settings_User_Permission : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //string query = "SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID> (Select UserLevel from logins where LoginUserName ='" + Page.User.Identity.Name.ToString() + "') ORDER BY [LevelID]";
            ////txtCurrentPosition.Text = SQLQuery.ReturnString("");
            //SQLQuery.PopulateDropDown(query, ddLevelX, "LevelID", "LevelName");
            //ddLevelX.DataBind();
            ddRole.DataBind();
            LoadRolePermission();
            MenuFilter();
        }
    }

    //Package wise menu filter
    private void MenuFilter()
    {
        //    string projectId = SQLQuery.ProjectID(User.Identity.Name);
        //    string package = SQLQuery.ReturnString("Select Package from Projects where VID='" + projectId + "'");
        //    if (package == "1")
        //    {
        //        pnlMarketing.Visible = false;
        //        pnlEmployee.Visible = false;
        //        pnlCoreAcc.Visible = false;
        //        pnlStore.Visible = false;
        //    }
        //    else if (package == "2")
        //    {
        //        pnlMarketing.Visible = true;
        //        pnlEmployee.Visible = true;
        //        pnlCoreAcc.Visible = false;
        //        pnlStore.Visible = false;
        //    }
        //    else if (package == "3")
        //    {
        //        pnlMarketing.Visible = true;
        //        pnlEmployee.Visible = true;
        //        pnlCoreAcc.Visible = false;
        //        pnlStore.Visible = true;
        //    }
        //    else
        //    {
        //        pnlMarketing.Visible = true;
        //        pnlEmployee.Visible = true;
        //        pnlCoreAcc.Visible = true;
        //        pnlStore.Visible = true;
        //    }
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //DropDownList1.DataBind();
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
            lblid.Text = PID.Text;
            Notify("Edit mode activated", "info", lblMsg);

            EditMode();
        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error", lblMsg);
        }
    }

    private void EditMode()
    {
        SqlCommand cmd7 = new SqlCommand("SELECT LoginUserName, EmployeeInfoID, UserLevel FROM [Logins] WHERE LID='" + lblid.Text + "' AND ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd7.Connection.Open();
        SqlDataReader dr = cmd7.ExecuteReader();
        if (dr.Read())
        {
            txtUserX.Text = dr[0].ToString();
            lblEID.Text = dr[1].ToString();
            // ddLevelX.SelectedValue= dr[2].ToString();
            Session["EmployeeId"] = dr[1];
            txtEmployeX.Text = SQLQuery.ReturnString("SELECT Name FROM [Employee] WHERE EmployeeID='" + dr[1].ToString() + "'");
            //if (eid != "")
            //{
            //    ddCounterX.SelectedValue = SQLQuery.ReturnString("SELECT DepartmentID FROM [EmployeeInfo] WHERE EmployeeInfoID='" + dr[1].ToString() + "'");
            //    ddEmployeX.DataBind();
            //    ddEmployeX.SelectedValue = eid;
            //}

            //eid = dr[2].ToString(); //SQLQuery.ReturnString("SELECT UserLevel FROM [Counters] WHERE UserLevel='" + dr[2].ToString() + "'");
            //if (eid != "")
            //{
            //    ddLevelX.SelectedValue = eid;
            //}

        }
        cmd7.Connection.Close();
        chkPermissions();
        ChkOparatePermission();
    }

    private void ChkOparatePermission()
    {
        //SqlCommand cmd = new SqlCommand(@"Select CanInsert, CanUpdate, CanDelete FROM UserOparateLevel Where EmpID='" + Session["EmployeeId"] + "' And ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        SqlCommand cmd = new SqlCommand(@"Select CanInsert, CanUpdate, CanDelete FROM UserOparateLevel Where EmpID='" + ddRole.SelectedValue + "' And ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            string insert = dr[0].ToString();
            if (insert == "1")
            {
                chkInsert.Checked = true;
            }
            else
            {
                chkInsert.Checked = false;
            }

            string update = dr[1].ToString();
            if (update == "1")
            {
                chkUpdate.Checked = true;
            }
            else
            {
                chkUpdate.Checked = false;
            }
            string delete = dr[2].ToString();
            if (delete == "1")
            {
                chkDelete.Checked = true;
            }
            else
            {
                chkDelete.Checked = false;
            }
            //if (insert == "1" || delete == "1" || update == "1")
            //{
            //    CheckBox30.Checked = true;
            //}
            //else
            //{
            //    CheckBox30.Checked = false;
            //}
        }
        cmd.Connection.Close();
    }

    private void chkPermissions()
    {
        // if (isBlocked("Initial Setup", CheckBox17) == "1")
        // {
        //     isBlocked("A/C Setup", CheckBox31);
        //     isBlocked("Others", CheckBox32);
        // }
        // if (isBlocked("Marketing", CheckBox1) != "1")
        // {
        //     isBlocked("Planning", CheckBox2);
        //     isBlocked("Activities", CheckBox3);
        //     isBlocked("Campaigns", CheckBox4);
        // }
        // if (isBlocked("Sales", CheckBox6) != "1")
        // {
        //     isBlocked("Products", CheckBox7);
        //     isBlocked("Customers", CheckBox8);
        //     isBlocked("Planning", CheckBox9);
        // }
        // if (isBlocked("Employee", CheckBox11) != "1")
        // {
        //     isBlocked("Setup", CheckBox12);
        //     isBlocked("Attendance", CheckBox13);
        //     isBlocked("Work", CheckBox14);
        //     /*isBlocked("INVENTORY4", CheckBox15);
        //     isBlocked("INVENTORY5", CheckBox135);*/
        // }
        // if (isBlocked("Store & Inventory", CheckBox5) != "1")
        // {
        //     isBlocked("Warehouses", CheckBox10);
        //     isBlocked("Purchase", CheckBox15);
        //     isBlocked("Store Activities", CheckBox19);
        //     isBlocked("Reports", CheckBox23);
        //     /*isBlocked("INVENTORY4", CheckBox15);
        //     isBlocked("INVENTORY5", CheckBox135);*/
        // }

        // if (isBlocked("Accounts", CheckBox16) != "1")
        // {
        //     isBlocked("Setup", CheckBox17);
        //     isBlocked("Data Entry", CheckBox18);
        //     isBlocked("Report", CheckBox20);
        //     //isBlocked("PRODUCTION4", CheckBox5);
        // }
        // if (isBlocked("Core Accounting", CheckBox21) != "1")
        // {
        //     isBlocked("Setup", CheckBox22);
        //     isBlocked("Voucher", CheckBox24);
        //     isBlocked("Reports", CheckBox25);
        // }
        // if (isBlocked("Maintenance", CheckBox26) != "1")
        // {
        //     isBlocked("Company", CheckBox27);
        //     isBlocked("Notice & Messages", CheckBox28);
        //     isBlocked("User & Security", CheckBox29);
        // }
        // /*if (isBlocked("CRM", CheckBox31) != "1")
        //{
        //    isBlocked("CRM1", CheckBox34);
        //    isBlocked("CR2", CheckBox32);
        //    isBlocked("CR3", CheckBox33);
        //}
        //if (isBlocked("FACTS", CheckBox36) != "1")
        //{
        //    isBlocked("FACTS1", CheckBox37);
        //    isBlocked("FACTS2", CheckBox38);
        //    isBlocked("FACTS3", CheckBox39);
        //}
        //if (isBlocked("ADMIN", CheckBox41) != "1")
        //{
        //    isBlocked("ADMIN1", CheckBox42);
        //    isBlocked("ADMIN2", CheckBox43);
        //    isBlocked("ADMIN3", CheckBox44);
        //    isBlocked("ADMIN4", CheckBox45);
        //    isBlocked("ADMIN5", CheckBox45d);
        //}*/
        // showHide(Panel1, CheckBox1);
        // showHide(Panel2, CheckBox6);
        // showHide(Panel3, CheckBox11);
        // showHide(Panel4, CheckBox16);
        // showHide(Panel5, CheckBox21);
        // showHide(Panel7, CheckBox5);
        // //showHide(Panel7, CheckBox16);
        // //showHide(Panel6, CheckBox31);
        // //showHide(Panel8, CheckBox36);
        // //showHide(Panel9, CheckBox41);

    }
    private string isBlocked(string formName, CheckBox chkBox)
    {
        string user = txtUserX.Text;
        string isBlockEd2 = SQLQuery.ReturnString("Select IsBlocked from UserForms where RoleId='" + user + "' AND  FormName='" + formName + "'AND ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'");
        if (isBlockEd2 == "1")
        {
            chkBox.Checked = false;
        }
        else
        {
            chkBox.Checked = true;
        }
        return isBlockEd2;
    }


    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        //Response.Redirect("New-user.aspx");
    }


    private void setOparatePermission()
    {
        string userId = lblEID.Text;
        string isInsert = "0";
        string isDelete = "0";
        string isUpdate = "0";
        if (chkInsert.Checked)
        {
            isInsert = "1";
        }
        if (chkDelete.Checked)
        {
            isDelete = "1";
        }
        if (chkUpdate.Checked)
        {
            isUpdate = "1";
        }
        if (CheckBox30.Checked)
        {
            string isExist = SQLQuery.ReturnString(@"SELECT EmpID FROM UserOparateLevel WHERE EmpID='" + ddRole.SelectedValue + "'");
            if (isExist=="")
            {
                SQLQuery.ExecNonQry(@"INSERT INTO UserOparateLevel (EmpID, CanInsert, CanUpdate, CanDelete, ProjectId)
                                      VALUES ('" + ddRole.SelectedValue + "', '" + isInsert + "', '" + isUpdate + "', '" + isDelete + "','" + SQLQuery.ProjectID(User.Identity.Name) + "')");
            }
            else
            {
                SQLQuery.ExecNonQry("Update UserOparateLevel Set EmpID='" + ddRole.SelectedValue + "',  CanInsert='" + isInsert + "', CanUpdate='" + isUpdate + "', CanDelete='" + isDelete + "' where EmpID='" + ddRole.SelectedValue + "' And ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'");
            }
            
        }
        else
        {
            SQLQuery.ExecNonQry("Update UserOparateLevel Set CanInsert='0', CanUpdate='0', CanDelete='0' where EmpID='" + ddRole.SelectedValue + "' And ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'");
        }

    }

    protected void ddCounter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //DropDownList ddEmploye = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddEmploye");
        //ddEmploye.DataBind();
    }

    private void showHide(Panel panelName, CheckBox chkBox)
    {
        if (chkBox.Checked)
        {
            panelName.Visible = true;
        }
        else
        {
            panelName.Visible = false;
        }
    }
    protected void CheckBox1_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel1, CheckBox1);
    }

    protected void CheckBox6_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel2, CheckBox6);
    }

    protected void CheckBox11_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel3, CheckBox11);
    }

    protected void CheckBox16_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel4, CheckBox16);
    }

    protected void CheckBox21_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel5, CheckBox21);
    }

    protected void CheckBox26_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel7, CheckBox26);
    }

    protected void CheckBox31_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel6, CheckBox31);
    }

    protected void CheckBox36_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel8, CheckBox36);
    }

    protected void CheckBox41_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel9, CheckBox41);
    }

    protected void CheckBox5_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel7, CheckBox5);
    }

    protected void CheckBox30_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(PanelOparate, CheckBox30);
    }

    protected void CheckBox17_OnCheckedChanged(object sender, EventArgs e)
    {
        //showHide(Panel9, CheckBox17);
    }

    protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
    {

    }

    protected void cb1_CheckedChanged(object sender, EventArgs e)
    {
        foreach (ListViewItem item in ListView1.Items)
        {
            CheckBox cb1 = item.FindControl("cb1") as CheckBox;
            CheckBoxList cblmenuItems = item.FindControl("cblmenuItems") as CheckBoxList;
            if (cb1.Checked)
            {
                cblmenuItems.Visible = true;
            }
            else
            {
                cblmenuItems.Visible = false;
            }

        }
    }


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        if (ddRole.Text != "")
        {
            string user = txtUserX.Text;
            // SQLQuery.ExecNonQry("Update Logins set UserLevel='" + ddLevelX.SelectedValue + "' where LID='" + lblid.Text + "' ");
            //SQLQuery.ExecNonQry("Delete UserForms where UserID='" + txtUserX.Text + "' ");
            SQLQuery.ExecNonQry("Delete UserForms where RoleId='" + ddRole.SelectedValue + "' ");
            //setPermissions();
            setOparatePermission();

            foreach (ListViewItem item in ListView1.Items)
            {
                CheckBox cb1 = item.FindControl("cb1") as CheckBox;
                SetBlock(cb1, "MenuGroup");

                CheckBoxList cblmenuItems = item.FindControl("cblmenuItems") as CheckBoxList;
                foreach (ListItem cbItem in cblmenuItems.Items)
                {
                    string isBlockEd2 = "1";
                    if (cbItem.Selected && cb1.Checked)
                    {
                        isBlockEd2 = "0";
                    }

                    SQLQuery.ExecNonQry("INSERT INTO UserForms (RoleId, FormName, IsBlocked, EntryBy, ProjectId, MenuType) " +
                        "VALUES ('" + ddRole.SelectedValue + "', '" + cbItem.Value + "', '" + isBlockEd2 + "', '" + User.Identity.Name + "','" + SQLQuery.ProjectID(User.Identity.Name) + "', 'Form')");
                }

            }


            Notify("Saved Successfully", "success", lblMsg);
        }
        else
        {
            Notify("Invalid User!", "error", lblMsg);
        }

    }
    private string SetBlock(CheckBox chkBox, string type)
    {
        string user = txtUserX.Text;
        string isBlockEd2 = "0";
        if (!chkBox.Checked)
        {
            isBlockEd2 = "1";
            SQLQuery.ExecNonQry("INSERT INTO UserForms (RoleId, FormName, IsBlocked, EntryBy, ProjectId, MenuType) " +
                "VALUES ('" + ddRole.SelectedValue + "', '" + chkBox.Text + "', '" + isBlockEd2 + "', '" + User.Identity.Name + "','" + SQLQuery.ProjectID(User.Identity.Name) + "', '" + type + "')");
        }
        return isBlockEd2;
    }

    protected void ddRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadRolePermission();
    }

    private void LoadRolePermission()
    {
        ChkOparatePermission();
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT sl, RoleId, FormName, IsBlocked, EntryBy, EntryDate, MenuType FROM UserForms WHERE RoleId='" + ddRole.SelectedValue + "'");
        foreach (ListViewItem item in ListView1.Items)
        {
            CheckBox cb1 = item.FindControl("cb1") as CheckBox;
            CheckBoxList cblmenuItems = item.FindControl("cblmenuItems") as CheckBoxList;
            foreach (ListItem cbItem in cblmenuItems.Items)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["MenuType"].ToString() == "MenuGroup" && cb1.Text == dr["FormName"].ToString())
                    {
                        if (dr["IsBlocked"].ToString() == "1")
                        {
                            cb1.Checked = false;
                            cblmenuItems.Visible = false;
                        }
                        else
                        {
                            cb1.Checked = true;
                            cblmenuItems.Visible = true;
                        }
                    }
                    else if (cbItem.Value == dr["FormName"].ToString())
                    {
                        string isBlocked = SQLQuery.ReturnString(@"SELECT IsBlocked FROM UserForms WHERE RoleId='" + ddRole.SelectedValue + "' AND FormName='" + cbItem.Value + "'");

                        if (dr["IsBlocked"].ToString() == "1")
                        {
                            cbItem.Selected = false;
                        }
                        else
                        {
                            cbItem.Selected = true;
                        }
                    }
                }
            }
        }
    }
}

