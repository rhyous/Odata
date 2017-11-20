using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhyous.Odata.Models
{
    public class RelatedEntityCollectionCollection : List<RelatedEntityCollection>, IOdataParent
    {
        /// <summary>
        /// Items in this list go to the parent of the related entities
        /// </summary>
        public List<RelatedEntityCollection> RelatedEntities { get; set; }

        List<IOdataChild> IOdataParent.Children
        {
            get { return RelatedEntities.ToList<IOdataChild>(); }
            set { RelatedEntities = value.Select(i => i as RelatedEntityCollection).ToList(); }
        }
    }
}
