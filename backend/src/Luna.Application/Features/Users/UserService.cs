using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Interfaces.Repositories;
using Luna.Application.Common.Models;
using Luna.Application.Common.Mapping;
using Luna.Domain.Exceptions;
using Luna.Domain.Enums;

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


        if (request.UserName is not null && !string.Equals(request.UserName, user.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existingUser = await _userRepository.GetByUserNameAsync(request.UserName);

            if (existingUser is not null) throw AppExceptions.Conflict("UserName is already taken");
        }

        if (request.Name is not null) user.Name = request.Name;

        if (request.Phone is not null) user.Phone = request.Phone;

        if (request.Bio is not null) user.Bio = request.Bio;

        if (request.UserName is not null) user.UserName = request.UserName;

        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);

        return updatedUser.MapUserToDto();
    }


    public async Task<UserDto> UpdateLifeStageAsync(Guid userId, UpdateLifeStageRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        user.LifeStage = request.LifeStage;

        if (request.LifeStage == LifeStage.Pregnancy)
        {
            user.LastMenstrualPeriod = request.LastMenstrualPeriod;
            user.EstimatedDueDate = request.EstimatedDueDate;
        }
        else
        {
            user.LastMenstrualPeriod = null;
            user.EstimatedDueDate = null;
        }

        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);

        return updatedUser.MapUserToDto();
    }
}
