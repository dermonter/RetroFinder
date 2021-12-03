using RetroFinder.Output;

namespace RetroFinder
{
    public class RetroFinder
    {
        public string DatabasesFilename { get; set; }

        public void Analyze(string path)
        {
            System.Console.WriteLine("Enter database file location (empty for default)");
            string temp = System.Console.ReadLine();
            if (temp.Length != 0)
            {
                DatabasesFilename = temp;
            }

            System.Console.WriteLine("Generate json? (y/n) (default: y)");
            string json = System.Console.ReadLine();
            System.Console.WriteLine("Generate xml? (y/n) (default: y)");
            string xml = System.Console.ReadLine();

            if (!FastaUtils.Validate(path)) return;
            var sequences = FastaUtils.Parse(path);

            foreach (var sequence in sequences)
            {
                var sa = new SequenceAnalysis()
                {
                    DatabasesFilename = DatabasesFilename,
                    Sequence = sequence
                };
                sa.Analyze();

                if (json == "y" || json.Length == 0)
                {
                    var js = new JsonSerializer();
                    js.SerializeAnalysisResult(sa);
                }
                if (xml == "y" || xml.Length == 0)
                {
                    var xmls = new XMLSerializer();
                    xmls.SerializeAnalysisResult(sa);
                }
            }
        }
    }
}
