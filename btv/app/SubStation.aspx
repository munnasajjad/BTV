<%@ Page Title="Sub Station" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="SubStation.aspx.cs" Inherits="app_SubStation" %>

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
                        <legend>Sub Station</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="hiddenSSId" />
                                    <asp:HiddenField runat="server" ID="hiddenSSVoucher" />
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
                                <td>SS Number</td>
                                <td>
                                    <asp:TextBox ID="txtSSNumber" ReadOnly="True" runat="server" CssClass="form-control"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td>PFI<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddlPFI" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtPFI" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Condition</td>
                                <td>
                                    <asp:DropDownList ID="ddlCondition" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>Transformer</td>
                                <td>
                                    <asp:DropDownList ID="ddlTransformer" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>HT Switch Gear</td>
                                <td>
                                    <asp:DropDownList ID="ddlHTSwitchGear" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>HT Battery Changer</td>
                                <td>
                                    <asp:DropDownList ID="ddlHTBatteryChanger" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td>HT Ampier X-Former</td>
                                <td>
                                    <asp:DropDownList ID="ddlHtAmpier" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>LT Panel</td>
                                <td>
                                    <asp:DropDownList ID="ddlLtPanel" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td>Generator Battery Condition</td>
                                <td>
                                    <asp:DropDownList ID="ddlGeneratorBatteryCondition" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>OK</asp:ListItem>
                                        <asp:ListItem>Not OK</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Load Shedding Start Time<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtLoadSheddingStartTime" TextMode="Time" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGeneratorStartTime" ValidationGroup="Save" runat="server" ErrorMessage="Enter Generator Start Time"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Load Shedding Stop Time<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtLoadSheddingStopTime" TextMode="Time" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGeneratorStopTime" ValidationGroup="Save" runat="server" ErrorMessage="Enter Generator Stop Time"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>Generator Start Time<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtGeneratorStartTime" TextMode="Time" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGeneratorStartTime" ValidationGroup="Save" runat="server" ErrorMessage="Enter Generator Start Time"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Generator Stop Time<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtGeneratorStopTime" TextMode="Time" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtGeneratorStopTime" ValidationGroup="Save" runat="server" ErrorMessage="Enter Generator Stop Time"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Fuel Stockin Litre<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtFuelStockinLitre" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFuelStockinLitre" ValidationGroup="Save" runat="server" ErrorMessage="Enter Fuel Stockin Litre"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Fuel Consumption In litre<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtFuelConsumptionInlitre" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFuelConsumptionInlitre" ValidationGroup="Save" runat="server" ErrorMessage="Enter Fuel Consumption In litre"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Running Hour<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRunningHour" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRunningHour" ValidationGroup="Save" runat="server" ErrorMessage="Enter Running Hour"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Attendent By<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtAttendentBy" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAttendentBy" ValidationGroup="Save" runat="server" ErrorMessage="Enter Attendent By"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Maintenance<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtMaintenance" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMaintenance" ValidationGroup="Save" runat="server" ErrorMessage="Enter Maintenance"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <%--<tr>
                                <td>Shift Incharge<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtShiftInCharge" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShiftInCharge" ValidationGroup="Save" runat="server" ErrorMessage="Enter Shift Incharge"></asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>
                            <tr>
                                <td>Remarks<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ValidationGroup="Save" runat="server" ErrorMessage="Enter Remarks"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Shift Incharge<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddShiftInCharge" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddShiftInCharge" InitialValue="0" ValidationGroup="Save" runat="server" ErrorMessage="Enter Shift Incharge"></asp:RequiredFieldValidator>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtWorkflowRemarks" ValidationGroup="User" runat="server" ErrorMessage="Enter remarks"></asp:RequiredFieldValidator>
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
                            <asp:GridView Width="400%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="SubStationID" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
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
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("SubStationID") %>'></asp:Label>
                                            <asp:Label ID="lblEntryBy" runat="server" Visible="false" Text='<%# Bind("EntryBy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:TemplateField HeaderText="ES Number" SortExpression="SSVoucher">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Width="120px" Target="_blank" NavigateUrl='<%#Eval("SubStationID") %>'>
                                                <asp:Label ID="lblSSNumber" runat="server" Text='<%# Bind("SSVoucher") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workflow Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink" Width="110px" runat="server" Target="_blank" NavigateUrl='<%# "WorkflowStatusForSubStation?id="+Eval("SubStationID") %>'>
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
                                    <asp:BoundField DataField="PFI" HeaderText="P F I" SortExpression="PFI" />
                                    <asp:BoundField DataField="Condition" HeaderText="Condition" SortExpression="Condition" />
                                    <asp:BoundField DataField="Transformer" HeaderText="Transformer" SortExpression="Transformer" />
                                    <asp:BoundField DataField="HTSwitchGear" HeaderText="H T Switch Gear" SortExpression="HTSwitchGear" />
                                    <asp:BoundField DataField="HTBatteryChanger" HeaderText="H T Battery Changer" SortExpression="HTBatteryChanger" />
                                    <asp:BoundField DataField="HTAmpierXFormer" HeaderText="H T Ampier X Former" SortExpression="HTAmpierXFormer" />
                                    <asp:BoundField DataField="LTPanel" HeaderText="L T Panel" SortExpression="LTPanel" />
                                    <asp:BoundField DataField="GeneratorBatteryCondition" HeaderText="Generator Battery Condition" SortExpression="GeneratorBatteryCondition" />
                                    <asp:BoundField DataField="LoadSheddingStartTime" HeaderText="Load Shedding Start Time" SortExpression="LoadSheddingStartTime" />
                                    <asp:BoundField DataField="LoadSheddingStopTime" HeaderText="Load Shedding Stop Time" SortExpression="LoadSheddingStopTime" />
                                    <asp:BoundField DataField="GeneratorStartTime" HeaderText="Generator Start Time" SortExpression="GeneratorStartTime" />
                                    <asp:BoundField DataField="GeneratorStopTime" HeaderText="Generator Stop Time" SortExpression="GeneratorStopTime" />
                                    <asp:BoundField DataField="FuelStockinLitre" HeaderText="Fuel Stockin Litre" SortExpression="FuelStockinLitre" />
                                    <asp:BoundField DataField="FuelConsumptionInlitre" HeaderText="Fuel Consumption Inlitre" SortExpression="FuelConsumptionInlitre" />
                                    <asp:BoundField DataField="RunningHour" HeaderText="Running Hour" SortExpression="RunningHour" />
                                    <asp:BoundField DataField="AttendentBy" HeaderText="Attendent By" SortExpression="AttendentBy" />
                                    <asp:BoundField DataField="Maintenance" HeaderText="Maintenance" SortExpression="Maintenance" />
                                    <asp:BoundField DataField="ShiftInCharge" HeaderText="Shift Incharge" SortExpression="ShiftInCharge" />
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





