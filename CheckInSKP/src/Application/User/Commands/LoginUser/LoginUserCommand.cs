﻿using CheckInSKP.Application.Common.Interfaces;
using CheckInSKP.Application.Device.Commands.UpdateDevice;
using CheckInSKP.Domain.Enums;
using CheckInSKP.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSKP.Application.User.Commands.LoginUser
{
    public record LoginUserCommand : IRequest<string>
    {
        public required Guid DeviceId { get; init; }
        public required string Username { get; init; }
        public required string Password { get; init; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginUserCommandHandler(IDeviceRepository deviceRepository, IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _deviceRepository = deviceRepository ?? throw new ArgumentNullException(nameof(deviceRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            Domain.Entities.Device device = await _deviceRepository.GetByIdAsync(request.DeviceId) ?? throw new Exception($"Device with id {request.DeviceId} not found");
            Domain.Entities.User user = await _userRepository.GetByUsernameAsync(request.Username) ?? throw new Exception($"User with username {request.Username} not found");

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                throw new Exception("Invalid password");

            // Authorize the device if the user is an admin
            if (user.RoleId == (int)RoleEnum.Admin && !device.IsAuthorized)
            {
                device.Authorize();
                await _deviceRepository.UpdateAsync(device);
            }

            if (!device.IsAuthorized)
                throw new Exception($"Device with id {request.DeviceId} is not authorized");

            string token = _tokenService.GenerateAccessToken(user, device) ?? throw new Exception("Token generation failed");

            user.LoggedIn();

            await _unitOfWork.CompleteAsync(cancellationToken);
            return token;
        }
    }
}
