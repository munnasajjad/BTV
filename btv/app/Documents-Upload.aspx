<%@ Page Title="Documents-Upload" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Documents-Upload.aspx.cs" Inherits="app_Documents_Upload" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>
        div#ctl00_BodyContent_ThumbUpload_ctl01 {
    background: none!important;
}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">        </asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server">
        <ProgressTemplate>
        <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
            <img src="../images/loader.gif" alt="Processing... Please Wait." />
        </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

             <script type="text/javascript" language="javascript">
                 Sys.Application.add_load(callJquery);
            </script>

            <div class="container-fluid">
                <div class="row-fluid">
                    <div class="span12">
                        <h3 class="page-title">Company Documents</h3>
                    </div>
                </div>
                
                <div class="row-fluid">
            <div class="span12">
                <!-- BEGIN SAMPLE FORM PORTLET-->
                <div class="portlet box blue">
                    
                    <div class="portlet-body form">

                        <div class="control-group">
                            <label class="control-label">Document Name</label>
                            <div class="controls">
                                <asp:TextBox ID="txtName" CssClass="span6 m-wrap" runat="server"></asp:TextBox>
                                <%--<input type="text" class="span6 m-wrap tooltips" data-trigger="hover" data-original-title="Tooltip text goes here. Tooltip text goes here." />    --%>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Document Description</label>
                            <div class="controls">
                                <asp:TextBox ID="txtDescription" CssClass="span6 m-wrap" runat="server" TextMode="MultiLine" Height="100px" Width="70%"></asp:TextBox>
                                <%--<input type="text" class="span6 m-wrap tooltips" data-trigger="hover" data-original-title="Tooltip text goes here. Tooltip text goes here." />    --%>
                            </div>
                        </div>
                        <div class="control-group">
                            <label class="control-label">Select File to Upload</label>
                            <div class="controls">
                                 <asp:FileUpload ID="ThumbUpload" runat="server" ClientIDMode="Static" />
                            </div>
                        </div>


                        <asp:Label ID="lblOrderID" runat="server" Visible="False"></asp:Label>
                        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" CssClass=""></asp:Label>
                        <asp:Label ID="lblProject" runat="server" Visible="false" />
                        <div class="form-actions">
                            <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_Click" OnClientClick="return validate();" />
                            
                        </div>

                    </div>

                </div>

            </div>

        </div>



        <div class="row-fluid">
            <div class="span12">
                <div class="portlet box yellow">
                    <%--<div class="portlet-title">
                        <h4><i class="icon-coffee"></i>Saved Data</h4>
                        <div class="tools">
                            <a href="javascript:;" class="collapse"></a>
                            <a href="#portlet-config" data-toggle="modal" class="config"></a>
                            <a href="javascript:;" class="reload"></a>
                            <a href="javascript:;" class="remove"></a>
                        </div>
                    </div>--%>
                    <div class="portlet-body">
                        
                         <div class="control-group">
                            <label class="control-label">Search Uploaded Files </label>
                            <div class="controls">
                                <asp:TextBox ID="txtSearch" CssClass="span6 m-wrap" runat="server" AutoPostBack="True" OnTextChanged="txtSearch_OnTextChanged"></asp:TextBox>
                                <%--<input type="text" class="span6 m-wrap tooltips" data-trigger="hover" data-original-title="Tooltip text goes here. Tooltip text goes here." />    --%>
                            </div>
                        </div>

                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="False"  
                            AllowSorting="True" PageSize="50" AllowPaging="True" DataKeyNames="Id" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
                            <Columns>
                                 <asp:TemplateField ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                        <asp:Label ID="lblEntryId" runat="server" CssClass="hidden" Text='<%# Bind("Id") %>'></asp:Label>
                                                    </ItemTemplate><ItemStyle Width="20px"></ItemStyle>
                                                </asp:TemplateField>
                                <asp:BoundField DataField="BusNo" HeaderText="Document Name" SortExpression="BusNo" />
                                <asp:BoundField DataField="ImgDetail" HeaderText="Document Description" SortExpression="ImgDetail" />
                                <asp:TemplateField HeaderText="Document File" SortExpression="Img">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Img") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLink1" Target="_blank" runat="server" Text="Open File"
                                            NavigateUrl='<%#"./" + Eval("Img") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Select" Visible="False" />
                                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.png" Text="Delete" ToolTip="Delete" />
                                                        
                                                        <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                        </asp:ConfirmButtonExtender>
                                                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                            PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                        <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                            <b style="color: red">Item will be deleted permanently!</b><br />
                                                            Are you sure you want to delete the item from order list?
                                                            <br />
                                                            <br />
                                                            <div style="text-align: right;">
                                                                <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                                <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel"  />
                                                            </div>
                                                        </asp:Panel>

                                                    </ItemTemplate>
                                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                            SelectCommand="SELECT [BusNo], [ImgDetail], [Img] FROM [ImportentDocuments] WHERE DocType='Company' AND (BusNo Like '%'+@BusNo+'%' OR ImgDetail Like '%'+@ImgDetail+'%') Where (ProjectId = @ProjectId) ORDER BY [EntryDate] DESC">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtSearch" Name="BusNo" PropertyName="Text" />
                                <asp:ControlParameter ControlID="txtSearch" Name="ImgDetail" PropertyName="Text" />
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
