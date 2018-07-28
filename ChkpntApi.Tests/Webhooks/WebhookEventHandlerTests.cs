using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
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

        [Fact]
        public void FileDoesNotExist()
        {
            _filesystem.AddFile("./handlers/repo1/push", new MockFileData("") );
            var options = Options.Create(new WebhooksSettings { HandlersPath = @"./handlers" });
            var sut = new WebhookEventHandler(options, _filesystem, _processRunner.Object);
            
            sut.Consume(@event: "does-not-exist", id: "repo1");

            _processRunner.Verify(p => p.Start(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void FileExists()
        {
            _filesystem.AddFile("./handlers/repo1/push", new MockFileData("") );
            var options = Options.Create(new WebhooksSettings { HandlersPath = @"./handlers" });
            var sut = new WebhookEventHandler(options, _filesystem, _processRunner.Object);

            sut.Consume(@event: "push", id: "repo1");

            _processRunner.Verify(p => p.Start("./handlers/repo1/push"));
        }
    }
}
