using System;
using Xunit;
using DlnaCore.Core;
using System.Threading.Tasks;
using System.Linq;
using Rssdp;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http;

namespace DlnaCore.Tests
{
    public class WhenPublishingDevice
    {
        [Fact]
        public async Task DeviceWithPublisherUuidIsDetected()
        {
            var sut = new SsdpPublisher();

            await sut.PublishAsync();

            var devices = await FindDevicesAsync();
            Assert.True(devices.Any(d => d.Usn.Contains(SsdpPublisher.Uuid)));
        }

        [Fact]
        public async Task CanReadDeviceDescriptionFromAddress()
        {
            using (var sut = Program.StartHost())
            {
                string xmlDoc;

                using (var client = new HttpClient())
                {
                    xmlDoc = await client.GetStringAsync("http://localhost:5000/DeviceDescription.xml");
                }
                
                Assert.False(string.IsNullOrWhiteSpace(xmlDoc));
            }
            
        }

        [Fact]
        public async Task DeviceDescriptionIsFound()
        {
            using (var h = Program.StartHost())
            {
                var sut = new SsdpPublisher();
                
                await sut.PublishAsync();
                
                var devices = await FindDevicesAsync();
                var ourDevice = devices.First(d => d.Usn.Contains(SsdpPublisher.Uuid));
                var deviceDesc = await ourDevice.GetDeviceInfo();

                Assert.NotNull(deviceDesc);
            }
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
