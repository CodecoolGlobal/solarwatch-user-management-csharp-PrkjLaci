﻿using SolarWatch.Contracts;

namespace SolarWatch.Service.Authentication;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password, string role);
    Task<AuthResult> LoginAsync(string email, string password);
    Task<UserResponse> GetUserInformation(string email);
}