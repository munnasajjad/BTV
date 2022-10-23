
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_GRNProduct : System.Web.UI.Page
{
}

//protected void Page_Load(object sender, EventArgs e)
//{
//btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
//if (!IsPostBack){
//bindDDProductID();
//bindDDGRNNo();
//txtProductMfgDate.Text=DateTime.Now.ToString("dd/MM/yyyy");
//txtProductExpDate.Text=DateTime.Now.ToString("dd/MM/yyyy");
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
//RunQuery.SQLQuery.ExecNonQry(" INSERT INTO GRNProduct (ProductID, GRNNo, ProductDescription, UnitNumber, Less, More, ReceiveProduct, RejectProduct, ProductMfgDate, ProductExpDate, PriceLetterNo, UnitPrice, TotalPrice, OtherCost, TotalCost) VALUES ('"+ddProductID.SelectedValue+"', '"+ddGRNNo.SelectedValue+"', '"+txtProductDescription.Text+"', '"+txtUnitNumber.Text+"', '"+txtLess.Text+"', '"+txtMore.Text+"', '"+txtReceiveProduct.Text+"', '"+txtRejectProduct.Text+"', '"+txtProductMfgDate.Text+"', '"+txtProductExpDate.Text+"', '"+txtPriceLetterNo.Text+"', '"+txtUnitPrice.Text+"', '"+txtTotalPrice.Text+"', '"+txtOtherCost.Text+"', '"+txtTotalCost.Text+"')    ");
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
//RunQuery.SQLQuery.ExecNonQry(" Update  GRNProduct SET ProductID= '"+ddProductID.SelectedValue+"',  GRNNo= '"+ddGRNNo.SelectedValue+"',  ProductDescription= '"+txtProductDescription.Text+"',  UnitNumber= '"+txtUnitNumber.Text+"',  Less= '"+txtLess.Text+"',  More= '"+txtMore.Text+"',  ReceiveProduct= '"+txtReceiveProduct.Text+"',  RejectProduct= '"+txtRejectProduct.Text+"',  ProductMfgDate= '"+txtProductMfgDate.Text+"',  ProductExpDate= '"+txtProductExpDate.Text+"',  PriceLetterNo= '"+txtPriceLetterNo.Text+"',  UnitPrice= '"+txtUnitPrice.Text+"',  TotalPrice= '"+txtTotalPrice.Text+"',  OtherCost= '"+txtOtherCost.Text+"',  TotalCost= '"+txtTotalCost.Text+"' WHERE GRNProductID='"+lblId.Text+"' ");
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
//DataTable dt = SQLQuery.ReturnDataTable(" Select GRNProductID, ProductID,GRNNo,ProductDescription,UnitNumber,Less,More,ReceiveProduct,RejectProduct,ProductMfgDate,ProductExpDate,PriceLetterNo,UnitPrice,TotalPrice,OtherCost,TotalCost FROM GRNProduct WHERE GRNProductID='"+lblId.Text+"'");
//foreach (DataRow dtx in dt.Rows)
//{
//ddProductID.SelectedValue=dtx["ProductID"].ToString();
//ddGRNNo.SelectedValue=dtx["GRNNo"].ToString();
//txtProductDescription.Text=dtx["ProductDescription"].ToString();
//txtUnitNumber.Text=dtx["UnitNumber"].ToString();
//txtLess.Text=dtx["Less"].ToString();
//txtMore.Text=dtx["More"].ToString();
//txtReceiveProduct.Text=dtx["ReceiveProduct"].ToString();
//txtRejectProduct.Text=dtx["RejectProduct"].ToString();
//txtProductMfgDate.Text=dtx["ProductMfgDate"].ToString();
//txtProductExpDate.Text=dtx["ProductExpDate"].ToString();
//txtPriceLetterNo.Text=dtx["PriceLetterNo"].ToString();
//txtUnitPrice.Text=dtx["UnitPrice"].ToString();
//txtTotalPrice.Text=dtx["TotalPrice"].ToString();
//txtOtherCost.Text=dtx["OtherCost"].ToString();
//txtTotalCost.Text=dtx["TotalCost"].ToString();

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
//RunQuery.SQLQuery.ExecNonQry(" Delete GRNProduct WHERE GRNProductID='"+lblId.Text+"' ");
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
//DataTable dt = SQLQuery.ReturnDataTable(" SELECT * FROM GRNProduct");
//GridView1.DataSource = dt;
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


//private void bindDDGRNNo()
//{
//SQLQuery.PopulateDropDown("Select InvoiceNo,IDGrnNO from GRNFrom", ddGRNNo, "IDGrnNO", "InvoiceNo");
//}


//protected void ddGRNNo_SelectedIndexChanged(object sender, EventArgs e)
//{
//GridView1.DataBind();
//}



//private void ClearControls()
//{
//txtProductDescription.Text=""; 
//txtUnitNumber.Text=""; 
//txtLess.Text=""; 
//txtMore.Text=""; 
//txtReceiveProduct.Text=""; 
//txtRejectProduct.Text=""; 
//txtProductMfgDate.Text=""; 
//txtProductExpDate.Text=""; 
//txtPriceLetterNo.Text=""; 
//txtUnitPrice.Text=""; 
//txtTotalPrice.Text=""; 
//txtOtherCost.Text=""; 
//txtTotalCost.Text=""; 

//}










//}
