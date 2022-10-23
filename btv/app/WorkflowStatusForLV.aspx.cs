using RunQuery;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkflowStatusForLV : System.Web.UI.Page
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
                    //string voucherNumber = SQLQuery.ReturnString("SELECT VoucherNo From WorkFlowUser Where WorkFlowUserID='" + userId + "'");
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
        //string lName = Page.User.Identity.Name.ToString();
        string query = @"SELECT LVProduct.LVProductID, CASE ProductDetails.SerialNo WHEN '0' THEN Product.Name ELSE Product.Name + '-' + ProductDetails.SerialNo END AS ProductName, LVProduct.QTYNeed, LVProduct.QTYDelivered, CONVERT(varchar, LVProduct.DeliveredDate, 103) AS DeliveredDate, LVProduct.ProductID
                        FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN LVProduct ON ProductDetails.ProductDetailsID = LVProduct.ProductDetailsID WHERE IDLVNo = '" + voucherID + "'";
        //string query = @"SELECT LVProductID, Product.Name AS ProductName, LVProduct.QTYNeed, LVProduct.QTYDelivered, convert(varchar, LVProduct.DeliveredDate, 103) AS DeliveredDate FROM LVProduct INNER JOIN Product ON LVProduct.ProductID = Product.ProductID WHERE IDLVNo = '" + voucherID + "'";

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
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'LV') ORDER BY WorkFlowUser.Priority";
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
        string query = @"SELECT  IDLvNo, LvInvoiceNo, DateofLv, LoanType, LoanToEmployee, ResponsiblePerson, CauseOfLoan, LocationID, Store, FinYear, DocumentUrl, Remarks, Verifier, RequisitionBy, DeliveredBy, PreparedBy, PreparedDate, EntryBy, 
                  EntryDate, SaveMode, SubmitDate, WorkflowStatus,  ReturnOrHoldUserID, CurrentWorkflowUser FROM LoanVouchar WHERE IDLvNo='" + id + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtDateofLV.Text = Convert.ToDateTime(dataReader["DateofLv"]).ToString("dd/MM/yyyy");
            txtLvNumber.Text = dataReader["LvInvoiceNo"].ToString();
            txtLoanType.Text = dataReader["LoanType"].ToString();
            if (dataReader["LoanType"].ToString() == "Employee")
            {
                txtEmployee.Text = SQLQuery.ReturnString("SELECT Name FROM Employee WHERE EmployeeID='" + dataReader["LoanToEmployee"].ToString() + "'");
                employee.Visible = true;
                responsible.Visible = false;
            }
            else
            {
                txtResponsible.Text = dataReader["ResponsiblePerson"].ToString();
                employee.Visible = false;
                responsible.Visible = true;
            }
            //txtStore.Text =S dataReader["Store"].ToString();
            txtCauseofLoan.Text = dataReader["CauseOfLoan"].ToString();
            lblRemarks.Text = dataReader["Remarks"].ToString();
        }
    }
}