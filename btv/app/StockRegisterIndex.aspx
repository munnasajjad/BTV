<%@ Page Title="Stock Register Index" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="StockRegisterIndex.aspx.cs" Inherits="app_StockRegisterIndex" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BodyContent" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server"> </asp:ScriptManager>

<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="pnl" runat="server">
<ProgressTemplate>
<div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
<img src="../images/loader.gif" alt="Processing... Please Wait." /></div>
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
<legend>Stock Register Index</legend>
<table border="0" class="membersinfo tdfirstright bg-green" width="100%">
<tr>
<td align="center" colspan="2">
<asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
<asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
</td></tr>


<tr>
<td>Stock Reg I D</td>
<td>
<asp:DropDownList ID="ddStockRegID" runat="server" CssClass="form-control select2me"
     AutoPostBack="False" OnSelectedIndexChanged="ddStockRegID_SelectedIndexChanged">
</asp:DropDownList>
</td>
</tr>


<tr>
<td>Store I D</td>
<td>
<asp:DropDownList ID="ddStoreID" runat="server" CssClass="form-control select2me"
     AutoPostBack="False" OnSelectedIndexChanged="ddStoreID_SelectedIndexChanged">
</asp:DropDownList>
</td>
</tr>


<tr>
<td>Produc I D</td>
<td>
<asp:DropDownList ID="ddProducID" runat="server" CssClass="form-control select2me"
     AutoPostBack="False" OnSelectedIndexChanged="ddProducID_SelectedIndexChanged">
</asp:DropDownList>
</td>
</tr>


<tr>
<td>Remarks</td>
<td>
<asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
</td>
</tr>


<tr style="background: none">
<td></td>
<td>
<asp:Button ID="btnSave" CssClass="btn btn-s-md btn-primary" runat="server" Text="Save" OnClick="btnSave_OnClick" />
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
<legend>Saved Data</legend>
<asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped" AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE"
    BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black"
    GridLines="Vertical" DataKeyNames="StockRegIndexID" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged" OnRowDeleting="GridView1_OnRowDeleting">
<Columns>

<asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
<ItemTemplate>
<%#Container.DataItemIndex+1 %>.
<asp:Label ID="Label1" runat="server" visible="false" Text='<%# Bind("StockRegIndexID") %>'></asp:Label>
</ItemTemplate>
<ItemStyle Width="20px" HorizontalAlign="Center" />
</asp:TemplateField>
<asp:BoundField DataField="StockRegID" HeaderText="Stock Reg I D" SortExpression="StockRegID" /><asp:BoundField DataField="StoreID" HeaderText="Store I D" SortExpression="StoreID" /><asp:BoundField DataField="ProducID" HeaderText="Produc I D" SortExpression="ProducID" /><asp:BoundField DataField="Remarks" HeaderText="Remarks" SortExpression="Remarks" />
<asp:TemplateField ItemStyle-HorizontalAlign="Center" ShowHeader="False">
<ItemTemplate>
<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/edit.png" Text="Select" />
<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" />
<asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
<asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
<asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
<b style="color: red">This entry will be deleted permanently!</b><br />
Are you sure you want to delete this ?<br /><br />
<div style="text-align: right;">
<asp:Button ID="ButtonOk" runat="server" CssClass="btn btn-success" Text="OK" />
<asp:Button ID="ButtonCancel" CssClass="btn_small btn_orange" runat="server" Text="Cancel" />
</div>
</asp:Panel>
</ItemTemplate>
</asp:TemplateField>
</Columns>
<RowStyle BackColor="#F7F7DE" CssClass="txtMult" />
<PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
<SelectedRowStyle BackColor="#EEF7F2" Font-Bold="True" ForeColor="#615B5B" />
<HeaderStyle BackColor="#FF6600" Font-Bold="True" ForeColor="#222" />
<AlternatingRowStyle BackColor="White" />
</asp:GridView>

</fieldset>
</section>
</div>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>





