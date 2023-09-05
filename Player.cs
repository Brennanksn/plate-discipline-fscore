using CsvHelper.Configuration.Attributes;

namespace PlateDiscipline;

public class Player
{
    /* READ IN FROM CSV */
    [Name("last_name")]
    public string? LastName { get; set; }

    [Name("first_name")]
    public string? FirstName { get; set; }

    public string Name => FirstName + " " + LastName;


    [Name("woba")]
    public double woba { get; set; }

    /* For confusion matrix */
    [Name("out_zone_swing")]
    public int oSwing { get; set; }
    [Name("out_zone")]
    public int OutZonePitches { get; set; }
    [Name("in_zone_swing")]
    public int zSwing { get; set;}
    [Name("in_zone")]
    public int InZonePitches { get; set;}

    public int zTake => InZonePitches - zSwing;
    public int oTake => OutZonePitches - oSwing;

    /* Metrics and Name*/
    public double SwingPrecision => (double)zSwing / (zSwing + oSwing);
    public double SwingRecall => (double)zSwing / InZonePitches;

    public double TakePrecision => (double) oTake / (zTake + oTake);
    public double TakeRecall => (double) oTake / OutZonePitches;
}