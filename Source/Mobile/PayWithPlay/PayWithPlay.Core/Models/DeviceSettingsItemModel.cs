using PayWithPlay.Core.Enums;

namespace PayWithPlay.Core.Models
{
    public class DeviceSettingsItemModel
    {
        public string? Title { get; set; }

        public DeviceSettingsType Type { get; set; }

        public bool Enabled { get; set; }
    }
}
