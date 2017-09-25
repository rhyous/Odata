using Microsoft.Data.Edm;
using Microsoft.Data.OData.Query;
using Rhyous.StringLibrary;
using System;
using System.Linq.Expressions;

namespace Rhyous.Odata
{
    public class FilterExpressionParserNaive<TEntity> : IFilterExpressionParser<TEntity>
    {
        public class Entity1
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Guid Guid { get; set; }
            public DateTime Date { get; set; }
        }

        public Expression<Func<TEntity, bool>> Parse(string filterExpression)
        {
            if (string.IsNullOrWhiteSpace(filterExpression))
                return null;
            var array = filterExpression.Split();
            if (array.Length != 3)
                return null;
            var property = array[0];
            var oper = array[1];
            var type = typeof(TEntity).GetPropertyInfo(property).PropertyType;
            return property.ToLambda<TEntity>(type, new[] { array[2].ToType(type), oper });
        }
    }
}
