using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace virtVendingMachine
{
    public class Item
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public Int16 Count { get; set; }
        public string Selector { get; set; }
    }
}
