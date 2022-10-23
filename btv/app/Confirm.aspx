<%@ Page Title="Confirm" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Confirm.aspx.cs" Inherits="app_Confirm" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" runat="Server">
    <style>
        .content {
            max-width: 1200px;
            margin: 0 auto;
            border: 1px solid #eee;
            background-color: #fff;
        }

        .alert-danger {
            color: #a94442;
            background-color: #f2dede;
            border-color: #ebccd1;
            text-align: center;
        }

        .alert-success {
            text-align: center;
        }

        .main {
            margin-top: 10%;
            height: 50%;
        }

        input#bodycontent_btnConfirm {
            text-align: center;
            margin: 10px 46%;
            /* position: fixed; */
            line-height: 30px;
        }
    </style>
    <link href="templates/2/source/css/style.css" rel="stylesheet" />
    <%--</asp:Content>--%>
    <%--<asp:Content ID="Content2" ContentPlaceHolderID="bodycontent" runat="Server">--%>
    <div class="content">
        <div class="main">
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6" runat="server" id="message">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </div>
                <div class="col-md-3"></div>
            </div>
            <div class="row">
                <div class="col-md-3"></div>
                <div class="col-md-6">
                    <div class="card">

                        <asp:Panel ID="pnlSuccess" runat="server">
                            <div class="container-fluid">

                                <div class="row-fluid">
                                    <div class="portlet box blue">
                                        <div class="preview">
                                            <div class="row">
                                                <div class="col-md-12 text-center">
                                                    <img src="../branding/LoginLogo.png" style="height: 150px; padding-bottom: 20px" />
                                                </div>

                                            </div>
                                            <div class="row1">
                                                <div class="verify-section text-center" style="padding: 20px; border: 4px dotted #00c1cf;">
                                                    <img src="../branding/confirm.png" alt="Email Confirm" style="margin: 0px auto; width: 100px; display: block">

                                                    <p>
                                                        <h2 style="text-align: center; color: #4ab30a;"><strong>Congratulations</strong></h2>
                                                        <h3 style="text-align: center; color: #4ab30a;">Email verification has been successfully done</h3>
                                                    </p>

                                                    <a href="../Login" target="_blank" class="btn btn-warning btn-block" style="width: 30%; margin-left: 150px" type="button">GO TO LOG IN</a>



                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
                <div class="col-md-3"></div>

            </div>
        </div>
    </div>
</asp:Content>



