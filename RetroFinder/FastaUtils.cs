using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RetroFinder
{
    public class FastaUtils
    {
        public static bool Validate(string path)
        {
            if (!File.Exists(path)) return false;

            using var file = new StreamReader(path);
            string line;
            uint counter;
            var id_match = new Regex(@"^>.+$");
            var sequence_match = new Regex(@"^[ACGTN]+$");
            var ids = new HashSet<string>();
            for (counter = 0; (line = file.ReadLine()) != null; ++counter)
            {
                if (counter % 2 == 0)
                {
                    if (!id_match.IsMatch(line)) return false;
                    if (ids.Contains(line)) return false;
                    ids.Add(line);
                }
                else
                {
                    if (!sequence_match.IsMatch(line)) return false;
                }
            }

            return counter > 1 && counter % 2 == 0;
        }

        public static IEnumerable<FastaSequence> Parse(string path)
        {
            using var file = new StreamReader(path);
            var result = new List<FastaSequence>();
            string id;
            while ((id = file.ReadLine()) != null)
            {
                var sequence = file.ReadLine();
                result.Add(new FastaSequence(id.Substring(1), sequence));
            }

            return result;
        }
    }
}
