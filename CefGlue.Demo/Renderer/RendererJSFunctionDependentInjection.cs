using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xilium.CefGule.GlobalConfig;

namespace Xilium.CefGlue.Demo.Renderer
{
    public class RendererJSFunctionDependentInjection
    {
        public static void Process(CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            var ass = Config.JsFunctionAssembly;
            var assembly = Assembly.Load(ass);
            var allType = assembly.GetTypes();
            foreach (var t in allType)
            {
                if (!(t.BaseType == typeof(JSFunctionBase)))
                {
                    continue;
                }
                RegisteredType(t, browser, frame, context);

            }
        }


        public static void RegisteredType(Type type, CefBrowser browser, CefFrame frame, CefV8Context context)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                var parames = method.GetParameters();

                var function = new JSFunctionV8Handler();
                function.JSFunction = (string name, CefV8Value obj, CefV8Value[] arguments, out CefV8Value returnValue, out string exception) =>
                {
                    //有参数
                    if (parames != null && parames.Length > 0)
                    {
                        var agrs = new object[parames.Length];
                        for (int i = 0; i < parames.Length; i++)
                        {
                            object arg = null;
                            if (i < arguments.Length)
                            {
                                arg = CefV8ValueConvert(arguments[i]);
                            }

                            var param = parames[i];

                            //获取value的值
                            if (arg == null)
                            {
                                continue;
                            }
                            //如果参数类型不对
                            if (arg.GetType() != param.ParameterType)
                            {
                                continue;
                            }
                            agrs[i] = arg;
                        }
                        method.Invoke(null, agrs);
                    }
                    else
                    {
                        //无参数
                        method.Invoke(null, null);
                    }
                    exception = null;
                    returnValue = CefV8Value.CreateNull();
                    return true;
                };

                //绑定到context中去
                context.GetGlobal().SetValue(method.Name, CefV8Value.CreateFunction(method.Name, function), CefV8PropertyAttribute.DontDelete | CefV8PropertyAttribute.ReadOnly);

            }
        }

        public static object CefV8ValueConvert(CefV8Value value)
        {
            if (value == null)
            {
                return null;
            }
            else if (value.IsBool)
            {
                return value.GetBoolValue();
            }
            else if (value.IsDate)
            {
                return value.GetDateValue();
            }
            else if (value.IsDouble)
            {
                return value.GetDoubleValue();
            }
            else if (value.IsInt)
            {
                return value.GetIntValue();
            }
            else if (value.IsString)
            {
                return value.GetStringValue();
            }
            return null;


        }
    }
}
