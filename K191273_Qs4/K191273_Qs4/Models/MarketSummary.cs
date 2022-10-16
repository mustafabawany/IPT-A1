using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace K191273_Qs4.Models
{
    public class MarketSummary
    {
        public String Script { get; set; }
        public String Price { get; set; }
        public String Category { get; set; }

        public MarketSummary(String Script, String Price, String Category)
        {
            this.Script = Script;
            this.Price = Price;
            this.Category = Category;
        }
    }
}