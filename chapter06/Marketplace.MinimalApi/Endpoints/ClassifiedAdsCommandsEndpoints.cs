using Marketplace.MinimalApi.Application;
using Marketplace.MinimalApi.Models;

namespace Marketplace.MinimalApi.Endpoints;

public static class ClassifiedAdsCommandsEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/ad",
            async (ClassifiedAdsApplicationService applicationService, ClassifiedAds.V1.Create request) =>
            {
                await applicationService.Handle(request);
                return Results.Ok();
            })
            .WithName("Create a new classified ad")
            .WithTags("ClassifiedAdsCommandsApi")
            .WithOpenApi();

        app.MapPut("/ad/name",
            async (ClassifiedAdsApplicationService applicationService, ClassifiedAds.V1.SetTitle request) =>
            {
                await applicationService.Handle(request);
                return Results.Ok();
            })
            .WithName("Set classified ad title")
            .WithTags("ClassifiedAdsCommandsApi")
            .WithOpenApi();

        app.MapPut("/ad/text", 
            async (ClassifiedAdsApplicationService applicationService, ClassifiedAds.V1.UpdateText request) =>
            {
                await applicationService.Handle(request);
                return Results.Ok();
            })
            .WithName("Update classified ad text")
            .WithTags("ClassifiedAdsCommandsApi")
            .WithOpenApi();

        app.MapPut("/ad/price", 
            async (ClassifiedAdsApplicationService applicationService, ClassifiedAds.V1.UpdatePrice request) =>
            {
                await applicationService.Handle(request);
                return Results.Ok();
            })
            .WithName("Update classified ad price")
            .WithTags("ClassifiedAdsCommandsApi")
            .WithOpenApi();

        app.MapPut("/ad/publish", 
            async (ClassifiedAdsApplicationService applicationService, ClassifiedAds.V1.RequestToPublish request) =>
            {
                await applicationService.Handle(request);
                return Results.Ok();
            })
            .WithName("Request to publish classified ad")
            .WithTags("ClassifiedAdsCommandsApi")
            .WithOpenApi();
    }
}