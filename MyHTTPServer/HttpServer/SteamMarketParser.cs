using System.Net;
using HtmlAgilityPack;

namespace HttpServer;

public class SteamMarketParser
{
    private static List<HtmlNode> caseListingBlocks;
    
    public static void GetCaseList(HtmlDocument html)
    {
        caseListingBlocks = html.GetElementbyId("searchResultsRows").ChildNodes.Where(x => x.Id.Split("_")[0] == "result").ToList();
    }

    public static string GetPage()
    {
        var newHtmlDoc = new HtmlDocument();

        for (int i = 0; i < 15; i++)
        {
            var htmlBody = newHtmlDoc.DocumentNode.SelectSingleNode("//body");
            foreach (var node in caseListingBlocks)
            {
                htmlBody.ChildNodes.Add(node);
            }
        }

        return newHtmlDoc.Text;
    }
}