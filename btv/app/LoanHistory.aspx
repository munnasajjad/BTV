<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="LoanHistory.aspx.cs" Inherits="app_LoanHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style>
        @media (min-width: 1025px) {
            .panel {
                min-height:150px !important;
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
                        <legend>Search By Employee</legend>
                        <table border="0" >
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <%-- <tr>
                                <td>Store Name<span class="required">*</span></td>                                
                                <td>
                                    <asp:DropDownList ID="ddStore" runat="server" Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddStore_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>
                            <tr>
                                <td style="width:150px">
                                    <label>Employee Name<span class="required">*</span></label>
                                </td>
                                <td style="width:50%; float:left">
                                    <asp:DropDownList ID="ddlEmployee" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>
            <div class="col-lg-12">
                <section class="panel">
                    <fieldset>
                        <legend>Loan History</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="280%" ID="gvLoanHistory" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" AllowPaging="True" OnPageIndexChanging="gvLoanHistory_OnPageIndexChanging" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"]) %>' BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical">
                                <Columns>

                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="DateofLv" />
                                    <asp:BoundField DataField="VoucherNo" HeaderText="Voucher No" SortExpression="LvInvoiceNo" />
                                    <asp:BoundField DataField="PName" HeaderText="Product Name" SortExpression="PName" />
                                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" SortExpression="QTYNeed" />
                                    <asp:BoundField DataField="StoreName" HeaderText="StoreName" SortExpression="StoreName" />
                                
                                </Columns>
                                <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                <PagerStyle CssClass="Pagination" BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
</asp:Content>

