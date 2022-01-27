namespace MDDPlatform.Messages.BrokerConfiguration {
    public class BusConfiguration : IBusConfiguration
    {
        public string HostName { get ; set; }
        public int Port { get; set ; }

        public BusConfiguration(string hostName, int port)
        {
            HostName = hostName;
            Port = port;
        }
    }
}