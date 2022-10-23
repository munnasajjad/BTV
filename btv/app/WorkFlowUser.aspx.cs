
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_WorkFlowUser : System.Web.UI.Page
{
protected void Page_Load(object sender, EventArgs e)
{
btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
if (!IsPostBack){
bindDDEmployeeID();
txtDate.Text=DateTime.Now.ToString("dd/MM/yyyy");
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
RunQuery.SQLQuery.ExecNonQry(" INSERT INTO WorkFlowUser (EmployeeID, EsclationTime, Remark, Date, Priority, TaskStatus) VALUES ('"+ddEmployeeID.SelectedValue+"', '"+txtEsclationTime.Text+"', '"+txtRemark.Text+"', '"+txtDate.Text+"', '"+txtPriority.Text+"', '')    ");
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
RunQuery.SQLQuery.ExecNonQry(" Update  WorkFlowUser SET EmployeeID= '"+ddEmployeeID.SelectedValue+"',  EsclationTime= '"+txtEsclationTime.Text+"',  Remark= '"+txtRemark.Text+"',  Date= '"+txtDate.Text+"',  Priority= '"+txtPriority.Text+"',  TaskStatus= '' WHERE WorkFlowUserID='"+lblId.Text+"' ");
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
DataTable dt = SQLQuery.ReturnDataTable(" Select WorkFlowUserID, EmployeeID,EsclationTime,Remark,Date,Priority,TaskStatus FROM WorkFlowUser WHERE WorkFlowUserID='"+lblId.Text+"'");
foreach (DataRow dtx in dt.Rows)
{
ddEmployeeID.SelectedValue=dtx["EmployeeID"].ToString();
txtEsclationTime.Text=dtx["EsclationTime"].ToString();
txtRemark.Text=dtx["Remark"].ToString();
txtDate.Text=dtx["Date"].ToString();
txtPriority.Text=dtx["Priority"].ToString();

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
RunQuery.SQLQuery.ExecNonQry(" Delete WorkFlowUser WHERE WorkFlowUserID='"+lblId.Text+"' ");
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
DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM WorkFlowUser");
GridView1.DataSource = dt;
GridView1.DataBind();
}


private void bindDDEmployeeID()
{
SQLQuery.PopulateDropDown("Select Name,EmployeeID from Employee", ddEmployeeID, "EmployeeID", "Name");
}


protected void ddEmployeeID_SelectedIndexChanged(object sender, EventArgs e)
{
GridView1.DataBind();
}



private void ClearControls()
{
txtEsclationTime.Text=""; 
txtRemark.Text=""; 
txtDate.Text=""; 
txtPriority.Text=""; 

}










}
