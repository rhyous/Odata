using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{
    public class Entity1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public Guid Guid { get; set; }
        [Editable(false)]
        public DateTime Date { get; set; }
        public string Entity { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
    }
}