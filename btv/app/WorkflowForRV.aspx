<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="WorkflowForRV.aspx.cs" Inherits="app_WorkflowForRV" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .panel {
            min-height: 0px !important;
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

            <div class="row">
                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                <div class="col-lg-12">
                    <section class="panel">
                        <fieldset>
                            <div class="caption">
                                <legend>Loan Voucher Infoformation</legend>
                            </div>
                            <div class="portlet-body form">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Date of RV:</label>
                                                <asp:Label ID="txtDateofRV" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Return Vouchar No:</label>
                                                <asp:Label ID="txtRVNumber" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdnLvSIRNo" runat="server" />
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                         <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Type:</label>
                                                <asp:Label ID="txtType" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <asp:Label ID="lblLVSIR" runat="server"></asp:Label>
                                                <asp:Label ID="txtLVSIR" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                         
                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Product Return Division:</label>
                                                <asp:Label ID="txtProductReturn" runat="server"></asp:Label>
                                            </div>

                                        </div>
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Product Return Cause:</label>
                                                <asp:Label ID="txtReturnCause" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        
                                         <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Store:</label>
                                                <asp:Label ID="txtStore" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                         <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Remarks:</label>
                                                <asp:Label ID="txtRemarks" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    
                                </div>
                            </div>


                        </fieldset>
                    </section>
                    <section class="panel">
                        <fieldset>

                            <div class="caption">
                                <legend>Products Details</legend>
                            </div>
                            <div class="col-lg-12">
                                 <div class="table-responsive">
                                            <asp:GridView runat="server" CssClass="table table-bordered" Width="100%" ID="productGridview" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                                <RowStyle BackColor="#F7F7DE" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="#SL" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20px" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Sl." SortExpression="ProductDetailsID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRvProductID" runat="server" Text='<%# Bind("RVProductID") %>'></asp:Label>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Returnt Quentity" SortExpression="ReturnQTY">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQTYReturn" runat="server" Text='<%# Bind("ReturnQTY") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product Status" SortExpression="ProductStatus">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("ProductStatus") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Unit Price" SortExpression="UnitPrice">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeliveredDate" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                            </div>
                            
                        </fieldset>
                    </section>
                    <section class="panel">
                        <fieldset>

                            <div class="caption">
                                <legend>Work Flow User</legend>
                            </div>
                            <div class="col-lg-12">
                                <%--<div class="table-responsive">--%>
                                <asp:GridView runat="server" Width="100%" CssClass="table table-bordered table-striped" ID="WorkFlowUserGridView" AutoGenerateColumns="False" BorderStyle="None" BorderWidth="1px" CellPadding="0" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                    <RowStyle BackColor="#F7F7DE" />
                                    <Columns>

                                        <asp:TemplateField ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                            <ItemStyle Width="20px" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Employee Name" SortExpression="EmployeeName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPriority" runat="server" Text='<%# Bind("Priority") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Esclation Start Date" SortExpression="EsclationStartTime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEsclationStartTime" runat="server" Text='<%# Bind("EsclationStartTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Esclation End Date" SortExpression="EsclationEndTime">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEsclationEndTime" runat="server" Text='<%# Bind("EsclationEndTime") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Approve/Decline Date" SortExpression="ApproveDeclineDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblApproveDeclineDate" runat="server" Text='<%# Bind("ApproveDeclineDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Permission Status" SortExpression="PermissionStatus">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPermissionStatus" runat="server" Text='<%# Bind("PermissionStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks to Approver" SortExpression="UserRemarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Remarks by Approver" SortExpression="UserRemarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserRemarks" runat="server" Text='<%# Bind("UserRemarks") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                   
                                </asp:GridView>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-1">
                                        <label>Remarks<span style="color: red">*</span></label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtYourRemark" runat="server"  Rows="3" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Enter your remarks" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtYourRemark"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:Button ID="btnApprove" CssClass="btn btn-success" ValidationGroup="Save" runat="server" Text="Approve" OnClick="btnApprove_OnClick" />
                                        <asp:Button ID="btnHold" CssClass="btn btn-warning" runat="server" ValidationGroup="Save" Text="Hold" OnClick="btnHold_OnClick" />
                                        <asp:Button ID="btnReturn" CssClass="btn btn-warning" runat="server" ValidationGroup="Save" Text="Return" OnClick="btnReturn_Click" />
                                        <asp:Button ID="btnDecline" CssClass="btn btn-danger" runat="server" ValidationGroup="Save" Text="Decline" OnClick="btnDecline_OnClick" />
                                    </div>
                                </div>

                            </div>
                            <br />


                            <%--</div>--%>
                        </fieldset>
                    </section>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
</asp:Content>

