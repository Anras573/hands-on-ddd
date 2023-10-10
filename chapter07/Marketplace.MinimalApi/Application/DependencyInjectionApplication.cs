using Marketplace.Domain;

namespace Marketplace.MinimalApi.Application;

public static class DependencyInjectionApplication
{
    public static void AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
        builder.Services.AddScoped<ClassifiedAdsApplicationService>();
    }
}