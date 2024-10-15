using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningApp
{
    public class Phonetic
    {
        public string? Text { get; set; }
        public string? Audio { get; set; }
        public string? SourceUrl { get; set; }
    }


    public class Definition
    {
        public string? DefinitionText { get; set; }
        public List<string>? Synonyms { get; set; }
        public List<string>? Antonyms { get; set; }
        public string? Example { get; set; }
    }

    public class Meaning
    {
        public string? PartOfSpeech { get; set; }
        public List<Definition>? Definitions { get; set; }
        public List<string>? Synonyms { get; set; }
        public List<string>? Antonyms { get; set; }
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
            string? nounDefinition = null;
            string? verbDefinition = null;
            string? adjectiveDefinition = null;
            string? adverbDefinition = null;

            foreach (var meaning in Meanings)
            {
                if (meaning.PartOfSpeech == "noun" && nounDefinition == null)
                {
                    nounDefinition = meaning.Definitions?[0].DefinitionText;
                }
                else if (meaning.PartOfSpeech == "verb" && verbDefinition == null)
                {
                    verbDefinition = meaning.Definitions?[0].DefinitionText;
                }
                else if (meaning.PartOfSpeech == "adjective" && adjectiveDefinition == null)
                {
                    adjectiveDefinition = meaning.Definitions?[0].DefinitionText;
                }
                else if (meaning.PartOfSpeech == "adverb" && adverbDefinition == null)
                {
                    adverbDefinition = meaning.Definitions?[0].DefinitionText;
                }
            }

            Console.WriteLine($"Noun: {nounDefinition}");
            Console.WriteLine($"Verb: {verbDefinition}");
            Console.WriteLine($"Adjective: {adjectiveDefinition}");
            Console.WriteLine($"Adverb: {adverbDefinition}");
        }
    }
}
