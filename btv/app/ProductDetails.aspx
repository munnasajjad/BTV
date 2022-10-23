<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="ProductDetails.aspx.cs" Inherits="app_ProductDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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

            <div class="col-lg-6">
                <section class="panel">

                    <fieldset>
                        <legend>Product Details</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="idHiddenField" />
                                </td>
                            </tr>


                            <tr>
                                <td>GRN No</td>
                                <td>
                                    <asp:DropDownList ID="ddGrnNO" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddGrnNO_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>Product Name</td>
                                <td>
                                    <asp:DropDownList ID="ddProductID" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddProductID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Item Quantity</td>
                                <td>
                                    <asp:TextBox ID="txtItemQuantity" ReadOnly="True" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Brand Name</td>
                                <td>
                                    <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Model No</td>
                                <td>
                                    <asp:TextBox ID="txtModelNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Serial No</td>
                                <td>
                                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Part No</td>
                                <td>
                                    <asp:TextBox ID="txtPartNo" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td>Country Of Origin</td>
                                <td>
                                    <asp:DropDownList ID="ddlCountry" runat="server" CssClass="form-control select2me" Width="100%"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txtCountryOfOrigin" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Manufacturing Country</td>
                                <td>
                                    <asp:DropDownList ID="ddlManufactureCountry" runat="server" CssClass="form-control select2me" Width="100%"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txtManufacturingCompany" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>
                            <tr class="hidden">
                                <td>Product Condition Status</td>
                                <td>
                                    <asp:TextBox ID="txtProductConditionStatus" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="hidden">
                                <td>Product Status</td>
                                <td>
                                    <asp:TextBox ID="txtProductStatus" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Manufacture Date</td>
                                <td>
                                    <asp:TextBox ID="txtManufactureDate" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="cetxtManufactureDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtManufactureDate" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Guaranty Period</td>
                                <td>
                                    <asp:TextBox ID="txtGuarantyPeriod" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceGuarantyPeriod" runat="server" Format="dd/MM/yyyy" TargetControlID="txtGuarantyPeriod" />
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnAdd" CssClass="btn btn-primary" runat="server" Text="ADD" OnClick="btnAdd_OnClick" />
                                </td>
                            </tr>
                            <tr>

                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" Width="100%" ID="AddItemsGridView" CssClass="table table-bordered" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical" OnSelectedIndexChanged="AddItemsGridView_OnSelectedIndexChanged" OnRowDeleting="AddItemsGridView_OnRowDeleting">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                            <%--<asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                                            <asp:ConfirmButtonExtender TargetControlID="ImageButton4" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                                            <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton4"
                                                                PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                            <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                                <b style="color: red">Entry will be deleted!</b><br />
                                                                Are you sure, you want to delete the item from entry list?
                                                            <br />
                                                                <br />
                                                                <div style="text-align: right;">
                                                                    <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                                    <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                                </div>
                                                            </asp:Panel>--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sl." SortExpression="ProductDetailsID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductDetailsID" runat="server" Text='<%# Bind("ProductDetailsID") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="GRN No" SortExpression="GRNInvoiceNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblGRNInvoiceNo" runat="server" Text='<%# Bind("GRNInvoiceNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Countries Of Origin" SortExpression="CountriesOfOrigin">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCountriesOfOrigin" runat="server" Text='<%# Bind("CountriesOfOrigin") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    
                                                    <asp:TemplateField HeaderText="Manufacturing Company" SortExpression="ManufacturingCompany">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblManufacturingCompany" runat="server" Text='<%# Bind("ManufacturingCompany") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Manufacture Date" SortExpression="ManufactureDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblManufactureDate" runat="server" Text='<%# Bind("ManufactureDate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Part No" SortExpression="PartNo">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPartNo" runat="server" Text='<%# Bind("PartNo") %>'></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Brand" SortExpression="Brand">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblBrand" runat="server" Text='<%# Bind("Brand") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>Store Name</td>
                                <td>
                                    <asp:DropDownList ID="ddStore" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>--%>


                            <tr style="background: none" class="hidden">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>

            <div class="col-lg-6 hidden">
                <section class="panel">
                    <fieldset>
                        <legend>Saved Data</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="280%" ID="SaveItemGridView" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" OnSelectedIndexChanged="SaveItemGridView_OnSelectedIndexChanged" OnRowDeleting="SaveItemGridView_OnRowDeleting">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                            <asp:Label ID="lblGrnNO" runat="server" Visible="False" Text='<%# Bind("GrnNO") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <%--                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False">
                                        <ItemTemplate>
                                           
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>


                                    <asp:BoundField DataField="GRNInvoiceNo" HeaderText="Grn NO" SortExpression="GrnNO" />
                                    <asp:BoundField DataField="Name" HeaderText="Store Name" SortExpression="Name" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False">
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
                                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
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





