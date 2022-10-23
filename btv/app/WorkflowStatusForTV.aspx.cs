using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_WorkflowStatusForTV : System.Web.UI.Page
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
                   // string userId = EncryptDecrypt.DecryptString(id);
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

        //string query = @"SELECT TvProduct.TvProductID, TvProduct.TVoucherNo, TvProduct.CategoryID, TvProduct.SubCategoryID, TvProduct.ProductID, Product.Name AS ProductName,TvProduct.TvQty, TvProduct.EntryBy, TvProduct.EntryDate
        //  FROM TvProduct INNER JOIN Product ON TvProduct.ProductID = Product.ProductID WHERE TvProduct.TvVoucherID = '" + voucherID + "'";

        string query = @"SELECT  TvProduct.TvProductID,CASE ProductDetails.SerialNo WHEN '0' THEN Product.Name ELSE Product.Name + '-' + ProductDetails.SerialNo END AS ProductName,TvProduct.ProductID,TvProduct.TvQty
                        FROM ProductDetails INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN TvProduct ON ProductDetails.ProductDetailsID = TvProduct.ProductDetailsID WHERE TvProduct.TvVoucherID = '" + voucherID + "'";
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
        string query = @"SELECT Employee.Name + ', ' + Designation.Name AS EmployeeName,WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS Priority, CONVERT(DATETIME,WorkFlowUser.EsclationStartTime, 121) AS EsclationStartTime, CONVERT(DATETIME, WorkFlowUser.EsclationEndTime, 121) AS EsclationEndTime, WorkFlowUser.UserRemarks, WorkFlowUser.Remark,
                  WorkFlowUser.ApproveDeclineDate, WorkFlowUser.PermissionStatus, DesignationWithEmployee.EmployeeID
FROM     WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN

                  WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID
WHERE  (WorkFlowUser.WorkFlowTypeID = '" + id + "') AND (WorkFlowUser.WorkFlowType = 'TV') ORDER BY WorkFlowUser.Priority";
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
        string query = @"SELECT TV.TvID, TV.TransferVoucherNo, TV.Date, TV.FormStoreID, FStore.Name AS StoreFrom, TV.RequsitionBy, TV.LocationID, TV.CenterID, TV.DepartmentSectionID, TV.ToStoreID, TStore.Name AS StoreTo, TV.Requirment, 
                         TV.DocumentUrl, TV.SaveMode, TV.WorkflowStatus, TV.ReturnOrHoldUserID, TV.WorkflowApprovedDate, TV.SubmitDate, TV.CurrentWorkflowUser, TV.Remarks, TV.EntryDate, TV.EntryBy
                         FROM TransferVoucher AS TV INNER JOIN
                         Store AS FStore ON TV.FormStoreID = FStore.StoreAssignID INNER JOIN
                         Store AS TStore ON TV.ToStoreID = TStore.StoreAssignID WHERE  (TV.TvID = '" + id + "')";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        SqlDataReader dataReader = command.ExecuteReader();
        if (dataReader.Read())
        {
            txtTVDate.Text = Convert.ToDateTime(dataReader["Date"]).ToString("dd/MM/yyyy");
            txtTVNo.Text = dataReader["TransferVoucherNo"].ToString();
            txtStoreFrom.Text = dataReader["StoreFrom"].ToString();
            txtStoreTo.Text = dataReader["StoreTo"].ToString();
            //txtReference.Text = SQLQuery.ReturnString("SELECT Name FROM Reference WHERE ReferenceID='" + dataReader["ReferenceID"].ToString() + "'");
            lblRemarks.Text = dataReader["Remarks"].ToString();
        }
    }
}