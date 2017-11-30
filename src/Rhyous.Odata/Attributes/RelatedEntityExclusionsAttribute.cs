using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class RelatedEntityExclusionsAttribute : Attribute
    {
        public RelatedEntityExclusionsAttribute(params string[] exclusions) { Exclusions = exclusions.ToList(); }
        public List<string> Exclusions { get; set; }
    }
}
