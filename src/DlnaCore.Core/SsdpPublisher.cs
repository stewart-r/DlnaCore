using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Rssdp;

namespace DlnaCore.Core
{
    public class SsdpPublisher
    {
        public const string Uuid = "c624d24f-8325-448d-a9c3-1845a102be29";
        private SsdpDevicePublisher _publisher;
        private SsdpRootDevice _deviceDefinition => new SsdpRootDevice()
        {
            CacheLifetime = TimeSpan.FromMinutes(10),
            Location = new Uri("http://localhost:5000/DeviceDescription.xml"),
            DeviceTypeNamespace = "DlnaCore",
            DeviceType = "MyDlnaServer",
            FriendlyName = "My Dlna Server",
            Manufacturer = "Stewart Robertson",
            Uuid = Uuid,
            ModelName = "My Model Dlna Server",
            
        };

        public string GetDescription()
        {
            return _deviceDefinition.ToDescriptionDocument();
        }

        public SsdpPublisher()
        {
            _publisher = new SsdpDevicePublisher();  
        }

        public async Task PublishAsync()
        {
            
            var endpoint = new IPEndPoint(IPAddress.Broadcast,1900);
            var basepayload = Encoding.UTF8.GetBytes(GenerateNotifyMessage($"uuid:{Uuid}",$"uuid:{Uuid}"));
            var rootpayload = Encoding.UTF8.GetBytes(_rootMsg);
            var mediaServerPayload = Encoding.UTF8.GetBytes(GenerateNotifyMessage($"uuid:{Uuid}::urn:schemas-upnp-org:device:MediaServer:1",
                "urn:schemas-upnp-org:device:MediaServer:1"));
            var mediaRegPayload = Encoding.UTF8.GetBytes(GenerateNotifyMessage($"uuid:{Uuid}::urn:microsoft.com:service:X_MS_MediaReceiverRegistrar:1",
                "urn:microsoft.com:service:X_MS_MediaReceiverRegistrar:1"));
            var contentDirPayload = Encoding.UTF8.GetBytes(GenerateNotifyMessage($"uuid:{Uuid}::urn:schemas-upnp-org:service:ContentDirectory:1",
                "urn:schemas-upnp-org:service:ContentDirectory:1"));
            var connectionManager = Encoding.UTF8.GetBytes(GenerateNotifyMessage($"uuid:{Uuid}::urn:schemas-upnp-org:service:ConnectionManager:1",
                "urn:schemas-upnp-org:service:ConnectionManager:1"));

            for (var i = 1; i < 10; i++)
            {
                using (var udpClient = new UdpClient())
                {
                    await BroadcastAsync(udpClient, basepayload, basepayload.Length, endpoint);
                    await BroadcastAsync(udpClient, rootpayload, rootpayload.Length, endpoint);
                    await BroadcastAsync(udpClient, mediaServerPayload, mediaServerPayload.Length, endpoint);
                    await BroadcastAsync(udpClient, mediaRegPayload, mediaRegPayload.Length, endpoint);
                    await BroadcastAsync(udpClient, contentDirPayload, contentDirPayload.Length, endpoint);
                    await BroadcastAsync(udpClient, connectionManager, connectionManager.Length, endpoint);
                    await Task.Delay(150);
                }

            }
            
        
        }

        private async Task BroadcastAsync(UdpClient udpClient, byte[] payload, int length, IPEndPoint endpoint)
        {
            
            { 
                var destination = new IPEndPoint(IPAddress.Parse("239.255.255.250"),1900);
                await udpClient.SendAsync(payload,length,destination);
            }
        }

        private string GenerateNotifyMessage(string usn, string nt)
        {

            return $@"NOTIFY * HTTP/1.1
Host: 239.255.255.250:1900
Location: http://192.168.0.25:5000/DeviceDescription.xml
Cache-Control: max-age=1800
Server: UPnP/1.0 DLNADOC/1.50 Platinum/1.0.5.13
NTS: ssdp:alive
USN: {usn}
NT: {nt}" + "\r\n\r\n";
        }

        private string _rootMsg 
        {
            get
            {
                return GenerateNotifyMessage($"uuid:{Uuid}::upnp:rootdevice", "upnp:rootdevice");
            }
        }

    }
}
