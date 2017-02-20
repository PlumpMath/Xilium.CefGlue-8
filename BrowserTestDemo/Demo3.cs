using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xilium.CefGlue.Demo;

namespace BrowserTestDemo
{
    public class Demo3
    {
       
        public static void A()
        {
            MessageBox.Show(Assembly.GetAssembly(typeof(Demo3)).FullName);
        }
    }
}
