using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using RunQuery;

public partial class app_DailyTranmitterLogEntryReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnSearch.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSearch, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        if (!IsPostBack)
        {
            string lName = Page.User.Identity.Name.ToString();
            txtDateFrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");

            SqlCommand cmd = new SqlCommand("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();
            lblProject.Text = Convert.ToString(cmd.ExecuteScalar());
            cmd.Connection.Close();

            GetGrade();
            GetCategory();
            GetProductList();
            GetpackSize();

            ddLocation.DataBind();
            //QtyinStock();
            QtyinStock();
            BindItemGrid();
            BindTransmitter();
        }
        //txtInv.Text = InvIDNo();
    }

    private void GetGrade()
    {
        //string gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='" + ddSubGroup.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
        //SQLQuery.PopulateDropDown(gQuery, ddGrade, "GradeID", "GradeName");
    }
    private void GetCategory()
    {
        //string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
        //SQLQuery.PopulateDropDown(gQuery, ddCategory, "CategoryID", "CategoryName");
    }

    private void GetpackSize()
    {
        //string gQuery = "SELECT BrandID, BrandName FROM [Brands] where BrandID='" + ddSize.SelectedValue + "' AND ProjectID='" + lblProject.Text + "'ORDER BY [BrandName]";
        //SQLQuery.PopulateDropDown(gQuery,ddSize, "BrandID", "BrandName");
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BindItemGrid();
        string str = String.Empty;
        string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";

        string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        //string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
        //    "XerpReports/DailyTransmitterReport.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&type=detail";
        string url = baseUrl + "DailyTransmitterReport.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo +
                     "&type=detail";
        //iFrame.Attributes.Add("src", url);
        Response.Redirect(url);
    }

    private DataTable BindItemGrid()
    {
        DataSet ds = new DataSet();
        try
        {

            string dateFrom = " ";
            if (txtDateFrom.Text != "")
            {
                dateFrom = " AND Date>='" + Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd") + "' ";
            }

            string dateTo = " ";
            if (txtDateFrom.Text != "")
            {
                dateTo = " AND Date<='" + Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd") + "' ";
            }

            string query = " FROM DailyTransmitterLogEntry WHERE Id<>0 " + dateFrom + dateTo;
            string url = "";// SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") + "PO-Print.aspx?pono=";

            query = @"SELECT        TOP (200) Id, StationId, TransmitterMachineId, ChannelNumber, Date, TransmitterOutputPowerVideo, TransmitterOutputPowerAudio, ReflectedPowerVideo, ReflectedPowerAudio, ExciterInOperation, PumpInOperation, 
                         LiouidStaticPressure, PumpOutputPressure, LiquidTemperature, DehydratorLinePressure, PA1AGCLevel, PA1VSWR, PA1Temperature, PA1T1, PA1T2, PA1T3, PA1T4, PA1T5, PA1T6, PA2AGCLevel, PA2VSWR, PA2Temperature, 
                         PA2T1, PA2T2, PA2T3, PA2T4, PA2T5, PA2T6, PA3AGCLevel, PA3VSWR, PA3Temperature, PA3T1, PA3T2, PA3T3, PA3T4, PA3T5, PA3T6, PA4AGCLevel, PA4VSWR, PA4Temperature, PA4T1, PA4T2, PA4T3, PA4T4, PA4T5, PA4T6, 
                         PA5AGCLevel, PA5VSWR, PA5Temperature, PA5T1, PA5T2, PA5T3, PA5T4, PA5T5, PA5T6, PA6AGCLevel, PA6VSWR, PA6Temperature, PA6T1, PA6T2, PA6T3, PA6T4, PA6T5, PA6T6, PA7AGCLevel, PA7VSWR, PA7Temperature, 
                         PA7T1, PA7T2, PA7T3, PA7T4, PA7T5, PA7T6, PA8AGCLevel, PA8VSWR, PA8Temperature, PA8T1, PA8T2, PA8T3, PA8T4, PA8T5, PA8T6, Remarks, EntryBy, EntryDate" + query;
            ds = SQLQuery.ReturnDataSet(query);

            ltrtotal.Text = "Total Result: " + ds.Tables[0].Rows.Count.ToString();
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
            return ds.Tables[0];
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_warning");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
            return null;
        }

        /*
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmd = new SqlCommand(@"SELECT  Id, (SELECT  [Purpose] FROM [Purpose] WHERE [pid]= StockinDetailsRaw.Purpose) AS Purpose, 
ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal, QtyBalance, DeliveredQty, UnitWeight, 
(SELECT  [Company] FROM [Party] WHERE [PartyID]= StockinDetailsRaw.Customer) AS Customer,
(SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=StockinDetailsRaw.BrandID) AS BrandID,
(SELECT [BrandName] FROM [Brands] WHERE BrandID=StockinDetailsRaw.SizeId) AS SizeId,
(SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=StockinDetailsRaw.Color) AS Color,
(SELECT [Spec] FROM [Specifications] WHERE id=StockinDetailsRaw.Spec) AS Spec
FROM StockinDetailsRaw WHERE GodownID='" + ddGodown.SelectedValue + "' AND EntryBy=@EntryBy AND OrderID='' AND StockType='Raw'  ORDER BY Id", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //cmd.CommandText = "SELECT Id, ProductName, UnitCost, Quantity, UnitWeight, UnitType, ItemTotal FROM StockinDetailsRaw WHERE EntryBy=@EntryBy AND OrderID='' ORDER BY Id";

        cmd.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        cmd.Connection.Open();
        //ItemGrid.EmptyDataText = "No items to view...";
        //ItemGrid.DataSource = cmd.ExecuteReader();
        //ItemGrid.DataBind();
        cmd.Connection.Close();
        */

    }


    protected void ItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //ltrQty.Text = RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(QtyBalance),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "pcs, " +
        //    RunQuery.SQLQuery.ReturnString("Select ISNULL(SUM(UnitWeight),0) from StockinDetailsRaw where OrderID='' AND GodownID='" + ddGodown.SelectedValue + "'") + "kg";

    }


    private void QtyinStock()
    {
        try
        {

            GridView1.DataBind();
            BindItemGrid();
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "ERROR: " + ex.Message.ToString();
        }
        //finally { LoadFormControls(); }

    }

    protected void ddGodown_SelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        BindItemGrid();
    }

    protected void ddPurpose_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        BindItemGrid();
    }

    protected void btnExport_OnClick(object sender, EventArgs e)
    {
        //exportExcel(BindItemGrid(), "Plastic-Container-History");

        string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/FormDailyPrdn.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&type=summery";

        iFrame.Attributes.Add("src", url);
    }
    private void exportExcel(DataTable data, string reportName)
    {
        var wb = new XLWorkbook();

        // Add DataTable as Worksheet
        wb.Worksheets.Add(data);

        // Create Response
        HttpResponse response = Response;

        //Prepare the response
        response.Clear();
        response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        response.AddHeader("content-disposition", "attachment;filename=" + reportName + ".xlsx");

        //Flush the workbook to the Response.OutputStream
        using (MemoryStream MyMemoryStream = new MemoryStream())
        {
            wb.SaveAs(MyMemoryStream);
            MyMemoryStream.WriteTo(response.OutputStream);
            MyMemoryStream.Close();
        }

        response.End();
    }


    protected void ddSubGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        QtyinStock();
        GetGrade();
        GetCategory();
        BindItemGrid();
        GetpackSize();

    }

    protected void ddGrade_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetCategory();
    }

    protected void ddcategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GetProductList();
    }
    private void GetProductList()
    {
        //string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory.SelectedValue +
        //                "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        //SQLQuery.PopulateDropDown(gQuery, ddItemName, "ProductID", "ItemName");

        //ltrUnitType.Text = SQLQuery.ReturnString("Select UnitType FROM Products where ProductID='" + ddItemName.SelectedValue + "'");

        //if (IsPostBack)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#ItemDetails';", true);
        //}

        //LoadItemsPanel();
    }

    protected void ddGroup_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnAllShift_OnClick(object sender, EventArgs e)
    {
        string dateFrom = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
        string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");

        string url = SQLQuery.ReturnString("Select DefaultLink from Settings_Project where sid=1") +
            "XerpReports/DailyPrdnAllShift.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&type=AllShift";

        iFrame.Attributes.Add("src", url);
    }

    private void BindTransmitter()
    {
        SQLQuery.PopulateDropDown("SELECT id, TransmitterName FROM Transmitters WHERE StationID = '" + ddLocation.SelectedValue + "'", ddTransmitterType, "id", "TransmitterName");
    }
    protected void ddLocation_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindTransmitter();
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }

    protected void btnSearch_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDateTime(txtDateFrom.Text) <= Convert.ToDateTime(txtDateTo.Text))
            {
                string str = String.Empty;
                string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";

                string dateForm = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
                string dateTo = Convert.ToDateTime(txtDateTo.Text).ToString("yyyy-MM-dd");
                string type = "";
                if (ddTransmitterType.SelectedItem.Text.ToLower().Contains("thales"))
                {
                    type = "thales";
                }
                else
                {
                    type = "";
                }
                string urlx = "stationId=" + ddLocation.SelectedValue + "&type=" + type + "&transmitterMachineId=" + ddTransmitterType.SelectedValue + "&dateFrom=" + dateForm + "&dateTo=" + dateTo;

                str = baseUrl + "DailyTransmitterReport.aspx?" + urlx;

                Response.Redirect(str);
                //iFrame.Attributes.Add("src", str);
                //Page.ClientScript.RegisterStartupScript(base.GetType(), "Open Window", "window.open('" + str + "' , 'no', 'height=680, width= 1024, top=300, left=500, location=1, scrollbars=1, resizable=1');", true);
            }
            else
            {
                MessageBox("Invalid Date Range!");
            }
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg2);
        }
    }
}