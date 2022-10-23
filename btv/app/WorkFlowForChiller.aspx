<%@ Page Title="Work Flow For Chiller" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="WorkFlowForChiller.aspx.cs" Inherits="app_WorkFlowForChiller" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .panel {
            min-height: 0px !important;
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

            <div class="row">
                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                <div class="col-lg-12">
                    <section class="panel">
                        <fieldset>
                            <div class="caption">
                                <legend>Chiller Voucher Information</legend>
                            </div>
                            <div class="portlet-body">

                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">
                                        <label>Date:</label>
                                        <asp:Label ID="txtDate" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>CL Number:</label>
                                        <asp:Label ID="txtCLNumber" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Time:</label>
                                        <asp:Label ID="txtTime" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-sm-12" style="margin-bottom: 10px">

                                    <div class="col-md-4">
                                        <label>Reading Taken By:</label>
                                        <asp:Label ID="txtReadingTakenBy" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Chiller Mode:</label>
                                        <asp:Label ID="txtChillerMode" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Active Chilled Water Setpoint(°F):</label>
                                        <asp:Label ID="txtActiveChilledWaterSetpoint" runat="server"></asp:Label>
                                    </div>

                                </div>


                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Average Line Current(%RLA):</label>
                                        <asp:Label ID="txtAverageLineCurrent" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Active Current Limit Setpoint(%RLA):</label>
                                        <asp:Label ID="txtActiveCurrentLimitSetpoint" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Evap Entering Water Temperature(°F):</label>
                                        <asp:Label ID="txtEvapEnteringWaterTemperature" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Evap Leaving Water Temperature(°F):</label>
                                        <asp:Label ID="txtEvapLeavingWaterTemperature" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Evap Sat Rfgt Temp(°F):</label>
                                        <asp:Label ID="txtEvapSatRfgtTemp" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Evap Approach Temp(Psia):</label>
                                        <asp:Label ID="txtEvapApproachTemp" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Evap Water Flow Switch Status:</label>
                                        <asp:Label ID="txtEvapWaterFlowSwitchStatus" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Expansion Valve Position(%):</label>
                                        <asp:Label ID="txtExpansionValvePosition" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Expansion Valve Position Steps:</label>
                                        <asp:Label ID="txtExpansionValvePositionSteps" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Evap Rfgt Liquidlevel(in):</label>
                                        <asp:Label ID="txtEvapRfgtLiquidlevel" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Cond Entering Water Temp(°F):</label>
                                        <asp:Label ID="txtCondEnteringWaterTemp" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Cond Leaving Water Temp(°F):</label>
                                        <asp:Label ID="txtCondLeavingWaterTemp" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Cond Sat Rfgt Temp(°F):</label>
                                        <asp:Label ID="txtCondSatRfgtTemp" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Cond Rftg Pressure(°F):</label>
                                        <asp:Label ID="txtCondRftgPressure" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Cond Approach Temp(psia):</label>
                                        <asp:Label ID="txtCondApproachTemp" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Cond Water Flow Swtich Status:</label>
                                        <asp:Label ID="txtCondWaterFlowSwtichStatus" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Compressor Starts(°F):</label>
                                        <asp:Label ID="txtCompressorStarts" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Compressor Runtime(in HR):</label>
                                        <asp:Label ID="txtCompressorRuntime" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>System Rfgt Diff Pressure(psid):</label>
                                        <asp:Label ID="txtSystemRfgtDiffPressure" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Oil Pressure(psia):</label>
                                        <asp:Label ID="txtOilPressure" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Compressor Rfgt Discharge Temp(°F):</label>
                                        <asp:Label ID="txtCompressorRfgtDischargeTemp" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>RLA L1 L2 L3(%):</label>
                                        <asp:Label ID="txtRLAL1L2L3" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Amps L1 L2 L3 (Amps):</label>
                                        <asp:Label ID="txtAmpsL1L2L3" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Volts AB BC CA (v):</label>
                                        <asp:Label ID="txtVoltsABBCCA" runat="server"></asp:Label>

                                    </div>

                                </div>

                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Shift Incharge:</label>
                                        <asp:Label ID="txtShiftInCharge" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-8">

                                        <label>Remarks:</label>
                                        <asp:Label ID="txtRemarks" runat="server"></asp:Label>

                                    </div>
                                </div>

                            </div>

                        </fieldset>
                    </section>

                    <section class="panel">
                        <fieldset>

                            <div class="caption">
                                <legend>Work Flow User</legend>
                            </div>
                            <div class="col-lg-12">
                                <%--<div class="table-responsive">--%>
                                <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-striped" ID="WorkFlowUserGridView" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                    <RowStyle BackColor="#F7F7DE" />
                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
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
                                        <asp:TemplateField HeaderText="Esclation Start Date" SortExpression="EsclationStartTime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEsclationStartTime" runat="server" Text='<%# Bind("EsclationStartTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Esclation End Date" SortExpression="EsclationEndTime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEsclationEndTime" runat="server" Text='<%# Bind("EsclationEndTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approve/Decline Date" SortExpression="ApproveDeclineDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApproveDeclineDate" runat="server" Text='<%# Bind("ApproveDeclineDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Permission Status" SortExpression="PermissionStatus">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPermissionStatus" runat="server" Text='<%# Bind("PermissionStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks" SortExpression="UserRemarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserRemarks" runat="server" Text='<%# Bind("UserRemarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>

                                </asp:GridView>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-1">
                                        <label>Remarks<span style="color: red">*</span></label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtYourRemark" runat="server" Rows="3" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Enter your remarks" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtYourRemark"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:Button ID="btnApprove" CssClass="btn btn-success" ValidationGroup="Save" runat="server" Text="Approve" OnClick="btnApprove_OnClick" />
                                        <asp:Button ID="btnHold" CssClass="btn btn-warning" runat="server" ValidationGroup="Save" Text="Hold" OnClick="btnHold_OnClick" />
                                        <asp:Button ID="btnReturn" CssClass="btn btn-warning" runat="server" ValidationGroup="Save" Text="Return" OnClick="btnReturn_Click" />
                                        <asp:Button ID="btnDecline" CssClass="btn btn-danger" runat="server" ValidationGroup="Save" Text="Decline" OnClick="btnDecline_OnClick" />
                                    </div>
                                </div>

                            </div>
                            <br />


                            <%--</div>--%>
                        </fieldset>
                    </section>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>


