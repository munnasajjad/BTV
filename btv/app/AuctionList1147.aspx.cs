
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_AuctionList1147 : System.Web.UI.Page
{
protected void Page_Load(object sender, EventArgs e)
{
btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
if (!IsPostBack){
bindDDProductID();
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
RunQuery.SQLQuery.ExecNonQry(" INSERT INTO AuctionList (ProductID, Qty, UnitPrice, TotalPrice, Date, Remark) VALUES ('"+ddProductID.SelectedValue+"', '"+txtQty.Text.Replace("'","''")+"', '"+txtUnitPrice.Text.Replace("'","''")+"', '"+txtTotalPrice.Text.Replace("'","''")+"', '"+txtDate.Text.Replace("'","''")+"', '"+txtRemark.Text.Replace("'","''")+"')    ");
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
RunQuery.SQLQuery.ExecNonQry(" Update  AuctionList SET ProductID= '"+ddProductID.SelectedValue+"',  Qty= '"+txtQty.Text.Replace("'","''")+"',  UnitPrice= '"+txtUnitPrice.Text.Replace("'","''")+"',  TotalPrice= '"+txtTotalPrice.Text.Replace("'","''")+"',  Date= '"+txtDate.Text.Replace("'","''")+"',  Remark= '"+txtRemark.Text.Replace("'","''")+"' WHERE AuctionID='"+lblId.Text+"' ");
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
DataTable dt = SQLQuery.ReturnDataTable(" Select AuctionID, ProductID,Qty,UnitPrice,TotalPrice,Date,Remark FROM AuctionList WHERE AuctionID='"+lblId.Text+"'");
foreach (DataRow dtx in dt.Rows)
{
ddProductID.SelectedValue=dtx["ProductID"].ToString();
txtQty.Text=dtx["Qty"].ToString();
txtUnitPrice.Text=dtx["UnitPrice"].ToString();
txtTotalPrice.Text=dtx["TotalPrice"].ToString();
txtDate.Text=dtx["Date"].ToString();
txtRemark.Text=dtx["Remark"].ToString();

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
RunQuery.SQLQuery.ExecNonQry(" Delete AuctionList WHERE AuctionID='"+lblId.Text+"' ");
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
DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM AuctionList");
GridView1.DataSource = dt;
GridView1.DataBind();
}


private void bindDDProductID()
{
SQLQuery.PopulateDropDown("Select ProductID from Product", ddProductID, "ProductID", "ProductID");
}


protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
{
GridView1.DataBind();
}



private void ClearControls()
{
txtQty.Text=""; 
txtUnitPrice.Text=""; 
txtTotalPrice.Text=""; 
txtDate.Text=""; 
txtRemark.Text=""; 

}










}
