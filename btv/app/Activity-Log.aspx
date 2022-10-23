<%@ Page Title="Activity-Log" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Activity-Log.aspx.cs" Inherits="app_Activity_Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <div class="col-lg-12">
        <section class="panel">
                    <fieldset>
                        <legend>Activity Log</legend>
                        <asp:GridView ID="GridView1" CellPadding="4" CssClass="table table-bordered table-striped" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource5">
                            <Columns>
                                <asp:TemplateField HeaderText="#SL" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                            <asp:Label ID="Label1" Visible="false" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />

                                    </asp:TemplateField>
                                <asp:BoundField DataField="FormName" HeaderText="Form Name" SortExpression="FormName"></asp:BoundField>
                                <asp:BoundField DataField="Activity" HeaderText="Activity" SortExpression="Activity"></asp:BoundField>
                                <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                                <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId"></asp:BoundField>
                                <asp:BoundField DataField="EntryDate" HeaderText="Entry Date" SortExpression="Entry Date" DataFormatString="{0:d}"></asp:BoundField> 
                                <asp:BoundField DataField="IPAddress" HeaderText="IP Address" SortExpression="IPAddress"></asp:BoundField>
                               
                            </Columns>
                             <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                                <AlternatingRowStyle BackColor="White" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT  Id, FormName, Activity, Description, UserId, IPAddress,EntryDate FROM ActivityLog"></asp:SqlDataSource>

                    </fieldset>
              
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>

