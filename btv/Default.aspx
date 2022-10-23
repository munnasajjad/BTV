<%@ Page Title="" Language="C#"   AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Book Your Room Today!</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <script type="application/x-javascript"> addEventListener("load", function() { setTimeout(hideURLbar, 0); }, false); function hideURLbar(){ window.scrollTo(0,1); } </script>
    <!-- font files -->
    <link href='//fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900' rel='stylesheet' type='text/css' />
    <link href='//fonts.googleapis.com/css?family=Poiret+One' rel='stylesheet' type='text/css' />
    <link href="https://fonts.googleapis.com/css?family=Hind+Siliguri&display=swap" rel="stylesheet" />
    <!-- //font files -->
    <!-- css files -->

    <link href="css/bootstrap.css" rel='stylesheet' type='text/css' media="all" />
    <link href="css/wickedpicker.css" rel="stylesheet" type="text/css" media="all" />
    <link href="css/style.css" rel="stylesheet" type="text/css" media="all" />
    <!-- /css files -->
    <!-- js files -->
    <link href="js/DateRangePicker/daterangepicker.min.css" rel="stylesheet" />
    <script src="js/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.24.0/moment.min.js"></script>

    <script src="js/DateRangePicker/jquery.daterangepicker.min.js"></script>
    <script type="text/javascript" src="js/move-top.js"></script>
    <script type="text/javascript" src="js/easing.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function ($) {
            $(".scroll").click(function (event) {
                event.preventDefault();
                $('html,body').animate({ scrollTop: $(this.hash).offset().top }, 1000);
            });
            var today = new Date();
            var start = new Date(today);
            start.setDate(start.getDate() + 7);
            var newdate = new Date(start);
            newdate.setDate(newdate.getDate() + 6);
            $('#txtCIDate').dateRangePicker({
                startDate: start,
                endDate: newdate


            });

        });
    </script>


    <style>
        .banner-top {
            width: 40% !important
        }

        img#myImg {
            float: right;
        }

        h2.inline {
            float: left;
        }

        .inline {
            display: inline-block;
        }

        .body {
            font-family: 'Hind Siliguri', sans-serif;
        }

        .panel {
            min-height: .01%;
            overflow-x: auto;
            border: 5px #24a7f2 solid !important;
            border-radius: 5px;
            -webkit-box-shadow: 5px 5px 5px rgba(0,0,0,.05);
            box-shadow: 5px 5px 5px rgba(0,0,0,.05);
            margin-top: 10px;
        }

        .membersinfo td {
            vertical-align: middle;
            color: #044a86;
        }

        .item1 {
            border: 3px solid #24a7f2;
            text-align: center;
            /*font-size: 18px;*/
            -webkit-box-shadow: 5px 5px 15px 5px #000000;
            box-shadow: -1px 1px 4px 3px #91d2f7;
            border-radius: 6px;
            /* Permalink - use to edit and share this gradient: https://colorzilla.com/gradient-editor/#feffff+0,ddf1f9+35,a0d8ef+100;Blue+3D+%2318 */
            background: #feffff; /* Old browsers */
            background: -moz-linear-gradient(top, #feffff 0%, #ddf1f9 35%, #a0d8ef 100%); /* FF3.6-15 */
            background: -webkit-linear-gradient(top, #feffff 0%,#ddf1f9 35%,#a0d8ef 100%); /* Chrome10-25,Safari5.1-6 */
            background: linear-gradient(to bottom, #feffff 0%,#ddf1f9 35%,#a0d8ef 100%); /* W3C, IE10+, FF16+, Chrome26+, Opera12+, Safari7+ */
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#feffff', endColorstr='#a0d8ef',GradientType=0 ); /* IE6-9 */
        }

        .item2 {
            background: rgba(255,255,255,1);
            background: -moz-linear-gradient(left, rgba(255,255,255,1) 0%, rgba(246,246,246,1) 47%, rgba(237,237,237,1) 100%);
            background: -webkit-gradient(left top, right top, color-stop(0%, rgba(255,255,255,1)), color-stop(47%, rgba(246,246,246,1)), color-stop(100%, rgba(237,237,237,1)));
            background: -webkit-linear-gradient(left, rgba(255,255,255,1) 0%, rgba(246,246,246,1) 47%, rgba(237,237,237,1) 100%);
            background: -o-linear-gradient(left, rgba(255,255,255,1) 0%, rgba(246,246,246,1) 47%, rgba(237,237,237,1) 100%);
            background: -ms-linear-gradient(left, rgba(255,255,255,1) 0%, rgba(246,246,246,1) 47%, rgba(237,237,237,1) 100%);
            background: linear-gradient(to right, rgba(255,255,255,1) 0%, rgba(246,246,246,1) 47%, rgba(237,237,237,1) 100%);
            filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ededed', GradientType=1 );
        }

        .tiles {
            margin-bottom: 30px;
        }

            .tiles .tooltiptext {
                visibility: hidden;
                width: 100%;
                background-color: black;
                color: #fff;
                text-align: center;
                border-radius: 6px;
                padding: 5px 2px;
                font-size: 14px;
                opacity: 0.5;
                transition: opacity .25s ease-in-out;
                -moz-transition: opacity .25s ease-in-out;
                -webkit-transition: opacity .25s ease-in-out;
                /* Position the tooltip */
                position: absolute;
                z-index: 1;
                transition: 0.3s;
                left: -2%;
            }

            .tiles:hover .tooltiptext {
                visibility: visible;
                opacity: 1;
            }

        input[type=checkbox] {
            zoom: 200%;
        }

        .tiles label {
            display: block;
        }

        .rooms {
            margin: 20px 0 50px;
            display: block;
            min-height: 150px;
        }

        .MyCalendar .ajax__calendar_container {
            border: 1px solid #646464;
            background-color: lemonchiffon;
            color: red;
        }

        .tiles.disabled {
            border: 3px solid #b5bcbf;
            text-align: center;
            font-size: 18px;
            color: darkgray;
            border-radius: 6px;
        }

        .org-Name h2 {
            color: #044a86;
            /* font-size: 24px; */
            margin: 16px;
            /* padding-left: 25px; */
            /* position: fixed; */
            text-align: center;
        }

        .header_right {
            /* width: 400px; */
            float: right;
            /* margin-right: 10px; */
            padding: 11px;
            color: wheat;
            border: 1px #3e597f solid;
        }

        #header {
            color: #044a86;
        }

        .header_left {
            width: 65px !important;
        }

        .user_info span {
            display: inline-block !important;
        }

        h3 {
            color: greenyellow;
            letter-spacing: 2px;
            font-size: 19px;
        }

        #Login1 p {
            margin-top: 2%;
        }

        .table {
            color: aliceblue;
            border: 1px solid #eee;
            border-radius: 4px;
        }

        a {
            color: #044a86;
        }

        .form-control {
            background-color: transparent;
            background-image: none;
            border: 1px solid #fff;
            color: #044a86;
        }

            .form-control:focus {
                border: 1px solid #fff;
            }

        option {
            color: #044a86;
        }

        input#Login1_LoginButton {
            float: right;
        }

        .form-control[disabled], .form-control[readonly], fieldset[disabled] .form-control {
            background-color: transparent;
            color: #044a86;
        }

        #cbtna {
            margin-top: 5px;
            zoom: 100%;
        }

        label[for=cbtna] {
            display: inline;
        }

        .terms {
            display: inline-block;
        }
        /* Style the Image Used to Trigger the Modal */
        #myImg {
            border-radius: 5px;
            cursor: pointer;
            transition: 0.3s;
        }

            #myImg:hover {
                opacity: 0.7;
            }

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
            background-color: rgba(0,0,0,0.9); /* Black w/ opacity */
        }

        /* Modal Content (Image) */
        .modal-content {
            margin: auto;
            display: block;
            width: 80%;
            max-width: 700px;
        }

        /* Caption of Modal Image (Image Text) - Same Width as the Image */
        #caption {
            margin: auto;
            display: block;
            width: 80%;
            max-width: 700px;
            text-align: center;
            color: #ccc;
            padding: 10px 0;
            height: 150px;
        }

        /* Add Animation - Zoom in the Modal */
        .modal-content, #caption {
            animation-name: zoom;
            animation-duration: 0.6s;
        }

        @keyframes zoom {
            from {
                transform: scale(0)
            }

            to {
                transform: scale(1)
            }
        }

        /* The Close Button */
        .close {
            position: absolute;
            top: 15px;
            right: 35px;
            color: #f1f1f1;
            font-size: 40px;
            font-weight: bold;
            transition: 0.3s;
        }

        div#pnlEmpty {
            margin-top: 95px;
        }

        .close:hover,
        .close:focus {
            color: #bbb;
            text-decoration: none;
            cursor: pointer;
        }

        /* 100% Image Width on Smaller Screens */
        @media only screen and (max-width: 700px) {
            .modal-content {
                width: 100%;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>


        <%--<script type="text/javascript" language="javascript">
            Sys.Application.add_load(callJquery);
        </script>--%>



        <div class="header">
            <img src="branding/gov.png" class="govLogo" />
            <h1>সড়ক ও জনপথ অধিদপ্তর</h1>
            <h3>Inspection Bungalow Booking System</h3>


            <%--<asp:Label ID="lblId" runat="server" EnableViewState="false"></asp:Label>--%>
        </div>
        </form>
</body>
</html>
