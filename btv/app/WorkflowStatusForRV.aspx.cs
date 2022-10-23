using RunQuery;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkflowStatusForRV : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string id = Request.QueryString["Id"];
                productGridview.EmptyDataText = "No data added ...";
                productGridview.DataSource = null;
                productGridview.DataBind();
                WorkFlowUserGridView.EmptyDataText = "No data added ...";
                WorkFlowUserGridView.DataSource = null;
                WorkFlowUserGridView.DataBind();

                if (id != null)
                {
                   // string userId = EncryptDecrypt.DecryptString(id);
                    //string voucherID = SQLQuery.ReturnString("SELECT WorkFlowTypeID From WorkFlowUser Where WorkFlowUserID='" + userId + "'");
                    BindWorkFlowItemsGridView(id);
                    BindWorkFlowUserGridView(id);
                    LoadData(id);
                }

            }
            catch (Exception ex)
            {
                Notify("ERROR!: " + ex, "error", lblMsg);
            }

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
        string lName = Page.User.Identity.Name.ToString();
        string query = @"SELECT CASE ProductDetails.SerialNo WHEN '0' THEN Product.Name ELSE Product.Name + '-' + ProductDetails.SerialNo END AS ProductName, RVProduct.ProductID, RVProduct.ReturnQTY, RVProduct.ProductStatus, RVProduct.RVProductID, RVProduct.ProductReceive, RVProduct.UnitPrice, RVProduct.EntryBy, 
                  RVProduct.TotalPrice, RVProduct.RVNo FROM     ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                  RVProduct ON ProductDetails.ProductDetailsID = RVProduct.ProductDetailsID WHERE RVProduct.RVNo = '" + voucherID + "'";

        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        productGridview.EmptyDataText = "No data added ...";
        productGridview.DataSource = command.ExecuteReader();
        productGridview.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindWorkFlowUserGridView(string id)
    {
        string lName = Page.User.Identity.Name.ToString();
        //string query = @"SELECT  Employee.Name AS EmployeeName ,WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks, WorkFlowUser.Remark,
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'RV') ORDER BY WorkFlowUser.Priority";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }


    private void LoadData(string id)
    {
        string query = @"SELECT IDRvNo, RvInvoiceNo, DateOfRV, LocationID, ProductReturnDivision, Store, ReturnType, LvSIRVoucherNo, ProductReturnCause, FinYear, Comments, DocumentUrl, SaveMode, SubmitDate, WorkflowStatus, 
                  WorkflowApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, PreparedBy, Receivedby, ReceivedDate, ApprovedBy, ApprovedDate, LedgerWriterStoreBy, LedgerWriterStoreDate, ValueDeterminedby, ValueDeterminedDate, 
                  StoreKeeperBy, StoreKeeperDate, EntryBy, EntryDate FROM     ReturnVauchar WHERE IDRvNo='" + id + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtDateofRV.Text = Convert.ToDateTime(dataReader["DateOfRV"]).ToString("dd/MM/yyyy");
            txtRVNumber.Text = dataReader["RvInvoiceNo"].ToString();
            txtStore.Text = SQLQuery.ReturnString("SELECT Name FROM Store WHERE StoreAssignID='" + dataReader["Store"].ToString() + "'");
            txtProductReturn.Text = dataReader["ProductReturnDivision"].ToString();
            txtType.Text = dataReader["ReturnType"].ToString();
            txtReturnCause.Text = dataReader["ProductReturnCause"].ToString();
            lblRemarks.Text = dataReader["Comments"].ToString();
            if (dataReader["ReturnType"].ToString() == "LV")
            {
                lblLVSIR.Text = "LV Number";
                txtLVSIR.Text = SQLQuery.ReturnString("SELECT LvInvoiceNo FROM LoanVouchar WHERE IDLvNo='" + dataReader["LvSIRVoucherNo"].ToString() + "'");
            }
            else
            {
                lblLVSIR.Text = "SIR Number";
                txtLVSIR.Text = SQLQuery.ReturnString("SELECT SirVoucherNo FROM SIRFrom WHERE IDSirNo='" + dataReader["LvSIRVoucherNo"].ToString() + "'");
            }

        }
    }
}