<%@ Page Title="Chiller" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="Chiller.aspx.cs" Inherits="app_Chiller" %>

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
                        <legend>Chiller</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="hiddenCLID" />
                                    <asp:HiddenField runat="server" ID="hiddenCLVoucher" />
                                    <asp:HiddenField runat="server" ID="hiddenWorkFlowUserID" />
                                </td>
                            </tr>


                            <tr>
                                <td>Date</td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDate" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDate" ValidationGroup="Save" runat="server" ErrorMessage="Enter date"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>CL Number</td>
                                <td>
                                    <asp:TextBox ID="txtCLNumber" ReadOnly="True" runat="server" CssClass="form-control"></asp:TextBox>

                                </td>
                            </tr>

                            <tr>
                                <td>Reading Taken By<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtReadingTaken" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReadingTaken" ValidationGroup="Save" runat="server" ErrorMessage="Enter Reading Taken By"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <%--<tr>
                                <td>Shift Incharge<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtShiftIncharge" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShiftIncharge" ValidationGroup="Save" runat="server" ErrorMessage="Enter Shift Incharge"></asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>


                            <tr>
                                <td>Time<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtTime" TextMode="Time" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtTime" ValidationGroup="Save" runat="server" ErrorMessage="Enter Time"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Chiller Mode</td>
                                <td>
                                    <asp:DropDownList ID="ddlChillerMode" runat="server" CssClass="form-control select2me" Width="100%">
                                        <asp:ListItem>Run</asp:ListItem>
                                        <asp:ListItem>Stop</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtChillerMode" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Active Chilled Water Setpoint(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtActiveChilledWaterSetpoint" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbLoad" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtActiveChilledWaterSetpoint" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtActiveChilledWaterSetpoint" ValidationGroup="Save" runat="server" ErrorMessage="Enter Active Chilled Water Setpoint"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Average Line Current(%RLA)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtAverageLineCurrent" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtAverageLineCurrent" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAverageLineCurrent" ValidationGroup="Save" runat="server" ErrorMessage="Enter Average Line Current"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Active Current Limit Setpoint(%RLA)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtActiveCurrentLimitSetpoint" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtActiveCurrentLimitSetpoint" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtActiveCurrentLimitSetpoint" ValidationGroup="Save" runat="server" ErrorMessage="Enter Active Current Limit Set Point"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Evap Entering Water Temperature(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEvapEnteringWaterTemperature" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEvapEnteringWaterTemperature" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEvapEnteringWaterTemperature" ValidationGroup="Save" runat="server" ErrorMessage="Enter Evap Entering Water Temperature"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Evap Leaving Water Temperature(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEvapLeavingWaterTemperature" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEvapLeavingWaterTemperature" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEvapLeavingWaterTemperature" ValidationGroup="Save" runat="server" ErrorMessage="Enter Evap Leaving Water Temperature(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Evap Sat Rfgt Temp(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEvapSatRfgtTemp" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEvapSatRfgtTemp" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEvapSatRfgtTemp" ValidationGroup="Save" runat="server" ErrorMessage="Enter Evap Sat Rfgt Temp(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Evap Approach Temp(Psia)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEvapApproachTemp" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEvapApproachTemp" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEvapApproachTemp" ValidationGroup="Save" runat="server" ErrorMessage="Enter Evap Approach Temp(Psia)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Evap Water Flow Switch Status<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEvapWaterFlowSwitchStatus" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEvapWaterFlowSwitchStatus" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEvapWaterFlowSwitchStatus" ValidationGroup="Save" runat="server" ErrorMessage="Enter Evap Water Flow Switch Status"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Expansion Valve Position(%)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtExpansionValvePosition" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtExpansionValvePosition" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtExpansionValvePosition" ValidationGroup="Save" runat="server" ErrorMessage="Enter Expansion Valve Position(%)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Expansion Valve Position Steps<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtExpansionValvePositionSteps" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender22" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtExpansionValvePositionSteps" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator30" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtExpansionValvePositionSteps" ValidationGroup="Save" runat="server" ErrorMessage="Enter Expansion Valve Position Steps"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Evap Rfgt Liquidlevel(in)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEvapRfgtLiquidlevel" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtEvapRfgtLiquidlevel" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEvapRfgtLiquidlevel" ValidationGroup="Save" runat="server" ErrorMessage="Enter Evap Rfgt Liquidlevel(in)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Cond Entering Water Temp(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCondEnteringWaterTemp" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCondEnteringWaterTemp" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCondEnteringWaterTemp" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cond Entering Water Temp(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Cond Leaving Water Temp(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCondLeaveingWaterTemp" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender23" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCondLeaveingWaterTemp" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator31" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCondLeaveingWaterTemp" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cond Leaving Water Temp(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Cond Sat Rfgt Temp(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCondSatRfgtTemp" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCondSatRfgtTemp" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCondSatRfgtTemp" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cond Sat Rfgt Temp(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Cond Rftg Pressure(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCondRftgPressure" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCondRftgPressure" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCondRftgPressure" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cond Rftg Pressure(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Cond Approach Temp(psia)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCondApproachTemp" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCondApproachTemp" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCondApproachTemp" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cond Approach Temp(psia)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Cond Water Flow Swtich Satatus<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCondWaterFlowSwtichSatatus" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCondWaterFlowSwtichSatatus" ValidationGroup="Save" runat="server" ErrorMessage="Enter Cond Water Flow Swtich Satatus"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Compressor Starts(°F)</td>
                                <td>
                                    <asp:TextBox ID="txtCompressorStarts" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCompressorStarts" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator22" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCompressorStarts" ValidationGroup="Save" runat="server" ErrorMessage="Enter Compressor Starts(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Compressor Runtime(in HR)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCompressorRuntime" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCompressorRuntime" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator23" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCompressorRuntime" ValidationGroup="Save" runat="server" ErrorMessage="Enter Compressor Runtime(in HR)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>System Rfgt Diff Pressure(psid)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtSystemRfgtDiffPressure" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtSystemRfgtDiffPressure" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator24" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSystemRfgtDiffPressure" ValidationGroup="Save" runat="server" ErrorMessage="Enter System Rfgt Diff Pressure(psid)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Oil Pressure(psia)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtOilPressure" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtOilPressure" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtOilPressure" ValidationGroup="Save" runat="server" ErrorMessage="Enter Oil Pressure(psia)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Compressor Rfgt Discharge Temp(°F)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtCompressorRfgtDischargeTemp" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtCompressorRfgtDischargeTemp" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCompressorRfgtDischargeTemp" ValidationGroup="Save" runat="server" ErrorMessage="Enter Compressor Rfgt Discharge Temp(°F)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>RLA L1 L2 L3(%)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRLA" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender19" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtRLA" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRLA" ValidationGroup="Save" runat="server" ErrorMessage="Enter RLA L1 L2 L3(%)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Amps L1 L2 L3 (Amps)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtAmps" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender20" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtAmps" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator28" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAmps" ValidationGroup="Save" runat="server" ErrorMessage="Enter Amps L1 L2 L3 (Amps)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Volts AB BC CA (v)<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtVoltsABBCCA" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender21" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtVoltsABBCCA" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator29" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtVoltsABBCCA" ValidationGroup="Save" runat="server" ErrorMessage="Enter Volts AB BC CA (v)"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Shift Incharge<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddShiftInCharge" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddShiftInCharge" InitialValue="0" ValidationGroup="Save" runat="server" ErrorMessage="Enter Shift Incharge"></asp:RequiredFieldValidator>
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
                            <asp:GridView Width="600%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="CillerID" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
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
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("CillerID") %>'></asp:Label>
                                            <asp:Label ID="lblEntryBy" runat="server" Visible="false" Text='<%# Bind("EntryBy") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:TemplateField HeaderText="CL Number" SortExpression="CLVoucher">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Width="120px" Target="_blank" NavigateUrl='<%#Eval("CillerID") %>'>
                                                <asp:Label ID="lblCLNumber" runat="server" Text='<%# Bind("CLVoucher") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Workflow Details">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink" Width="110px" runat="server" Target="_blank" NavigateUrl='<%# "WorkflowStatusForChiller?id="+Eval("CillerID") %>'>
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
                                    <asp:BoundField DataField="ReadingTaken" HeaderText="Reading Taken" SortExpression="ReadingTaken" />
                                    <asp:BoundField DataField="ShiftIncharge" HeaderText="Shift Incharge" SortExpression="ShiftIncharge" />
                                    <asp:BoundField DataField="Time" HeaderText="Time" SortExpression="Time" />
                                    <asp:BoundField DataField="ChillerMode" HeaderText="Chiller Mode" SortExpression="ChillerMode" />
                                    <asp:BoundField DataField="ActiveChilledWaterSetpoint" HeaderText="Active Chilled Water Setpoint" SortExpression="ActiveChilledWaterSetpoint" />
                                    <asp:BoundField DataField="AverageLineCurrent" HeaderText="Average Line Current" SortExpression="AverageLineCurrent" />
                                    <asp:BoundField DataField="ActiveCurrentLimitSetpoint" HeaderText="Active Current Limit Setpoint" SortExpression="ActiveCurrentLimitSetpoint" />
                                    <asp:BoundField DataField="EvapEnteringWaterTemperature" HeaderText="Evap Entering Water Temperature" SortExpression="EvapEnteringWaterTemperature" />
                                    <asp:BoundField DataField="EvapLeavingWaterTemperature" HeaderText="Evap Leaving Water Temperature" SortExpression="EvapLeavingWaterTemperature" />
                                    <asp:BoundField DataField="EvapSatRfgtTemp" HeaderText="Evap Sat Rfgt Temp" SortExpression="EvapSatRfgtTemp" />
                                    <asp:BoundField DataField="EvapApproachTemp" HeaderText="Evap Approach Temp" SortExpression="EvapApproachTemp" />
                                    <asp:BoundField DataField="EvapWaterFlowSwitchStatus" HeaderText="Evap Water Flow Switch Status" SortExpression="EvapWaterFlowSwitchStatus" />
                                    <asp:BoundField DataField="ExpansionValvePosition" HeaderText="Expansion Valve Position" SortExpression="ExpansionValvePosition" />
                                    <asp:BoundField DataField="ExpansionValvePositionSteps" HeaderText="Expansion Valve Position Steps" SortExpression="ExpansionValvePositionSteps" />
                                    <asp:BoundField DataField="EvapRfgtLiquidlevel" HeaderText="Evap Rfgt Liquidlevel" SortExpression="EvapRfgtLiquidlevel" />
                                    <asp:BoundField DataField="CondEnteringWaterTemp" HeaderText="Cond Entering Water Temp" SortExpression="CondEnteringWaterTemp" />
                                    <asp:BoundField DataField="CondLeavingWaterTemp" HeaderText="Cond Leaving Water Temp" SortExpression="CondLeavingWaterTemp" />
                                    <asp:BoundField DataField="CondSatRfgtTemp" HeaderText="Cond Sat Rfgt Temp" SortExpression="CondSatRfgtTemp" />
                                    <asp:BoundField DataField="CondRftgPressure" HeaderText="Cond Rftg Pressure" SortExpression="CondRftgPressure" />
                                    <asp:BoundField DataField="CondApproachTemp" HeaderText="Cond Approach Temp" SortExpression="CondApproachTemp" />
                                    <asp:BoundField DataField="CondWaterFlowSwtichSatatus" HeaderText="Cond Water Flow Swtich Satatus" SortExpression="CondWaterFlowSwtichSatatus" />
                                    <asp:BoundField DataField="CompressorStarts" HeaderText="Compressor Starts" SortExpression="CompressorStarts" />
                                    <asp:BoundField DataField="CompressorRuntime" HeaderText="Compressor Runtime" SortExpression="CompressorRuntime" />
                                    <asp:BoundField DataField="SystemRfgtDiffPressure" HeaderText="System Rfgt Diff Pressure" SortExpression="SystemRfgtDiffPressure" />
                                    <asp:BoundField DataField="OilPressure" HeaderText="Oil Pressure" SortExpression="OilPressure" />
                                    <asp:BoundField DataField="CompressorRfgtDischargeTemp" HeaderText="Compressor Rfgt Discharge Temp" SortExpression="CompressorRfgtDischargeTemp" />
                                    <asp:BoundField DataField="RLA" HeaderText="R L A" SortExpression="RLA" />
                                    <asp:BoundField DataField="Amps" HeaderText="Amps" SortExpression="Amps" />
                                    <asp:BoundField DataField="VoltsABBCCA" HeaderText="Volts A B B C C A" SortExpression="VoltsABBCCA" />
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





