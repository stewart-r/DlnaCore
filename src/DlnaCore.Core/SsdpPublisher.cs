using System;
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
            ModelName = "My Model Dlna Server"
        };

        public string GetDescription()
        {
            return _deviceDefinition.ToDescriptionDocument();
        }

        public SsdpPublisher()
        {
            _publisher = new SsdpDevicePublisher();  
        }

        public void Publish()
        {
            _publisher.AddDevice(_deviceDefinition);
        }
    }
}
