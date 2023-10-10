# Chapter 06: Acting With Commands

## Changes

- Minimal Api
- Folder Structure
- Docker Compose
- Testing

### Minimal Api

I decided on using the Minimal API approach for the Marketplace project.
In order to better separate my solution and [Alexey's](https://github.com/alexeyzimarev) I decided on creating my project as a new project (and tried to keep his solution in the Marketplace solution).

Since the controller actions are so slim in this chapter, I thought the Minimal API approach would be better suited.
Minimal API did not exists in .NET Core 2.1, so I highly doubt that [Alexey](https://github.com/alexeyzimarev) thought about it (and who can blame him?).

### Folder Structure

I tried to keep the folder structure as close to [Alexey's](https://github.com/alexeyzimarev) as possible, at least in the Marketplace project.

In the Minimal API project, I have taken the liberty to change the folder structure a bit.
I have the following folders:

- Application
- Endpoints
- Infrastructure
- Models

#### Application

The Application folder contains the `ClassifiedAdsApplicationService.cs`, and the `FixedCurrencyLookup.cs` classes.
While I agree that the entire solution is small enough that it doesn't warrant a separate project for the Application layer, I decided to keep it in a separate folder, to better demonstrate that these files belong in the Application layer.

#### Endpoints

The Endpoints folder contains the `ClassifiedAdsCommandsEndpoints.cs` class.
The class is responsible for setting up the endpoints for the application.

```csharp
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

        // Rest of the endpoints here
    }
}
```

#### Infrastructure

The Infrastructure folder contains the `ClassifiedAdRepository.cs` class.
While I agree that the entire solution is small enough that it doesn't warrant a separate project for the Infrastructure layer, I decided to keep it in a separate folder, to better demonstrate that this file belong in the Infrastructure layer.

#### Models

The Models folder contains the `ClassifiedAd.cs` class.

While I think that nested classes are a bad idea, I also think that separating the models into separate files for each version can quickly turn into a mess, especially for the endpoints.

```csharp
public static class ClassifiedAds
{
    public static class V1
    {
        public class Create
        {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
        }
        // The rest of the models here
    }
}
```

[Alexey](https://github.com/alexeyzimarev) writes that the "contract" models are practically DTO's or POCO's. I have worked too long with MVC, so in my head the file(s) go into a model folder. He chose a Contracts folder, which I don't necessary disagree on, but we could also go with DTO as the folder name 🤷‍♂️

### Docker Compose

In his docker compose file, [Alexey](https://github.com/alexeyzimarev) is using the `latest` tag for the RavenDB image. I have changed it to use the `6.0` tag, which is the latest stable version at the time of writing.
I think you should be very specific about the version of the image you are using, especially in Production.
By aligning development and production, you can faster discover issues that might arise from using different versions.

```yaml
version: '3.5'

services:
  ravendb:
    container_name: marketplace-ravendb
    image: ravendb/ravendb:6.0-ubuntu-latest
    ports:
      - "8080:8080"
    environment:
      - RAVEN_Security_UnsecuredAccessAllowed=PublicNetwork
      - RAVEN_ARGS="--Setup.Mode=None"

```

### Testing

In this chapter he skips writing any test entirely, and I think the sooner you implement tests for your API, the easier it is to write them.

So I have decided to write tests for the endpoints, application, and infrastructure layer.

This was a great opportunity to try out the new `Microsoft.AspNetCore.Mvc.Testing` package, which allows you to test your API using the `WebApplicationFactory<TEntryPoint>` class.
It's also a great opportunity to use TestContainers, which allows you to spin up a Docker container for your tests. - I use it for RavenDB in this project.

## Thoughts

 - RavenDB
 - Command Handler Pattern

### RavenDB

I have never used RavenDB before, and I also think that a lot of .NET developers haven't either.
I would have like a bit more explanation on the setup, and why RavenDB was chosen.

### Command Handler Pattern

He talks a bit about the Command Handler pattern, and that he has decided against it for this project.
I understand his reasoning, but I think using the MediatR library would have eased the implementation of the pattern, and the library lends it self very well to event sourcing and CQRS in general.
