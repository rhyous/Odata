using System;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An interface for character handlers when parsing $filter expressions character by character.
    /// </summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public interface IFilterCharacterHandler<TEntity>
    {
        /// <summary>
        /// The action as a method.
        /// </summary>
        Action<ParserState<TEntity>> Action { get; }
    }
}
