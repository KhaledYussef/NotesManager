using Azure.Core;

using Core.Domains;
using Core.Domains.Auth;

using Data.DbContext;
using Data.DTOs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Services;
using Services.LoggerService;

using System;
using System.Security.Claims;

namespace Service.AuthService
{
    public class AuthService : BaseService<AuthService>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly HttpContext _httpContext;
        private readonly AppDbContext _dbContext;
        public AuthService(AppDbContext dbContext,
            ILoggerService<AuthService> logger,
            IHttpContextAccessor httpAccessor,
            UserManager<ApplicationUser> userManager) : base(dbContext, logger, httpAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        #region Register
        public async Task<ResponseResult> Register(RegisterDTO model)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
                    {

                    };
                    await _userManager.AddClaimsAsync(user, claims);

                    return Success();
                }
                else
                {
                    return Error(result.Errors.Select(x => x.Description).ToList());
                }
            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }

        #endregion

        #region Password
        public async Task<ResponseResult> ForgetPassword(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                    return Error("User not found");
                

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordUrl = string.Format("https://notesmanager/account/resetpassword?email={0}&token={1}",user.Email,token);

                return Success(resetPasswordUrl);

            }
            catch (Exception ex)
            {
                return Error(ex);
            }
        }
        #endregion


    }
}
