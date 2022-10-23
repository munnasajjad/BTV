﻿<%@ Page Title="Create Login Account" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="app_CreateUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .portlet-body table tr:nth-child(odd) {
            /*background: #ddf7e3  !important;*/
        }

        td {
            vertical-align: middle !important;
        }

        table {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>--%>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/xerp_loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>
            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">User Accounts</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Create Login Account
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>

                                <asp:Panel ID="Panel1" runat="Server" Visible="False">

                                    <div class="control-group span12 col-md-12">
                                        <label class="control-label">Login Id: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtUserX" CssClass="span12 m-wrap" runat="server" ReadOnly="True"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="ftbPurchaseOrderNo" runat="server" FilterType="Custom" InvalidChars="+?#!^*()<>{}/|=~\$& " TargetControlID="txtUserX" />
                                        </div>
                                    </div>

                                    <div class="control-group span12 col-md-12">
                                        <label class="control-label">Permission Role: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddLevelX" runat="server" CssClass="form-control">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7p" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT LevelID, LevelName FROM [UserLevel] WHERE ORDER BY [LevelID]"></asp:SqlDataSource>

                                        </div>
                                    </div>



                                    <div class="control-group span12 col-md-12">
                                        <label class="control-label">Department/Section: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddCounterX" runat="server"
                                                CssClass="form-control"
                                                DataTextField="Name"
                                                DataValueField="DepartmentSectionID">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7x" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT DepartmentSectionID, Name FROM [DepartmentSection] ORDER BY [Name]"></asp:SqlDataSource>


                                        </div>
                                    </div>

                                    <div class="control-group  span12 col-md-12">
                                        <label class="control-label">Employee Name: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddEmployeX" runat="server"
                                                CssClass="form-control"
                                                DataSourceID="SqlDataSource40x" DataTextField="Name"
                                                DataValueField="EmployeeID">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource40x" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [Name], EmployeeID FROM [Employee] where DepartmentSectionID=@DepartmentSectionID  ORDER BY [Name]">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="ddCounterX" Name="DepartmentSectionID" PropertyName="SelectedValue" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </div>
                                    </div>


                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Update" OnClick="btnSave_OnClick" />
                                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                    </div>


                                </asp:Panel>

                                <asp:Panel ID="pnlRegister" runat="Server">

                                    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" Width="100%"
                                        OnCreatedUser="CreateUserWizard1_CreatedUser" LoginCreatedUser="False" RequireEmail="False" UnknownErrorMessage="User account was not created. Please try again.">
                                        <WizardSteps>
                                            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr>
                                                            <td align="right">
                                                                <label class="control-label">Permission Role: </label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddLevel" runat="server" CssClass="form-control">
                                                                </asp:DropDownList>

                                                                <asp:SqlDataSource ID="SqlDataSource7p1" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                    SelectCommand="SELECT LevelID, LevelName FROM [UserLevel] WHERE LevelID<@LevelID ORDER BY [LevelID]"></asp:SqlDataSource>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <label>Main Office</label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddLocationID" Width="100%" runat="server" CssClass="form-control select2me"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddLocationID_SelectedIndexChanged">
                                                                </asp:DropDownList>

                                                                <asp:SqlDataSource ID="SqlDataSource3" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                    SelectCommand="Select LocationID, Name from Location ORDER BY [Name]"></asp:SqlDataSource>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <label>Functional Office: </label>
                                                            </td>
                                                            <td>

                                                                <asp:DropDownList ID="ddCenterID" Width="100%" AppendDataBoundItems="true" runat="server" CssClass="form-control select2me"
                                                                    AutoPostBack="True" OnSelectedIndexChanged="ddCenterID_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                    SelectCommand="Select CenterID, Name from Center WHERE LocationID=@LocationID">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="ddLocationID" Name="LocationID" PropertyName="SelectedValue" />
                                                                    </SelectParameters>
                                                                </asp:SqlDataSource>



                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="Label1" runat="server" AssociatedControlID="ddCounter">Department Name : </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddCounter" runat="server" AppendDataBoundItems="true" CssClass="form-control select2me"
                                                                    Width="100%" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddCounter_OnSelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:SqlDataSource ID="SqlDataSource7" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                    SelectCommand="SELECT DepartmentSectionID, Name FROM [DepartmentSection] WHERE CenterID=@CenterID">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="ddCenterID" Name="CenterID" PropertyName="SelectedValue" />
                                                                    </SelectParameters>
                                                                </asp:SqlDataSource>

                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="Label2" runat="server" AssociatedControlID="ddEmploye">Employee Name : </asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddEmploye" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddEmploye_SelectedIndexChanged" CssClass="form-control select2me">
                                                                </asp:DropDownList>

                                                                <%--  <asp:SqlDataSource ID="SqlDataSource40" runat="server"
                                                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                                    SelectCommand="SELECT [Name], EmployeeID FROM [Employee] where StoreID=@StoreID  AND EmployeeID NOT IN (Select  EmployeeInfoID from Logins) ORDER BY [Name]">
                                                                    <SelectParameters>
                                                                        <asp:ControlParameter ControlID="ddlStore" Name="StoreID" PropertyName="SelectedValue" />
                                                                    </SelectParameters>
                                                                </asp:SqlDataSource>--%>

                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Login ID:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="UserName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                                <asp:FilteredTextBoxExtender ID="ftbPurchaseOrderNo" runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars=" +?#!^*()<>{}/|=~\$&%]" TargetControlID="UserName" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="Email" runat="server" CssClass="form-control"></asp:TextBox>
                                                                <%--<asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>--%>
                                                            </td>
                                                        </tr>
                                                        <%--  <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="QuestionLabel" runat="server" AssociatedControlID="Question">Security Question:</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="Question" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="QuestionRequired" runat="server" ControlToValidate="Question" ErrorMessage="Security question is required." ToolTip="Security question is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="AnswerLabel" runat="server" AssociatedControlID="Answer">Security Answer:</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="Answer" runat="server"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="AnswerRequired" runat="server" ControlToValidate="Answer" ErrorMessage="Security answer is required." ToolTip="Security answer is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>--%>
                                                        <tr>
                                                            <td align="center" colspan="2">
                                                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match." ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" colspan="2" style="color: Red;">
                                                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:CreateUserWizardStep>
                                            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                                            </asp:CompleteWizardStep>
                                        </WizardSteps>
                                    </asp:CreateUserWizard>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="lrlSavedBox" runat="server">Saved Users</asp:Literal>
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

                                <asp:Label ID="lblid" runat="server" Visible="False"></asp:Label>

                                <asp:GridView ID="GridView1" runat="server"  AutoGenerateColumns="False" CssClass="table table table-bordered"
                                   Width="100%" DataKeyNames="LID"
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
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("LID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="LoginUserName" HeaderText="Login ID" SortExpression="LoginUserName" ReadOnly="True" />
                                        <asp:BoundField DataField="EmpName" HeaderText="Employee Name" SortExpression="EmpName" ReadOnly="True" />
                                        <%--<asp:BoundField DataField="EmployeeID" HeaderText="Employee ID" SortExpression="EmployeeID" />--%>
                                        <asp:BoundField DataField="UserLevel" HeaderText="Permission Level" SortExpression="UserLevel" />
                                        <asp:TemplateField Visible="false" ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" Visible="False" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
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
                                    SelectCommand="SELECT LID, [LoginUserName], [EmployeeInfoID], (Select LevelName from userlevel where levelid= Logins.[UserLevel]) as UserLevel FROM [Logins] where LoginUserName<>'rony' ORDER BY [LoginUserName]"
                                    DeleteCommand="delete Logins where lid=0"></asp:SqlDataSource>

                                <asp:Label ID="txtCurrentPosition" runat="server" Text="" Visible="False"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>

