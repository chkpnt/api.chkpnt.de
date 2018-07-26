using System.IO.Abstractions;
using Microsoft.Extensions.Options;

namespace ChkpntApi.Webhooks
{
    public class WebhookEventHandler
    {
        private readonly WebhooksSettings _settings;
        private readonly IFileSystem _filesystem;

        public WebhookEventHandler(IOptions<WebhooksSettings> options, IFileSystem filesystem)
        {
            _settings = options.Value;
            _filesystem = filesystem;
        }

        public void Consume(string @event, string id) {
            
        }
    }
}