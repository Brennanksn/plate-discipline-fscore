using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.ML.Data;
namespace PlateDiscipline
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Player> players = new List<Player>();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                PrepareHeaderForMatch = args => args.Header.ToLower().Trim(),
            };
            using (var reader = new StreamReader("./stats.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<PlayerMap>();
                var records = csv.GetRecords<Player>();
                players = records.ToList();
            }
            Scorer unbiased = new Scorer(1.0);
            // write the swing report
            unbiased.WriteFscoreReport("UnbiasedReport", players, true);
            // write the take report
            unbiased.WriteFscoreReport("UnbiasedReport", players, false);
        }
    }

    class Scorer
    {
        public Scorer(double beta)
        {
            this.beta = beta;
        }

        /* Greater than one -> favor recall
        *  Less than one -> favor precision */
        public double beta { get; private set; }

        private double _precisionCoeff => Math.Pow(this.beta, 2);
        
        private double _numeratorCoeff => 1 + _precisionCoeff;

        public double SwingFscore(Player player)
        {
            double numerator = _numeratorCoeff * player.SwingPrecision * player.SwingRecall;
            double denominator = player.SwingRecall + (_precisionCoeff * player.SwingPrecision);
            return numerator / denominator;
        }
        public double TakeFscore(Player player)
        {
            double numerator = _numeratorCoeff * player.TakePrecision * player.TakeRecall;
            double denominator = player.TakeRecall + (_precisionCoeff * player.TakePrecision);
            return numerator / denominator;
        }
        public void WriteFscoreReport(string outputFileName, IEnumerable<Player> players, bool forSwing)
        {
            var r = new Random();
            var diffFileName = $"{(forSwing ? "SwingScore": "TakeScore")} - {this.beta:N3}-" + outputFileName + $"{-r.NextInt64(2000)}.csv";
            using(StreamWriter writer = new StreamWriter(diffFileName))
            {
                if(forSwing)
                writer.WriteLine("name,swing precision,swing recall,swing fscore,woba");
                else
                writer.WriteLine("name,take precision,take recall,take fscore,woba");
                foreach(var p in players)
                {
                    if(forSwing)
                    writer.WriteLine($"{p.Name},{p.SwingPrecision},{p.SwingRecall},{SwingFscore(p)},{p.woba}");
                    else
                    writer.WriteLine($"{p.Name},{p.TakePrecision},{p.TakeRecall},{TakeFscore(p)},{p.woba}");
                }
                writer.WriteLine("");
            }
        }
    }
}