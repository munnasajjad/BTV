<%@ Page Title="Advanced Search" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="AdvanchedSearch.aspx.cs" Inherits="app_AdvanchedSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        @media (min-width: 1025px) {
            .panel {
                min-height: 100% !important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
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

            <div class="col-lg-12">
                <section class="panel">

                    <fieldset>
                        <legend>Search Stock Register</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>


                            <tr>
                                <td style="width: 150px">Category</td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AppendDataBoundItems="true" Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px">Sub Category</td>
                                <td>
                                    <asp:DropDownList ID="ddlSubCategory" Width="100%" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px">Product Name</td>
                                <td>
                                    <asp:DropDownList ID="ddlProduct" AppendDataBoundItems="true" runat="server" Width="100%" CssClass="form-control select2me" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 150px">Store Name</td>
                                <td>
                                    <asp:DropDownList ID="ddlStore" Width="100%" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged" CssClass="form-control select2me"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 150px"></td>
                                <td>
                                    <asp:TextBox ID="txtSearch" CssClass="form-control" placeholder="Search by product name" runat="server"></asp:TextBox>
                                </td>
                                <td style="width: 150px"></td>
                                <td>
                                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" OnClick="btnSearch_Click" Text="Search" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>
            <div class="col-lg-12">
                <section class="panel">
                    <fieldset>
                        <legend>Stock Register</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="100%" AllowPaging="True" OnPageIndexChanging="gridSearch_OnPageIndexChanging" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"]) %>' ID="gridSearch" OnRowDataBound="gridSearch_RowDataBound" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" DataKeyNames="ProductID" BorderWidth="1px" ForeColor="Black"
                                GridLines="Vertical">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("ProductID") %>'></asp:Label>
                                            <a href="JavaScript:StateCity('div<%# Eval("ProductID") %>');">
                                                <img alt="details" width="12" id="imgdiv<%# Eval("ProductID") %>" src="../app/images/plus.png" />
                                            </a>
                                            <div id="div<%# Eval("ProductID") %>" style="display: none;">
                                                <asp:GridView ID="GridDetails" runat="server" AutoGenerateColumns="false" DataKeyNames="ProductID" CssClass="table table-bordered table-striped" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>.
                                                                <%-- <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("StockRegID") %>'></asp:Label>--%>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductDetailsID" Visible="false" runat="server" Text='<%# Bind("ProductDetailsID") %>'></asp:Label>
                                                                <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Serial No" SortExpression="SerialNo">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%# Bind("SerialNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Model No" SortExpression="ModelNo">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblModelNo" runat="server" Text='<%# Bind("ModelNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Part No" SortExpression="PartNo">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPartNo" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Product Status" SortExpression="ProductStatus">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblProductStatus" runat="server" Text='<%# Bind("ProductStatus") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                           <%-- <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("StockRegID") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="LocationName" HeaderText="Main Office" SortExpression="LocationName" />
                                    <asp:BoundField DataField="Name" HeaderText="Store Name" SortExpression="Name" />
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                                    <asp:BoundField DataField="QTY" HeaderText="Stock QTY" SortExpression="QTY" />
                                    <asp:BoundField DataField="UnitName" HeaderText="Unit" SortExpression="UnitName" />

                                </Columns>
                                <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                <PagerStyle CssClass="Pagination" BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
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
    <script type="text/javascript">  
        function StateCity(input) {
            var displayIcon = "img" + input;
            if ($("#" + displayIcon).attr("src") == "../app/images/plus.png") {
                $("#" + displayIcon).closest("tr")
                    .after("<tr><td></td><td colspan = '100%'>" + $("#" + input)
                        .html() + "</td></tr>");
                $("#" + displayIcon).attr("src", "../app/images/minus.png");
            } else {
                $("#" + displayIcon).closest("tr").next().remove();
                $("#" + displayIcon).attr("src", "../app/images/plus.png");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

