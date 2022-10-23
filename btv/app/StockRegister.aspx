<%@ Page Title="Stock Register" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="StockRegister.aspx.cs" Inherits="app_StockRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        @media (min-width: 1025px)  {
            .panel{
                  min-height: 320px !important;
            }
          
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

                            <tr class="hidden">
                                <td>Transaction Type<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddType" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddType" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter transaction type"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>Date From<span class="required">*</span></td>
                                <td>
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
                                <td>Product Name<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddProductID" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddProductID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddProductID" InitialValue="0" ValidationGroup="Add" runat="server" ErrorMessage="Enter product"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            



                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSearch" CssClass="btn btn-primary pull-right" runat="server" Text="Search" OnClick="btnSearch_OnClick" />
                                    <%--<asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" />--%>
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
                                    <%--<asp:BoundField DataField="StockRegIndexID" HeaderText="Stock Reg Index I D" SortExpression="StockRegIndexID" />--%>
                                    <%--<asp:BoundField DataField="ProductDescription" HeaderText="Product Description" SortExpression="ProductDescription" />--%>
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:BoundField DataField="OpeningBalance" HeaderText="Previous Stock In" SortExpression="OpeningBalance" />
                                    <asp:BoundField DataField="StockIn" HeaderText="Receipts" SortExpression="StockIn" />
                                    <asp:BoundField DataField="StockInCashMemoChallanNo" HeaderText="C.Memo Challan No(In)" SortExpression="StockInCashMemoChallanNo" />
                                    <asp:BoundField DataField="Total" HeaderText="Total" SortExpression="Total" />
                                    <asp:BoundField DataField="StockOutCashMemoChallanNo" HeaderText="C.Challan No(Out)" SortExpression="StockOutCashMemoChallanNo" />
                                    <asp:BoundField DataField="Issued" HeaderText="Issued Qty" SortExpression="Issued" />
                                    <asp:BoundField DataField="Balance" HeaderText="Remaining" SortExpression="Balance" />
                                    <asp:BoundField DataField="UnitName" HeaderText="Unit" SortExpression="UnitName" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                    <%--<asp:BoundField DataField="PriceLetterNo" HeaderText="Price Letter No" SortExpression="PriceLetterNo" />--%>
                                    <%--<asp:BoundField DataField="TotalPrice" HeaderText="Total Price" SortExpression="TotalPrice" />--%>
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False" Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                            <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                                            <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                            <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                <b style="color: red">This entry will be deleted permanently!</b><br />
                                                Are you sure you want to delete this ?<br />
                                                <br />
                                                <div style="text-align: right;">
                                                    <asp:Button ID="ButtonOk" runat="server" CssClass="btn btn-success" Text="OK" />
                                                    <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                </div>
                                            </asp:Panel>
                                        </ItemTemplate>
                                    </asp:TemplateField>
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





