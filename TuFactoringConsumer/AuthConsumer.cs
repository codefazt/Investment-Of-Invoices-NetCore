using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json;
using TuFactoringModels;
using TuFactoringModels.nuevaVersion;

namespace TuFactoringGraphql
{
    public class AuthConsumer
    {
        private readonly GraphQLClient _client;

        public AuthConsumer(GraphQLClient client)
        {
            _client = client;
        }

        #region createuser
        public async Task<User> CreateUser(int country_id, string participant, string name, string email, string password)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                mutation createUser(
                    $country: Int!
                    $participant: String!
                    $name: String!
                    $email: String!
                    $password: String!
                ) {
                    createUser(
                        user: {
                            country: $country
                            participant: $participant
                            name: $name
                            email: $email
                            password: $password
                            password_hash: $password
                        }
                    ) {
                        id
                        country_id
                        participant
                        owner_id
                        name
                        email
                        password
                    }
                }",
                Variables = new { country = country_id, participant, name, email = email, password = password }
            };

            try
            {
                var response = await _client.PostAsync(query);
                return response.GetDataFieldAs<User>("createUser");
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region getUserByName
        public async Task<User> GetUserByName(string userName)
        {

            return null;
            /*

            //userName = userName.ToLower();

            var query = new GraphQLRequest
            {
                Query = @"
                query userByEmail( $country: Int!
                    $discriminator: String!
                    $participant: String!
                    $owner: String!
                    $email: String!) {
                    user: userByEmail( country: $country
                        discriminator: $discriminator
                        participant: $participant
                        owner: $owner
                        email: $email) {
                        id
                        country_id
                        participant
                        owner_id
                        name
                        email
                        password
                    }
                }",
                Variables = new { country= 32, participant = "DEBTOR",discriminator ="LEGAL",owner= "7d097c73-ac28-4ed2-a2d6-499f7a09c725",  email = userName }
            };

            try
            {
                var response = await _client.PostAsync(query);
                var user = response.GetDataFieldAs<User>("user");

                return user;
            }
            catch
            {
                throw;
            }
            */
        }
        #endregion

        #region getUserById
        public async Task<User> GetUserById(string userId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query users($id: ID) {
                    users(id: $id) {
                        list{
                                person{
                                    id
                                }
                              name
                              roles {
                                  id
                                  name
                                  abbreviation
                              }
                        }
                    }
                }",
                Variables = new { id = userId }
            };

            try
            {
                var response = await _client.PostAsync(query);
                var user = response.GetDataFieldAs<UsersResponse>("users").List[0];
                user.NormalizedUserName = user.Name;
                user.UserName = user.Name;
                user.OwnerId = user.Person.Id;

                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<User> GetProfileUserById(string userId, string token)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query users($id: ID) {
                    users(id: $id) {
                        list{
                            person{
                                id
                                name
                            }
                            email
                            name
                            roles {
                                id
                                name
                                abbreviation
                            }
                        }
                    }
                }",
                Variables = new { id = userId }
            };
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }
            try
            {
                var response = await _client.PostAsync(query);
                var user = response.GetDataFieldAs<UsersResponse>("users").List[0];
                if (user.Person != null) user.NormalizedUserName = user.Person.Name;
                if (user.Person != null) user.UserName = user.Person.Name;
                if (user.Person != null) user.OwnerId = user.Person.Id;

                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion


        #region login
        public async Task<User> Login(string email, string language, int country, string discriminator, string participant, int identification, int prefix, string number)
        {
            User user = null;

            var query = new GraphQLRequest
            {
                Query = @"
               mutation login($language:String!, $country: Int!, $discriminator: String!,
				                        $participant:String! $identification: Int!, $number:String!
				                        , $email:String!, $prefix: Int!){
                          login(input:{
                            language:$language,
                            country:$country,
                            discriminator:$discriminator,
                            document: { identification:$identification, number:$number, prefix: $prefix, country: $country}
                            participant:$participant,
                            email:$email,
                          }){
                       confirmant { id }
                       token
                       user{
                           id
                           name
                           password
                           person{
                               id
                           }
                           roles{
                               id
                               abbreviation
                               name
                           }
                       }
                      }
                    }",
                Variables = new { language = language, country = country, discriminator = discriminator, participant = participant, email = email,
                    identification = identification, number = number, prefix = prefix}
            };

            try
            {
                var response = await _client.PostAsync(query);

                if (response.Errors != null)
                {
                    if (response.Errors[0].Message == "Person not valid" || response.Errors[0].Message == "User not valid")
                    {
                        return new User() { Error = "Invalid user" };
                    }
                    return new User() { Error = response.Errors[0].Message};
                }

                var login = response.GetDataFieldAs<LoginInput>("login");

                user = login.User;
                user.Token = login.Token;

                if (user.Name != null) {
                    user.NormalizedUserName = user.Name;
                    user.UserName = user.Name;
                } else {
                    user.NormalizedUserName = email;
                    user.UserName = email;
                }
                user.OwnerId = user.Person.Id;
                user.CountryId = country;
                user.Discriminator = discriminator;
                user.Number = number;
                user.Email = email;
                if (login.Entity != null) {
                    user.Confirmant = login.Entity.Id;
                }
                user.Participant = participant;
                user.Identification = identification;
                user.Prefix = prefix;
            }
            catch (Exception e)
            {
                return new User() { Error = e.Message };
            }

            return user;
        }
        #endregion

        #region getUserForLogin
        public async Task<User> GetUserForLogin(int countryId, string discriminator, string participant, string ownerId, string email)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query user(
                    $country: Int!
                    $discriminator: String!
                    $participant: String!
                    $owner: String!
                    $email: String!
                ) {
                    user: userByEmail(
                        country: $country
                        discriminator: $discriminator
                        participant: $participant
                        owner: $owner
                        email: $email
                    ) {
                        id
                        country_id
                        participant
                        owner_id
                        name
                        email
                        confirmant
                        password
                    }
                }",
                Variables = new { country = countryId, discriminator = discriminator, participant = participant, owner = ownerId, email = email }
            };

            try
            {
                var response = await _client.PostAsync(query);
                var user = response.GetDataFieldAs<User>("user");
                user.NormalizedUserName = user.Email;
                user.UserName = user.Email;

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region getRolesAsync
        public async Task<List<Role>> GetRolesAsync(string userId)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query user(
                    $id: String!
                ) {
                    user(id: $id) {
                        id
                        roles {
                            id
                            discriminator
                            name
                            abbreviation
                            status
                        }
                    }
                }",
                Variables = new { id = userId }
            };

            try
            {
                var response = await _client.PostAsync(query);
                var user = response.GetDataFieldAs<User>("user");
                user.NormalizedUserName = user.Email;
                user.UserName = user.Email;

                return user.Roles;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        #region GetMenuesAsync
        public async Task<List<Menu>> GetMenuesAsync(string participant)
        {
            List<Menu> menues = new List<Menu>();
            var query = new GraphQLRequest
            {
                Query = @"
                query menues(
                    $participant: String!
                ) {
                    menues(participant: $participant) {
                        id
                        participant
                        parent_id
                        item_type
                        content
                        description
                        icon
                        url
                        selected_style
                        selected_class
                        on_click
                        order
                    }
                }",
                Variables = new { participant = participant }
            };

            try
            {

                var response = await _client.PostAsync(query);
                menues = response.GetDataFieldAs<List<Menu>>("menues");
                return menues;
            }
            catch (Exception)
            {
                return menues;
            }
        }
        #endregion

        #region getPersonIdByDocument
        public async Task<String> GetPersonIdByDocument(int countryId, int identificationId, int prefixId, string number, Boolean @default)
        {
            var query = new GraphQLRequest
            {
                Query = @"
                query person($country: Int!, $identification: Int!, $prefix: Int!, $number: String!, $def: Boolean!) {
                    person(
                        country: $country,
                        identification: $identification,
                        prefix: $prefix,
                        number: $number,
                        defaults: $def
                    )
                }",
                Variables = new { country = countryId, identification = identificationId, prefix = prefixId, number = number, def = @default }
            };

            try
            {
                var response = await _client.PostAsync(query);

                if(response.Errors != null)
                {
                    return response.Errors[0].Message;
                }

                return response.GetDataFieldAs<string>("person");
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        #region CreateRole
        public async Task<Role> CreateRole(Role rol, string token)
        {
            if (!new RolesValidation().Validate(rol).IsValid)
            {
                return new Role() { Error = "Invalid Role to create" };
            }

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"
                            mutation($role:RoleInput!){
                              createRole(input:$role){
                                id
                              }
                            }
                        ",
                    Variables = new
                    {
                        role = new
                        {
                            participant = rol.Participant,
                            abbreviation = rol.Abbreviation,
                            name = rol.Name
                        }
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return new Role() { Error = respuesta.Errors[0].Message }; ;
                }

                rol = respuesta.GetDataFieldAs<Role>("createRole");

            }
            catch (Exception e)
            {
                return new Role() { Error = e.Message };
            }


            return rol;

        }
        #endregion

        #region InviteUser
        public async Task<User> InviteUser(User user, string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }


            if (!new usersValidation().Validate(user).IsValid)
            {
                return new User() { Error = "invalidUser" };
            }

            List<RoleInput> roles = new List<RoleInput>();

            foreach (var r in user.Roles)
            {
                roles.Add(new RoleInput()
                {
                    Id = r.Id
                });
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"
                    mutation($user:UserInput!){
                          inviteUser(input:$user){
                            id
                            createdAt
                          }
                        }
                    ",
                    Variables = new
                    {
                        user = new
                        {
                            country = user.CountryId,
                            person = user.OwnerId,
                            name = user.Name,
                            email = user.Email,
                            roles = roles
                        }
                    }
                };
               // _client.DefaultRequestHeaders.Add("Authorization", "Aja23ds135fjhjlksduqeouccmAksnAJh.Aus.aiSHA");
                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return new User() { Error = respuesta.Errors[0].Message }; ;
                }

                user = respuesta.GetDataFieldAs<User>("inviteUser");

            }
            catch (Exception e)
            {
                return new User() { Error = e.Message };
            }


            return user;

        }
        #endregion

        #region GetRolesParticipant
        public async Task<List<Role>> GetRoles(string participant,string token)
        {
            List<Role> roles = new List<Role>();

            if(token != null & token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"query($p:String){
                              roles(participant:$p){
                               list{
                                    id
                                    abbreviation
                                    participant
                                    name
                                    status
                                }
                              }
                             }",
                    Variables = new { p = participant }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return roles;
                }

                roles = respuesta.GetDataFieldAs<RolesResponse>("roles").List;

            }
            catch (Exception)
            {
                return roles;
            }

            return roles;
        }
        #endregion

        #region GetStaff
        public async Task<List<User>> GetStaff(string owner, string participant, string token)
        {
            List<User> usuarios = new List<User>();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"query ($id:String!,$participant:String!){
                            staff(id:$id,participant:$participant){
                             list{
                                   id
                                  email
                                  name
                                  person{
                                    documents{
                                        identification
                                      number
                                    }
                                  }
                                  createdAt
                                  state
                                  roles{
                                      id
                                  }
                              }
                            }
                          }",
                    Variables = new { id = owner, participant = participant }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return usuarios;
                }

                usuarios = respuesta.GetDataFieldAs<UsersResponse>("staff").List;

            }
            catch (Exception)
            {
                return usuarios;
            }


            return usuarios;
        }
        #endregion

        #region FindByEmailAsync
        public async Task<User> FindByEmailAsync(string email,string participant, string discriminator, string owner, int country)
        {
            User user = new User();

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"query($country:Int!, $discriminator: String!, $participant: String!, $owner: String!
			                        $email:String!){
                                  userByEmail(
                                    country:$country,
                                    discriminator: $discriminator,
                                    participant:$participant,
                                    owner:$owner,
                                    email:$email
                                  ){
                                    id
                                    name
                                    participant
                                    email
                                  }
                            }",
                    Variables = new { country = country, discriminator = discriminator, participant = participant, owner = owner, email = email }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return user;
                }

                user = respuesta.GetDataFieldAs<User>("userByEmail");
                
                user.Discriminator = discriminator;
                user.CountryId = country;

            }
            catch (Exception)
            {
                return user;
            }


            return user;
        }
        #endregion

        #region ChangePassword
        public async Task<string> changePassword(User u)
        {
            var user = "";

            if (u.AuthenticationType != null && u.AuthenticationType != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", u.AuthenticationType);
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"mutation changePassword($id: ID,$country: Int,$person: String, $email: String,$password: String){
                                          changePassword(input:{
                                            id:$id
                                            country:$country
                                            person:$person
                                            email:$email
                                            password:$password
                                          }){
                                            id
                                          } }",
                    Variables = new { id = u.Id,country = u.Country,person = u.Person.Id,email=u.Email,password=u.PasswordHash}
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return user;
                }

                user = respuesta.GetDataFieldAs<User>("changePassword").Id.ToString();

            }
            catch (Exception)
            {
                return user;
            }


            return user;
        }
        #endregion

        #region ResetPassword
        public async Task<string> resetPassword(User u)
        {
            var user = "";
            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"mutation resetPassword($token: String,$country: Int,$person: String, $email: String, $password: String){
                                          resetPassword(input:{
                                            country:$country
                                            person:$person
                                            email:$email
                                            password:$password
                                            confirmation:$token
                                          }){
                                            id
                                          }
                                        }",
                    Variables = new { token = u.Token, country = u.Country, person = u.Person.Id, email = u.Email, password = u.PasswordHash }
                };

                //_client.DefaultRequestHeaders.Add("Authorization", "Aja23ds135fjhjlksduqeouccmAksnAJh.Aus.aiSHA");

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Data == null)
                {
                    return user;
                }

                user = respuesta.GetDataFieldAs<User>("resetPassword").Id.ToString();

            }
            catch (Exception)
            {
                return user;
            }


            return user;
        }
        #endregion

        #region ValidateToken
        public async Task<User> ValidateTokenPassword(string token)
        {
            User user = new User();
            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"query($token: String!){
                              validateTokenPassword(
                                token: $token
                              ){
                                id
                                name
                                email
                                country
                                person{
                                  id
                                  documents{
                                    number
                                    identification
                                    prefix
                                  }
                                }
                              }
                            }",
                    Variables = new { token = token }
                };
                
                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    user.Error = respuesta.Errors[0].Message;
                    return user;
                }

                user = respuesta.GetDataFieldAs<User>("validateTokenPassword"); 

            }
            catch (Exception e)
            {
                user.Error = e.Message;
                return user;
            }


            return user;
        }
        #endregion

        #region ResetPassword
        public async Task<User> emailResetPassword(string email, string number, int? prefix, string participant, int country, string discriminator)
        {
            User user = new User();
            try
            {
                if (number == null) number = "";
                if (country == 32) prefix = null;

                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"mutation($email: String!, $number: String!, $prefix: Int, $participant: String!, $country: Int!, $discriminator: String!){
                              forgotPassword(
                               input:{
                                    email: $email
                                    number: $number
                                    prefix: $prefix
                                    participant: $participant
                                    country: $country
                                    discriminator: $discriminator
                                }
                              ){
                                 email
                                }
                            }",
                    Variables = new { email = email, number = number, participant = participant, country = country, prefix = prefix, discriminator = discriminator}
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    user.Error = respuesta.Errors[0].Message;
                    return user;
                }

                user = respuesta.GetDataFieldAs<User>("forgotPassword");
                
                return user;
            }
            catch (Exception e)
            {
                user.Error = e.Message;
                return user;
            }
        }
        #endregion

        #region blockUser
        public async Task<User> BlockUser(User usuario, string token)
        {
            User user = new User();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"mutation($id: ID!){
                              blockUser(
                                id: $id
                              ){
                                id
                             }
                            }",
                    Variables = new { id = usuario.Id}
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    user.Error = respuesta.Errors[0].Message;
                    return user;
                }

                var result = respuesta.GetDataFieldAs<User>("blockUser");
                
                return user;
            }
            catch (Exception e)
            {
                user.Error = e.Message;
                return user;
            }
        }
        #endregion

        #region UpdateUser
        public async Task<User> UpdateInviteUser(User usuario,string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            if (!new usersValidation().Validate(usuario).IsValid)
            {
                //return new User() { Error = "invalidUser" };
            }

            List<RoleInput> roles = new List<RoleInput>();

            foreach (var r in usuario.Roles_id)
            {
                roles.Add(new RoleInput()
                {
                    Id = Int32.Parse(r)
                });
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"
                    mutation($id: ID!,$user:UserInput!){
                          updateUser(id:$id,input:$user){
                            id
                          }
                        }
                    ",
                    Variables = new
                    {
                        id = usuario.Id.ToString(),
                        user = new
                        {
                            name = usuario.Name,
                            email = usuario.Email,
                            roles = roles
                        }
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    usuario.Error = respuesta.Errors[0].Message;
                }
                
                return usuario;
            }
            catch (Exception e)
            {
                usuario.Error = e.Message;
                return usuario;
            }
        }
        public async Task<User> UpdateUser(User usuario, string token)
        {

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");
                _client.DefaultRequestHeaders.Add("Authorization", token);
            }
            List<RoleInput> roles = new List<RoleInput>();
            foreach (var r in usuario.Roles)
            {
                roles.Add(new RoleInput()
                {
                    Id = r.Id
                });
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"
                    mutation($id: ID!,$user:UserInput!){
                          updateUser(id:$id,input:$user){
                            id
                            person{
                                id
                                name
                            }
                            email
                            name
                            roles {
                                id
                                name
                                abbreviation
                            }
                          }
                        }
                    ",
                    Variables = new
                    {
                        id = usuario.Id.ToString(),
                        user = new
                        {
                            name = usuario.Name,
                            email = usuario.Email,
                            roles = roles
                        }
                    }
                };

                var respuesta = await this._client.PostAsync(data);
                if (respuesta.Errors != null) usuario.Error = respuesta.Errors[0].Message;
                usuario = respuesta.GetDataFieldAs<User>("updateUser");
                return usuario;
            }
            catch (Exception e)
            {
                usuario.Error = e.Message;
                return usuario;
            }
        }
        #endregion

        #region BlockRole
        public async Task<Response> BlockRole(int id,string token)
        {
            Response resp = new Response();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"
                            mutation($id:Int!){
                              blockRole(id:$id){ id }
                            }
                        ",
                    Variables = new
                    {
                        id = id
                    }
                };

                var respuesta = await this._client.PostAsync(data);


                if (respuesta.Data == null)
                {
                    return new Response() { Error = respuesta.Errors[0].Message }; 
                }

                var role = respuesta.GetDataFieldAs<Role>("blockRole");
                
                resp.Id = "" + role.Id;
            }
            catch (Exception e)
            {
                return new Response() { Error = e.Message };
            }


            return resp;
        }

        #endregion

        #region UpdateRole
        public async Task<Response> UpdateRole(Role role,string token)
        {
            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            Response resp = new Response();
            
            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"
                            mutation($role:RoleInput!){
                              updateRole(input:$role){
                                    id
                                }
                            }
                        ",
                    Variables = new
                    {
                        role = new
                        {
                            id = role.Id,
                            name = role.Name
                        }
                    }
                };

                var respuesta = await this._client.PostAsync(data);


                if (respuesta.Data == null)
                {
                    return new Response() { Error = respuesta.Errors[0].Message };
                }

                var roleResp = respuesta.GetDataFieldAs<Role>("updateRole");
                
                resp.Id = "" + roleResp.Id;
            }
            catch (Exception e)
            {
                return new Response() { Error = e.Message };
            }


            return resp;
        }

        #endregion
        

        #region Staff Lista de Ejecutivos de Cuentas
        public async Task<List<User>> ConsultaListaEjectuvosAsync(string owner, string participant, string token)
        {
            List<User> usuarios = new List<User>();

            try
            {
                if (token != null && token != "")
                {
                    _client.DefaultRequestHeaders.Remove("Authorization");
                    _client.DefaultRequestHeaders.Add("Authorization", token);
                }

                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"query ($id:String!,$participant:String!){
                            staff(id:$id,participant:$participant){
                             list{
                                id
                                name
                                state
                              }
                            }
                          }",
                    Variables = new { id = owner, participant = participant }
                };

                var respuesta = await this._client.PostAsync(data);
                if (respuesta.Data == null) return usuarios;

                usuarios = respuesta.GetDataFieldAs<UsersResponse>("staff").List;
            }
            catch (Exception) { return usuarios; }
            return usuarios;
        }
        #endregion


        #region Groups
        public async Task<List<Group>> GetGroups(int country,string token, filterInvoice filter)
        {
            List<Group> groups = new List<Group>();

            if (token != null && token != "")
            {

                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);

            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"query($country:Int){
                              groups(input:{
                                country:$country
                              }){
                                list{
                                  id
                                  abbreviation
                                  name
                                  description status
                                  currency{
                                    id
                                  }
                                  program{
                                    id
                                  }
                                  details{
                                    id
                                    concept
                                    category
                                    event
                                    account
                                  }
                                }
                              }
                            }",
                    Variables = new { country = country }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    groups.Add( new Group() { Errors = respuesta.Errors[0].Message });
                    return groups;
                }

                groups = respuesta.GetDataFieldAs<GroupResponse>("groups").List;

            }
            catch (Exception e)
            {
                groups.Add(new Group() { Errors = e.Message });
                return groups;
            }


            return groups;
        }

        public async Task<Group> CreateGroup(Group groupCreate, string token)
        {
            Group group = new Group();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                groupCreate.Status = true;

                for (var i=0; i<groupCreate.Details.Count;i++)
                {
                    groupCreate.Details[i].ID = null;
                }

                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"mutation ($group:GroupInput!){
                                  createGroup(input:$group){
                                    id
                                    abbreviation
                                    name
                                    description status
                                    currency{
                                      id
                                    }
                                    program{
                                      id
                                    }
                                    details{
                                      id
                                      concept
                                      category
                                      event
                                      account
                                    }
                                  }
                                }",
                    Variables = new { group = new {
                       name = groupCreate.Name,
                       abbreviation = groupCreate.Abbreviattion,
                       description = groupCreate.Description,
                       program = groupCreate.Program.Id,
                       currency = groupCreate.Currency.Id,
                       details = groupCreate.Details,
                       country = groupCreate.Country.Id
                    } }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    return new Group() { Errors = respuesta.Errors[0].Message };
                }

                group = respuesta.GetDataFieldAs<Group>("createGroup");

            }
            catch (Exception e)
            {
                return new Group() { Errors = e.Message };
            }


            return group;
        }

        public async Task<Group> UpdateGroup(Group groupUpdate, string token)
        {
            Group group = new Group();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                for (var i = 0; i < groupUpdate.Details.Count; i++)
                {
                    if (groupUpdate.Details[i].ID == "0" || groupUpdate.Details[i].ID == "")
                        groupUpdate.Details[i].ID = null;
                }

                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"mutation ($group:GroupInput!, $id: ID!){
                                    updateGroup(input:$group, id: $id){
                                    id
                                    abbreviation
                                    name
                                    description status
                                    currency{
                                      id
                                    }
                                    program{
                                      id
                                    }
                                    details{
                                      id
                                      concept
                                      category
                                      event
                                      account
                                    }
                                  }
                                }",
                    Variables = new {
                        group = new
                        {
                            name = groupUpdate.Name,
                            abbreviation = groupUpdate.Abbreviattion,
                            description = groupUpdate.Description,
                            program = groupUpdate.Program.Id,
                            currency = groupUpdate.Currency.Id,
                            details = groupUpdate.Details,
                            country = groupUpdate.Country.Id
                        }, id = groupUpdate.Id}
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    return new Group() { Errors = respuesta.Errors[0].Message };
                }

                group = respuesta.GetDataFieldAs<Group>("updateGroup");

            }
            catch (Exception e)
            {
                return new Group() { Errors = e.Message };
            }


            return group;
        }

        #endregion


        #region Token
        public async Task<Login> RefreshToken(string id, string language, string participant, string token = null, string confirmant = null)
        {
            Login resp = new Login();

            if (token != null && token != "")
            {
                _client.DefaultRequestHeaders.Remove("Authorization");

                _client.DefaultRequestHeaders.Add("Authorization", token);
            }

            try
            {
                GraphQLRequest data = new GraphQLRequest()
                {
                    Query = @"mutation ($id:ID!,$language:String!,$participant:String!,$confirmant:String){
                                  refreshToken(input:{
                                    confirmant:$confirmant
                                    id:$id
                                    participant:$participant
                                    language:$language
                                  }){
                                    confirmant{
                                      id
                                    }
                                    user{
                                      id
                                    }
                                    token
                                  }
                                }",
                    Variables = new
                    {
                        id,
                        language,
                        participant,
                        confirmant
                    }
                };

                var respuesta = await this._client.PostAsync(data);

                if (respuesta.Errors != null)
                {
                    return new Login() { Error = new Error(){ Message = respuesta.Errors[0].Message } };
                }

                resp = respuesta.GetDataFieldAs<Login>("refreshToken");

            }
            catch (Exception e)
            {
                return new Login() { Error = new Error() { Message = e.Message } };
            }


            return resp;
        }
        #endregion

    }
}
