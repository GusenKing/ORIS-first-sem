﻿namespace Plane.Attribuets;

public class GetAttribute : Attribute, IHttpMethodAttribute
{
    public GetAttribute(string actionName)
    {
        ActionName = actionName;
    }

    public string ActionName { get; }
}