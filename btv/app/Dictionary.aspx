<%@ Page Title="Dictionary" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Dictionary.aspx.cs" Inherits="app_Dictionary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="uPanel" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>


            <div class="col-lg-6">
                <section class="panel">

                    <legend>Translations</legend>
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>
                    <br/>
                         <div class="control-group">
                                <asp:Label ID="lblDeptName" runat="server" Text="English: "></asp:Label>
                             <div class="controls">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="50%"></asp:TextBox>
                                 <asp:Button ID="Button2" runat="server" Text="Search" CssClass="button blue"  OnClick="Button2_OnClick" />
                           </div>
                       </div>
                        <div class="control-group">
                                <asp:Label ID="lblDid" runat="server" Text="বাংলা : "></asp:Label>
                             <div class="controls">
                                <asp:TextBox ID="txtDescription" runat="server"  CssClass="form-control" Width="50%"></asp:TextBox>
                                 <asp:Button ID="Button1" runat="server" Text="Search" CssClass="button blue"  OnClick="Search" />
                          </div>
                       </div>
                    
                        <div class="control-group">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button blue"  OnClick="btnSave_Click" />
                                
                          </div>

                    
                </section>
            </div>




            <div class="col-lg-6">
                <section class="panel">

                            <fieldset>
                                <legend>Search Result</legend>

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"  DataKeyNames="id" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting"  BackColor="White" BorderColor="#DEDFDE"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" GridLines="Vertical">
                                    <Columns>
                                         <asp:TemplateField ItemStyle-Width="40px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="40px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="English">
                                             <ItemTemplate>
                                                 <asp:Label ID="Label5" runat="server" Text='<%# Bind("english") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>
                                        <asp:TemplateField HeaderText="বাংলা">
                                             <ItemTemplate>
                                                 <asp:Label ID="Label6" runat="server" Text='<%# Bind("bangla") %>'></asp:Label>
                                             </ItemTemplate>
                                         </asp:TemplateField>

                                         <asp:TemplateField ItemStyle-Width="50px" ShowHeader="False">
                                             <ItemTemplate>
                                                 <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                                 <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                                                 <asp:ConfirmButtonExtender ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1" TargetControlID="ImageButton2">
                                                 </asp:ConfirmButtonExtender>
                                                 <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" BackgroundCssClass="modalBackground" CancelControlID="ButtonCancel" OkControlID="ButtonOk" PopupControlID="PNL" TargetControlID="ImageButton2" />
                                                 <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                     <b style="color: red">This entry will be deleted permanently!</b><br /> Are you sure you want to delete this ?
                                                     <br />
                                                     <br />
                                                     <div style="text-align: right;">
                                                         <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                         <asp:Button ID="ButtonCancel" runat="server" CssClass="btn_small btn_orange" Text="Cancel" />
                                                     </div>
                                                 </asp:Panel>
                                             </ItemTemplate>
                                             <ItemStyle Width="90px" />
                                         </asp:TemplateField>


                                    </Columns>
                                    <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                    <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                                    <AlternatingRowStyle BackColor="White" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT Name, Description, Agents, Vendors, Customers, Products FROM TargetIndustries Where (ProjectId = @ProjectId) ORDER BY [Name]"
                                    DeleteCommand="Delete TargetIndustries where id='0'" >
                                            <SelectParameters>
		        <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
        </SelectParameters>
                                    </asp:SqlDataSource>

                </section>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
