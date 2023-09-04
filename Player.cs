using CsvHelper.Configuration.Attributes;

namespace PlateDiscipline;

public class Player
{
    /* READ IN FROM CSV */
    [Name("last_name")]
    string LastName { get; set; }

    [Name("first_name")]
    string FirstName { get; set; }

    [Name("player_id")] // Unused
    int Id { get; set; }

    [Name("year")] // Unused
    int Year { get; set; }

    [Name("xwoba")]
    double xwoba { get; set; }
    [Name("out_zone_swing")]
    int OutZoneSwings { get; set; }
    [Name("out_zone")]
    int OutZonePitches { get; set; }
    [Name("in_zone_swing")]
    int InZoneSwings { get; set;}
    [Name("in_zone")]
    int InZonePitches { get; set;}

    double Precision => (double)InZoneSwings / InZonePitches;

    double Recall => (double)InZoneSwings / (InZoneSwings + OutZoneSwings);
}