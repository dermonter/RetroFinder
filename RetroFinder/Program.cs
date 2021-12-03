using System;

namespace RetroFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var prot_domain = ".\\Data\\protein_domains.fa";
            var test_seq = ".\\Data\\test_sequence.fa";
            var rf = new RetroFinder()
            {
                DatabasesFilename = prot_domain
            };
            rf.Analyze(test_seq);
        }
    }
}
