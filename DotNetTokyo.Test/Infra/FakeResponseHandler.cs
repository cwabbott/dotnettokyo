﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetTokyo.Test.Infra
{
    public class FakeResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _fakeResponses =
            new Dictionary<Uri, HttpResponseMessage>();

        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _fakeResponses.Add(uri, responseMessage);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_fakeResponses.ContainsKey(request.RequestUri))
                return Task.FromResult(_fakeResponses[request.RequestUri]);
            else
                return Task.FromResult(
                    new HttpResponseMessage(HttpStatusCode.NotAcceptable) {
                    RequestMessage = request
                    }
                );
        }
    }
}
