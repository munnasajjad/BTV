
using RunQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_LoanHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadEmployee();
            LoanHistory();

        }
    }

    private void LoanHistory()
    {
        DataTable dt = new DataTable();
        DataRow dtRow = null;
        dt.Columns.Add("Date", typeof(string));
        dt.Columns.Add("VoucherNo", typeof(string));
        dt.Columns.Add("PName", typeof(string));
        dt.Columns.Add("Quantity", typeof(string));
        dt.Columns.Add("StoreName", typeof(string));

        string locationId = SQLQuery.GetLocationID(User.Identity.Name);
        DataTable dtLV =SQLQuery.ReturnDataTable(@"SELECT LoanVouchar.IDLvNo, LoanVouchar.LvInvoiceNo, Convert(varchar,LoanVouchar.DateofLv,103) AS DateofLv, LoanVouchar.LoanType, LoanVouchar.LoanToEmployee, LoanVouchar.LocationID, LoanVouchar.WorkflowStatus,Product.[Name]+'-'+ProductDetails.SerialNo AS PName, ProductDetails.ModelNo, LVProduct.QTYNeed, LVProduct.ProductDetailsID, LVProduct.ProductID, Store.[Name] AS StoreName FROM LoanVouchar INNER JOIN LVProduct ON LoanVouchar.IDLvNo = LVProduct.IDLVNo INNER JOIN ProductDetails ON LVProduct.ProductDetailsID = ProductDetails.ProductDetailsID INNER JOIN Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN Store ON LoanVouchar.Store = Store.StoreAssignID WHERE LoanVouchar.LoanToEmployee='" + ddlEmployee.SelectedValue+ "' AND LoanVouchar.LocationID='"+locationId+"'");
        foreach (DataRow item in dtLV.Rows)
        {
            dtRow = dt.NewRow();
            dtRow["Date"] = item["DateofLv"];
            dtRow["VoucherNo"] = item["LvInvoiceNo"];
            dtRow["PName"] = item["PName"];
            dtRow["Quantity"] = item["QTYNeed"];
            dtRow["StoreName"] = item["StoreName"];
            dt.Rows.Add(dtRow);
        }

        DataTable dtSir = SQLQuery.ReturnDataTable(@"SELECT SIRFrom.IDSirNo, CONVERT(varchar, SIRFrom.DateOfSir, 103) AS Date, SIRFrom.SirVoucherNo, SIRFrom.LocationID, SIRFrom.LoanToEmployee, SIRProduct.QTYNeed, SIRProduct.ProductDetailsID, Product.Name + '-' + ProductDetails.SerialNo AS PName, Store.Name AS StoreName, ProductDetails.ModelNo, ProductDetails.PartNo, ProductDetails.ProductID FROM SIRFrom INNER JOIN SIRProduct ON SIRFrom.IDSirNo = SIRProduct.IDSirNo INNER JOIN ProductDetails ON SIRProduct.ProductDetailsID = ProductDetails.ProductDetailsID INNER JOIN
                  Product ON ProductDetails.ProductID = Product.ProductID INNER JOIN
                  Store ON SIRFrom.Store = Store.StoreAssignID WHERE SIRFrom.LoanToEmployee='"+ddlEmployee.SelectedValue+ "' AND SIRFrom.LocationID='"+locationId+"'");

        foreach (DataRow item in dtSir.Rows)
        {
            dtRow = dt.NewRow();
            dtRow["Date"] = item["Date"];
            dtRow["VoucherNo"] = item["SirVoucherNo"];
            dtRow["PName"] = item["PName"];
            dtRow["Quantity"] = item["QTYNeed"];
            dtRow["StoreName"] = item["StoreName"];
            dt.Rows.Add(dtRow);
        }


        gvLoanHistory.DataSource = dt;
        gvLoanHistory.EmptyDataText = "No data founds....";
        gvLoanHistory.DataBind();
    }

    private void LoadEmployee()
    {
        SQLQuery.PopulateDropDown("Select EmployeeID, Name+' ('+ Mobile+')' AS Name from Employee WHERE EmployeeID NOT IN( SELECT EmployeeInfoID FROM Logins WHERE  (LoginUserName = '" + User.Identity.Name + "')) AND LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'", ddlEmployee, "EmployeeID", "Name");
    }
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoanHistory();
    }

    protected void gvLoanHistory_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLoanHistory.PageIndex = e.NewPageIndex;
        LoanHistory();
    }

}