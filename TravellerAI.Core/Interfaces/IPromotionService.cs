using System.Runtime.InteropServices.JavaScript;

namespace TravellerAI.Core.Interfaces;

public interface IPromotionService
{
    Task<string> CreatePromoCodeAsync(string keyWord, JSType.Date startDate,  JSType.Date endDate);
    Task<string> ApplyPromoCodeAsync();
    Task<string> ActivatePromotionAsync();
    Task<string> DeactivatePromotionAsync();
    Task<string> getActivePromotionAsync();
    Task<string> getPromotionByIdAsync(string promotionId);
}