
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Role : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            BindGrid();
        }
    }

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
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    if (txtTitle.Text != "")
                    {
                        //RunQuery.SQLQuery.ExecNonQry(" INSERT INTO Role (Title, Description, Status) VALUES ('" + txtTitle.Text + "', '" + txtDescription.Text + "', '1')    ");

                        string appId = SQLQuery.ReturnString(@"SELECT ApplicationId FROM aspnet_Applications");
                        Guid roleId = Guid.NewGuid();
                        RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO aspnet_Roles (ApplicationId, RoleId, RoleName, LoweredRoleName, Description)
                                      VALUES ('" + appId + "', '" + roleId + "','" + txtTitle.Text + "','" + txtTitle.Text.ToLower() + "','" + txtDescription.Text + "')");

                        RunQuery.SQLQuery.ExecNonQry(@"INSERT INTO UserLevel (LevelName, CanRead, CanInsert, CanUpdate, CanDelete, RoleId)
                                      VALUES ('" + txtTitle.Text + "','1','1','1','0','" + roleId + "')");
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);
                    }
                    else
                    {

                    }
                    
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
                    //RunQuery.SQLQuery.ExecNonQry(" Update  Role SET Title= '" + txtTitle.Text + "',  Description= '" + txtDescription.Text + "',  Status= '1' WHERE RoleID='" + lblId.Text + "' ");
                    
                    RunQuery.SQLQuery.ExecNonQry(@"Update aspnet_Roles SET RoleName= '" + txtTitle.Text + "', LoweredRoleName= '" + txtTitle.Text.ToLower() + "',  Description= '" + txtDescription.Text + "' WHERE RoleId='" + lblId.Text + "' ");
                    RunQuery.SQLQuery.ExecNonQry(@"Update UserLevel SET LevelName= '" + txtTitle.Text + "' WHERE RoleId='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(@"Select ApplicationId, RoleId, RoleName, LoweredRoleName, Description FROM aspnet_Roles WHERE RoleId='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtTitle.Text = dtx["RoleName"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();

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
            string isExistRoleId = SQLQuery.ReturnString(@"SELECT RoleId FROM aspnet_UsersInRoles WHERE RoleId='" + lblId.Text + "'");
            if (isExistRoleId == "")
            {
                RunQuery.SQLQuery.ExecNonQry("Delete aspnet_Roles WHERE RoleID='" + lblId.Text + "' ");
                RunQuery.SQLQuery.ExecNonQry("Delete UserLevel WHERE RoleID='" + lblId.Text + "' ");
                BindGrid();
                Notify("Successfully Deleted...", "success", lblMsg);
            }
            else
            {
                Notify("This role already assign!", "warn", lblMsg);
            }


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
        DataTable dt = SQLQuery.ReturnDataTable("SELECT * FROM aspnet_Roles Order By Priority ASC");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        txtTitle.Text = "";
        txtDescription.Text = "";

    }










}
