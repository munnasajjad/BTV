using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Application_PageGenerator : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                string connectString = ConfigurationManager.ConnectionStrings["LocalSqlServer"].ToString();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);

                SQLQuery.PopulateDropDown("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_CATALOG = '" +
                        builder.InitialCatalog + "' ORDER BY TABLE_NAME", ddTableName, "TABLE_NAME", "TABLE_NAME");

                BindddMainMenu();
                BindGrid();
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error                     
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }


    private void BindGrid()
    {
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT c.*
                                                    FROM information_schema.columns c
                                                    WHERE c.table_schema = 'dbo'    --or whatever
                                                    AND c.table_name = '" + ddTableName.SelectedValue + @"'
                                                    ORDER BY c.ORDINAL_POSITION");
        GridView1.DataSource = dt;
        GridView1.DataBind();

        DataTable dt1 = SQLQuery.ReturnDataTable(@"SELECT Referenced_Table_Name, Referenced_Column_As_FK, Referencing_Table_Name, Referencing_Column_Name, Constraint_name
                                        FROM vwTableRelations WHERE Referencing_Table_Name='" + ddTableName.SelectedValue + "' ");
        GridView2.DataSource = dt1;
        GridView2.DataBind();

        foreach (GridViewRow gvr in GridView2.Rows)
        {
            Label lblPTable = gvr.FindControl("lblPTable") as Label;
            DropDownList ddColumns = gvr.FindControl("ddColumns") as DropDownList;
            SQLQuery.PopulateDropDown(@"SELECT c.*
                                                    FROM information_schema.columns c
                                                    WHERE c.table_schema = 'dbo'    --or whatever
                                                    AND c.table_name = '" + lblPTable.Text + @"'
                                                    ORDER BY c.ORDINAL_POSITION", ddColumns, "COLUMN_NAME", "COLUMN_NAME");
        }

        txtName.Text = XEngine.NormalizeText(ddTableName.SelectedValue);
    }
    private void BindddMainMenu()
    {
        SQLQuery.PopulateDropDown(@"SELECT Sl, GroupName, DesplaySerial, Show FROM MenuGroup where GroupName<>'Dashboard' ORder by DesplaySerial ", ddMainMenu, "sl", "GroupName");
    }

    protected void ddTableName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string pageLoad = "", pkColumn = "", fkColumn = "", pkTable = "", ddtextField = "", leftContent = "", insertQryRight = "", insertQryLeft = "", SelectQuery = "", UpdateQuery = "", DeleteQuery = "", idColumnName = "", ddListEvents = "", editingControls = "", clearControls = "", gridColumns = "", gridSQL = "";
            string maxID = SQLQuery.ReturnString("SELECT ISNULL(MAX(Sl),0)+1 FROM MenuStructure");
            string formName = ddTableName.SelectedValue;
            string pageName = txtName.Text;//.Replace(" ", "");

            string isExist = SQLQuery.ReturnString("Select PageName FROM MenuStructure  Where PageName='" + formName + "' ");

            if (isExist != "")
            {
                formName = ddTableName.SelectedValue + maxID;
            }
            //else { 
            SQLQuery.ExecNonQry(@"INSERT INTO  MenuStructure  (TableName, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority,  EntryBy)
                                VALUES ('" + ddTableName.SelectedValue + "', '" + ddMainMenu.SelectedItem.Text + "', '" + ddMainMenu.SelectedValue + "', '" + txtName.Text + "', '" + txtName.Text.Replace(" ", "") + "', '" + ddTableName.SelectedValue + "', '" + maxID + "', '" + User.Identity.Name + "' )");


            //DataTable dt1 = SQLQuery.ReturnDataTable(@"SELECT Referenced_Table_Name, Referenced_Column_As_FK, Referencing_Table_Name, Referencing_Column_Name, Constraint_name
            //                            FROM vwTableRelations WHERE Referencing_Table_Name='" + ddTableName.SelectedValue + "' ");

            //SQLQuery.ExecNonQry(@"INSERT into MenuStructure (TableName,MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority, EntryBy)VALUES
            //('" + ddTableName.Text + "','" + ddMainMenu.SelectedValue + "','" + ddMainMenu.SelectedItem.Text + "','" + txtName.Text + "','" + formName + "','" + formName + maxID + "','" + maxID + "','" + User.Identity.Name + "')");

            DataTable dt = SQLQuery.ReturnDataTable(@"SELECT c.*
                                                    FROM information_schema.columns c
                                                    WHERE c.table_schema = 'dbo'    --or whatever
                                                    AND c.table_name = '" + ddTableName.SelectedValue + @"'
                                                    ORDER BY c.ORDINAL_POSITION");
            int count = 0;
            foreach (DataRow drx in dt.Rows)
            {
                bool isDropdown = false, isDateField = false, isNumberField = false;

                string sl = drx["ORDINAL_POSITION"].ToString();
                string colName = drx["COLUMN_NAME"].ToString();
                string dataType = drx["DATA_TYPE"].ToString();
                string isNullable = drx["IS_NULLABLE"].ToString();
                string maxl = drx["CHARACTER_MAXIMUM_LENGTH"].ToString();
                string dflt = drx["COLUMN_DEFAULT"].ToString();

                //***************************************** LEFT SIDE ******************************//
                /// create the fields
                if (count == 0)//sl is the Identity column
                {
                    idColumnName = colName;
                    SelectQuery += colName + ", ";
                    //UpdateQuery +=" WHERE "+ colName + "=lblId.Text";
                    //DeleteQuery += " WHERE " + colName + "=lblId.Text";
                    //insertQuery += colName + ", ";
                    count++;
                }
                else
                {
                    leftContent += "";
                    //Looping the relations
                    foreach (GridViewRow gvr in GridView2.Rows)
                    {
                        Label lblPTable = gvr.FindControl("lblPTable") as Label;
                        Label lblPColumn = gvr.FindControl("lblPColumn") as Label;
                        DropDownList ddColumns = gvr.FindControl("ddColumns") as DropDownList;
                        Label lblRefColumn = gvr.FindControl("lblRefColumn") as Label;

                        pkTable = lblPTable.Text;
                        pkColumn = lblPColumn.Text;
                        ddtextField = ddColumns.SelectedValue;
                        fkColumn = lblRefColumn.Text;

                        if (colName == fkColumn)
                        {
                            break;
                        }
                    }
                    if (colName == fkColumn)//It is a dropdownlist
                    {
                        pageLoad += "bindDD" + colName + "();" + Environment.NewLine;
                        ddListEvents += XEngine.BindDDList(colName, ddtextField, pkColumn, pkTable);
                        leftContent += XEngine.GenerateDropDown(colName);
                        insertQryLeft += colName + ", ";
                        insertQryRight += " '\"+dd" + colName + ".SelectedValue+\"',";
                        SelectQuery += colName + ",";
                        UpdateQuery += "  " + colName + "= '\"+dd" + colName + ".SelectedValue+\"',";
                        editingControls += "dd" + colName + ".SelectedValue=dtx[\"" + colName + "\"].ToString();" + Environment.NewLine;
                    }
                    else if (dataType == "bit")
                    {
                        leftContent += XEngine.GenerateCheckBox(colName);
                        insertQryLeft += colName + ", ";
                        insertQryRight += " '\"+dd" + colName + ".SelectedValue+\"',";
                        SelectQuery += colName + ",";
                        UpdateQuery += "  " + colName + "= '\"+dd" + colName + ".SelectedValue+\"',";

                    }
                    else //Textbox
                    {
                        if (colName.ToLower() == "entryby")
                        {
                            insertQryLeft += colName + ", ";
                            insertQryRight += " '\"+User.Identity.Name+\"',";
                        }
                        else if (colName.ToLower() == "entrydate")
                        {

                        }
                        else
                        {
                            string include2PageLoad = "";
                            leftContent += XEngine.GenerateTextBox(colName, isNullable, dataType, out include2PageLoad);
                            pageLoad += include2PageLoad;

                            insertQryLeft += colName + ", ";
                            insertQryRight += " '\"+txt" + colName + ".Text.Replace(\"'\",\"''\")+\"',";
                            SelectQuery += colName + ",";
                            UpdateQuery += "  " + colName + "= '\"+txt" + colName + ".Text.Replace(\"'\",\"''\")+\"',";
                            editingControls += "txt" + colName + ".Text=dtx[\"" + colName + "\"].ToString();" + Environment.NewLine;
                            clearControls += "txt" + colName + ".Text=\"\"; " + Environment.NewLine;
                        }
                    }
                    gridSQL += " " + colName + ",";
                    gridColumns += "<asp:BoundField DataField=\"" + colName + "\" HeaderText=\"" + XEngine.NormalizeText(colName) + "\" SortExpression=\"" + colName + "\" />";
                    count++;
                }
            }
            pageLoad += "BindGrid();";
            string where = " WHERE " + idColumnName + "='\"+lblId.Text+\"'";
            string insertQuery = " INSERT INTO " + ddTableName.SelectedValue + " (" + insertQryLeft.Trim().TrimEnd(',') + ") VALUES (" + insertQryRight.Trim().TrimEnd(',') + ")   ";
            SelectQuery = SelectQuery.Trim().TrimEnd(',');
            SelectQuery = " Select " + SelectQuery + " FROM " + ddTableName.SelectedValue + where;
            UpdateQuery = " Update  " + ddTableName.SelectedValue + " SET " + UpdateQuery.Trim().TrimEnd(',') + where;
            DeleteQuery = " Delete " + ddTableName.SelectedValue + where;
            gridSQL = " SELECT * FROM " + ddTableName.SelectedValue;

            XEngine.SaveASPXFile(pageName, formName, Server.MapPath("./"), leftContent, gridColumns, idColumnName, count);
            XEngine.SaveCSharpFile(formName, Server.MapPath("./"), pageLoad, insertQuery, UpdateQuery, SelectQuery, DeleteQuery, editingControls, clearControls, gridSQL, ddListEvents);
            Notify("The page <a href='./" + formName + "' target='_blank'>" + formName + "</a> generated successfully...", "success", lblMsg);
            //}
            //}
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }
}










