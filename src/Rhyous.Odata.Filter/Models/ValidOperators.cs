using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Filter
{
    /// <summary>An HashSet of valid $filter operators including conjunctions.</summary>
    public class ValidOperators : HashSet<string>
    {
        #region Singleton

        private static readonly Lazy<ValidOperators> Lazy = new Lazy<ValidOperators>(() => new ValidOperators());

        /// <summary>This singleton instance</summary>
        public static ValidOperators Instance { get { return Lazy.Value; } }
        /// <summary>The constructor. Builds the HashSet to include all Operator enums and both Conjunction enums.</summary>
        internal ValidOperators() : base(StringComparer.OrdinalIgnoreCase)
        {
            foreach (var op in Enum.GetValues(typeof(Operator)))
            {
                Add(op.ToString());
            }
            Add(Conjunction.And.ToString());
            Add(Conjunction.Or.ToString());
        }

        #endregion
    }
}