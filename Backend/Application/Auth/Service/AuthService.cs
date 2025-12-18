
using Application.Auth.Dto.Request;
using Application.Auth.Dto.Response;
using Application.Auth.Interfaces;
using Application.Exceptions;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;   
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper; 
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IJwtService jwtService, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterDto request)
        {
            var userNameExists = await _userManager.FindByNameAsync(request.UserName);
            if(userNameExists != null) throw new ConflictException("Username already exists");

            var exists = await _userManager.FindByEmailAsync(request.Email);
            if (exists != null)
            {
                throw new ConflictException("User already exists");
            }

            var user = _mapper.Map<User>(request);
            var result = await _userManager.CreateAsync(user, request.Password); 
            if(result == null || !result.Succeeded) throw new ArgumentException("Failed to create user");

            await _userManager.AddToRoleAsync(user, "User");
            return _mapper.Map<RegisterResponseDto>(user);
        }

        public async Task<LoginDtoResponse> LoginAsync(LoginDto request, CancellationToken ct)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new KeyNotFoundException("Invalid credentials");
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if(!result.Succeeded) throw new UnauthorizedAccessException("Invalid credentials");

            var role = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, role);
            var refreshToken = GenerateSecureToken();
            var refreshTokenHash = Sha256(refreshToken);
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshTokenHash,
                JwtId = token,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAtUtc = DateTime.UtcNow,
            });
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(ct);

            var response = _mapper.Map<LoginDtoResponse>(user);
            response.AccessToken = token;
            response.RefreshToken = refreshToken;
            return response;
        }

        public async Task<UpdateProfileResponse> UpdateProfileAsync(Guid userId, UpdateProfileDto request, CancellationToken ct)
        {
            var userExist = await _userManager.FindByIdAsync(userId.ToString());
            if(userExist == null)
            {
                throw new InvalidOperationException("User not found");
            }

                if (!string.IsNullOrEmpty(request.FirstName))
                {
                    userExist.FirstName = request.FirstName;
                }

                if (!string.IsNullOrEmpty(request.LastName))
                {
                    userExist.LastName = request.LastName;
                }

                if(!string.IsNullOrEmpty(request.Email) && !string.Equals(userExist.Email, request.Email, StringComparison.OrdinalIgnoreCase))
                {                                  
                    userExist.Email = request.Email;
                    userExist.UserName = request.Email;
                }

                if(!string.IsNullOrEmpty(request.PhoneNumber) && !string.Equals(userExist.PhoneNumber, request.PhoneNumber, StringComparison.OrdinalIgnoreCase))
                {
                    userExist.PhoneNumber = request.PhoneNumber;
                }


             await _userManager.UpdateAsync(userExist);
            var updatedUser = await _userManager.FindByIdAsync(userId.ToString());

            var (accessToken, refreshToken) = await ReissueTokensAsync(updatedUser,ct);
            return new UpdateProfileResponse
            {
                AccessToken =  accessToken,
                RefreshToken = refreshToken
            };

        }

        public async Task<UpdateProfileResponse> ChangePassword(Guid userId, ChangePasswordDto request, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }       

            var newPassword = await _userManager.ChangePasswordAsync(user,request.CurrentPassword,request.NewPassword);
            if(!newPassword.Succeeded)
            {
                throw new ArgumentException("Failed to change password");
            }

            var update = await _userManager.UpdateAsync(user);
            var updatedUser = await _userManager.FindByIdAsync(userId.ToString());
            
                var (accessToken, refreshToken) = await ReissueTokensAsync(updatedUser, ct);
                return new UpdateProfileResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };        
        }

        public async Task Logout(Guid userId, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if(user == null)
            {
                throw new InvalidOperationException("User not found");
            }
            await _jwtService.RevokeAllAsync(user, ct);
        }

        //Helper methods 
        private static string GenerateSecureToken()
        {
            var randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        public async Task<(string AccessToken, string RefreshToken)> ReissueTokensAsync(User user, CancellationToken ct)
        {
            await _jwtService.RevokeAllAsync(user, ct);
            
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtService.GenerateToken(user, roles);
            var refreshToken = GenerateSecureToken();
            var refreshTokenHash = Sha256(refreshToken);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshTokenHash,
                JwtId = accessToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedAtUtc = DateTime.UtcNow
            });

            await _userManager.UpdateAsync(user);

            return (accessToken, refreshToken);
        }
    }
}
