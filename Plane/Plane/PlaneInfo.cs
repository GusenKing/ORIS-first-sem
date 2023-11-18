namespace Plane;

public class PlaneInfo
{
    public int Planeid { get; set; }
    
    public string PlaneName { get; set; }
    
    public bool IsOperating { get; set; }
    
    public DateTime LastFlightDate { get; set; }
    
    public string LastVisitedAeroport { get; set; }
    
    public int SeatsAmount { get; set; }
}