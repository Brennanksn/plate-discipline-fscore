using CsvHelper.Configuration;

namespace PlateDiscipline;
public class PlayerMap : ClassMap<Player>
{
    public PlayerMap()
    {
        Map(m => m.LastName).Name("last_name");
        Map(m => m.FirstName).Name(" first_name");
        Map(m => m.woba).Name("woba");
        Map(m => m.oSwing).Name("out_zone_swing");
        Map(m => m.zSwing).Name("in_zone_swing");
        Map(m => m.InZonePitches).Name("in_zone");
        Map(m => m.OutZonePitches).Name("out_zone");
    }
}