using System;
using Xunit;
using DlnaCore.Core;
using System.Threading.Tasks;
using System.Linq;
using Rssdp;
using System.Collections.Generic;

namespace DlnaCore.Tests
{
    public class WhenDiscoveringDevices
    {
        [Fact]
        public async Task CanPublishSsdp()
        {
            var sut = new SsdpPublisher();
            sut.Publish();

            var devices = await FindDevicesAsync();
            Assert.True(devices.Any(d => d.Usn.Contains(SsdpPublisher.Uuid)));
        }


        private async Task<IEnumerable<DiscoveredSsdpDevice>> FindDevicesAsync()
        {
            using (var deviceLocator = new SsdpDeviceLocator())
            {
                return await deviceLocator.SearchAsync();
            }
        }
    }
}
