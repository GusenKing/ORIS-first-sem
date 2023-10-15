using System.Net;
using HtmlAgilityPack;

namespace HttpServer;

public static class SteamMarketParser
{
    private static List<HtmlNode> _caseListingBlocks = new List<HtmlNode>();
    
    public static void GetCaseList(HtmlDocument html)
    {

        // var temp = html.GetElementbyId("searchResultsRows").ChildNodes;
        // caseListingBlocks = temp.Where(x => x.Id.Split("_")[0] == "resultlink").ToList();
        for (int i = 0; i < 10; i++)
        {
            _caseListingBlocks.Add(html.GetElementbyId($"result_{i}"));
        }
    }

    public static string GetPage()
    {
        var newHtmlDoc = new HtmlDocument();

        for (int i = 0; i < 15; i++)
        {
                newHtmlDoc.DocumentNode.AppendChild(_caseListingBlocks[i]);
        }

        var res = newHtmlDoc.DocumentNode.WriteTo();
        return res;
    }
}