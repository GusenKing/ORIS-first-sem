using System.Text;
using Plane.Attribuets;
using MyOrm;

namespace Plane;

[Controller("Table")]
public class TableController
{
    [Get("ShowTable")]
    public string ShowTable()
    {
        var connectionString =
            "Server=tcp:192.168.1.136,1433;Database=Planes;User=EminComputer;Password=1234;TrustServerCertificate=true";
        // var connectionString =
        //     "Server=localhost;Database=Planes;Trusted_Connection=True;TrustServerCertificate=true";
        
        var orm = 
            new Orm(connectionString);
        var tableContent = orm.Select(new PlaneInfo())
            .Result;
        
        var sb = new StringBuilder();
        sb.Append("<head>\n<meta charset=\"UTF-8\">\n</head>");
        sb.Append("<table>\n<tbody>");
        foreach (var rowElement in tableContent)
        {
            sb.Append($"<tr>\n<td>{rowElement.Id}</td>");
            sb.Append($"<td>{rowElement.PlaneName}</td>");
            sb.Append($"<td>{rowElement.IsOperating}</td>");
            sb.Append($"<td>{rowElement.LastFlightDate}</td>");
            sb.Append($"<td>{rowElement.LastVisitedAeroport}</td>");
            sb.Append($"<td>{rowElement.SeatsAmount}</td>\n</tr>");
        }

        sb.Append("</tbody>\n</table>");
        return sb.ToString();
    }
}