<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="CurrentStock.aspx.cs" Inherits="app_CurrentStock" %>


<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css">
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js"></script>

    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/1.5.1/css/buttons.dataTables.min.css">
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.1/js/dataTables.buttons.min.js"></script>

    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.print.min.js"></script>

    <%--<script type="text/javascript" src="https://cdn.datatables.net/1.10.10/js/jquery.dataTables.min.js"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.10/css/jquery.dataTables.min.css">--%>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[25, 25, 50, 100, -1], [25, 25, 50, 100, "All"]], //value:item pair
                dom: 'Bfrtip',
                buttons: ['print', 'excel', 'pdf']
            });
        });

        $(window).load(function () {
            //jScript();
        });
        $('.tbl_default').DataTable({
            buttons: [
                'print'
            ]
        });
        function jScript() {
            $(".tbl_default").prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]] //value:item pair
            });
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div style="padding:50%;background-color: #949896;position: absolute;z-index: 1000;border-radius: 12px;">
                <div id="IMGDIV" style="position: fixed; left: 50%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                    <img src="../images/loader.gif" alt="Processing... Please Wait." />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>--%>

    <script type="text/javascript" language="javascript">
        Sys.Application.add_load(callJquery);
    </script>
    <div class="row">
        <div class="col-md-12">
            <div class="portlet box blue">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder">Current Stock Position</i>
                    </div>
                    <div class="tools">
                        <a href="" class="collapse"></a>
                        <a href="#portlet-config" data-toggle="modal" class="config"></a>
                        <a href="" class="reload"></a>
                        <a href="" class="remove"></a>
                    </div>
                </div>
                <div class="portlet-body form">
                    <asp:Label ID="lblProject" runat="server" Visible="false" />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                    <div class="form-horizontal" role="form">
                        <div class="form-body">
                            <div class="row">
                                <div class="col-md-6 hidden">
                                    <div class="control-group">
                                        <label class="control-label">Date Range(Upto) : </label>
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" placeholder="DD/MM/YYYY"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                            Enabled="True" TargetControlID="txtDate">
                                        </asp:CalendarExtender>
                                    </div>
                                </div>

                                <div class="col-md-6 hidden">
                                    <div class="form-group">
                                        <label class="control-label">
                                            <asp:Literal ID="lblName" runat="server" Text="Product"></asp:Literal>
                                            <asp:Literal ID="Literal5" runat="server" Text="Group : "></asp:Literal></label>
                                        <asp:DropDownList ID="ddGroup" CssClass="select2me" Width="70%" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddGroup_OnSelectedIndexChanged" AppendDataBoundItems="True" OnDataBound="ddGroup_OnDataBound">
                                        </asp:DropDownList>
                                        
                                    </div>
                                </div>

                            </div>
                            <div class="row">
                                
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">Store Name : </label>
                                        <asp:DropDownList ID="ddStore" runat="server" CssClass="form-control select2me" AutoPostBack="True" >
                                            
                                        </asp:DropDownList>
                                        <asp:SqlDataSource ID="SqlDataSource6" runat="server"
                                                           ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                           SelectCommand="SELECT StoreAssignID, Name FROM Store ORDER BY [Name]">
                                            <%--<SelectParameters>
                                                <asp:ControlParameter ControlID="lblProject" Name="ProjectId" PropertyName="Text" Type="String" />
                                            </SelectParameters>--%>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">
                                            <asp:Literal ID="lblPrdct" runat="server" Text="Product"></asp:Literal></label>
                                        <asp:DropDownList ID="ddProducts" CssClass="select2me" Width="70%" runat="server">
                                        </asp:DropDownList>

                                    </div>
                                </div>

                                

                            </div>

                        </div>
                    </div>
                    <div class="form-actions">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button runat="server" CssClass="btn blue" Text="Search" OnClick="OnClick" />
                                <asp:Button runat="server" CssClass="btn blue" Text="Print" OnClick="OnPrintClick" Visible="False"/>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
        <i frame id="frame1" runat="server" height="800px" width="100%" visible="False"></i>
    </div>
    <div id="divGrid" runat="server" class="row" visible="True">
        <div class="col-md-12">
            <div class="table-responsive">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="tbl_default odd gradeX">

                    <Columns>

                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <%#Container.DataItemIndex+1 %>.
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="GroupName" HeaderText="Product Type" SortExpression="GroupName" DataFormatString="{0:d}"></asp:BoundField>
                        <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                        <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" />

                        <%--<asp:BoundField DataField="Product" HeaderText="Product Name" SortExpression="Product" />
                                        
                                        <asp:BoundField DataField="InQuantity" HeaderText="In Qty" SortExpression="InWeight" />
                                        <asp:BoundField DataField="OutQuantity" HeaderText="Out Qty" SortExpression="OutWeight" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                        <asp:BoundField DataField="Remark" HeaderText="Remark" SortExpression="Remark" />--%>
                        <%--<asp:BoundField DataField="Warehouse" HeaderText="Warehouse" SortExpression="Warehouse" />--%>
                    </Columns>

                </asp:GridView>

            </div>
        </div>
    </div>
    <%--</ContentTemplate>
        </asp:UpdatePanel>--%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>


