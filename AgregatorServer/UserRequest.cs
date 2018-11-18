using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class UserRequest
    {
        public int UserId { get; set; }
        public string DishName { get; set; }
        public string DateRequest { get; set; }
    }
}