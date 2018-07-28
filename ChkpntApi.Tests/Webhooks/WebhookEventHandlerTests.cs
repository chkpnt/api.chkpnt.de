using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ChkpntApi.Webhooks.Tests
{
    public class WebhookEventHandlerTests
    {

        private MockFileSystem _filesystem;
        private Mock<IProcessRunner> _processRunner;

        public WebhookEventHandlerTests()
        {
            _filesystem = new MockFileSystem();
            _processRunner = new Mock<IProcessRunner>();
        }

        [SkippableFact]
        public void FileDoesNotExist_Linux()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.OSX));
            
            _filesystem.AddFile("./handlers/repo1/push", new MockFileData("") );
            var options = Options.Create(new WebhooksSettings { HandlersPath = @"./handlers" });
            var sut = new WebhookEventHandler(options, _filesystem, _processRunner.Object);

            sut.Consume(@event: "does-not-exist", id: "repo1");

            _processRunner.Verify(p => p.Start(It.IsAny<string>()), Times.Never());
        }

        [SkippableFact]
        public void FileDoesNotExist_Windows()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
            
            _filesystem.AddFile(@".\handlers\repo1\push", new MockFileData("") );
            var options = Options.Create(new WebhooksSettings { HandlersPath = @".\handlers" });
            var sut = new WebhookEventHandler(options, _filesystem, _processRunner.Object);

            sut.Consume(@event: "does-not-exist", id: "repo1");

            _processRunner.Verify(p => p.Start(It.IsAny<string>()), Times.Never());
        }

        [SkippableFact]
        public void FileExists_Linux()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.OSX));

            _filesystem.AddFile("./handlers/repo1/push", new MockFileData("") );
            var options = Options.Create(new WebhooksSettings { HandlersPath = @"./handlers" });
            var sut = new WebhookEventHandler(options, _filesystem, _processRunner.Object);

            sut.Consume(@event: "push", id: "repo1");

            _processRunner.Verify(p => p.Start("./handlers/repo1/push"));
        }

        [SkippableFact]
        public void FileExists_Windows()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            _filesystem.AddFile(@".\handlers\repo1\push", new MockFileData("") );
            var options = Options.Create(new WebhooksSettings { HandlersPath = @".\handlers" });
            var sut = new WebhookEventHandler(options, _filesystem, _processRunner.Object);

            sut.Consume(@event: "push", id: "repo1");

            _processRunner.Verify(p => p.Start(@".\handlers\repo1\push"));
        }
    }
}
