<%@ Page Title="Welcome!" Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8">
    <title>Bungalow Management System</title>
    <meta name="description" content="Login to Bungalow Management System of Ministry of Roads And Highways Department RHD. GOVERNMENT OF THE PEOPLE'S REPUBLIC OF BANGLADESH.">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="stylesheet" type="text/css" href="./css/login-style.css">

    <script src="js/jquery-1.8.3.min.js"></script>
    <script type="text/javascript">
        $('.Login .Textbox').focus(function () {
            $(this).attr('class', 'Hover');
        });

        $('.Login .Textbox').blur(function () {
            $(this).attr('class', 'Textbox');
        });

        $(document).ready(function () {
            $(":text.Textbox")(function () {
                $(this).addClass('focus');
            }).blur(function () {
                $(this).removeClass('focus');
            });
        });
    </script>
    <style>
        span.copyright {
            margin-top: 10px !important;
        }
        .error{
            color:red !important;
        }
        .success{
            color:green !important;
        }
        .btn {
            color: white;
            padding: 8px 20px;
            margin: 8px 0;
            cursor: pointer;
            width: 100%;
            background-color: #007bff;
            border-color: #007bff;
            font-size: 18px;
            border-radius: 4px;
        }
        
h2 {
    margin-top: 0 !important;
}
input[type=submit] {
    margin: 10px 0 !important;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="upl" runat="server">
            <ProgressTemplate>
                <div id="IMGDIV" style="position: absolute; left: 50%; top: 50%; vertical-align: middle; border-style: inset; border-color: black; background-color: transparent; z-index: 1000;">
                    <img src="./images/loader.gif" alt="Processing... Please Wait." />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
            <ContentTemplate>



                <div class="mainlogin-container" id="bgImg" runat="server">
                    <%--<a href="#" class="btn btn-primary">Find Room</a>--%>
                   
                    <div class="mainlogin-section">
                        <div class="login-logo">
                            <img src="./branding/LoginLogo.png" />
                        </div>

                        <div runat="server" id="clsAttriute" class="error">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                            
                            <asp:Label ID="Label1" runat="server" EnableViewState="False"></asp:Label>
                        </div>

                        <asp:Label ID="lblMsg2" runat="server" EnableViewState="False"></asp:Label>
                        <%--Login Panel--%>
                        <asp:Panel DefaultButton="Login1$LoginButton" runat="server" ID="pnlLogin" CssClass="login-form">

                            <div class="login-text">
                                <h2>Sign in</h2>
                            </div>

                            <asp:Login ID="Login1" runat="server" TitleText="Login to your Account"
                                OnLoggedIn="Login1_LoggedIn" OnLoginError="Login1_LoginError" Width="100%"
                                FailureText="Invalid Username or Password" OnLoggingIn="Login1_LoggingIn">
                                <LayoutTemplate>

                                    <label>
                                        <b>Email/ Username</b>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1" ForeColor="Red" Font-Bold="true">*</asp:RequiredFieldValidator></label>
                                    <asp:TextBox ID="UserName" runat="server" placeholder="Enter Your Login ID" name="uname" onchange="makeLower(this)"></asp:TextBox>
                                    <script>
                                        function makeLower(ctrl) {
                                            ctrl.value = ctrl.value.toLowerCase().trim();
                                        }
                                    </script>
                                    <label>
                                        <b>Password</b>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1" ForeColor="Red" Font-Bold="true">*</asp:RequiredFieldValidator></label>
                                    <asp:TextBox ID="Password" runat="server" TextMode="Password" placeholder="Enter Password" name="psw"></asp:TextBox>

                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Sign me in" ValidationGroup="Login1" />

                                </LayoutTemplate>
                            </asp:Login>

                            <asp:CheckBox ID="chkRemember" runat="server" Text="Remember me" Checked="true" />

                            <span class="psw">
                                <asp:LinkButton ID="lbForgot" runat="server" OnClick="lbForgot_Click">Forgot password?</asp:LinkButton></span>
                            <br />

                        </asp:Panel>


                        <%--Login Status--%>
                        <asp:Panel ID="PanelError" runat="server" EnableViewState="false">
                            <asp:Label ID="lblMsg3" runat="server" EnableViewState="False"></asp:Label>
                            <div class="login_invalid" runat="server" visible="False">
                                <span class="icon"></span>

                                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label>
                                <asp:Literal ID="Literal1" runat="server" EnableViewState="False"></asp:Literal>
                                To Try Again <a href="Login.aspx">Click Here</a>
                            </div>
                        </asp:Panel>
                        <%--Forgot Password--%>
                        <asp:Panel DefaultButton="btnNext" Visible="false" runat="server" ID="pnlForgot" CssClass="login-form">

                            <div class="login-text">
                                <h2>Password Reset</h2>
                            </div>

                            <i>If you forgot your password, please enter your email/username by which you are authorized to sign into customer portal. If you forgot your username, please contact with customer service team for further assistance. </i>
                            <br />
                            <br />
                            <label>
                                <b>Email/ Username</b>
                                <asp:RequiredFieldValidator ID="tUserNameRequired" runat="server" ControlToValidate="txtUserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="Login1" ForeColor="Red" Font-Bold="true">*</asp:RequiredFieldValidator></label>
                            <asp:TextBox ID="txtUserName" runat="server" placeholder="Enter Email/ Username" name="uname"></asp:TextBox>

                            <asp:CheckBox ID="CheckBox1" runat="server" Text="I am authorized to reset this account password." />
                            <asp:Button ID="btnNext" runat="server" Text="Send Reset Link" OnClick="btnNext_Click" />

                            <span class="psw">
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Cancel (Go to Sign in page)</asp:LinkButton></span>
                        </asp:Panel>

                        <%--Subscribe--%>
                        <asp:Panel DefaultButton="btnSubscribe" Visible="false" runat="server" ID="pnlSubscribe" CssClass="login-form">

                            <div class="login-text">
                                <h2>Subscribe</h2>
                            </div>

                            <i>By becoming a subscriber you agree to receive promotional & discounted products announcements from EXTREME SOLUTIONS</i>
                            <br />
                            <br />
                            <label>
                                <b>Your Name</b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required." ToolTip="Name is required." ValidationGroup="Login1" ForeColor="Red" Font-Bold="true">*</asp:RequiredFieldValidator></label>
                            <asp:TextBox ID="txtName" runat="server" placeholder="Enter Your Name"></asp:TextBox>

                            <label>
                                <b>Your Email Address</b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." ToolTip="Email is required." ValidationGroup="Login1" ForeColor="Red" Font-Bold="true">*</asp:RequiredFieldValidator></label>
                            <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter Email Address"></asp:TextBox>

                            <asp:CheckBox ID="CheckBox2" runat="server" Text="I am the owner of this email account" />
                            <asp:Button ID="btnSubscribe" runat="server" Text="Subscribe" OnClick="btnSubscribe_Click" />

                            <span class="psw">
                                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton1_Click">Cancel (Go to Sign in page)</asp:LinkButton></span>
                        </asp:Panel>

                        <%--Reset confirm--%>
                        <asp:Panel DefaultButton="btnFinish" Visible="false" runat="server" ID="pnlFinish" CssClass="login-form" onc>

                            <div class="login-text">
                                <h2>Setup a new password</h2>
                            </div>

                            <i id="reseText" runat="server"></i>
                            <br />
                            <br />
                            <label>
                                <b>Your New Password</b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1" ForeColor="Red" Font-Bold="true">*</asp:RequiredFieldValidator></label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>

                            <label>
                                <b>Confirm New Password</b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtConfirm" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1" ForeColor="Red" Font-Bold="true">*</asp:RequiredFieldValidator></label>
                            <asp:TextBox ID="txtConfirm" runat="server" TextMode="Password"></asp:TextBox>

                            <asp:Button ID="btnFinish" runat="server" Text="Finish" OnClick="btnFinish_Click" />

                            <span class="psw">
                                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton1_Click">Cancel (Go to Sign in page)</asp:LinkButton></span>
                        </asp:Panel>

                        <div style="text-align: center; display:none;">
                            <br />
                           Don't have an account? <a style="color:#007bff" href="Sign-Up.aspx">Sign Up</a>
                            <br />
                            <br />
                            <a href="Default.aspx" class="btn btn-priamary">Find Room</a>
                            <br />
                            <span class="copyright">&copy;
                                <asp:Literal ID="ltrYr" runat="server" />
                                <a href="http://rhd.portal.gov.bd/">Roads and Highways Department</a></span>

                            
                           

                            <a href="http://os.com.bd" target="_blank" style="font-size: 12px; line-height: 5px;">powered by-
                         <img src="branding/os.png" alt="Optimum Soft" style="margin: 0 auto;" /></a>

                        </div>



                    </div>

                    <div class="loginpage-vedio">
                        <%--<iframe width="850" height="550" src="https://www.youtube.com/embed/60axGwcFZNY" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>--%>
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="color: #333; bottom: 0px; position: fixed; right: 10px;">
            <asp:HiddenField runat="server" ID="hfDeviceID" />
            Device ID:
            <asp:Label runat="server" ID="lblDeviceID" Text="test" />
        </div>

    </form>
    <script src="./js/client.min.js"></script>
    <script type="text/javascript">
        var client = new ClientJS(); // Create A New Client Object
        var fingerprint = client.getFingerprint(); // Get Client's Fingerprint
        document.getElementById("<%= hfDeviceID.ClientID %>").value = fingerprint;
        document.getElementById("<%= lblDeviceID.ClientID %>").innerHTML = fingerprint;

    </script>
</body>
</html>
