using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.Odata.Csdl.Tests
{
    public enum QuantityType
    {
        Inherited = 1,
        Fixed,
        Percentage
    }

    public class SuiteMembership
    {
        public int Id { get; set; }

        public int SuiteId { get; set; }

        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public QuantityType QuantityType { get; set; }
    }
}
