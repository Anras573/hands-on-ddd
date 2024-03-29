﻿using Marketplace.Domain;
using Marketplace.Framework;
using Marketplace.MinimalApi.Models;

namespace Marketplace.MinimalApi.Application;

public class ClassifiedAdsApplicationService : IApplicationService
{
    private readonly IClassifiedAdRepository _repository;
    private readonly ICurrencyLookup _currencyLookup;

    public ClassifiedAdsApplicationService(IClassifiedAdRepository repository, ICurrencyLookup currencyLookup)
    {
        _repository = repository;
        _currencyLookup = currencyLookup;
    }
    
    public async Task Handle(object command)
    {
        switch (command)
        {
            case ClassifiedAds.V1.Create cmd:
                await HandleCreate(cmd);
                break;
            case ClassifiedAds.V1.SetTitle cmd:
                await HandleUpdate(cmd.Id, c => 
                    c.SetTitle(new ClassifiedAdTitle(cmd.Title)));
                break;
            case ClassifiedAds.V1.UpdateText cmd:
                await HandleUpdate(cmd.Id, c => 
                    c.UpdateText(ClassifiedAdText.FromString(cmd.Text)));
                break;
            case ClassifiedAds.V1.UpdatePrice cmd:
                await HandleUpdate(cmd.Id, c => 
                    c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.CurrencyCode, _currencyLookup)));
                break;
            case ClassifiedAds.V1.RequestToPublish cmd:
                await HandleUpdate(cmd.Id, c => 
                    c.RequestToPublish());
                break;
            default:
                throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown");
        }
    }

    private async Task HandleCreate(ClassifiedAds.V1.Create cmd)
    {
        if (await _repository.Exists(new ClassifiedAdId(cmd.Id)))
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

        var classifiedAd = new ClassifiedAd(new ClassifiedAdId(cmd.Id), new UserId(cmd.OwnerId));

        await _repository.Save(classifiedAd);
    }

    private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
    {
        var classifiedAd = await _repository.Load(new ClassifiedAdId(classifiedAdId));
        
        if (classifiedAd == null)
            throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
        
        operation(classifiedAd);
        
        await _repository.Save(classifiedAd);
    }
}