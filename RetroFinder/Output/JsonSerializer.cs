using System;
using System.IO;
using System.Linq;

namespace RetroFinder.Output
{
    class JsonSerializer : ISerializer
    {
        public void SerializeAnalysisResult(SequenceAnalysis analysis)
        {
            Console.WriteLine("Enter the location to generate the json to:");
            var location = Console.ReadLine();

            var options = new System.Text.Json.JsonSerializerOptions();
            options.WriteIndented = true;
            var jsonString = System.Text.Json.JsonSerializer.Serialize(
                analysis.Transposons.Select(trans => new SerializedTransposon()
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
                }),
                options
                );
            File.WriteAllText(Path.Combine(location, analysis.Sequence.Id + ".json"), jsonString);
        }
    }
}
