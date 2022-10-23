<%@ Page Title="FormUserLevelSecurity" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="FormUserLevelSecurity.aspx.cs" Inherits="app_FormUserLevelSecurity" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        #ctl00_ContentPlaceHolder1_cblForms,  #ctl00_ContentPlaceHolder1_cblForms label {
            width: 70%;
        }

    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>

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


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Form Level Security</h3>
                </div>
            </div>

            <div class="row">

                <div class="col-md-5">
                    <div class="portlet box">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Form Level Security
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblID" runat="server" Visible="false"></asp:Label>
                                <div class="control-group">
                                    <label>Login Id: </label>

                                    <asp:DropDownList ID="ddUser" runat="server" AutoPostBack="True"
                                        DataSourceID="SqlDataSource4" AppendDataBoundItems="True"
                                        DataTextField="LoginUserName" DataValueField="LoginUserName" OnSelectedIndexChanged="ddUser_OnSelectedIndexChanged"></asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [LoginUserName] FROM [Logins] Where (ProjectId = @ProjectId) AND [LoginUserName]<>'rony' order by [LoginUserName]">
                                        <SelectParameters>
	                                         <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="control-group">
                                    <label class="field_title">Form Group :</label>
                                    <asp:DropDownList ID="ddFormGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddFormGroup_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT DISTINCT [MenuGroup] FROM [MenuSubGroup] WHERE ([show] = @show) ">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="1" Name="show" Type="Int32" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="control-group">
                                    <label class="field_title">Form Sub-Group :</label>
                                    <asp:DropDownList ID="ddSubGroup" runat="server"
                                        DataSourceID="SqlDataSource2" DataTextField="MenuSubGroup" DataValueField="sl" AutoPostBack="True" OnSelectedIndexChanged="ddSubGroup_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [sl], [MenuSubGroup] FROM [MenuSubGroup] WHERE ([show] =1) AND [MenuGroup]=@MenuGroup ">
                                        <SelectParameters>
                                            <asp:ControlParameter Name="MenuGroup" ControlID="ddFormGroup" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <label>Form Name (Title)</label>
                                   <asp:CheckBoxList runat="server" ID="cblForms"/>

                                </div>

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="btn_small btn_blue" OnClick="btnSave_Click" />
                                    <asp:Button ID="Button2" runat="server" Text="Clear Form" class="btn_small btn_orange" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-7">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                List of Blocked Items
                            </div>
                            <div class="tools">
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">

                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table"
                                        DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="SL"
                                        OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">

                                        <Columns>

                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>


                                            <asp:TemplateField HeaderText="sl" SortExpression="sl" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("SL") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="MenuGroup" HeaderText="Menu Group" SortExpression="LoginUserName" ReadOnly="True" />
                                            <asp:BoundField DataField="MenuSubGroup" HeaderText="Menu SubGroup" SortExpression="EmployeeInfoID" />
                                            <asp:BoundField DataField="FormName" HeaderText="Form Name" SortExpression="UserLevel" />

                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/Images/edit.png" Text="Select" />
                                                    <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/Images/delete.gif" Text="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                    </asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">This entry will be deleted permanently!</b><br />
                                                        Are you sure you want to delete this ?
                                                            <br />
                                                        <br />
                                                        <div style="text-align: right;">
                                                            <asp:Button ID="ButtonOk" runat="server" Text="OK" />
                                                            <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                        </div>
                                                    </asp:Panel>

                                                </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT SL, MenuGroup, (Select MenuSubGroup from MenuSubGroup where sl=FormAccessSecurity.MenuSubGroup) as MenuSubGroup, (Select FormName from MenuStructure WHERE sl=FormAccessSecurity.MenuItemID) AS FormName FROM [FormAccessSecurity] where (ProjectId = @ProjectId) AND  UserID=@UserID ORDER BY MenuItemID"
                                        DeleteCommand="delete Logins where lid=0">
                                        <SelectParameters>
                                            <asp:ControlParameter Name="UserID" ControlID="ddUser" PropertyName="SelectedValue" />
                                            <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                    <asp:Label ID="lblUser" runat="server" Text="" Visible="False"></asp:Label>
                                    <asp:Label ID="lblProjectID" runat="server" Visible="False"></asp:Label>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>


