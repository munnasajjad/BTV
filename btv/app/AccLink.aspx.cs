
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_AccLink : System.Web.UI.Page
{
protected void Page_Load(object sender, EventArgs e)
{
btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
if (!IsPostBack){
bindDDHeadIdDr();
bindDDHeadIdCr();
txtUpdateDate.Text=DateTime.Now.ToString("dd/MM/yyyy");BindGrid();
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
RunQuery.SQLQuery.ExecNonQry(" INSERT INTO AccLink (TransactionType, HeadIdDr, DrHeadName, HeadIdCr, CrHeadName, UpdateDate, UpdateBy, SyncBalanceDr, SyncBalanceCr, IsActive, ParticularId) VALUES ('"+txtTransactionType.Text+"', '"+ddHeadIdDr.SelectedValue+"', '"+txtDrHeadName.Text+"', '"+ddHeadIdCr.SelectedValue+"', '"+txtCrHeadName.Text+"', '"+txtUpdateDate.Text+"', '"+txtUpdateBy.Text+"', '"+txtSyncBalanceDr.Text+"', '"+txtSyncBalanceCr.Text+"', '"+txtIsActive.Text+"', '"+txtParticularId.Text+"')    ");
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
RunQuery.SQLQuery.ExecNonQry(" Update  AccLink SET   TransactionType= '"+txtTransactionType.Text+"',  HeadIdDr= '"+ddHeadIdDr.SelectedValue+"',  DrHeadName= '"+txtDrHeadName.Text+"',  HeadIdCr= '"+ddHeadIdCr.SelectedValue+"',  CrHeadName= '"+txtCrHeadName.Text+"',  UpdateDate= '"+txtUpdateDate.Text+"',  UpdateBy= '"+txtUpdateBy.Text+"',  SyncBalanceDr= '"+txtSyncBalanceDr.Text+"',  SyncBalanceCr= '"+txtSyncBalanceCr.Text+"',  IsActive= '"+txtIsActive.Text+"',  ParticularId= '"+txtParticularId.Text+"', WHERE TID='"+lblId.Text+"' ");
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
DataTable dt = SQLQuery.ReturnDataTable(" Select TID, TransactionType,HeadIdDr,DrHeadName,HeadIdCr,CrHeadName,UpdateDate,UpdateBy,SyncBalanceDr,SyncBalanceCr,IsActive,ParticularId FROM AccLink WHERE TID='"+lblId.Text+"'");
foreach (DataRow dtx in dt.Rows)
{
txtTransactionType.Text=dtx["TransactionType"].ToString();
ddHeadIdDr.SelectedValue=dtx["HeadIdDr"].ToString();
txtDrHeadName.Text=dtx["DrHeadName"].ToString();
ddHeadIdCr.SelectedValue=dtx["HeadIdCr"].ToString();
txtCrHeadName.Text=dtx["CrHeadName"].ToString();
txtUpdateDate.Text=dtx["UpdateDate"].ToString();
txtUpdateBy.Text=dtx["UpdateBy"].ToString();
txtSyncBalanceDr.Text=dtx["SyncBalanceDr"].ToString();
txtSyncBalanceCr.Text=dtx["SyncBalanceCr"].ToString();
txtIsActive.Text=dtx["IsActive"].ToString();
txtParticularId.Text=dtx["ParticularId"].ToString();

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
RunQuery.SQLQuery.ExecNonQry(" Delete AccLink WHERE TID='"+lblId.Text+"' ");
ClearControls();
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
DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM AccLink");
GridView1.DataSource = dt;
GridView1.DataBind();
}


private void bindDDHeadIdDr()
{
SQLQuery.PopulateDropDown("Select EntryID,AccountsHeadID from HeadSetup", ddHeadIdDr, "AccountsHeadID", "EntryID");
}


protected void ddHeadIdDr_SelectedIndexChanged(object sender, EventArgs e)
{
GridView1.DataBind();
}


private void bindDDHeadIdCr()
{
SQLQuery.PopulateDropDown("Select EntryID,AccountsHeadID from HeadSetup", ddHeadIdCr, "AccountsHeadID", "EntryID");
}


protected void ddHeadIdCr_SelectedIndexChanged(object sender, EventArgs e)
{
GridView1.DataBind();
}



private void ClearControls()
{
txtTransactionType.Text=""; 
txtDrHeadName.Text=""; 
txtCrHeadName.Text=""; 
txtUpdateDate.Text=""; 
txtUpdateBy.Text=""; 
txtSyncBalanceDr.Text=""; 
txtSyncBalanceCr.Text=""; 
txtIsActive.Text=""; 
txtParticularId.Text=""; 

}










}
