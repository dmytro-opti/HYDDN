using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface IUserService
{
    Task<UserModel> GetUser(Guid userId);
    Task<UserModel> CreateUser(UserModel model);
    Task<UserModel> UpdateUser(UserModel model);
    Task<UserModel> DeleteUser(Guid userId);
    
    Task<IEnumerable<UserInfoModel>> GetByMemberIds(IEnumerable<Guid> memberIds);
}