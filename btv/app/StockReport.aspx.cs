using RunQuery;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_StockReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindGrid();
        }
    }
    private void BindGrid()
    {
        DataTable dt = SQLQuery.ReturnDataTable("SELECT (SELECT Name FROM Product WHERE ProductID=StockRegister.ProductID) As ProductName,  Convert(varchar,Date,103) As Date,* FROM StockRegister");
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }
}