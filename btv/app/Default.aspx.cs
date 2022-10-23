using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class AdminCentral_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                Session["ProjectID"] = SQLQuery.ProjectID(User.Identity.Name);
                //News Scroll
                SqlCommand cmdxxe = new SqlCommand("Select FullNews from NewsUpdates where ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "' AND MsgID=(Select max (MsgID) from NewsUpdates where Msgfor='News')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmdxxe.Connection.Open();
                lblNews.Text = Convert.ToString(cmdxxe.ExecuteScalar());

                cmdxxe.Connection.Close();
                cmdxxe.Connection.Dispose();

                //int outQty = Convert.ToInt32(SQLQuery.ReturnString("Select ISNULL(SUM(InQty)-SUM(OutQty),0) from SMSBalance WHERE ProjectId='" + SQLQuery.ProjectID(Page.User.Identity.Name) + "'"));

                //ltrsms.Text = "You have remaining <span>" + outQty + "</span> SMS.!";

                txtDate.Text = DateTime.Now.AddDays(1).ToString("dd/MM/yyyy");
                txtDateFrom.Text = DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy");
                txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //ltrtotal.Text = "0";
                txtDate.Text = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy");
                lblUserName.Text = Page.User.Identity.Name.ToString();
                // GridView2.DataBind();

                BindLocation();
                BindCenterID();
                BindDepartmentSection();
                BindDDStoreID();

                lblResponsibilities.Text = SQLQuery.ReturnString("Select '<b>'+Name+'</b> (<i>'+Designation+'</i>)<br/>'+ Responsibilities from Employee where LoginID='" + User.Identity.Name + "'").Replace("\r\n", "<br />");
                Image2.ImageUrl = SQLQuery.ReturnString("Select '.'+PhotoURL from Photos WHERE PhotoID=(Select Photo from Employee where LoginID='" + User.Identity.Name + "')");

                string startDate = DateTime.Today.ToString("yyyy-MM-01");
                string endDate = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
                SQLQuery.IsUserActive(User.Identity.Name);
                string isActive = SQLQuery.ReturnString("SELECT IsActive FROM Users WHERE Username = '" + User.Identity.Name + "'");
                //int loginCount =int.Parse(SQLQuery.ReturnString("SELECT LoginCount FROM Users WHERE Username = '" + User.Identity.Name + "'"));
                if (isActive == "False")
                {
                    Notify("Please change your default password. Please note that your account will be locked, in case your default password is not changed", "warning", lblNotify);
                    pnlNotify.Visible = true;
                }

                //double authors = Convert.ToDouble(SQLQuery.ReturnString("SELECT COUNT(UID) FROM Users"));
                string query = "";
                if (ddLocationID.SelectedValue != "0")
                {
                    query += " WHERE LocationID = '" + ddLocationID.SelectedValue + "'";
                }
                labelTotalEmployee.Text = SQLQuery.ReturnString("SELECT COUNT(EmployeeID) AS TotalEmployee FROM Employee" + query);
                labelTotalDepartment.Text = SQLQuery.ReturnString("SELECT COUNT(DepartmentSectionID) FROM DepartmentSection" + query);
                double authors = Convert.ToDouble(SQLQuery.ReturnString("SELECT COUNT(dbo.Users.UID) AS TotalUsers FROM dbo.Users INNER JOIN dbo.Employee ON dbo.Users.BranchName = dbo.Employee.EmployeeID" + query));
                labelUsers.Text = "<b style='color:#ff9900'>" + FormatLocal(authors) + " </b>";
                double monthTarget = Convert.ToDouble(SQLQuery.ReturnString("SELECT COUNT(CenterID) FROM Center" + query));
                labelTotalFunctionalOffice.Text = "<b style='color:#0f5aa8'>" + FormatLocal(monthTarget) + " </b>";
                labelTotalStore.Text = SQLQuery.ReturnString("SELECT  COUNT(StoreAssignID)  FROM Store" + query);
                //lblTotalIssue.Text = SQLQuery.ReturnString("SELECT COUNT(sl) FROM ");

                //string late = SQLQuery.ReturnString("Select COUNT(LID) FROM Attendance WHERE Status='Late' AND EmployeeID='" + User.Identity.Name + "' AND InTime>'" + startDate + "' AND InTime<'" + endDate + "' ");
                //string leave = SQLQuery.ReturnString("Select COUNT(LID) FROM Attendance WHERE Status='Leave' AND EmployeeID='" + User.Identity.Name + "' AND InTime>'" + startDate + "' AND InTime<'" + endDate + "' ");
                //string present = SQLQuery.ReturnString("Select COUNT(LID) FROM Attendance WHERE (Status='Present' OR Status='Late') AND EmployeeID='" + User.Identity.Name + "' AND InTime>'" + startDate + "' AND InTime<'" + endDate + "' ");

                //string totalHoliday = SQLQuery.ReturnString("Select COUNT(date) from Holydays where Date>='" + startDate + "' AND Date<='" + endDate + "'");
                //double workingDays = (DateTime.Today.AddDays(1) - Convert.ToDateTime(startDate)).TotalDays - Convert.ToDouble(totalHoliday);
                //int absentDays = Convert.ToInt32(workingDays - Convert.ToDouble(present) - Convert.ToDouble(leave));

                //string avg = SQLQuery.ReturnString("Select ISNULL(AVG(WorkingTimeHr),0) FROM Attendance WHERE (Status='Present' OR Status='Late') AND EmployeeID='" + User.Identity.Name + "' AND InTime>'" + startDate + "' AND InTime<'" + endDate + "' ");

                //lblHistory.Text = "Late: " + late + " days. &nbsp; Absent: " + absentDays + " days. &nbsp; Leave: " + leave + " days. &nbsp; Present: " + present + " days. AVG: " + Math.Round(Convert.ToDecimal(avg), 2) + " hr.";
                stats();
                PopulateSubscription();
                string package = SQLQuery.ReturnString("Select Package from Projects where VID='" + SQLQuery.ProjectID(Page.User.Identity.Name) + "'");

                //SubscriptionPayment();

                string lName = Page.User.Identity.Name.ToString();
                string prjId = SQLQuery.ProjectID(Page.User.Identity.Name);
                string Core = SQLQuery.ReturnString("Select Accounting from Projects where Package='1' and VID='" + prjId + "'");
                if (Core == "1")
                {
                    SwitchBar.Attributes.Remove("class");
                    SwitchBar.Attributes.Add("class", "hidden");
                }
                txtHighlyPurchasedItem.Text = "10";

                ItemInStoreChart();
                InventoryLevelByCategoryStore();
                PurchaseAmountAnalysis();
                HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
                ItemConsumption(txtHighlyPurchasedItem.Text);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }

    }

    private string FormatLocal(double value)
    {
        return String.Format("{0:n}", value).Replace(".00", "");
    }

    private void stats()
    {
        try
        {
            string pid = SQLQuery.ProjectID(User.Identity.Name);
            string dateFrom = Convert.ToDateTime(DateTime.Now.ToString("01/MM/yyyy")).ToString("yyyy-MM-dd");
            string dateTo =
                Convert.ToDateTime(DateTime.Now.AddMonths(1).ToString("01/MM/yyyy")).AddDays(-1).ToString("yyyy-MM-dd");


            ListView1.DataBind();

            DataTable dtx =
                SQLQuery.ReturnDataTable(
                    @"SELECT TOP (1) VID, Type, ProjectName, ShowHeader, ReportHeader, NTamount, VAT, Logo, 
TrialDate, DoctorComm, Developed, EntryBy, EntryDate, ProjectId, UserLimit, Accounting, Inventory, Payroll FROM Projects WHERE VID='" +
                    pid + "'");
            foreach (DataRow drx in dtx.Rows)
            {
                string showStat = drx["NTamount"].ToString();
                if (showStat == "Show")
                {
                    pnlStat.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Convert.ToDateTime(txtDate.Text) >= DateTime.Today)
        {
            string dt = Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd");
            RunQuery.SQLQuery.ExecNonQry("Insert into Tasks (TaskName, TaskDetails, DeadLine, Priority, Status, EntryBy,ProjectId) VALUES ('Admin', '" + txtDetail.Text + "', '" + dt + "', '" + ddType.SelectedValue + "', 'Pending', '" + Page.User.Identity.Name.ToString() + "','" + SQLQuery.ProjectID(User.Identity.Name) + "')");
            GridView1.DataBind();
        }
        else
        {
            lblMsg.Text = "Schedule Date Must Be Greater Then Today.";
        }
    }
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView1.SelectedIndex);
            Label CrID = GridView1.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ReturnString("Update Tasks set Status='Done' where tid= " + CrID.Text);
            GridView1.DataBind();
        }
        catch (Exception ex)
        {

        }
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView1.Rows[index].FindControl("Label1") as Label;
            string OrderID = RunQuery.SQLQuery.ReturnString("Select Status from Tasks where tid='" + lblItemCode.Text + "'AND ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "'");

            if (OrderID == "Pending")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE Tasks WHERE tid=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                GridView1.DataBind();
                //uPanel.Update();
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Task deleted ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Pending Task is Locked for delete...";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }
    //protected void GridView3_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int index = Convert.ToInt32(GridView1.SelectedIndex);
    //        Label CrID = GridView3.Rows[index].FindControl("Label1") as Label;

    //        string isPending = SQLQuery.ReturnString("Select Status from Tasks  where tid= " + CrID.Text);

    //        if (isPending == "Done")
    //        {
    //            SQLQuery.ReturnString("Update Tasks set Status='Pending' where tid= " + CrID.Text);
    //        }
    //        else
    //        {
    //            SQLQuery.ReturnString("Update Tasks set Status='Done' where tid= " + CrID.Text);
    //        }

    //        GridView3.DataBind();
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblItemCode = GridView2.Rows[index].FindControl("Label1") as Label;
            string OrderID = SQLQuery.ReturnString("Select Status from Tasks where tid='" + lblItemCode.Text + "'");

            if (OrderID == "Pending")
            {
                SqlCommand cmd7 = new SqlCommand("DELETE Tasks WHERE tid=" + lblItemCode.Text + "", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                cmd7.Connection.Open();
                cmd7.ExecuteNonQuery();
                cmd7.Connection.Close();

                //GridView2.DataBind();
                //uPanel.Update();
                lblMsg.Attributes.Add("class", "xerp_warning");
                lblMsg.Text = "Task deleted ...";
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Pending Task is Locked for delete...";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "ERROR: " + ex.Message.ToString();
        }
    }

    private void PopulateSubscription()
    {
        try
        {
            //SQLQuery.PopulateDropDown("SELECT Id, Duration FROM SubsDuration Where Id<>1", ddSubscription, "Id", "Duration");

        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
        }
    }

    protected void lbActivity_Click(object sender, EventArgs e)
    {
        lbActivity.Visible = false;
        tblActivity.Visible = true;
        txtActivity.Focus();
    }

    protected void Button4_Click(object sender, EventArgs e)
    {
        lbActivity.Visible = true;
        tblActivity.Visible = false;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (txtActivity.Text.Length > 3)
        {
            string dt = DateTime.Now.ToString("yyyy-MM-dd");
            RunQuery.SQLQuery.ExecNonQry(@"Insert into Tasks (TaskName, TaskDetails, DeadLine, Priority, Status, EntryBy,ProjectId)
                VALUES ('Activity', '" + txtActivity.Text + "', '" + dt + "', 'Low', 'Pending', '" + Page.User.Identity.Name.ToString() + "','" + SQLQuery.ProjectID(User.Identity.Name) + "')");
            txtActivity.Text = "";
            lbActivity.Visible = true;
            tblActivity.Visible = false;
            GridView2.DataBind();
            //pnl.Update();
        }
        else
        {
            Notify("Please write more bigger activity! ", "error", lblMsg);
        }
    }
    //Message & Notify For Alerts
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(GridView2.SelectedIndex);
            Label CrID = GridView2.Rows[index].FindControl("Label1") as Label;
            RunQuery.SQLQuery.ReturnString("Update Tasks set Status='Done' where tid= " + CrID.Text);
            GridView2.DataBind();
        }
        catch (Exception ex)
        {

        }
    }



    protected void btnRegister_OnClick(object sender, EventArgs e)
    {
        string company = SQLQuery.ProjectID(Page.User.Identity.Name);
        //string validity = SQLQuery.ReturnString("Select Month from SubsDuration where id='" + ddSubscription.SelectedValue+"'");
        //SQLQuery.ExecNonQry("Insert into SubscriptionPayment( Company, Subscription, ExpDate, Rate, NetAmnt, Discount, Total, PayMethod, TransId, Status, EntryBy) " +                                                        "Values('" + company + "','" + validity + "','" + DateTime.Now.AddMonths(Convert.ToInt32(validity)).ToString("yyyy-MM-dd") + "','"+ Session["Price"] + "','"+ Session["Gross"] + "','"+ Session["Discount"] + "','" + txtAmmount.Text + "','" + ddPayMethod.SelectedItem.Text + "','" + txtTransicId.Text + "','Pending','" + Page.User.Identity.Name + "')");
    }

    protected void ddSubscription_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //SubscriptionPayment();
    }
    private void BindLocation()
    {
        string query = "";
        if (Page.User.IsInRole("Admin") || Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer") || Page.User.IsInRole("User"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        SQLQuery.PopulateDropDown("SELECT LocationID, Name from Location " + query, ddLocationID, "LocationID", "Name");
        if (Page.User.IsInRole("Super Admin"))
        {
            ddLocationID.Items.Insert(0, new ListItem("--- All ---", "0"));
        }
    }
    private void BindCenterID()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer") || Page.User.IsInRole("User"))
        {
            query = " WHERE LocationID='" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' ";
        }

        string strQuery = "SELECT CenterID, Name FROM Center " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddCenterID, "CenterID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddCenterID.Items.Insert(0, new ListItem("--- Select ---", "0"));
        }
    }
    private void BindDepartmentSection()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            query = " WHERE LocationID = '" + ddLocationID.SelectedValue + "' AND CenterID = '" + ddCenterID.SelectedValue + "' ";
        }
        else if (Page.User.IsInRole("Department Admin") || Page.User.IsInRole("Senior Store Officer") || Page.User.IsInRole("User"))
        {
            query = " WHERE LocationID = '" + RunQuery.SQLQuery.GetLocationID(User.Identity.Name) + "' AND CenterID = '" + RunQuery.SQLQuery.GetCenterId(User.Identity.Name) + "' AND DepartmentSectionID='" + RunQuery.SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'";
        }

        string strQuery = @"SELECT DepartmentSectionID, Name FROM DepartmentSection " + query + "";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddDepartmentSectionID, "DepartmentSectionID", "Name");
        if (Page.User.IsInRole("Super Admin") || Page.User.IsInRole("Admin"))
        {
            ddDepartmentSectionID.Items.Insert(0, new ListItem("--- all ---", "0"));
        }
    }
    private void BindDDStoreID()
    {
        string strQuery = @"SELECT StoreAssignID, StoreID, Name, Description, LocationID, CenterID, DepartmentSectionID
            FROM Store WHERE (DepartmentSectionID = '" + ddDepartmentSectionID.SelectedValue + "')";
        SQLQuery.PopulateDropDownWithoutSelect(strQuery, ddStoreID, "StoreAssignID", "Name");
        if (ddStoreID.Text == "")
        {
            ddStoreID.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    protected void ddLocationID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string query = "";
        if (ddLocationID.SelectedValue != "0")
        {
            query += " WHERE LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        labelTotalEmployee.Text = SQLQuery.ReturnString("SELECT COUNT(EmployeeID) AS TotalEmployee FROM Employee" + query);
        labelTotalDepartment.Text = SQLQuery.ReturnString("SELECT COUNT(DepartmentSectionID) FROM DepartmentSection" + query);
        double authors = Convert.ToDouble(SQLQuery.ReturnString("SELECT COUNT(dbo.Users.UID) AS TotalUsers FROM dbo.Users INNER JOIN dbo.Employee ON dbo.Users.BranchName = dbo.Employee.EmployeeID" + query));
        labelUsers.Text = "<b style='color:#ff9900'>" + FormatLocal(authors) + " </b>";
        double monthTarget = Convert.ToDouble(SQLQuery.ReturnString("SELECT COUNT(CenterID) FROM Center" + query));
        labelTotalFunctionalOffice.Text = "<b style='color:#0f5aa8'>" + FormatLocal(monthTarget) + " </b>";
        labelTotalStore.Text = SQLQuery.ReturnString("SELECT  COUNT(StoreAssignID)  FROM Store" + query);
        BindCenterID();
        BindDepartmentSection();
        BindDDStoreID();
        ItemInStoreChart();
        InventoryLevelByCategoryStore();
        PurchaseAmountAnalysis();
        HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
        ItemConsumption(txtHighlyPurchasedItem.Text);
    }
    private void ItemInStoreChart()
    {
        string query = "";
        if (ddLocationID.SelectedValue != "0")
        {
            query += " AND S.LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        if (ddCenterID.SelectedValue != "0")
        {
            query += " AND S.CenterID = '" + ddCenterID.SelectedValue + "'";
        }
        if (ddDepartmentSectionID.SelectedValue != "0")
        {
            query += " AND S.DepartmentID = '" + ddDepartmentSectionID.SelectedValue + "'";
        }
        if (ddStoreID.SelectedValue != "0")
        {
            query += " AND S.StoreID = '" + ddStoreID.SelectedValue + "'";
        }
        if (txtDateFrom.Text != "")
        {
            string fromDate = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            query += " AND S.Date>= '" + fromDate + "'";
        }
        if (txtDateTo.Text != "")
        {
            string toDate = Convert.ToDateTime(txtDateTo.Text).AddDays(1).ToString("yyyy-MM-dd");
            query += " AND S.Date< '" + toDate + "'";
        }
        DataTable weekChart = SQLQuery.ReturnDataTable(@"SELECT (REPLACE(P.Name,'''','""')+'('+CAST((SUM(S.StockIn) - SUM(S.SellQty)) as varchar(100))+')') AS Name,S.LocationID,S.CenterID,S.DepartmentID,S.StoreID,(SUM(S.StockIn) - SUM(S.SellQty)) AS Quantity
FROM StockRegister AS S INNER JOIN Product AS P ON S.ProductID = P.ProductID WHERE P.ProductID <> '0' " + query + " GROUP BY Name,S.LocationID,S.CenterID,S.DepartmentID,S.StoreID");
        string array = "['Name', 'Quantity'],";
        foreach (DataRow weekRow in weekChart.Rows)
        {
            array += "['" + weekRow["Name"] + "', " + weekRow["Quantity"] + "],";
        }
        itemInStoreLiteral.Text = array.Trim().TrimEnd(',');
    }

    private void InventoryLevelByCategoryStore()
    {
        string query = "";
        if (ddLocationID.SelectedValue != "0")
        {
            query += " AND SR.LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        if (ddCenterID.SelectedValue != "0")
        {
            query += " AND SR.CenterID = '" + ddCenterID.SelectedValue + "'";
        }
        if (ddDepartmentSectionID.SelectedValue != "0")
        {
            query += " AND SR.DepartmentID = '" + ddDepartmentSectionID.SelectedValue + "'";
        }
        if (ddStoreID.SelectedValue != "0")
        {
            query += " AND SR.StoreID = '" + ddStoreID.SelectedValue + "'";
        }
        if (txtDateFrom.Text != "")
        {
            string fromDate = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            query += " AND SR.Date>= '" + fromDate + "'";
        }
        if (txtDateTo.Text != "")
        {
            string toDate = Convert.ToDateTime(txtDateTo.Text).AddDays(1).ToString("yyyy-MM-dd");
            query += " AND SR.Date< '" + toDate + "'";
        }
        string sql = "SELECT SR.CatgoryId, C.Name, SUM(SR.StockIn) AS Quantity FROM StockRegister AS SR INNER JOIN ProductCategory  AS C ON C.ProductCategoryID = SR.CatgoryId WHERE SR.CatgoryId <> '0' " + query + " GROUP BY SR.CatgoryId, C.Name";
        DataTable weekChart = SQLQuery.ReturnDataTable(sql);
        string array = "['Name', 'Quantity'],";
        foreach (DataRow weekRow in weekChart.Rows)
        {
            array += "['" + weekRow["Name"] + "', " + weekRow["Quantity"] + "],";
        }
        inventoryLevelByCategoryStore.Text = array.Trim().TrimEnd(',');
    }

    private void PurchaseAmountAnalysis()
    {
        string query = "";
        if (ddLocationID.SelectedValue != "0")
        {
            query += " AND S.LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        if (ddCenterID.SelectedValue != "0")
        {
            query += " AND S.CenterID = '" + ddCenterID.SelectedValue + "'";
        }
        if (ddDepartmentSectionID.SelectedValue != "0")
        {
            query += " AND S.DepartmentSectionID = '" + ddDepartmentSectionID.SelectedValue + "'";
        }
        if (ddStoreID.SelectedValue != "0")
        {
            query += " AND S.StoreAssignID = '" + ddStoreID.SelectedValue + "'";
        }
        if (txtDateFrom.Text != "")
        {
            string fromDate = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            query += " AND GF.DateOfGRN>= '" + fromDate + "'";
        }
        if (txtDateTo.Text != "")
        {
            string toDate = Convert.ToDateTime(txtDateTo.Text).AddDays(1).ToString("yyyy-MM-dd");
            query += " AND GF.DateOfGRN< '" + toDate + "'";
        }
        string sql = @"SELECT GF.StoreID, S.Name, SUM(GF.TotalAmount) AS TotalAmount FROM GRNFrom AS GF INNER JOIN
        Store AS S ON GF.StoreID = S.StoreAssignID WHERE GF.SaveMode = 'Submitted' AND GF.WorkflowStatus='Approved' " + query + " GROUP BY GF.StoreID, S.Name";
        DataTable weekChart = SQLQuery.ReturnDataTable(sql);
        string array = "['Name', 'TotalAmount'],";
        foreach (DataRow weekRow in weekChart.Rows)
        {
            array += "['" + weekRow["Name"] + "', " + weekRow["TotalAmount"] + "],";
        }
        purchaseAmountAnalysis.Text = array.Trim().TrimEnd(',');
    }

    private void HighlyPurchasedItem(string n)
    {
        string query = "";
        if (ddLocationID.SelectedValue != "0")
        {
            query += " AND GF.LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        if (ddCenterID.SelectedValue != "0")
        {
            query += " AND S.CenterID = '" + ddCenterID.SelectedValue + "'";
        }
        if (ddDepartmentSectionID.SelectedValue != "0")
        {
            query += " AND S.DepartmentSectionID = '" + ddDepartmentSectionID.SelectedValue + "'";
        }
        if (ddStoreID.SelectedValue != "0")
        {
            query += " AND GF.StoreID = '" + ddStoreID.SelectedValue + "'";
        }
        if (txtDateFrom.Text != "")
        {
            string fromDate = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            query += " AND GF.DateOfGRN>= '" + fromDate + "'";
        }
        if (txtDateTo.Text != "")
        {
            string toDate = Convert.ToDateTime(txtDateTo.Text).AddDays(1).ToString("yyyy-MM-dd");
            query += " AND GF.DateOfGRN< '" + toDate + "'";
        }
        if (txtHighlyPurchasedItem.Text != "")
        {
            n = " TOP(" + n + ") ";
        }
        query = @"SELECT" + n + " GP.ProductID, (REPLACE(P.Name,'''','\"\"')+' ('+CAST((SUM(GP.ReceiveProduct) - SUM(GP.RejectProduct)) as varchar(100))+') '+S.Name) AS Name, S.StoreID, S.Name AS StoreName,  SUM(GP.ReceiveProduct) - SUM(GP.RejectProduct) AS Quantity FROM GRNProduct AS GP INNER JOIN GRNFrom AS GF ON GP.GRNInvoiceNo = GF.GRNInvoiceNo INNER JOIN Product AS P ON GP.ProductID = P.ProductID INNER JOIN Store AS S ON GF.StoreID = S.StoreAssignID WHERE GF.SaveMode = 'Submitted' AND GF.WorkflowStatus = 'Approved' " + query + " GROUP BY GP.ProductID, P.Name, S.StoreID, S.Name ORDER BY Quantity DESC";
        DataTable weekChart = SQLQuery.ReturnDataTable(query);
        //        DataTable weekChart = SQLQuery.ReturnDataTable(@"SELECT REPLACE(P.Name,'''','""') AS Name,S.LocationID,S.CenterID,S.DepartmentID,S.StoreID,(SUM(S.StockIn) - SUM(S.SellQty)) AS Quantity
        //FROM StockRegister AS S INNER JOIN Product AS P ON S.ProductID = P.ProductID WHERE S.LocationID = '" + ddLocationID.SelectedValue + "' " + query + " GROUP BY Name,S.LocationID,S.CenterID,S.DepartmentID,S.StoreID");
        string array = "['Name', 'Quantity'],";
        foreach (DataRow weekRow in weekChart.Rows)
        {
            array += "['" + weekRow["Name"] + "', " + weekRow["Quantity"] + "],";
        }
        test.Text = array.Trim().TrimEnd(',');
    }

    private void ItemConsumption(string n)
    {
        string query = "";
        if (ddLocationID.SelectedValue != "0")
        {
            query += " AND LocationID = '" + ddLocationID.SelectedValue + "'";
        }
        if (ddCenterID.SelectedValue != "0")
        {
            query += " AND CenterID = '" + ddCenterID.SelectedValue + "'";
        }
        if (ddDepartmentSectionID.SelectedValue != "0")
        {
            query += " AND DepartmentSectionID = '" + ddDepartmentSectionID.SelectedValue + "'";
        }
        if (ddStoreID.SelectedValue != "0")
        {
            query += " AND StoreID = '" + ddStoreID.SelectedValue + "'";
        }
        if (txtDateFrom.Text != "")
        {
            string fromDate = Convert.ToDateTime(txtDateFrom.Text).ToString("yyyy-MM-dd");
            query += " AND EntryDate>= '" + fromDate + "'";
        }
        if (txtDateTo.Text != "")
        {
            string toDate = Convert.ToDateTime(txtDateTo.Text).AddDays(1).ToString("yyyy-MM-dd");
            query += " AND EntryDate< '" + toDate + "'";
        }
        if (txtHighlyPurchasedItem.Text != "")
        {
            n = "TOP(" + n + ")";
        }

        DataTable weekChart = SQLQuery.ReturnDataTable(@"SELECT " + n + " ProductId, (REPLACE(Name,'''','\"\"')+' - '+StoreName) AS Name, StoreID, StoreName, SUM(QTYDelivered) AS Quantity FROM VwItemConsumptionSIRLV WHERE ProductId<> '0'" + query + " GROUP BY ProductId, Name, StoreID, StoreName");
        string array = "['Name', 'Quantity'],";
        foreach (DataRow weekRow in weekChart.Rows)
        {
            array += "['" + weekRow["Name"] + "', " + weekRow["Quantity"] + "],";
        }
        consumption.Text = array.Trim().TrimEnd(',');
    }

    protected void ddCenterID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDepartmentSection();
        BindDDStoreID();
        ItemInStoreChart();
        InventoryLevelByCategoryStore();
        PurchaseAmountAnalysis();
        HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
        ItemConsumption(txtHighlyPurchasedItem.Text);
    }

    protected void ddDepartmentSectionID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindDDStoreID();
        ItemInStoreChart();
        InventoryLevelByCategoryStore();
        PurchaseAmountAnalysis();
        HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
        ItemConsumption(txtHighlyPurchasedItem.Text);
    }

    protected void ddStoreID_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ItemInStoreChart();
        InventoryLevelByCategoryStore();
        PurchaseAmountAnalysis();
        HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
        ItemConsumption(txtHighlyPurchasedItem.Text);
    }

    protected void txtDateFrom_OnTextChanged(object sender, EventArgs e)
    {
        ItemInStoreChart();
        InventoryLevelByCategoryStore();
        PurchaseAmountAnalysis();
        HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
        ItemConsumption(txtHighlyPurchasedItem.Text);
    }

    protected void txtDateTo_OnTextChanged(object sender, EventArgs e)
    {
        ItemInStoreChart();
        InventoryLevelByCategoryStore();
        PurchaseAmountAnalysis();
        HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
        ItemConsumption(txtHighlyPurchasedItem.Text);
    }

    protected void txtHighlyPurchasedItem_OnTextChanged(object sender, EventArgs e)
    {
        HighlyPurchasedItem(txtHighlyPurchasedItem.Text);
        ItemConsumption(txtHighlyPurchasedItem.Text);
    }
}
