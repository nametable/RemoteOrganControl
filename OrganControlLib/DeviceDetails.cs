namespace OrganControlLib
{
    public class DeviceDetails
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceString
        {
            get => $"{DeviceId, 10}: {DeviceName}";
        }
    }
}