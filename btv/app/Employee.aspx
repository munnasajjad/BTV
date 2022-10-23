<%@ Page Title="Employee" Language="C#" MasterPageFile="~/app/MasterPage.Master" AutoEventWireup="true" CodeFile="Employee.aspx.cs" Inherits="app_Employee" %>

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
                        <legend>Employee</legend>
                        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                    <asp:Label ID="lblId" Visible="false" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>Employee ID<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEmpId" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmpId" ValidationGroup="Save" runat="server" ErrorMessage="Enter Employee ID"></asp:RequiredFieldValidator>

                                </td>
                            </tr>

                            <tr>
                                <td>Name<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtName" ValidationGroup="Save" runat="server" ErrorMessage="Enter Name"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Designation</td>
                                <td>
                                    <asp:DropDownList ID="ddDesignationID" runat="server" CssClass="form-control select2me" Width="100%"
                                        AutoPostBack="False" OnSelectedIndexChanged="ddDesignationID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>Gender</td>
                                <td>
                                    <asp:DropDownList ID="ddlGender" runat="server" Width="100%" CssClass="form-control select2me">
                                        <asp:ListItem>Male</asp:ListItem>
                                        <asp:ListItem>Female</asp:ListItem>
                                        <asp:ListItem>Other</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtGender" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Blood Group</td>
                                <td>
                                    <asp:DropDownList ID="ddlBloodGroup" runat="server" Width="100%" CssClass="form-control select2me">
                                        <asp:ListItem>A+</asp:ListItem>
                                        <asp:ListItem>A-</asp:ListItem>
                                        <asp:ListItem>B+</asp:ListItem>
                                        <asp:ListItem>B-</asp:ListItem>
                                        <asp:ListItem>AB+</asp:ListItem>
                                        <asp:ListItem>AB-</asp:ListItem>
                                        <asp:ListItem>O+</asp:ListItem>
                                        <asp:ListItem>O-</asp:ListItem>
                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtBloodGroup" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Material Status</td>
                                <td>
                                    <asp:DropDownList ID="ddlMarriedStatus" runat="server" Width="100%" CssClass="form-control select2me">
                                        <asp:ListItem>Married</asp:ListItem>
                                        <asp:ListItem>Unmarried</asp:ListItem>

                                    </asp:DropDownList>
                                    <%--<asp:TextBox ID="txtMaterialStatus" runat="server" CssClass="form-control"></asp:TextBox>--%>
                                </td>
                            </tr>


                            <tr>
                                <td>Mobile<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtMobile" runat="server" AutoPostBack="True" OnTextChanged="txtMobile_OnTextChanged" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtMobile" ValidationGroup="Save" runat="server" ErrorMessage="Enter Mobile Number"></asp:RequiredFieldValidator>
                                    <asp:FilteredTextBoxExtender ID="ftbInvoiceNo" runat="server" FilterType="Custom" ValidChars="+0123456789" TargetControlID="txtMobile" />
                                </td>
                            </tr>


                            <tr>
                                <td>Telephone Office</td>
                                <td>
                                    <asp:TextBox ID="txtTelephoneOffice" runat="server" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr>
                                <td>Email<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtEmail" ValidationGroup="Save" runat="server" ErrorMessage="Enter Email"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regexEmailValid" ValidationGroup="Save" Display="Dynamic" runat="server" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Please enter valid email address"></asp:RegularExpressionValidator>
                                </td>
                            </tr>


                            <tr>
                                <td>Education Qualifiaction</td>
                                <td>
                                    <asp:DropDownList ID="ddEduQualifiactionID" runat="server" CssClass="form-control select2me" Width="100%"
                                        AutoPostBack="False" OnSelectedIndexChanged="ddEduQualifiactionID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>Date Of Birth<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDOB" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDOB" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDOB" ValidationGroup="Save" runat="server" ErrorMessage="Enter Date of Birth"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Father Name<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtFatherName" ValidationGroup="Save" runat="server" ErrorMessage="Enter Father Name"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>NID Number<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtNID" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtNID" ValidationGroup="Save" runat="server" ErrorMessage="Enter NID Number"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Relagion</td>
                                <td>
                                    <asp:DropDownList ID="ddRelagion" CssClass="select2me" Width="100%" runat="server">
                                        <asp:ListItem Value="Islam">Islam</asp:ListItem>
                                        <asp:ListItem Value="Hindu">Hindu</asp:ListItem>
                                        <asp:ListItem Value="Buddhist">Buddhist</asp:ListItem>
                                        <asp:ListItem Value="Christian">Christian</asp:ListItem>
                                        <asp:ListItem Value="Others">Others</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>


                            <tr>
                                <td>Present Address<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtPresentAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPresentAddress" ValidationGroup="Save" runat="server" ErrorMessage="Enter Present Address"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr>
                                <td>Permanent Address</td>
                                <td>
                                    <asp:TextBox ID="txtPermanentAddress" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                                </td>
                            </tr>


                            <tr>
                                <td>Date Of Joining<span class="required">*</span></td>
                                <td>
                                    <asp:TextBox ID="txtDateOfJoining" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:CalendarExtender ID="ceDateOfJoining" runat="server" Format="dd/MM/yyyy" TargetControlID="txtDateOfJoining" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtDateOfJoining" ValidationGroup="Save" runat="server" ErrorMessage="Enter Date Of Joining"></asp:RequiredFieldValidator>

                                </td>
                            </tr>


                            <tr class="hidden">
                                <td>Office</td>
                                <td>
                                    <asp:DropDownList ID="ddOfficeID" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="False" OnSelectedIndexChanged="ddOfficeID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Main Office</td>
                                <td>
                                    <asp:DropDownList ID="ddLocationID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" OnSelectedIndexChanged="ddLocationID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Functional Office</td>
                                <td>
                                    <asp:DropDownList ID="ddCenterID" Width="100%" runat="server" CssClass="form-control select2me"
                                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddCenterID_OnSelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Department Section</td>
                                <td>
                                    <asp:DropDownList ID="ddDepartmentSectionID" AppendDataBoundItems="true" Width="100%" AutoPostBack="true" runat="server" CssClass="form-control select2me"
                                        OnSelectedIndexChanged="ddDepartmentSectionID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Upload Signature</td>
                                <td>
                                    <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" CssClass="form-control" Width="45%" />
                                    <asp:Image ID="imgLogo" runat="server" Width="100px" />
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
                        <legend>Saved Data</legend>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtSearch" runat="server" placeholder="Search by Name, Mobile, Email" CssClass="form-control"></asp:TextBox>
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
                                                <asp:TemplateField ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center" ShowHeader="False">
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
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="#Sl" ItemStyle-Width="20px">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                        <asp:Label ID="Label1" runat="server" Visible="false" Text='<%# Bind("EmployeeID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                <asp:BoundField DataField="Designation" HeaderText="Designation" SortExpression="Designation" />
                                                <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                                                <%--<asp:BoundField DataField="BloodGroup" HeaderText="Blood Group" SortExpression="BloodGroup" />--%>
                                                <%--<asp:BoundField DataField="MaterialStatus" HeaderText="Material Status" SortExpression="MaterialStatus" />--%>
                                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" SortExpression="Mobile" />
                                                <%--<asp:BoundField DataField="TelephoneOffice" HeaderText="Telephone Office" SortExpression="TelephoneOffice" />--%>
                                                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                                <asp:BoundField DataField="Education" HeaderText="Education Qualifiaction" SortExpression="Education" />
                                                <%--<asp:BoundField DataField="DOB" HeaderText="D O B" SortExpression="DOB" />--%>
                                                <%--<asp:BoundField DataField="FatherName" HeaderText="Father Name" SortExpression="FatherName" />--%>
                                                <asp:BoundField DataField="NID" HeaderText="NID" SortExpression="NID" />
                                                <%--<asp:BoundField DataField="Relagion" HeaderText="Relagion" SortExpression="Relagion" />--%>
                                                <%--<asp:BoundField DataField="PresentAddress" HeaderText="Present Address" SortExpression="PresentAddress" />--%>
                                                <%--<asp:BoundField DataField="PermanentAddress" HeaderText="Permanent Address" SortExpression="PermanentAddress" />--%>
                                                <%--<asp:BoundField DataField="DateOfJoining" HeaderText="Date Of Joining" SortExpression="DateOfJoining" />--%>
                                                <%--<asp:BoundField DataField="OfficeID" HeaderText="Office I D" SortExpression="OfficeID" />--%>
                                                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />

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





