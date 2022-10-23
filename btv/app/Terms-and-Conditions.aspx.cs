using RunQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_Terms_and_Conditions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getContentSections();
            if (SQLQuery.AuthLevel(Page.User.Identity.Name) <= 2)//or 3
            {

            }
            else
            {
                btnSave.Visible = false;
                Notify("Warning: Only an Administrator can edit the plan!", "warn", lblMsg);
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        if (SQLQuery.OparatePermission(lName, "Update") == "1")
        {
            /*
            if (btnSave.Text == "Edit T&A")
            {
                getContentSections();
                //btnSave.Text = "Update T&A";
                pnlEdit.Visible = true;
                //lblContent.Visible = false;
            }
            else
            {*/
            SaveTextParts();
            getContentSections();
            btnSave.Text = "Edit T&A";
            //pnlEdit.Visible = false;
            //lblContent.Visible = true;
            //}
            Notify("T&A has been updated successfully!", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation", "warn", lblMsg);
        }

    }


    private void getContentSections()
    {
        string pgCnt = RunQuery.SQLQuery.ReturnString("SELECT SUBSTRING((SELECT left(('' + PageContent),3900) FROM BodyContent where ContentType='Business Plan' AND ProjectId='" + SQLQuery.ProjectID(User.Identity.Name) + "' ORDER BY ContentSerial FOR XML PATH('')),2,200000) AS CSV");
        PageContents.Text = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(pgCnt));
        PageContents.Text = HttpUtility.HtmlDecode(pgCnt);
    }

    private void SaveTextParts()
    {
        RunQuery.SQLQuery.ExecNonQry("Delete BodyContent where ContentType='Business Plan'");

        string pgCnt = HttpUtility.HtmlEncode(PageContents.Text);
        int countChar = pgCnt.Length; //CountChars(pgCnt);

        int startPt = 0;
        int endPt = 3900;

        if (countChar < endPt)
        {
            InsertContent(1, pgCnt);
        }
        else
        {
            //get the qty of parts
            decimal partQtyD = Convert.ToDecimal(countChar) / Convert.ToDecimal(endPt);
            int partQty = Convert.ToInt32(partQtyD) + 1;

            for (int i = 1; i <= partQty; i++)
            {
                string saveCnt = pgCnt.Substring(startPt, endPt);
                InsertContent(i, saveCnt);

                startPt = startPt + endPt;
                //endPt = endPt + 3900;

                if ((startPt + 3900) > countChar)
                {
                    endPt = countChar - startPt; //countChar-3800;
                }
            }

        }

    }

    private void InsertContent(int cntSerial, string content)
    {
        if (cntSerial == 1)
        {
            content = " " + content;
        }

        RunQuery.SQLQuery.ExecNonQry("INSERT INTO BodyContent (PageID, ContentSerial, PageContent, ContentType, EntryBy, ProjectId)" +
            " VALUES ('0', '" + cntSerial + "', N'" + content + "', 'Business Plan', '" + Page.User.Identity.Name.ToString() + "'," + SQLQuery.ProjectID(User.Identity.Name) + ")");

    }
}
