using System.Net;
using System.Reflection;
using MyHTTPServer.Contollers;

namespace MyHTTPServer.Handlers;

public class ControllerHandler : Handler
{
    private static bool Check(HttpListenerContext context)
    {
        string[] strParams = context.Request.Url!
            .Segments
            .Skip(1)
            .Select(s => s.Replace("/", ""))
            .ToArray();

        foreach (var e in strParams)
        {
            Console.WriteLine(e);
        }
        
        var assembly = Assembly.GetExecutingAssembly();

        var controller = assembly.GetTypes()
            .Where(t => Attribute.IsDefined(t, typeof(HttpControllerAttribute)))
            .FirstOrDefault(c => ((HttpControllerAttribute)Attribute.GetCustomAttribute(c, typeof(HttpControllerAttribute))!)
                .controllerName.Equals("controllerName", StringComparison.OrdinalIgnoreCase));

        if (controller == null) return false;

        var test = typeof(HttpControllerAttribute).Name;
        var method = controller.GetMethods()
            .FirstOrDefault(t => t.Name.Equals("methodName", StringComparison.OrdinalIgnoreCase));

        if (method == null) return false;

        object[] queryParams = method.GetParameters()
            .Select((p, i) => Convert.ChangeType(strParams[i], p.ParameterType))
            .ToArray();

        var ret = method.Invoke(Activator.CreateInstance(controller), queryParams);
        return true;
    }
    public override void HandleRequest(HttpListenerContext context)
    {
        Console.WriteLine("in controller handler");
        if (Check(context))
        {
            // завершение выполнения запроса;
        }
        else if (Successor != null)
        {
            Successor.HandleRequest(context);
        }
    }
}