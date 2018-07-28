using System.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.Options;

namespace ChkpntApi.Webhooks
{
    public class WebhookEventHandler
    {
        private readonly WebhooksSettings _settings;
        private readonly IFileSystem _filesystem;
        private readonly IProcessRunner _process;

        public WebhookEventHandler(IOptions<WebhooksSettings> options, IFileSystem filesystem, IProcessRunner processRunner)
        {
            _settings = options.Value;
            _filesystem = filesystem;
            _process = processRunner;
        }

        public void Consume(string @event, string id) {
            var executable = _filesystem.Path.Combine(_settings.HandlersPath, id, @event);
            if (DoesNotExist(executable)) { return; }

            _process.Start(executable);
        }

        private bool DoesNotExist(string path)
        {
            var absolutePath = _filesystem.Path.GetFullPath(path);
            return !_filesystem.File.Exists(absolutePath);
        }
    }
}