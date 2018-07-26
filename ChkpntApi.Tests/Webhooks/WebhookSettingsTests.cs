using System;
using System.IO;
using System.Runtime.InteropServices;
using Xunit;

namespace ChkpntApi.Webhooks.Tests
{
    public class WebhookSettingsTests
    {
        [Fact]
        public void RelativePath()
        {
            var sut = new WebhooksSettings { HandlersPath = "./script" };

            var expected = Path.Combine(Directory.GetCurrentDirectory(), "script");
            Assert.Equal(expected, sut.HandlersPath);
        }

        [SkippableFact]
        public void AbsolutePath_Windows()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));

            var sut = new WebhooksSettings { HandlersPath = @"c:\handlers\scripts" };

            Assert.Equal(@"c:\handlers\scripts", sut.HandlersPath);
        }

        [SkippableFact]
        public void AbsolutePath_LinuxOrMacOs()
        {
            Skip.IfNot(RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.OSX));

            var sut = new WebhooksSettings { HandlersPath = @"/var/lib/handlers/scripts" };

            Assert.Equal(@"/var/lib/handlers/scripts", sut.HandlersPath);
        }
    }
}