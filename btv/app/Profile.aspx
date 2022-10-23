<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="app_Profile" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .portlet-body table tr:nth-child(odd) {
    background: transparent !important;
}img#ctl00_ContentPlaceHolder1_imgPhoto {
    float: right;
    margin-right: 7px;
    border-radius: 8px;
}
input[type=password], input#ctl00_body_ChangePassword1_ChangePasswordContainerID_CurrentPassword {
    font-size: 40px;
    font-family: Arial, Helvetica, sans-serif;
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


    <div class="row">
        <div class="col-md-12">
            <h3 class="page-title">User Profile</h3>
        </div>
    </div>

    <div class="row">
        
    <asp:UpdatePanel ID="pnl" runat="server">
         <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

                <div class="col-md-6 ">
                    <div class="portlet box">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Personal Info
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblId" runat="server" Visible="False"></asp:Label>

                                <div class="control-group">
                                    <label>Login ID:</label>
                                    <asp:TextBox ID="txtCode" runat="server" ReadOnly="True"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label>Name:</label>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                </div>
                                
                                <div class="control-group">
                                    <label>Designation:</label>
                                    <asp:TextBox ID="txtDesignation" runat="server"></asp:TextBox>
                                </div>
                                
                                <div class="control-group">
                                    <asp:Label ID="lblPhoto" runat="server" Text="Photo: "></asp:Label>
                                    <asp:FileUpload ID="FileUpload2" runat="server" ClientIDMode="Static" CssClass="form-control" Width="45%" />
                                    <asp:Image ID="imgPhoto" runat="server" Width="60px" />
                                </div>

                                <div class="control-group">
                                    <label>Address: </label>
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" Height="60px" TextMode="MultiLine"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label>Date of Birth:</label>
                                    <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDOB"></asp:CalendarExtender>
                                </div>

                                <div class="control-group">
                                    <label>Mobile No:</label>
                                    <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                                </div>

                                <div class="control-group">
                                    <label>Email Address:</label>
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                </div>
                                

                                <div class="form-actions">
                                    <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Save" OnClick="btnSave_OnClick" />
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>



        <div class="col-md-6 ">
            <div class="portlet box">
                <div class="portlet-title">
                    <div class="caption">
                        <i class="fa fa-reorder"></i>Change Login Password
                    </div>
                </div>
                <div class="portlet-body form">
                    <div class="form-body">
                        <asp:ChangePassword ID="ChangePassword1" runat="server" CancelDestinationPageUrl="Default.aspx" ContinueDestinationPageUrl="Default.aspx" Width="100%">
                            <ChangePasswordTemplate>

                                <div class="control-group">
                                    <label>Current Password:</label>

                                    <asp:TextBox ID="CurrentPassword" runat="server" class="tip_top" title="Current password" TextMode="Password" Width="40%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="CurrentPasswordRequired" runat="server"
                                        ControlToValidate="CurrentPassword" ErrorMessage="Password is required."
                                        ToolTip="Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                </div>

                                <div class="control-group">
                                    <label>New Password:</label>
                                    <asp:TextBox ID="NewPassword" runat="server" title="New password" TextMode="Password" Width="40%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="NewPasswordRequired" runat="server"
                                        ControlToValidate="NewPassword" ErrorMessage="New Password is required."
                                        ToolTip="New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                </div>

                                <div class="control-group">
                                    <label>Confirm new Password: </label>
                                    <asp:TextBox ID="ConfirmNewPassword" runat="server" title="Confirm password" TextMode="Password" Width="40%"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="ConfirmNewPasswordRequired" runat="server"
                                        ControlToValidate="ConfirmNewPassword"
                                        ErrorMessage="Confirm New Password is required."
                                        ToolTip="Confirm New Password is required." ValidationGroup="ChangePassword1">*</asp:RequiredFieldValidator>
                                </div>

                                    <asp:CompareValidator ID="NewPasswordCompare" runat="server"
                                        ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                        Display="Dynamic"
                                        ErrorMessage="The Confirm New Password must match the New Password entry."
                                        ValidationGroup="ChangePassword1"></asp:CompareValidator>

                                    <span align="center" style="color: Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </span>
                               
                                

                                <div class="form-actions">

                                    <asp:Button ID="ChangePasswordPushButton" runat="server"
                                        CommandName="ChangePassword" OnClick="ChangePasswordPushButton_Click" Text="Change Password"
                                        ValidationGroup="ChangePassword1" />
                                    <asp:Button ID="CancelPushButton" runat="server" CausesValidation="False"
                                        Visible="false" CommandName="Cancel" Text="Cancel" />
                                </div>



                            </ChangePasswordTemplate>
                        </asp:ChangePassword>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>

