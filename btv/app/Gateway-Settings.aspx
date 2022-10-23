<%@ Page Title="Gateway-Settings" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Gateway-Settings.aspx.cs" Inherits="app_Gateway_Settings" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        input#ctl00_BodyContent_txtEmail {
            text-align: center;
        }
        td {
            max-width: 100px;
            overflow: hidden;
        }
        input#ctl00_BodyContent_txtEmail {
            margin-left: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

   
   
    <center>
    <legend>Test all active Gateway</legend>
        <div class="center">
          <tr>
              
            <td> <asp:TextBox ID="txtEmail" runat="server" Text="ronyjob@gmail.com" TextMode="Email" width="60%" CssClass="form-control" required></asp:TextBox> </td>
              <td> <asp:Button ID="Button1" runat="server" Text="Send Test Mail" OnClick="Button1_OnClick"/></td> 
              <td> <asp:Button ID="btnNew" runat="server" Text="Add New" OnClick="Button2_OnClick" /></td>
            </tr>  
    <asp:Label runat="server" ID="lblMsg"></asp:Label>
        </div>
    </center>
     <div class="col-lg-12 center">
                <section class="panel">
                    <%--Body Contants--%>
                    <div id="NewGateway" runat="server" class="col-lg-6" Visible="False">
                       
                            <fieldset>
                                <legend>Add new Gateway</legend>
                                <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                                    <tr>
                                        <td>Choose Mail</td>
                                        <td>
                                             <asp:DropDownList ID="ddMailSelect" runat="server" CssClass="form-control select2me" AutoPostBack="True" OnSelectedIndexChanged="ddMailSelect_OnSelectedIndexChanged"> 
                                                 <asp:ListItem Value="0" Text="---Select---"></asp:ListItem>
                                                 <asp:ListItem Value="1" Text="Gmail"></asp:ListItem>
<%--                                                 <asp:ListItem Value="2" Text="Yahoo"></asp:ListItem>
                                                 <asp:ListItem Value="3" Text="Microsoft Mail"></asp:ListItem>--%>
                                                 <asp:ListItem Value="4" Text="Others"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td>UserName (E-mail)</td>
                                        <td>
                                            <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" type="email"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Password</td>
                                        <td>
                                            <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Month ID</td>
                                        <td>
                                            <asp:TextBox ID="txtMonthId" runat="server" CssClass="form-control"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy"
                                                Enabled="True" TargetControlID="txtMonthId">
                                            </asp:CalendarExtender>
                                        </td>
                                    </tr>
                                    <asp:Panel ID="pnlMailDetails" runat="server" Visible="False">
                                     <tr>
                                        <td>Gateway Name</td>
                                        <td>
                                            <asp:TextBox ID="txtGatewayName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>SMTP Server Link</td>
                                        <td>
                                            <asp:TextBox ID="txtLink" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>Port</td>
                                        <td>
                                            <asp:TextBox ID="txtPort" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>SSL</td>
                                        <td>
                                             <asp:DropDownList ID="ddSSL" runat="server" CssClass="form-control select2me">
                                                 <asp:ListItem Value="0" Text="False"></asp:ListItem>
                                                 <asp:ListItem Value="1" Text="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Month Limit</td>
                                        <td>
                                            <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    
                                    <tr>
                                        <td>Month Send</td>
                                        <td>
                                            <asp:TextBox ID="TxtMonthSend" runat="server" CssClass="form-control" ></asp:TextBox>
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td>Total Send</td>
                                        <td>
                                            <asp:TextBox ID="txttotal" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>IsActive</td>
                                        <td>
                                            <asp:DropDownList ID="ddIsActive" runat="server" CssClass="form-control select2me">
                                                 <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                                 <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Ratio</td>
                                        <td>
                                            <asp:TextBox ID="txtRatio" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Priority</td>
                                        <td>
                                            <asp:TextBox ID="txtPriority" runat="server" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    </asp:Panel>
                                    <tr style="background: none">
                                        <td></td>
                                        <td style="text-align: center;">
                                            <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick"/>
                                            <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Clear" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                    </div>
                    <div class="Instruction col-lg-6">
                        <asp:Panel ID="GmailIns" runat="server" Visible="False">
                            <img src="../Uploads/Sign%20in%20%20%20security%20(1).png" class="img-responsive img-thumbnail" width="100%"/>
                            <a href="https://myaccount.google.com/security?utm_source=OGB&utm_medium=app&pli=1#connectedapps" target="blank" class="text_bold_co">Click To Change Settings</a>
                        </asp:Panel> 
                        <asp:Panel ID="YahoolIns" runat="server" Visible="False">
                            <img src="../Uploads/Security%20%20%20Yahoo%20Account Settings.png" class="img-responsive img-thumbnail" width="100%"/>
                            <a href="https://login.yahoo.com/account/security?el=1&done=https%3A%2F%2Fwww.yahoo.com&crumb=C39LoFl1dtx&scrumb=0gQox820oPb" target="blank" class="text_bold_co">Click To Change Settings</a>
                        </asp:Panel>
                    </div>

                    <%--End Body Contants--%>
                </section>
                     </div>
         <div class="row">
             <div class="col-lg-12">
                  <div class="table-responsive">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" AutoGenerateEditButton="True" Width="100%" DataKeyNames="id">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" InsertVisible="False" ReadOnly="True" />
            <asp:BoundField DataField="GatewayName" HeaderText="Gateway Name" SortExpression="SettingName" />
            <asp:BoundField DataField="Link" HeaderText="Link" SortExpression="SettingValue" />
            <asp:BoundField DataField="UserName" HeaderText="UserName" SortExpression="Remark" />
            <asp:BoundField DataField="Password" HeaderText="Password" SortExpression="SettingName" />
            <asp:BoundField DataField="Port" HeaderText="Port" SortExpression="SettingValue" />
            <asp:BoundField DataField="EnableSSL" HeaderText="SSL" SortExpression="Remark" />
            <asp:BoundField DataField="MonthlyLimit" HeaderText="Month Limit" SortExpression="SettingName" />
            <asp:BoundField DataField="MonthID" HeaderText="Month ID" SortExpression="SettingValue" />
            <asp:BoundField DataField="MonthlySendQty" HeaderText="Month Send" SortExpression="Remark" />

            <asp:BoundField DataField="TotalSendQty" HeaderText="Total Send" SortExpression="Remark" ReadOnly="True" />
            <asp:BoundField DataField="IsActive" HeaderText="IsActive" SortExpression="SettingName" />
            <asp:BoundField DataField="PriorityRatio" HeaderText="Ratio" SortExpression="SettingValue" />
            <asp:BoundField DataField="SendingPriority" HeaderText="Priority" SortExpression="Remark" ReadOnly="True" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
        SelectCommand="SELECT [id], [GatewayName], [Link], [UserName], [Password], [Port], [EnableSSL], [MonthlyLimit], [MonthID], [MonthlySendQty], 
        [TotalSendQty], [IsActive], [PriorityRatio], [SendingPriority] FROM [EmailGateways] Where (ProjectId = @ProjectId)"
        UpdateCommand="UPDATE EmailGateways SET GatewayName = @GatewayName, Link = @Link, UserName = @UserName,
        Password = @Password, Port = @Port, EnableSSL = @EnableSSL,MonthlyLimit = @MonthlyLimit, MonthID = @MonthID, MonthlySendQty = @MonthlySendQty,
        IsActive = @IsActive, PriorityRatio = @PriorityRatio WHERE (id = @id) ">
                <SelectParameters>
		        <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="SettingName" />
            <asp:Parameter Name="SettingValue" />
            <asp:Parameter Name="Remark" />
            <asp:Parameter Name="id" />
        </UpdateParameters>
    </asp:SqlDataSource>
    </div>
             </div>
         </div>
                                 


</asp:Content>

