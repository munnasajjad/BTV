<%@ Page Title="Employee Transfer" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="EmployeeTransfer.aspx.cs" Inherits="app_EmployeeTransfer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .paginationArea {
            padding: 8px 0px;
            font-weight: 400;
            text-align: center;
            background: #F5F5F5;
            border-radius: 5px;
        }

            .paginationArea a {
                padding: 3px 13px;
                border: 1px solid #c2c2c2;
            }

        .site-content-size {
            margin-right: 5px;
        }

        activePagination, .activePagination a, .paginationArea a:focus, .paginationArea a:hover {
            background: #009446 !important;
            color: #fff !important;
        }

        .paginationArea {
            position: relative;
        }

            .paginationArea a, .paginationArea a:visited {
                color: #333 !important;
                text-decoration: none;
                color: #fff !important;
                text-decoration: none;
                padding: 5px 10px;
                border-radius: 5px;
                border: none;
                background: #444;
            }

        .activePagination {
            background-color: #356835 !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
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

            <div class="col-lg-6">
                <section class="panel">

                    <fieldset>
                        <legend>Employee Transfer</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Employee<span class="required">*</span></td>
                                <td>
                                    <asp:DropDownList ID="ddEmployee" runat="server" CssClass="form-control select2me" Width="100%"
                                                      AutoPostBack="True" OnSelectedIndexChanged="ddEmployee_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>

                            <tr>
                                <td>Name<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtName" ValidationGroup="Save" runat="server" ErrorMessage="Enter Name"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Designation</td>
                                <td>
                                    <asp:DropDownList ID="ddDesignationID" runat="server" CssClass="form-control select2me" Width="100%"
                                        AutoPostBack="False" OnSelectedIndexChanged="ddDesignationID_SelectedIndexChanged" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>Current Main Office</td>
                                <td>
                                    <asp:DropDownList ID="ddLocationID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddLocationID_SelectedIndexChanged" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Current Functional Office</td>
                                <td>
                                    <asp:DropDownList ID="ddCenterID" Width="100%" runat="server" CssClass="form-control select2me" Enabled="False" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddCenterID_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Current Department Section</td>
                                <td>
                                    <asp:DropDownList ID="ddDepartmentSectionID" AppendDataBoundItems="true" Width="100%" AutoPostBack="true" runat="server" CssClass="form-control select2me"
                                        OnSelectedIndexChanged="ddDepartmentSectionID_SelectedIndexChanged" Enabled="False">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>New Main Office</td>
                                <td>
                                    <asp:DropDownList ID="ddNewLocationID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddNewLocationID_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>New Functional Office</td>
                                <td>
                                    <asp:DropDownList ID="ddNewCenterID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddNewCenterID_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>New Department Section</td>
                                <td>
                                    <asp:DropDownList ID="ddNewDepartment" AppendDataBoundItems="true" Width="100%" AutoPostBack="true" runat="server" CssClass="form-control select2me">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            
                            <tr style="background: none">
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" ValidationGroup="Save" Text="Save" OnClick="btnSave_OnClick" />
                                    <asp:Button ID="btnClear" type="reset" CssClass="btn btn-s-md btn-white" runat="server" Text="Cancel" OnClick="btnClear_OnClick" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </section>
            </div>

            <div class="col-lg-6">
                <section class="panel">
                    <fieldset>
                        <legend>Transfer History</legend>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by Name" CssClass="form-control"></asp:TextBox>
                                    <asp:Button ID="btnSearch" Style="margin-left: 5px" OnClick="btnSearch_Click" runat="server" CssClass="btn btn-primary" Text="Search" />
                                </div>

                            </div>
                        </div>
                        <br />
                       
                        <div class="table-responsive">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <asp:GridView Width="100%" ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
                                            BorderStyle="None" BorderWidth="1px" AllowPaging="true" PageSize='<%#int.Parse(ConfigurationManager.AppSettings["PageSize"])%>' CellPadding="4" ForeColor="Black"
                                            GridLines="Vertical" DataKeyNames="EmployeeID" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDeleting="GridView1_OnRowDeleting">
                                            <Columns>
                                                <%--<asp:TemplateField ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
                                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" Visible="false" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
                                                        <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                                        <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                            PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                        <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                            <b style="color: red">This entry will be deleted permanently!</b><br />
                                                            Are you sure you want to delete this ?<br />
                                                            <br />
                                                            <div style="text-align: right;">
                                                                <asp:Button ID="ButtonOk" runat="server" CssClass="btn btn-success" Text="OK" />
                                                                <asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
                                                            </div>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="30px" HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                        <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("EmployeeID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                <%--<asp:BoundField DataField="Designation" HeaderText="Designation" SortExpression="Designation" />--%>
                                                <asp:BoundField DataField="CurrentMainOffice" HeaderText="Current Main Office" SortExpression="CurrentMainOffice" />
                                                <asp:BoundField DataField="CurrentFunctionalOffice" HeaderText="Current Functional Office" SortExpression="CurrentFunctionalOffice" />
                                                <asp:BoundField DataField="CurrentDepartment" HeaderText="Current Department" SortExpression="CurrentDepartment" />
                                                <asp:BoundField DataField="PreviousMainOffice" HeaderText="Previous Main Office" SortExpression="PreviousMainOffice" />
                                                <asp:BoundField DataField="PreviousFunctionalOffice" HeaderText="Previous Functional Office" SortExpression="PreviousFunctionalOffice" />
                                                <asp:BoundField DataField="PreviousDepartment" HeaderText="Previous Department" SortExpression="Previous Department" />
                                                

                                            </Columns>
                                            <RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
                                            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                            <PagerStyle CssClass="Pagination" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
                                            <HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
                                            <AlternatingRowStyle BackColor="White" />
                                            
                                        </asp:GridView>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </section>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>