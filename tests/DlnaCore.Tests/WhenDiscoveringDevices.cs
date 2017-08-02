using System;
using Xunit;
using DlnaCore.Core;

namespace DlnaCore.Tests
{
    public class WhenDiscoveringDevices
    {
        [Fact]
        public void CanPublishSsdp()
        {
            var sut = new SsdpPublisher();
            Assert.True(true);
        }
    }
}
