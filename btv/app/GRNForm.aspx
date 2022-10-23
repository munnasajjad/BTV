<%@ Page Title="GRN Form" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="GRNForm.aspx.cs" Inherits="app_GRNForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
 
    <style>
        h5.heading {
            margin-bottom: 5px;
            background: #ed2024;
            color: white;
            padding: 4px;
            font-size: 17px;
            margin-left: -220px;
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        
    </script>
    <script type="text/javascript">
       
        function calPrice() {

            var receive = $('#<%=txtReceiveProduct.ClientID%>').val();
            var unitPrice = $('#<%=txtUnitPrice.ClientID%>').val();
            var totalPrice = (parseFloat(receive) * parseFloat(unitPrice));
            $('#<%=txtTotalPrice.ClientID%>').val(totalPrice.toString());
            var otherCost = 0;
            if ($('#<%=txtOthersCost.ClientID%>').val() != "") {
                otherCost = $('#<%=txtOthersCost.ClientID%>').val();
            }
            var totalCost = parseFloat(otherCost) + parseFloat(totalPrice);

            $('#<%=txtTotalCost.ClientID%>').val(totalCost.toString());

        }
    </script>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDraft" />
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            <script type="text/javascript">
                Sys.Application.add_load(callJquery);
            </script>
            <div class="col-lg-6">
                <section class="panel">
                    <fieldset>
                        <legend>GRN Form</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                  <asp:HiddenField runat="server" ID="hdnWorkFlowUserId" />
                                    <asp:HiddenField runat="server" ID="hdnProductId" />
                                    <asp:HiddenField runat="server" ID="hdnGrnId" Value="0"/>
                                    <asp:HiddenField runat="server" ID="hdnGRNInvoiceNo" />
                                     <asp:HiddenField runat="server" ID="hdnDocumentUrl" />
                                </td>
                            </tr>
                             <tr>
                                <td>Store Name</td>
                                <td>
                                    <asp:DropDownList ID="ddStore" runat="server" Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddStore_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Reference</td>
                                <td>
                                    <asp:DropDownList ID="ddReferenceID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddReferenceID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Date Of GRN<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDateOfGRN" AutoPostBack="true" OnTextChanged="txtDateOfGRN_TextChanged" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateOfGRN" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateOfGRN" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDateOfGRN" ValidationGroup="Save" runat="server" ErrorMessage="Enter GRN date"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>GRN No<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtGrnNo" runat="server" OnTextChanged="txtGrnNo_TextChanged" AutoPostBack="true" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGrnNo" ValidationGroup="Save" runat="server" ErrorMessage="Enter GRN No"></asp:RequiredFieldValidator>

                                  
                                </td>
                            </tr>



                            
                            <tr>
                                <td>Cash Memo/Invoice No</td>
                                <td>
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtInvoiceNo" ValidationGroup="Save" runat="server" ErrorMessage="Enter invoice no"></asp:RequiredFieldValidator>--%>

                                </td>
                            </tr>

                            <tr>
                                <td>Supplier/Source Name<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtSupplier" runat="server" CssClass="form-control"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSupplier" ValidationGroup="Save" runat="server" ErrorMessage="Enter supplier"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>Date of Cash Memo/Invoice No</td>
                                <td>
                                    <asp:TextBox ID="txtDateofInvoiceNo" AutoPostBack="true" runat="server" OnTextChanged="txtDateofInvoiceNo_TextChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateofInvoiceNo" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateofInvoiceNo" />
                                </td>
                            </tr>


                            <tr>
                                <td>Work Order/Purchase Order No</td>
                                <td>
                                    <asp:TextBox ID="txtPurchaseOrderNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:FilteredTextBoxExtender ID="ftbPurchaseOrderNo" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtPurchaseOrderNo" />--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Date of Work Order/Purchase Order No<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDateofPurchaseOrderNo" AutoPostBack="true" OnTextChanged="txtDateofPurchaseOrderNo_TextChanged" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateofPurchaseOrderNo" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateofPurchaseOrderNo" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDateofPurchaseOrderNo" ValidationGroup="Save" runat="server" ErrorMessage="Enter date of purchase order no"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Product Receive Date<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtProductSHReceiveDate" runat="server" AutoPostBack="true" OnTextChanged="txtProductSHReceiveDate_TextChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceProductSHReceiveDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtProductSHReceiveDate" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductSHReceiveDate" ValidationGroup="Save" runat="server" ErrorMessage="Enter product SH receive date"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr class="hhh">
                                <td></td>
                                <td>
                                    <h5 class="heading">Product Information</h5>
                                </td>
                            </tr>

                            <tr>
                                <td>Category<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddCategoryID" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddCategoryID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddCategoryID" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter category"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Sub Category<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddProductSubCategory" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddProductSubCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddProductSubCategory" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter sub category"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>Product Name<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddProductName" Width="100%" AppendDataBoundItems="true" CssClass="form-control select2me" AutoPostBack="True" OnSelectedIndexChanged="ddProductName_OnSelectedIndexChanged" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddProductName" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter product"></asp:RequiredFieldValidator>

                                </td>
                            </tr>

                            <tr>
                                <td>Unit name</td>
                                <td>
                                    <asp:TextBox ID="txtUnitName" ReadOnly="True" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>

                            </tr>
                            <tr>
                                <td>Less</td>
                                <td>
                                    <asp:TextBox ID="txtless" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="Filteredtxtless" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtless" />
                                </td>
                            </tr>



                            <tr>
                                <td>More</td>
                                <td>
                                    <asp:TextBox ID="txtMore" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredtxtMore" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtMore" />
                                </td>
                            </tr>


                            <tr>
                                <td>Receive Product<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtReceiveProduct" runat="server" onkeyup="calPrice()" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredtxtReceiveProduct" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtReceiveProduct" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReceiveProduct" ValidationGroup="Add" runat="server" ErrorMessage="Enter receive product"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Reject Product</td>
                                <td>
                                    <asp:TextBox ID="txtRejectProduct" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredtxtRejectProduct" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtRejectProduct" />
                                </td>
                            </tr>


                            <tr>
                                <td>Price Letter No</td>
                                <td>
                                    <asp:TextBox ID="txtPriceLetterNo" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbPriceLetterNo" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtPriceLetterNo" />
                                </td>
                            </tr>

                            <tr>
                                <td>Currency<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddCurrencyID" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddCurrencyID" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter Currency"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>Unit Price<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtUnitPrice" runat="server" onkeyup="calPrice()" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbUnitPrice" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtUnitPrice" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtUnitPrice" ValidationGroup="Add" runat="server" ErrorMessage="Enter unit price"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Total Price</td>
                                <td>
                                    <asp:TextBox ID="txtTotalPrice" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbTotalPrice" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtTotalPrice" />

                                </td>
                            </tr>


                            <tr>
                                <td>Others Cost</td>
                                <td>
                                    <asp:TextBox ID="txtOthersCost" runat="server" Text="0" onkeyup="calPrice()" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbOthersCost" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtOthersCost" />
                                </td>
                            </tr>


                            <tr>
                                <td>Total Cost</td>
                                <td>
                                    <asp:TextBox ID="txtTotalCost" runat="server" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbTotalCost" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtTotalCost" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnAdd" CssClass="btn btn-s-md btn-primary" runat="server" ValidationGroup="Add" Text="Add Product" OnClick="btnAdd_OnClick" />
                                </td>
                            </tr>

                            <tr>

                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" Width="100%" DataKeyNames="GRNProductID" ID="productAddGridView" OnRowDeleting="productAddGridView_OnRowDeleting" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical" OnSelectedIndexChanged="productAddGridView_OnSelectedIndexChanged">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>

                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sl." SortExpression="CrID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGRNProductID" runat="server" Text='<%# Bind("GRNProductID") %>'></asp:Label>
                                                            <asp:Label ID="lblEntryBy" runat="server" Visible="false" Text='<%# Bind("EntryBy") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Receive Product" SortExpression="ReceiveProduct">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReceiveProduct" runat="server" Text='<%# Bind("ReceiveProduct") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Unit Price" SortExpression="UnitPrice">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Cost" SortExpression="TotalCost">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalCost" runat="server" Text='<%# Bind("TotalCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <%-- <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btnDelete" runat="server" Text="Delete"
                                                                CommandArgument='<%# Eval("GRNProductID") %>' OnClientClick="return confirm('Pressing OK will delete this record. Do you want to continue?')" OnCommand="btnDelete_Command"></asp:ImageButton>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="GridCellStyle" HorizontalAlign="Center" />
                                                    </asp:TemplateField>--%>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                            <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="False" OnClientClick="return confirm('Pressing OK will delete this record. Do you want to continue?')" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                                           <%-- <asp:ConfirmButtonExtender TargetControlID="ImageButton4" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton4" PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                            <asp:Panel ID="PNL" runat="server" Style="width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                                <b style="color: red">Entry will be deleted!</b><br />
                                                                Are you sure, you want to delete the item from entry list?
                                                            <br />
                                                                <br />
                                                                <div style="text-align: right;">
                                                                    <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                                    <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                                </div>
                                                            </asp:Panel>--%>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Total Amount(tk.)</td>
                                <td>
                                    <asp:TextBox ID="txtTotalAmount" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredtxtTotalAmount" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtTotalAmount" />
                                </td>
                            </tr>
                            <asp:Panel runat="server" ID="workFlowPanel">
                                <tr class="hhh">
                                    <td></td>
                                    <td>
                                        <h5 class="heading">Work flow</h5>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Designation</td>
                                    <td>
                                        <asp:DropDownList ID="ddlDesignation" Width="100%" runat="server" CssClass="form-control select2me"
                                            AutoPostBack="True" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Employee</td>
                                    <td>
                                        <asp:DropDownList ID="ddEmployee" Width="100%" runat="server" CssClass="form-control select2me"
                                            AutoPostBack="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>


                                <tr>
                                    <td>Esclation Days<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationDay" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="EsclationDay" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEsclationDay" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationDay" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation Day"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>

                                <tr>
                                    <td>Priority</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPriority" Width="100%" runat="server" CssClass="form-control select2me"
                                            AutoPostBack="False">
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Remark<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtRemark" TextMode="MultiLine" Rows="2" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemark" ValidationGroup="User" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>

                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnWorkFlowSave" CssClass="btn btn-s-md btn-primary" ValidationGroup="User" runat="server" Text="Add User" OnClick="btnWorkFlowSave_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView runat="server" Width="100%" ID="WorkFlowUserGridView" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical" OnRowDeleting="WorkFlowUserGridView_OnRowDeleting" OnSelectedIndexChanged="WorkFlowUserGridView_OnSelectedIndexChanged">
                                                    <RowStyle BackColor="#F7F7DE" />
                                                    <Columns>

                                                        <asp:TemplateField ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Sl." SortExpression="WorkFlowUserID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWorkFlowUserID" runat="server" Text='<%# Bind("WorkFlowUserID") %>'></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("SequenceBan") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Esclation Day" SortExpression="EsclationDay">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEsclationDay" runat="server" Text='<%# Bind("EsclationDay") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Remarks" SortExpression="Remark">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                                <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton4" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton4"
                                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                                    <b style="color: red">Entry will be deleted!</b><br />
                                                                    Are you sure, you want to delete the item from entry list?
                                                            <br />
                                                                    <br />
                                                                    <div style="text-align: right;">
                                                                        <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                                        <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                                    </div>
                                                                </asp:Panel>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </asp:Panel>
                           
                            <tr>
                                <td>Upload</td>
                                <td>

                                    <asp:FileUpload ID="document" runat="server" CssClass="form-control" />

                                </td>
                            </tr>
                            <tr>
                                <td>Remarks<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ValidationGroup="Save" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" Visible="False" runat="server" ValidationGroup="Save" Text="Submit" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnDraft" CssClass="btn btn-s-md btn-primary" runat="server" ValidationGroup="Save" Text="Save as Draft" OnClick="btnDraft_OnClick" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>

            <div class="col-lg-6">
                <section class="panel">
                    <fieldset>
                        <legend>Saved Data</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="600%" ID="GrnGridView" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="true" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"])%>'
                                GridLines="Vertical" DataKeyNames="IDGrnNO" OnSelectedIndexChanged="GrnGridView_OnSelectedIndexChanged" OnPageIndexChanging="GrnGridView_PageIndexChanging" OnRowDeleting="GrnGridView_OnRowDeleting">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                                            <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                PopupControlID="PNLControl" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                            <asp:Panel ID="PNLControl" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                <b style="color: red">This entry will be deleted permanently!</b><br />
                                                Are you sure you want to delete this ?<br />
                                                <br />
                                                <div style="text-align: right;">
                                                    <asp:Button ID="ButtonOk" runat="server" CssClass="btn btn-success" Text="OK" />
                                                    <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                </div>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                            <asp:Label ID="lblEntryBy" runat="server" Visible="false" Text='<%# Bind("EntryBy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="GRN Number">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" Width="120px" runat="server" Target="_blank" NavigateUrl='<%#Eval("GrnId") %>'>
                                                <asp:Label ID="lblGRNInvoiceNo" runat="server" Text='<%# Bind("GRNInvoiceNo") %>'></asp:Label>
                                                <asp:Label ID="lblIDGrnNO" runat="server" Visible="False" Text='<%# Bind("IDGrnNO") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workflow Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink" Width="100%" runat="server" Target="_blank" NavigateUrl='<%# "WorkflowStatusForGrn?id="+Eval("IDGrnNO") %>'>
                                                <asp:Label ID="lblDetails" runat="server" Text="Workflow Details"></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="DateOfGRN" HeaderText="Date Of GRN" SortExpression="DateOfGRN" />
                                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" SortExpression="TotalAmount" />
                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text="View Remarks" ToolTip='<%# Bind("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <%--<asp:BoundField DataField="Remarks" HeaderText="Remark" SortExpression="Remarks" />--%>
                                    <asp:BoundField DataField="SaveMode" HeaderText="Save Mode" SortExpression="SaveMode" />
                                    <asp:BoundField DataField="WorkFlowStatus" HeaderText="WorkFlow Status" SortExpression="WorkFlowStatus" />
                                    <asp:BoundField DataField="CurrentWorkflowUser" ItemStyle-Width="100px" HeaderText="Current Work flow User" SortExpression="CurrentWorkflowUser" />
                                    <asp:BoundField DataField="EntryBy" HeaderText="Prepared By" SortExpression="EntryBy" />
                                    <asp:TemplateField HeaderText="Document">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink2" Width="100%" runat="server" Target="_blank" NavigateUrl='<%#Eval("DocumentURL") %>'>
                                                <asp:Label ID="lblDocument" runat="server" Text='<%# Bind("DocumentURL") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                </Columns>
                                <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                <PagerStyle CssClass="Pagination" BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>

                    </fieldset>
                </section>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
