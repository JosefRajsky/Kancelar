using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Web_Api
{
    public class ProxyHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken cancellationToken)
        {
            UriBuilder forwardUri = new UriBuilder(request.RequestUri);
            //strip off the proxy port and replace with an Http port
            forwardUri.Port = 80;
            //send it on to the requested URL
            request.RequestUri = forwardUri.Uri;
            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
            return response;
        }
    }
}
