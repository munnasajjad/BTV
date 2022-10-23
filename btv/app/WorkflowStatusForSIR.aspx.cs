using RunQuery;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkflowStatusForSIR : System.Web.UI.Page
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
                    //string userId = EncryptDecrypt.DecryptString(id);
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
        string query = @"SELECT SIRProduct.SIRProductID, CASE ProductDetails.SerialNo WHEN '0' THEN Product.Name ELSE Product.Name + '-' + ProductDetails.SerialNo END AS ProductName, SIRProduct.QTYDelivered, SIRProduct.QTYNeed, SIRProduct.QTYAvailable, SIRProduct.DeliveredQTYTotalPrice, SIRProduct.UnitPrice, SIRProduct.ProductId FROM ProductDetails INNER JOIN   Product ON ProductDetails.ProductID = Product.ProductId INNER JOIN SIRProduct ON ProductDetails.ProductDetailsID = SIRProduct.ProductDetailsID WHERE IDSirNo = '" + voucherID + "'";
        //string query = @"SELECT SIRProduct.SIRProductID, Product.Name AS ProductName, SIRProduct.QTYDelivered, SIRProduct.QTYNeed,SIRProduct.QTYAvailable,SIRProduct.DeliveredQTYTotalPrice,SIRProduct.UnitPrice FROM SIRProduct INNER JOIN Product ON SIRProduct.ProductId = Product.ProductID WHERE IDSirNo = '" + voucherID + "'";
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
        //string query = @"SELECT  Employee.Name AS EmployeeName, WorkFlowUser.Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime,UserRemarks, ApproveDeclineDate, PermissionStatus FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + lvId + "' Order By Priority DESC ";
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks,  WorkFlowUser.Remark,
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'SIR') ORDER BY WorkFlowUser.Priority";
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
        string query = @"SELECT  IDSirNo, DateOfSir, SirVoucherNo, LocationID, FinYear, LoanToEmployee, Store, GivenDivision, GivenDivisionDate, ProductUseAim, HeadOfCost, Remarks, DocumentUrl, PreparedBy, SaveMode, SubmitDate, WorkflowStatus, 
                  ApprovedDate, ReturnOrHoldUserID, CurrentWorkflowUser, EntryDate, EntryBy FROM SIRFrom WHERE IDSirNo='" + id + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtDateofSIR.Text = Convert.ToDateTime(dataReader["DateOfSir"]).ToString("dd/MM/yyyy");
            txtSirNumber.Text = dataReader["SirVoucherNo"].ToString();
            txtEmployee.Text = SQLQuery.ReturnString("SELECT Name FROM Employee WHERE EmployeeID='" + dataReader["LoanToEmployee"].ToString() + "'");
            txtStore.Text = SQLQuery.ReturnString("SELECT Name FROM Store WHERE StoreAssignID='" + dataReader["Store"].ToString() + "'");
            txtCauseofLoan.Text = dataReader["HeadOfCost"].ToString();
            txtProductPurpose.Text = dataReader["ProductUseAim"].ToString();
            lblRemarks.Text = dataReader["Remarks"].ToString();
        }
    }
}