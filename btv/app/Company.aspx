<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Company.aspx.cs" Inherits="app_Company" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HtmlEditor" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
img#ctl00_BodyContent_imgLogo {
    float: right;
    margin-right: 7px;
    border-radius: 5px;
    width: 100px; 
}

label[for=ctl00_BodyContent_chkAcc] {
                    width: 100px !important;
                }
label[for=ctl00_BodyContent_chkPayroll] {
                    width: 80px !important;
                }
label[for=ctl00_BodyContent_chkInventory] {
                    width: 105px !important;
                }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">
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

            <h3 class="page-title">Company Information</h3>

            <div class="row">
                <div class="col-md-6">
                    <div class="portlet box red">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Company Details
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body">
                                &nbsp;<asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>

                                <asp:Label ID="lblEid" runat="server" Text="1" Visible="false"></asp:Label>
                                

                                <div class="form-group">
                                    <asp:Label ID="Label5" runat="server" Text="Company Type : "></asp:Label>
                                    <asp:DropDownList ID="ddIndustry" runat="server" CssClass="form-control select2me" DataSourceID="SqlDataSource3"
                                    DataTextField="Name" DataValueField="id" Width="80%" AutoPostBack="True" OnSelectedIndexChanged="ddIndustry_OnSelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT id, Name FROM [TargetIndustries] Where ProjectID=1 ORDER BY [Name]"></asp:SqlDataSource>
                                <span class="help-block"></span>
                                </div>  

                                <div class="control-group">
                                    <asp:Label ID="lblEname" runat="server" Text="Company Name : "></asp:Label>
                                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                </div>
                                <div class="control-group">
                                    <asp:Label ID="Label4" runat="server" Text="Letter Head : "></asp:Label>
                                    <asp:RadioButton ID="rbShow" runat="server" Text="Show" Checked="True" GroupName="g" />
                                    <asp:RadioButton ID="rbHide" runat="server" Text="Hide" Checked="False" GroupName="g" />

                                    <asp:Editor ID="txtAddress" runat="server" Width="100%" Height="200px" TabIndex="5" AutoFocus="False" />

                                </div>
                                
                                
                                <div class="control-group">
                                    <asp:Label ID="Label3" runat="server" Text="Business Status in Dashboard : "></asp:Label>
                                    <asp:DropDownList ID="ddNT" runat="server">
                                        <asp:ListItem>Show</asp:ListItem>
                                        <asp:ListItem>Hide</asp:ListItem>
                                    </asp:DropDownList>
                                </div>  
                                
                               <div class="control-group">
                                <asp:Label ID="OfcSTime" runat="server" Text="Office Start Time : "></asp:Label>
                                  <asp:TextBox ID="txtOfcSTime" runat="server" Text="9:30"></asp:TextBox>
                                </div>
                                    
                                    <div class="control-group">
                                <asp:Label ID="MnthSTarget" runat="server" Text="Monthly Sales Target : "></asp:Label>
                                  <asp:TextBox ID="txtMnthSTarget" runat="server"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtende3r1" runat="server" 
                                            FilterType="Custom, Numbers" ValidChars="0123456789" TargetControlID="txtMnthSTarget"></asp:FilteredTextBoxExtender>
                                </div>
                                    
                                <div class="control-group">
                                    <asp:Label ID="Label2" runat="server" Text="VAT% on Sales : "></asp:Label>
                                    <asp:TextBox ID="txtVAT" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtOpBalance_FilteredTextBoxExtender" runat="server" 
                                            FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtVAT"></asp:FilteredTextBoxExtender>
                                </div>
                                
                                <div class="control-group">
                                    <asp:Label ID="Label6" runat="server" Text="Comm. Agents : "></asp:Label>
                                    <asp:DropDownList ID="ddDocComm" runat="server">
                                        <asp:ListItem Value="1">Show</asp:ListItem>
                                        <asp:ListItem Value="0">Hide</asp:ListItem>
                                    </asp:DropDownList>
                                </div>  

                                <div class="control-group">
                                    <asp:Label ID="lblPhoto" runat="server" Text="Company Logo: "></asp:Label>
                                    <asp:FileUpload ID="FileUpload2" runat="server" ClientIDMode="Static" CssClass="form-control" Width="45%" />
                                    <asp:Image ID="imgLogo" runat="server" Width="100px" />
                                </div>
                                
                                <asp:panel ID="pnlExpDate" runat="server" Visible="False">
                                <div class="control-group">
                                    <label class="control-label">Developed By:</label>
                                    <asp:DropDownList ID="ddDeveloped" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddDeveloped_OnSelectedIndexChanged"
                                        CssClass="span6 chosen" DataSourceID="SqlDataSource4" DataTextField="DevelopedBy"
                                        DataValueField="sid" >
                                    </asp:DropDownList>

                                    <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT sid, [DevelopedBy] FROM [settings_branding] Where (ProjectId = @ProjectId) ORDER BY [sid]">
                                               <SelectParameters>
		                                                <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                                </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>
                                    <div class="control-group">
                                <asp:Label ID="lblLimit" runat="server" Text="Max Employee Quota: "></asp:Label>
                                  <asp:TextBox ID="txtLimitQuota" runat="server" Text="5"></asp:TextBox>
                                </div>
                                    <div class="control-group">
                                <asp:Label ID="Label9" runat="server" Text="SMS Quota Per Month: "></asp:Label>
                                  <asp:TextBox ID="txtSmsQouta" runat="server" Text="5"></asp:TextBox>
                                </div>
                                    <div class="control-group">
                                    <asp:Label ID="Label8" runat="server" Text="Pakage : "></asp:Label>
                                    <asp:DropDownList ID="ddPackage" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddPackage_OnSelectedIndexChanged">
                                        <asp:ListItem Value="1">Basic</asp:ListItem>
                                        <asp:ListItem Value="2">Essential</asp:ListItem>
                                        <asp:ListItem Value="3">Standard</asp:ListItem>
                                        <asp:ListItem Value="4">Premium</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                    
                                    <div class="control-group">
                                    <asp:Label ID="Label10" runat="server" Text="Is Active : "></asp:Label>
                                    <asp:DropDownList ID="ddIsActive" runat="server" AutoPostBack="True">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Deactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                
                                    <div class="control-group">
                                <asp:Label ID="Label7" runat="server" Text="Ad-ons:" Width="23%"></asp:Label>
                                  <asp:CheckBox ID="chkAcc" runat="server" Checked="false" Text="Core Accounting" />
                                        <asp:CheckBox ID="chkInventory" runat="server" Checked="false" Text="Stock & Inventory" />
                                        <asp:CheckBox ID="chkPayroll" runat="server" Checked="false" Text="Payroll" Visible="False"/>
                                </div>
                                <div class="control-group">
                                    <label>Expire Date:</label>
                                    <asp:TextBox ID="txtExpDate" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="txtExpDate"></asp:CalendarExtender>
                                </div>
                                  
                                </asp:panel>

                                <div class="form-actions">

                                    <asp:Button ID="btnSave" runat="server" Text="Save Company Info" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnclr" runat="server" Text="Delete Company" OnClick="btnclr_OnClick" Visible="False"/>
                                </div>
                                <div>
                                    <label></label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 ">
                    <div class="portlet box green">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i><%--Saved Data--%>
                            </div>
                            <div class="tools">
                                <a href="" class="collapse"></a>
                                <a href="#portlet-config" data-toggle="modal" class="config"></a>
                                <a href="" class="reload"></a>
                                <a href="" class="remove"></a>
                            </div>
                        </div>
                        <div class="portlet-body form">

                            <div class="form-body table-responsive">

                                <asp:Label ID="lblSubscription" runat="server" EnableViewState="False"></asp:Label>

                                <asp:GridView ID="GridView1" runat="server" Width="100%" AllowSorting="True" Visible="False"
                                    AutoGenerateColumns="False" DataSourceID="SqlDataSource1" ForeColor="Black" GridLines="Vertical" DataKeyNames="VID"
                                    OnRowDeleting="GridView1_OnRowDeleting" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:Label ID="Label1" runat="server" Visible="False" Text='<%# Bind("VID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="VID" HeaderText="VID" SortExpression="VID" InsertVisible="False" ReadOnly="True" />
                                        <asp:BoundField DataField="ProjectName" HeaderText="Company Name" SortExpression="ProjectName" ReadOnly="True" />
                                        <asp:TemplateField HeaderText="Report Header" SortExpression="ReportHeader">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuestion" runat="server" Text='<%# Server.HtmlDecode(Eval("ReportHeader").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                       <%-- <asp:BoundField DataField="Logo" HeaderText="Logo" SortExpression="Logo" />--%>
                                        <asp:BoundField DataField="TrialDate" HeaderText="Expire Date" SortExpression="TrialDate" DataFormatString="{0:d}" />
                                        <%--<asp:BoundField DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" />
                                        <asp:BoundField DataField="EntryDate" HeaderText="EntryDate" SortExpression="EntryDate" />--%>

                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" ToolTip="Edit" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.png" Text="Delete" ToolTip="Delete" Visible="False"/>

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1">
                                                </asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Entry will be deleted!</b><br />
                                                    Are you sure you want to delete the item from entry list?
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
                                    SelectCommand="SELECT [VID], [ProjectName], [ReportHeader], [Logo], [TrialDate],  [EntryBy], [EntryDate] FROM [Projects] Where  (VID = '1') ORDER BY [VID]">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddDeveloped" Name="Developed" PropertyName="SelectedValue" />
                                        <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
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


