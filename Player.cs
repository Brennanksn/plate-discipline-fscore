using CsvHelper.Configuration.Attributes;

namespace PlateDiscipline;

public class Player
{
    /* READ IN FROM CSV */
    [Name("last_name")]
    public string? LastName { get; set; }

    [Name("first_name")]
    public string? FirstName { get; set; }

    [Name("woba")] // Unused for now
    public double woba { get; set; }

    /* For confusion matrix */
    [Name("out_zone_swing")]
    public int OutZoneSwings { get; set; }
    [Name("out_zone")]
    public int OutZonePitches { get; set; }
    [Name("in_zone_swing")]
    public int InZoneSwings { get; set;}
    [Name("in_zone")]
    public int InZonePitches { get; set;}

    /* Metrics and Name*/
    public double Precision => (double)InZoneSwings / InZonePitches;
    public string Name => FirstName + " " + LastName;
    public double Recall => (double)InZoneSwings / (InZoneSwings + OutZoneSwings);
}