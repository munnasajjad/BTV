<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="sql_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <header>
        <h1>Welcome to Extreme!</h1>
    </header>
    <div id="content" class="body-content content">

        <form id="form1" runat="server">

            <asp:Panel runat="server" ID="pnlPassword">
                <div class="form-group">
                    <label>Password: </label>
                    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>

                </div>
                <div class="form-action">
                    <label></label>
                    <asp:Button ID="Button1" runat="server" Text="Open" OnClick="Button1_OnClick" />
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlExecute" Visible="False">
                Method: 
            <div>
                <asp:CheckBox ID="CheckBox1" runat="server" Text="Upload Script" Checked="True" AutoPostBack="True" OnCheckedChanged="CheckBox1_OnCheckedChanged" />
            </div>
                <div>
                    <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" />
                    <asp:TextBox ID="txtScript" runat="server" TextMode="MultiLine" Width="100%" Height="500px" Visible="False" Font-Names="'Courier New', Courier, 'Lucida Sans Typewriter', 'Lucida Typewriter', monospace;"></asp:TextBox>
                </div>
                <asp:Button ID="btnExecute" runat="server" Text="Execute" OnClick="btnExecute_OnClick" />
                <asp:Button ID="Button3" runat="server" Text="Logout" OnClick="Button3_OnClick" />

            </asp:Panel>


            <%--<asp:Button ID="Button2" runat="server" Text="Run Date Convertion" OnClick="Button2_OnClick" />--%>

        </form>
    </div>
</body>
</html>
