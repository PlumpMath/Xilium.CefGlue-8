using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Xilium.CefGlue.Demo
{
    public delegate bool Execute_(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception);

    public class JSFunctionV8Handler : CefV8Handler
    {
        public Execute_ JSFunction;
        protected override bool Execute(string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception)
        {
            return JSFunction(name,obj, arguments,out returnValue,out exception);
        }
    }


}
