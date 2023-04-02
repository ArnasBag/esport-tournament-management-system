using ESTMS.API.Core.Exceptions;
using ESTMS.API.DataAccess.Entities;
using ESTMS.API.DataAccess.Repositories;

namespace ESTMS.API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id)
            ?? throw new BadRequestException("User with this id doesn't exist.");

        return user;
    }
}
