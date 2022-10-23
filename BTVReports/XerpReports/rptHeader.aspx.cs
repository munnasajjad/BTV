using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using RunQuery;

namespace Oxford.XerpReports
{
    public partial class rptHeader : System.Web.UI.Page
    {
        ReportDocument rpt = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            //DataTable dtx2 = SQLQuery.ReturnDataTable(@"SELECT TOP (1) [CompanyID], [CompanyName], [MobileNo], [Email], [ProjectName], [CompanySpeciality], [CompanyAddress], [AuthorityName], (Select PhotoURL from Photos where PhotoID=Company.Logo) AS Logo  FROM [Company]");
            //DataTableReader drx = dtx2.CreateDataReader();
            //XerpDataSet ds = new XerpDataSet();
            //ds.Load(drx, LoadOption.OverwriteChanges, ds.Company);
            //rpt.Subreports["CrptHeader.rpt"].SetDataSource((DataTable)ds.Company);

            //ReportDocument rpt = new ReportDocument();
            //rpt.Load(Server.MapPath("CrptHeader.rpt"));
            DataSet dsx = RunQuery.SQLQuery.ReturnDataSet("SELECT TOP (1) [CompanyID], [CompanyName], [MobileNo], [Email], [ProjectName], [CompanySpeciality], [CompanyAddress], [AuthorityName], (Select PhotoURL from Photos where PhotoID=Company.Logo) AS Logo  FROM [Company]");

            DataTable dt = dsx.Tables[0];

            //DataColumn photo = new DataColumn("Uploads/no-photo.jpg", Type.GetType("System.Byte[]"));
            //dt.Columns.Add(photo);

            AddImageColumn(dt, "photo");
            for (int index = 0; index < dt.Rows.Count; index++)
            {
                if (dt.Rows[index]["Logo"].ToString() != "")
                {
                    if (File.Exists(this.Server.MapPath(dt.Rows[index]["Logo"].ToString())))
                    {

                        LoadImage(dt.Rows[index], "photo", Server.MapPath(dt.Rows[index]["Logo"].ToString()));
                    }
                    else
                    {
                        LoadImage(dt.Rows[index], "photo", "Uploads/no-photo.jpg");
                    }
                }
                else
                {
                    LoadImage(dt.Rows[index], "photo", "Uploads/no-photo.jpg");
                }
            }

            //rpt.SetDataSource(dt);
            //CrystalReportViewer1.ReportSource = rpt;
            DataTableReader dt2 = dt.CreateDataReader();
            XerpDataSet ds = new XerpDataSet();
            ds.Load(dt2, LoadOption.OverwriteChanges, ds.Company);
            rpt.Subreports["CrptHeader.rpt"].SetDataSource((DataTable)ds.Company);

        }

        private void AddImageColumn(DataTable objDataTable, string strFieldName)
        {
            try
            {
                DataColumn objDataColumn = new DataColumn(strFieldName, Type.GetType("System.Byte[]"));
                objDataTable.Columns.Add(objDataColumn);
            }
            catch (Exception ex)
            {

            }
        }
        private void LoadImage(DataRow objDataRow, string strImageField, string FilePath)
        {
            try
            {
                FileStream fs = new FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                byte[] Image = new byte[fs.Length];
                fs.Read(Image, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                objDataRow[strImageField] = Image;
            }
            catch (Exception ex)
            {

            }
        }

        protected void CrystalReportViewer1_OnUnload(object sender, EventArgs e)
        {
            rpt.Close();
            rpt.Dispose();
            CrystalReportViewer1.Dispose();
        }
    }
}