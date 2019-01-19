using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DemoIdentity.IdentityIsolated.ContextConfiguration;
using DemoIdentity.IdentityIsolated.Repository;

namespace DemoIdentity.IdentityIsolated.Entities
{
    public class CustomSignInManager : SignInManager<ApplicationUser>
    {
        private async Task<bool> IsTfaEnabled(ApplicationUser user)
            => UserManager.SupportsUserTwoFactor &&
            await UserManager.GetTwoFactorEnabledAsync(user) &&
            (await UserManager.GetValidTwoFactorProvidersAsync(user)).Count > 0;        

        public CustomSignInManager(UserManager<ApplicationUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<ApplicationUser>> logger,
            IAuthenticationSchemeProvider schemes,
            ApplicationDbContext applicationDbContext) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {           
            CustomClaimsFactory = new CustomUserClaimsPrincipalFactory(userManager, optionsAccessor, applicationDbContext);
        }             

        public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user) => await CustomClaimsFactory.CreateAsync(user);

        public CustomUserClaimsPrincipalFactory CustomClaimsFactory { get; set; }

        public override async Task RefreshSignInAsync(ApplicationUser user)
        {
            var auth = await Context.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            var authenticationMethod = auth?.Principal?.FindFirstValue(ClaimTypes.AuthenticationMethod);
            await SignInAsync(user, auth?.Properties, authenticationMethod);
        }

        public override Task SignInAsync(ApplicationUser user, bool isPersistent, string authenticationMethod = null)
        {
            return SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent }, authenticationMethod);
        }

        public override async Task SignInAsync(ApplicationUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            var userPrincipal = await CreateUserPrincipalAsync(user);
            // Review: should we guard against CreateUserPrincipal returning null?
            if (authenticationMethod != null)
            {
                userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
            }
            await Context.SignInAsync(IdentityConstants.ApplicationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

        public override bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            return principal?.Identities != null &&
                principal.Identities.Any(i => i.AuthenticationType == IdentityConstants.ApplicationScheme);
        }

        public override async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password,
           bool isPersistent, bool lockoutOnFailure)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var attempt = await CheckPasswordSignInAsync(user, password, lockoutOnFailure);
            return attempt.Succeeded
                ? await SignInOrTwoFactorAsync(user, isPersistent)
                : attempt;
        }
    }
}
