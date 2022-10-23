<%@ Page Title="Return Vauchar" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="ReturnVaucher.aspx.cs" Inherits="app_ReturnVaucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        h5.heading {
            margin-bottom: 5px;
            background: #ed2024;
            color: white;
            padding: 4px;
            font-size: 17px;
            margin-left: -280px;
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        function totalPrice() {
            var receive = $('#<%=txtProductReceive.ClientID%>').val();
            var unitPrice = $('#<%=txtUnitPrice.ClientID%>').val();
            var totalPrice = (parseFloat(receive) * parseFloat(unitPrice));

            $('#<%=txtTotalPrice.ClientID%>').val(totalPrice.toString());


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
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="col-lg-6">
                <section class="panel">

                    <fieldset>
                        <legend>Return Vauchar</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnRVID" />
                                    <asp:HiddenField runat="server" ID="hdnRVVoucher" />
                                    <asp:HiddenField runat="server" ID="hdnProductId" />
                                    <asp:HiddenField runat="server" ID="hdnWorkFlowUserId" />
                                    <asp:HiddenField runat="server" ID="hdnMode" />
                                     <asp:HiddenField runat="server" ID="hdnDocumentUrl" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:RadioButton ID="rdLoan" runat="server" CssClass="radio-m" AutoPostBack="true" Checked="true" GroupName="voucher" Text="Loan Voucher" OnCheckedChanged="rdLoan_CheckedChanged" />
                                    <asp:RadioButton ID="rdSir" runat="server" CssClass="radio-m" AutoPostBack="true" GroupName="voucher" Text="SIR Form" OnCheckedChanged="rdLoan_CheckedChanged" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>Store</td>
                                <td>
                                    <asp:DropDownList ID="ddlStore" runat="server" OnSelectedIndexChanged="ddlStore_OnSelectedIndexChanged" Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSirLV" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddSIR_LVID" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddSIR_LVID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Date of RV<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRV" runat="server" AutoPostBack="true" OnTextChanged="txtRV_TextChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceRV" runat="server" Format="dd/MM/yyyy" TargetControlID="txtRV" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRV" ValidationGroup="Save" runat="server" ErrorMessage="Enter Date of RV"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>Return Voucher No<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtReturnVoucharNo" AutoPostBack="true" OnTextChanged="txtReturnVoucharNo_TextChanged" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReturnVoucharNo" ValidationGroup="Save" runat="server" ErrorMessage="Enter Return Vouchar No"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Return Division<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtProductReturnDivision" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductReturnDivision" ValidationGroup="Save" runat="server" ErrorMessage="Enter Product Return Division"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Return Cause</td>
                                <td>
                                    <asp:TextBox ID="txtProductReturnCause" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="hhh">
                                <td></td>
                                <td>
                                    <h5 class="heading">Product Information</h5>
                                </td>
                            </tr>
                            <tr>
                                <td>Product<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddProductID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddProductID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddProductID" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter product"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>Issue Details</td>
                                <td>
                                    <asp:TextBox ID="txtIssueDetails" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Return Quantity</td>
                                <td>
                                    <asp:TextBox ID="txtReturnQuantity" runat="server" Enabled="false" Text="0" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbReturnQuantity" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtReturnQuantity" />
                                </td>
                            </tr>
                            <tr>
                                <td>Product Status</td>
                                <td>
                                    <asp:TextBox ID="txtProductStatus" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Receive<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtProductReceive" runat="server" onkeyup="totalPrice()" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductReceive" ValidationGroup="Add" runat="server" ErrorMessage="Enter receive product"></asp:RequiredFieldValidator>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtProductReceive" />
                                </td>
                            </tr>
                            <tr>
                                <td>Unit Price</td>
                                <td>
                                    <asp:TextBox ID="txtUnitPrice" runat="server" onkeyup="totalPrice()" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbUnitPrice" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtUnitPrice" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtUnitPrice" ValidationGroup="Add" runat="server" ErrorMessage="Enter Unit Price"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Total Price</td>
                                <td>
                                    <asp:TextBox ID="txtTotalPrice" runat="server" Enabled="false" Text="0" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbTotalPrice" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtTotalPrice" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTotalPrice" ValidationGroup="Add" runat="server" ErrorMessage="Enter total price"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Deposit</td>
                                <td>
                                    <asp:TextBox ID="txtDeposit" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbDeposit" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtDeposit" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnAdd" CssClass="btn btn-s-md btn-primary" ValidationGroup="Add" runat="server" OnClick="btnAdd_Click" Text="ADD PRODUCT" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" CssClass="table table-bordered" Width="100%" ID="AddItemsGridView" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" OnSelectedIndexChanged="AddItemsGridView_SelectedIndexChanged" OnRowDeleting="AddItemsGridView_RowDeleting" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="#SL" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sl." SortExpression="ProductDetailsID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRvProductID" runat="server" Text='<%# Bind("RVProductID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Returnt Quentity" SortExpression="ReturnQTY">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQTYReturn" runat="server" Text='<%# Bind("ReturnQTY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Status" SortExpression="ProductStatus">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("ProductStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit Price" SortExpression="UnitPrice">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeliveredDate" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
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
                            <asp:Panel runat="server" ID="workFlowPanel">
                                <tr class="hhh">
                                    <td></td>
                                    <td>
                                        <h5 class="heading">Work flow</h5>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblMsgWorkflow" runat="server" EnableViewState="false"></asp:Label>
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
                                    <td>Escalation Days<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationDay" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="EsclationDay" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEsclationDay" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationDay" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation Day"></asp:RequiredFieldValidator>

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
                                        <asp:TextBox ID="txtWorkflowRemarks" TextMode="MultiLine" Rows="2" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtWorkflowRemarks" ValidationGroup="User" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:Button ID="btnWorkFlowSave" CssClass="btn btn-s-md btn-primary" ValidationGroup="User" runat="server" Text="ADD USER" OnClick="btnWorkFlowSave_OnClick" />
                                    </td>
                                </tr>

                            </asp:Panel>
                            <tr>

                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" Width="100%" ID="WorkFlowUserGridView" CssClass="table table-bordered" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical" OnRowDeleting="WorkFlowUserGridView_OnRowDeleting" OnSelectedIndexChanged="WorkFlowUserGridView_OnSelectedIndexChanged">
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
                                                    <asp:TemplateField HeaderText="SequenceBan" SortExpression="SequenceBan">
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
                            <tr>
                                <td></td>
                                <td>
                                    <br />
                                </td>
                            </tr>


                            <tr>
                                <td>Upload</td>
                                <td>

                                    <asp:FileUpload ID="document" runat="server" CssClass="form-control" />

                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <br />
                                </td>
                            </tr>


                           
                            <tr>
                                <td>Remarks<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtComments" ValidationGroup="Save" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" ValidationGroup="Save" CssClass="btn btn-s-md btn-primary" runat="server" Text="SUBMIT" OnClick="btnSave_OnClick" />
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
                            <asp:GridView Width="400%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" AllowPaging="true" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"])%>' CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="IDRvNo" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                <Columns>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                                            <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                            <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
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
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("IDRvNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="RV Number">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Width="110px" Target="_blank" NavigateUrl='<%#Eval("RvId") %>'>
                                                <asp:Label ID="lblRVInvoiceNo" runat="server" Text='<%# Bind("RvInvoiceNo") %>'></asp:Label>
                                                <asp:Label ID="lblIDRvNo" runat="server" Visible="False" Text='<%# Bind("IDRvNo") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workflow Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink" Width="110px" runat="server" Target="_blank" NavigateUrl='<%# "WorkflowStatusForRV?id="+Eval("IDRvNo") %>'>
                                                <asp:Label ID="lblDetails" runat="server" Text="Workflow Details"></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text="View Remarks" ToolTip='<%# Bind("Comments") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Save Mode" SortExpression="SaveMode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaveMode" runat="server" Text='<%# Bind("SaveMode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Work flow Status" SortExpression="WorkflowStatus">
                                        <ItemTemplate>
                                            <asp:Label ID="lblWorkflowStatus" runat="server" Text='<%# Bind("WorkflowStatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Current Workflow User " SortExpression="CurrentWorkflowUser">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrentWorkflowUser" runat="server" Text='<%# Bind("CurrentWorkflowUser") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Prepared By " SortExpression="PreparedBy">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPreparedBy" runat="server" Text='<%# Bind("PreparedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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
