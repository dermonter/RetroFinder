using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroFinder.Domains
{
    public class DomainPicker
    {
        public IEnumerable<Feature> Domains { get; set; }
        public string Sequence { get; set; }

        public IEnumerable<Feature> PickDomains()
        {
            var result = new List<(int score, List<Feature> feats)>();

            var gags = PickBest(FeatureType.GAG);
            var prots = PickBest(FeatureType.PROT);
            var intts = PickBest(FeatureType.INT);
            var rts = PickBest(FeatureType.RT);
            var rhs = PickBest(FeatureType.RH);

            foreach (var gag in gags)
            {
                var temp = new List<Feature>();
                if (gag != null) temp.Add(gag);
                foreach (var prot in prots)
                {
                    if (prot != null) {
                        if (temp.Count() != 0)
                        {
                            if (temp.Last().Location.end <= prot.Location.start)
                            {
                                temp.Add(prot);
                            }
                        }
                        else
                        {
                            temp.Add(prot);
                        }
                    }
                    foreach (var intt in intts)
                    {
                        if (intt != null)
                        {
                            if (temp.Count() != 0)
                            {
                                if (temp.Last().Location.end <= intt.Location.start) 
                                { 
                                    temp.Add(intt); 
                                }
                            }
                            else
                            {
                                temp.Add(intt);
                            }
                        }
                        foreach (var rt in rts)
                        {
                            if (rt != null)
                            {
                                if (temp.Count() != 0)
                                {
                                    if (temp.Last().Location.end <= rt.Location.start)
                                    {
                                        temp.Add(rt);
                                    }
                                }
                                else
                                {
                                    temp.Add(rt);
                                }
                            }
                            foreach (var rh in rhs)
                            {
                                if (rh != null)
                                {
                                    if (temp.Count() != 0)
                                    {
                                        if (temp.Last().Location.end <= rh.Location.start)
                                        {
                                            temp.Add(rh);
                                        }
                                    }
                                    else
                                    {
                                        temp.Add(rh);
                                    }
                                }

                                if (temp.Count != 0)
                                {
                                    result.Add((temp.Aggregate(0, (acc, feat) => acc + feat.Priority), temp));
                                }
                            }
                        }
                    }
                }
            }

            if (result.Count == 0)
            {
                return new List<Feature>();
            }

            var max = result.Max(pair => pair.score);
            return result.First(pair => pair.score == max).feats;
        }

        IEnumerable<Feature> PickBest(FeatureType type) => Domains
            .Where(d => d.Type == type)
            .OrderByDescending(d => d.Priority)
            .Append(null);
    }
}
