
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class app_ProductDelivery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);
            LoadLabelWithDropdown();
            //bindDDProductID();
            //bindDDSIR_RV_LVID();
            txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ProductGridView();
            //BindGrid();
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
                    if (rdLoan.Checked)
                    {
                        if (ddSIR_RV_LVID.SelectedValue!="0")
                        {
                            SQLQuery.ExecNonQry("Update LoanVouchar SET ReceiverName='" + txtRcv.Text.Trim() + "', ReceiverDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "', ReceivedStatus='Received' WHERE IDLvNo='" + ddSIR_RV_LVID.SelectedValue + "'");
                            Notify("Successfully Delivered...", "success", lblMsg);
                            txtRcv.Text = "";
                            LoadLabelWithDropdown();
                        }
                       
                    }
                    else
                    {
                        if (ddSIR_RV_LVID.SelectedValue != "0")
                        {
                            SQLQuery.ExecNonQry("Update SIRFrom SET ReceiverName='" + ddSIRReceivedBy.SelectedValue + "', ReceiverDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "', ReceivedStatus='Received' WHERE IDSirNo='" + ddSIR_RV_LVID.SelectedValue + "'");
                            Notify("Successfully Delivered...", "success", lblMsg);
                            LoadLabelWithDropdown();
                            txtRcv.Text = "";
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

            Notify("ERROR:"+ex, "warn", lblMsg);
        }

        //try
        //{
        //    string lName = Page.User.Identity.Name.ToString();
        //    if (btnSave.Text == "Save")
        //    {
        //        if (SQLQuery.OparatePermission(lName, "Insert") == "1")
        //        {
        //            if (rdLoan.Checked)
        //            {

        //                bool flag = true;

        //                string query = "";

        //                foreach (GridViewRow row in ProductGrid.Rows)
        //                {
        //                    Label lblProductId = row.FindControl("lblProductId") as Label;
        //                    Label lblProductName = row.FindControl("lblProductName") as Label;
        //                    TextBox txtQTYDelivered = row.FindControl("txtQTYDelivered") as TextBox;
        //                    Label lblApprovedQty = row.FindControl("lblApprovedQty") as Label;
        //                    Label lblLVProductID = row.FindControl("lblLVProductID") as Label;
        //                    Label lblRemainingQty = row.FindControl("lblRemainingQty") as Label;
        //                    int approvedQty = Convert.ToInt32(lblApprovedQty.Text);
        //                    int deliveredQty = Convert.ToInt32(txtQTYDelivered.Text);
        //                    int remainingQty = Convert.ToInt32(lblRemainingQty.Text);
                            
        //                    if (approvedQty >= deliveredQty)
        //                    {
        //                        if (deliveredQty > 0)
        //                        {
        //                            if (remainingQty >= deliveredQty)
        //                            {
        //                                remainingQty = approvedQty - deliveredQty;
        //                                query += " Insert Into ProductDeliveryDetails(ProductDeliveryId,ProductName, ApprovedQty, DeliveredQty,RemainingQty, EntryBy) Values('0','" + lblProductName.Text + "','" + lblApprovedQty.Text + "','" + txtQTYDelivered.Text + "','" + remainingQty + "','" + User.Identity.Name + "')";
        //                                query += " Update LvProduct SET QTYDelivered=QTYDelivered+'" + txtQTYDelivered.Text + "', DeliveredDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' Where LVProductID='" + lblLVProductID.Text + "'";
        //                                query += " INSERT INTO StockRegister (ProductID,EntryType, Date, PreviousStockIn, StockIn,  Total, StockOutCashMemoChallanNo, SellQty,  EntryBy) VALUES('" + lblProductId.Text + "','LV','"+DateTime.Now.ToString("yyyy-MM-dd")+"','0','0','"+deliveredQty+"','"+ddSIR_RV_LVID.SelectedValue+"','"+deliveredQty+"','"+User.Identity.Name+"')";
        //                                //Accounting.VoucherEntry.StockEntry(lblLVProductID.Text,"LV","0","0","","",ddSIR_RV_LVID.SelectedValue,deliveredQty.ToString(),"Loan Voucher",User.Identity.Name);
        //                                //DataTable dt = SQLQuery.ReturnDataTable("SELECT GRNProductID, GRNInvoiceNo, CategoryID, SubCategoryID, ProductID,   ReceiveProduct FROM     GRNProduct Where GRNInvoiceNo='" + txtGrnNo.Text.Trim() + "'");
        //                                //foreach (DataRow item in dt.Rows)
        //                                //{
        //                                //    string grnNo = item["GRNInvoiceNo"].ToString();
        //                                //    string productID = item["ProductID"].ToString();
        //                                //    string receiveProduct = item["ReceiveProduct"].ToString();
        //                                //    Accounting.VoucherEntry.StockEntry(productID, "GRN", "0", receiveProduct, grnNo, receiveProduct, "", "0", "", User.Identity.Name);
        //                                //}

        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        //Notify("Successfully Saved...", "warn", lblMsg);
        //                        flag = false;
        //                    }
        //                }
        //                if (flag)
        //                {
        //                    if (query != "")
        //                    {
        //                        SQLQuery.ExecNonQry(query);
        //                        SQLQuery.ExecNonQry(" INSERT INTO ProductDelivery (Type, SIR_RV_LVID, Date, Remarks, EntryBy) VALUES ('Loan Voucher', '" + ddSIR_RV_LVID.SelectedValue + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtRemarks.Text.Replace("'", "''") + "', '" + User.Identity.Name + "')    ");
        //                        string maxId = SQLQuery.ReturnString("SELECT MAX(ProductDeliveryID) AS ID FROM ProductDelivery WHERE EntryBy='" + lName + "'");
        //                        SQLQuery.ExecNonQry("Update ProductDeliveryDetails SET ProductDeliveryId='" + maxId + "' WHERE ProductDeliveryId='0' AND EntryBy='" + lName + "'");
        //                        if (IsComplete(ddSIR_RV_LVID.SelectedValue))
        //                        {
        //                            SQLQuery.ExecNonQry("Update LoanVouchar SET DeliveredStatus='1' WHERE LvInvoiceNo='" + ddSIR_RV_LVID.SelectedValue + "'");
        //                        }

        //                        ClearControls();

        //                        Notify("Successfully Saved...", "success", lblMsg);

        //                    }


        //                }
        //                else
        //                {
        //                    Notify("Please ensure delivered qauntity less then or equal to approved quantity!", "warn", lblMsg);
        //                    return;
        //                }
        //            }
        //            //else if (rdReturn.Checked)
        //            //{
        //            //    bool flag = true;
        //            //    string query = "";

        //            //    foreach (GridViewRow row in rvProductGrid.Rows)
        //            //    {
        //            //        Label lblProductName = row.FindControl("lblProductName") as Label;
        //            //        Label lblProductId = row.FindControl("lblProductId") as Label;
        //            //        TextBox txtQTYDelivered = row.FindControl("txtQTYDelivered") as TextBox;
        //            //        Label lblApprovedQty = row.FindControl("lblApprovedQty") as Label;
        //            //        Label lblRVProductID = row.FindControl("lblRVProductID") as Label;
        //            //        Label lblRemainingQty = row.FindControl("lblRemainingQty") as Label;
        //            //        int approvedQty = Convert.ToInt32(lblApprovedQty.Text);
        //            //        int deliveredQty = Convert.ToInt32(txtQTYDelivered.Text);                           
        //            //        int remainingQty = Convert.ToInt32(lblRemainingQty.Text);
        //            //        if (approvedQty >= deliveredQty)
        //            //        {
        //            //            if (deliveredQty > 0)
        //            //            {
        //            //                if (remainingQty >= deliveredQty)
        //            //                {
        //            //                    remainingQty = approvedQty - deliveredQty;
        //            //                    query += " Insert Into ProductDeliveryDetails(ProductDeliveryId,ProductName, ApprovedQty, DeliveredQty,RemainingQty, EntryBy) Values('0','" + lblProductName.Text + "','" + lblApprovedQty.Text + "','" + txtQTYDelivered.Text + "','" + remainingQty + "','" + User.Identity.Name + "')";
        //            //                    query += " Update RVProduct SET ReturnQTY=ReturnQTY+'" + txtQTYDelivered.Text + "', ReturnDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' Where RVProductID='" + lblRVProductID.Text + "'";
        //            //                    query += " INSERT INTO StockRegister (ProductID,EntryType, Date, PreviousStockIn, StockIn, Total, StockOutCashMemoChallanNo, SellQty, EntryBy) VALUES('" + lblProductId.Text + "','RV','"+DateTime.Now.ToString("yyyy-MM-dd")+"','0','0','"+deliveredQty+"','"+ddSIR_RV_LVID.SelectedValue+ "','" + deliveredQty + "','" + User.Identity.Name+"')";

        //            //                }
        //            //            }
        //            //        }
        //            //        else
        //            //        {
        //            //            //Notify("Successfully Saved...", "warn", lblMsg);
        //            //            flag = false;
        //            //        }
        //            //    }
        //            //    if (flag)
        //            //    {
        //            //        if (query != "")
        //            //        {
        //            //            SQLQuery.ExecNonQry(query);
        //            //            SQLQuery.ExecNonQry(" INSERT INTO ProductDelivery (Type, SIR_RV_LVID, Date, Remarks, EntryBy) VALUES ('Return Voucher', '" + ddSIR_RV_LVID.SelectedValue + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtRemarks.Text.Replace("'", "''") + "', '" + User.Identity.Name + "')");
        //            //            string maxId = SQLQuery.ReturnString("SELECT MAX(ProductDeliveryID) AS ID FROM ProductDelivery WHERE EntryBy='" + lName + "'");
        //            //            SQLQuery.ExecNonQry("Update ProductDeliveryDetails SET ProductDeliveryId='" + maxId + "' WHERE ProductDeliveryId='0' AND EntryBy='" + lName + "'");
        //            //            if (IsComplete(ddSIR_RV_LVID.SelectedValue))
        //            //            {
        //            //                SQLQuery.ExecNonQry("Update ReturnVauchar SET ReturnStatus='1' WHERE RvInvoiceNo='" + ddSIR_RV_LVID.SelectedValue + "'");
        //            //            }
        //            //            ClearControls();
        //            //            Notify("Successfully Saved...", "success", lblMsg);
        //            //        }
        //            //    }
        //            //    else
        //            //    {

        //            //        Notify("Please ensure delivered qauntity less then or equal to approved quantity!", "warn", lblMsg);
        //            //        return;
        //            //    }
        //            //}
        //            else
        //            {
        //                bool flag = true;
        //                string query = "";

        //                foreach (GridViewRow row in sirProductGrid.Rows)
        //                {
        //                    Label lblProductName = row.FindControl("lblProductName") as Label;
        //                    Label lblProductId = row.FindControl("lblProductId") as Label;
        //                    TextBox txtQTYDelivered = row.FindControl("txtQTYDelivered") as TextBox;
        //                    Label lblApprovedQty = row.FindControl("lblApprovedQty") as Label;
        //                    Label lblSIRProductID = row.FindControl("lblSIRProductID") as Label;
        //                    Label lblRemainingQty = row.FindControl("lblRemainingQty") as Label;
        //                    int approvedQty = Convert.ToInt32(lblApprovedQty.Text);
        //                    int deliveredQty = Convert.ToInt32(txtQTYDelivered.Text);
        //                    int remainingQty = Convert.ToInt32(lblRemainingQty.Text);
        //                    if (approvedQty >= deliveredQty)
        //                    {
        //                        if (deliveredQty > 0)
        //                        {
        //                            if (remainingQty >= deliveredQty)
        //                            {
        //                                remainingQty = approvedQty - deliveredQty;
        //                                query += " Insert Into ProductDeliveryDetails(ProductDeliveryId,ProductName, ApprovedQty, DeliveredQty,RemainingQty, EntryBy) Values('0','" + lblProductName.Text + "','" + lblApprovedQty.Text + "','" + txtQTYDelivered.Text + "','" + remainingQty + "','" + User.Identity.Name + "')";
        //                                query += " Update SIRProduct SET QTYDelivered=QTYDelivered+'" + txtQTYDelivered.Text + "', DeliveredDate='" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "' Where SIRProductID='" + lblSIRProductID.Text + "'";
        //                                query += " INSERT INTO StockRegister (ProductID,EntryType, Date, PreviousStockIn, StockIn, Total, StockOutCashMemoChallanNo, SellQty, EntryBy) VALUES('" + lblProductId.Text + "','SIR','"+DateTime.Now.ToString("yyyy-MM-dd")+"','0','0','"+deliveredQty+"','"+ddSIR_RV_LVID.SelectedValue+ "','" + deliveredQty + "','" + User.Identity.Name+"')";

        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //Notify("Successfully Saved...", "warn", lblMsg);
        //                        flag = false;
        //                    }
        //                }
        //                if (flag)
        //                {
        //                    if (query != "")
        //                    {
        //                        SQLQuery.ExecNonQry(query);
        //                        SQLQuery.ExecNonQry(" INSERT INTO ProductDelivery (Type, SIR_RV_LVID, Date, Remarks, EntryBy) VALUES ('SIR Voucher', '" + ddSIR_RV_LVID.SelectedValue + "', '" + Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd") + "', '" + txtRemarks.Text.Replace("'", "''") + "', '" + User.Identity.Name + "')");
        //                        string maxId = SQLQuery.ReturnString("SELECT MAX(ProductDeliveryID) AS ID FROM ProductDelivery WHERE EntryBy='" + lName + "'");
        //                        SQLQuery.ExecNonQry("Update ProductDeliveryDetails SET ProductDeliveryId='" + maxId + "' WHERE ProductDeliveryId='0' AND EntryBy='" + lName + "'");
        //                        if (IsComplete(ddSIR_RV_LVID.SelectedValue))
        //                        {
        //                            SQLQuery.ExecNonQry("Update SIRFrom SET DeliveredStatus='1' WHERE SirVoucherNo='" + ddSIR_RV_LVID.SelectedValue + "'");
        //                        }
        //                        ClearControls();
        //                        Notify("Successfully Saved...", "success", lblMsg);
        //                    }
        //                }

        //                else
        //                {

        //                    Notify("Please ensure delivered qauntity less then or equal to approved quantity!", "warn", lblMsg);
        //                    return;
        //                }
        //            }


        //        }
        //        else
        //        {
        //            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //        }
        //    }
        //    //else
        //    //{
        //    //    if (SQLQuery.OparatePermission(lName, "Update") == "1")
        //    //    {
        //    //        // RunQuery.SQLQuery.ExecNonQry(" Update  ProductDelivery SET ProductID= '" + ddProductID.SelectedValue + "',  SIR_RV_LVID= '" + ddSIR_RV_LVID.SelectedValue + "',  QTY= '" + txtQTY.Text + "',  Status= '" + txtStatus.Text + "',  Date= '" + txtDate.Text + "',  Remarks= '" + txtRemarks.Text + "' WHERE ProductDeliveryID='" + lblId.Text + "' ");
        //    //        ClearControls();
        //    //        btnSave.Text = "Save";
        //    //        Notify("Successfully Updated...", "success", lblMsg);
        //    //    }
        //    //    else
        //    //    {
        //    //        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //    //    }
        //    //}
        //}
        //catch (Exception ex)
        //{
        //    Notify(ex.ToString(), "error", lblMsg);
        //}
        //finally
        //{
        //    LaodLabelWithDropdwon();
        //    ProductGridView();
        //    BindGrid();
        //}
    }

    private bool IsComplete(string invoice)
    {
        bool isComplete = true;
        string query = "";
        if (rdLoan.Checked)
        {
            query = @"SELECT LoanVouchar.LvInvoiceNo AS InvoiceNo,LVProduct.LVProductID, (SELECT Name FROM Product WHERE (ProductID = LVProduct.ProductID)) AS ProductName, LVProduct.ApprovedQty, LVProduct.QTYDelivered,ISNULL(SUM(LVProduct.ApprovedQty),0) - ISNULL(SUM(LVProduct.QTYDelivered),0) AS RemainingQty
                                                FROM LoanVouchar INNER JOIN LVProduct ON LoanVouchar.IDLvNo = LVProduct.IDLVNo WHERE LvInvoiceNo='" + ddSIR_RV_LVID.SelectedValue + "' Group By LoanVouchar.LvInvoiceNo,LVProduct.ProductID,LVProduct.QTYNeed,LVProduct.QTYDelivered,ApprovedQty,LVProduct.LVProductID";
            DataTable dt = SQLQuery.ReturnDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                int approvedQty = Convert.ToInt32(item["ApprovedQty"]);
                int deliveredQty = Convert.ToInt32(item["QTYDelivered"]);
                if (approvedQty != deliveredQty)
                    isComplete = false;

            }

        }

        //else if (rdReturn.Checked)
        //{
        //    query = @"SELECT ReturnVauchar.IDRvNo, RVProduct.ApprovedQty,RVProduct.RVProductID, ReturnVauchar.RvInvoiceNo,ISNULL(SUM(RVProduct.ApprovedQty),0) - ISNULL(SUM(RVProduct.ReturnQTY),0) AS RemainingQty,
        //              (SELECT Name FROM Product WHERE   (ProductID = RVProduct.ProductID)) AS ProductName, RVProduct.ReturnQTY FROM ReturnVauchar INNER JOIN RVProduct ON ReturnVauchar.IDRvNo = RVProduct.RVNo Where ReturnVauchar.RvInvoiceNo='" + ddSIR_RV_LVID.SelectedValue + "' Group By IDRvNo,ApprovedQty,RvInvoiceNo,ProductID,ReturnQTY,RVProductID";
        //    DataTable dt = SQLQuery.ReturnDataTable(query);
        //    foreach (DataRow item in dt.Rows)
        //    {
        //        int approvedQty = Convert.ToInt32(item["ApprovedQty"]);
        //        int returnQTY = Convert.ToInt32(item["ReturnQTY"]);
        //        if (approvedQty != returnQTY)
        //            isComplete = false;

        //    }


        //}
        else
        {
            query = @"SELECT TOP SIRFrom.IDSirNo, SIRFrom.SirVoucherNo,(SELECT Name
                       FROM Product WHERE   (ProductID = SIRProduct.ProductID)) AS ProductName, SIRProduct.SIRProductID, SIRProduct.ApprovedQty, SIRProduct.QTYDelivered,ISNULL(SUM(SIRProduct.ApprovedQty),0) - ISNULL(SUM(SIRProduct.QTYDelivered),0) AS RemainingQty
                    FROM SIRFrom INNER JOIN SIRProduct ON SIRFrom.IDSirNo = SIRProduct.SIRNo Where SIRFrom.SirVoucherNo='" + ddSIR_RV_LVID.SelectedValue + "' Group By IDSirNo,SirVoucherNo,ProductID,ApprovedQty,QTYDelivered,SIRProductID";
            DataTable dt = SQLQuery.ReturnDataTable(query);
            foreach (DataRow item in dt.Rows)
            {
                int approvedQty = Convert.ToInt32(item["ApprovedQty"]);
                int deliveredQty = Convert.ToInt32(item["QTYDelivered"]);
                if (approvedQty != deliveredQty)
                    isComplete = false;

            }

        }

        return isComplete;
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
                DataTable dt = SQLQuery.ReturnDataTable(" Select ProductDeliveryID, ProductID,SIR_RV_LVID,QTY,Status,Date,Remarks FROM ProductDelivery WHERE ProductDeliveryID='" + lblId.Text + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    //ddProductID.SelectedValue = dtx["ProductID"].ToString();
                    ddSIR_RV_LVID.SelectedValue = dtx["SIR_RV_LVID"].ToString();
                    //txtQTY.Text = dtx["QTY"].ToString();
                    //txtStatus.Text = dtx["Status"].ToString();
                    txtDate.Text = dtx["Date"].ToString();
                    txtRcv.Text = dtx["Remarks"].ToString();

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
    private void LoadLvInvoice()
    {
        SQLQuery.PopulateDropDown("SELECT IDLvNo, LvInvoiceNo FROM LoanVouchar WHERE ReceivedStatus='Pending' AND Store IN (SELECT StoreID FROM StoreAssign WHERE  (EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))", ddSIR_RV_LVID, "IDLvNo", "LvInvoiceNo");
    }
    
    private void LoadSirInovice()
    {
        SQLQuery.PopulateDropDown("SELECT IDSirNo, SirVoucherNo FROM  SIRFrom WHERE ReceivedStatus='Pending' AND Store IN (SELECT StoreID FROM StoreAssign WHERE  (EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))", ddSIR_RV_LVID, "IDSirNo", "SirVoucherNo");
    }
    private void LoadSirReceivedBy()
    {
        string query = " WHERE LocationID='" + SQLQuery.GetLocationIdBySirNo(ddSIR_RV_LVID.SelectedValue) + "' AND EmployeeID NOT IN(SELECT EmployeeInfoID FROM Logins WHERE (LoginUserName = '" + User.Identity.Name + "'))";
        SQLQuery.PopulateDropDown("Select EmployeeID, Name+' ('+ Mobile+')' AS Name FROM Employee" + query + "", ddSIRReceivedBy, "EmployeeID", "Name");
    }
   
    protected void GridView1_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = GridView1.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ExecNonQry(" Delete ProductDelivery WHERE ProductDeliveryID='" + lblId.Text + "' ");
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
        DataTable dt = SQLQuery.ReturnDataTable("SELECT ProductDeliveryID, Type, SIR_RV_LVID, CONVERT(varchar, Date, 103) AS Date, Remarks, EntryBy FROM ProductDelivery Order by Date Desc");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }


    //private void bindDDProductID()
    //{
    //    //SQLQuery.PopulateDropDown("Select ProductID from Product", ddProductID, "ProductID", "ProductID");
    //}


    protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }


    private void bindDDSIR_RV_LVID()
    {
        SQLQuery.PopulateDropDown("Select IDLvNo from LoanVouchar", ddSIR_RV_LVID, "IDLvNo", "IDLvNo");
    }


    protected void ddSIR_RV_LVID_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSirReceivedBy();
        ProductGridView();
    }
    private void ProductGridView()
    {
        string query = "";
        //if (rdLoan.Checked)
        //{
        //    lvGrid.Visible = true;
        //    rvGrid.Visible = false;
        //    sirGrid.Visible = false;
        //    query = @"SELECT LoanVouchar.LvInvoiceNo AS InvoiceNo,LVProduct.LVProductID,LVProduct.ProductID, (SELECT Name FROM Product WHERE (ProductID = LVProduct.ProductID)) AS ProductName, LVProduct.ApprovedQty, LVProduct.QTYDelivered,ISNULL(SUM(LVProduct.ApprovedQty),0) - ISNULL(SUM(LVProduct.QTYDelivered),0) AS RemainingQty
        //                                        FROM LoanVouchar INNER JOIN LVProduct ON LoanVouchar.IDLvNo = LVProduct.IDLVNo WHERE LvInvoiceNo='" + ddSIR_RV_LVID.SelectedValue + "' Group By LoanVouchar.LvInvoiceNo,LVProduct.ProductID,LVProduct.QTYNeed,LVProduct.QTYDelivered,ApprovedQty,LVProduct.LVProductID";
        //    DataTable dt = SQLQuery.ReturnDataTable(query);
        //    ProductGrid.DataSource = dt;
        //    ProductGrid.EmptyDataText = "Data not found!";
        //    ProductGrid.DataBind();
        //}
        //else if (rdReturn.Checked)
        //{
        //    lvGrid.Visible = false;
        //    rvGrid.Visible = true;
        //    sirGrid.Visible = false;
        //    query = @"SELECT ReturnVauchar.IDRvNo, RVProduct.ApprovedQty,RVProduct.RVProductID,RVProduct.ProductID, ReturnVauchar.RvInvoiceNo,ISNULL(SUM(RVProduct.ApprovedQty),0) - ISNULL(SUM(RVProduct.ReturnQTY),0) AS RemainingQty,
        //              (SELECT Name FROM Product WHERE   (ProductID = RVProduct.ProductID)) AS ProductName, RVProduct.ReturnQTY FROM ReturnVauchar INNER JOIN RVProduct ON ReturnVauchar.IDRvNo = RVProduct.RVNo Where ReturnVauchar.RvInvoiceNo='" + ddSIR_RV_LVID.SelectedValue + "' Group By IDRvNo,ApprovedQty,RvInvoiceNo,ProductID,ReturnQTY,RVProductID";
        //    DataTable dt = SQLQuery.ReturnDataTable(query);
        //    rvProductGrid.DataSource = dt;
        //    rvProductGrid.EmptyDataText = "Data not found!";
        //    rvProductGrid.DataBind();
        //}
        //else
        //{
            //lvGrid.Visible = false;
            //rvGrid.Visible = false;
            //sirGrid.Visible = true;
            //query = @"SELECT TOP (200) SIRFrom.IDSirNo, SIRFrom.SirVoucherNo,SIRProduct.ProductID,(SELECT Name
            //           FROM Product WHERE   (ProductID = SIRProduct.ProductID)) AS ProductName, SIRProduct.SIRProductID, SIRProduct.ApprovedQty, SIRProduct.QTYDelivered,ISNULL(SUM(SIRProduct.ApprovedQty),0) - ISNULL(SUM(SIRProduct.QTYDelivered),0) AS RemainingQty
            //        FROM SIRFrom INNER JOIN SIRProduct ON SIRFrom.IDSirNo = SIRProduct.SIRNo Where SIRFrom.SirVoucherNo='" + ddSIR_RV_LVID.SelectedValue + "' Group By IDSirNo,SirVoucherNo,ProductID,ApprovedQty,QTYDelivered,SIRProductID";
            //DataTable dt = SQLQuery.ReturnDataTable(query);
            //sirProductGrid.DataSource = dt;
            //sirProductGrid.EmptyDataText = "Data not found!";
            //sirProductGrid.DataBind();
       // }

    }


    private void ClearControls()
    {
        //txtQTY.Text = "";

        txtDate.Text = "";
        txtRcv.Text = "";

    }
    
    protected void rdLoan_CheckedChanged(object sender, EventArgs e)
    {
        //ProductGridView();
        LoadLabelWithDropdown();
    }

    private void LoadLabelWithDropdown()
    {

        if (rdLoan.Checked)
        {
            lblSirLVRV.Text = "LV Number";
            LoadLvInvoice();
        }
        else 
        {
            lblSirLVRV.Text = "SIR Number";
            LoadSirInovice();
            LoadSirReceivedBy();
        }
    }
}
