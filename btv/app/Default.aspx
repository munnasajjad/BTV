<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="AdminCentral_Default" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=15.1.4.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<script src="DataTables/jquery.dataTables.min.js"></script>

    <link rel="stylesheet" type="text/css" href="//cdn.datatables.net/1.10.4/css/jquery.dataTables.min.css">--%>
    <style>
        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
            opacity: 0.93 !important;
        }

        /* Modal Content */
        .modal-content {
            background-color: #fefefe;
            margin: auto;
            padding: 20px;
            border: 1px solid #888;
            width: 80%;
        }

        /* The Close Button */
        .close {
            font-size: 40px !important;
            line-height: 1.5 !important;
            opacity: 10 !important;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }

        .btn {
            color: #000;
            background-color: transparent;
            outline: 0;
            -moz-transition: background-color 0.3s ease;
            -o-transition: background-color 0.3s ease;
            -webkit-transition: background-color 0.3s ease;
            transition: background-color 0.3s ease;
            font-family: 'Ubuntu', sans-serif;
        }

        .btn-square {
            border: 0;
        }

        .btn-lg {
            padding: 10px 37.5px 10px 37.5px;
            font-size: 1.2em;
        }

        .btn-ghost-green {
            border: 2px solid #2ecc71 !important;
        }

            .btn-ghost-green:hover {
                background-color: #2ecc71;
            }

            .btn-ghost-green:active {
                background-color: #25a25a;
            }

        .btn-ghost-red {
            border: 2px solid #ca0710 !important;
        }

            .btn-ghost-red:hover {
                background-color: #ca0710;
            }

            .btn-ghost-red:active {
                background-color: #ca0710;
            }

        .btn-ghost-blue {
            border: 2px solid #07afca !important;
        }

            .btn-ghost-blue:hover {
                background-color: #07afca;
            }

            .btn-ghost-blue:active {
                background-color: #07afca;
            }

        .btn-ghost-yellow {
            border: 2px solid #f8f348 !important;
        }

            .btn-ghost-yellow:hover {
                background-color: #f8f348;
            }

            .btn-ghost-yellow:active {
                background-color: #f8f348;
            }

        .custom h6 {
            overflow: hidden; /* Ensures the content is not revealed until the animation */
            border-right: .50em solid orange; /* The typwriter cursor */
            color: #1e90ff;
            text-transform: uppercase;
            white-space: nowrap; /* Keeps the content on a single line */
            margin: 0 auto; /* Gives that scrolling effect as the typing happens */
            letter-spacing: .12em; /* Adjust as needed */
            animation: typing 3.5s steps(50, end), blink-caret .75s step-end infinite;
        }

        /* The typing effect */
        @keyframes typing {
            from {
                width: 0
            }

            to {
                width: 100%
            }
        }

        /* The typewriter cursor effect */
        @keyframes blink-caret {
            from, to {
                border-color: transparent
            }

            50% {
                border-color: orange;
            }
        }

        h6 span {
            color: red;
            font-weight: bold;
        }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="deshboard" runat="server">


    <div class="page_title">
        <div class="live_news">
            <marquee class="smooth_m" behavior="scroll" scrollamount="3" direction="left" width="100%" onmouseout="start()" onmouseover="stop()">
	    <h3>
	        <%--Welcome to Message Courier Service Limited. Best of the bests courier service and money transfer company in Bangladesh.--%>
	        <asp:Label ID="lblNews" runat="server" Text=""></asp:Label>
	    </h3></marquee>
        </div>
    </div>
    <div id="SwitchBar" class="switch_bar" runat="server" hidden>
        <ul>
            <li id="menuID167" runat="server"><a href="Employee"><span class="stats_icon customers_sl"></span><span class="label">Employees</span></a></li>
            <li id="menuID170" runat="server"><a href="CreateUser"><span class="stats_icon user_sl"></span><span class="label">Create User</span></a></li>

            <li id="menuID174" runat="server" visible="false"><a href="Guest_Info"><span class="stats_icon  archives_sl"></span><span class="label">User Data</span></a></li>

            <li id="menuID173" runat="server"><a href="Product"><span class="stats_icon current_work_sl"></span><span class="label">Products Info</span></a></li>
            <li id="menuID" runat="server"><a href="GRNForm"><span class="stats_icon issue_sl"></span><span class="label">GRN</span></a></li>
            <li id="lvID" runat="server"><a href="LoanVoucher"><span class="stats_icon category_sl"></span><span class="label">LV</span></a></li>
            <li id="Li1" runat="server"><a href="ReturnVaucher"><span class="stats_icon category_sl"></span><span class="label">RV</span></a></li>
            <li id="Li2" runat="server"><a href="SIRForm"><span class="stats_icon communication_sl"></span><span class="label">SIR</span></a></li>
            <li id="menuID175" runat="server"><a href="#"><span class="stats_icon address_sl"></span><span class="label">Request</span></a></li>




            <li id="menuID171" runat="server" visible="false"><a href="Booking_App"><span class="stats_icon upcoming_work_sl"></span><span class="label">Approve</span></a></li>
            <li id="menuID172" runat="server" visible="false"><a href="AssignBungalow"><span class="stats_icon communication_sl"></span><span class="label">Security</span></a></li>

            <li id="menuID176" runat="server"><a href="Profile"><span class="stats_icon issue_sl"></span><span class="label">Profile</span></a></li>
        </ul>
    </div>
    <span class="clear"></span>
    <asp:Panel ID="pnlNotify" Visible="false" runat="server">
        <div class="social_activities">
            <asp:Label ID="lblNotify" runat="server"></asp:Label>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlStat" runat="server">
        <table border="0" class="membersinfo tdfirstright bg-green" width="100%">
            <tr>
                <td>Main Office</td>
                <td>Functional Office</td>
                <td>Department</td>
                <td>Store</td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddLocationID" Width="100%" runat="server" CssClass="form-control select2me"
                        AutoPostBack="True" OnSelectedIndexChanged="ddLocationID_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddCenterID" Width="100%" runat="server" CssClass="form-control select2me"
                        AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddCenterID_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddDepartmentSectionID" AppendDataBoundItems="true" Width="100%" AutoPostBack="true" runat="server" CssClass="form-control select2me" OnSelectedIndexChanged="ddDepartmentSectionID_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="ddStoreID" Width="100%" runat="server" CssClass="form-control select2me" AutoPostBack="True" OnSelectedIndexChanged="ddStoreID_OnSelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Date From</td>
                <td>Date To</td>
                <td>Highly Purchased Item(s) & Top Item Consumption</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control" OnTextChanged="txtDateFrom_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender1"  runat="server" Enabled="True" TargetControlID="txtDateFrom" Format="dd/MM/yyyy"></asp:CalendarExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control" OnTextChanged="txtDateTo_OnTextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True" TargetControlID="txtDateTo" Format="dd/MM/yyyy"></asp:CalendarExtender>
                </td>
                <td>
                    <asp:TextBox ID="txtHighlyPurchasedItem" runat="server" CssClass="form-control" AutoPostBack="True" OnTextChanged="txtHighlyPurchasedItem_OnTextChanged"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="FilteredtxtHighlyPurchasedItem" runat="server" FilterType="Numbers" ValidChars="0123456789" TargetControlID="txtHighlyPurchasedItem"/></td>
            </tr>
        </table>




        <div class="social_activities">
            <div class="activities_s">
                <div class="block_label">
                    Total Employee
                    <span><b style='color: #990099'>
                        <asp:Literal ID="labelTotalEmployee" runat="server">0</asp:Literal>
                    </b></span>
                </div>
            </div>
            <div class="activities_s">
                <div class="block_label">
                    Total Department
                    <span><b style='color: #dc0b0b'>
                        <asp:Literal ID="labelTotalDepartment" runat="server"></asp:Literal>
                    </b></span>
                </div>
            </div>

            <div class="activities_s">
                <div class="block_label">
                    <asp:Literal ID="ltrProfit" runat="server" Text="Total Users" />
                    <span>
                        <asp:Literal ID="labelUsers" runat="server" /></span>
                </div>
            </div>
            <div class="activities_s">
                <div class="block_label">
                    <asp:Literal ID="ltrTarget" runat="server" Text="Total Functional Office" />
                    <span>
                        <asp:Literal ID="labelTotalFunctionalOffice" runat="server" /></span>
                </div>
            </div>
            <div class="activities_s">
                <div class="block_label">
                    Total Store<span><b style='color: #1690d6'><asp:Literal ID="labelTotalStore" runat="server"></asp:Literal></b></span>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <span class="clear"></span>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="span_6 col-lg-6">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <h6>Inventory Level by category</h6>
            </div>

            <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
            <div id="chart_div" style="height: 250px;"></div>
            <script>
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawAnnotations);

                function drawAnnotations() {
                    var data = google.visualization.arrayToDataTable([
                        <asp:Literal ID="inventoryLevelByCategoryStore" runat="server"></asp:Literal >
                    ]);

                    var options = {
                        title: 'Inventory Level by category store',
                        chartArea: { width: '50%' },
                        annotations: {
                            alwaysOutside: true,
                            textStyle: {
                                fontSize: 12,
                                auraColor: 'none',
                                color: '#555'
                            },
                            boxStyle: {
                                stroke: '#ccc',
                                strokeWidth: 1,
                                gradient: {
                                    color1: '#f3e5f5',
                                    color2: '#f3e5f5',
                                    x1: '0%', y1: '0%',
                                    x2: '100%', y2: '100%'
                                }
                            }
                        },
                        hAxis: {
                            title: 'Total Items',
                            minValue: 0
                        },
                        vAxis: {
                            title: 'Category'
                        }
                    };
                    var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
                    chart.draw(data, options);
                }
            </script>

        </div>
    </div>


    <div class="span_6 col-lg-12">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <h6>ITEM IN STORE</h6>
            </div>

            <div id="piechart" style="height: 250px;"></div>
            <script>
                google.charts.load('current', { 'packages': ['corechart'] });
                google.charts.setOnLoadCallback(drawChart);


                function drawChart() {

                    var data = google.visualization.arrayToDataTable([
                        <asp:Literal ID="itemInStoreLiteral" runat="server"></asp:Literal >
                    ]);

                    var options = {
                        title: ''
                    };

                    var chart = new google.visualization.PieChart(document.getElementById('piechart'));
                    chart.draw(data, options);
                }
            </script>

        </div>
    </div>

<div class="span_6 col-lg-12">
    <div class="widget_wrap">
        <div class="widget_top">
            <span class="h_icon list_images"></span>
            <h6>HIGHLY PURCHASED ITEM</h6>
        </div>

        <div id="piechart2" style="height: 250px;"></div>
        <script>
            google.charts.load('current', { 'packages': ['corechart'] });
            google.charts.setOnLoadCallback(drawChart);


            function drawChart() {

                var data = google.visualization.arrayToDataTable([
                    <asp:Literal ID="test" runat="server"></asp:Literal>
                ]);

                var options = {
                    title: ''
                };

                var chart = new google.visualization.PieChart(document.getElementById('piechart2'));
                chart.draw(data, options);
            }
        </script>

    </div>
</div>

    <div class="span_6 col-lg-6">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <h6>Purchase Amount analysis</h6>
            </div>
            <%--https://developers.google.com/chart/interactive/docs/gallery/combochart--%>
            <div id="combochart" style="height: 250px;"></div>
            <script>
                google.charts.load('current', { 'packages': ['corechart'] });
                google.charts.setOnLoadCallback(drawVisualization);

                function drawVisualization() {
                    // Some raw data (not necessarily accurate)
                    var data = google.visualization.arrayToDataTable([
                        <asp:Literal ID="purchaseAmountAnalysis" runat="server"></asp:Literal >
                    ]);

                    var options = {
                        title: 'Purchase Amount analysis',
                        vAxis: { title: 'Amount' },
                        hAxis: { title: 'Store' },
                        seriesType: 'bars',
                        series: { 5: { type: 'line' } }
                    };

                    var chart = new google.visualization.ColumnChart(document.getElementById('combochart'));
                    chart.draw(data, options);
                }
            </script>

        </div>
    </div>

<div class="span_6 col-lg-6">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <h6>Top Item consumption</h6>
            </div>
            <%--https://developers.google.com/chart/interactive/docs/gallery/combochart--%>
            <div id="combochart2" style="height: 250px;"></div>
            <script>
                google.charts.load('current', { 'packages': ['corechart'] });
                google.charts.setOnLoadCallback(drawVisualization);

                function drawVisualization() {
                    // Some raw data (not necessarily accurate)
                    var data = google.visualization.arrayToDataTable([
                        <asp:Literal ID="consumption" runat="server"></asp:Literal>
                    ]);

                    //var data = google.visualization.arrayToDataTable([
                    //    ['Year', 'Sales', 'Expenses', 'Profit'],
                    //    ['1'''' Brass ball float valve (4)', 1000, 400, 200],
                    //    ['2015', 1170, 460, 250],
                    //    ['2016', 660, 1120, 300],
                    //    ['2017', 1030, 540, 350]
                    //]);

                    var options = {
                        title: 'Top Item consumption',
                        vAxis: { title: 'Quantity' },
                        hAxis: { title: 'Item(s)' },
                        seriesType: 'bars',
                        series: { 5: { type: 'line' } }
                    };

                    var chart = new google.visualization.ComboChart(document.getElementById('combochart2'));
                    chart.draw(data, options);
                }
            </script>

        </div>
    </div>


    <div class="grid_12 hidden">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <h6>Activity Feeds</h6>
            </div>


            <div class="">

                <table style="width: 100%" id="tblActivity" runat="server" visible="false">
                    <tr>
                        <td style="width: 90%">
                            <asp:TextBox ID="txtActivity" runat="server" Width="100%" CssClass="form-control" ToolTip="Write your work tivity and press SAVE ACTIVITY"></asp:TextBox>
                        </td>
                        <td style="vertical-align: top; padding-top: 2px;">
                            <asp:Button ID="Button1" runat="server" Text="Save Activity" OnClick="Button1_Click" />
                        </td>
                        <td style="vertical-align: top; padding-top: 2px;">
                            <asp:Button ID="Button4" runat="server" Text="Cancel" OnClick="Button4_Click" CssClass="btn_orange" />
                        </td>
                    </tr>
                </table>

                <div style="width: 100%; text-align: center;">
                    <asp:LinkButton ID="lbActivity" runat="server" Text="Add my activity!" OnClick="lbActivity_Click" />
                </div>

                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                    CssClass="zebra table" BorderWidth="1px" Width="100%" CellPadding="3" OnSelectedIndexChanged="GridView2_SelectedIndexChanged"
                    DataKeyNames="tid" AllowSorting="True">
                    <RowStyle ForeColor="#000066" />
                    <Columns>

                        <asp:TemplateField HeaderText="CrID" SortExpression="CrID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("tid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="4%" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="TaskDetails" HeaderText="Activity Description" SortExpression="TaskDetails" ItemStyle-Width="85%" />
                        <%--<asp:BoundField DataField="DeadLine" HeaderText="DeadLine" SortExpression="DeadLine" DataFormatString="{0:d}" />
                                <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                    <ItemTemplate>
                                        <asp:Label ID="Label12" runat="server" CssClass='<%# Bind("pcss") %>' Text='<%# Bind("Priority") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" CssClass='<%# Bind("scss") %>' Text='<%# Bind("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                        <asp:BoundField DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" />


                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <%--<asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/approve.gif" Text="Approve" ToolTip="Approve" />--%>
                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Remove" ToolTip="Remove" />

                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                    Are you sure you want to delete the activity from the list?
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
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <EmptyDataTemplate>
                        <p style="text-align: center">No activity found from anyone!</p>
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="Green" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="Black" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                    SelectCommand="SELECT top(10) tid, [TaskDetails], [DeadLine], 'badge_style b_'+[Priority] as pcss, [Priority],'badge_style b_'+[Status] as scss, [Status], [EntryBy]
                            FROM [Tasks] WHERE TaskName='Activity' AND ([Status] &lt;&gt; @Status) AND (ProjectId = @ProjectId) ORDER BY [tid] desc"
                    DeleteCommand="Update Tasks set Status='inactive' where tid=@tid">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="inactive" Name="Status" Type="String" />
                        <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="tid" />
                    </DeleteParameters>
                </asp:SqlDataSource>

            </div>
        </div>
    </div>





    <span class="clear"></span>


    <div class="span_8 col-lg-8 hidden">
        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <h6>Tasks & Reminders</h6>
            </div>

            <div class="widget_content1">
                <div class="col-lg-12 hidden">
                    <br />
                    <%--Monthly summary for--%>
                    <asp:Label ID="lblUserName" runat="server" Visible="false" />
                    <div style="font-size: 16px; color: maroon;">
                        <asp:Literal ID="lblHistory" runat="server" Visible="false"></asp:Literal>
                    </div>
                </div>

                <section class="scrollable wrapper">
                    <div class="row">
                        <div class="col-lg-12">

                            <%--<img src="./images/for.jpg" />--%>


                            <div class="col-lg-6 hidden">
                                <%--<section class="panel dashboardpanel">--%>
                                <h5 style="text-align: center;">Last 3 days Attendance</h5>

                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-striped m-b-none text-sm" Width="100%" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True">
                                    <Columns>

                                        <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:M}" />
                                        <asp:BoundField DataField="Date" HeaderText="Day" SortExpression="Date" DataFormatString="{0:ddd}" />
                                        <asp:BoundField DataField="InTime" HeaderText="In" SortExpression="InTime" DataFormatString="{0:H:mm tt}" />
                                        <asp:BoundField DataField="OutTime" HeaderText="Out" SortExpression="OutTime" DataFormatString="{0:H:mm tt}" />
                                        <asp:BoundField DataField="WorkingTimeHr" HeaderText="hr." SortExpression="WorkingTimeHr" />

                                    </Columns>
                                    <EmptyDataTemplate>
                                        You Dont Have any attendance yet!
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <%--<asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT top(3) [Date], [InTime], [OutTime], [WorkingTimeHr] FROM [Attendance] WHERE ([EmployeeID] = @EmployeeID) order by lid desc">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblUserName" Name="EmployeeID" PropertyName="Text" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>--%>

                                <%--</section>--%>
                            </div>


                            <div class="col-lg-6 hidden">
                                <h5 style="text-align: center;">Responsibilities</h5>
                                <hr />
                                <asp:Image ID="Image2" runat="server" Width="100px" CssClass="right" />
                                <asp:Literal ID="lblResponsibilities" runat="server"></asp:Literal>

                            </div>
                            <div class="col-lg-12">

                                <table style="width: 100%">

                                    <tr>
                                        <td>&nbsp;</td>
                                        <td style="vertical-align: middle;">
                                            <asp:Label ID="Label3" runat="server" Text="Task Details:"></asp:Label>
                                            <span class="help-tip">
                                                <span>Add Your Task Information Here </span>
                                            </span>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtDetail" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Text="Deadline:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Format="dd/MM/yyyy" Enabled="True" TargetControlID="txtDate"></asp:CalendarExtender>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Priority:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddType" runat="server">
                                                <asp:ListItem>Low</asp:ListItem>
                                                <asp:ListItem>Medium</asp:ListItem>
                                                <asp:ListItem>High</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Save Task" OnClick="btnSave_Click" />
                                            <asp:Label ID="lblMsg" runat="server" Text="" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>

                                </table>



                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2"
                                    CssClass="zebra table" BorderWidth="1px" Width="100%" CellPadding="3" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    DataKeyNames="tid" AllowSorting="True">
                                    <RowStyle ForeColor="#000066" />
                                    <Columns>

                                        <asp:TemplateField HeaderText="CrID" SortExpression="CrID" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("tid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TaskDetails" HeaderText="Task Details" SortExpression="TaskDetails" />
                                        <asp:BoundField DataField="Deadline" HeaderText="Deadline" SortExpression="DeadLine" DataFormatString="{0:d}" />
                                        <asp:TemplateField HeaderText="Priority" SortExpression="Priority">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" runat="server" CssClass='<%# Bind("pcss") %>' Text='<%# Bind("Priority") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" CssClass='<%# Bind("scss") %>' Text='<%# Bind("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="EntryBy" HeaderText="EntryBy" SortExpression="EntryBy" />--%>


                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="False" CommandName="Select" ImageUrl="~/images/approve.gif" Text="Approve" ToolTip="Approve" />
                                                <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Delete" ImageUrl="~/images/delete.gif" Text="Delete" ToolTip="Delete" />

                                                <asp:ConfirmButtonExtender TargetControlID="ImageButton2" ID="confBtnDelete" runat="server" DisplayModalPopupID="ModalPopupExtender1"></asp:ConfirmButtonExtender>
                                                <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="ImageButton2"
                                                    PopupControlID="PNL" OkControlID="ButtonOk" CancelControlID="ButtonCancel" BackgroundCssClass="modalBackground" />
                                                <asp:Panel ID="PNL" runat="server" Style="display: none; width: 300px; background-color: White; border-width: 2px; border-color: Black; border-style: solid; padding: 20px;">
                                                    <b style="color: red">Item will be deleted permanently!</b><br />
                                                    Are you sure you want to delete the item from order list?
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
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <EmptyDataTemplate>
                                        You Dont Have any tasks yet!
                                    </EmptyDataTemplate>
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="Green" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="Black" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                                    SelectCommand="SELECT tid, [TaskDetails], [DeadLine], 'badge_style b_'+[Priority] as pcss, [Priority],'badge_style b_'+[Status] as scss, [Status], [EntryBy] FROM [Tasks] WHERE  TaskName='Admin' AND EntryBy=@EntryBy AND ([Status] &lt;&gt; @Status) AND (ProjectId = @ProjectId) ORDER BY [tid]"
                                    DeleteCommand="Update Tasks set Status='inactive' where tid=@tid">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="lblUserName" Name="EntryBy" PropertyName="Text" />
                                        <asp:Parameter DefaultValue="inactive" Name="Status" Type="String" />
                                        <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                                    </SelectParameters>
                                    <DeleteParameters>
                                        <asp:Parameter Name="tid" />
                                    </DeleteParameters>
                                </asp:SqlDataSource>

                            </div>
                        </div>


                    </div>

                </section>

            </div>
        </div>
    </div>


    <div class="span_4 col-lg-4 hidden">

        <div class="widget_wrap">
            <div class="widget_top">
                <span class="h_icon list_images"></span>
                <h6>Notice Board</h6>
            </div>

            <div class="widget_content1" style="min-height: 131px;">
                <section class="">
                    <footer class="panel-footer bg-light lter">
                        <section class="custom hidden">
                            <h6>
                                <asp:Literal runat="server" ID="ltrsms" />
                            </h6>
                        </section>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server"
                            ConnectionString="<%$ ConnectionStrings:Connection_String %>"
                            SelectCommand="SELECT  [Headline], (CONVERT(varchar,PublishDate,103) +' &nbsp; &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp;  &nbsp; ') AS [PublishDate], [FullNews] FROM [NewsUpdates] where Msgfor='News' AND  ProjectID=@ProjectID ORDER BY [MsgID] DESC">
                            <SelectParameters>
                                <asp:SessionParameter DefaultValue="0" Name="ProjectId" SessionField="ProjectID" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                        <br />
                        <asp:ListView ID="ListView1" runat="server" DataSourceID="SqlDataSource4">
                            <ItemTemplate>
                                <li>
                                    <h4 style="color: #C6480C">
                                        <asp:Label ID="HeadlineLabel" runat="server" Text='<%# Eval("Headline") %>' /></h4>
                                </li>
                                <li>
                                    <small style="color: skyblue">
                                        <asp:Label ID="PublishDateLabel" runat="server" Text='<%# Eval("PublishDate") %>' /></small>
                                </li>
                                <li>
                                    <asp:Label ID="FullNewsLabel" runat="server" Text='<%# Eval("FullNews") %>' />
                                    <hr />
                                </li>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <span>No notice was found!</span>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <ul class="nav nav-pills" id="itemPlaceholderContainer" runat="server">
                                    <span id="itemPlaceholder" runat="server" />
                                </ul>
                                <div class="grid-pager">
                                    <asp:DataPager ID="DataPager1" runat="server" PageSize="3">
                                        <Fields>
                                            <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True"
                                                ShowLastPageButton="True" />
                                        </Fields>
                                    </asp:DataPager>
                                </div>
                            </LayoutTemplate>
                        </asp:ListView>

                    </footer>
                </section>

            </div>
        </div>
    </div>

    <script>
        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the button that opens the modal
        var btn1 = document.getElementById("myBtn");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal 
        btn1.onclick = function () {
            modal.style.display = "none";
        }
        // Get the modal
        var modal2 = document.getElementById('myModal2');

        // Get the button that opens the modal
        var btn2 = document.getElementById("myBtn2");

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close2")[0];

        // When the user clicks the button, open the modal 
        btn2.onclick = function () {
            modal2.style.display = "none";
        }

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
            modal2.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal || event.target == modal2) {
                modal.style.display = "none";
                modal2.style.display = "none";
            }
        }
    </script>

    <script>


</script>

    <script>
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    <script>
        $(function () {
            $("#tabs2").tabs();
        });
    </script>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <span class="clear"></span>
</asp:Content>

