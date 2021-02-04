using GraphQL.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringGraphql;
using TuFactoringModels;
using TuFactoring.CustomProviders;

namespace TuFactoring.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthConsumer _consumer;
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _consumer = new AuthConsumer(new GraphQLClient(_configuration["AuthEndpoint"]));
        }

        #region createuser
        public async Task<IdentityResult> CreateUser(User user)
        {
            var newUser = await _consumer.CreateUser(user.CountryId, user.Participant, user.Name, user.Email, user.PasswordHash);

            if (newUser != null)
            {
                user.Id = newUser.Id;
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert user {user.Email}." });
        }
        #endregion

        #region FindByNameAsync
        public async Task<User> FindByNameAsync(string userName)
        {

            try
            {
                return await _consumer.GetUserByName(userName);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region FindByIdAsync
        public async Task<User> FindByIdAsync(string userId)
        {
            try
            {
                return await _consumer.GetUserById(userId);
            }
            catch
            {
                throw;
            }
        }
        public async Task<User> GetProfileUserById(string userId, string token)
        {
            try
            {
                return await _consumer.GetProfileUserById(userId, token);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetUserForLogin
        public async Task<User> GetUserForLogin(int countryId, string discriminator, string participant, string ownerId, string email)
        {
            try
            {
                return await _consumer.GetUserForLogin(countryId, discriminator, participant, ownerId, email);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region GetUserRoles
        public async Task<List<Role>> GetUserRoles(string userId)
        {
            try
            {
                return await _consumer.GetRolesAsync(userId);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region createRole
        public async Task<Role> CreateRole(Role rol, string token = "")
        {
            try
            {
                return await this._consumer.CreateRole(rol, token);
            }
            catch (Exception e)
            {
                return new Role() { Error = e.Message };
            }
        }
        #endregion

        #region inviteUser
        public async Task<User> InviteUser(User user, string token = "")
        {
            try
            {
                return await this._consumer.InviteUser(user, token);
            }
            catch (Exception e)
            {
                return new User() { Error = e.Message };
            }
        }
        #endregion

        #region Roles
        public async Task<List<Role>> GetRoles(string participant, string token = "")
        {
            try
            {
                return await this._consumer.GetRoles(participant, token);
            }
            catch (Exception)
            {
                return new List<Role>();
            }
        }

        public async Task<Response> UpdateRole(Role role, string token = "")
        {
            try
            {
                return await this._consumer.UpdateRole(role, token);
            }
            catch (Exception e)
            {
                return new Response() { Error = e.Message };
            }

        }

        public async Task<Response> BlockRole(int id, string token)
        {
            try
            {
                return await this._consumer.BlockRole(id, token);
            }
            catch (Exception e)
            {
                return new Response() { Error = e.Message };
            }
        }
        #endregion

        #region GetStaff
        public async Task<List<User>> GetStaff(string owner, string participant, string token = "")
        {
            try
            {
                return await this._consumer.GetStaff(owner, participant, token);
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }
        #endregion

        #region GetUserMenu
        public async Task<List<Menu>> GetUserMenu(string participant)
        {
            try
            {
                return await _consumer.GetMenuesAsync(participant);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public async Task<string> GetPersonIdByDocument(int countryId, int identificationId, int prefixId, string number, bool @default)
        {
            try
            {
                return await _consumer.GetPersonIdByDocument(countryId, identificationId, prefixId, number, @default);
            }
            catch
            {
                return null;
                throw;
            }
        }

        public async Task<List<User>> ConsultaListaEjectuvosAsync(string owner, string participant, string token)
        {
            try
            {
                return await _consumer.ConsultaListaEjectuvosAsync(owner, participant, token);
            }
            catch { return null; }

        }

        public async Task<User> FindByEmailAsync(string email, string participant, string discriminator, string owner, int country)
        {
            try
            {
                return await _consumer.FindByEmailAsync(email, participant, discriminator, owner, country);
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> changePassword(User user)
        {
            try
            {
                if (user.Token == null)
                {
                    return await this._consumer.changePassword(user);
                }
                else
                {
                    return await this._consumer.resetPassword(user);
                }
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }

        #region ResetPassword
        public async Task<User> ValidateTokenPassword(string token)
        {
            try
            {
                return await this._consumer.ValidateTokenPassword(token);
            }
            catch (Exception e)
            {
                return new User() { Error = e.Message };
            }
        }

        public async Task<User> emailResetPassword(string email, string number, int prefix, string participant, int country, string discriminator)
        {
            try
            {
                return await this._consumer.emailResetPassword(email, number, prefix, participant, country, discriminator);
            }
            catch (Exception e)
            {
                return new User() { Error = e.Message };
            }
        }
        #endregion

        public async Task<User> BlockUser(User usuario, string token = "")
        {
            try
            {
                return await this._consumer.BlockUser(usuario, token);
            }
            catch (Exception e)
            {
                return new User()
                {
                    Error = e.Message
                };
            }
        }

        public async Task<User> UpdateInviteUser(User usuario, string token = "")
        {
            try
            {
                return await this._consumer.UpdateInviteUser(usuario, token);
            }
            catch (Exception e)
            {
                return new User()
                {
                    Error = e.Message
                };
            }
        }
        public async Task<User> UpdateUser(User usuario, string token = "")
        {
            try
            {
                return await this._consumer.UpdateUser(usuario, token);
            }
            catch (Exception e)
            {
                return new User()
                {
                    Error = e.Message
                };
            }
        }

        #region login
        public async Task<User> Login(string email, string language, int country, string discriminator, string participant, int identification, int prefix, string number)
        {
            try
            {
                return await this._consumer.Login(email, language, country, discriminator, participant, identification, prefix, number);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion


        #region Groups

        public async Task<List<Group>> GetGroups(int country, string token, filterInvoice filter)
        {
            try
            {
                return await _consumer.GetGroups(country, token, filter);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Group> CreateGroup(Group group, string token)
        {
            try
            {
                return await _consumer.CreateGroup(group, token);
            }
            catch
            {
                throw;
            }
        }

        public async Task<Group> UpdateGroup(Group group, string token)
        {
            try
            {
                return await _consumer.UpdateGroup(group, token);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Token

        public async Task<Login> RefreshToken(string id, string language, string participant, string token = null, string confirmant = null)
        {
            try
            {
                return await _consumer.RefreshToken(id, language, participant,token, confirmant);
            }
            catch (Exception e)
            {
                return new Login() { Error = new Error() { Message = e.Message } };
            }
        }
        #endregion
    }
}
