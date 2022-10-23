<%@ Page Title="Product" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="Product.aspx.cs" Inherits="app_Product" %>

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
                        <legend>Product</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Category</td>
                                <td>
                                    <asp:DropDownList ID="ddProductCategoryID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddProductCategoryID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Product Sub Category</td>
                                <td>
                                    <asp:DropDownList ID="ddProductSubCategory" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddProductSubCategory_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Unit</td>
                                <td>
                                    <asp:DropDownList ID="ddUnitID" runat="server" Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="False" OnSelectedIndexChanged="ddUnitID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>Name<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtName" ValidationGroup="Save" runat="server" ErrorMessage="Enter name"></asp:RequiredFieldValidator>

                                </td>
                            </tr>








                            <tr class="hidden">
                                <td>Product Sub Category</td>
                                <td>
                                    <asp:TextBox ID="txtProductSubCategoryID" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbProductSubCategoryID" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtProductSubCategoryID" />
                                </td>
                            </tr>



                            <tr>
                                <td>Description</td>
                                <td>
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Stock Book</td>
                                <td>
                                    <asp:TextBox ID="txtStockBookID" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbStockBookID" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtStockBookID" />
                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Min Stock Qty</td>
                                <td>
                                    <asp:TextBox ID="txtMinStockQty" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbMinStockQty" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtMinStockQty" />
                                </td>
                            </tr>


                            <tr>
                                <td>Reorder alert Qty<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtMinOrderQTy" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="ftbMinOrderQTy" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtMinOrderQTy" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMinOrderQTy" ValidationGroup="Save" runat="server" ErrorMessage="Enter reorder alert quentiy"></asp:RequiredFieldValidator>

                                </td>
                            </tr>

                             <tr>
                                <td>Product Type</td>
                                <td>
                                    <asp:DropDownList ID="ddProductType" Width="100%" runat="server" CssClass="form-control select2me">
                                       
                                    </asp:DropDownList>

                                </td>
                            </tr>


                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" ValidationGroup="Save" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>

            <div class="col-lg-6">
                <section class="panel">
                    <fieldset>
                        <legend>Saved Data</legend>
                        <div class="table-responsive">
                            <asp:GridView Width="180%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="ProductID" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                <Columns>

                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("ProductID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                    <asp:BoundField DataField="UnitName" HeaderText="Unit" SortExpression="UnitID" />
                                    <asp:BoundField DataField="CategoryName" HeaderText="Product Category" SortExpression="ProductCategoryID" />
                                    <asp:BoundField DataField="SubCategoryName" HeaderText="Sub Category" SortExpression="ProductSubCategoryID" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                    <asp:BoundField DataField="StockBookID" HeaderText="Stock Book I D" SortExpression="StockBookID" Visible="False" />
                                    <asp:BoundField DataField="MinStockQty" HeaderText="Min Stock Qty" SortExpression="MinStockQty" Visible="False" />
                                    <asp:BoundField DataField="MinOrderQTy" HeaderText="Reorder Qty" SortExpression="MinOrderQTy" />
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





