<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Management" %>
<%@ Import Namespace="App_Start" %>
<%@ Import Namespace="Microsoft.AspNet.FriendlyUrls" %>
<%@ Import Namespace="System.Web.Optimization" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        System.Timers.Timer myTimer = new System.Timers.Timer();
        // Set the Interval to 5 seconds (5000 milliseconds).
        myTimer.Interval = 500000;
        myTimer.AutoReset = true;
        myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
        myTimer.Enabled = true;
        
        RouteConfig.RegisterRoutes(System.Web.Routing.RouteTable.Routes);

        Application["OnlineVisitors"] = 75;
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
    public void myTimer_Elapsed(object source, System.Timers.ElapsedEventArgs e)
    {
        // call mail sending code. name of email sender clsScheduleMail
        //clsScheduleMail objScheduleMail = new clsScheduleMail();
        //objScheduleMail.SendScheduleMail();
    }
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        var ex = Server.GetLastError();
        var httpException = ex as HttpException ?? ex.InnerException as HttpException;
        if(httpException == null) return;

        if(httpException.WebEventCode == WebEventCodes.RuntimeErrorPostTooLarge)
        {
            //handle the error
            Response.Write("Too big a file, dude!"); //for example
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Application.Lock();
        Application["OnlineVisitors"] = (int)Application["OnlineVisitors"] + 1;
        Application.UnLock();
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

        Application.Lock();
        Application["OnlineVisitors"] = (int)Application["OnlineVisitors"] - 1;
        Application.UnLock();

    }
    void Application_BeginRequest(object sender, EventArgs e)
    {
        // Get the current path
        string CurrentURL_Path = Request.Path.ToLower();
        if (CurrentURL_Path.EndsWith(".aspx"))
        {
            HttpContext MyContext = HttpContext.Current;
        }
        
        
        
    }

    void Application_PreRequestHandlerExecute(object sender, EventArgs e)
    {
        if (HttpContext.Current.Request.Url.OriginalString == "")
        {
            //Response.Redirect("Default.aspx");
        }
        if (System.Web.HttpContext.Current.Session != null)
        {
            string str = "";
            if (Session["randomStr"] != null)
            {
                str = Session["randomStr"].ToString();
                // Code that runs when a new session is started
                Session["xCaptchaS"] = str;
            }
        }        
    }
</script>
