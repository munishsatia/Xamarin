using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.OS;
using HybridApp.Views;
using HybridApp.Models;
using HybridApp.Core;
using System.Collections.Generic;
using System.Collections;

namespace HybridApp
{
    [Activity(Label = "HybridApp", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;

            // Use subclassed WebViewClient to intercept hybrid native calls
            webView.SetWebViewClient(new HybridWebViewClient());

            // Render the view from the type generated from RazorView.cshtml
            var model = new Model1() { Text = "Text goes here" };
            var template = new RazorView() { Model = model };
            var page = template.GenerateString();

            // Load the rendered HTML into the view with a base URL 
            // that points to the root of the bundled Assets folder
            webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);

        }

        private class HybridWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView webView, string url)
            {
                
                var scheme = "hybrid:";
                // This handler will treat everything between the protocol and "?"
                // as the method name.  The querystring has all of the parameters.
                var resources = url.Substring(scheme.Length).Split('?');
                var method = resources[0];
                var name = resources[1].Split('&')[0];
                resources = resources[1].Split('&');

                Dictionary<string, string> qansList = new Dictionary<string, string>();
                
                foreach (string res in resources)
                {
                    var key = res.Split('=')[0];
                    var value = res.Split('=')[1];
                    qansList.Add(key, value);                    
                }
                //var parameters = System.Web.HttpUtility.ParseQueryString(resources[1]);

                if (method == "Save")
                {
                    
                    //var textbox = parameters["textbox"];

                    //// Add some text to our string here so that we know something
                    //// happened on the native part of the round trip.
                    //var prepended = string.Format("C# says \"{0}\"", textbox);

                    //// Build some javascript using the C#-modified result
                    //var js = string.Format("SetLabelText('{0}');", prepended);

                    //webView.L int i = 1;
                    List<QA> qalist = new List<QA>();
                    
                    var username = qansList["fname"];
                    for (var i=1;i<5;i++)
                    {
                        string q1 = "q"+i+"h";
                        string q1a = "q"+i;
                        var question = qansList[q1];
                        var ans = qansList[q1a];
                        qalist.Add(new QA() { Question = question, Answer = ans,UserName= username });
                    }


                    
                    QAService serv = new QAService();
                    serv.SaveForm(qalist);

                    var prepended = "Saved Successfully";

                    var js = string.Format("SetLabelText('{0}');", prepended);

                    webView.LoadUrl("javascript:" + js);

                }

                return true;
            }

            public override void OnPageFinished(WebView view, string url)
            {
                base.OnPageFinished(view, url);

                // If the URL is not our own custom scheme, just let the webView load the URL as usual
                var scheme = "hybrid:";

                if (!url.StartsWith(scheme))
                    return ;

                //var username = "supersecretusername";
                //var password = "supersecretpassword";

                //// to ensure Log_in click isn't called before username & password are set lets
                //// create the script as single item instead of calling them separately
                //var script = $"document.getElementById(\"username\").value = \"{username}\";" +
                //             $"document.getElementById(\"password\").value = \"{password}\";" +
                //             "document.getElementById(\"Log_in\").click()";

                //view.LoadUrl($"javascript: {script}");

                //// separate calls
                ////view.LoadUrl("javascript: document.getElementById(\"username\").value = \"sami\";document.getElementById(\"password\").value = \"password\";");
                ////view.LoadUrl("javascript: document.getElementById(\"password\").value = \"password\"");
                ////view.LoadUrl("javascript: document.getElementById(\"Log_in\").click()");
            }
        }
    }
}

