using System.Net;
using System.Net.Mime;
using HtmlAgilityPack;
using HttpServer.Attribuets;
using MimeKit;

namespace HttpServer.Controllers;

[Controller("Orders")]
public class OrdersController
{
    [Get("List")]
    public string List()
    {
        var wClient = new WebClient();
        HtmlDocument html = new HtmlDocument();
        html.LoadHtml(wClient.DownloadString("https://steamcommunity.com/market/search?q=#p1_popular_desc"));
        SteamMarketParser.GetCaseLinks(html);
        html.LoadHtml(wClient.DownloadString("https://steamcommunity.com/market/search?q=#p2_popular_desc"));
        SteamMarketParser.GetCaseLinks(html);

        string page;
        using (var sr = new StreamReader("newHtml.html"))
        {
            var page = sr.ReadToEndAsync();
        }

        return page;
    }
    
}