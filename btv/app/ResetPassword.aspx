<%@ Page Title="ResetPassword" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="app_ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
    <h2>Reset User Password</h2>
<fieldset>
    <legend>Reset Password of an Account</legend>
    
    &nbsp;<table class="table1">
        <tr><%--Department ID--%>
            <td>
                <asp:Label ID="lblDid" runat="server" Text="User ID/ Email Address:"></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtUid" runat="server" CssClass="form-control" Width="100px"></asp:TextBox>
                
                </td>
            <td>
                <asp:Button ID="btnSave0" runat="server" Text="Load" onclick="btnSave0_Click" /></td>
        </tr>
        <tr><%--Department Name--%>
            <td>
                <asp:Label ID="lblDid0" runat="server" Text="Email Check: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:Label ID="lblCurrEmail" CssClass="form-control" runat="server"></asp:Label>
                </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDeptName" runat="server" Text="New Password: "></asp:Label>
            </td>
            <td class="textbox">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
        <tr>
            <td>
                &nbsp;</td>
            <td class="textbox">
                <asp:Button ID="btnSave" runat="server" Text="Save New Password" CssClass="btn btn-default"
                    onclick="btnSave_Click" />
                <asp:Label ID="lblMsg" CssClass="msg" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>
</fieldset> 

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>

