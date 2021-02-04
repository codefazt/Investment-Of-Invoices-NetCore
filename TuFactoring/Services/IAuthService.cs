using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuFactoringModels;
using TuFactoring.CustomProviders;

namespace TuFactoring.Services
{
    public interface IAuthService
    {

        #region login
        Task<User> Login(string email,string language, int country, string discriminator, string participant, int identification, int prefix, string number);

        #endregion

        Task<string> changePassword(User user);
        Task<IdentityResult> CreateUser(User user);
        Task<User> FindByNameAsync(string userName);
        Task<User> FindByIdAsync(string userId);
        Task<User> GetProfileUserById(string userId, string token);
        Task<User> GetUserForLogin(int countryId, string discriminator, string participant, string ownerId, string email);
        Task<List<Role>> GetUserRoles(string userId);
        Task<List<Menu>> GetUserMenu(string participant);

        Task<List<Role>> GetRoles(string participant, string token = "");
        Task<Response> UpdateRole(Role rol, string token = "");
        Task<Response> BlockRole(int id, string token = "");

        Task<List<User>> GetStaff(string owner, string participant, string token = "");
        Task<User> InviteUser(User user, string token = "");
        Task<Role> CreateRole(Role rol, string token = "");
        Task<String> GetPersonIdByDocument(int countryId, int identificationId, int prefixId, string number, Boolean @default);

        //Vista Segmentar
        Task<List<User>> ConsultaListaEjectuvosAsync(string owner, string participant, string token);


        Task<User> FindByEmailAsync(string email, string participant, string discriminator, string owner, int country);

        Task<User> ValidateTokenPassword(string token);

        Task<User> emailResetPassword(string email, string number, int prefix, string participant, int country, string discriminator);

        Task<User> BlockUser(User usuario, string token = "");

        Task<User> UpdateInviteUser(User usuario, string token = "");
        Task<User> UpdateUser(User usuario, string token);

        #region Groups
        Task<List<Group>> GetGroups(int country, string token, filterInvoice filter);

        Task<Group> CreateGroup(Group group, string token);

        Task<Group> UpdateGroup(Group group, string token);

        #endregion

        #region token
        Task<Login> RefreshToken(string id, string language, string participant, string token = null, string confirmant = null);
        #endregion

    }
}
