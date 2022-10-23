<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="StockReport.aspx.cs" Inherits="app_StockReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>
    <div class="col-lg-12">
                <section class="panel">
                    <fieldset>
                        <legend>Stock Register</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="240%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="StockRegID" >
                                <Columns>

                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("StockRegID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" SortExpression="ProductName" />
                                    <asp:BoundField DataField="EntryType" HeaderText="Entry Type" SortExpression="EntryType" />
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:BoundField DataField="PreviousStockIn" HeaderText="Previous Stock In" SortExpression="PreviousStockIn" />
                                    <asp:BoundField DataField="StockIn" HeaderText="Stock In" SortExpression="StockIn" />
                                    <asp:BoundField DataField="StockInCashMemoChallanNo" HeaderText="Stock In Cash Memo Challan No" SortExpression="StockInCashMemoChallanNo" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                                    <asp:BoundField DataField="StockOutCashMemoChallanNo" HeaderText="Stock Out Cash Memo Challan No" SortExpression="StockOutCashMemoChallanNo" />
                                    <asp:BoundField DataField="SellQty" HeaderText="Sell Qty" SortExpression="SellQty" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                    <%--<asp:BoundField DataField="EntryBy" HeaderText="Entry By" SortExpression="EntryBy" />--%>
                                    
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

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" Runat="Server">
</asp:Content>

