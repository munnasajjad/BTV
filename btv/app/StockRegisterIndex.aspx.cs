
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_StockRegisterIndex : System.Web.UI.Page
{
protected void Page_Load(object sender, EventArgs e)
{
btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
if (!IsPostBack){
bindDDStockRegID();
bindDDStoreID();
bindDDProducID();
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
RunQuery.SQLQuery.ExecNonQry(" INSERT INTO StockRegisterIndex (StockRegID, StoreID, ProducID, Remarks) VALUES ('"+ddStockRegID.SelectedValue+"', '"+ddStoreID.SelectedValue+"', '"+ddProducID.SelectedValue+"', '"+txtRemarks.Text+"')    ");
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
RunQuery.SQLQuery.ExecNonQry(" Update  StockRegisterIndex SET StockRegID= '"+ddStockRegID.SelectedValue+"',  StoreID= '"+ddStoreID.SelectedValue+"',  ProducID= '"+ddProducID.SelectedValue+"',  Remarks= '"+txtRemarks.Text+"' WHERE StockRegIndexID='"+lblId.Text+"' ");
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
DataTable dt = SQLQuery.ReturnDataTable(" Select StockRegIndexID, StockRegID,StoreID,ProducID,Remarks FROM StockRegisterIndex WHERE StockRegIndexID='"+lblId.Text+"'");
foreach (DataRow dtx in dt.Rows)
{
ddStockRegID.SelectedValue=dtx["StockRegID"].ToString();
ddStoreID.SelectedValue=dtx["StoreID"].ToString();
ddProducID.SelectedValue=dtx["ProducID"].ToString();
txtRemarks.Text=dtx["Remarks"].ToString();

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
RunQuery.SQLQuery.ExecNonQry(" Delete StockRegisterIndex WHERE StockRegIndexID='"+lblId.Text+"' ");
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
DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM StockRegisterIndex");
GridView1.DataSource = dt;
GridView1.DataBind();
}


private void bindDDStockRegID()
{
SQLQuery.PopulateDropDown("Select StockRegID from StockRegister", ddStockRegID, "StockRegID", "StockRegID");
}


protected void ddStockRegID_SelectedIndexChanged(object sender, EventArgs e)
{
GridView1.DataBind();
}


private void bindDDStoreID()
{
SQLQuery.PopulateDropDown("Select StoreID from Store", ddStoreID, "StoreID", "StoreID");
}


protected void ddStoreID_SelectedIndexChanged(object sender, EventArgs e)
{
GridView1.DataBind();
}


private void bindDDProducID()
{
SQLQuery.PopulateDropDown("Select Name,ProductID from Product", ddProducID, "ProductID", "Name");
}


protected void ddProducID_SelectedIndexChanged(object sender, EventArgs e)
{
GridView1.DataBind();
}



private void ClearControls()
{
txtRemarks.Text=""; 

}










}
