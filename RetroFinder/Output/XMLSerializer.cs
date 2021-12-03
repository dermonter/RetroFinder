using RetroFinder.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace RetroFinder.Output
{
    class XMLSerializer : ISerializer
    {
        public void SerializeAnalysisResult(SequenceAnalysis analysis)
        {
            Console.WriteLine("Enter the location to generate the xml to:");
            var location = Console.ReadLine();

            var xmlSerializer = new XmlSerializer(typeof(SerializedTransposon[]));
            using TextWriter writer = new StreamWriter(Path.Combine(location, analysis.Sequence.Id + ".xml"));
            xmlSerializer.Serialize(writer, analysis.Transposons.Select(trans => new SerializedTransposon()
            {
                Location = new SerializedLocation()
                {
                    Start = trans.Location.start,
                    End = trans.Location.end
                },
                Features = trans.Features.Select(feat => new SerializedFeature()
                {
                    Type = feat.Type.ToString(),
                    Location = new SerializedLocation()
                    {
                        Start = feat.Location.start,
                        End = feat.Location.end
                    }
                })
                .ToArray()
            }).ToArray());
        }
    }
}
