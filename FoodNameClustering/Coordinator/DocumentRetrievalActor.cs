using System;
using System.Net.Http;
using Akka.Actor;

namespace Coordinator
{
    public class DocumentRetrievalActor : HttpClientReceiveActor
    {
        public DocumentRetrievalActor()
        {
            Receive<RetrieveDocumentRequestMessage>(async r =>
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, r.DocumentUri);
                    var response = await DoHttpRequestAsync(request, r.Timeout);
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
            throw new NotImplementedException();
        }
    }
}