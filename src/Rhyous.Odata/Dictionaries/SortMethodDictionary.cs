using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.Odata
{
    public class SortMethodDictionary<T> : Dictionary<RelatedEntity.Type, Func<IEnumerable<T>, IEnumerable<RelatedEntity>, SortDetails, List<RelatedEntityCollection>>>
    {
        public SortMethodDictionary()
        {
            Add(RelatedEntity.Type.OneToOne, ManyToOneSorter.Sort);        // No enforcement yet of only one
            Add(RelatedEntity.Type.OneToOneForeign, OneToManySorter.Sort); // No enforcement yet of only one
            Add(RelatedEntity.Type.OneToMany, OneToManySorter.Sort);
            Add(RelatedEntity.Type.ManyToOne, ManyToOneSorter.Sort);
            Add(RelatedEntity.Type.ManyToMany, ManyToManySorter.Sort);
        }

        public IRelatedEntitySorter<T> ManyToManySorter
        {
            get { return _ManyToManySorter ?? (_ManyToManySorter = new RelatedEntityManyToManySorter<T>()); }
            set { _ManyToManySorter = value; }
        } private IRelatedEntitySorter<T> _ManyToManySorter;

        public IRelatedEntitySorter<T> OneToManySorter
        {
            get { return _OneToManySorter ?? (_OneToManySorter = new RelatedEntityOneToManySorter<T>()); }
            set { _OneToManySorter = value; }
        }
        private IRelatedEntitySorter<T> _OneToManySorter;
        public IRelatedEntitySorter<T> ManyToOneSorter
        {
            get { return _ManyToOneSorter ?? (_ManyToOneSorter = new RelatedEntityManyToOneSorter<T>()); }
            set { _ManyToOneSorter = value; }
        } private IRelatedEntitySorter<T> _ManyToOneSorter;        
    }
}