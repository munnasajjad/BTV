<%@ Page Title="Daily Transmitter Reading Entry" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Daily-Transmetter-reading-entry.aspx.cs" Inherits="app_Daily_Transmetter_reading_entry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="col-lg-7">
                <section class="panel">
                    <%--Body Contants--%>
                    <div id="Div2">
                        <div>

                            <fieldset>
                                <legend>Daily Transmetter Log Entry</legend>
                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td align="center" colspan="3">
                                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblId" runat="server" Visible="False"></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Station Name: </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddLocation" runat="server" CssClass="form-control select2me" AppendDataBoundItems="true" DataSourceID="SqlDataSource3"
                                                DataTextField="Name" DataValueField="LocationID" AutoPostBack="True" OnSelectedIndexChanged="ddLocation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT LocationID, Name FROM Location"></asp:SqlDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Name of Transmitter</td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddTransmitter" runat="server" CssClass="form-control select2me" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddTransmitter_OnSelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Channel Number</td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddChannelNo" runat="server" CssClass="form-control select2me" AppendDataBoundItems="true"
                                                DataTextField="Division" DataValueField="Id" AutoPostBack="True">
                                                <asp:ListItem>9</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT distinct  [Id], [Division] FROM [RoadDivision] where Circle=@Circle">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddTransmitter" Name="Circle" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Date</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender TargetControlID="txtDate" ID="ce1" runat="server" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Transmitter Output Power</td>
                                        <td>Video (W)
                                            <asp:TextBox ID="txtTransmitterOutputPowerVideo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>Audio (W)
                                            <asp:TextBox ID="txtTransmitterOutputPowerAudio" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Reflected Power</td>
                                        <td>Video (W)
                                            <asp:TextBox ID="txtReflectedPowerVideo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>Audio (W)
                                            <asp:TextBox ID="txtReflectedPowerAudio" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Exciter inoperation (A/B)</td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddExciterInOperation" runat="server" CssClass="form-control select2me" AppendDataBoundItems="true">
                                                <asp:ListItem Value="A">A</asp:ListItem>
                                                <asp:ListItem Value="B">B</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Pump in operation ( A/B)</td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddPumpInOperation" runat="server" CssClass="form-control select2me" AppendDataBoundItems="true">
                                                <asp:ListItem Value="A">A</asp:ListItem>
                                                <asp:ListItem Value="B">B</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Liquid Static Pressure (bar)</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtLiquidStaticPressure" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Pump Output Pressure</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtPumpOutputPressure" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>Liquid Temperature (°C)</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtLiquidTemperature" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>De-hydrator Line Pressure (Kpa)</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtDehydratorLinePressure" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <legend>Power Amplifier Data</legend>
                                        </td>
                                    </tr>
                                    <asp:Panel runat="server" ID="PA1">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-1</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA1AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA1VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA1Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA1T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA1T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA1T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA1T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA1T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA1T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PA2">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-2</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA2AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA2VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA2Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA2T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA2T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA2T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA2T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA2T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA2T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PA3">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-3</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA3AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA3VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA3Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA3T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA3T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA3T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA3T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA3T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA3T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PA4">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-4</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA4AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA4VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA4Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA4T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA4T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA4T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA4T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA4T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA4T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PA5">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-5</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA5AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA5VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA5Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA5T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA5T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA5T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA5T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA5T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA5T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PA6">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-6</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA6AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA6VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA6Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA6T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA6T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA6T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA6T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA6T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA6T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PA7">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-7</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA7AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA7VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA7Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA7T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA7T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA7T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA7T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA7T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA7T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="PA8">
                                        <tr>
                                            <td colspan="3" style="font-size: 20px; text-align: center;">
                                                <b>PA-8</b>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>AGC Level (V)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA8AGCLevel" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>VSWR (W)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA8VSWR" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Temperature (°C)</td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtPA8Temperature" runat="server" CssClass="form-control"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>Current in Ampears</td>
                                            <td colspan="2">
                                                <table style="text-align: center;">
                                                    <tr>
                                                        <td>T1
                                                        <asp:TextBox ID="PA8T1" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T2
                                                        <asp:TextBox ID="PA8T2" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T3
                                                        <asp:TextBox ID="PA8T3" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T4
                                                        <asp:TextBox ID="PA8T4" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T5
                                                        <asp:TextBox ID="PA8T5" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                        <td>T6
                                                        <asp:TextBox ID="PA8T6" runat="server" CssClass="form-control"></asp:TextBox></td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td>Remarks</td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtRemarks" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>


                                    <tr style="display: none;">
                                        <td>Status</td>
                                        <td>
                                            <asp:DropDownList ID="ddStatus" runat="server" CssClass="form-control">
                                                <asp:ListItem>সন্তোষজনক</asp:ListItem>
                                                <asp:ListItem>সন্তোষজনক নয়</asp:ListItem>
                                            </asp:DropDownList>
                                            <%-- <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                        </td>
                                    </tr>

                                    <tr style="display: none;">
                                        <td>Deployment type</td>
                                        <td>
                                            <asp:DropDownList ID="ddDtype" runat="server" CssClass="form-control select2me"
                                                DataTextField="Type" DataValueField="Type" AutoPostBack="True">
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT distinct  [Id], [Type] FROM [Deployment]"></asp:SqlDataSource>
                                        </td>
                                    </tr>

                                    <tr style="display: none;">
                                        <td>Comments</td>
                                        <td>
                                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr style="display: none;">
                                        <td>Photo Upload</td>
                                        <td>
                                            <asp:Image ID="Image1" runat="server" Width="200px" EnableViewState="false" />
                                            <asp:FileUpload ID="ImgUpload" runat="server" ClientIDMode="Static" CssClass="form-control" />
                                        </td>
                                    </tr>



                                    <tr style="background: none">
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                                            <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Clear" OnClick="btnClear_OnClick" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>


                        </div>
                    </div>
                    <%--End Body Contants--%>
                </section>
            </div>




            <div class="col-lg-5">
                <section class="panel">

                    <div id="Div1">
                        <div>

                            <fieldset>
                                <legend>Saved Data</legend>



                                <div class="table-responsive">
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical" DataKeyNames="Id" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                        <Columns>
                                            
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">This entry will be deleted permanently!</b><br />
                                                        Are you sure you want to delete this ?
                                                            <br />
                                                        <br />
                                                        <div style="text-align: right;">
                                                            <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                            <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                        </div>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="SL" ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>. 
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="Id" SortExpression="sl" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date"/>
                                            <asp:BoundField DataField="Station" HeaderText="Station" SortExpression="Station"/>
                                            <asp:BoundField DataField="Machine" HeaderText="Machine" SortExpression="Machine"/>
                                            <asp:BoundField DataField="ChannelNumber" HeaderText="Channel Number" SortExpression="ChannelNumber"/>
                                            <asp:BoundField DataField="TransmitterOutputPowerVideo" HeaderText="T.Output Video" SortExpression="TransmitterOutputPowerVideo"/>
                                            <asp:BoundField DataField="TransmitterOutputPowerAudio" HeaderText="T.Output Audio" SortExpression="TransmitterOutputPowerAudio"/>
                                            <asp:BoundField DataField="ReflectedPowerVideo" HeaderText="R.Power Video" SortExpression="ReflectedPowerVideo"/>
                                            <asp:BoundField DataField="ReflectedPowerAudio" HeaderText="R.PowerAudio" SortExpression="ReflectedPowerAudio"/>
                                            <asp:BoundField DataField="ExciterInOperation" HeaderText="ExciterInOperation" SortExpression="ExciterInOperation"/>
                                            <asp:BoundField DataField="PumpInOperation" HeaderText="PumpInOperation" SortExpression="PumpInOperation"/>
                                            <asp:BoundField DataField="LiouidStaticPressure" HeaderText="LiouidStaticPressure" SortExpression="LiouidStaticPressure"/>
                                            <asp:BoundField DataField="PumpOutputPressure" HeaderText="PumpOutputPressure" SortExpression="PumpOutputPressure"/>
                                            <asp:BoundField DataField="LiquidTemperature" HeaderText="LiquidTemperature" SortExpression="LiquidTemperature"/>
                                            <asp:BoundField DataField="DehydratorLinePressure" HeaderText="DehydratorLinePressure" SortExpression="DehydratorLinePressure"/>
                                            <asp:BoundField DataField="PA1AGCLevel" HeaderText="PA1AGCLevel" SortExpression="PA1AGCLevel"/>
                                            <asp:BoundField DataField="PA1VSWR" HeaderText="PA1VSWR" SortExpression="PA1VSWR"/>
                                            <asp:BoundField DataField="PA1Temperature" HeaderText="PA1Temperature" SortExpression="PA1Temperature"/>
                                            <asp:BoundField DataField="PA1T1" HeaderText="PA1T1" SortExpression="PA1T1"/>
                                            <asp:BoundField DataField="PA1T2" HeaderText="PA1T2" SortExpression="PA1T2"/>
                                            <asp:BoundField DataField="PA1T3" HeaderText="PA1T3" SortExpression="PA1T3"/>
                                            <asp:BoundField DataField="PA1T4" HeaderText="PA1T4" SortExpression="PA1T4"/>
                                            <asp:BoundField DataField="PA1T5" HeaderText="PA1T5" SortExpression="PA1T5"/>
                                            <asp:BoundField DataField="PA1T6" HeaderText="PA1T6" SortExpression="PA1T6"/>
                                            <asp:BoundField DataField="PA2AGCLevel" HeaderText="PA2AGCLevel" SortExpression="PA2AGCLevel"/>
                                            <asp:BoundField DataField="PA2VSWR" HeaderText="PA2VSWR" SortExpression="PA2VSWR"/>
                                            <asp:BoundField DataField="PA2Temperature" HeaderText="PA2Temperature" SortExpression="PA2Temperature"/>
                                            <asp:BoundField DataField="PA2T1" HeaderText="PA2T1" SortExpression="PA2T1"/>
                                            <asp:BoundField DataField="PA2T2" HeaderText="PA2T2" SortExpression="PA2T2"/>
                                            <asp:BoundField DataField="PA2T3" HeaderText="PA2T3" SortExpression="PA2T3"/>
                                            <asp:BoundField DataField="PA2T4" HeaderText="PA2T4" SortExpression="PA2T4"/>
                                            <asp:BoundField DataField="PA2T5" HeaderText="PA2T5" SortExpression="PA2T5"/>
                                            <asp:BoundField DataField="PA2T6" HeaderText="PA2T6" SortExpression="PA2T6"/>
                                            <asp:BoundField DataField="PA3AGCLevel" HeaderText="PA3AGCLevel" SortExpression="PA3AGCLevel"/>
                                            <asp:BoundField DataField="PA3VSWR" HeaderText="PA3VSWR" SortExpression="PA3VSWR"/>
                                            <asp:BoundField DataField="PA3Temperature" HeaderText="PA3Temperature" SortExpression="PA3Temperature"/>
                                            <asp:BoundField DataField="PA3T1" HeaderText="PA3T1" SortExpression="PA3T1"/>
                                            <asp:BoundField DataField="PA3T2" HeaderText="PA3T2" SortExpression="PA3T2"/>
                                            <asp:BoundField DataField="PA3T3" HeaderText="PA3T3" SortExpression="PA3T3"/>
                                            <asp:BoundField DataField="PA3T4" HeaderText="PA3T4" SortExpression="PA3T4"/>
                                            <asp:BoundField DataField="PA3T5" HeaderText="PA3T5" SortExpression="PA3T5"/>
                                            <asp:BoundField DataField="PA3T6" HeaderText="PA3T6" SortExpression="PA3T6"/>
                                            <asp:BoundField DataField="PA4AGCLevel" HeaderText="PA4AGCLevel" SortExpression="PA4AGCLevel"/>
                                            <asp:BoundField DataField="PA4VSWR" HeaderText="PA4VSWR" SortExpression="PA4VSWR"/>
                                            <asp:BoundField DataField="PA4Temperature" HeaderText="PA4Temperature" SortExpression="PA4Temperature"/>
                                            <asp:BoundField DataField="PA4T1" HeaderText="PA4T1" SortExpression="PA4T1"/>
                                            <asp:BoundField DataField="PA4T2" HeaderText="PA4T2" SortExpression="PA4T2"/>
                                            <asp:BoundField DataField="PA4T3" HeaderText="PA4T3" SortExpression="PA4T3"/>
                                            <asp:BoundField DataField="PA4T4" HeaderText="PA4T4" SortExpression="PA4T4"/>
                                            <asp:BoundField DataField="PA4T5" HeaderText="PA4T5" SortExpression="PA4T5"/>
                                            <asp:BoundField DataField="PA4T6" HeaderText="PA4T6" SortExpression="PA4T6"/>
                                            <asp:BoundField DataField="PA5AGCLevel" HeaderText="PA5AGCLevel" SortExpression="PA5AGCLevel"/>
                                            <asp:BoundField DataField="PA5VSWR" HeaderText="PA5VSWR" SortExpression="PA5VSWR"/>
                                            <asp:BoundField DataField="PA5Temperature" HeaderText="PA5Temperature" SortExpression="PA5Temperature"/>
                                            <asp:BoundField DataField="PA5T1" HeaderText="PA5T1" SortExpression="PA5T1"/>
                                            <asp:BoundField DataField="PA5T2" HeaderText="PA5T2" SortExpression="PA5T2"/>
                                            <asp:BoundField DataField="PA5T3" HeaderText="PA5T3" SortExpression="PA5T3"/>
                                            <asp:BoundField DataField="PA5T4" HeaderText="PA5T4" SortExpression="PA5T4"/>
                                            <asp:BoundField DataField="PA5T5" HeaderText="PA5T5" SortExpression="PA5T5"/>
                                            <asp:BoundField DataField="PA5T6" HeaderText="PA5T6" SortExpression="PA5T6"/>
                                            <asp:BoundField DataField="PA6AGCLevel" HeaderText="PA6AGCLevel" SortExpression="PA6AGCLevel"/>
                                            <asp:BoundField DataField="PA6VSWR" HeaderText="PA6VSWR" SortExpression="PA6VSWR"/>
                                            <asp:BoundField DataField="PA6Temperature" HeaderText="PA6Temperature" SortExpression="PA6Temperature"/>
                                            <asp:BoundField DataField="PA6T1" HeaderText="PA6T1" SortExpression="PA6T1"/>
                                            <asp:BoundField DataField="PA6T2" HeaderText="PA6T2" SortExpression="PA6T2"/>
                                            <asp:BoundField DataField="PA6T3" HeaderText="PA6T3" SortExpression="PA6T3"/>
                                            <asp:BoundField DataField="PA6T4" HeaderText="PA6T4" SortExpression="PA6T4"/>
                                            <asp:BoundField DataField="PA6T5" HeaderText="PA6T5" SortExpression="PA6T5"/>
                                            <asp:BoundField DataField="PA6T6" HeaderText="PA6T6" SortExpression="PA6T6"/>
                                            <asp:BoundField DataField="PA7AGCLevel" HeaderText="PA7AGCLevel" SortExpression="PA7AGCLevel"/>
                                            <asp:BoundField DataField="PA7VSWR" HeaderText="PA7VSWR" SortExpression="PA7VSWR"/>
                                            <asp:BoundField DataField="PA7Temperature" HeaderText="PA7Temperature" SortExpression="PA7Temperature"/>
                                            <asp:BoundField DataField="PA7T1" HeaderText="PA7T1" SortExpression="PA7T1"/>
                                            <asp:BoundField DataField="PA7T2" HeaderText="PA7T2" SortExpression="PA7T2"/>
                                            <asp:BoundField DataField="PA7T3" HeaderText="PA7T3" SortExpression="PA7T3"/>
                                            <asp:BoundField DataField="PA7T4" HeaderText="PA7T4" SortExpression="PA7T4"/>
                                            <asp:BoundField DataField="PA7T5" HeaderText="PA7T5" SortExpression="PA7T5"/>
                                            <asp:BoundField DataField="PA7T6" HeaderText="PA7T6" SortExpression="PA7T6"/>
                                            <asp:BoundField DataField="PA8AGCLevel" HeaderText="PA8AGCLevel" SortExpression="PA8AGCLevel"/>
                                            <asp:BoundField DataField="PA8VSWR" HeaderText="PA8VSWR" SortExpression="PA8VSWR"/>
                                            <asp:BoundField DataField="PA8Temperature" HeaderText="PA8Temperature" SortExpression="PA8Temperature"/>
                                            <asp:BoundField DataField="PA8T1" HeaderText="PA8T1" SortExpression="PA8T1"/>
                                            <asp:BoundField DataField="PA8T2" HeaderText="PA8T2" SortExpression="PA8T2"/>
                                            <asp:BoundField DataField="PA8T3" HeaderText="PA8T3" SortExpression="PA8T3"/>
                                            <asp:BoundField DataField="PA8T4" HeaderText="PA8T4" SortExpression="PA8T4"/>
                                            <asp:BoundField DataField="PA8T5" HeaderText="PA8T5" SortExpression="PA8T5"/>
                                            <asp:BoundField DataField="PA8T6" HeaderText="PA8T6" SortExpression="PA8T6"/>
                                            <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks"/>
                                        </Columns>
                                        <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                        <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                        <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </div>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT DailyTransmitterLogEntry.Id, Location.Name AS Station, Transmitters.TransmitterName AS Machine, ChannelNumber, Date, TransmitterOutputPowerVideo, TransmitterOutputPowerAudio, ReflectedPowerVideo, ReflectedPowerAudio, ExciterInOperation, PumpInOperation, 
                         LiouidStaticPressure, PumpOutputPressure, LiquidTemperature, DehydratorLinePressure, PA1AGCLevel, PA1VSWR, PA1Temperature, PA1T1, PA1T2, PA1T3, PA1T4, PA1T5, PA1T6, PA2AGCLevel, PA2VSWR, PA2Temperature, 
                         PA2T1, PA2T2, PA2T3, PA2T4, PA2T5, PA2T6, PA3AGCLevel, PA3VSWR, PA3Temperature, PA3T1, PA3T2, PA3T3, PA3T4, PA3T5, PA3T6, PA4AGCLevel, PA4VSWR, PA4Temperature, PA4T1, PA4T2, PA4T3, PA4T4, PA4T5, PA4T6, 
                         PA5AGCLevel, PA5VSWR, PA5Temperature, PA5T1, PA5T2, PA5T3, PA5T4, PA5T5, PA5T6, PA6AGCLevel, PA6VSWR, PA6Temperature, PA6T1, PA6T2, PA6T3, PA6T4, PA6T5, PA6T6, PA7AGCLevel, PA7VSWR, PA7Temperature, 
                         PA7T1, PA7T2, PA7T3, PA7T4, PA7T5, PA7T6, PA8AGCLevel, PA8VSWR, PA8Temperature, PA8T1, PA8T2, PA8T3, PA8T4, PA8T5, PA8T6, Remarks
FROM            DailyTransmitterLogEntry 
INNER JOIN Location ON Location.LocationID = DailyTransmitterLogEntry.StationId
INNER JOIN Transmitters ON Transmitters.Id = DailyTransmitterLogEntry.TransmitterMachineId">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddChannelNo" Name="Division" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </fieldset>



                        </div>
                    </div>
                    <%--End Body Contants--%>
                </section>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

