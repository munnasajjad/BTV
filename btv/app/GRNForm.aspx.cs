
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.SqlServer.Server;
using RunQuery;

public partial class app_GRNForm : System.Web.UI.Page
{
    private readonly SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Page.Form.Enctype = "multipart/form-data";
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        if (!IsPostBack)
        {
            SQLQuery.IsUserActive(User.Identity.Name);

            BindStore();
            BindDdReferenceId();
            bindDDCategoryID();
            BindddProductSubCategory();
            BindddCurrencyId();
            BindddProduct();
            BindddlDesignation();
            BindddEmployee();
            BindddPriority();
            BindproductAddGridView();
            BindWorkFlowUserGridView();
            txtDateOfGRN.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtGrnNo.Text = GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
            txtDateofInvoiceNo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDateofPurchaseOrderNo.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtProductSHReceiveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            BindGrnGridView();
            VisibleWorkflowDateAndDay();
        }
    }

    private void VisibleWorkflowDateAndDay()
    {
        //string dayName = Convert.ToDateTime(txtEsclationStartTime.Text).ToString("dddd");
        DataTable dt = SQLQuery.ReturnDataTable("SELECT WorkFlowUserID,EsclationDay,EsclationEndTime FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnGrnId.Value + "' AND WorkFlowType = 'GRN' AND EntryBy = '" + User.Identity.Name + "'");

        if (dt.Rows.Count > 0)
        {
            txtEsclationDay.Text = dt.Rows[0]["EsclationDay"].ToString();
            txtEsclationDay.Enabled = false;

        }
        else
        {
            txtEsclationDay.Enabled = true;
            txtEsclationDay.Text = "1";
        }

    }
    private void BindddPriority()
    {
        SQLQuery.PopulateDropDown("SELECT  SequenceId,SequenceBan +' ('+Convert(varchar,Priority)+')' AS SequenceBan, SequenceEng, Priority, Type FROM WorkflowUserSequence WHERE  (Type = 'GRN')", ddlPriority, "Priority", "SequenceBan");
    }
    private bool CheckDate()
    {
        bool status = true;
        DateTime grnDate = Convert.ToDateTime(txtDateOfGRN.Text);
        DateTime invDate = Convert.ToDateTime(txtDateofInvoiceNo.Text);
        DateTime poDate = Convert.ToDateTime(txtDateofPurchaseOrderNo.Text);
        DateTime prDate = Convert.ToDateTime(txtProductSHReceiveDate.Text);
        if (poDate > grnDate || poDate > invDate || poDate > prDate)
        {
            status = false;
        }
        else if (invDate > grnDate || invDate > prDate || invDate > poDate)
        {
            status = false;
        }
        return status;
    }
    private void BindddProductSubCategory()
    {
        SQLQuery.PopulateDropDown("SELECT ProductSubCategoryID, Name FROM ProductSubCategory WHERE CategoryID = '" + ddCategoryID.SelectedValue + "'", ddProductSubCategory, "ProductSubCategoryID", "Name");
    }

    private void BindStore(string query = "")
    {
        if (Page.User.IsInRole("Super Admin"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store";
        }
        else if (Page.User.IsInRole("Admin"))
        {
            query = @"SELECT StoreAssignID, Name FROM Store WHERE (CenterID = '" + SQLQuery.GetCenterId(User.Identity.Name) + "')";
        }
        else
        {
            query = @"SELECT Store.StoreAssignID, Store.Name
            FROM Store INNER JOIN StoreAssign ON Store.StoreAssignID = StoreAssign.StoreID
            WHERE (StoreAssign.EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "')";
        }

        SQLQuery.PopulateDropDownWithoutSelect(query, ddStore, "StoreAssignID", "Name");
        if (ddStore.Text == "")
        {
            ddStore.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    private void BindddlDesignation()
    {
        string query = @"SELECT DesignationID, Name, Description, RoleID, Priority FROM Designation";
        SQLQuery.PopulateDropDown(query, ddlDesignation, "DesignationID", "Name");
    }
    private void BindddEmployee()
    {
        string sqlquery = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            sqlquery = "AND Employee.LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        string query = @"SELECT DesignationWithEmployee.Id, Employee.Name + ', ' + Designation.Name AS Name
                  FROM DesignationWithEmployee INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE DesignationWithEmployee.DesignationID='" + ddlDesignation.SelectedValue + "'" + sqlquery + "";
        SQLQuery.PopulateDropDown(query, ddEmployee, "Id", "Name");
    }

    protected void ddProductSubCategory_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindddProduct();
    }
    private void Notify(string msg, string type, Label lblNotify)
    {
        //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
        //Types: success, info, warn, error
        lblNotify.Attributes.Add("class", "xerp_" + type);
        lblNotify.Text = msg;
    }

    private bool IsProductDetailsEntry()
    {
        bool status = true;
        DataTable dtProduct = SQLQuery.ReturnDataTable(
            @"SELECT GRNProduct.GRNProductID, GRNProduct.GrnFormID, GRNProduct.GRNInvoiceNo, GRNProduct.CategoryID, GRNProduct.SubCategoryID, GRNProduct.ProductID, GRNProduct.UnitName, GRNProduct.CountryOfOrigin, 
                            GRNProduct.ManufacturingCompany, GRNProduct.Less, GRNProduct.More, GRNProduct.ReceiveProduct, GRNProduct.ReceiveQtyInWords, GRNProduct.RejectProduct, GRNProduct.PriceLetterNo, GRNProduct.CurrencyID, 
                            GRNProduct.UnitPrice, GRNProduct.TotalPrice, GRNProduct.OtherCost, GRNProduct.TotalCost, GRNProduct.EntryBy, GRNProduct.EntryDate
                            FROM GRNProduct INNER JOIN Product ON GRNProduct.ProductID = Product.ProductID INNER JOIN
                            ProductType ON Product.ProductType = ProductType.TypeId
                            WHERE  (GRNProduct.GrnFormID = '" + hdnGrnId.Value + "') AND (ProductType.Type = 'Detail')");
        foreach (DataRow item in dtProduct.Rows)
        {
            string productId = item["ProductID"].ToString();
            string qty = item["ReceiveProduct"].ToString();
            string productType = SQLQuery.GetProductType(productId);
            if ("Detail" == productType)
            {
                DataTable dtDetails = SQLQuery.ReturnDataTable(@"SELECT ProductDetailsID, GrnNO, ProductID, ProductConditionStatus, ProductStatus, CountryOfOrigin, ManufacturingCompany, ManufactureDate, PartNo, SerialNo, ModelNo, IsGuaranty, GuarantyWarrantyPeriod, StoreID, EntryBy, EntryDate, 
                                                                    Status FROM ProductDetails WHERE  (GrnNO = '" + hdnGrnId.Value + "') AND ProductID='" + productId + "'");
                if (int.Parse(qty) != dtDetails.Rows.Count)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }

        }

        return status;
    }
    protected void btnSave_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (txtRemarks.Text != "")
            {
                if (btnSave.Text.ToUpper() == "SUBMIT" && btnDraft.Text.ToUpper() == "SAVE AS DRAFT")
                {
                    
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        if (IsProductDetailsEntry())
                        {
                            if (productAddGridView.Rows.Count > 0)
                            {
                                if (WorkFlowUserGridView.Rows.Count > 0)
                                {
                                    if (VerifyPrioritySequence())
                                    {
                                        string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfGRN.Text));
                                        string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                        string isExist = SQLQuery.ReturnString("SELECT GRNInvoiceNo FROM GRNFrom WHERE GRNInvoiceNo='" + txtGrnNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND StoreID='" + ddStore.SelectedValue + "'");
                                        if (isExist == "")
                                        {
                                            if (document.HasFile)
                                            {
                                                string tExt = Path.GetExtension(document.FileName);
                                                if (tExt != ".pdf")
                                                {
                                                    Notify("Please upload the pdf file!", "warning", lblMsg);
                                                    return;
                                                }
                                            }
                                            SaveGrnData("Submitted");
                                            ClearControls();
                                            Notify("Successfully Submitted...", "success", lblMsg);
                                        }
                                        else
                                        {
                                            Notify("This " + txtGrnNo.Text + " GRN Number already exist.", "warn", lblMsg);
                                        }
                                    }
                                    else
                                    {
                                        Notify("Please check workflow priority sequence!", "warn", lblMsg);
                                    }
                                    
                                }
                                else
                                {
                                    Notify("Please add workflow details", "warn", lblMsg);
                                }
                            }
                            else
                            {
                                Notify("Please add product details", "warn", lblMsg);

                            }
                        }
                        else
                        {
                            Notify("Please add product details! before GRN submit", "warn", lblMsg);
                        }
                        
                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
                else
                {
                    if (SQLQuery.OparatePermission(lName, "Update") == "1")
                    {
                        if (IsProductDetailsEntry())
                        {
                            if (VerifyPrioritySequence())
                            {
                                if (document.HasFile)
                                {
                                    string tExt = Path.GetExtension(document.FileName);
                                    if (tExt != ".pdf")
                                    {
                                        Notify("Please upload the pdf file!", "warning", lblMsg);
                                        return;
                                    }
                                }
                                UpdateGrnData("Submitted");
                                ClearControls();
                                btnSave.Text = "Submit";
                                btnDraft.Text = "SAVE AS DRAFT";
                                productAddGridView.DataBind();
                                Notify("Successfully Submitted...", "success", lblMsg);
                                btnDraft.Enabled = true;
                            }
                            else
                            {
                                Notify("Please check workflow priority sequence!", "warn", lblMsg);
                            }
                            
                        }
                        else
                        {
                            Notify("Please add product details! before GRN submit", "warn", lblMsg);
                        }
                        
                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
            }
            else
            {
                Notify("Remarks field can't be empty!", "warn", lblMsg);
            }

        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            hdnGRNInvoiceNo.Value = "";
            hdnGrnId.Value = "";
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";

            // VisibleWorkflowDateAndDay();
            BindWorkFlowUserGridView();
            BindproductAddGridView();
            BindGrnGridView();
            txtGrnNo.Text = GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
        }
    }

    private bool VerifyPrioritySequence()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = true;
        int priorityCount = 1;
        DataTable priorityDataTable = new DataTable();
        if (String.Empty == hdnGrnId.Value)
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND EntryBy = '" + User.Identity.Name.Trim() + "' AND WorkFlowType = 'GRN' ORDER BY Priority");
        }
        else
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND WorkFlowType = 'GRN' ORDER BY Priority");
        }
         
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            if (priorityCount != int.Parse(priorityDataRow["Priority"].ToString()))
            {
                sequentStatus = false;
            }
            priorityCount++;
        }

        return sequentStatus;
    }
    private void bindDDCategoryID()
    {
        SQLQuery.PopulateDropDown("Select ProductCategoryID, Name from ProductCategory", ddCategoryID, "ProductCategoryID", "Name");
    }
    protected void ddCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddProductSubCategory();
        BindddProduct();
    }

    private void SaveGrnData(string saveMode)
    {

        string type = "New";
        string insertQuery = "";
        string parameter = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);

            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "GRN-" + Guid.NewGuid().GetHashCode() + tExt);
                document.SaveAs(Server.MapPath("./Uploads/GRN/") + fileName);
                docUrl = "./Uploads/GRN/" + fileName;
                insertQuery = "DocumentUrl,";
                parameter = "@DocumentUrl,";
            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }


        SQLQuery.Empty2Zero(txtPurchaseOrderNo);
        string date = Convert.ToDateTime(txtDateOfGRN.Text).ToString("yyyy-MM-dd");
        //string submitDate = "";

        if (saveMode == "Submitted")
        {
            insertQuery += "SubmitDate,";
            parameter += "@SubmitDate,";
        }

        SqlCommand command = new SqlCommand(@"INSERT INTO GRNFrom (" + insertQuery + @"GRNInvoiceNo,Type, DateOfGRN,LocationID, StoreID, ReferenceID, InvoiceNo,FinYear,Supplier, DateofInvoiceNo, PurchaseOrderNo, DateofPurchaseOrderNo, ProductSHReceiveDate, TotalAmount, Remarks, PreparedBy, PreparedDate, SaveMode, EntryBy)
                                            VALUES (" + parameter + "@GRNInvoiceNo,@Type, @DateOfGRN,@LocationID, @StoreID, @ReferenceID, @InvoiceNo,@FinYear,@Supplier, @DateofInvoiceNo, @PurchaseOrderNo, @DateofPurchaseOrderNo, @ProductSHReceiveDate, @TotalAmount, @Remarks, @PreparedBy, @PreparedDate, @SaveMode, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        if (ddReferenceID.SelectedItem.Text == "Existing")
        {
            type = "Old";
            //command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
        }
        string lName = Page.User.Identity.Name.ToString();
        command.Parameters.Add("@GRNInvoiceNo", SqlDbType.VarChar).Value = txtGrnNo.Text.Trim();
        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
        command.Parameters.Add("@DateOfGRN", SqlDbType.DateTime).Value = date;
        command.Parameters.Add("@LocationID", SqlDbType.Int).Value = SQLQuery.GetLocationID(User.Identity.Name);
        command.Parameters.Add("@StoreID", SqlDbType.Int).Value = ddStore.SelectedValue;
        command.Parameters.Add("@ReferenceID", SqlDbType.Int).Value = ddReferenceID.SelectedValue;
        command.Parameters.Add("@InvoiceNo", SqlDbType.VarChar).Value = txtInvoiceNo.Text;
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfGRN.Text));
        command.Parameters.Add("@Supplier", SqlDbType.NVarChar).Value = txtSupplier.Text;
        command.Parameters.Add("@DateofInvoiceNo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateofInvoiceNo.Text).ToString("yyyy-MM-dd");
        command.Parameters.Add("@PurchaseOrderNo", SqlDbType.VarChar).Value = txtPurchaseOrderNo.Text;
        command.Parameters.Add("@DateofPurchaseOrderNo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateofPurchaseOrderNo.Text).ToString("yyyy-MM-dd");
        command.Parameters.Add("@ProductSHReceiveDate", SqlDbType.DateTime).Value = Convert.ToDateTime(txtProductSHReceiveDate.Text).ToString("yyyy-MM-dd");
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotalAmount.Text);
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;
        command.Parameters.Add("@PreparedBy", SqlDbType.Int).Value = SQLQuery.GetEmployeeID(lName);
        command.Parameters.Add("@PreparedDate", SqlDbType.DateTime).Value = DateTime.Now;
        command.Parameters.Add("@SaveMode", SqlDbType.VarChar).Value = saveMode;
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.VarChar).Value = docUrl;
        }
        if (saveMode == "Submitted")
        {
            command.Parameters.Add("@SubmitDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }
        command.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

        string query = "";
        if (hdnGrnId.Value == "")
        {
            query = " AND EntryBy='" + lName + "'";
        }
        string grnId = SQLQuery.ReturnString("SELECT MAX(IDGrnNO) AS GrnId FROM GRNFrom WHERE LocationID='" + SQLQuery.GetLocationID(lName) + "' AND EntryBy='" + lName + "'");
        SQLQuery.ExecNonQry("UPDATE GRNProduct SET GRNInvoiceNo='" + txtGrnNo.Text.Trim() + "',GrnFormID='" + grnId + "'  WHERE GrnFormID='" + hdnGrnId.Value + "' " + query);
        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET WorkFlowTypeID='" + grnId + "', VoucherNo='" + txtGrnNo.Text.Trim() + "'  WHERE WorkFlowTypeID = '0' AND WorkFlowType='GRN' AND EntryBy='" + lName + "' ");

        if (saveMode == "Submitted")
        {
            string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + grnId + "' AND WorkFlowType='GRN'";

            DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
            foreach (DataRow item in dtUser.Rows)
            {
                if (item["Priority"].ToString() == "1")
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), grnId);
                }
            }

        }

    }

    private void UpdateGrnData(string saveMode)
    {
        SQLQuery.Empty2Zero(txtPurchaseOrderNo);
        string type = "New";
        string updateQuery = "";
        string returnUser = "";
        string workflowStatus = SQLQuery.ReturnString("SELECT WorkflowStatus FROM GRNFrom WHERE IDGrnNO='" + hdnGrnId.Value + "'");
        if (workflowStatus == "Return" && saveMode == "Submitted")
        {
            returnUser = SQLQuery.ReturnString("SELECT ReturnOrHoldUserID FROM GRNFrom WHERE IDGrnNO='" + hdnGrnId.Value + "'");
            workflowStatus = "Pending";
        }

        //string submitDate = "";
        if (saveMode == "Submitted")
        {
            updateQuery = "SubmitDate=@SubmitDate,";
        }

        //string parameter = "";
        string docUrl = "";
        if (document.HasFile)
        {
            string tExt = Path.GetExtension(document.FileName);
            try
            {
                string file = Path.GetFileName(document.FileName);
                string fileName = file.Replace(file, "GRN-" + Guid.NewGuid().GetHashCode() + tExt);
                //string fileName = file.Replace(file, "Document-" + txtGrnNo.Text + "." + tExt);
                if (hdnDocumentUrl.Value != "")
                {
                    SQLQuery.DeleteFile(Server.MapPath(hdnDocumentUrl.Value));
                }
                document.SaveAs(Server.MapPath("./Uploads/GRN/") + fileName);
                docUrl = "./Uploads/GRN/" + fileName;
                updateQuery += "DocumentUrl=@DocumentUrl,";

            }
            catch (Exception ex)
            {
                Notify("ERROR" + ex.ToString(), "error", lblMsg);
            }

        }
        if (ddReferenceID.SelectedItem.Text == "Existing")
        {
            type = "Old";

        }

        SqlCommand command = new SqlCommand("Update  GRNFrom SET DateOfGRN=@DateOfGRN, " + updateQuery + " StoreID= @StoreID,Type=@Type, ReferenceID= @ReferenceID,FinYear=@FinYear, InvoiceNo=  @InvoiceNo,Supplier=@Supplier,  DateofInvoiceNo=@DateofInvoiceNo,  PurchaseOrderNo= @PurchaseOrderNo,  DateofPurchaseOrderNo= @DateofPurchaseOrderNo,  ProductSHReceiveDate= @ProductSHReceiveDate, TotalAmount= @TotalAmount,  Remarks= @Remarks,  PreparedBy= @PreparedBy,  PreparedDate= @PreparedDate,  SaveMode=@SaveMode,WorkflowStatus=@WorkflowStatus, EntryBy=@EntryBy WHERE IDGrnNO='" + hdnGrnId.Value + "' ", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        string lName = Page.User.Identity.Name.ToString();
        command.Parameters.Add("@DateOfGRN", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateOfGRN.Text).ToString("yyyy-MM-dd");
        command.Parameters.Add("@StoreID", SqlDbType.Int).Value = ddStore.SelectedValue;
        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
        command.Parameters.Add("@ReferenceID", SqlDbType.Int).Value = ddReferenceID.SelectedValue;
        command.Parameters.Add("@FinYear", SqlDbType.VarChar).Value = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfGRN.Text));
        command.Parameters.Add("@InvoiceNo", SqlDbType.VarChar).Value = txtInvoiceNo.Text;
        command.Parameters.Add("@Supplier", SqlDbType.NVarChar).Value = txtSupplier.Text;
        command.Parameters.Add("@DateofInvoiceNo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateofInvoiceNo.Text).ToString("yyyy-MM-dd");
        command.Parameters.Add("@PurchaseOrderNo", SqlDbType.VarChar).Value = txtPurchaseOrderNo.Text;
        command.Parameters.Add("@DateofPurchaseOrderNo", SqlDbType.DateTime).Value = Convert.ToDateTime(txtDateofPurchaseOrderNo.Text).ToString("yyyy-MM-dd");
        command.Parameters.Add("@ProductSHReceiveDate", SqlDbType.DateTime).Value = Convert.ToDateTime(txtProductSHReceiveDate.Text).ToString("yyyy-MM-dd");
        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(txtTotalAmount.Text);
        command.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = txtRemarks.Text;
        command.Parameters.Add("@PreparedBy", SqlDbType.Int).Value = SQLQuery.GetEmployeeID(lName);
        command.Parameters.Add("@PreparedDate", SqlDbType.DateTime).Value = DateTime.Now;
        command.Parameters.Add("@SaveMode", SqlDbType.VarChar).Value = saveMode;
        command.Parameters.Add("@WorkflowStatus", SqlDbType.VarChar).Value = workflowStatus;
        if (document.HasFile)
        {
            command.Parameters.Add("@DocumentUrl", SqlDbType.NVarChar).Value = docUrl;
        }
        if (saveMode == "Submitted")
        {
            command.Parameters.Add("@SubmitDate", SqlDbType.DateTime).Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt");
        }
        command.Parameters.Add("@EntryBy", SqlDbType.NVarChar).Value = lName;
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();

        if (saveMode == "Submitted")
        {
            if (returnUser != "")
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowUserID='" + returnUser + "'";
                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    DateTime startDateTime = DateTime.Now;
                    DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                    SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1',PermissionStatus=''  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "'");
                    NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnGrnId.Value);
                }
            }
            else
            {
                string sqlquery = @"SELECT WorkFlowUserID, WorkFlowTypeID, WorkFlowType, VoucherNo, EmployeeID, DesignationId, Priority, EsclationStartTime, EsclationEndTime, EsclationDay, Remark, TaskStatus, UserRemarks, ApproveDeclineDate, 
                  PermissionStatus, EntryBy, EntryDate, IsActive FROM WorkFlowUser WHERE WorkFlowTypeID='" + hdnGrnId.Value + "' AND WorkFlowType='GRN'";

                DataTable dtUser = SQLQuery.ReturnDataTable(sqlquery);
                foreach (DataRow item in dtUser.Rows)
                {
                    if (item["Priority"].ToString() == "1")
                    {
                        DateTime startDateTime = DateTime.Now;
                        DateTime endDateTime = startDateTime.AddDays(int.Parse(item["EsclationDay"].ToString()));
                        SQLQuery.ExecNonQry("UPDATE WorkFlowUser SET EsclationStartTime='" + startDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "', EsclationEndTime='" + endDateTime.ToString("yyyy-MM-dd hh:mm:ss tt") + "',IsActive='1'  WHERE WorkFlowUserID = '" + item["WorkFlowUserID"] + "' AND EntryBy='" + lName + "' ");
                        NotifyToEmployee(item["EmployeeID"].ToString(), item["VoucherNo"].ToString(), hdnGrnId.Value);
                    }
                }
            }
        }


    }
    protected void GrnGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                int index = Convert.ToInt32(GrnGridView.SelectedIndex);
                Label lblEditId = GrnGridView.Rows[index].FindControl("lblIDGrnNO") as Label;
                Label lblEntryBy = GrnGridView.Rows[index].FindControl("lblEntryBy") as Label;
                hdnGrnId.Value = lblEditId.Text;
                Label lblGrnInvoiceNo = GrnGridView.Rows[index].FindControl("lblGRNInvoiceNo") as Label;
                hdnGRNInvoiceNo.Value = lblGrnInvoiceNo.Text;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM GRNFrom WHERE IDGrnNO='" + lblEditId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM GRNFrom WHERE IDGrnNO='" + lblEditId.Text + "'");

                if (Page.User.IsInRole("Super Admin"))
                {
                    if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Approved"))
                    {
                        EditMode(lblEditId.Text);
                        btnDraft.Enabled = true;
                        btnSave.Enabled = true;
                    }
                }
                else if (Page.User.Identity.Name == lblEntryBy.Text)
                {
                    if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                    {
                        EditMode(lblEditId.Text);
                        btnDraft.Enabled = true;
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        Notify("This " + lblGrnInvoiceNo.Text + " already submitted. If you need to any change please contact higher authority.", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("This voucher " + lblGrnInvoiceNo.Text + " entry by user is " + lblEntryBy.Text + ". You are not authorize edit the voucher. If you need to any change please contact higher authority.", "warn", lblMsg);
                }
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

    private void EditMode(string lblEditId)
    {
        DataTable dt = SQLQuery.ReturnDataTable("SELECT IDGrnNO, Supplier,DocumentUrl, StoreDivLedgerWritter, GRNInvoiceNo, DateOfGRN, ReferenceID, InvoiceNo, DateofInvoiceNo, PurchaseOrderNo, DateofPurchaseOrderNo, ProductSHReceiveDate, TotalAmount, Remarks, PreparedBy, PreparedDate, ProductInspectorApprovedBy, ApprovedDate, AccountDivLedgerWritter, AccountDivLedgerDate, StoreID, SaveMode, EntryDate, EntryBy FROM GRNFrom WHERE IDGrnNO='" + lblEditId + "'");
        foreach (DataRow dtx in dt.Rows)
        {
            hdnGrnId.Value = dtx["IDGrnNO"].ToString();
            btnSave.Visible = true;
            hdnGRNInvoiceNo.Value = dtx["GRNInvoiceNo"].ToString();
            txtGrnNo.Text = dtx["GRNInvoiceNo"].ToString();
            txtDateOfGRN.Text = Convert.ToDateTime(dtx["DateOfGRN"]).ToString("dd/MM/yyyy");
            ddReferenceID.SelectedValue = dtx["ReferenceID"].ToString();
            txtInvoiceNo.Text = dtx["InvoiceNo"].ToString();
            txtSupplier.Text = dtx["Supplier"].ToString();
            txtDateofInvoiceNo.Text = Convert.ToDateTime(dtx["DateofInvoiceNo"]).ToString("dd/MM/yyyy");
            txtPurchaseOrderNo.Text = dtx["PurchaseOrderNo"].ToString();
            txtDateofPurchaseOrderNo.Text = Convert.ToDateTime(dtx["DateofPurchaseOrderNo"]).ToString("dd/MM/yyyy");
            txtProductSHReceiveDate.Text = Convert.ToDateTime(dtx["ProductSHReceiveDate"]).ToString("dd/MM/yyyy");
            txtTotalAmount.Text = dtx["TotalAmount"].ToString();
            txtRemarks.Text = dtx["Remarks"].ToString();
            hdnDocumentUrl.Value = dtx["DocumentUrl"].ToString();
            BindStore();
            ddStore.SelectedValue = dtx["StoreID"].ToString();
        }
        BindproductAddGridView();
        BindWorkFlowUserGridView();
        VisibleWorkflowDateAndDay();
        btnSave.Text = "Submit";
        btnDraft.Text = "Update Draft";
        Notify("Edit mode activated ...", "info", lblMsg);

    }

    protected void GrnGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (SQLQuery.OparatePermission(lName, "Delete") == "1")
            {
                int index = Convert.ToInt32(e.RowIndex);
                Label lblId = GrnGridView.Rows[index].FindControl("lblIDGrnNO") as Label;
                string saveMode = SQLQuery.ReturnString(@"SELECT SaveMode FROM GRNFrom WHERE IDGrnNO='" + lblId.Text + "'");
                string workflowStatus = SQLQuery.ReturnString(@"SELECT WorkflowStatus FROM GRNFrom WHERE IDGrnNO='" + lblId.Text + "'");

                string grnInvoiceNo = SQLQuery.ReturnString("SELECT GRNInvoiceNo FROM GRNFrom WHERE IDGrnNO = '" + lblId.Text + "'");
                if (Page.User.IsInRole("Super Admin"))
                {
                    if ((saveMode == "Submitted" && workflowStatus == "Approved") || (saveMode == "Drafted" && workflowStatus == "Pending"))
                    {
                        DeleteGrnData(lblId.Text);
                    }

                }
                else
                {
                    if ((saveMode == "Drafted" || workflowStatus == "Return") && workflowStatus != "Approved")
                    {
                        DeleteGrnData(lblId.Text);
                    }
                    else
                    {
                        Notify("This " + grnInvoiceNo + " already submitted. If you need to delete. please contact higher authority.", "warn", lblMsg);
                    }
                }

                BindproductAddGridView();
                BindWorkFlowUserGridView();
                BindGrnGridView();
                VisibleWorkflowDateAndDay();
                txtGrnNo.Text = GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
                ClearControls();

            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "warn", lblMsg);
        }

    }

    private void DeleteGrnData(string lblId)
    {
        string query = "DELETE GRNFrom WHERE IDGrnNO='" + lblId + "' ";
        query += "Delete GRNProduct WHERE GrnFormID='" + lblId + "' ";
        query += " Delete WorkFlowUser WHERE WorkFlowTypeID='" + lblId + "' AND WorkFlowType='GRN'";
        SQLQuery.ExecNonQry(query);
        Notify("Successfully Deleted...", "success", lblMsg);
    }
    protected void btnClear_OnClick(object sender, EventArgs e)
    {
        Response.Redirect("./Default.aspx");
    }
    private void BindGrnGridView()
    {
        string reportUrl = ConfigurationManager.AppSettings["ReportUrl"].ToString();
        string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + reportUrl + "/XerpReports/";
        string query = "";
        if (!Page.User.IsInRole("Super Admin"))
        {
            //query = "WHERE LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "' AND StoreID IN(SELECT StoreID FROM StoreAssign WHERE(EmployeeID = '" + SQLQuery.GetEmployeeID(User.Identity.Name) + "'))";
            query = "AND LocationID='" + SQLQuery.GetLocationID(User.Identity.Name) + "'";
        }
        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT IDGrnNO,EntryBy,CurrentWorkflowUser, GRNInvoiceNo, '" + baseUrl + "' +'GRNReport.aspx?GrnId='+CONVERT(VARCHAR,IDGrnNO) As GrnID, CONVERT(VARCHAR,DateOfGRN,103) AS DateOfGRN,DocumentUrl, TotalAmount, Remarks, SaveMode, WorkflowStatus, PreparedBy FROM GRNFrom Where StoreID='" + ddStore.SelectedValue + "'" + query + " Order by EntryDate Desc"); ;
        GrnGridView.DataSource = dt;
        GrnGridView.DataBind();
    }
    private void BindDdReferenceId()
    {
        SQLQuery.PopulateDropDown("Select ReferenceID, Name from Reference", ddReferenceID, "ReferenceID", "Name");
    }
    private void BindddCurrencyId()
    {
        SQLQuery.PopulateDropDownWithoutSelect("SELECT Id, Country + ', ' + Symbol + ' (' + ISOCode + ')' AS CurrencyName FROM Currencies", ddCurrencyID, "Id", "CurrencyName");
    }
    private void BindddProduct()
    {
        SQLQuery.PopulateDropDown("Select ProductID, Name from Product Where ProductCategoryID='" + ddCategoryID.SelectedValue + "' AND ProductSubCategoryID='" + ddProductSubCategory.SelectedValue + "'", ddProductName, "ProductID", "Name");
        txtUnitName.Text = SQLQuery.ReturnString(@"SELECT Unit.Name AS UnitName FROM Product INNER JOIN Unit ON Product.UnitID = Unit.UnitID WHERE (Product.ProductID = '" + ddProductName.SelectedValue + "')");
    }

    protected void ddReferenceID_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtGrnNo.Text = ddReferenceID.SelectedItem.Text == "Existing" ? GenerateVoucherNumber.GetOldGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue) : GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
    }

    private void ClearControls()
    {
        txtDateOfGRN.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txtInvoiceNo.Text = "";
        txtDateofInvoiceNo.Text = "";
        txtPurchaseOrderNo.Text = "";
        txtDateofPurchaseOrderNo.Text = "";
        txtProductSHReceiveDate.Text = "";
        txtUnitName.Text = "";
        //txtUnitNo.Text = "";
        txtSupplier.Text = "";
        txtless.Text = "";
        txtMore.Text = "";
        txtReceiveProduct.Text = "";
        txtRejectProduct.Text = "";
        txtPriceLetterNo.Text = "";
        txtUnitPrice.Text = "";
        txtTotalPrice.Text = "";
        txtOthersCost.Text = "";
        txtTotalCost.Text = "";
        txtRemarks.Text = "";
    }
    protected void ddProductName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtUnitName.Text = SQLQuery.ReturnString(@"SELECT Unit.Name AS UnitName FROM Product INNER JOIN Unit ON Product.UnitID = Unit.UnitID WHERE (Product.ProductID = '" + ddProductName.SelectedValue + "')");
    }
    private void InsertToGrnProduct()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        if (btnDraft.Text.ToUpper() != "UPDATE DRAFT")
        {
            command = new SqlCommand(@"INSERT INTO GRNProduct (GrnFormID, ProductID,CategoryID,SubCategoryID,UnitName, Less, More, ReceiveProduct, ReceiveQtyInWords, RejectProduct, PriceLetterNo, CurrencyID, UnitPrice, TotalPrice, OtherCost, TotalCost, EntryBy, EntryDate) 
                                       VALUES ('',@ProductID,@CategoryID,@SubCategoryID,@UnitName,@Less,@More,@ReceiveProduct,@ReceiveQtyInWords,@RejectProduct,@PriceLetterNo,@CurrencyID,@UnitPrice,@TotalPrice,@OtherCost,@TotalCost,@EntryBy,@EntryDate )", connection);
        }
        else
        {
            command = new SqlCommand(@"INSERT INTO GRNProduct (GrnFormID,GRNInvoiceNo,CategoryID,SubCategoryID,ProductID,UnitName, Less, More, ReceiveProduct, ReceiveQtyInWords,RejectProduct, PriceLetterNo, CurrencyID, UnitPrice, TotalPrice, OtherCost, TotalCost, EntryBy, EntryDate) 
                                       VALUES (@GrnFormID,@GRNInvoiceNo,@CategoryID,@SubCategoryID,@ProductID,@UnitName,@Less,@More,@ReceiveProduct,@ReceiveQtyInWords,@RejectProduct,@PriceLetterNo,@CurrencyID,@UnitPrice,@TotalPrice,@OtherCost,@TotalCost,@EntryBy,@EntryDate )", connection);
        }
        SQLQuery.Empty2Zero(txtless);
        SQLQuery.Empty2Zero(txtMore);
        SQLQuery.Empty2Zero(txtReceiveProduct);
        SQLQuery.Empty2Zero(txtRejectProduct);
        SQLQuery.Empty2Zero(txtPriceLetterNo);
        SQLQuery.Empty2Zero(txtUnitPrice);
        SQLQuery.Empty2Zero(txtOthersCost);

        command.Parameters.AddWithValue("@GrnFormID", hdnGrnId.Value);
        command.Parameters.AddWithValue("@GRNInvoiceNo", hdnGRNInvoiceNo.Value);
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@ProductID", ddProductName.SelectedValue);
        command.Parameters.AddWithValue("@UnitName", txtUnitName.Text);
        //command.Parameters.AddWithValue("@CountryOfOrigin", txtCountryOfOrigin.Text);
        //command.Parameters.AddWithValue("@ManufacturingCompany", txtManufacturingCompany.Text);
        command.Parameters.AddWithValue("@Less", Convert.ToDecimal(txtless.Text));
        command.Parameters.AddWithValue("@More", Convert.ToDecimal(txtMore.Text));
        command.Parameters.AddWithValue("@ReceiveProduct", Convert.ToDecimal(txtReceiveProduct.Text));
        command.Parameters.AddWithValue("@ReceiveQtyInWords", SQLQuery.Int2WordsBangla(txtReceiveProduct.Text.ToString()));
        command.Parameters.AddWithValue("@RejectProduct", Convert.ToDecimal(txtRejectProduct.Text));
        command.Parameters.AddWithValue("@PriceLetterNo", Convert.ToInt32(txtPriceLetterNo.Text));
        command.Parameters.AddWithValue("@CurrencyID", ddCurrencyID.SelectedValue);
        command.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(txtUnitPrice.Text));
        decimal totalPrice = Convert.ToDecimal(txtReceiveProduct.Text) * Convert.ToDecimal(txtUnitPrice.Text);
        command.Parameters.AddWithValue("@TotalPrice", totalPrice);
        command.Parameters.AddWithValue("@OtherCost", Convert.ToDecimal(txtOthersCost.Text));
        decimal totalCost = Convert.ToDecimal(txtOthersCost.Text) + totalPrice;
        command.Parameters.AddWithValue("@TotalCost", totalCost);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
    }
    private void UpdateGrnProduct()
    {


        SQLQuery.Empty2Zero(txtless);
        SQLQuery.Empty2Zero(txtMore);
        SQLQuery.Empty2Zero(txtReceiveProduct);
        SQLQuery.Empty2Zero(txtRejectProduct);
        SQLQuery.Empty2Zero(txtPriceLetterNo);
        SQLQuery.Empty2Zero(txtUnitPrice);
        SQLQuery.Empty2Zero(txtOthersCost);
        string query = @"UPDATE GRNProduct SET ProductID=@ProductID,CategoryID=@CategoryID, SubCategoryID=@SubCategoryID, Less=@Less, More=@More, ReceiveProduct=@ReceiveProduct, ReceiveQtyInWords=@ReceiveQtyInWords, RejectProduct=@RejectProduct, PriceLetterNo=@PriceLetterNo, CurrencyID=@CurrencyID, UnitPrice=@UnitPrice, TotalPrice=@TotalPrice, OtherCost=@OtherCost, TotalCost=@TotalCost WHERE GRNProductID = '" + hdnProductId.Value + "'";
        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@ProductID", ddProductName.SelectedValue);
        command.Parameters.AddWithValue("@CategoryID", ddCategoryID.SelectedValue);
        command.Parameters.AddWithValue("@SubCategoryID", ddProductSubCategory.SelectedValue);
        command.Parameters.AddWithValue("@Less", Convert.ToDecimal(txtless.Text));
        command.Parameters.AddWithValue("@More", Convert.ToDecimal(txtMore.Text));
        command.Parameters.AddWithValue("@ReceiveProduct", Convert.ToDecimal(txtReceiveProduct.Text));
        command.Parameters.AddWithValue("@ReceiveQtyInWords", SQLQuery.Int2WordsBangla(txtReceiveProduct.Text.ToString()));
        command.Parameters.AddWithValue("@RejectProduct", Convert.ToDecimal(txtRejectProduct.Text));
        command.Parameters.AddWithValue("@PriceLetterNo", Convert.ToInt32(txtPriceLetterNo.Text));
        command.Parameters.AddWithValue("@CurrencyID", ddCurrencyID.SelectedValue);
        command.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(txtUnitPrice.Text));
        decimal totalPrice = Convert.ToDecimal(txtReceiveProduct.Text) * Convert.ToDecimal(txtUnitPrice.Text);
        command.Parameters.AddWithValue("@TotalPrice", totalPrice);
        command.Parameters.AddWithValue("@OtherCost", Convert.ToDecimal(txtOthersCost.Text));
        decimal totalCost = Convert.ToDecimal(txtOthersCost.Text) + totalPrice;
        command.Parameters.AddWithValue("@TotalCost", totalCost);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    protected void btnAdd_OnClick(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name.ToString();
        try
        {
            if (btnAdd.Text.ToUpper() == "ADD PRODUCT")
            {
                string isProductExists = SQLQuery.ReturnString("SELECT ProductID FROM GRNProduct WHERE ProductID = '" + ddProductName.SelectedValue + "' AND GrnFormID='" + hdnGrnId.Value + "' AND EntryBy = '" + lName + "'");
                if (isProductExists != ddProductName.SelectedValue)
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        InsertToGrnProduct();
                        BindproductAddGridView();
                        ClearProductField();
                        Notify("Insert Successful", "info", lblMsg);
                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("Product already exists!", "warn", lblMsg);
                }
            }
            else
            {
                string isProductExists = SQLQuery.ReturnString("SELECT ProductID FROM GRNProduct WHERE ProductID = '" + ddProductName.SelectedValue + "' AND GrnFormID='" + hdnGrnId.Value + "' AND GRNProductID <>'" + hdnProductId.Value + "' AND EntryBy = '" + lName + "'");
                if (isProductExists == "")
                {
                    if (SQLQuery.OparatePermission(lName, "Update") == "1")
                    {
                        UpdateGrnProduct();
                        BindproductAddGridView();
                        ClearProductField();
                        btnAdd.Text = "Add Product";
                        Notify("Successfully Updated...", "success", lblMsg);
                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
                else
                {
                    Notify("Product already exists!", "warn", lblMsg);
                }
            }

        }
        catch (Exception ex)
        {
            Notify("ERROR" + ex, "error", lblMsg);
        }
    }
    private void ClearProductField()
    {
        ddProductName.SelectedValue = "0";
        txtUnitName.Text = "";
        txtless.Text = "";
        txtMore.Text = "";
        txtReceiveProduct.Text = "";
        txtRejectProduct.Text = "";
        txtPriceLetterNo.Text = "";
        txtUnitPrice.Text = "";
        txtTotalPrice.Text = "";
        txtOthersCost.Text = "";
        txtTotalCost.Text = "";
    }
    private void BindproductAddGridView()
    {
        string query = "";
        if (Page.User.IsInRole("Super Admin") && hdnGrnId.Value!="")
        {
            if (btnSave.Visible)
            {
                query = @"WHERE GRNProduct.GrnFormID = '" + hdnGrnId.Value + "'";
            }
            else
            {
                query = @"WHERE GRNProduct.GrnFormID = '" + hdnGrnId.Value + "' AND EntryBy='" + User.Identity.Name + "'";
            }
            
        }
        else
        {
            query = @"WHERE GRNProduct.GrnFormID = '" + hdnGrnId.Value + "' AND EntryBy='" + User.Identity.Name + "'";
        }
        string sql = @"SELECT GRNProduct.GRNProductID, GRNProduct.GRNInvoiceNo, GRNProduct.CountryOfOrigin, GRNProduct.ManufacturingCompany, GRNProduct.Less, GRNProduct.More, GRNProduct.ReceiveProduct AS ReceiveProduct , 
                         GRNProduct.RejectProduct, GRNProduct.PriceLetterNo, GRNProduct.CurrencyID, GRNProduct.UnitPrice AS UnitPrice, GRNProduct.TotalPrice, GRNProduct.OtherCost, GRNProduct.TotalCost AS TotalCost, GRNProduct.EntryBy, GRNProduct.EntryDate, 
                         Product.Name AS ProductName FROM GRNProduct INNER JOIN
                         Product ON GRNProduct.ProductID = Product.ProductID " + query + " ";

        SqlCommand command = new SqlCommand(sql, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        productAddGridView.EmptyDataText = "No data added ...";
        productAddGridView.DataSource = command.ExecuteReader();
        productAddGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
        txtTotalAmount.Text = SQLQuery.ReturnString("SELECT ISNULL(SUM(TotalCost),0) FROM GRNProduct WHERE GrnFormID='" + hdnGrnId.Value + "' AND EntryBy='" + User.Identity.Name + "'");
    }

    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(e.CommandArgument.ToString());
            string lName = Page.User.Identity.Name;
            if (SQLQuery.OparatePermission(lName, "Delete") == "1")
            {
                //int index = Convert.ToInt32(e.RowIndex);
                //Label lblId = productAddGridView.Rows[index].FindControl("lblGRNProductID") as Label;
                SQLQuery.ExecNonQry(" Delete GRNProduct WHERE GRNProductID='" + id + "' ");
                BindproductAddGridView();
                Notify("Successfully Deleted...", "success", lblMsg);
            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void productAddGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(productAddGridView.SelectedIndex);
            Label label = productAddGridView.Rows[index].FindControl("lblGRNProductID") as Label;
            hdnProductId.Value = label.Text;
            string query = @"SELECT GRNProductID, GRNInvoiceNo, ProductID,CategoryID,SubCategoryID, CountryOfOrigin, ManufacturingCompany, Less, More, ReceiveProduct, RejectProduct, PriceLetterNo, CurrencyID, UnitPrice, TotalPrice, OtherCost, TotalCost FROM GRNProduct WHERE GRNProductID = '" + hdnProductId.Value + "'";
            SqlCommand command = new SqlCommand(query, connection);
            command.Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                btnAdd.Text = "Update";
                bindDDCategoryID();
                ddCategoryID.SelectedValue = dataReader["CategoryID"].ToString();
                BindddProductSubCategory();
                ddProductSubCategory.SelectedValue = dataReader["SubCategoryID"].ToString();
                BindddProduct();
                ddProductName.SelectedValue = dataReader["ProductID"].ToString();
                //txtCountryOfOrigin.Text = dataReader["CountryOfOrigin"].ToString();
                //txtManufacturingCompany.Text = dataReader["ManufacturingCompany"].ToString();
                txtless.Text = dataReader["Less"].ToString();
                txtMore.Text = dataReader["More"].ToString();
                txtReceiveProduct.Text = dataReader["ReceiveProduct"].ToString();
                txtRejectProduct.Text = dataReader["RejectProduct"].ToString();
                txtPriceLetterNo.Text = dataReader["PriceLetterNo"].ToString();
                ddCurrencyID.SelectedValue = dataReader["CurrencyID"].ToString();
                txtUnitPrice.Text = dataReader["UnitPrice"].ToString();
                txtTotalPrice.Text = dataReader["TotalPrice"].ToString();
                txtOthersCost.Text = dataReader["OtherCost"].ToString();
                txtTotalCost.Text = dataReader["TotalCost"].ToString();

            }
            dataReader.Close();
            connection.Close();
        }
        catch (Exception ex)
        {
            Notify("ERROR: " + ex, "error", lblMsg);

        }

    }

    private void InsertToWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand command;
        int typeId;
        if (hdnGrnId.Value == "")
        {
            typeId = 0;
        }
        else
        {
            typeId = Convert.ToInt32(hdnGrnId.Value);
        }
        command = new SqlCommand(@"INSERT INTO WorkFlowUser ( WorkFlowTypeID,VoucherNo,WorkFlowType, EmployeeID,DesignationId, Priority, EsclationDay,  Remark, TaskStatus, EntryBy, EntryDate) 
                                       VALUES (@WorkFlowTypeID,@VoucherNo,'GRN',@EmployeeID,@DesignationId,@Priority,@EsclationDay,@Remark,'1',@EntryBy,@EntryDate )", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@WorkFlowTypeID", typeId);
        command.Parameters.AddWithValue("@VoucherNo", hdnGRNInvoiceNo.Value);
        command.Parameters.AddWithValue("@EmployeeID", ddEmployee.SelectedValue);
        command.Parameters.AddWithValue("@DesignationId", ddlDesignation.SelectedValue);
        command.Parameters.AddWithValue("@Priority", ddlPriority.SelectedValue);
        command.Parameters.AddWithValue("@EsclationDay", txtEsclationDay.Text);
        command.Parameters.AddWithValue("@Remark", txtRemark.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now.ToString("yyyy-MM-dd"));
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();

        txtRemark.Text = "";
    }
    private void UpdateWorkFlowUser()
    {
        string lName = Page.User.Identity.Name.ToString();

        string query = @"UPDATE WorkFlowUser SET EmployeeID=@EmployeeID,VoucherNo=@VoucherNo,DesignationId=@DesignationId,EsclationDay=@EsclationDay, Priority=@Priority, Remark=@Remark, EntryBy=@EntryBy, EntryDate=@EntryDate WHERE WorkFlowUserID = '" + hdnWorkFlowUserId.Value + "'";
        SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Parameters.AddWithValue("@DesignationId", ddlDesignation.SelectedValue);
        command.Parameters.AddWithValue("@EmployeeID", ddEmployee.SelectedValue);
        command.Parameters.AddWithValue("@VoucherNo", hdnGRNInvoiceNo.Value);
        command.Parameters.AddWithValue("@EsclationDay", txtEsclationDay.Text);
        command.Parameters.AddWithValue("@Priority", ddlPriority.SelectedValue);
        command.Parameters.AddWithValue("@Remark", txtRemark.Text);
        command.Parameters.AddWithValue("@EntryBy", lName);
        command.Parameters.AddWithValue("@EntryDate", DateTime.Now);
        command.Connection.Open();
        command.ExecuteNonQuery();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    protected void WorkFlowUserGridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(WorkFlowUserGridView.SelectedIndex);
            Label label = WorkFlowUserGridView.Rows[index].FindControl("lblWorkFlowUserID") as Label;

            hdnWorkFlowUserId.Value = label.Text;
            string query = @"SELECT WorkFlowUserID,DesignationId,EmployeeID, Priority, convert(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, convert(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, Remark FROM WorkFlowUser WHERE WorkFlowUserID = '" + hdnWorkFlowUserId.Value + "'";
            SqlCommand command = new SqlCommand(query, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            command.Connection.Open();
            SqlDataReader dataReader = command.ExecuteReader();
            if (dataReader.Read())
            {
                btnWorkFlowSave.Text = "Update";
                BindddlDesignation();
                ddlDesignation.SelectedValue = dataReader["DesignationId"].ToString();
                BindddEmployee();
                ddEmployee.SelectedValue = dataReader["EmployeeID"].ToString();
                ddlPriority.SelectedValue = dataReader["Priority"].ToString();
                txtRemark.Text = dataReader["Remark"].ToString();
            }
            Notify("Edit mode activated ...", "info", lblMsg);
            dataReader.Close();
            command.Connection.Close();
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "info", lblMsg);
        }

    }
    protected void WorkFlowUserGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name;
            //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
            //{
            int index = Convert.ToInt32(e.RowIndex);
            Label lblId = WorkFlowUserGridView.Rows[index].FindControl("lblWorkFlowUserID") as Label;
            SQLQuery.ExecNonQry(" Delete WorkFlowUser FROM WorkFlowUser WHERE WorkFlowUserID='" + lblId.Text + "' ");
            BindWorkFlowUserGridView();
            VisibleWorkflowDateAndDay();
            Notify("Successfully Deleted...", "success", lblMsg);
            //}
            //else
            //{
            //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            //}
        }
        catch (Exception ex)
        {

            Notify("ERROR" + ex, "error", lblMsg);
        }

    }

    private void BindWorkFlowUserGridView()
    {
        string lName = Page.User.Identity.Name.ToString();
        string query="";
        if (User.IsInRole("Super Admin") && hdnGrnId.Value != "")
        {
            if (btnSave.Visible)
            {
                query = "AND WorkFlowTypeID='" + hdnGrnId.Value + "'";
            }
            else
            {
                query = "AND WorkFlowTypeID='" + hdnGrnId.Value + "' AND EntryBy = '" + lName + "'";
            }
            
        }
        else
        {
            query = "AND WorkFlowTypeID='" + hdnGrnId.Value + "' AND EntryBy = '" + lName + "'";
        }
        string sql = @"SELECT WorkFlowUser.WorkFlowUserID, WorkFlowUser.Priority, WorkFlowUser.EsclationDay, WorkflowUserSequence.SequenceBan + ' (' + CONVERT(varchar, WorkFlowUser.Priority) + ')' AS SequenceBan, 
                  Employee.Name + ', ' + Designation.Name AS EmployeeName, CONVERT(varchar, WorkFlowUser.EsclationStartTime, 103) AS EsclationStartTime, CONVERT(varchar, WorkFlowUser.EsclationEndTime, 103) AS EsclationEndTime, 
                  WorkFlowUser.Remark FROM WorkFlowUser INNER JOIN DesignationWithEmployee ON WorkFlowUser.EmployeeID = DesignationWithEmployee.Id INNER JOIN
                  Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID INNER JOIN WorkflowUserSequence ON WorkFlowUser.Priority = WorkflowUserSequence.Priority AND WorkFlowUser.WorkFlowType = WorkflowUserSequence.Type INNER JOIN
                  Designation ON DesignationWithEmployee.DesignationID = Designation.DesignationID WHERE  WorkFlowUser.WorkFlowType = 'GRN' "+ query + " Order By Priority ASC";

        SqlCommand command = new SqlCommand(sql, new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        command.Connection.Open();
        WorkFlowUserGridView.EmptyDataText = "No data added ...";
        WorkFlowUserGridView.DataSource = command.ExecuteReader();
        WorkFlowUserGridView.DataBind();
        command.Connection.Close();
        command.Connection.Dispose();
    }
    protected void productAddGridView_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string lName = Page.User.Identity.Name;
        //if (SQLQuery.OparatePermission(lName, "Delete") == "1")
        //{
        int index = Convert.ToInt32(e.RowIndex);
        Label lblId = productAddGridView.Rows[index].FindControl("lblGRNProductID") as Label;
        SQLQuery.ExecNonQry(" Delete GRNProduct WHERE GRNProductID='" + lblId.Text + "' ");
        BindproductAddGridView();
        Notify("Successfully Deleted...", "success", lblMsg);
        //}
        //else
        //{
        //    Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
        //}
    }
    private void NotifyToEmployee(string employeeID, string grnNumber, string grnId)
    {
        string sqlQuery = @"SELECT DesignationWithEmployee.Id, Employee.EmployeeID, Employee.Name, Employee.Email
                        FROM DesignationWithEmployee INNER JOIN Employee ON DesignationWithEmployee.EmployeeID = Employee.EmployeeID
                        WHERE (DesignationWithEmployee.Id = '" + employeeID + "')";
        DataTable dt = SQLQuery.ReturnDataTable(sqlQuery);

        foreach (DataRow item in dt.Rows)
        {
            string name = item["Name"].ToString();
            string email = item["Email"].ToString();
            string emailBody = "Dear " + name +
                          ", <br><br>Approve workflow, check your notification .<br><br>";

            emailBody += " <br><br>Regards, <br><br>Development Team.";

            SQLQuery.ExecNonQry("UPDATE GRNFrom SET CurrentWorkflowUser='" + name + "' WHERE IDGrnNO = '" + grnId + "'");
            SQLQuery.SendEmail2(email, "btvstoremanagementsystem@gmail.com", "Workflow for #" + grnNumber, emailBody);

        }
    }
    protected void btnDraft_OnClick(object sender, EventArgs e)
    {
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            if (txtRemarks.Text != "")
            {
                if (btnDraft.Text.ToUpper() == "SAVE AS DRAFT")
                {
                    if (SQLQuery.OparatePermission(lName, "Insert") == "1")
                    {
                        if (productAddGridView.Rows.Count > 0)
                        {
                            if (WorkFlowUserGridView.Rows.Count > 0)
                            {
                                if (VerifyPrioritySequence())
                                {
                                    string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfGRN.Text));
                                    string locationId = SQLQuery.GetLocationID(User.Identity.Name);
                                    string isExist = SQLQuery.ReturnString("SELECT GRNInvoiceNo FROM GRNFrom WHERE GRNInvoiceNo='" + txtGrnNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "' AND StoreID='" + ddStore.SelectedValue + "'");
                                    if (isExist == "")
                                    {
                                        if (document.HasFile)
                                        {
                                            string tExt = Path.GetExtension(document.FileName);
                                            if (tExt != ".pdf")
                                            {
                                                Notify("Please upload the pdf file!", "warning", lblMsg);
                                                return;
                                            }
                                        }
                                        SaveGrnData("Drafted");
                                        ClearControls();
                                        Notify("Successfully Save as Draft...", "success", lblMsg);
                                    }
                                    else
                                    {
                                        Notify("GRN Number already exist.", "error", lblMsg);
                                    }
                                }
                                else
                                {
                                    Notify("Please check workflow priority sequence!", "warn", lblMsg);
                                }
                                
                            }
                            else
                            {
                                Notify("Please add workflow details", "warn", lblMsg);
                            }

                        }
                        else
                        {
                            Notify("Please add product details", "warn", lblMsg);
                        }

                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
                else
                {
                    if (SQLQuery.OparatePermission(lName, "Update") == "1")
                    {
                        if (VerifyPrioritySequence())
                        {
                            if (document.HasFile)
                            {
                                string tExt = Path.GetExtension(document.FileName);
                                if (tExt != ".pdf")
                                {
                                    Notify("Please upload the pdf file!", "warning", lblMsg);
                                    return;
                                }
                            }
                            UpdateGrnData("Drafted");
                            ClearControls();
                            btnDraft.Text = "SAVE AS DRAFT";
                            Notify("Successfully Updated as Draft...", "success", lblMsg);
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            Notify("Please check workflow priority sequence!", "warn", lblMsg);
                        }
                        
                    }
                    else
                    {
                        Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
                    }
                }
            }
            else
            {
                Notify("Remarks field can't be empty!", "warn", lblMsg);
            }

        }
        catch (Exception ex)
        {
            Notify(ex.ToString(), "error", lblMsg);
        }
        finally
        {
            hdnGRNInvoiceNo.Value = "";
            hdnGrnId.Value = "";
            hdnProductId.Value = "";
            hdnWorkFlowUserId.Value = "";
            BindproductAddGridView();
            BindWorkFlowUserGridView();
            BindGrnGridView();
            VisibleWorkflowDateAndDay();
            txtGrnNo.Text = GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
        }
    }
    private bool PriorityCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool priorityStatus = true;
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority, EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'GRN'");

        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {
            string escDate = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND WorkFlowType = 'GRN'");
            if (priorityDataRow["Priority"].ToString() == ddlPriority.SelectedValue)
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
        DataTable priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'GRN' ");
        if (btnSave.Visible)
        {
            priorityDataTable = SQLQuery.ReturnDataTable(@"SELECT Priority,EmployeeID FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND WorkFlowType = 'GRN' ");
        }
        foreach (DataRow priorityDataRow in priorityDataTable.Rows)
        {

            if (ddEmployee.SelectedValue == priorityDataRow["EmployeeID"].ToString())
            {
                string priority = "0";
                if (btnSave.Visible)
                {
                    priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'GRN'");
                }
                else
                {
                    priority = SQLQuery.ReturnString("SELECT IsNull(Count(Priority),0) FROM WorkFlowUser WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND Priority='" + ddlPriority.SelectedValue + "' AND EmployeeID <>'" + priorityDataRow["EmployeeID"] + "' AND WorkFlowType = 'GRN' AND EntryBy = '" + lName + "'");
                }
                if (int.Parse(priority) > 0)
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

    protected void btnWorkFlowSave_Click(object sender, EventArgs e)
    {
        string lName = Page.User.Identity.Name;

        string isUserExists = SQLQuery.ReturnString("SELECT EmployeeID FROM WorkFlowUser WHERE DesignationId='" + ddlDesignation.SelectedValue + "' AND EmployeeID = '" + ddEmployee.SelectedValue + "'AND WorkFlowTypeID ='" + hdnGrnId.Value + "' AND WorkFlowType = 'GRN' AND EntryBy = '" + lName + "'");
        if (btnWorkFlowSave.Text.ToUpper() == "ADD USER")
        {
            //if (isUserExists != ddEmployee.SelectedValue)
            //{
            if (PrioritySequenceCheck())
            {
                if (PriorityCheck())
                {
                    InsertToWorkFlowUser();
                    BindWorkFlowUserGridView();
                    VisibleWorkflowDateAndDay();
                    Notify("Insert Successful", "info", lblMsg);
                }
                else
                {
                    Notify("Already you have assigned this priority!", "warn", lblMsg);
                }
            }
            else
            {
                Notify("Please check workflow priority sequence!", "warn", lblMsg);
            }
            


            //}
            //else
            //{
            //    Notify("This employee is already added!", "warn", lblMsg);
            //}
        }
        else
        {
            if (SQLQuery.OparatePermission(lName, "Update") == "1")
            {
                if (PriorityCheckForUpdate())
                {
                    UpdateWorkFlowUser();
                    BindWorkFlowUserGridView();
                    VisibleWorkflowDateAndDay();
                    btnWorkFlowSave.Text = "ADD USER";
                    txtRemark.Text = "";
                    Notify("Update Successful", "info", lblMsg);
                }
                else
                {
                    Notify("Already you have assigned this priority or date!", "warn", lblMsg);
                }



            }
            else
            {
                Notify("You are not eligible to attempt this operation!", "warn", lblMsg);
            }

        }


    }

    private bool PrioritySequenceCheck()
    {
        string lName = Page.User.Identity.Name.ToString();
        bool sequentStatus = false;
        int priority = int.Parse(SQLQuery.ReturnString(@"SELECT ISNULL(MAX(Priority),0) FROM WorkFlowUser WITH(NOLOCK) WHERE WorkFlowTypeID = '" + hdnGrnId.Value + "' AND EntryBy='" + lName + "' AND WorkFlowType = 'GRN'"));
        if (priority > 0)
        {
            if ((priority + 1) == int.Parse(ddlPriority.SelectedValue))
            {
                sequentStatus = true;
            }
        }
        else
        {
            if ("1" == ddlPriority.SelectedValue)
            {
                sequentStatus = true;
            }
        }

        return sequentStatus;
    }
    protected void ddlDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindddEmployee();
    }

    protected void txtDateOfGRN_TextChanged(object sender, EventArgs e)
    {
        txtGrnNo.Text = GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
        if (!CheckDate())
        {
            Notify("Date of Purchase Order No can not be later than the Date of Invoice No and Product Receive Date.", "warn", lblMsg);
        }
    }

    protected void txtGrnNo_TextChanged(object sender, EventArgs e)
    {
        if (txtGrnNo.Text.Length == 16)
        {
            string finYear = GenerateVoucherNumber.GetFinYear(Convert.ToDateTime(txtDateOfGRN.Text));
            string locationId = SQLQuery.GetLocationID(User.Identity.Name);
            string isExist = SQLQuery.ReturnString("SELECT GRNInvoiceNo FROM GRNFrom WHERE GRNInvoiceNo='" + txtGrnNo.Text.Trim() + "' AND LocationID='" + locationId + "' AND FinYear='" + finYear + "'");
            if (isExist != "")
            {
                Notify("This " + txtGrnNo.Text + " GRN Number already exist.", "warn", lblMsg);
            }
        }
        else
        {
            Notify("GRN Number should be 16 characters", "warn", lblMsg);
        }
        //if (!CheckDate())
        //{
        //    Notify("Date of Purchase Order No can not be later than the Date of Invoice No and Product Receive Date.", "warn", lblMsg);
        //}

    }

    protected void GrnGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GrnGridView.PageIndex = e.NewPageIndex;
        BindGrnGridView();
    }

    protected void txtDateofInvoiceNo_TextChanged(object sender, EventArgs e)
    {
        if (!CheckDate())
        {
            Notify("Date of Purchase Order No can not be later than the Date of Invoice No and Product Receive Date.", "warn", lblMsg);
        }
    }

    protected void txtDateofPurchaseOrderNo_TextChanged(object sender, EventArgs e)
    {
        if (!CheckDate())
        {
            Notify("Date of Purchase Order No can not be later than the Date of Invoice No and Product Receive Date.", "warn", lblMsg);
        }
    }

    protected void txtProductSHReceiveDate_TextChanged(object sender, EventArgs e)
    {
        if (!CheckDate())
        {
            Notify("Date of Purchase Order No can not be later than the Date of Invoice No and Product Receive Date.", "warn", lblMsg);
        }
    }
    protected void ddStore_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtGrnNo.Text = ddReferenceID.SelectedItem.Text == "Existing" ? GenerateVoucherNumber.GetOldGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue) : GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
        BindGrnGridView();
        //txtGrnNo.Text = GenerateVoucherNumber.GetGrnNumber(Convert.ToDateTime(txtDateOfGRN.Text), User.Identity.Name, ddStore.SelectedValue);
    }
}
