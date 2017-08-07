using System;
using Xunit;
using DlnaCore.Core;
using System.Threading.Tasks;
using System.Linq;
using Rssdp;
using System.Collections.Generic;
using System.Threading;

namespace DlnaCore.Tests
{
    public class WhenPublishingDevice
    {
        [Fact]
        public async Task DeviceWithPublisherUuidIsDetected()
        {
            var sut = new SsdpPublisher();

            sut.Publish();

            var devices = await FindDevicesAsync();
            Assert.True(devices.Any(d => d.Usn.Contains(SsdpPublisher.Uuid)));
        }

        [Fact]
        public async Task DeviceDescriptionIsFound()
        {
            var ctSrc = new CancellationTokenSource();
            var t = Task.Run(() => Program.RunServerWithCancellation(ctSrc.Token));
            var sut = new SsdpPublisher();
            
            sut.Publish();
            
            var devices = await FindDevicesAsync();
            var ourDevice = devices.First(d => d.Usn.Contains(SsdpPublisher.Uuid));
            var deviceDesc = await ourDevice.GetDeviceInfo();

            Assert.NotNull(deviceDesc);
            ctSrc.Cancel();
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
