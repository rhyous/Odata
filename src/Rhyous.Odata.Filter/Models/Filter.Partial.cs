using Rhyous.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rhyous.Odata.Filter
{
    public partial class Filter<TEntity>
    {
        #region ToString
        /// <summary>Overrides ToString so that it returns the expression as a string.</summary>
        /// <returns>The expression as a string.</returns>
        /// <example>Id eq 1</example>
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(NonFilter))
            {
                return NonFilter;
            }
            return $"{Left} {Method} {Right}";
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// An implementation of IEnumerable, so the Filter{TEntity} object is IEnumerable and can be used in 
        /// a foreach loop.
        /// </summary>
        /// <returns>And IEnumerator{Filter{TEntity}}</returns>
        public IEnumerator<Filter<TEntity>> GetEnumerator()
        {
            yield return this;
            if (Left != null && !Left.IsSimpleString)
            {
                foreach (var filter in Left)
                    yield return filter;
            }
            if (Right != null && !Right.IsSimpleString)
            {
                foreach (var filter in Right)
                {
                    yield return filter;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region implicit casts
        /// <summary>An implicit cast from a Filter{TEntity} to a string.</summary>
        /// <param name="filter"></param>
        public static implicit operator string(Filter<TEntity> filter)
        {
            if (filter == null || !filter.IsComplete)
                return null;
            return filter.ToString();
        }

        /// <summary>An implicit cast from a string to a Filter{TEntity}.</summary>
        public static implicit operator Filter<TEntity>(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;
            if (str.IsQuoted() || !str.HasWhitespace())
                return new Filter<TEntity> { NonFilter = str };
            return FilterExpressionParser<TEntity>.Instance.ParseAsFilterAsync(str, false).Result;
        }

        /// <summary>An implicit cast from an array to a Filter{TEntity}.</summary>
        public static implicit operator Filter<TEntity>(Array array)
        {
            if (array is null)
                return null;
            var type = typeof(ArrayFilter<,>);
            var genericType = type.MakeGenericType(typeof(TEntity), array.GetType().GetElementType());
            var arrayFilter = Activator.CreateInstance(genericType);
            arrayFilter.GetPropertyInfo("Array").SetValue(arrayFilter, array);
            return arrayFilter as Filter<TEntity>;
        }

        /// <summary>An implicit cast from a Filter{TEntity} to an Expression{Func{TEntity, bool}}.</summary>
        public static implicit operator Expression<Func<TEntity, bool>>(Filter<TEntity> filter)
        {
            return FilterToExpressionConverter.Convert(filter);
        }

        internal static IFilterToExpressionConverter FilterToExpressionConverter
        {
            get { return _Converter ?? (_Converter = Filter.FilterToExpressionConverter.Instance); }
            set { _Converter = value; }
        } private static IFilterToExpressionConverter _Converter;

        #endregion

        #region Clone
        /// <summary>
        /// Clones the current $Filter{TEntity} into a new instance.
        /// </summary>
        /// <returns>A new Filter{TEntity} cloned from the original.</returns>
        public virtual Filter<TEntity> Clone()
        {
            return IsSimpleString
                   ? new Filter<TEntity> { NonFilter = NonFilter }
                   : new Filter<TEntity>
                   {
                       Parent = Parent,
                       Left = Left,
                       Method = Method,
                       Right = Right,
                       Not = Not
                   };
        }

        /// <summary>
        /// Clones the current $Filter{TEntity} into a new instance.
        /// </summary>
        /// <returns>A new Filter{TEntity} cloned from the original.</returns>
        public virtual Filter<TEntity> Clone(bool skipLeft, bool skipRight)
        {
            return IsSimpleString
                   ? new Filter<TEntity> { NonFilter = NonFilter }
                   : new Filter<TEntity>
                   {
                       Parent = Parent,
                       Left = skipLeft ? null : Left,
                       Method = Method,
                       Right = skipRight ? null : Right,
                       Not = Not
                   };
        }
        #endregion
    }
}