using RunQuery;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class app_LVApproved : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            LoadLVInovice();
            //BindStation();
            //BindCenter();
            //BindStore();
            //BindSaveToStore();
            BindEmployee();
            //BindVerifier();
            //BindRequsitionBy();
            //BindDeliveredBy();
            //bindDDCategoryID();
            //BindddProductSubCategory();
            //BindDdProductId();

            //txtDateofLv.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtDeliveredDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtEsclationStartTime.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtEsclationEndTime.Text = DateTime.Now.ToString("dd/MM/yyyy");

            BindWorkFlowUserGridView();
            BindAddItemsGridView();
            BindSaveItemsGridView();
            //txtLoanVoucharNo.Text = GenerateLvNo();
        }
    }
    private void LoadLVInovice()
    {
        SQLQuery.PopulateDropDown("SELECT IDLvNo, LvInvoiceNo FROM  LoanVouchar Where RequestStatus='1' AND ApprovedStatus='0'", ddlLvVoucher, "IDLvNo", "LvInvoiceNo");
    }
    //private void BindStation()
    //{
    //    SQLQuery.PopulateDropDown("Select Name,LocationID from Location ", ddlStation, "LocationID", "Name");
    //    BindCenter();
    //    BindStore();
    //}

    //private void BindCenter()
    //{
    //    SQLQuery.PopulateDropDown("Select Name,CenterID from Center Where LocationID='" + ddlStation.SelectedValue + "'", ddLoanFromCenter, "CenterID", "Name");
    //    BindStore();
    //}
    //private void BindStore()
    //{
    //    SQLQuery.PopulateDropDown("Select StoreID, Name from Store WHERE CenterID = '" + ddLoanFromCenter.SelectedValue + "' AND LocationID='" + ddlStation.SelectedValue + "'", ddLoanFromStore, "StoreID", "Name");
    //}
    //private void BindSaveToStore()
    //{
    //    SQLQuery.PopulateDropDown("Select StoreID, Name from Store", ddSaveToStore, "StoreID", "Name");
    //}
    //private void BindEmployee()
    //{
    //    SQLQuery.PopulateDropDown("Select EmployeeID, Name from Employee", ddEmployee, "EmployeeID", "Name");
    //}
    private void BindEmployee()
    {
        SQLQuery.PopulateDropDown("Select EmployeeID, Name from Employee WHERE EmployeeID NOT IN( SELECT EmployeeInfoID FROM Logins WHERE  (LoginUserName = '" + User.Identity.Name + "')) AND DepartmentSectionID='" + SQLQuery.GetDepartmentSectionId(User.Identity.Name) + "'", ddEmployee, "EmployeeID", "Name");
    }
    //private void BindVerifier()
    //{
    //    SQLQuery.PopulateDropDown("Select EmployeeID, Name from Employee", ddVerifier, "EmployeeID", "Name");
    //}
    //private void BindRequsitionBy()
    //{
    //    SQLQuery.PopulateDropDown("Select EmployeeID, Name from Employee", ddRequisitionBy, "EmployeeID", "Name");
    //}
    //private void BindDeliveredBy()
    //{
    //    SQLQuery.PopulateDropDown("Select EmployeeID, Name from Employee", ddDeliveredBy, "EmployeeID", "Name");
    //}

    private void Notify(string msg, string type, Label lblNotify)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }


    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (btnSave.Text == "Save")
            {
                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                {
                    //string isExist = "";// SQLQuery.ReturnString("SELECT LvInvoiceNo FROM LoanVouchar WHERE LvInvoiceNo='" + txtLoanVoucharNo.Text.Trim() + "'");
                    //if (isExist == "")
                    //{
                    //RunQuery.SQLQuery.ExecNonQry(" INSERT INTO LoanVouchar (LvInvoiceNo, DateofLv, ResponsiblePerson, CauseOfLoan, Station,LoanFromCenter, LoanFromStore, SaveToStore, Remarks, Verifier, RequisitionBy, DeliveredBy, EntryBy, EntryDate) " +
                    //                         "VALUES ('" + txtLoanVoucharNo.Text + "','" + Convert.ToDateTime(txtDateofLv.Text).ToString("yyyy-MM-dd") + "', '" + txtResponsiblePerson.Text + "', '" + txtCauseOfLoan.Text + "', '" + ddlStation.SelectedValue + "','" + ddLoanFromCenter.SelectedValue + "','" + ddLoanFromStore.SelectedValue + "','" + ddSaveToStore.SelectedValue + "','" + txtRemarks.Text + "', '" + ddVerifier.SelectedValue + "', '" + ddRequisitionBy.SelectedValue + "', '" + ddDeliveredBy.SelectedValue + "', '" + lName + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "')    ");
                     SQLQuery.ExecNonQry("UPDATE LoanVouchar SET ApprovedStatus='1',ApprovedDate='"+DateTime.Now.ToString("yyyy-MM-dd")+"' WHERE IDLvNo='" + ddlLvVoucher.SelectedValue+"'");
                        ClearControls();
                        Notify("Successfully Saved...", "success", lblMsg);
                       // string lvId = SQLQuery.ReturnString("SELECT IDLvNo lvId FROM LoanVouchar WHERE LvInvoiceNo='"++"'");
                        //SQLQuery.ExecNonQry("UPDATE LVProduct SET IDLVNo='" + lvId + "'  WHERE IDLVNo = '" + LvIdHiddenField.Value + "' AND EntryBy='" + lName + "' ");
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + ddlLvVoucher.SelectedValue + "'  WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND WorkFlowType='LV-A' AND EntryBy='" + lName + "' ");

                        string sqlQuery = @"SELECT TOP (1) WFU.WorkFlowTypeID,WFU.WorkFlowUserID, LV.LvInvoiceNo, WFU.Priority, WFU.EsclationStartTime, WFU.EsclationEndTime, WFU.TaskStatus, WFU.IsActive, Employee_1.Name, Employee_1.Email
                        FROM  WorkFlowUser AS WFU INNER JOIN
                        LoanVouchar AS LV ON WFU.WorkFlowTypeID = LV.IDLvNo INNER JOIN
                        Employee AS Employee_1 ON WFU.EmployeeID = Employee_1.EmployeeID
                        WHERE WFU.WorkFlowTypeID='" + ddlLvVoucher.SelectedValue + "'AND WFU.WorkFlowType='LV-A' AND (WFU.TaskStatus = '1') AND (WFU.IsActive = '0') ORDER BY WFU.Priority DESC";
                        DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);

                        foreach (DataRow item in dt.Rows)
                        {
                            string name = item["Name"].ToString();
                            string email = item["Email"].ToString();
                            string LvInvoiceNo = item["LvInvoiceNo"].ToString();
                            string wfuId = item["WorkFlowUserID"].ToString();
                            string emailBody = "Dear " + name +
                                          ", <br><br>Approve workflow, check your notification .<br><br>";

                            emailBody += " <br><br>Regards, <br><br>Development Team.";

                            SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + LvInvoiceNo, emailBody);
                            SQLQuery.ExecNonQry("Update WorkFlowUser SET IsActive='1' Where WorkFlowUserID='" + wfuId + "'");
                        }
                    //}
                    //else
                    //{
                    //    Notify("Loan voucher number already exist.", "error", lblMsg);
                    //}
                }
                else
                {
                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                }
            }
            //else
            //{
            //    if (SQLQuery.OparatePermission(lName, "Update") == "1")
            //    {
            //       // RunQuery.SQLQuery.ExecNonQry(" Update  LoanVouchar SET DateofLv= '" + Convert.ToDateTime(txtDateofLv.Text).ToString("yyyy-MM-dd") + "', LvInvoiceNo= '" + txtLoanVoucharNo.Text + "',  ResponsiblePerson= '" + txtResponsiblePerson.Text + "',  CauseOfLoan= '" + txtCauseOfLoan.Text + "',  Station='" + ddlStation.SelectedValue + "', LoanFromCenter= '" + ddLoanFromCenter.SelectedValue + "', LoanFromStore= '" + ddLoanFromStore.SelectedValue + "', SaveToStore= '" + ddSaveToStore.SelectedValue + "',   Remarks= '" + txtRemarks.Text + "', Verifier= '" + ddVerifier.SelectedValue + "',  RequisitionBy= '" + ddRequisitionBy.SelectedValue + "',    DeliveredBy= '" + ddDeliveredBy.SelectedValue + "',  EntryBy= '" + lName + "',  EntryDate= '" + DateTime.Now.ToString("yyyy-MM-dd") + "' WHERE IDLvNo='" + LvIdHiddenField.Value + "' ");
            //        ClearControls();
            //        btnSave.Text = "Save";
            //        Notify("Successfully Updated...", "success", lblMsg);
            //    }
            //    else
            //    {
            //        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            //    }
            //}
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            LvIdHiddenField.Value = "";
            LoadLVInovice();
            BindSaveItemsGridView();
            BindAddItemsGridView();
            BindWorkFlowUserGridView();
            //txtLoanVoucharNo.Text = GenerateLvNo();
        }
    }

    protected void SaveItemsGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                int index = Convert.ToInt32(SaveItemsGridView.SelectedIndex);
                Label lblEditId = SaveItemsGridView.Rows[index].FindControl("lblIDLvNo") as Label;
                LvIdHiddenField.Value = lblEditId.Text;
                DataTable dt = SQLQuery.ReturnDataTable("SELECT IDLvNo, LvInvoiceNo,Station, DateofLv, ResponsiblePerson, CauseOfLoan, LoanFromCenter, LoanFromStore, SaveToStore, Remarks, Verifier, RequisitionBy, DeliveredBy, EntryBy, EntryDate FROM LoanVouchar WHERE IDLvNo='" + LvIdHiddenField.Value + "'");
                foreach (DataRow dtx in dt.Rows)
                {
                    //txtLoanVoucharNo.Text = dtx["LvInvoiceNo"].ToString();
                    txtDateofLv.Text = Convert.ToDateTime(dtx["DateofLv"]).ToString("dd/MM/yyyy");
                    txtResponsiblePerson.Text = dtx["ResponsiblePerson"].ToString();
                    txtCauseOfLoan.Text = dtx["CauseOfLoan"].ToString();
                   // BindStation();
                   // ddlStation.SelectedValue = dtx["Station"].ToString();
                    //BindCenter();
                    //ddLoanFromCenter.SelectedValue = dtx["LoanFromCenter"].ToString();
                    //BindStore();
                    //ddLoanFromStore.SelectedValue = dtx["LoanFromStore"].ToString();
                    //ddSaveToStore.SelectedValue = dtx["SaveToStore"].ToString();
                    //txtRemarks.Text = dtx["Remarks"].ToString();
                    //ddVerifier.SelectedValue = dtx["Verifier"].ToString();
                    //ddRequisitionBy.SelectedValue = dtx["RequisitionBy"].ToString();
                    //ddDeliveredBy.SelectedValue = dtx["DeliveredBy"].ToString();

                }
                btnSave.Text = "Update";
                Notify("Edit mode activated ...", "info", lblMsg);
                BindAddItemsGridView();
                BindWorkFlowUserGridView();
            }
            else
            {
                Notify("You are not eligible to attempt this operation", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
    }

    protected void SaveItemsGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Delete") == "1")
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblId = SaveItemsGridView.Rows[index].FindControl("lblIDLvNo") as Label;
                RunQuery.SQLQuery.ExecNonQry(" Delete LoanVouchar WHERE IDLvNo='" + lblId.Text + "' ");
                SQLQuery.ExecNonQry("Delete LVProduct WHERE IDLVNo='" + lblId.Text + "' ");
                SQLQuery.ExecNonQry("Delete WorkFlowUser WHERE WorkFlowTypeID='" + lblId.Text + "' AND WorkFlowType='LV'");
                BindSaveItemsGridView();
                Notify("Successfully Deleted...", "success", lblMsg);
            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
            Notify("ERROR:" + ex, "error", lblMsg);

        }

    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }

    private void BindSaveItemsGridView()
    {
        DataTable dt = SQLQuery.ReturnDataTable(" SELECT DISTINCT LoanVouchar.IDLvNo, LoanVouchar.LvInvoiceNo,convert(varchar, LoanVouchar.DateofLv, 103) AS DateofLv,  Center.Name AS CenterName, Store.Name AS StoreName FROM LoanVouchar INNER JOIN Center ON LoanVouchar.LoanFromCenter = Center.CenterID INNER JOIN Store ON LoanVouchar.LoanFromStore = Store.StoreID WHERE LoanVouchar.ApprovedStatus='1' Order by DateofLv Desc");
        SaveItemsGridView.DataSource = dt;
        SaveItemsGridView.DataBind();
    }


    //private void BindDdProductId()
    //{
    //    SQLQuery.PopulateDropDown("Select Name,ProductID from Product Where ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "'", ddProductID, "ProductID", "Name");
    //}


    //protected void ddProductID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    SaveItemsGridView.DataBind();
    //}



    private void ClearControls()
    {
        txtDateofLv.Text = "";
        txtResponsiblePerson.Text = "";
        txtCauseOfLoan.Text = "";
        txtStation.Text = "";
        txtCenter.Text = "";
        txtStore.Text = "";
        //txtDivision.Text=""; 
        //txtProductDescription.Text = "";
        //txtQtyNeed.Text = "";
        //txtQtyDelivered.Text = "";
        //txtDeliveredDate.Text = "";
        //txtQtyReturn.Text = "";
        //txtProductCondition.Text = "";
        //txtVerifier.Text = "";
        txtRemarks.Text = "";
        //txtRequisitionBy.Text = "";
        //txtPreparedBy.Text = "";
        //txtDeliveredBy.Text = "";
        //txtReceivedBy.Text = "";
        //txtStoreHouseReturnReceivedBy.Text = "";

    }


    //protected void btnAdd_OnClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string lName = Page.User.Identity.Name.ToString();
    //        string isProductExists = SQLQuery.ReturnString("SELECT ProductID FROM LVProduct WHERE ProductID = '" + ddProductID.SelectedValue + "' AND IDLVNo = '" + LvIdHiddenField.Value + "' AND EntryBy = '" + lName + "'");
    //        SQLQuery.Empty2Zero(txtQtyDelivered);
    //        if (btnAdd.Text == "ADD")
    //        {
    //            if (isProductExists != ddProductID.SelectedValue)
    //            {
    //                if (SQLQuery.OparatePermission(lName, "Insert") == "1")
    //                {
    //                    InsertToLvProduct();
    //                    Notify("Insert Successful", "info", lblMsg);
    //                    BindAddItemsGridView();

    //                }
    //                else
    //                {
    //                    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //                }
    //            }
    //            else
    //            {
    //                Notify("This Product is already added!", "warn", lblMsg);
    //            }

    //        }
    //        else
    //        {
    //            if (SQLQuery.OparatePermission(lName, "Update") == "1")
    //            {
    //                UpdateLvProduct();
    //                BindAddItemsGridView();
    //                Notify("Update Successful", "info", lblMsg);
    //            }
    //            else
    //            {
    //                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
    //            }


    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Notify("ERROR: " + ex, "error", lblMsg);
    //    }
    //    finally
    //    {
    //        //ddProductID.SelectedValue = "0";
    //        //ddCategoryID.SelectedValue = "0";
    //        //ddProductSubCategory.SelectedValue = "0";
    //        //txtQtyNeed.Text = "";

    //    }


    //}

    //private void InsertToLvProduct()
    //{
    //    string lName = Page.User.Identity.Name.ToString();
    //    SqlCommand command;
    //    int lvNo;
    //    if (LvIdHiddenField.Value == "")
    //    {
    //        lvNo = 0;
    //    }
    //    else
    //    {
    //        lvNo = Convert.ToInt32(LvIdHiddenField.Value);
    //    }


        //command = new SqlCommand(@"INSERT INTO LVProduct ( ProductID,CategoryID,SubCategoryID,IDLVNo, QTYNeed, QTYDelivered,  EntryBy, EntryDate) 
        //                               VALUES (@ProductID,@CategoryID,@SubCategoryID,@IDLVNo,@QTYNeed,@QTYDelivered,@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //command.Parameters.AddWithValue("@IDLVNo", lvNo);
        //command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        //command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        //command.Parameters.AddWithValue("@ProductID", ddProductID.SelectedValue);
        //command.Parameters.AddWithValue("@QTYNeed", Convert.ToInt32(txtQtyNeed.Text));
        //command.Parameters.AddWithValue("@QTYDelivered", Convert.ToInt32(txtQtyDelivered.Text));
        ////command.Parameters.AddWithValue("@DeliveredDate", Convert.ToDateTime(txtDeliveredDate.Text).ToString("yyyy-MM-dd"));
        ////command.Parameters.AddWithValue("@ReturnQTY", Convert.ToInt32(txtQtyReturn.Text));
        ////command.Parameters.AddWithValue("@ProductCondition",txtProductCondition.Text);
        //command.Parameters.AddWithValue("@EntryBy", lName);
        //command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        //command.Connection.Open();
        //command.ExecuteNonQuery();
        //command.Connection.Close();
    //}
    //private void UpdateLvProduct()
    //{
        //string lName = Page.User.Identity.Name.ToString();

        //string query = @"UPDATE LVProduct SET  QTYNeed=@QTYNeed, CategoryID=@CategoryID,SubCategoryID=@SubCategoryID,QTYDelivered=@QTYDelivered,  EntryBy=@EntryBy, EntryDate=@EntryDate WHERE LVProductID = '" + idHiddenField.Value + "'";
        //SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        //command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        //command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        //command.Parameters.AddWithValue("@QTYNeed", txtQtyNeed.Text);
        //command.Parameters.AddWithValue("@QTYDelivered", txtQtyDelivered.Text);
        ////command.Parameters.AddWithValue("@DeliveredDate", Convert.ToDateTime(txtDeliveredDate.Text));
        //command.Parameters.AddWithValue("@EntryBy", lName);
        //command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));

        //command.Connection.Open();
        //command.ExecuteNonQuery();
        //command.Connection.Close();
        //command.Connection.Dispose();
    //}
    private void BindAddItemsGridView()
    {
       
        if (ddlLvVoucher.SelectedValue!="0")
        {
            string lName = Page.User.Identity.Name.ToString();

            string query = @"SELECT LVProductID, Product.Name AS ProductName, LVProduct.QTYNeed, LVProduct.QTYDelivered, convert(varchar, LVProduct.DeliveredDate, 103) AS DeliveredDate FROM LVProduct INNER JOIN Product ON LVProduct.ProductID = Product.ProductID WHERE IDLVNo = '" + ddlLvVoucher.SelectedValue + "'";

            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

            command.Connection.Open();
            
            AddItemsGridView.EmptyDataText = "No data added ...";
            AddItemsGridView.DataSource = command.ExecuteReader();
            AddItemsGridView.DataBind();
            command.Connection.Close();
            command.Connection.Dispose();
        }
        else
        {
            AddItemsGridView.EmptyDataText = "No data added ...";
            AddItemsGridView.DataSource = null;
            AddItemsGridView.DataBind();
        }
       
    }

    protected void AddItemsGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        //    int index = Convert.ToInt32(AddItemsGridView.SelectedIndex);
        //    Label label = AddItemsGridView.Rows[index].FindControl("lblLVProductID") as Label;
        //    idHiddenField.Value = label.Text;
        //    string query = @"SELECT LVProductID, ProductID,CategoryID,SubCategoryID, QTYNeed, QTYDelivered, DeliveredDate FROM LVProduct WHERE LVProductID = '" + idHiddenField.Value + "'";
        //    SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        //    command.Connection.Open();
        //    SqlDataReader dataReader = command.ExecuteReader();
        //    if (dataReader.Read())
        //    {
        //        btnAdd.Text = "Update";
        //        bindDDCategoryID();
        //        ddCategoryID.SelectedValue = dataReader["CategoryID"].ToString();
        //        BindddProductSubCategory();
        //        ddProductSubCategory.SelectedValue = dataReader["SubCategoryID"].ToString();
        //        BindDdProductId();
        //        ddProductID.SelectedValue = dataReader["ProductID"].ToString();
        //        txtQtyNeed.Text = dataReader["QTYNeed"].ToString();
        //        txtQtyDelivered.Text = dataReader["QTYDelivered"].ToString();
        //        txtDeliveredDate.Text = dataReader["DeliveredDate"].ToString();
        //    }
        //    Notify("Edit mode activated ...", "info", lblMsg);
        //    //ddProductID.Enabled = false;
        //    dataReader.Close();
        //    command.Connection.Close();
        //}
        //catch (Exception ex)
        //{

        //    Notify("ERROR: " + ex, "error", lblMsg);
        //}

    }

    protected void AddItemsGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = AddItemsGridView.Rows[index].FindControl("lblLVProductID") as Label;
            SQLQuery.ExecNonQry(" Delete LVProduct FROM LVProduct WHERE LVProductID='" + lblId.Text + "' ");
            BindAddItemsGridView();
            Notify("Successfully Deleted...", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }

    //protected void ddLoanFromCenter_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //BindStore();
    //}


    protected void btnWorkFlowSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;

            string initialValue = SQLQuery.ReturnString("SELECT WorkFlowUserID FROM WorkFlowUser WHERE WorkFlowTypeID='" + LvIdHiddenField.Value + "' AND EntryBy = '" + lName + "'");
            if (initialValue == "" && btnWorkFlowSave.Text == "ADD USER")
            {
                InsertToWorkFlowUser();
                BindWorkFlowUserGridView();
            }
            else
            {
                DateTime startdate = DateTime.Parse(txtEsclationStartTime.Text);
                string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + LvIdHiddenField.Value + "' AND WorkFlowType='LV-A' AND EntryBy = '" + lName + "'");
                string endDate = SQLQuery.ReturnString("SELECT MAX(EsclationEndTime) AS EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID ='" + LvIdHiddenField.Value + "' AND EntryBy = '" + lName + "' ");
                DateTime lastDate = DateTime.Parse(endDate);
                if (btnWorkFlowSave.Text == "ADD USER")
                {
                    if (isUserExists != ddEmployee.SelectedValue)
                    {
                        if (startdate >= lastDate)
                        {
                            if (PriorityCheck())
                            {
                                InsertToWorkFlowUser();
                                BindWorkFlowUserGridView();
                                Notify("Insert Successful", "info", lblMsg);
                            }
                            else
                            {
                                Notify("Already you have assigned this priority or date!", "warn", lblMsg);
                            }
                        }
                        else
                        {
                            Notify("Already you have assigned this Date!", "warn", lblMsg);
                        }


                    }
                    else
                    {
                        Notify("This employee is already added!", "warn", lblMsg);
                    }
                }
                else
                {
                    if (SQLQuery.OparatePermission(lName, "Update") == "1")
                    {
                        if (startdate >= lastDate)
                        {
                            if (PriorityCheckForUpdate())
                            {
                                UpdateWorkFlowUser();
                                BindWorkFlowUserGridView();
                                Notify("Update Successful", "info", lblMsg);
                            }
                            else
                            {
                                Notify("Already you have assigned this priority or date!", "warn", lblMsg);
                            }

                        }
                        else
                        {
                            Notify("Already you have assigned this Date!", "warn", lblMsg);
                        }

                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }

                }

            }
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "error", lblMsg);
        }
        finally
        {
            txtPriority.Text = "";
            txtWorkflowRemarks.Text = "";
        }


    }

    private void InsertToWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();


        SqlCommand command;
        int typeId;
        if (LvIdHiddenField.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(LvIdHiddenField.Value);
        }

        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,WorkFlowType, EmployeeID, Priority, EsclationStartTime, EsclationEndTime, Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,'LV-A',@EmployeeID,@Priority,@EsclationStartTime,@EsclationEndTime,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        command.Parameters.AddWithValue("@WorkFlowTypeID", typeId);
        command.Parameters.AddWithValue("@EmployeeID", ddEmployee.SelectedValue);
        command.Parameters.AddWithValue("@Priority", Convert.ToInt32(txtPriority.Text));
        command.Parameters.AddWithValue("@EsclationStartTime", Convert.ToDateTime(txtEsclationStartTime.Text).ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@EsclationEndTime", Convert.ToDateTime(txtEsclationEndTime.Text).ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("@Remark", txtWorkflowRemarks.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();


    }
    private void UpdateWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();
        string query = @"UPDATE WorkFlowUser SET EsclationStartTime=@EsclationStartTime, EsclationEndTime=@EsclationEndTime, Priority=@Priority, Remark=@Remark, EntryBy=@EntryBy, EntryDate=@EntryDate WHERE WorkFlowUserID = '" + WorkFlowIdHiddenField.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@EsclationStartTime", Convert.ToDateTime(txtEsclationStartTime.Text));
        command.Parameters.AddWithValue("@EsclationEndTime", Convert.ToDateTime(txtEsclationEndTime.Text));
        command.Parameters.AddWithValue("@Priority", txtPriority.Text);
        command.Parameters.AddWithValue("@Remark", txtWorkflowRemarks.Text);

        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    private void BindWorkFlowUserGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string query;
        //if (btnWorkFlowSave.Text == "Update")
        //{
        //    query = @"SELECT WorkFlowUser.WorkFlowUserID, Employee.Name AS EmployeeName , convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + LvIdHiddenField.Value + "' AND EntryBy = '" + lName + "'";
        //}
        //else
        //{
        query = @"SELECT WorkFlowUser.WorkFlowUserID,WorkFlowUser.Priority, Employee.Name AS EmployeeName , convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN Employee ON WorkFlowUser.EmployeeID = Employee.EmployeeID WHERE WorkFlowTypeID='" + LvIdHiddenField.Value + "' AND WorkFlowType='LV-A' AND EntryBy = '" + lName + "' Order By Priority DESC";
        //} 

        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    //private bool PriorityCheck()
    //{
    //    string lName = Page.User.Identity.Name.ToString();
    //    bool priorityStatus = true;
    //    DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND EntryBy='" + lName + "'");
    //    foreach (DataRow priorityDataRow in priorityDataTable.Rows)
    //    {
    //        if (priorityDataRow["Priority"].ToString() == txtPriority.Text.Trim())
    //        {
    //            priorityStatus = false;
    //        }
    //    }
    //    return priorityStatus;
    //}
    private bool PriorityCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND WorkFlowType='LV-A' AND EntryBy='" + lName + "'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND WorkFlowType='LV-A' AND (EsclationEndTime>='" + Convert.ToDateTime(txtEsclationEndTime.Text).ToString("yyyy-MM-dd") + "' AND EsclationEndTime<='" + Convert.ToDateTime(txtEsclationEndTime.Text).ToString("yyyy-MM-dd") + "')");
            if (priorityDataRow["Priority"].ToString() == txtPriority.Text.Trim())
            {
                priorityStatus = false;
            }
            else if (int.Parse(escDate) > 0)
            {
                priorityStatus = false;
            }
        }
        return priorityStatus;
    }
    private bool PriorityCheckForUpdate()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND WorkFlowType='LV-A'");
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {

            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND Priority='" + txtPriority.Text + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "'");
                string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + LvIdHiddenField.Value + "' AND (EsclationEndTime>='" + Convert.ToDateTime(txtEsclationEndTime.Text).ToString("yyyy-MM-dd") + "' AND EsclationEndTime<='" + Convert.ToDateTime(txtEsclationEndTime.Text).ToString("yyyy-MM-dd") + "') AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "'");
                if (int.Parse(priority) > 0)
                {
                    priorityStatus = false;
                }
                else if (int.Parse(escDate) > 0)
                {
                    priorityStatus = false;
                }
                else
                {
                    priorityStatus = true;
                }

            }



        }
        return priorityStatus;
    }
    protected void WorkFlowUserGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        {
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = WorkFlowUserGridView.Rows[index].FindControl("lblWorkFlowUserID") as Label;
            SQLQuery.ExecNonQry(" Delete WorkFlowUser FROM WorkFlowUser WHERE WorkFlowUserID='" + lblId.Text + "' ");
            BindWorkFlowUserGridView();
            Notify("Successfully Deleted...", "success", lblMsg);
        }
        else
        {
            Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        }
    }

    protected void WorkFlowUserGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(WorkFlowUserGridView.SelectedIndex);
            Label label = WorkFlowUserGridView.Rows[index].FindControl("lblWorkFlowUserID") as Label;
            WorkFlowIdHiddenField.Value = label.Text;
            string query = @"SELECT WorkFlowUserID,EmployeeID, Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, Remark FROM WorkFlowUser WHERE WorkFlowUserID = '" + WorkFlowIdHiddenField.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                btnWorkFlowSave.Text = "Update";
                ddEmployee.SelectedValue = dataReader["EmployeeID"].ToString();
                txtPriority.Text = dataReader["Priority"].ToString();
                txtEsclationStartTime.Text = dataReader["EsclationStartTime"].ToString();
                txtEsclationEndTime.Text = dataReader["EsclationEndTime"].ToString();
                txtWorkflowRemarks.Text = dataReader["Remark"].ToString();
            }
            ddEmployee.Enabled = false;
            Notify("Edit mode activated ...", "info", lblMsg);
            dataReader.Close();
            command.Connection.Close();
        }
        catch (Exception ex)
        {

            Notify("ERROR:" + ex, "error", lblMsg);
        }

    }
    //private void bindDDCategoryID()
    //{
    //    SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
    //}
    //private void BindddProductSubCategory()
    //{
    //    SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
    //}
    public string GenerateLvNo()
    {
        SqlCommand command = new SqlCommand("SELECT CONVERT(VARCHAR, (ISNULL (MAX(IDLvNo),0)+1001 )) FROM LoanVouchar", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        string lvno = Convert.ToString(command.ExecuteScalar());
        lvno = "LV-" + lvno;
        command.Connection.Close();
        command.Connection.Dispose();
        return lvno;
    }

    protected void ddlStation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //BindCenter();
       // BindStore();
    }

    //protected void ddCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindddProductSubCategory();
    //    BindDdProductId();

    //}

    //protected void ddProductSubCategory_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindDdProductId();
    //}

    protected void ddlLvVoucher_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = SQLQuery.ReturnDataTable("SELECT IDLvNo, LvInvoiceNo,Station, DateofLv, ResponsiblePerson, CauseOfLoan, LoanFromCenter, LoanFromStore, SaveToStore, Remarks, Verifier, RequisitionBy, DeliveredBy, EntryBy, EntryDate FROM LoanVouchar WHERE IDLvNo='" + ddlLvVoucher.SelectedValue + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            //txtLoanVoucharNo.Text = dtx["LvInvoiceNo"].ToString();
            txtDateofLv.Text = Convert.ToDateTime(dtx["DateofLv"]).ToString("dd/MM/yyyy");
            txtResponsiblePerson.Text = dtx["ResponsiblePerson"].ToString();
            txtCauseOfLoan.Text = dtx["CauseOfLoan"].ToString();
            txtStation.Text =SQLQuery.ReturnString("SELECT  Name FROM Location WHERE LocationID='"+ dtx["Station"].ToString()+"'");
            txtCenter.Text = SQLQuery.ReturnString("Select Name  from Center Where LocationID='" + dtx["Station"].ToString() + "' AND CenterID='"+dtx["LoanFromCenter"].ToString()+"'");
            txtStore.Text = SQLQuery.ReturnString("Select Name from Store WHERE StoreID = '" + dtx["LoanFromStore"].ToString() + "' ");// dtx["LoanFromStore"].ToString();

            // BindStation();
            //ddlStation.SelectedValue = dtx["Station"].ToString();
            //BindCenter();
            //ddLoanFromCenter.SelectedValue = dtx["LoanFromCenter"].ToString();
           // BindStore();
            //ddLoanFromStore.SelectedValue = dtx["LoanFromStore"].ToString();
            //ddSaveToStore.SelectedValue = dtx["SaveToStore"].ToString();
            //txtRemarks.Text = dtx["Remarks"].ToString();
            //ddVerifier.SelectedValue = dtx["Verifier"].ToString();
            //ddRequisitionBy.SelectedValue = dtx["RequisitionBy"].ToString();
            //ddDeliveredBy.SelectedValue = dtx["DeliveredBy"].ToString();
            BindAddItemsGridView();
        }
    }
}