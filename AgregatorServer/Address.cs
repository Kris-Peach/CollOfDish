
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class Address
    {
        public string City { get; set; }
        public double Street { get; set; }
        public double House { get; set; }
        public double Apartment { get; set; }
        public double Comment { get; set; }
    }
}