using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class User
    {
        public int UserId { get; set; }
        public string UserSessionId { get; set; }
        public string UserFirstName { get; set; }
        public string UserSecondName    { get; set; }
        public string UserLogin { get; set; }

        public User (int userId, string userSessionId, string userFirstName, string userSecondName, string userLogin)
        { 
            UserId = userId;
            UserSessionId = userSessionId;
            UserFirstName = userFirstName;
            UserSecondName = userSecondName;
            UserLogin = userLogin;

        }
    }
}