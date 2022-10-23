<%@ Page Title="Post-Message" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Post-Message.aspx.cs" Inherits="app_Post_Message" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HtmlEditor" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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


            <div class="row">
                <div class="col-md-12">
                    <h3 class="page-title">Messaging</h3>
                </div>
            </div>

            <div class="row">

                <div class="col-md-6 ">
                    <div class="portlet box">
                        <div class="portlet-title">
                            <div class="caption">
                                <i class="fa fa-reorder"></i>Send Message
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-body">

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>

                                <div class="control-group">
                                    <label class="field_title">Receiver :</label>
                                    <asp:DropDownList ID="ddUsers" runat="server"
                                        DataSourceID="SqlDataSource2" AppendDataBoundItems="True"
                                        DataTextField="LoginUserName" DataValueField="LoginUserName" AutoPostBack="True" OnSelectedIndexChanged="ddUsers_OnSelectedIndexChanged">
                                        <asp:ListItem Value="0">Software Support Team</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server"
                                        ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT [LoginUserName] FROM [Logins] Where (ProjectId = @ProjectId) AND LoginUserName<>'rony' AND LoginUserName<>@LoginUserName order by [LoginUserName]">
                                        <SelectParameters>
                                             <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                            <asp:ControlParameter Name="LoginUserName" ControlID="lblUser" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>

                                </div>

                                <div class="control-group">
                                    <label class="field_title">Subject</label>
                                    <div class="form_input">
                                        <asp:TextBox ID="txtSubject" runat="server" TabIndex="0" title="Subject"></asp:TextBox>
                                    </div>
                                    <br />
                                    <br />
                                </div>

                                <div class="control-group">
                                    <%--<div class="form_input">--%>
                                    <asp:Editor ID="txtMsgBody" runat="server" Width="490px" class="form-control" Height="380px" TabIndex="5" AutoFocus="False" />

                                    <%--</div>--%>
                                </div>

                                <div class="control-group">
                                    <asp:Label ID="lblPhoto" runat="server" Text="Attachment: "></asp:Label>
                                    <asp:FileUpload ID="FileUpload2" runat="server" ClientIDMode="Static" CssClass="form-control" Width="45%" />
                                    <asp:Image ID="imgPhoto" runat="server" Width="60px" />
                                </div>
                                
                                <div class="form-actions">
                                        <asp:Button ID="btnSave" runat="server" Text="Send Message" class="btn_small btn_blue" OnClick="btnSave_Click" />
                                        <asp:Button ID="Button1" runat="server" Text="Clear Form" class="btn_small btn_orange" />

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
                                List of Send Messages
                            </div>
                            <div class="tools">
                            </div>
                        </div>
                        <div class="portlet-body form">
                            <div class="form-horizontal" role="form">
                                <div class="form-body">
                                    <asp:GridView ID="GridView1" runat="server" DataKeyNames="MsgID" DataSourceID="SqlDataSource1" AutoGenerateColumns="False">
                                        <Columns>
                                            <%--<asp:BoundField DataField="Sender" HeaderText="Sender" SortExpression="Sender" />--%>
                                            <asp:BoundField DataField="Receiver" HeaderText="Receiver" SortExpression="Receiver" />
                                            <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
                                            <asp:BoundField DataField="EntryDate" HeaderText="Date" SortExpression="EntryDate" DataFormatString="{0:d}" />
                                            <asp:TemplateField HeaderText="Attachment" SortExpression="MsgID">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="Label1" runat="server" Text="File" Target="_blank" NavigateUrl='<%# Bind("Attachment") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                        SelectCommand="SELECT TOP(10) Sender, [Receiver], [Subject], [EntryDate], [MsgID],
                        (Select PhotoURL from  Photos where PhotoType='Messaging' AND MsgID=Messaging.MsgID AND (ProjectId = @ProjectId)) AS Attachment
                                         FROM [Messaging] WHERE ([Receiver] = @Receiver) AND Sender=@Sender AND (ProjectId = @ProjectId) ORDER BY MsgID DESC"
                                        DeleteCommand="Delete Messaging where MsgID='0'">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddUsers" Name="Receiver" PropertyName="SelectedValue" Type="String" />
                                            <asp:ControlParameter ControlID="lblUser" Name="Sender" PropertyName="Text" Type="String" />
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
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>
