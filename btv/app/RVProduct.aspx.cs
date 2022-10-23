
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_RVProduct : System.Web.UI.Page
{
//protected void Page_Load(object sender, EventArgs e)
//{
//btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
//if (!IsPostBack){
//bindDDRVNo();
//bindDDProductID();
//BindGrid();
//}
//}

//private void Notify(string msg, string type, Label lblNotify)
//{
//ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
////Types: success, info, warn, error
//lblNotify.Attributes.Add("class", "xerp_" + type);
//lblNotify.Text = msg;
//}


//protected void btnSave_OnClick(object sender, EventArgs e)
//{
//try
//{
//string lName = Page.User.Identity.Name.ToString();
//if (btnSave.Text == "Save")
//{
//if (SQLQuery.OparatePermission(lName, "Insert") == "1")
//{
//RunQuery.SQLQuery.ExecNonQry(" INSERT INTO RVProduct (RVNo, ProductID, ProductDescription, ReturnQTY, ProductReceive, IssueDescription, UnitPrice, DepositalAccount) VALUES ('"+ddRVNo.SelectedValue+"', '"+ddProductID.SelectedValue+"', '"+txtProductDescription.Text+"', '"+txtReturnQTY.Text+"', '"+txtProductReceive.Text+"', '"+txtIssueDescription.Text+"', '"+txtUnitPrice.Text+"', '"+txtDepositalAccount.Text+"')    ");
//ClearControls();
//Notify("Successfully Saved...", "success", lblMsg);
//}
//else
//{
//Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
//}
//}
//else
//{
//if (SQLQuery.OparatePermission(lName, "Update") == "1")
//{
//RunQuery.SQLQuery.ExecNonQry(" Update  RVProduct SET RVNo= '"+ddRVNo.SelectedValue+"',  ProductID= '"+ddProductID.SelectedValue+"',  ProductDescription= '"+txtProductDescription.Text+"',  ReturnQTY= '"+txtReturnQTY.Text+"',  ProductReceive= '"+txtProductReceive.Text+"',  IssueDescription= '"+txtIssueDescription.Text+"',  UnitPrice= '"+txtUnitPrice.Text+"',  DepositalAccount= '"+txtDepositalAccount.Text+"' WHERE RVProductID='"+lblId.Text+"' ");
//ClearControls();
//btnSave.Text = "Save";
//Notify("Successfully Updated...", "success", lblMsg);
//}
//else
//{
//Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
//}
//}
//}
//catch (Exception ex)
//{
//Notify(ex.ToString(), "error", lblMsg);
//}
//finally
//{
//BindGrid();
//}
//}

//protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
//{
//try
//{
//string lName = Page.User.Identity.Name.ToString();
//if (SQLQuery.OparatePermission(lName, "Update") == "1")
//{
//int index = Convert.ToInt32(GridView1.SelectedIndex);
//Label lblEditId = GridView1.Rows[index].FindControl("Label1") as Label;
//lblId.Text = lblEditId.Text;
//DataTable dt = SQLQuery.ReturnDataTable(" Select RVProductID, RVNo,ProductID,ProductDescription,ReturnQTY,ProductReceive,IssueDescription,UnitPrice,DepositalAccount FROM RVProduct WHERE RVProductID='"+lblId.Text+"'");
//foreach (DataRow dtx in dt.Rows)
//{
//ddRVNo.SelectedValue=dtx["RVNo"].ToString();
//ddProductID.SelectedValue=dtx["ProductID"].ToString();
//txtProductDescription.Text=dtx["ProductDescription"].ToString();
//txtReturnQTY.Text=dtx["ReturnQTY"].ToString();
//txtProductReceive.Text=dtx["ProductReceive"].ToString();
//txtIssueDescription.Text=dtx["IssueDescription"].ToString();
//txtUnitPrice.Text=dtx["UnitPrice"].ToString();
//txtDepositalAccount.Text=dtx["DepositalAccount"].ToString();

//}
//btnSave.Text = "Update";
//Notify("Edit mode activated ...", "info", lblMsg);
//}
//else
//{
//Notify("You are not eligible to attempt this operation", "warn", lblMsg);
//}
//}
//catch (Exception ex)
//{
//Notify(ex.ToString(), "error", lblMsg);
//}
//}

//protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
//{
//string lName = Page.User.Identity.Name.ToString();
//if (SQLQuery.OparatePermission(lName, "Delete") == "1")
//{
//int index = Convert.ToInt32(e.RowIndex);
//Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
//RunQuery.SQLQuery.ExecNonQry(" Delete RVProduct WHERE RVProductID='"+lblId.Text+"' ");
//BindGrid();
//Notify("Successfully Deleted...", "success", lblMsg);
//}
//else
//{
//Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
//}
//}
//protected void btnClear_OnClick(object sender, EventArgs e)
//{
//Response.Redirect("./Default.aspx");
//}

//private void BindGrid()
//{
//DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM RVProduct");
//GridView1.DataSource = dt;
//GridView1.DataBind();
//}


//private void bindDDRVNo()
//{
//SQLQuery.PopulateDropDown("Select RV,IDRvNo from ReturnVauchar", ddRVNo, "IDRvNo", "RV");
//}


//protected void ddRVNo_SelectedIndexChanged(object sender, EventArgs e)
//{
//GridView1.DataBind();
//}


//private void bindDDProductID()
//{
//SQLQuery.PopulateDropDown("Select Name,ProductID from Product", ddProductID, "ProductID", "Name");
//}


//protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
//{
//GridView1.DataBind();
//}



//private void ClearControls()
//{
//txtProductDescription.Text=""; 
//txtReturnQTY.Text=""; 
//txtProductReceive.Text=""; 
//txtIssueDescription.Text=""; 
//txtUnitPrice.Text=""; 
//txtDepositalAccount.Text=""; 

//}










}
