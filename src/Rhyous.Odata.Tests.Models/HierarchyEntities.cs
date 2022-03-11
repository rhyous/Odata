using System;
using System.Collections.Generic;
using System.Text;

namespace Rhyous.Odata.Tests
{
    [RelatedEntityForeign(nameof(GroupAHierarchy), nameof(GroupA), RelatedEntityAlias = "ParentGroupAMap", ForeignKeyProperty = nameof(GroupAHierarchy.ChildGroupAId))]
    [RelatedEntityForeign(nameof(GroupAHierarchy), nameof(GroupA), RelatedEntityAlias = "ChildGroupAMap", ForeignKeyProperty = nameof(GroupAHierarchy.ParentGroupAId))]
    [RelatedEntityMapping(nameof(GroupA), nameof(GroupAHierarchy), nameof(GroupA), EntityAlias = "ChildGroupA", MappingEntityAlias = "ParentGroupAMap", RelatedEntityAlias = "ParentGroupA")]
    [RelatedEntityMapping(nameof(GroupA), nameof(GroupAHierarchy), nameof(GroupA), EntityAlias = "ParentGroupA", MappingEntityAlias = "ChildGroupAMap", RelatedEntityAlias = "ChildGroupA")]
    public class GroupA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    [MappingEntity(Entity1 = nameof(GroupA), Entity1MappingProperty = nameof(ParentGroupAId), Entity1Alias = "ParentGroupA",
                   Entity2 = nameof(GroupA), Entity2MappingProperty = nameof(ChildGroupAId), Entity2Alias = "ChildGroupA")]
    public class GroupAHierarchy
    {
        /// <inhertidoc />
        [RelatedEntity(nameof(GroupA), RelatedEntityAlias = "ParentGroupA")]
        public int ParentGroupAId { get; set; }
        /// <inhertidoc />
        [RelatedEntity(nameof(GroupA), RelatedEntityAlias = "ChildGroupA")]
        public int ChildGroupAId { get; set; }
    }
}
