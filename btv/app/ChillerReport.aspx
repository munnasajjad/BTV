<%@ Page Title="Chiller Report" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="ChillerReport.aspx.cs" Inherits="app_ChillerReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <!-- BEGIN PAGE LEVEL STYLES -->
    <link rel="stylesheet" type="text/css" href="assets/plugins/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2.css" />
    <link rel="stylesheet" type="text/css" href="assets/plugins/select2/select2-metronic.css" />
    <link rel="stylesheet" href="assets/plugins/data-tables/DT_bootstrap.css" />

    <!-- END PAGE LEVEL STYLES -->
    <style type="text/css">
        label {
            padding-top: 6px;
            text-align: right;
        }

        .table1 {
            width: 100%;
        }

            .table1 th {
                vertical-align: middle;
                font-weight: 700;
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
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ScriptManager>

    <%-- <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>--%>




    <h3 class="page-title">Earth Station Report</h3>

    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>


    <div class="row">

        <div class="col-lg-12">
            <section class="panel">
                <%--Body Contants--%>
                <div id="Div2">
                    <div>

                        <fieldset>
                            <%--<legend>Search Terms</legend>--%>
                            <table border="0" width="100%" style="width: 100%" class="table1">
                                <tr>
                                    <th>Location</th>
                                    <th></th>
                                    <th>Date From</th>
                                    <th></th>
                                    <th>Date To</th>
                                    <td></td>
                                    <th></th>
                                </tr>
                                <tr>
                                    <td style="vertical-align: middle; width: 45%">
                                        <asp:DropDownList ID="ddLocation" runat="server" CssClass="form-control select2me"
                                            DataTextField="Name" DataValueField="LocationID" AppendDataBoundItems="true" AutoPostBack="True">
                                            <%--<asp:ListItem>---ALL---</asp:ListItem>--%>
                                        </asp:DropDownList>
                                        <%--                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                    SelectCommand="SELECT [PartyID], [Company] FROM [Party] WHERE ([Type] = 'customer') ORDER BY [Company]"></asp:SqlDataSource>--%>

                                    </td>



                                    <td>&nbsp; </td>
                                    <td>
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                    </td>

                                    <td>&nbsp; </td>
                                    <td>
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                    </td>

                                    <td></td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Button ID="btnSearch" CssClass="btn btn-s-md btn-primary" runat="server" Text="Search" OnClick="btnShow_Click" OnClientClick="SetTarget();" />
                                        <asp:Button ID="btnReset" CssClass="btn btn-s-md btn-danger" runat="server" Text="Reset" OnClick="btnReset_OnClick" />
                                        <%--<asp:Button ID="btnPrint" CssClass="btn btn-s-md btn-default" runat="server" Text="Print" OnClick="btnPrint_Click" />--%>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>


                    </div>
                </div>

            </section>
        </div>
    </div>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>



