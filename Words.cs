using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LearningApp
{
    public class Phonetic
    {
        public string? Text { get; set; }
        public string? Audio { get; set; }
    }

    public class Definition
    {
        [JsonProperty("definition")]
        public string? DefinitionText { get; set; }
        public string? Example { get; set; }
    }

    public class Meaning
    {
        public string? PartOfSpeech { get; set; }
        public List<Definition>? Definitions { get; set; }
    }

    public class Words
    {
        public string? Word { get; set; }
        public List<Phonetic>? Phonetics { get; set; }
        public List<Meaning>? Meanings { get; set; }

        public Words(string? word, List<Phonetic>? phonetics, List<Meaning>? meanings)
        {
            Word = word;
            Phonetics = phonetics;
            Meanings = meanings;
        }

        public void ExtractPartsOfSpeech()
        {
            string nounDefinition = "Not available";
            string verbDefinition = "Not available";
            string adjectiveDefinition = "Not available";
            string adverbDefinition = "Not available";

            if (Meanings != null)
            {
                foreach (var meaning in Meanings)
                {
                    if (meaning.Definitions != null && meaning.Definitions.Count > 0)
                    {
                        var firstDefinition = meaning.Definitions[0].DefinitionText;

                        switch (meaning.PartOfSpeech)
                        {
                            case "noun" when nounDefinition == "Not available":
                                nounDefinition = firstDefinition;
                                break;
                            case "verb" when verbDefinition == "Not available":
                                verbDefinition = firstDefinition;
                                break;
                            case "adjective" when adjectiveDefinition == "Not available":
                                adjectiveDefinition = firstDefinition;
                                break;
                            case "adverb" when adverbDefinition == "Not available":
                                adverbDefinition = firstDefinition;
                                break;
                        }
                    }
                }
            }

            Console.WriteLine($"Noun: {nounDefinition}");
            Console.WriteLine($"Verb: {verbDefinition}");
            Console.WriteLine($"Adjective: {adjectiveDefinition}");
            Console.WriteLine($"Adverb: {adverbDefinition}");
        }
    }
}
