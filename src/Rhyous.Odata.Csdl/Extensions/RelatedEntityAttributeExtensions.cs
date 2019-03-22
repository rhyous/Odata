using System;
using System.Linq;

namespace Rhyous.Odata.Csdl
{
    public static class RelatedEntityAttributeExtensions
    {
        public static RelatedEntityAttribute Merge(this IGrouping<string, RelatedEntityAttribute> group)
        {
            var list = group.ToList();
            var mergedAttribute = list[0];
            list.RemoveAt(0);
            while (list.Count > 0)
            {
                mergedAttribute = mergedAttribute.Merge(list[0]);
                list.RemoveAt(0);
            }
            return mergedAttribute;
        }

        public static RelatedEntityAttribute Merge(this RelatedEntityAttribute re1, RelatedEntityAttribute re2)
        {
            if (re1.Entity != re2.Entity)
                throw new ArgumentException("Attributes for different entities cannot be merged.");
            if (re1.Property != re2.Property)
                throw new ArgumentException("Attributes for different entity properties cannot be merged.");
            var mergedAttribute = new RelatedEntityAttribute(re1.Entity);
            mergedAttribute.Entity = string.IsNullOrWhiteSpace(re1.Entity) ? re2.Entity : re1.Entity;
            mergedAttribute.EntityAlias = string.IsNullOrWhiteSpace(re1.EntityAlias) ? re2.EntityAlias : re1.EntityAlias;
            mergedAttribute.RelatedEntity = string.IsNullOrWhiteSpace(re1.RelatedEntity) ? re2.RelatedEntity : re1.RelatedEntity;
            mergedAttribute.RelatedEntityAlias = string.IsNullOrWhiteSpace(re1.RelatedEntityAlias) ? re2.RelatedEntityAlias : re1.RelatedEntityAlias;
            mergedAttribute.GetAll = re1.GetAll || re2.GetAll;
            mergedAttribute.AutoExpand = re1.AutoExpand || re2.AutoExpand;
            mergedAttribute.Filter = string.IsNullOrWhiteSpace(re1.Filter) ? re2.Filter : re1.Filter;
            mergedAttribute.ForeignKeyProperty = GetForeignKey(re1.ForeignKeyProperty, re2.ForeignKeyProperty);
            mergedAttribute.ForeignKeyType = re1.ForeignKeyType ?? re2.ForeignKeyType;
            mergedAttribute.Property = string.IsNullOrWhiteSpace(re1.Property) ? re2.Property : re1.Property;
            mergedAttribute.Nullable = re1.Nullable || re2.Nullable;
            mergedAttribute.RelatedEntityMustExist = re1.RelatedEntityMustExist || re2.RelatedEntityMustExist;
            return mergedAttribute;
        }

        private static string GetForeignKey(string fkLeft, string fkRight)
        {
            if (string.IsNullOrWhiteSpace(fkLeft) && string.IsNullOrWhiteSpace(fkRight))
                return Constants.Id;
            if (string.IsNullOrWhiteSpace(fkLeft))
                return fkRight;
            if (string.IsNullOrWhiteSpace(fkRight))
                return fkLeft;
            if (fkLeft == Constants.Id)
                return fkRight;
            return fkLeft;
        }
    }
}