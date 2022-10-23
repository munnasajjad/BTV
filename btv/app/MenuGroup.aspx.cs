
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_MenuGroup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            bindDDIconClass();
            BindGrid();
            GetRoleList();
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }


    private void GetRoleList()
    {
        DataTable dt = SQLQuery.ReturnDataTable("SELECT RoleName, Priority FROM aspnet_Roles");
        cblRoles.DataSource = dt;
        cblRoles.DataValueField = "Priority";
        cblRoles.DataTextField = "RoleName";
        cblRoles.DataBind();

        foreach (ListItem li in cblRoles.Items)
        {
                li.Selected = true;            
        }
    }

    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    RunQuery.SQLQuery.ExecNonQry(" INSERT INTO MenuGroup (GroupName, DesplaySerial, Show, IconClass) VALUES ('" + txtGroupName.Text + "', '" + txtDesplaySerial.Text + "', '" + cbShow.Checked + "', '" + ddIconClass.SelectedValue + "')    ");
                    lblId.Text = SQLQuery.ReturnString("Select MAX(SL) From MenuGroup ");
                    UpdateSecurity();
                    ClearControls();
                    Notify("Successfully Saved...", "success", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
            else
            {
                if (SQLQuery.OparatePermission(lName, "Update") == "1")
                {
                    RunQuery.SQLQuery.ExecNonQry(" Update  MenuGroup SET GroupName= '" + txtGroupName.Text + "',  DesplaySerial= '" + txtDesplaySerial.Text + "',  Show= '" + cbShow.Checked + "',  IconClass= '" + ddIconClass.SelectedValue + "' WHERE Sl='" + lblId.Text + "' ");
                    UpdateSecurity();
                    ClearControls();
                    btnSave.Text = "Save";
                    Notify("Successfully Updated...", "success", lblMsg);
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            BindGrid();
        }
    }
    private void UpdateSecurity()
    {
        //SQLQuery.ExecNonQry("Delete MenuAccessSecurity WHERE MenuGroup='" + lblId.Text + "' ");

        foreach (ListItem li in cblRoles.Items)
        {
            int isChecked = li.Selected ? 1 : 0;
            //SQLQuery.ExecNonQry("INSERT INTO MenuAccessSecurity (IsGranted, RoleID, MenuGroup, EntryBy) VALUES ('" + isChecked + "', '" + li.Value + "', '" + lblId.Text + "', '" + User.Identity.Name + "')  ");            
        }
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                int index = Convert.ToInt32(GridView1.SelectedIndex);
                Label lblEditId = GridView1.Rows[index].FindControl("Label1") as Label;
                lblId.Text = lblEditId.Text;
                
                DataTable dt = SQLQuery.ReturnDataTable(" Select Sl, GroupName,DesplaySerial,Show,IconClass FROM MenuGroup WHERE Sl='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtGroupName.Text = dtx["GroupName"].ToString();
                    txtDesplaySerial.Text = dtx["DesplaySerial"].ToString();
                    if (dtx["IconClass"].ToString() != "")
                    {
                        ddIconClass.SelectedValue = dtx["IconClass"].ToString();
                    }
                    cbShow.Checked = Convert.ToBoolean(dtx["Show"].ToString());
                }
                
                foreach (ListItem li in cblRoles.Items)
                {
                    string isActive = SQLQuery.ReturnString("Select IsGranted from MenuAccessSecurity WHERE RoleID='" + li.Value + "' AND MenuGroup='" + lblId.Text + "'  ");
                    if (isActive == "0")
                    {
                        li.Selected = false;
                    }
                    else
                    {
                        li.Selected = true;
                    }
                }
                
                btnSave.Text = "Update";
                Notify("Edit mode activated ...", "info", lblMsg);
            }
            else
            {
                Notify("You are not eligible to attempt this operation", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ExecNonQry(" Delete MenuGroup WHERE Sl='" + lblId.Text + "' ");
            BindGrid();
            Notify("Successfully Deleted...", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindGrid()
    {
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM MenuGroup");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    private void bindDDIconClass()
    {
        SQLQuery.PopulateDropDown("Select IconName from IconsLibrary", ddIconClass, "IconName", "IconName");
    }


    protected void ddIconClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        txtGroupName.Text = "";
        txtDesplaySerial.Text = "";

    }










}
