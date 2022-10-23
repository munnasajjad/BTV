<%@ Page Title="Product Received" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="ProductReceived.aspx.cs" Inherits="app_ProductReceived" %>

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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            <div class="col-lg-6">
                <section class="panel">

                    <fieldset>
                        <legend>Product Received</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdnRpID" />
                                </td>
                            </tr>
                            <tr>
                                <td>Store Name</td>
                                <td>
                                    <asp:DropDownList ID="ddlStore" runat="server" Width="100%" CssClass="form-control select2me"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td>Reapir No<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddRepair" Width="100%" CssClass="form-control select2me" AutoPostBack="true" OnSelectedIndexChanged="ddRepair_SelectedIndexChanged" AppendDataBoundItems="true" runat="server"></asp:DropDownList>
                                    <%--<asp:TextBox ID="txtReapirId" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                    <%--<asp:FilteredTextBoxExtender ID="ftbReapirId" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtReapirId" />--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Select Repair No" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddRepair" ValidationGroup="Save" InitialValue="0"></asp:RequiredFieldValidator>
                                    </td>
                            </tr>
                            <tr>
                                <td>Receive Date<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtReceiveDate" placeholder="dd/MM/yyyy" AutoPostBack="true" OnTextChanged="txtReceiveDate_TextChanged" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceApprovedDate" runat="server" Format="dd/MM/yyyy" TargetControlID="txtReceiveDate" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReceiveDate" ValidationGroup="Save" runat="server" ErrorMessage="Enter Receive date"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td>Receive No<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtReceiveNo" runat="server" AutoPostBack="true" OnTextChanged="txtReceiveNo_TextChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReceiveNo" ValidationGroup="Save" runat="server" ErrorMessage="Enter repair no"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            
                           
                            <tr>
                                <td colspan="2">
                                    <div class="col-lg-12">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" CssClass="table table-bordered" Width="100%" ID="productGrid" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="#SL" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                            <asp:Label ID="lblProductRepairDetailsID" Visible="false" runat="server" Text='<%# Bind("ProductRepairDetailsID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>
                                                   
                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   

                                                    

                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </td>
                            </tr>

                             <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>


                            <tr>
                                <td>Upload<span class="required">*</span></td>
                                <td>

                                    <asp:FileUpload ID="document" runat="server" CssClass="form-control" />

                                </td>
                            </tr>
                            <tr>
                                <td>Received by<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtReceivedby" runat="server" CssClass="form-control"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtReceivedby" ValidationGroup="Save" runat="server" ErrorMessage="Enter Received by"></asp:RequiredFieldValidator>

                                    </td>
                            </tr>


                            

                            <tr>
                                <td>Remarks<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Rows="2" runat="server" CssClass="form-control"></asp:TextBox>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtRemarks" ValidationGroup="Save" runat="server" ErrorMessage="Enter Remarks"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" ValidationGroup="Save" Text="Save" OnClick="btnSave_OnClick" />
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
                            <asp:GridView Width="120%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
                                GridLines="Vertical" DataKeyNames="ProductReceiveID" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                <Columns>
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
                                    <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>.
                                            <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("ProductReceiveID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ReceiveNo" HeaderText="Receive No" SortExpression="ReceiveNo" />
                                    <asp:BoundField DataField="Receivedby" HeaderText="Received by" SortExpression="Receivedby" />
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
                                    <asp:BoundField DataField="EntryBy" HeaderText="Entry By" SortExpression="EntryBy" />
                                    
                                    <asp:TemplateField HeaderText="Document">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink2" Width="100%" runat="server" Target="_blank" NavigateUrl='<%#Eval("DocumentURL") %>'>
                                                <asp:Label ID="lblDocument" runat="server" Text='<%# Bind("DocumentURL") %>'></asp:Label>
                                            </asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
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





