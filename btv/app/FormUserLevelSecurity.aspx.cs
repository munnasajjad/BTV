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
public partial class app_FormUserLevelSecurity : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick",
            " disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            ddUser.DataBind();
            lblUser.Text = Page.User.Identity.Name.ToString();
            lblProjectID.Text = SQLQuery.ProjectID(lblUser.Text);

            //ddFormGroup.DataBind();
            GroupLoad();
            ddSubGroup.DataBind();
            GetFormList();
            GridView1.DataBind();
        }
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');",
            true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SQLQuery.ExecNonQry("Delete FormAccessSecurity WHERE MenuSubGroup='" + ddSubGroup.SelectedValue +
                                "' AND UserID='" + ddUser.SelectedValue + "'       ");
            foreach (ListItem li in cblForms.Items)
            {
                if (!li.Selected)
                {
                    SQLQuery.ExecNonQry(
                        "INSERT INTO FormAccessSecurity (UserID, MenuGroup, MenuSubGroup, MenuItemID, show, EntryBy,ProjectId) VALUES('" +
                        ddUser.SelectedValue + "', '" + ddFormGroup.SelectedValue + "', '" +
                        ddSubGroup.SelectedValue + "', '" + li.Value + "','0','" + User.Identity.Name + "','" +
                        SQLQuery.ProjectID(User.Identity.Name) + "')");
                }
            }

            Notify("Access permission has been saved!", "success", lblMsg);

            btnSave.Text = "Save";
            GridView1.DataBind();
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }

    }

    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetFormList();
        GridView1.DataBind();
    }

    protected void ddFormGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddSubGroup.DataBind();
        GetFormList();
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label PID = GridView1.Rows[index].FindControl("Label1") as Label;
            lblID.Text = PID.Text;

            EditMode();
            btnSave.Text = "Update";
            lblMsg.Attributes.Add("class", "xerp_warn");
            Notify("Edit mode activated ...", "info", lblMsg);
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.ToString();
        }
    }

    private void EditMode()
    {
        DataTable dt =
            SQLQuery.ReturnDataTable(@"Select TOP(1) sl, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority, show, EntryBy, EntryDate
                            FROM MenuStructure WHERE SL='" + lblID.Text + "' AND ProjectId='" +
                                     SQLQuery.ProjectID(User.Identity.Name) + "'");
        foreach (DataRow dr in dt.Rows)
        {
            ddSubGroup.DataBind();
            ddSubGroup.SelectedValue = dr["MenuSubGroup"].ToString();
            string act = dr["show"].ToString();
        }

    }

    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;

            SQLQuery.ExecNonQry("Delete MenuStructure WHERE sl='" + lblItemCode.Text + "'");
            GridView1.DataBind();
            lblMsg.Attributes.Add("class", "xerp_warning");

            Notify("Entry removed successfully ...", "success", lblMsg);
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void GetFormList()
    {
        DataTable dt = SQLQuery.ReturnDataTable("Select sl, FormName from MenuStructure where MenuSubGroup='" +
                                                ddSubGroup.SelectedValue + "' ");
        cblForms.DataSource = dt;
        cblForms.DataValueField = "sl";
        cblForms.DataTextField = "FormName";
        cblForms.DataBind();

        foreach (ListItem li in cblForms.Items)
        {
            string isActive =
                SQLQuery.ReturnString("Select show from FormAccessSecurity WHERE MenuItemID='" + li.Value +
                                      "' AND UserID='" + ddUser.SelectedValue + "'  ProjectId='" +
                                      SQLQuery.ProjectID(User.Identity.Name) + "'     ");
            if (isActive == "0")
            {
                li.Selected = false;
            }
            else
            {
                li.Selected = true;
            }
        }
    }

    protected void ddUser_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        GetFormList();
    }

    private void GroupLoad()
    {
        string projectId = SQLQuery.ProjectID(User.Identity.Name);
        string package = SQLQuery.ReturnString("Select Package from Projects where VID='" + projectId + "'");
        if (package == "1")
        {
            string coreAcc = SQLQuery.ReturnString("Select Accounting from Projects where VID='" + projectId + "'");
            string inventory = SQLQuery.ReturnString("Select Inventory from Projects where VID='" + projectId + "'");
            if (coreAcc == "1")
            {
                SQLQuery.PopulateDropDown(
                "SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE MenuGroup<>'Employee' And MenuGroup<>'Marketing' And MenuGroup<>'Store & Inventory' And ([show] = '1')",
                ddFormGroup, "MenuGroup", "MenuGroup");
            }
            else if (inventory == "1")
            {
                SQLQuery.PopulateDropDown(
                "SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE MenuGroup<>'Employee' And MenuGroup<>'Marketing' And MenuGroup<>'Core Accounting' And ([show] = '1')",
                ddFormGroup, "MenuGroup", "MenuGroup");
            }
            else if (coreAcc == "1" && inventory == "1")
            {
                SQLQuery.PopulateDropDown(
                "SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE MenuGroup<>'Employee' And MenuGroup<>'Marketing' And ([show] = '1')",
                ddFormGroup, "MenuGroup", "MenuGroup");
            }
            else
            {
                SQLQuery.PopulateDropDown(
                "SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE MenuGroup<>'Employee' And MenuGroup<>'Marketing' And MenuGroup<>'Core Accounting' And MenuGroup<>'Store & Inventory' And ([show] = '1')",
                ddFormGroup, "MenuGroup", "MenuGroup");
            }
        }
        else if (package == "2")
        {
            SQLQuery.PopulateDropDown(
                "SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE MenuGroup<>'Core Accounting' And MenuGroup<>'Store & Inventory' And ([show] = '1')",
                ddFormGroup, "MenuGroup", "MenuGroup");
        }
        else if (package == "3")
        {
            SQLQuery.PopulateDropDown(
                "SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE MenuGroup<>'Core Accounting' And ([show] = '1')",
                ddFormGroup, "MenuGroup", "MenuGroup");
        }
        else if (package == "4")
        {
            SQLQuery.PopulateDropDown(
                "SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE ([show] = '1')",
                ddFormGroup, "MenuGroup", "MenuGroup");
        }
    }
}
