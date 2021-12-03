using RetroFinder.Domains;
using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder
{
    public class SequenceAnalysis
    {
        public FastaSequence Sequence { get; set; }

        public IEnumerable<Transposon> Transposons { get; set; }
        public string DatabasesFilename { get; set; }

        public void Analyze()
        {
            var ltrFinder = new LTRFinder()
            {
                Sequence = Sequence
            };
            Transposons = ltrFinder.IdentifyElements();
            foreach (var trans in Transposons)
            {
                var df = new DomainFinder()
                {
                    DatabasesFilename = DatabasesFilename,
                    Sequence = Sequence.Sequence[trans.Location.start..trans.Location.end],
                    Offset = trans.Location.start
                };

                var domains = df.IdentifyDomains();

                var dp = new DomainPicker()
                {
                    Domains = domains,
                    Sequence = Sequence.Sequence
                };

                var pickedDomains = dp.PickDomains();
                trans.Features = trans.Features.Concat(pickedDomains);
            }
        }
    }
}
