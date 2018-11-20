
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class Address
    {
        public string City { get; set; }
        public string Street { get; set; }
        public int House { get; set; }
        public int Apartment { get; set; }
        public string Comment { get; set; }
    }
}