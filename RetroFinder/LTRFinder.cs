using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RetroFinder
{
    public class LTRFinder
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> IdentifyElements()
        {
            var result = new List<Transposon>();
            var ltrs = new Regex(@"(?<leftLTR>[ACGT]{100,300})(?<elements>[ACGTN]{1000,3500})(?<rightLTR>\k<leftLTR>)");

            foreach (Match match in ltrs.Matches(Sequence.Sequence))
            {
                var leftLTR = match.Groups["leftLTR"];
                var rightLTR = match.Groups["rightLTR"];
                var elements = match.Groups["elements"];

                result.Add(new Transposon()
                {
                    Location = (elements.Index, elements.Index + elements.Value.Length),
                    Features = new List<Feature>()
                    {
                        new Feature()
                        {
                            Type = FeatureType.LTRLeft,
                            Location = (leftLTR.Index, leftLTR.Index + leftLTR.Value.Length)
                        },
                        new Feature()
                        {
                            Type = FeatureType.LTRRight,
                            Location = (rightLTR.Index, rightLTR.Index + rightLTR.Length)
                        }

                    }
                });
            }

            return result;
        }
    }
}
