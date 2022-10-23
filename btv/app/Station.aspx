<%@ Page Title="Main Office" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="Station.aspx.cs" Inherits="app_Station" %>

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
                        <legend>Create Main Office</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>


                            <tr>
                                <td>Office Name<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtName" ValidationGroup="Save" runat="server" ErrorMessage="Enter office name"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td>Location<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="2" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAddress" ValidationGroup="Save" runat="server" ErrorMessage="Enter location"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Category</td>
                                <td>
                                    <asp:RadioButton ID="rdIsStation" Visible="False" runat="server" Checked="true" GroupName="station" Text="Is station" />
                                    <asp:RadioButton ID="rdIsSubStation" Visible="False" runat="server" GroupName="station" Text="Is a sub station" />

                                    <asp:DropDownList ID="ddOffice" runat="server" Width="100%" CssClass="form-control select2me">
                                        <asp:ListItem Value="Station">Station</asp:ListItem>
                                        <asp:ListItem Value="Sub Station">Sub Station</asp:ListItem>
                                        <asp:ListItem Value="Head Office">Head Office</asp:ListItem>
                                        <asp:ListItem Value="Project">Project</asp:ListItem>
                                        <asp:ListItem Value="Other">Other</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" ValidationGroup="Save" CausesValidation="true" runat="server" Text="Save" OnClick="btnSave_OnClick" />
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
                        <div class="col-md-12">
                             <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AllowPaging="true" PageSize="20" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                            BorderStyle="None" BorderWidth="1px" ForeColor="Black"
                            GridLines="Vertical" DataKeyNames="LocationID" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                            <Columns>

                                <asp:TemplateField HeaderText="#Sl" Visible="false" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>.
                                        <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("LocationID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Office Name" SortExpression="Name" />
                                <asp:BoundField DataField="Address" HeaderText="Location" SortExpression="Address" />
                                <asp:BoundField DataField="Status" HeaderText="Category" SortExpression="Status" />
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
                            <PagerStyle CssClass="gridview" HorizontalAlign="Left" />
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





