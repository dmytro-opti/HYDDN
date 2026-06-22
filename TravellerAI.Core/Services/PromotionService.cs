using System.Runtime.InteropServices.JavaScript;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Services;

public class PromotionService : IPromotionService
{
    public Task<string> CreatePromoCodeAsync(string keyWord, JSType.Date startDate, JSType.Date endDate)
    {
        throw new NotImplementedException();
    }

    public Task<string> ApplyPromoCodeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> ActivatePromotionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> DeactivatePromotionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> getActivePromotionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> getPromotionByIdAsync(string promotionId)
    {
        throw new NotImplementedException();
    }
}