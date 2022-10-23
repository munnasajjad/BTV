<%@ Page Title="Terms-and-Conditions" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Terms-and-Conditions.aspx.cs" Inherits="app_Terms_and_Conditions" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="deshboard" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BodyContent" runat="server">  



    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="uPanel" runat="server">
        <ProgressTemplate>
            <div id="IMGDIV" style="position: fixed; left: 95%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: White; z-index: 1000;">
                <img src="../images/loader.gif" alt="Processing... Please Wait." />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="uPanel" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
        <ContentTemplate>

            <script type="text/javascript" language="javascript">
                Sys.Application.add_load(callJquery);
            </script>


                    <h2>Reservation Terms and Conditions</h2>

            <div class="col-lg-12">
                <section class="panel">
                    
                    <%--<asp:Literal ID="lblContent" runat="server"></asp:Literal>--%>

                    <asp:Panel ID="pnlEdit" runat="server">
                        <legend>Edit T&A</legend>
                        <CKEditor:CKEditorControl ID="PageContents" BasePath="~/ckeditor/" runat="server" Height="500px"></CKEditor:CKEditorControl>
                    </asp:Panel>
                    <br />
                    <asp:Label ID="lblMsg" runat="server" EnableViewState="False"></asp:Label>
                    <div class="clear2 center">
                        <asp:Button ID="btnSave" runat="server" Text="Edit T&A" CssClass="btn btn-s-md btn-primary" OnClick="btnSave_Click" />
                    </div>
                </section>
            </div>



        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Foot" runat="server">
</asp:Content>


