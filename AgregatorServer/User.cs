using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class User
    {
        public int UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserSecondName    { get; set; }
        public User (int userId, string userFirstName, string userSecondName)
        { 
            UserId = userId;
            UserFirstName = UserFirstName;
            UserSecondName = userSecondName;
        }
    }
}