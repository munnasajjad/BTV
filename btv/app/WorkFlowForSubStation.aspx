﻿<%@ Page Title="WorkFlow For Sub Station" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="WorkFlowForSubStation.aspx.cs" Inherits="app_WorkFlowForSubStation" %>

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
                                <legend>Sub Station Voucher Information</legend>
                            </div>
                            <div class="portlet-body">

                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">
                                        <label>Date:</label>
                                        <asp:Label ID="txtDate" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>SS Number:</label>
                                        <asp:Label ID="txtSSNumber" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>PFI:</label>
                                        <asp:Label ID="txtPFI" runat="server"></asp:Label>
                                    </div>
                                </div>

                                <div class="col-sm-12" style="margin-bottom: 10px">

                                    <div class="col-md-4">
                                        <label>Condition:</label>
                                        <asp:Label ID="txtCondition" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>Transformer:</label>
                                        <asp:Label ID="txtTransformer" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-4">
                                        <label>HT Switch Gear:</label>
                                        <asp:Label ID="txtHTSwitchGear" runat="server"></asp:Label>
                                    </div>

                                </div>


                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>HT Battery Changer:</label>
                                        <asp:Label ID="txtHTBatteryChanger" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>HT Ampier X-Former:</label>
                                        <asp:Label ID="txtHTAmpierXFormer" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>LT Panel:</label>
                                        <asp:Label ID="txtLTPanel" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Generator Battery Condition:</label>
                                        <asp:Label ID="txtGeneratorBatteryCondition" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Load Shedding Start Time:</label>
                                        <asp:Label ID="txtLoadSheddingStartTime" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Load Shedding Stop Time:</label>
                                        <asp:Label ID="txtLoadSheddingStopTime" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Generator Start Time:</label>
                                        <asp:Label ID="txtGeneratorStartTime" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Generator Stop Time:</label>
                                        <asp:Label ID="txtGeneratorStopTime" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Fuel Stockin Litre:</label>
                                        <asp:Label ID="txtFuelStockinLitre" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Fuel Consumption In litre:</label>
                                        <asp:Label ID="txtFuelConsumptionInlitre" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Running Hour:</label>
                                        <asp:Label ID="txtRunningHour" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Attendent By:</label>
                                        <asp:Label ID="txtAttendentBy" runat="server"></asp:Label>

                                    </div>

                                </div>
                                <div class="col-sm-12" style="margin-bottom: 10px">
                                    <div class="col-md-4">

                                        <label>Maintenance:</label>
                                        <asp:Label ID="txtMaintenance" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

                                        <label>Shift Incharge:</label>
                                        <asp:Label ID="txtShiftInCharge" runat="server"></asp:Label>

                                    </div>
                                    <div class="col-md-4">

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
