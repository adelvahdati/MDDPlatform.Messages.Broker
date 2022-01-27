namespace MDDPlatform.Messages.BrokerConfiguration {
    public interface IBusConfiguration {
        public string HostName { get; set; }
        public int Port { get; set; }
    }
}