using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rhyous.Odata.Tests
{
    public class EntityWithNullables
    {
        public int Id { get; set; }                     // Should be required
        [Required]
        public string Name { get; set; }                // Should be required
        public string Description { get; set; }         // Should be nullable
        [Editable(false)]
        public DateTime CreateDate { get; set; }        // Should be required
        [Editable(false)]
        public DateTime? LastUpdated { get; set; }      // Should be nullable
        [Editable(false)]
        public int CreatedByUserId { get; set; }        // Should be required
        [Editable(false)]
        public int? LastUpdatedByUserId { get; set; }   // Should be nullable
        [RelatedEntity("Entity1", Nullable = true, RelatedEntityMustExist = false)]
        public int Entity1Id { get; set; }
    }
}