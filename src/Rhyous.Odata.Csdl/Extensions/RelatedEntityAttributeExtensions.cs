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
                throw new ArgumentException("Attributes for different values for Property cannot be merged.");
            var mergedAttribute = new RelatedEntityAttribute(re1.Entity)
            {
                Entity = string.IsNullOrWhiteSpace(re1.Entity) ? re2.Entity : re1.Entity,
                EntityAlias = string.IsNullOrWhiteSpace(re1.EntityAlias) ? re2.EntityAlias : re1.EntityAlias,
                RelatedEntity = string.IsNullOrWhiteSpace(re1.RelatedEntity) ? re2.RelatedEntity : re1.RelatedEntity,
                RelatedEntityAlias = string.IsNullOrWhiteSpace(re1.RelatedEntityAlias) ? re2.RelatedEntityAlias : re1.RelatedEntityAlias,
                GetAll = re1.GetAll || re2.GetAll,
                AutoExpand = re1.AutoExpand || re2.AutoExpand,
                DisplayCondition = string.IsNullOrWhiteSpace(re1.DisplayCondition) ? re2.DisplayCondition : re1.DisplayCondition,
                Filter = string.IsNullOrWhiteSpace(re1.Filter) ? re2.Filter : re1.Filter,
                ForeignKeyProperty = GetValue(re1.ForeignKeyProperty, re2.ForeignKeyProperty, RelatedEntityAttribute.DefaultForeignKey, string.IsNullOrWhiteSpace),
                ForeignKeyType = GetValue(re1.ForeignKeyType, re2.ForeignKeyType, RelatedEntityAttribute.DefaultForeignKeyType),
                Property = string.IsNullOrWhiteSpace(re1.Property) ? re2.Property : re1.Property,
                Nullable = re1.Nullable || re2.Nullable,
                RelatedEntityMustExist = re1.RelatedEntityMustExist && re2.RelatedEntityMustExist // If either is false, it is optional.
            };
            return mergedAttribute;
        }

        internal static T GetValue<T>(T fkLeft, T fkRight, T defaultValue = default(T), Func<T,bool> check = null)
        {
            if (check == null)
                check = IsNull;
            if (check(fkLeft) && check(fkRight))
                return defaultValue;
            if (check(fkLeft))
                return fkRight;
            if (check(fkRight))
                return fkLeft;
            if (fkLeft.Equals(defaultValue))
                return fkRight;
            return fkLeft;
        }

        internal static bool IsNull<T>(T value) => value == null;
    }
}