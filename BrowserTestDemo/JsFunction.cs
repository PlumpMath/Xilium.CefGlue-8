using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue.Demo;

namespace BrowserTestDemo
{
    public class JSFunction: JSFunctionBase
    {
        public static void HelloWorld(string Message)
        {
            MessageBox.Show(Message);
        }
    }
}
