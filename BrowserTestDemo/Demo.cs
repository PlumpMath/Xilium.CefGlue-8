using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue;
using Xilium.CefGlue.Demo;
using Xilium.CefGule.GlobalConfig;
using MenuItem = Xilium.CefGlue.Demo.MenuItem;
using MenuItemImpl = System.Windows.Forms.MenuItem;

namespace BrowserTestDemo
{
    public partial class Demo : Form
    {
        private const string DumpRequestDomain = "dump-request.demoapp.cefglue.xilium.local";
        public static string HomeUrl = "www.ly.com";
        CefWebBrowser browserCtl = null;
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        public Demo()
        {
            //窗体自带初始化程序
            InitializeComponent();
            //AllocConsole();
            InitializeBrowser();
        }
        public bool InitializeBrowser()
        {
            AllocConsole();
            CefRuntime.Load();

            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = CefRuntime.Platform == CefRuntimePlatform.Windows;
            settings.ReleaseDCheckEnabled = true;
            settings.LogSeverity = CefLogSeverity.Info;
            settings.LogFile = "cef.log";
            //settings.ResourcesDirPath = System.IO.Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetEntryAssembly().CodeBase).LocalPath);
            settings.RemoteDebuggingPort = 20480;
            settings.Locale = "zh-CN";
            var a = Config.JsFunctionAssembly;


            var mainArgs = new CefMainArgs(new string[0]);
            var app = new DemoCefApp();

            var exitCode = CefRuntime.ExecuteProcess(mainArgs, app);
            Console.WriteLine("CefRuntime.ExecuteProcess() returns {0}", exitCode);
            //返回值-1为成功
            if (exitCode != -1)
            {
                return false;
            }

            //初始化
            CefRuntime.Initialize(mainArgs, settings, app);
            CefRuntime.RegisterSchemeHandlerFactory("http", DumpRequestDomain, new DemoAppSchemeHandlerFactory());
            browserCtl = new CefWebBrowser();
            browserCtl.Parent = this.panel1;
            browserCtl.Dock = DockStyle.Fill;
            browserCtl.BringToFront();
            var browser = browserCtl.WebBrowser;
            browser.StartUrl = HomeUrl;
            
            //navBox.Attach(browser);

            return true;
        }

        public void ShutdownBrowser()
        {
            CefRuntime.Shutdown();
        }
        private void Demo_FormClosed(object sender, FormClosedEventArgs e)
        {
            FreeConsole();
            Application.Exit();
            ShutdownBrowser();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //browserCtl.WebBrowser.CefBrowser.GetFocusedFrame().V8Context.GetGlobal().SetValue("CfxHelloWorld", CefV8Value.CreateFunction("CfxHelloWorld", handel), CefV8PropertyAttribute.DontDelete | CefV8PropertyAttribute.ReadOnly);
            //browserCtl.WebBrowser.CefBrowser.GetFocusedFrame().V8Context.GetGlobal().SetValue("");
            browserCtl.WebBrowser.CefBrowser.GetFocusedFrame().ExecuteJavaScript("HelloWorld('HelloWorld');", "inner",0);
            //var funName = "HelloWorld!";
            //var message = CefProcessMessage.Create("CreateJSFunction");
            //var arg = message.Arguments;
            //arg.SetSize(1);
            //arg.SetString(0, "funName");
            //browserCtl.WebBrowser.CefBrowser.SendProcessMessage(CefProcessId.Renderer, message);
            //Demo3.A();

        }
    }
}
