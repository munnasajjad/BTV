<%@ Page Title="Check-Msg.aspx" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Check-Msg.aspx.cs" Inherits="app_Check_Msg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="pnl" runat="server">
        <ContentTemplate>
            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>

            
            <div class="grid_12 full_block">
                <div class="widget_wrap">
                    <div class="widget_top">
                        <span class="h_icon list_image"></span>
                        <h6>Messages for 
                            <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label></h6>
                    </div>
                    <div class="widget_content">

                        <div class="form_container left_label">

                            <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                SelectCommand="SELECT MsgID, Sender, Receiver, Subject, BodyText, IsRead, IsFlag, EntryDate,
                        (Select PhotoURL from  Photos where PhotoType='Messaging' AND MsgID=Messaging.MsgID) AS Attachment
                                             FROM [Messaging] where Receiver=@Receiver and IsRead=0 ORDER BY [MsgID] DESC">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lblBranch" Name="Receiver" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>


                            <br />
                            <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource1">
                                <ItemTemplate>
                                    <span style="background-color: #FFF8D0; color: #000000;">
                                        <hr />
                                        <h3>
                                            <asp:Label ID="HeadlineLabel" runat="server" Text='<%# Eval("Subject") %>' />
                                        </h3>
                                        <p>
                                            Send by: <b>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Sender") %>' /></b> on 
            <asp:Label ID="PublishDateLabel" runat="server" Text='<%# Eval("EntryDate") %>' />
                                        </p>
                                        <p>
                                            <asp:Label ID="FullNewsLabel" runat="server" Text='<%# Eval("BodyText") %>' />
                                        </p>
                                        <i><a href="<%# Eval("Attachment") %>"><%# Eval("Attachment") %></a></i>
                                        <br />
                                        <i style="float: right;"><a href="Check-Msg.aspx?read=<%# Eval("MsgID") %>">Mark as Read.</a></i>
                                        <hr />
                                    </span>
                                </ItemTemplate>

                                <EmptyDataTemplate>
                                    <span>No message was found into inbox.</span>
                                </EmptyDataTemplate>

                                <LayoutTemplate>
                                    <div style="font-family: Verdana, Arial, Helvetica, sans-serif;"
                                        id="itemPlaceholderContainer" runat="server">
                                        <span id="itemPlaceholder" runat="server" />
                                    </div>
                                    <div style="text-align: center; background-color: #CCCCCC; font-family: Verdana, Arial, Helvetica, sans-serif; color: #000000;">
                                        <asp:DataPager ID="DataPager1" runat="server" Visible="False">
                                            <Fields>
                                                <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True"
                                                    ShowLastPageButton="True" />
                                            </Fields>
                                        </asp:DataPager>
                                    </div>
                                </LayoutTemplate>

                            </asp:ListView>



                        </div>
                    </div>
                </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
