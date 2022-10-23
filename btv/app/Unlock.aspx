<%@ Page Title="Unlock" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Unlock.aspx.cs" Inherits="app_Unlock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .portlet-body table tr:nth-child(odd) {
            background: transparent !important;
        }

        img#ctl00_ContentPlaceHolder1_imgPhoto {
            float: right;
            margin-right: 7px;
            border-radius: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Block User</h3>
                </div>
            </div>

            <div class="row">

                <div class="col-md-6 ">
                    <div class="portlet box">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Block/ Unblock User Accounts
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">
                                <asp:Label ID="lblStatus" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblId" runat="server" Visible="False"></asp:Label>

                                <asp:Label ID="lblUser" runat="server" Text="" Visible="False"></asp:Label>
                                <asp:Label ID="lblProjectID" runat="server" Visible="False"></asp:Label>

                                <div class="control-group">
                                    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="g" Text="Block a user" OnCheckedChanged="RadioButton2_CheckedChanged" AutoPostBack="true" />
                                    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="g" Text="Unblock a User" OnCheckedChanged="RadioButton2_CheckedChanged" AutoPostBack="true" Checked="true" />
                                    <%--<asp:RadioButton ID="RadioButton3" runat="server" GroupName="g" Text="Unblock All" Checked="True" />--%>
                                </div>

                                <div class="form-group">
                                    <label class="field_title">User ID :</label>
                                    <asp:DropDownList ID="ddUsers" runat="server" AutoPostBack="True" CssClass="control-group select2me" Width="70%" OnSelectedIndexChanged="ddUsers_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT aspnet_Users.UserName FROM aspnet_Membership INNER JOIN aspnet_Users ON aspnet_Membership.UserId = aspnet_Users.UserId WHERE (aspnet_Membership.IsLockedOut = 'true') order by [UserName]">
                                        <SelectParameters>
                                             <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                            <asp:ControlParameter Name="LoginUserName" ControlID="lblUser" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>

                                
                                
                                <div class="form-actions">
                                    <asp:Button ID="btnSave" runat="server" Text="Unlock All Users" OnClick="btnSave_Click" />
                                </div>
                            </div>
                        </div>
                        </div>
                </div>


                <div class="col-md-6 ">
                    <!-- BEGIN SAMPLE FORM PORTLET-->
                    <div class="portlet box green ">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                List of Blocked Users
                            </div>
                            <div class="tools">
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">

                                    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1">
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT UserName, Email, LastLockoutDate FROM vw_aspnet_MembershipUsers  WHERE IsLockedOut = 1 AND UserName IN (SELECT [LoginUserName] FROM [Logins])">
                                        <SelectParameters>
                                             <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                            <asp:ControlParameter Name="LoginUserName" ControlID="lblUser" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>




                                </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>




</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>
