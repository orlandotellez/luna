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

    public async Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user is null) throw AppExceptions.NotFound("User NotFound");

        if (request.Name is not null) user.Name = request.Name;

        if (request.Phone is not null) user.Phone = request.Phone;

        if (request.Bio is not null) user.Bio = request.Bio;

        if (request.UserName is not null) user.UserName = request.UserName;

        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);

        return updatedUser.MapUserToDto();
    }
}
