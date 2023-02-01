using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web.TokenCacheProviders;
using Microsoft.Identity.Web.TokenCacheProviders.Distributed;
using MsalTransientFaultHandling.AuthenticationProvider;
using MsalTransientFaultHandling.Configuration;
using MsalTransientFaultHandling.WeatherForecastApi;
using MsalTransientFaultHandling.Policies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDistributedTokenCaches();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    builder.Services.AddStackExchangeRedisCache(o =>
    {
        o.Configuration = app.Configuration.GetConnectionString("RedisDistributedCache");
    });
}

builder.Services.Configure<MsalDistributedTokenCacheAdapterOptions>(app.Configuration.GetSection("MsalTokenCacheOptions"));

builder.Services.AddSingleton<IMsalTokenCacheProvider, MsalDistributedTokenCacheAdapter>();

builder.Services.Configure<DistributedCacheEntryOptions>(app.Configuration.GetSection("CachingOptions"));

builder.Services.AddSingleton<IActiveDirectoryTokenCache>(sp =>
{
    var distributedCache = sp.GetService<IDistributedCache>();
    var distributedCacheEntryOptions = sp.GetService<IOptions<DistributedCacheEntryOptions>>();
    return new ActiveDirectoryDistributedTokenCache(distributedCache, distributedCacheEntryOptions);
});

var policyOptions = app.Configuration.GetSection("PolicyOptions").Get<PolicyConfig>();

builder.Services.AddHttpClient<IMsalHttpClientFactory, MsalApiClient>()
        .AddPolicyHandler(HttpRetryPolicies.GetRetryPolicy(policyOptions, builder.Services.BuildServiceProvider().GetService<ILogger<MsalApiClient>>()));

builder.Services.AddWeatherForecastApiInfrastructure(app.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
