namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public interface IConfigurable<out TOptions> where TOptions : class, new()
    {
        TOptions Options { get; }
    }
}
