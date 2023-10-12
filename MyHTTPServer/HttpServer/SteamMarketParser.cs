using System.Net;
using HtmlAgilityPack;

namespace HttpServer;

public class SteamMarketParser
{
    private static List<HtmlAttribute> caseLinks = new List<HtmlAttribute>();
    
    public static void GetCaseLinks(HtmlDocument html)
    {
        var aLinks = html.GetElementbyId("searchResultsRows").ChildNodes.Where(x => x.Name == "a");

        foreach (var link in aLinks)
        {
            var t = link.Attributes.Where(x => x.Name == "href").ToArray()[0];
            caseLinks.Add(t);
        }
    }

    public static void GetPage()
    {
        var wClient = new WebClient();
        var newHtmlDoc = new HtmlDocument();
        var parsDoc = new HtmlDocument();

        for (int i = 0; i < 15; i++)
        {
            parsDoc.LoadHtml(wClient.DownloadString(caseLinks[0].Value));
            
            var htmlBody = newHtmlDoc.DocumentNode.SelectSingleNode("//body");
		
            HtmlNode newPara = HtmlNode.CreateNode("<div></div>");
            newPara.ChildNodes.Add(HtmlNode.CreateNode($"<img src={parsDoc.GetElementbyId("mainContents").ChildNodes.Where(x => x.GetClasses().Contains("market_listing_largeimage")).First().ChildNodes.First().GetAttributeValue("src", "0")}>"));
            
            htmlBody.ChildNodes.Add(newPara);
        }

        newHtmlDoc.Save("newHtml.html");
    }
}