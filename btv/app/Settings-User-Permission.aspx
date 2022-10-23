<%@ Page Title="User Role Permission" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Settings-User-Permission.aspx.cs" Inherits="app_Settings_User_Permission" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .panel input[type=checkbox], .panel input[type=radio] {
            padding: 0;
            width: 30px !important;
            border: none !important;
            height: 22px !important;
            box-shadow: none !important;
            margin-left: 12px;
        }

        .panel {
            min-height: 10px !important;
        }

        @media (min-width: 1025px) {
            .panel label {
                padding: 11px 0;
                vertical-align: super;
            }

            .portlet-body table tr:nth-child(odd) {
                background: #ddf7e3 !important;
            }

            td {
                vertical-align: middle !important;
            }

            table {
                width: 100%;
            }
        }

        legend input[type=checkbox] {
            margin-right: 10px;
            zoom: 150%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="                    position: fixed;
                    left: 95%;
                    top: 50%;
                    vertical-align: middle;
                    border-style: inset;
                    border-color: black;
                    background-color: White;
                    z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
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
                    <h3 class="page-title">User Role Permissions</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>
                                <asp:Literal ID="lrlSavedBox" runat="server">User Role Info</asp:Literal>
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

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped" DataSourceID="SqlDataSource1" Width="100%" DataKeyNames="LID"
                                    OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" >

                                    <Columns>
                                           <asp:TemplateField ShowHeader="False" Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/Images/edit.png" Text="Select" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-Width="20px" HeaderText="#Sl">
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
                                        <asp:BoundField DataField="EmployeeInfoID" HeaderText="Employee" SortExpression="EmployeeInfoID" />
                                        <asp:BoundField DataField="Role" HeaderText="User Role" SortExpression="Role" />

                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                    SelectCommand="SELECT LID, [LoginUserName], (Select Name from Employee where EmployeeID= Logins.EmployeeInfoID) as EmployeeInfoID, (SELECT LevelName FROM UserLevel WHERE LevelID= Logins.UserLevel) AS Role FROM [Logins] where LoginUserName<>'rony' AND (ProjectId = @ProjectId) ORDER BY [LoginUserName]"
                                    DeleteCommand="delete Logins where lid=0">
                                    <SelectParameters>
	                                     <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                    </SelectParameters>
                                </asp:SqlDataSource>

                                <asp:Label ID="txtCurrentPosition" runat="server" Text="" Visible="False"></asp:Label>

                            </div>
                        </div>
                    </div>
                </div>


                <div class="col-md-6 ">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Set role permission
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>
                                <div class="control-group span12 col-md-12">
                                        <label class="control-label" style="color:white">Permission Role: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddRole" runat="server" CssClass="form-control" DataSourceID="SqlDataSource7p" DataTextField="LevelName" DataValueField="LevelID" AutoPostBack="True" OnSelectedIndexChanged="ddRole_SelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7p" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT LevelID, LevelName FROM [UserLevel]  ORDER BY [LevelID]"></asp:SqlDataSource>

                                        </div>
                                    </div>

                                    <div class="control-group span12 col-md-12 hidden">
                                        <label class="control-label">Login Id: </label>
                                        <div class="controls">
                                            <asp:TextBox ID="txtUserX" CssClass="span12 m-wrap" runat="server" ReadOnly="True"></asp:TextBox>

                                        </div>
                                    </div>
                                

                                    <div class="control-group  span12 col-md-12 hidden">
                                        <label class="control-label">Employee Name: </label>
                                        <div class="controls">
                                            <asp:Textbox ID="txtEmployeX" runat="server" CssClass="form-control" ReadOnly="True"/>
                                            <asp:Label ID="lblEID" runat="server" Visible="False"/>

                                            <%--<asp:Textbox ID="ddEmployeX" runat="server"
                                                CssClass="form-control">
                                            </asp:Textbox>--%>

                                            <asp:SqlDataSource ID="SqlDataSource40x" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT [Name], EmployeeID FROM [Employee] Where (ProjectId = @ProjectId) ORDER BY [Name]">
                                                <SelectParameters>
                                                    <%--<asp:ControlParameter ControlID="ddCounterX" Name="DepartmentID" PropertyName="SelectedValue" />--%>
                                                    <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </div>
                                    </div>

                                    <%--<div class="control-group span12 col-md-12 hidden">
                                        <label class="control-label">Department: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddCounterX" runat="server"
                                                CssClass="form-control"
                                                DataSourceID="SqlDataSource7x" DataTextField="DeptName"
                                                DataValueField="DeptId">
                                            </asp:DropDownList>

                                            <asp:SqlDataSource ID="SqlDataSource7x" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT DeptId, DeptName FROM [tblDepartment] ORDER BY [DeptName]"></asp:SqlDataSource>


                                        </div>
                                    </div>--%>

                                   <%-- <div class="control-group span12 col-md-12">
                                        <label class="control-label">Permission Level: </label>
                                        <div class="controls">

                                            <asp:DropDownList ID="ddLevelX" runat="server" CssClass="form-control" DataSourceID="SqlDataSource7p" DataTextField="LevelName"
                                                DataValueField="LevelID">
                                            </asp:DropDownList>
                                            <%--<asp:Textbox ID="LevelX" runat="server" CssClass="form-control"/>--%>

                                           <%-- <asp:SqlDataSource ID="SqlDataSource7p" runat="server"
                                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                SelectCommand="SELECT LevelID, LevelName FROM [UserLevel] Where (ProjectId = @ProjectId) ORDER BY [LevelID]">
                                                <SelectParameters>
	                                                 <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>

                                        </div>
                                    </div>--%>

                                
                                <div>
                                <asp:Panel ID="pnlOparatePermission" runat="server" Visible="True">
                                <legend> <asp:CheckBox ID="CheckBox30" runat="server" Text="" Visible="false" AutoPostBack="True" Checked="true" OnCheckedChanged="CheckBox30_OnCheckedChanged"/>Operating Permission </legend>
                                    <asp:Panel runat="server" ID="PanelOparate">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="chkInsert" runat="server" Text="Insert" />
                                                <asp:CheckBox ID="chkUpdate" runat="server" Text="Update" />
                                               <asp:CheckBox ID="chkDelete" runat="server" Text="Delete" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    </asp:Panel>
                                </div>

                                <asp:ListView ID="ListView1" DataSourceID="SqlDataSource2"  runat="server" DataKeyNames="Sl" OnItemDataBound="ListView1_ItemDataBound">
                                    <ItemTemplate>
                                        <legend>
                                            <asp:CheckBox ID="cb1" Checked="true" runat="server" Text='<%#Eval("GroupName") %>' AutoPostBack="True" OnCheckedChanged="cb1_CheckedChanged" />
                                        </legend>
                                        <asp:Panel runat="server" ID="Panel99">

                                            <div class="panel panel-default">
                                                <div class="panel-heading">

                                                    <asp:CheckBoxList ID="cblmenuItems" runat="server" DataSourceID="SqlDataSource1"
                                                        DataTextField="FormName" DataValueField="sl">
                                                    </asp:CheckBoxList>
                                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                                        SelectCommand="SELECT sl, TableName, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, Priority, Show, EntryBy, EntryDate, ProjectId FROM MenuStructure WHERE MenuGroup=@MenuGroup AND ([Show] = @Show) ORDER BY [Priority]">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="cb1" PropertyName="Text" Name="MenuGroup" />
                                                             <asp:Parameter DefaultValue="true" Name="Show" Type="Boolean" />
                                                        </SelectParameters>

                                                    </asp:SqlDataSource>

                                                </div>
                                            </div>
                                        </asp:Panel>

                                    </ItemTemplate>
                                </asp:ListView>

                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT * FROM [MenuGroup] WHERE ([Show] = @Show) ORDER BY [DesplaySerial]">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="true" Name="Show" Type="Boolean" />
                                    </SelectParameters>
                                </asp:SqlDataSource>





                               <%-- <asp:Panel ID="pnlSetup" runat="server" Visible="True">
                                    <legend> <asp:CheckBox ID="CheckBox17" runat="server" Text="Initial Setup" AutoPostBack="True" OnCheckedChanged="CheckBox17_OnCheckedChanged"/> </legend>
                                    <asp:Panel runat="server" ID="Panel9">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <asp:CheckBox ID="CheckBox31" runat="server" Text="A/C Setup" />
                                                <asp:CheckBox ID="CheckBox32" runat="server" Text="Others" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    </asp:Panel>
                                <asp:Panel ID="pnlMarketing" runat="server" Visible="False">
                                    <legend> <asp:CheckBox ID="CheckBox1" runat="server" Text="Marketing" AutoPostBack="True" OnCheckedChanged="CheckBox1_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel1">
                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox2" runat="server" Text="Planning" />
                                                <asp:CheckBox ID="CheckBox3" runat="server" Text="Activities" />
                                               <asp:CheckBox ID="CheckBox4" runat="server" Text="Campaigns" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    </asp:Panel>
                                <asp:Panel ID="pnlSales" runat="server" Visible="True">
                                    <legend> <asp:CheckBox ID="CheckBox6" runat="server" Text="Sales"  AutoPostBack="True" OnCheckedChanged="CheckBox6_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel2">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox7" runat="server" Text="Products" />
                                                <asp:CheckBox ID="CheckBox8" runat="server" Text="Customers" />
                                               <asp:CheckBox ID="CheckBox9" runat="server" Text="Planning" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    </asp:Panel>
                                <asp:Panel ID="pnlEmployee" runat="server" Visible="False">
                                    <legend> <asp:CheckBox ID="CheckBox11" runat="server" Text="Employee"  AutoPostBack="True" OnCheckedChanged="CheckBox11_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel3">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox12" runat="server" Text="Setup" />
                                                <asp:CheckBox ID="CheckBox13" runat="server" Text="Attendance" />
                                                <asp:CheckBox ID="CheckBox14" runat="server" Text="Work" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    </asp:Panel>
                                <asp:Panel ID="pnlStore" runat="server" Visible="False">
                                <legend> <asp:CheckBox ID="CheckBox5" runat="server" Text="Store & Inventory"  AutoPostBack="True" OnCheckedChanged="CheckBox5_OnCheckedChanged"/> </legend>
                                    <asp:Panel runat="server" ID="Panel7">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox10" runat="server" Text="Warehouses" />
                                                <asp:CheckBox ID="CheckBox15" runat="server" Text="Purchase" />
                                                <asp:CheckBox ID="CheckBox19" runat="server" Text="Store Activities" />
                                                <asp:CheckBox ID="CheckBox23" runat="server" Text="Reports" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    </asp:Panel>
                                <asp:Panel ID="pnlAccounts" runat="server" Visible="True">
                                    <legend> <asp:CheckBox ID="CheckBox16" runat="server" Text="Accounts"  AutoPostBack="True" OnCheckedChanged="CheckBox16_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel4">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                                <asp:CheckBox ID="CheckBox18" runat="server" Text="Data Entry" />
                                                <asp:CheckBox ID="CheckBox20" runat="server" Text="Report" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    </asp:Panel>
                                <asp:Panel ID="pnlCoreAcc" runat="server" Visible="False">
                                    <legend> <asp:CheckBox ID="CheckBox21" runat="server" Text="Core Accounting"  AutoPostBack="True" OnCheckedChanged="CheckBox21_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel5">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox22" runat="server" Text="Setup" />
                                               <asp:CheckBox ID="CheckBox24" runat="server" Text="Voucher" />
                                                <asp:CheckBox ID="CheckBox25" runat="server" Text="Reports" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                    </asp:Panel>
                                <asp:Panel ID="pnlMaintenance" runat="server" Visible="True">
                                <legend> <asp:CheckBox ID="CheckBox26" runat="server" Text="Maintenance"  AutoPostBack="True" OnCheckedChanged="CheckBox21_OnCheckedChanged" /> </legend>
                                    <asp:Panel runat="server" ID="Panel6">

                                        <div class="panel panel-default">
                                            <div class="panel-heading">
                                               <asp:CheckBox ID="CheckBox27" runat="server" Text="Company" />
                                               <asp:CheckBox ID="CheckBox28" runat="server" Text="Notice & Messages" />
                                                <asp:CheckBox ID="CheckBox29" runat="server" Text="User & Security" />
                                            </div>
                                        </div>
                                </asp:Panel>
                                  </asp:Panel> --%> 
                                    <div class="form-actions">
                                        <asp:Button ID="btnSave" CssClass="btn blue" runat="server" Text="Update" OnClick="btnSave_OnClick" />
                                        <asp:Button ID="btnClear" CssClass="btn" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                    </div>



                            </div>
                        </div>
                    </div>
                </div>



            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>

