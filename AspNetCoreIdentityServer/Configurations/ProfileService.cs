using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentityServer.Models;
using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityServer.Configurations
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.AddRange(context.Subject.Claims);

            var user = await _userManager.FindByIdAsync(context.Subject.GetSubjectId());

            var claims = getClaims(user);
            
            //if (!context.AllClaimsRequested)
            //{
                claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            //}

            context.IssuedClaims.AddRange(claims);
            //return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }

        private List<Claim> getClaims(ApplicationUser user)
        {
            var claims = new List<Claim>();
            //var claims = new List<Claim>
            //{
            //    new Claim(JwtClaimTypes.Subject, await _userManager.GetUserIdAsync(user)),
            //    new Claim(JwtClaimTypes.Name, await _userManager.GetUserNameAsync(user))
            //};

            //if (_userManager.SupportsUserEmail)
            //{
            //    var email = await _userManager.GetEmailAsync(user);
            //    if (!string.IsNullOrWhiteSpace(email))
            //    {
            //        claims.AddRange(new[]
            //        {
            //            new Claim(JwtClaimTypes.Email, email),
            //            new Claim(JwtClaimTypes.EmailVerified,
            //                await _userManager.IsEmailConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
            //        });
            //    }
            //}

            //if (_userManager.SupportsUserPhoneNumber)
            //{
            //    var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //    if (!string.IsNullOrWhiteSpace(phoneNumber))
            //    {
            //        claims.AddRange(new[]
            //        {
            //            new Claim(JwtClaimTypes.PhoneNumber, phoneNumber),
            //            new Claim(JwtClaimTypes.PhoneNumberVerified,
            //                await userManager.IsPhoneNumberConfirmedAsync(user) ? "true" : "false", ClaimValueTypes.Boolean)
            //        });
            //    }
            //}

            if (_userManager.SupportsUserClaim)
            {
                var x = _userManager.GetClaimsAsync(user);
                x.Wait();
                claims.AddRange(x.Result);
            }
            if (user.UserName == "borisgr04@gmail.com") {
                var claim = new Claim(JwtClaimTypes.Role, "Admin");
                claims.Add(claim);
            }
            if (user.UserName == "borisgr04@hotmail.com")
            {
                var claim = new Claim(JwtClaimTypes.Role, "Front");
                claims.Add(claim);
            }

            //if (_userManager.SupportsUserRole)
            //{
            //    var roles = _userManager.GetRolesAsync(user);
            //    roles.Wait();
            //    claims.AddRange(roles.Result.Select(role => new Claim(JwtClaimTypes.Role, role)));
            //}

            return claims;
        }
    }
}