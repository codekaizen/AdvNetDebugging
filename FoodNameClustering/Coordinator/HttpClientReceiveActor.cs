using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;

namespace Coordinator
{
    public abstract class HttpClientReceiveActor : ReceiveActor
    {
        protected async Task<HttpResponseMessage> DoHttpRequestAsync(HttpRequestMessage request, TimeSpan timeout)
        {
            var cancellationToken = new CancellationTokenSource();

            using (var handler = CreateHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    AddDefaultHeadersToClient(client);
                    cancellationToken.CancelAfter(timeout);
                    var response = await client.SendAsync(request, cancellationToken.Token);
                    response.EnsureSuccessStatusCode();
                    return response;
                }
            }
        }

        protected virtual HttpClientHandler CreateHandler()
        {
            return new HttpClientHandler
            {
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
        }

        protected virtual void AddDefaultHeadersToClient(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/40.0.2214.115 Safari/537.36");
            client.DefaultRequestHeaders.Add("AcceptCharset", "utf-8");
        }
    }
}