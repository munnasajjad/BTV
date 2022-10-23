using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;

public partial class sql_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        if (!IsPostBack)
        {
            btnExecute.Attributes.Add("onclick",
                  " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnExecute, null) + ";");

            //txtTeacherID.Text = RunQuery.SQLQuery.ReturnInvNo("Teachers", "sl", "TeacherID");
        }
    }

    protected void Button1_OnClick(object sender, EventArgs e)
    {
        if (txtPass.Text == "3p3200170")
        {
            pnlPassword.Visible = false;
            pnlExecute.Visible = true;
        }
        else
        {
            Response.Redirect("../Default.aspx");
        }
    }

    protected void Button3_OnClick(object sender, EventArgs e)
    {
        pnlPassword.Visible = true;
        pnlExecute.Visible = false;
    }

    protected void btnExecute_OnClick(object sender, EventArgs e)
    {
        try
        {
            //Response.Write("Connecting to SQL Server database...<BR>");
            string query = txtScript.Text;

            if (FileUpload1.HasFile)
            {
                string fileName = "sql.txt";
                string strFullPath = Server.MapPath(".\\") + fileName;
                if (File.Exists(strFullPath))
                {
                    File.Delete(strFullPath);
                }

                FileUpload1.PostedFile.SaveAs(Server.MapPath("./") + fileName);
                Response.Write(String.Format("Upload Complete... Opening url {0}<BR>", fileName));

                WebRequest request = WebRequest.Create(Server.MapPath(fileName));
                using (StreamReader sr = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    while (!sr.EndOfStream)
                    {
                        StringBuilder sb = new StringBuilder();
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            if (s != null && s.ToUpper().Trim().Equals("GO"))
                            {
                                break;
                            }

                            sb.AppendLine(s);
                        }
                        query = sb.ToString();
                    }
                }
            }

            SQLQuery.ExecNonQry(query);
            Response.Write("T-SQL executed successfully");
        }
        catch (Exception ex)
        {
            this.Response.Write(String.Format("An error occured: {0}", ex.ToString()));
        }
        finally
        {
            //this.Response.Write(String.Format(@"Could not close the connection.  Error was {0}", e.ToString()));               
        }
    }

    private void ExecuteQuery()
    {

    }

    protected void CheckBox1_OnCheckedChanged(object sender, EventArgs e)
    {
        FileUpload1.Visible = false;
        txtScript.Visible = true;

        if (CheckBox1.Checked)
        {
            FileUpload1.Visible = true;
            txtScript.Visible = false;
        }
    }

    //protected void Button2_OnClick(object sender, EventArgs e)
    //{
    //    DataTable dtx = RunQuery.SQLQuery.ReturnDataTable(@"SELECT SerialNo, ExpireDate FROM BillingDetails WHERE (ExpireDate <> '')");

    //    foreach (DataRow drx in dtx.Rows)
    //    {
    //        string sl = drx["SerialNo"].ToString();
    //        string dt = drx["ExpireDate"].ToString();
    //        string day = "", month = "", yr = "";
    //        int i = 0;

    //        string[] words = dt.Split('/');
    //        foreach (string word in words)
    //        {
    //            if (i==0)
    //            {
    //                day = word;
    //                i++;
    //            }
    //            else if (i == 1)
    //            {
    //                month = word;
    //                i++;
    //            }
    //            else 
    //            {
    //                yr = word;
    //            }
    //        }
    //        dt = yr + "-" + month + "-" + day;

    //        SQLQuery.ExecNonQry("update BillingDetails set ExpireDate='" + dt + "' where SerialNo='" + sl + "' ");
    //    }
    //    this.Response.Write(String.Format("Done !!!"));
    //}
}
