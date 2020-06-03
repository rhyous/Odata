using System;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    public interface IFilterExpressionParser<TEntity>
    {
        Expression<Func<TEntity, bool>> Parse(string filterExpression, bool unquote = true);
    }
}
