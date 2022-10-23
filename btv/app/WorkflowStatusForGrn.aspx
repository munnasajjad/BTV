<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="WorkflowStatusForGrn.aspx.cs" Inherits="app_WorkflowStatusForGrn" %>

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
                <div class="col-md-12">
                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                </div>
                <div class="col-lg-12">
                    <section class="panel">
                        <fieldset>
                            <div class="caption">
                                <legend>GRN Infoformation</legend>
                            </div>
                            <div class="portlet-body form">
                                <div class="form-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Date of GRN:</label>
                                                <asp:Label ID="txtGrnDate" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>GRN No:</label>
                                                <asp:Label ID="txtGrnNo" runat="server"></asp:Label>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Reference:</label>
                                                <asp:Label ID="txtReference" runat="server"></asp:Label>
                                            </div>

                                        </div>
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Invoice No:</label>
                                                <asp:Label ID="txtInvoiceNo" runat="server"></asp:Label>
                                            </div>
                                        </div>


                                    </div>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Supplier:</label>
                                                <asp:Label ID="txtSupplier" runat="server"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="control-group">
                                                <label>Remarks:</label>
                                                <asp:Label ID="lblRemarks" runat="server"></asp:Label>
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
                                    <asp:GridView runat="server" Width="100%" ShowFooter="True" ID="productAddGridView" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BorderStyle="None" OnRowDataBound="productAddGridView_RowDataBound" BorderWidth="1px" CellPadding="4" ForeColor="Black" BackColor="White" BorderColor="#DEDFDE" GridLines="Vertical">
                                        <RowStyle BackColor="#F7F7DE" />
                                        <Columns>

                                            <asp:TemplateField ItemStyle-Width="20px">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Sl." SortExpression="CrID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGRNProductID" runat="server" Text='<%# Bind("GRNProductID") %>'></asp:Label>

                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Product Name" SortExpression="ProductName">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductName" runat="server" Text='<%# Bind("ProductName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Receive Product" SortExpression="ReceiveProduct">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblReceiveProduct" runat="server" Text='<%# Bind("ReceiveProduct") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Unit Price" SortExpression="UnitPrice">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Bind("UnitPrice") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Cost" SortExpression="TotalCost">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalCost" runat="server" Text='<%# Bind("TotalCost") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField Visible="false" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/app/images/edit.png" Text="Select" ToolTip="Edit" />
                                                    <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/app/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                                    <asp:ConfirmButtonExtender TargetControlID="ImageButton4" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                                    <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton4"
                                                        PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                    <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                        <b style="color: red">Entry will be deleted!</b><br />
                                                        Are you sure, you want to delete the item from entry list?
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
                                </div>
                            </div>
                            <%--</div>--%>
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
                            <%--<div class="row">
                                <div class="col-md-12">
                                    <div class="col-md-1">
                                        <label>Remarks<span style="color: red">*</span></label>
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtYourRemark" runat="server" Rows="3" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Save" runat="server" ErrorMessage="Enter your remarks" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtYourRemark"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-5">
                                        <asp:Button ID="btnApprove" CssClass="btn btn-success" ValidationGroup="Save" runat="server" Text="Approve" OnClick="btnApprove_OnClick" />
                                        <asp:Button ID="btnHold" CssClass="btn btn-warning" runat="server" ValidationGroup="Save" Text="Hold" OnClick="btnHold_OnClick" />
                                        <asp:Button ID="btnReturn" CssClass="btn btn-warning" runat="server" ValidationGroup="Save" Text="Return" OnClick="btnReturn_Click" />
                                        <asp:Button ID="btnDecline" CssClass="btn btn-danger" runat="server" ValidationGroup="Save" Text="Decline" OnClick="btnDecline_OnClick" />
                                    </div>
                                </div>

                            </div>--%>
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

