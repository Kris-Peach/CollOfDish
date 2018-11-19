using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class CreditCard
    {
        public string CardNumber { get; set; }
        public double CardholdersName { get; set; }
        public double ValidityMM { get; set; }
        public double ValidityYY { get; set; }
        public double CVC { get; set; }
    }
}