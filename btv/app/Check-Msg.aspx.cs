using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RunQuery;
public partial class app_Check_Msg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Get Branch/Centre Name
            string branch = Page.User.Identity.Name.ToString();
            string prjId = SQLQuery.ProjectID(branch);
            if (prjId == "1")
            {
                lblBranch.Text = "0";
            }
            else
            {
                lblBranch.Text = prjId;
            }
            ListView1.DataBind();

            string readId = Request.QueryString["read"];
            if (readId != null)
            {
                SQLQuery.ExecNonQry("update Messaging set IsRead=1 where MsgID='" + readId + "'");
                ListView1.DataBind();
            }

        }

    }
}
 
