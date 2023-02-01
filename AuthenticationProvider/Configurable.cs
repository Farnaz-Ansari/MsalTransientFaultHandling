namespace MsalTransientFaultHandling.AuthenticationProvider
{
    public abstract class Configurable<TOptions> : IConfigurable<TOptions> where TOptions : class, new()
    {
        protected Configurable(TOptions options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            Options = options;
        }

        public TOptions Options { get; }
    }
}
