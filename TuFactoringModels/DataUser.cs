using System;
using System.Collections.Generic;
using System.Text;

namespace TuFactoringModels
{
    public class DataUser
    {
        private User _Usuario { get; set; }

        public DataUser(User Usuario)
        {
            _Usuario = Usuario;
        }

        public string get_Id()
        {
            return _Usuario.Id.ToString();
        }

        public string get_OwnerId()
        {
            return _Usuario.OwnerId.ToString();
        }

        public string get_ConfirmantId()
        {
            return _Usuario.Confirmant.ToString();
        }

        public string get_CountryId()
        {
            return _Usuario.CountryId.ToString();
        }

        public string get_Name()
        {
            return _Usuario.Name.ToString();
        }

        public string get_Participant()
        {
            return _Usuario.Participant.ToString();
        }

        public List<Role> get_Roles()
        {
            return _Usuario.Roles;
        }
    }
}
