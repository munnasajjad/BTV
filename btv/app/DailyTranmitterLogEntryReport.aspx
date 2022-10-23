<%@ Page Title="Daily Transmitter Log Entry Report" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="DailyTranmitterLogEntryReport.aspx.cs" Inherits="app_DailyTranmitterLogEntryReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.10/css/jquery.dataTables.min.css">
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2.css" />
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2-metronic.css" />

    <style type="text/css">
        .col-md-4 .control-group input, .col-md-4 .control-group select {
            width: 100%;
        }

        .col-md-4 .control-group label {
            padding-bottom: 4px;
        }

        .table-bordered > thead > tr > th, .table-bordered > tbody > tr > th, .table-bordered > tfoot > tr > th, .table-bordered > thead > tr > td, .table-bordered > tbody > tr > td, .table-bordered > tfoot > tr > td {
            border: 1px solid #ddd;
            color: GrayText;
            font-size: 15px;
        }

        input#ctl00_BodyContent_chkMerge, height_fix {
            height: 17px !important;
            top: -3px !important;
            margin-top: -5px !important;
        }

        bottom_fix {
            margin-bottom: -15px !important;
        }
        .table1 {
            width: 100%;
        }

        .table1 th {
            vertical-align: middle;
            font-weight: 800;
            font-size: 15px;
        }

        .table1 .form-control, .table1 select {
            width: 100%;
        }

        table#ctl00_BodyContent_GridView1 {
            /*min-width: 1200px;*/
        }

        table#ctl00_BodyContent_GridView1 tr {
            height: 20px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BodyContent" runat="Server">

    <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>--%>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="row">
                <div class="col-md-12 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Daily Transmitter Report
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">
                                    <asp:Label ID="lblMsg2" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:LoginName ID="LoginName1" runat="server" Visible="false" />
                                    <asp:Label ID="lblProject" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblOrderID" runat="server" Visible="false"></asp:Label>

                                    <table border="1" width="100%" style="width: 100%">
                                        <tr>
                                            <th style="font-size: 15px">Location</th>
                                            <th></th>
                                            <th style="font-size: 15px">Transmitter</th>
                                            <th></th>
                                            <th style="font-size: 15px">Date From</th>
                                            <th></th>
                                            <th style="font-size: 15px">Date To</th>
                                            <td></td>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: middle; width: 25%">
                                                <asp:DropDownList ID="ddLocation" runat="server" CssClass="form-control select2me" DataSourceID="SqlDataSource3"
                                                    DataTextField="Name" DataValueField="LocationID" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddLocation_OnSelectedIndexChanged">
                                                    <%--<asp:ListItem>---ALL---</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT LocationID, Name FROM Location"></asp:SqlDataSource>

                                            </td>
                                            <td>&nbsp; </td>
                                            <td style="vertical-align: middle; width: 25%">
                                                <asp:DropDownList ID="ddTransmitterType" runat="server" CssClass="form-control select2me" AppendDataBoundItems="true">
                                                    <%--<asp:ListItem>---ALL---</asp:ListItem>--%>
                                                </asp:DropDownList>
                                                <%--<asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT id, TransmitterName FROM Transmitters WHERE StationID = @StationID">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="ddLocation" Name="StationID" PropertyName="SelectedValue" />
                                                    </SelectParameters>
                                                </asp:SqlDataSource>--%>

                                            </td>
                                            <td>&nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                            </td>

                                            <td>&nbsp; </td>
                                            <td>
                                                <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                            </td>

                                            <td></td>
                                            <td style="text-align: center; vertical-align: middle;">
                                                <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="Search" OnClick="btnSearch_OnClick" />
                                                <%--<asp:Button ID="btnReset" CssClass="btn btn-s-md btn-danger" runat="server" Text="Reset" />--%>
                                                <%--<asp:Button ID="btnPrint" CssClass="btn btn-s-md btn-default" runat="server" Text="Print" OnClick="btnPrint_Click" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box blue">
                        <%--<div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Daily Production Report
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>--%>
                        <div class="portlet-body form">



                            <iframe id="iFrame" runat="server" height="800px" width="100%"></iframe>


                            <div class="table-responsive">
                                <asp:Literal ID="ltrtotal" runat="server"></asp:Literal>


                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" SelectedRowStyle-BackColor="LightBlue"
                                    OnRowDataBound="ItemGrid_RowDataBound" CssClass="tbl_default zebra table" Visible="False">

                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>.
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>

                                    </Columns>

                                    <SelectedRowStyle BackColor="LightBlue" />

                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT TOP(100) [EntryDate], [EntryType],
                            (Select Purpose from Purpose where pid=stock.Purpose) AS Purpose,
                                    [InvoiceID], [RefNo], [ProductName],
                                    (SELECT  [Company] FROM [Party] WHERE [PartyID]= Stock.Customer) AS Customer,
                                    (SELECT [BrandName] FROM [CustomerBrands] WHERE BrandID=Stock.BrandID) AS BrandID,
                                    (SELECT [BrandName] FROM [Brands] WHERE BrandID=Stock.SizeId) AS SizeId,
                                    (SELECT [DepartmentName] FROM [Colors] WHERE Departmentid=Stock.Color) AS Color,
                                    (SELECT [spec] FROM [Specifications] WHERE  CAST(id AS nvarchar)=Stock.Spec) AS Spec,
                                    [InQuantity], [OutQuantity], [InWeight], [OutWeight], Remark FROM [Stock] WHERE ([ProductID] = @ProductID)
                                   ORDER BY [EntryDate] DESC, [EntryID] DESC">
                                    <SelectParameters>
                                        <%--<asp:ControlParameter ControlID="ddProduct" Name="ProductID" PropertyName="SelectedValue" Type="String" />
                                        <asp:ControlParameter ControlID="ddType" Name="ItemType" PropertyName="SelectedValue" Type="String" />--%>
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


