<%@ Page Title="Notice-Board" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Notice-Board.aspx.cs" Inherits="app_Notice_Board" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>

            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Notice Board</h3>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Post New Notice
                            </div>                            
                        </div>
                        <div class="portlet-body form">                            
                            <div class="form-body">
                                <asp:Label ID="lblProject" runat="server" Visible="false" />
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false" ></asp:Label>
                                <asp:Label ID="lblId" runat="server" Visible="False"></asp:Label>

                               <%--<div class="form-group">
                                    <label>Test Categories Name:  </label>
                                    <asp:TextBox ID="txtTestName" runat="server" CssClass="form-control"   placeholder="Enter Test Name" Height="200px" />
                               
                                </div>--%>

                                <div class="form-group">
                                    <label>Type Notice: </label>
                                    <asp:TextBox ID="txtFullNews" runat="server" CssClass="form-control" Height="200px" TextMode="MultiLine" cols="20" Wrap="True" />
                                    <small style="padding-left: 35%; text-align: center; color: green"><i>Latest updated news will scroll on home page.</i></small>
                                </div>  

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click1" />
                                    <asp:Button ID="btnClear" CssClass="btn default" runat="server" Text="Cancel" OnClick="btnClear_Click1" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>




                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Saved Notices
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">

                                <div class="table-responsive">

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                    DataSourceID="SqlDataSource1" Width="100%"  DataKeyNames="MsgID"  OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                                    <Columns>                
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText="GradeID" SortExpression="GradeID" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("MsgID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                       
                                      <asp:BoundField DataField="FullNews" HeaderText="Notice" SortExpression="FullNews" />
                                 
                                        
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" ToolTip="Select to Edit" />
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                    </asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">Item will be deleted permanently!</b><br /><br />
                                                        Are you sure to delete the item from list?
                                                            <br /> <br /> <br/> <br/>
                                                        <div style="text-align: right;">
                                                            <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                            <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                        </div>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                                </div>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT MsgID, [FullNews], [ProjectID] FROM [NewsUpdates]  Where (ProjectId = @ProjectId) ORDER BY [MsgID] desc"
                                    DeleteCommand="Delete FROM [NewsUpdates]  where MsgID='0'">
                                     <SelectParameters>
                                        <asp:ControlParameter ControlID="lblProject" Name="ProjectId" PropertyName="Text" Type="String" />
                                     </SelectParameters>
                                </asp:SqlDataSource>

                            </div>
                        </div>
                    </div>
                </div>
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>