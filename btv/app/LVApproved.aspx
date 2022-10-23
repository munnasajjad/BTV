<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LVApproved.aspx.cs" Inherits="app_LVApproved" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="col-lg-6">
                <section class="panel">

                    <fieldset>
                        <legend>Loan Voucher Approved</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="idHiddenField" />
                                    <asp:HiddenField runat="server" ID="WorkFlowIdHiddenField" />
                                    <asp:HiddenField runat="server" ID="LvIdHiddenField" />
                                </td>
                            </tr>
                            <tr>
                                <td>Loan Voucher No<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlLvVoucher" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlLvVoucher_SelectedIndexChanged">
                                    </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlLvVoucher" ValidationGroup="Save" InitialValue="0" runat="server" ErrorMessage="Select Loan Voucher No"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            
                            <tr>
                                <td>Date of Lv</td>
                                <td>
                                    <asp:TextBox ID="txtDateofLv" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:CalendarExtender ID="ceDateofLv" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateofLv" />--%>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDateofLv" ValidationGroup="Save" runat="server" ErrorMessage="Enter loan voucher date"></asp:RequiredFieldValidator>--%>
                                
                                </td>
                            </tr>
                            <tr>
                                <td>Main Office</td>
                                <td>
                                    <asp:TextBox ID="txtStation" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    <%--<asp:DropDownList ID="ddlStation" Enabled="false" Width="100%" runat="server" CssClass="form-control select2me"
                                        >
                                    </asp:DropDownList>--%>

                               
                                </td>
                            </tr>
                            <tr>
                                <td>Loan From  (Functional Office)</td>
                                <td>
                                    <asp:TextBox ID="txtCenter" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>

                                    <%--<asp:DropDownList ID="ddLoanFromCenter" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddLoanFromCenter_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="0" ControlToValidate="ddLoanFromCenter" ValidationGroup="Save" runat="server" ErrorMessage="Select center"></asp:RequiredFieldValidator>--%>

                                </td>
                            </tr>
                            <tr>
                                <td>Loan From  (Store)</td>
                                <td>
                                    <asp:TextBox ID="txtStore" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>

                                    <%--<asp:DropDownList ID="ddLoanFromStore" AppendDataBoundItems="true" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" InitialValue="0" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddLoanFromStore" ValidationGroup="Save" runat="server" ErrorMessage="Select store"></asp:RequiredFieldValidator>--%>

                                </td>
                            </tr>
                          <%--  <tr>
                                <td>Loan Vouchar No<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtLoanVoucharNo" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLoanVoucharNo" ValidationGroup="Save" runat="server" ErrorMessage="Enter Loan Vouchar No"></asp:RequiredFieldValidator>
                                
                                </td>
                            </tr>--%>
                            <tr>


                                <tr>
                                    <td>Responsible Person</td>
                                    <td>
                                        <asp:TextBox ID="txtResponsiblePerson" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>


                                <tr>
                                    <td>Requirements</td>
                                    <td>
                                        <asp:TextBox ID="txtCauseOfLoan" ReadOnly="true" runat="server" CssClass="form-control"></asp:TextBox>
                                    </td>
                                </tr>

                                <td>
                                    <h2>Product Information</h2>
                                </td>
                            </tr>

                             <%--<tr>
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
                                        AutoPostBack="False" AppendDataBoundItems="true" OnSelectedIndexChanged="ddProductID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12"  ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddProductID" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter product"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Product Description</td>
                                <td>
                                    <asp:TextBox ID="txtProductDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr>
                                <td>Qty Need<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtQtyNeed" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbQtyNeed" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtQtyNeed" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQtyNeed" ValidationGroup="Add" runat="server" ErrorMessage="Enter Qty Need"></asp:RequiredFieldValidator>
                                
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Qty Delivered</td>
                                <td>
                                    <asp:TextBox ID="txtQtyDelivered" runat="server" Text="0" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbQtyDelivered" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtQtyDelivered" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Delivered Date</td>
                                <td>
                                    <asp:TextBox ID="txtDeliveredDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDeliveredDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDeliveredDate" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Qty Return</td>
                                <td>
                                    <asp:TextBox ID="txtQtyReturn" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbQtyReturn" runat="server" FilterType="Custom, Numbers" ValidChars=".0123456789" TargetControlID="txtQtyReturn" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Product Condition</td>
                                <td>
                                    <asp:TextBox ID="txtProductCondition" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>



                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnAdd" CssClass="btn btn-s-md btn-primary" runat="server" ValidationGroup="Add" Text="ADD" OnClick="btnAdd_OnClick" />
                                </td>
                            </tr>
                            <tr>--%>

                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" CssClass="table table-bordered table-striped" Width="100%" ID="AddItemsGridView" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical" >
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="SL#" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sl." SortExpression="ProductDetailsID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLVProductID" runat="server" Text='<%# Bind("LVProductID") %>'></asp:Label>

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
                                                   <%-- <asp:TemplateField HeaderText="QTY Delivered" SortExpression="QTYDelivered">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQTYDelivered" runat="server" Text='<%# Bind("QTYDelivered") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>

                                                   <%-- <asp:TemplateField HeaderText="Delivered Date" SortExpression="DeliveredDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeliveredDate" runat="server" Text='<%# Bind("DeliveredDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>





                                                   <%-- <asp:TemplateField ShowHeader="False">
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
                                                    </asp:TemplateField>--%>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <asp:Panel runat="server" ID="workFlowPanel">
                                <tr>
                                    <td>
                                        <h1>Work Flow User</h1>
                                    </td>
                                </tr>


                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="Label2" runat="server" EnableViewState="false"></asp:Label>
                                        <asp:Label ID="Label3" Visible="false" runat="server" Text=""></asp:Label>
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

                                    <td>Esclation Start Date<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationStartTime" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEsclationStartTimeCalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEsclationStartTime" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationStartTime" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation Start Date"></asp:RequiredFieldValidator>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>Esclation End Date<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtEsclationEndTime" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEsclationEndTimeCalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="txtEsclationEndTime" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationEndTime" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation End Date"></asp:RequiredFieldValidator>
                                   
                                        </td>


                                </tr>
                                <tr>
                                    <td>Priority<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtPriority" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="ftbPriority" runat="server" FilterType="Numbers" ValidChars="123456789" TargetControlID="txtPriority" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPriority" ValidationGroup="User" runat="server" ErrorMessage="Enter priority"></asp:RequiredFieldValidator>
                                    
                                    </td>
                                </tr>


                                <tr>
                                    <td>Remark<span class="required">*</span></td>
                                    <td>
                                        <asp:TextBox ID="txtWorkflowRemarks" TextMode="MultiLine" Rows="2" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtWorkflowRemarks" ValidationGroup="User" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>

                                    </td>
                                </tr>


                                <tr class="hidden">
                                    <td>Date</td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="ceDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate" />
                                    </td>
                                </tr>






                                <tr class="hidden">
                                    <td>Task Status</td>
                                    <td>
                                        <asp:CheckBox ID="cbTaskStatus" runat="server"
                                            AutoPostBack="False"></asp:CheckBox>
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
                                                            <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("Priority") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="E.Start Time" SortExpression="EsclationStartTime">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEsclationStartTime" runat="server" Text='<%# Bind("EsclationStartTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="E.End Time" SortExpression="EsclationEndTime">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEsclationEndTime" runat="server" Text='<%# Bind("EsclationEndTime") %>'></asp:Label>
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
                            <%--<tr class="hidden">
                                <td>Verifier</td>
                                <td>
                                    <asp:TextBox ID="txtVerifier" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Verifier</td>
                                <td>

                                    <asp:DropDownList runat="server" ID="ddVerifier" Width="100%" AutoPostBack="False" CssClass="form-control" />
                                </td>
                            </tr>

                            <tr class="hidden">
                                <td>Requisition By</td>
                                <td>
                                    <asp:TextBox ID="txtRequisitionBy" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Requisition By</td>
                                <td>

                                    <asp:DropDownList runat="server" ID="ddRequisitionBy"  Width="100%" AutoPostBack="False" CssClass="form-control" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Prepared By</td>
                                <td>
                                    <asp:TextBox ID="txtPreparedBy" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Delivered By</td>
                                <td>
                                    <asp:TextBox ID="txtDeliveredBy" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Delivered By</td>
                                <td>

                                    <asp:DropDownList runat="server" ID="ddDeliveredBy"  Width="100%" AutoPostBack="False" CssClass="form-control" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Received By</td>
                                <td>
                                    <asp:TextBox ID="txtReceivedBy" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Store House Return Received By</td>
                                <td>
                                    <asp:TextBox ID="txtStoreHouseReturnReceivedBy" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Save To  (Store)</td>
                                <td>
                                    <asp:DropDownList ID="ddSaveToStore" runat="server"  Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>Remarks<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Rows="2" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ValidationGroup="Save" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>
                                
                                </td>
                            </tr>



                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" ValidationGroup="Save" runat="server" Text="Save" OnClick="btnSave_OnClick" />
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
                            <asp:GridView Width="420%" ID="SaveItemsGridView" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="IDLvNo" OnSelectedIndexChanged="SaveItemsGridView_OnSelectedIndexChanged" OnRowDeleting="SaveItemsGridView_OnRowDeleting">
                                <Columns>

                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                            <asp:Label ID="lblIDLvNo" runat="server" Visible="false" Text='<%# Bind("IDLvNo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LV No" SortExpression="LvInvoiceNo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLvInvoiceNo" runat="server" Text='<%# Bind("LvInvoiceNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date" SortExpression="DateofLv">
                                        <ItemTemplate>
                                           
                                            <asp:Label ID="lblDateofLv" runat="server" Text='<%# Bind("DateofLv") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Center Name(From)" SortExpression="CenterName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCenterName" runat="server" Text='<%# Bind("CenterName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Store Name(From)" SortExpression="StoreName">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStoreName" runat="server" Text='<%# Bind("StoreName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" Visible="false" ShowHeader="False">
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
                                </Columns>
                                <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
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


