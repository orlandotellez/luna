using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Interfaces.Repositories;
using Luna.Application.Common.Models;
using Luna.Application.Common.Mapping;
using Luna.Domain.Exceptions;

namespace Luna.Application.Features.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetMyProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null) throw AppExceptions.NotFound("User NotFound");

        return user.MapUserToDto();
    }
}
