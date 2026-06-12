using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class UserService : IUserService
{
    public Task<UserModel> CreateUser(UserModel model)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> DeleteUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> GetUser(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> UpdateUser(UserModel model)
    {
        throw new NotImplementedException();
    }
    
    public Task<IEnumerable<UserInfoModel>>  GetByMemberIds(IEnumerable<Guid> memberIds)
    {
        throw new NotImplementedException();
    }
}