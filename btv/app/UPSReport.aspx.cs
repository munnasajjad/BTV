using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using RunQuery;


public partial class app_UPSReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateFrom.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
            txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindCust();
            BindMainOfficeLocation();
            //LoadGridData();
        }
    }
    private void bindCust()
    {
        string query = "";
        if (!User.IsInRole("Super Admin"))
        {
            query = "Where LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";

        }
        SQLQuery.PopulateDropDown("SELECT Name, LocationID from Location " + query, ddLocation, "LocationID", "Name");
    }
    private void BindMainOfficeLocation()
    {
        SQLQuery.PopulateDropDown("SELECT  Id, MainOfficeLocationName FROM MainOfficeLocation WHERE MainOfficeId ='" + ddLocation.SelectedValue + "'", ddMainOfficeLocation, "Id", "MainOfficeLocationName");
        if (ddMainOfficeLocation.Text == "")
        {
            ddMainOfficeLocation.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }


    protected void btnReset_OnClick(object sender, EventArgs e)
    {
        ddLocation.DataBind();
        txtDateFrom.Text = "01/" + DateTime.Now.ToString("MM/yyyy");
        txtDateTo.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }
    private void MessageBox(string msg)
    {
        Label lbl = new Label();
        lbl.Text = "<script language='javascript'>" + Environment.NewLine + "window.alert('" + msg + "')</script>";
        Page.Controls.Add(lbl);
    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }
    protected void btnShow_Click(object sender, EventArgs e)
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
                string urlx = "MainOfficeId=" + ddLocation.SelectedValue + "&LocationId=" + ddMainOfficeLocation.SelectedValue + "&DateForm=" + dateForm + "&DateTo=" + dateTo;

                str = baseUrl + "UPSReport.aspx?" + urlx;

                Response.Redirect(str);
                //Page.ClientScript.RegisterStartupScript(base.GetType(), "Open Window", "window.open('" + str + "' , 'no', 'height=680, width= 1024, top=300, left=500, location=1, scrollbars=1, resizable=1');", true);
            }
            else
            {
                MessageBox("Invalid Date Range!");
            }
        }
        catch (Exception exception)
        {
            Notify(exception.ToString(), "error", lblMsg);
        }
    }
    private void SecondGrid()
    {
        SqlCommand cmd = new SqlCommand("SELECT CollectionNo, CollectionDate, PartyName, PaidAmount, ChqDetail, ChqDate FROM Collection WHERE (IsApproved = 'p') and PartyName='" + ddLocation.SelectedValue + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Connection.Open();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet("Board");

        da.Fill(ds, "Board");
        SqlDataReader dr = cmd.ExecuteReader();
        DataTable dt = ds.Tables["Board"];

        cmd.Connection.Close();
        cmd.Connection.Dispose();
        //GridView2.DataSource = dt;
        //GridView2.DataBind();

    }

    protected void ddCustomer_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //ddPO.DataBind();
        //search();
        BindMainOfficeLocation();
    }

    public static class ResponseHelper
    {
        public static void Redirect(string url, string target, string windowFeatures)
        {
            HttpContext context = HttpContext.Current;

            if ((String.IsNullOrEmpty(target) ||
                target.Equals("_self", StringComparison.OrdinalIgnoreCase)) &&
                String.IsNullOrEmpty(windowFeatures))
            {

                context.Response.Redirect(url);
            }
            else
            {
                Page page = (Page)context.Handler;
                if (page == null)
                {
                    throw new InvalidOperationException(
                        "Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }

                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page,
                    typeof(Page),
                    "Redirect",
                    script,
                    true);
            }
        }
    }
}