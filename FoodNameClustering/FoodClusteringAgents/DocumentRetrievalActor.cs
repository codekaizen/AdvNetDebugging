using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Akka.Actor;
using AngleSharp.Network;
using LemmaSharp;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Esha.Analysis.FoodClusteringAgents
{
#if DEBUG
    internal static class DebugHelper
    {
        private static readonly HashSet<String> _knownMimeTypes = new HashSet<String>();

        public static Boolean CheckUnknownMimeType(HttpResponseMessage response)//(Object value, ICollection<Object> objects, Boolean @break)
        {
            return !_knownMimeTypes.Contains(response.Content.Headers.ContentType.MediaType);
        }

        // Works in old debugger (managed compatibility mode) only
        public static Boolean CollectUnknownMimeTypes(Object c, Object value)
        {
            ((ICollection<String>)c).Add(value?.ToString());
            return true;
        }
    }
#endif

    public class DocumentRetrievalActor : HttpClientReceiveActor
    {
        private static readonly HashSet<String> _stopWords = new HashSet<String>(new[] { "a", "about", "above", "above", "across", "after", "afterwards", "again", "against", "all", "almost", "alone", "along", "already", "also", "although", "always", "am", "among", "amongst", "amoungst", "amount", "an", "and", "another", "any", "anyhow", "anyone", "anything", "anyway", "anywhere", "are", "around", "as", "at", "back", "be", "became", "because", "become", "becomes", "becoming", "been", "before", "beforehand", "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom", "but", "by", "call", "can", "cannot", "cant", "co", "con", "could", "couldnt", "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", "eight", "either", "eleven", "else", "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front", "full", "further", "get", "give", "go", "had", "has", "hasnt", "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", "herself", "him", "himself", "his", "how", "however", "hundred", "ie", "if", "in", "inc", "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", "latterly", "least", "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of", "off", "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over", "own", "part", "per", "perhaps", "please", "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", "these", "they", "thickv", "thin", "third", "this", "those", "though", "three", "through", "throughout", "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was", "we", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves", "the" });
        private static readonly Regex _splitRegex = new Regex(@"\W");
        public DocumentRetrievalActor()
        {
            Receive<RetrieveDocumentRequestMessage>(async r =>
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, r.DocumentUri);
                    var response = await DoHttpRequestAsync(request, r.Timeout);

                    if (response.Content.Headers.ContentType.MediaType != MimeTypeNames.Html)
                    {
                        throw new UnsupportedDocumentContentTypeException(response.Content.Headers.ContentType.MediaType, r.DocumentUri);
                    }

                    var htmlResult = await response.Content.ReadAsStringAsync();
                    var documentVector = vectorizeDocument(htmlResult);
                    var searchResultDoc = new SearchResultDocument(r.SearchUri, r.FoodNameQuery, r.DocumentUri, documentVector);
                    Sender.Tell(new RetrieveDocumentResultMessage(r, searchResultDoc));
                }
                catch (Exception exp)
                {
                    Sender.Tell(new RetrieveDocumentFailedMessage(r, exp));
                }
            });
        }

        private DocumentVector vectorizeDocument(String htmlResult)
        {
            // Get term vector
            var lmtz = new LemmatizerPrebuiltCompact(LanguagePrebuilt.English);
            var documentVector = from s in _splitRegex.Split(htmlResult)
                                 where !String.IsNullOrWhiteSpace(s)
                                 let canonical = s.ToLower()
                                 where !_stopWords.Contains(canonical) && canonical.Length > 1
                                 select lmtz.Lemmatize(s);
            return new DocumentVector(documentVector);
        }
    }
}