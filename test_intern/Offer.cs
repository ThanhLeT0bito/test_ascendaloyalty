using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace test_intern
{
    public class Offer
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public List<Merchant> Merchants { get; set; }

        public DateTime ValidTo { get; set; }
    }
    public class Merchant : IComparable<Merchant>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }

        public int CompareTo(Merchant other)
        {
            return Distance.CompareTo(other.Distance);
        }
    }
    public class OffersData
    {
        public List<Offer> Offers { get; set; }
    }
   
}
