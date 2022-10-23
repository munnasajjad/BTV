<%@ Page Title="Registers" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Registers.aspx.cs" Inherits="app_Registers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        @media (min-width: 1025px) {
            .panel {
                min-height: 380px !important;
            }
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
        });
    </script>
    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>--%>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="col-lg-12">
                <section class="panel">

                    <fieldset>
                        <legend>Search Register</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <tr class="hidden">
                                <td>Transaction Type<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddType" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddType" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter transaction type"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="color: orange; font-size: 1.2rem">
                                    <asp:RadioButton ID="rdGrn" runat="server" CssClass="radio-m" AutoPostBack="true" OnCheckedChanged="rdLoan_CheckedChanged" GroupName="voucher" Text="GRN Voucher" />
                                    <asp:RadioButton ID="rdSir" runat="server" CssClass="radio-m" AutoPostBack="true" OnCheckedChanged="rdLoan_CheckedChanged" Checked="true" GroupName="voucher" Text="SIR Voucher" />
                                    <asp:RadioButton ID="rdLoan" runat="server" CssClass="radio-m" AutoPostBack="true" OnCheckedChanged="rdLoan_CheckedChanged" GroupName="voucher" Text="Loan Voucher"/>
                                    <asp:RadioButton ID="rdReturn" runat="server" CssClass="radio-m" AutoPostBack="true" OnCheckedChanged="rdLoan_CheckedChanged" GroupName="voucher" Text="Return Voucher" />
                                   
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>Date From<span class="required">*</span></td>
                                <td >
                                    <asp:TextBox ID="txtDateFrom" AutoPostBack="true" runat="server" CssClass="form-control" OnTextChanged="txtDateFrom_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateFrom" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateFrom" PopupPosition="TopLeft" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDateFrom" ValidationGroup="Save" runat="server" ErrorMessage="Enter from date"></asp:RequiredFieldValidator>

                                </td>
                            </tr>

                            <tr>
                                <td>Date To<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDateTo" AutoPostBack="true" runat="server" CssClass="form-control" OnTextChanged="txtDateTo_TextChanged"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateTo" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateTo" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDateTo" ValidationGroup="Save" runat="server" ErrorMessage="Enter to date"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Store Name<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddStore" runat="server" Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddStore_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <%--<td>Voucher Number<span class="required">*</span></td>--%>
                                <td>
                                    <asp:Label ID="lblSirLVRV" runat="server" class="required">*</asp:Label>
                                </td>
                                
                                <td>
                                    <asp:DropDownList ID="ddVoucherNumber" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddVoucherNumber_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddVoucherNumber" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter product"></asp:RequiredFieldValidator>
                                </td>
                            </tr>


                            <tr style="background: none">
                                <td></td>
                                <td class="form-actions">
                                    <asp:Button ID="btnSearch" CssClass="btn btn-primary pull-right" runat="server" Text="Search" OnClick="btnSearch_OnClick" OnClientClick="SetTarget();"/>
                                    <%--<asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" />--%>
                                    
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>

            <div class="col-lg-12 hidden">
                <section class="panel">
                    <fieldset>
                        <legend>SIR Register</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="280%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" AllowPaging="True" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"]) %>' OnPageIndexChanging="GridView1_OnPageIndexChanging" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical">
                                <Columns>

                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                           <%-- <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("StockRegID") %>'></asp:Label>--%>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:BoundField DataField="OpeningBalance" HeaderText="SIR Number" SortExpression="OpeningBalance" />
                                    <asp:BoundField DataField="StockIn" HeaderText="Description of Goods" SortExpression="StockIn" />
                                    <asp:BoundField DataField="StockInCashMemoChallanNo" HeaderText="Qty" SortExpression="StockInCashMemoChallanNo" />
                                    <asp:BoundField DataField="Total" HeaderText="Remarks" SortExpression="Total" />
                                    
                                    
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="Server">
    
</asp:Content>

