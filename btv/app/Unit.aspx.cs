
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_Unit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
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
                    string isExist = SQLQuery.ReturnString("SELECT Name FROM Unit WHERE Name='"+txtName.Text.Trim()+"'");
                    if (isExist=="")
                    {
                        RunQuery.SQLQuery.ExecNonQry(" INSERT INTO Unit (Name, Description, Value) VALUES (N'" + txtName.Text.Replace("'", "''") + "', N'" + txtDescription.Text.Replace("'", "''") + "', '" + txtValue.Text + "')    ");
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);
                    }
                    else
                    {
                        Notify("This unit name already exists!", "warn", lblMsg);
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
                    RunQuery.SQLQuery.ExecNonQry(" Update  Unit SET Name= N'" + txtName.Text.Replace("'", "''") + "',  Description= N'" + txtDescription.Text.Replace("'", "''") + "',  Value= '" + txtValue.Text + "' WHERE UnitID='" + lblId.Text + "' ");
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select UnitID, Name,Description,Value FROM Unit WHERE UnitID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    txtName.Text = dtx["Name"].ToString();
                    txtDescription.Text = dtx["Description"].ToString();
                    txtValue.Text = dtx["Value"].ToString();

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
            RunQuery.SQLQuery.ExecNonQry(" Delete Unit WHERE UnitID='" + lblId.Text + "' ");
            ClearControls();
            Notify("Successfully Deleted...", "success", lblMsg);
            BindGrid();
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
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM Unit Order by UnitID Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }



    private void ClearControls()
    {
        txtName.Text = "";
        txtDescription.Text = "";
        txtValue.Text = "";

    }










}
