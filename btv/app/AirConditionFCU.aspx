<%@ Page Title="Air Condition F C U" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="AirConditionFCU.aspx.cs" Inherits="app_AirConditionFCU" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        h5.heading {
            margin-bottom: 5px;
            background: #ed2024;
            color: white;
            padding: 4px;
            font-size: 17px;
            margin-left: -300px;
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
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="col-lg-6">
                <section class="panel">

                    <fieldset>
                        <legend>Air Condition FCU</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="hiddenACFID" />
                                    <asp:HiddenField runat="server" ID="hiddenACFVoucher" />
                                    <asp:HiddenField runat="server" ID="hiddenWorkFlowUserID" />
                                </td>
                            </tr>


                            <tr>
                                <td>Date<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDate" ValidationGroup="Save" runat="server" ErrorMessage="Enter date"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>ACF Number</td>
                                <td>
                                    <asp:TextBox ID="txtACFNumber" ReadOnly="True" runat="server" CssClass="form-control"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td>FCU Number<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtFCUNumber" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFCUNumber" ValidationGroup="Save" runat="server" ErrorMessage="Enter FCU Number"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Type<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtType" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtType" ValidationGroup="Save" runat="server" ErrorMessage="Enter Type"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>Cooling Area<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCoolingArea" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCoolingArea" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cooling Area"></asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>Main Office<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddLocationID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="false" OnSelectedIndexChanged="ddLocationID_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="0" ControlToValidate="ddLocationID" ValidationGroup="Save" runat="server" ErrorMessage="Select Main Office"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Cooling Area<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddLocationInMainOffice" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="false" Width="100%">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" InitialValue="0" ControlToValidate="ddLocationInMainOffice" ValidationGroup="Save" runat="server" ErrorMessage="Select Location In Main Office"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>Filter<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlFilter" runat="server" Width="100%" CssClass="form-control selec2me">
                                        <asp:ListItem>Cleaned</asp:ListItem>
                                        <asp:ListItem>Replaced</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtFilter" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Drain<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlDrain" runat="server" Width="100%" CssClass="form-control selec2me">
                                        <asp:ListItem>Cleaned</asp:ListItem>
                                        <asp:ListItem>Replaced</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtDrain" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Fins<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlFins" runat="server" Width="100%" CssClass="form-control selec2me">
                                        <asp:ListItem>Cleaned</asp:ListItem>
                                        <asp:ListItem>Replaced</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtFins" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Strainer<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlStrainer" runat="server" Width="100%" CssClass="form-control selec2me">
                                        <asp:ListItem>Cleaned</asp:ListItem>
                                        <asp:ListItem>Replaced</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtStrainer" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>Shift Incharge<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtShiftIncharge" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShiftIncharge" ValidationGroup="Save" runat="server" ErrorMessage="Enter Shift Incharge"></asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>Remarks<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" Rows="2" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ValidationGroup="Save" runat="server" ErrorMessage="Enter Remarks"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Shift Incharge<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddShiftInCharge" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddShiftInCharge" InitialValue="0" ValidationGroup="Save" runat="server" ErrorMessage="Enter Shift Incharge"></asp:RequiredFieldValidator>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEsclationDay" ValidationGroup="User" runat="server" ErrorMessage="Enter Esclation Day"></asp:RequiredFieldValidator>

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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtWorkflowRemarks" ValidationGroup="User" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>
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
                                                    <%--<asp:TemplateField HeaderText="E.End Time" SortExpression="EsclationEndTime">
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


                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <br />
                                    <asp:Button ID="btnSave" CssClass="btn btn-primary" ValidationGroup="Save" runat="server" Text="SUBMIT" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnDraft" CssClass="btn btn-success" ValidationGroup="Save" runat="server" Text="SAVE AS DRAFT" OnClick="btnDraft_OnClick" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-white" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
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
                            <asp:GridView Width="240%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="AirConditionFcuId" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
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
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("AirConditionFcuId") %>'></asp:Label>
                                            <asp:Label ID="lblEntryBy" runat="server" Visible="false" Text='<%# Bind("EntryBy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:TemplateField HeaderText="ACF Number" SortExpression="ACFVoucher">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Width="120px" Target="_blank" NavigateUrl='<%#Eval("AirConditionFcuId") %>'>
                                                <asp:Label ID="lblACFNumber" runat="server" Text='<%# Bind("ACFVoucher") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workflow Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink" Width="110px" runat="server" Target="_blank" NavigateUrl='<%# "WorkflowStatusForAirConditionFCU?id="+Eval("AirConditionFcuId") %>'>
                                                <asp:Label ID="lblDetails" runat="server" Text="Workflow Details"></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Save Mode" SortExpression="SaveMode">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSaveMode" runat="server" Text='<%# Bind("SaveMode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Current Workflow User " SortExpression="CurrentWorkflowUser">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrentWorkflowUser" runat="server" Text='<%# Bind("CurrentWorkflowUser") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FCUNumber" HeaderText="F C U Number" SortExpression="FCUNumber" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                    <asp:BoundField DataField="CoolingArea" HeaderText="Cooling Area" SortExpression="CoolingArea" />
                                    <asp:BoundField DataField="Filter" HeaderText="Filter" SortExpression="Filter" />
                                    <asp:BoundField DataField="Drain" HeaderText="Drain" SortExpression="Drain" />
                                    <asp:BoundField DataField="Fins" HeaderText="Fins" SortExpression="Fins" />
                                    <asp:BoundField DataField="Strainer" HeaderText="Strainer" SortExpression="Strainer" />
                                    <asp:BoundField DataField="ShiftIncharge" HeaderText="Shift Incharge" SortExpression="ShiftIncharge" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                    <asp:BoundField DataField="EntryBy" HeaderText="Entry By" SortExpression="EntryBy" />
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





