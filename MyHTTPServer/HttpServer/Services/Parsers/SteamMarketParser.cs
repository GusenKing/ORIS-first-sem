using System.Net;
using HtmlAgilityPack;

namespace HttpServer;

public static class SteamMarketParser
{
    private static List<HtmlNode> _caseListingBlocks = new List<HtmlNode>();
    
    public static void GetCaseList(HtmlDocument html)
    {
        for (int i = 0; i < 10; i++)
        {
            var buttonNode = HtmlNode.CreateNode("<button>Купить</button>");
            var parsedNode = html.GetElementbyId($"result_{i}");
            parsedNode.AppendChild(buttonNode);
            _caseListingBlocks.Add(parsedNode);
        }
    }

    public static string GetPage()
    {
        var newHtmlDoc = new HtmlDocument();
        var headHtml = @"<head>
    <meta charset=""UTF-8"">
    <style>
        .market_listing_row{
            border: black solid 1px;
            padding-bottom: 15%;
            margin-right: 20%;
            margin-bottom: 20%;
        }
    </style>
</head>";
        var headNode = HtmlNode.CreateNode(headHtml);
        newHtmlDoc.DocumentNode.AppendChild(headNode);
        var wrapperHtml = @"<div class=""wrapper"" style=""display: grid; grid-template-columns: 1fr 1fr 1fr""></div>";
        var wrapperNode = HtmlNode.CreateNode(wrapperHtml);
        newHtmlDoc.DocumentNode.AppendChild(wrapperNode);
        
        for (int i = 0; i < 15; i++)
        {
                wrapperNode.AppendChild(_caseListingBlocks[i]);
        }

        var res = newHtmlDoc.DocumentNode.WriteTo();
        return res;
    }
}