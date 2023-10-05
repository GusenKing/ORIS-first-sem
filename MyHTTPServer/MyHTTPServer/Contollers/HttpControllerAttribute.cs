namespace MyHTTPServer.Contollers;

public class HttpControllerAttribute : Attribute
{
    public string controllerName;

    public HttpControllerAttribute(string controllerName)
    {
        this.controllerName = controllerName;
    }
}