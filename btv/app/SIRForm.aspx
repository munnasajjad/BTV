<%@ Page Title="S I R Form" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="SIRForm.aspx.cs" Inherits="AppSirForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        h5.heading {
            margin-bottom: 5px;
            background: #ed2024;
            color: white;
            padding: 4px;
            font-size: 17px;
            margin-left: -235px;
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <script type="text/javascript">
        function totalPrice() {
            var receive = $('#<%=txtQtyNeed.ClientID%>').val();
            var unitPrice = $('#<%=txtUnitPrice.ClientID%>').val();
            var totalPrice = (parseFloat(receive) * parseFloat(unitPrice));

            $('#<%=txtDeliveredQtyTotalPrice.ClientID%>').val(totalPrice.toString());


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
                        <legend>S I R Form</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnSIRID" />
                                    <asp:HiddenField runat="server" ID="hdnSIRVoucher" />
                                    <asp:HiddenField runat="server" ID="hdnProductId" />
                                    <asp:HiddenField runat="server" ID="hdnWorkFlowUserId" />
                                     <asp:HiddenField runat="server" ID="hdnDocumentUrl" />
                                </td>
                            </tr>

                            <tr>
                                <td>Date Of SIR<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDateOfSir" runat="server" AutoPostBack="true" OnTextChanged="txtDateOfSir_TextChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateOfSir" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateOfSir" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDateOfSir" ValidationGroup="Save" runat="server" ErrorMessage="Enter Date of SIR"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>Store</td>
                                <td>
                                    <asp:DropDownList ID="ddSaveToStore" runat="server" Width="100%" OnSelectedIndexChanged="ddSaveToStore_SelectedIndexChanged" CssClass="form-control select2me"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <%-- <tr>
                                <td>Given Division</td>
                                <td>
                                    <asp:TextBox ID="txtGivenDivision" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>SIR No<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtSIRVoucharNo" AutoPostBack="true" OnTextChanged="txtSIRVoucharNo_TextChanged" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSIRVoucharNo" ValidationGroup="Save" runat="server" ErrorMessage="Enter SIR Vouchar No"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <%-- <tr>
                                <td>Given Division Date<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtGivenDivisionDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceGivenDivisionDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtGivenDivisionDate" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGivenDivisionDate" ValidationGroup="Save" runat="server" ErrorMessage="Enter Given Division Date"></asp:RequiredFieldValidator>

                                </td>
                            </tr>--%>

                            <tr>
                                <td>Receiver (<asp:LinkButton Style="color: blue" ID="btnLnk" runat="server" OnClick="btnLnk_Click">New</asp:LinkButton>)</td>
                                <td>
                                    <asp:DropDownList ID="ddlEmployee" CssClass="form-control select2me" Width="100%" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Using Purpose<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtProductUseAim" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductUseAim" ValidationGroup="Save" runat="server" ErrorMessage="Enter Product Using Purpose"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Cost Of Sector</td>
                                <td>
                                    <asp:TextBox ID="txtHeadOfCost" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtHeadOfCost" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cost Of Sector"></asp:RequiredFieldValidator>--%>

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
                                <td>Product<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddProductID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ddProductID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddProductID" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter product"></asp:RequiredFieldValidator>

                                </td>
                            </tr>

                            <tr>
                                <td>Qty Need<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtQtyNeed" Enabled="false" Text="0" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbQtyNeed" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtQtyNeed" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQtyNeed" ValidationGroup="Add" runat="server" ErrorMessage="Enter Qty Need"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Qty Delivered</td>
                                <td>
                                    <asp:TextBox ID="txtQtyDelivered" Enabled="false" Text="0" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbQtyDelivered" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtQtyDelivered" />
                                </td>
                            </tr>


                            <tr>
                                <td>Qty Available</td>
                                <td>
                                    <asp:TextBox ID="txtQtyAvailable" Text="0" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbQtyAvailable" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtQtyAvailable" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Unit Price</td>
                                <td>
                                    <asp:TextBox ID="txtUnitPrice" runat="server" Text="0" onkeyup="totalPrice()" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbUnitPrice" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtUnitPrice" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Total Price<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDeliveredQtyTotalPrice" Text="0" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbDeliveredQtyTotalPrice" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtDeliveredQtyTotalPrice" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDeliveredQtyTotalPrice" ValidationGroup="Add" runat="server" ErrorMessage="Enter Total Price"></asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnAdd" CssClass="btn btn-s-md btn-primary" ValidationGroup="Add" runat="server" Text="ADD PRODUCT" OnClick="btnAdd_Click" />
                                </td>
                            </tr>
                            <tr>

                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" Width="100%" ID="AddItemsGridView" CssClass="table table-bordered" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" OnSelectedIndexChanged="AddItemsGridView_SelectedIndexChanged" OnRowDeleting="AddItemsGridView_RowDeleting" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="#SL" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sl." SortExpression="SIRProductID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSirProductID" runat="server" Text='<%# Bind("SIRProductID") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="QTY Need" SortExpression="QTYNeed">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQTYNeed" runat="server" Text='<%# Bind("QTYNeed") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QTY Delivered" SortExpression="QTYDelivered">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQTYDelivered" runat="server" Text='<%# Bind("QTYDelivered") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QTY Available" SortExpression="QTYAvailable">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQTYAvailable" runat="server" Text='<%# Bind("QTYAvailable") %>'></asp:Label>
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
                                    <td>Esclation Days<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationDay" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="EsclationDay" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEsclationDay" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationDay" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation Day"></asp:RequiredFieldValidator>

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
                                <td>Upload</td>
                                <td>

                                    <asp:FileUpload ID="document" runat="server" CssClass="form-control" />

                                </td>
                            </tr>
                            <tr>
                                <td>Remarks<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ValidationGroup="Save" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>

                                </td>
                            </tr>




                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" ValidationGroup="Save" runat="server" Text="SUBMIT" OnClick="btnSave_OnClick" />
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
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" AllowPaging="true" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"])%>' ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="IDSirNo" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleting="GridView1_OnRowDeleting">
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
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("IDSirNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SIR Voucher No" SortExpression="LvInvoiceNo">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Width="115px" Target="_blank" NavigateUrl='<%#Eval("SirId") %>'>
                                                <asp:Label ID="lblSirVoucherNo" runat="server" Text='<%# Bind("SirVoucherNo") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workflow Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink" Width="110px" runat="server" Target="_blank" NavigateUrl='<%# "WorkflowStatusForSIR?id="+Eval("IDSirNo") %>'>
                                                <asp:Label ID="lblDetails" runat="server" Text="Workflow Details"></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProductUseAim" HeaderText="Product Use Aim" SortExpression="ProductUseAim" />
                                    <asp:BoundField DataField="HeadOfCost" HeaderText="Head Of Cost" SortExpression="HeadOfCost" />
                                    <asp:TemplateField HeaderText="SaveMode" SortExpression="SaveMode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaveMode" runat="server" Text='<%# Bind("SaveMode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remarks" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRemarks" runat="server" Text="View Remarks" ToolTip='<%# Bind("Remarks") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
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





