using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkflowStatusForGrn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                string id = Request.QueryString["Id"];
                productAddGridView.EmptyDataText = "No data added ...";
                productAddGridView.DataSource = null;
                productAddGridView.DataBind();
                WorkFlowUserGridView.EmptyDataText = "No data added ...";
                WorkFlowUserGridView.DataSource = null;
                WorkFlowUserGridView.DataBind();
                
                if (id != null)
                {
                    BindWorkFlowItemsGridView(id);
                    // string userId = EncryptDecrypt.DecryptString(id);
                    //string voucherID = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + id + "'");
                    //string voucherNumber = SQLQuery.ReturnString("SELECT VoucherNo From WorkFlowUser Where WorkFlowUserID='" + userId + "'");

                    BindWorkFlowUserGridView(id);
                    LoadData(id);
                }
                //PermissionToAction();

            }
            catch (Exception ex)
            {
                Notify("ERROR!: " + ex, "error", lblMsg);
            }

        }
    }
    decimal total = 0;
    protected void productAddGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalCost"));
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[4].Text = "Total:";
            e.Row.Cells[5].Text = Convert.ToString(total);
            total = 0;
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void BindWorkFlowItemsGridView(string voucherID)
    {
        //string lName = Page.User.Identity.Name.ToString();

        string query = @"SELECT GRNProduct.GRNProductID, GRNProduct.GRNInvoiceNo, GRNProduct.CountryOfOrigin, GRNProduct.ManufacturingCompany, GRNProduct.Less, GRNProduct.More, GRNProduct.ReceiveProduct AS ReceiveProduct , 
                         GRNProduct.RejectProduct, GRNProduct.PriceLetterNo, GRNProduct.UnitPrice AS UnitPrice, GRNProduct.TotalPrice, GRNProduct.OtherCost, GRNProduct.TotalCost AS TotalCost, GRNProduct.EntryBy, GRNProduct.EntryDate, 
                         Product.Name AS ProductName FROM GRNProduct INNER JOIN
                         Product ON GRNProduct.ProductID = Product.ProductID WHERE GRNProduct.GrnFormID = '" + voucherID + "'";

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Connection.Open();
        productAddGridView.EmptyDataText = "No data added ...";
        productAddGridView.DataSource = command.ExecuteReader();
        productAddGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindWorkFlowUserGridView(string id)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks,WorkFlowUser.Remark, 
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID
                  FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'GRN') ORDER BY WorkFlowUser.Priority";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void LoadData(string id)
    {
        string query = @"SELECT IDGrnNO, GRNInvoiceNo, DateOfGRN, StoreID, ReferenceID, Supplier, InvoiceNo, DateofInvoiceNo, PurchaseOrderNo, DateofPurchaseOrderNo, ProductSHReceiveDate, TotalAmount, Remarks, PreparedBy, PreparedDate, 
                  ProductInspectorApprovedBy, ApprovedDate, StoreDivLedgerWritter, AccountDivLedgerWritter, AccountDivLedgerDate, SaveMode, WorkflowStatus, EntryDate, EntryBy FROM     GRNFrom WHERE  (IDGrnNO = '" + id + "')";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtGrnDate.Text = Convert.ToDateTime(dataReader["DateOfGRN"]).ToString("dd/MM/yyyy");
            txtGrnNo.Text = dataReader["GRNInvoiceNo"].ToString();
            txtInvoiceNo.Text = dataReader["InvoiceNo"].ToString();
            txtReference.Text = SQLQuery.ReturnString("SELECT Name FROM Reference WHERE ReferenceID='" + dataReader["ReferenceID"].ToString() + "'");
            txtSupplier.Text = dataReader["Supplier"].ToString();
            lblRemarks.Text = dataReader["Remarks"].ToString();
        }
    }
}