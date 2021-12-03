using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder.Domains
{
    public class DomainFinder
    {
        public string DatabasesFilename { get; set; }
        public string Sequence { get; set; }
        public int Offset { get; set; }

        public IEnumerable<Feature> IdentifyDomains()
        {
            var result = new List<Feature>();
            var databases = FastaUtils.Parse(DatabasesFilename);

            foreach (var seq in databases)
            {
                var feat = SmithWaterman(Sequence, seq);
                if (feat != null)
                {
                    result.Add(feat);
                }
            }

            return result;
        }

        Feature SmithWaterman(string seq1, FastaSequence seq2)
        {
            FeatureType type;
            if (seq2.Id.Contains("GAG"))
            {
                type = FeatureType.GAG;
            }
            else if (seq2.Id.Contains("PROT"))
            {
                type = FeatureType.PROT;
            }
            else if (seq2.Id.Contains("INT"))
            {
                type = FeatureType.INT;
            }
            else if (seq2.Id.Contains("RT"))
            {
                type = FeatureType.RT;
            }
            else
            {
                type = FeatureType.RH;
            }

            (int val, int traceback)[,] h = new (int, int)[seq1.Length + 1, seq2.Sequence.Length + 1];
            (int i, int j) max = (0, 0);
            for (int i = 1; i < seq1.Length + 1; ++i)
            {
                for (int j = 1; j < seq2.Sequence.Length + 1; ++j)
                {
                    var values = new int[]
                    {
                        h[i - 1, j - 1].val + (seq1[i - 1] == seq2.Sequence[j - 1] ? 3 : -3),
                        h[i, j - 1].val - 2,
                        h[i - 1, j].val - 2,
                        0
                    };
                    var valuesM = values.Max();
                    var valuesMI = Array.IndexOf(values, valuesM);
                    h[i, j] = (valuesM, valuesMI);
                    if (valuesM > h[max.i, max.j].val) max = (i, j);
                }
            }

            (int i, int j) curr = max;
            int score = 0;
            while (h[curr.i, curr.j].val != 0)
            {
                score += h[curr.i, curr.j].val;
                switch (h[curr.i, curr.j].traceback)
                {
                    case 0:
                        curr = (curr.i - 1, curr.j - 1);
                        break;
                    case 1:
                        curr = (curr.i, curr.j - 1);
                        break;
                    case 2:
                        curr = (curr.i - 1, curr.j);
                        break;
                    default:
                        break;
                }
            }

            if (max.i - curr.i < seq2.Sequence.Length) return null;

            return new Feature()
            {
                Type = type,
                Location = (Offset + curr.i, Offset + max.i),
                Priority = score
            };
        }
    }
}
