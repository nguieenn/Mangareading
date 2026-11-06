using Microsoft.Extensions.Caching.Memory;
using Mangareading.Services ;
namespace Services
{
    public class Startup
    {
                public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ApiCacheService>();
        }
    }
} 