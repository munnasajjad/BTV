<%@ Page Title="Transfer Voucher" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="TransferVoucher.aspx.cs" Inherits="app_TransferVoucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        h5.heading {
            margin-bottom: 5px;
            background: #ed2024;
            color: white;
            padding: 4px;
            font-size: 17px;
            margin-left: -196px;
            text-align: center;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

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
                        <legend>Transfer Voucher</legend>


                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnWorkFlowUserId" />
                                    <asp:HiddenField runat="server" ID="hdnProductId" />
                                    <asp:HiddenField runat="server" ID="hdnTVId" />
                                    <asp:HiddenField runat="server" ID="hdnTvVoucherNo" />
                                </td>
                            </tr>
                            <tr>
                                <td>Date</td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" AutoPostBack="true" OnTextChanged="txtDate_TextChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate" />
                                </td>
                            </tr>
                            <tr>
                                <td>Form Store</td>
                                <td>

                                    <asp:DropDownList ID="ddFromStoreID" Width="100%" runat="server" CssClass="form-control select2me" OnSelectedIndexChanged="ddFromStoreID_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddFromStoreID" InitialValue="0" ValidationGroup="Save" runat="server" ErrorMessage="Enter from store"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>Transfer Voucher No</td>
                                <td>
                                    <asp:TextBox ID="txtTransferVoucherNo" runat="server" AutoPostBack="true" OnTextChanged="txtTransferVoucherNo_TextChanged" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>




                            <tr>
                                <td>Requisition By</td>
                                <td>
                                    <asp:TextBox ID="txtRequisitionBy" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Main Office</td>
                                <td>
                                    <asp:DropDownList ID="ddLocationID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddLocationID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Functional Office</td>
                                <td>
                                    <asp:DropDownList ID="ddCenterID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddCenterID_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Department Section</td>
                                <td>
                                    <asp:DropDownList ID="ddDepartmentSectionID" AppendDataBoundItems="true" Width="100%" runat="server" CssClass="form-control select2me" AutoPostBack="true" OnSelectedIndexChanged="ddDepartmentSectionID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>To Store</td>
                                <td>
                                    <%--<asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>--%>

                                    <asp:DropDownList ID="ddToStoreID" Width="100%" runat="server" CssClass="form-control select2me" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddToStoreID" InitialValue="0" ValidationGroup="Save" runat="server" ErrorMessage="Enter To store"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>To Be Received By</td>
                                <td>
                                    <%--<asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>--%>

                                    <asp:DropDownList ID="ddReceivedBy" Width="100%" runat="server" CssClass="form-control select2me" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddToStoreID" InitialValue="0" ValidationGroup="Save" runat="server" ErrorMessage="Enter To store"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Requirement</td>
                                <td>
                                    <asp:TextBox ID="txtRequirement" runat="server" CssClass="form-control"></asp:TextBox>
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
                                    <asp:DropDownList ID="ddCategoryID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddCategoryID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddCategoryID" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter category"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Sub Category<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddProductSubCategory" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddProductSubCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddProductSubCategory" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter sub category"></asp:RequiredFieldValidator>

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
                                <td>Transfer Qty<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtQtyNeed" runat="server" Text="0" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbQtyNeed" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtQtyNeed" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQtyNeed" ValidationGroup="Add" runat="server" ErrorMessage="Enter Qty Need"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnAdd" CssClass="btn btn-s-md btn-primary" runat="server" ValidationGroup="Add" Text="ADD PRODUCT" OnClick="btnAdd_OnClick" />
                                </td>
                            </tr>
                            <tr>

                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" Width="100%" ID="AddItemsGridView" CssClass="table table-bordered" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical" OnSelectedIndexChanged="AddItemsGridView_SelectedIndexChanged" OnRowDeleting="AddItemsGridView_RowDeleting">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>

                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sl." SortExpression="ProductID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTvProductID" runat="server" Text='<%# Bind("TvProductID") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Transfer QTY" SortExpression="TvQty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQTY" runat="server" Text='<%# Bind("TvQty") %>'></asp:Label>
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
                                <td>
                                    <br />
                                </td>

                            </tr>

                            <asp:Panel runat="server" ID="workFlowPanel">
                                <tr class="hhh">
                                    <td></td>
                                    <td>
                                        <h5 class="heading">Workflow</h5>
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

                                <%--<tr runat="server" id="escStartDate">

                                    <td>Esclation Start Date<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationStartTime" runat="server" placeholder="dd/MM/yyyy" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEsclationStartTimeCalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEsclationStartTime" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationStartTime" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation Start Date"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>--%>
                                <tr>
                                    <td>Esclation Days<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationDay" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="EsclationDay" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEsclationDay" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationDay" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation Day"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>
                                <%-- <tr>
                                    <td>Esclation End Date<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationEndTime" runat="server" placeholder="dd/MM/yyyy" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEsclationEndTimeCalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEsclationEndTime" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationEndTime" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation End Date"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>--%>
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
                                                        <%--<asp:TemplateField HeaderText="E.Start Time" SortExpression="EsclationStartTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEsclationStartTime" runat="server" Text='<%# Bind("EsclationStartTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="E.End Time" SortExpression="EsclationEndTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEsclationEndTime" runat="server" Text='<%# Bind("EsclationEndTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>

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


                            <%--<tr>
                                <td>Save Mode</td>
                                <td>
                                    <asp:TextBox ID="txtSaveMode" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>--%>


                            <%-- <tr>
                                <td>Workflow Status</td>
                                <td>
                                    <asp:TextBox ID="txtWorkflowStatus" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>--%>


                            <tr>
                                <td>Remarks</td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="SUBMIT" OnClick="btnSave_OnClick" />
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
                            <asp:GridView Width="280%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" AllowPaging="true" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"])%>'
                                GridLines="Vertical" DataKeyNames="TvID" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
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
                                            <%#Container.DataItemIndex+1 %>.
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TV Invoice">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Width="115px" Target="_blank" NavigateUrl='<%#Eval("Url") %>'>
                                                <asp:Label ID="lblTransferVoucherNo" runat="server" Text='<%# Bind("TransferVoucherNo") %>'></asp:Label>
                                                <asp:Label ID="LblTvID" runat="server" Visible="false" Text='<%# Bind("TvID") %>'></asp:Label>
                                                <asp:Label ID="lblEntryBy" runat="server" Visible="false" Text='<%# Bind("EntryBy") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:TemplateField HeaderText="Workflow Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink" Width="110px" runat="server" Target="_blank" NavigateUrl='<%# "WorkflowStatusForTV?id="+Eval("TvID") %>'>
                                                <asp:Label ID="lblDetails" runat="server" Text="Workflow Details"></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FromStore" HeaderText="Form Store" SortExpression="FromStore" />
                                    <asp:BoundField DataField="RequsitionBy" HeaderText="Requsition By" SortExpression="RequsitionBy" />
                                    <asp:BoundField DataField="ToStore" HeaderText="To Store" SortExpression="ToStore" />
                                    <asp:BoundField DataField="ReceivedBy" HeaderText="Received By" SortExpression="ReceivedBy" />
                                    <asp:BoundField DataField="Requirment" HeaderText="Requirment" SortExpression="Requirment" />

                                    <asp:BoundField DataField="SaveMode" HeaderText="Save Mode" SortExpression="SaveMode" />
                                    <asp:BoundField DataField="WorkflowStatus" HeaderText="Workflow Status" SortExpression="WorkflowStatus" />
                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text="View Remarks" ToolTip='<%# Bind("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="EntryBy" HeaderText="Entry By" SortExpression="EntryBy" />
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





