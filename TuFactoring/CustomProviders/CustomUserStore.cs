using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using System.Threading;
using TuFactoringGraphql;
using TuFactoring.Services;
using System.Collections.Generic;
using TuFactoringModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace TuFactoring.CustomProviders
{
    /// <summary>
    /// This store is only partially implemented. It supports user creation and find methods.
    /// </summary>
    public class CustomUserStore : IUserStore<User>, IUserPasswordStore<User>, IUserRoleStore<User>, IUserClaimStore<User>
    {
        private readonly IAuthService _authService;

        public CustomUserStore(IAuthService authService)
        {
            _authService = authService;
        }

        #region Internal
        public void Dispose()
        {
        }
        #endregion

        #region IUserStore Implementation
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await _authService.CreateUser(user);
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            //return await _usersTable.DeleteAsync(user);
            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            Guid idGuid;

            if (!Guid.TryParse(userId, out idGuid))
            {
                throw new ArgumentException("Not a valid Guid id", nameof(userId));
            }

            return await _authService.FindByIdAsync(idGuid.ToString());
        }

        public async Task<User> FindByNameAsync(string userName,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userName == null) throw new ArgumentNullException(nameof(userName));

            return await _authService.FindByNameAsync(userName);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedUserName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _authService.changePassword(user);
                if (String.IsNullOrEmpty(token))
                {
                    return null;
                }
                
                return new IdentityResult();
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region IUserPasswordStore Implementation
        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));

            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);

        }
        #endregion

        #region IUserRoleStore Implementation
        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            IList<string> data = new List<string>();

            var roles = await _authService.GetUserRoles(user.Id.ToString());

            foreach (var role in roles)
            {
                data.Add(role.Abbreviation);
            }

            return await Task.FromResult(data);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IUserClaimStore Implementation
        public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            if (cancellationToken != null)
                cancellationToken.ThrowIfCancellationRequested();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            IList<Claim> data = new List<Claim>();

            data.Add(new Claim("Id", user.Id.ToString(), ClaimValueTypes.String));
            data.Add(new Claim("Country", user.CountryId.ToString(), ClaimValueTypes.Integer32));
            data.Add(new Claim("Discriminator", user.Discriminator, ClaimValueTypes.String));
            data.Add(new Claim("Participant", user.Participant, ClaimValueTypes.String));
            data.Add(new Claim("Email", user.Email, ClaimValueTypes.Email));
            data.Add(new Claim("Owner", user.OwnerId, ClaimValueTypes.String));
            if (user.Confirmant != null) {
                data.Add(new Claim("Confirmant", user.Confirmant, ClaimValueTypes.String));
            }

            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    data.Add(new Claim(ClaimTypes.Role, role.Abbreviation));
                }
            }

            data.Add(new Claim(ClaimTypes.Role, user.Participant));
            data.Add(new Claim(ClaimTypes.Role, "D"+user.Discriminator));

            return await Task.FromResult(data);
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion
        /*
        #region IUserEmailStore

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByEmailAsync(string email, string participant, string discriminator, string owner, int country, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (email == null) throw new ArgumentNullException(nameof(email));

            return _authService.FindByEmailAsync(email, participant,discriminator,owner,country);

        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        #endregion

        */
    }
}
