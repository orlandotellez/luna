using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Interfaces.Repositories;
using Luna.Application.Common.Models;
using Luna.Application.Common.Mapping;
using Luna.Domain.Exceptions;
using Luna.Domain.Enums;
using Luna.Domain.Entities;

namespace Luna.Application.Features.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public UserService(
        IUserRepository userRepository,
        IUserProfileRepository userProfileRepository
        )
    {
        _userRepository = userRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<UserDto> GetMyProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        user.Profile = await _userProfileRepository.GetByUserIdAsync(userId);

        return user.MapUserToDto();
    }

    public async Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var profile = await _userProfileRepository.GetByUserIdAsync(userId);
        if (profile is null)
        {
            profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userProfileRepository.CreateAsync(profile);
        }


        if (request.UserName is not null && !string.Equals(request.UserName, profile.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existingProfile = await _userProfileRepository.GetByUserNameAsync(request.UserName);

            if (existingProfile is not null) throw AppExceptions.Conflict("UserName is already taken");

            profile.UserName = request.UserName;
        }

        if (request.Name is not null) user.Name = request.Name;
        if (request.Phone is not null) user.Phone = request.Phone;
        if (request.Bio is not null) profile.Bio = request.Bio;

        user.UpdatedAt = DateTime.UtcNow;
        profile.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        await _userProfileRepository.UpdateAsync(profile);

        return user.MapUserToDto();
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

        updatedUser.Profile = await _userProfileRepository.GetByUserIdAsync(userId);

        return updatedUser.MapUserToDto();
    }
}
