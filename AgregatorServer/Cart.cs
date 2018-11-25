using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgregatorServer
{
    public class Cart
    {
        public int CartId { get; set; }
        public List<Product> ProductList { get; set; }
        public string Date { get; set; }
        public double TotalPrice { get; set; }
    }
}