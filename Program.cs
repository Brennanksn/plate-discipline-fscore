using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

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
            unbiased.WriteFscoreReport("UnbiasedReport", players);
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

        public double Fscore(Player player)
        {
            double numerator = _numeratorCoeff * player.Precision * player.Recall;
            double denominator = player.Recall + (_precisionCoeff * player.Precision);
            return numerator / denominator;
        }

        public void WriteFscoreReport(string outputFileName, IEnumerable<Player> players)
        {
            var r = new Random();
            var diffFileName = $"{this.beta:N3}-" + outputFileName + $"{-r.NextInt64(2000)}.csv";
            using(StreamWriter writer = new StreamWriter(diffFileName))
            {
                writer.WriteLine("name,precision,recall,fscore,woba");
                foreach(var p in players)
                {
                    writer.WriteLine($"{p.Name},{p.Precision},{p.Recall},{Fscore(p)},{p.woba}");
                }
                writer.WriteLine("");
            }
        }
    }
}