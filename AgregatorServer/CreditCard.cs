using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class CreditCard
    {
        public string CardNumber { get; set; }
        public string CardholdersName { get; set; }
        public int ValidityMM { get; set; }
        public int ValidityYY { get; set; }
        public int CVC { get; set; }
    }
}