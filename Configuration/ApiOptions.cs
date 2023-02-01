namespace MsalTransientFaultHandling.Configuration
{
    public abstract class ApiOptions
    {
        public string EndPoint { get; set; }
        public PolicyConfig PolicyOptions { get; set; }
        public MicrosoftIdentityClient Client { get; set; }
        public void Validate() => ArgumentNullException.ThrowIfNull(EndPoint, nameof(EndPoint));
    }
}
