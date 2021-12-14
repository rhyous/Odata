using Rhyous.Collections;
using System;

namespace Rhyous.Odata.Filter
{
    /// <summary>An interface defining a dictionary of actions based on characters found in the $filter expression string.</summary>
    /// <typeparam name="TEntity">The entity.</typeparam>
    public interface IFilterExpressionParserActionDictionary<TEntity> : IDictionaryDefaultValueProvider<char, Action<ParserState<TEntity>>>
    {
    }
}