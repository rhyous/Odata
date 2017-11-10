using LinqKit;
using Rhyous.StringLibrary;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Rhyous.Odata
{
    public class Filter<TEntity> : IParent<Filter<TEntity>>
    {
        #region Properties
        private string NonFilter { get; set; }

        public Filter<TEntity> Parent { get; set; }

        public Filter<TEntity> Left
        {
            get { return _Left; }
            set { SafeSet(value, ref _Left, this); }
        } private Filter<TEntity> _Left;

        public Filter<TEntity> Right
        {
            get { return _Right; }
            set { SafeSet(value, ref _Right, this); }
        } private Filter<TEntity> _Right;

        public string Method
        {
            get { return _Method; }
            set { SafeSet(value, ref _Method); }
        } private string _Method;
        
        internal void SafeSet<T>(T value, ref T _backingField, T parent = default(T))
        {
            if (Equals(value, _backingField))
                return;
            _backingField = value;
            if (value != null)
            {
                NonFilter = null;
                IParent<T> parentable = value as IParent<T>;
                if (parentable != null)
                    parentable.Parent = parent;
            }
        }

        public bool Not { get; set; }
        public bool IsStarted { get { return Length > 0; } }

        public bool IsSimpleString { get { return (NonFilter?.Length ?? 0) > 0; } }
        public bool IsComplete { get { return IsSimpleString || (IsLeftComplete && IsMethodComplete && IsRightComplete); } }
        public bool IsLeftComplete { get { return (Left?.Length ?? 0) > 0; } }
        public bool IsMethodComplete { get { return (Method?.Length ?? 0) > 0; } }
        public bool IsRightComplete { get { return (Right?.Length ?? 0) > 0; } }

        public int Length { get { return ToString().Length; } }
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(NonFilter) ? $"{Left} {Method} {Right}" : NonFilter;
        }
        #endregion

        #region implicit casts
        // User-defined conversion from Digit to double
        public static implicit operator string(Filter<TEntity> item)
        {
            return item.ToString();
        }

        // User-defined conversion from Digit to double
        public static implicit operator Filter<TEntity>(string str)
        {
            return new Filter<TEntity> { NonFilter = str };
        }

        public static implicit operator Expression<Func<TEntity, bool>>(Filter<TEntity> filter)
        {
            var possiblePropName = filter.Left.ToString();
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression left = filter.Left.IsSimpleString ? Expression.Property(parameter, possiblePropName) as Expression : filter.Left;
            Type propType = typeof(TEntity).GetPropertyInfo(possiblePropName)?.PropertyType;
            Expression right = (propType != null && filter.Right.IsSimpleString) ? Expression.Constant(filter.Right.ToString().ToType(propType)) as Expression : filter.Right;
            Expression method = null;
            Func<Expression, Expression, Expression> func;
            if (ExpressionMethodDictionary.Instance.TryGetValue(filter.Method, out func))
            {
                var isNumeric = filter.Method.All(c => Char.IsDigit(c));
                Conjunction conj;
                if (!isNumeric && Enum.TryParse(filter.Method, true, out conj))
                {
                    return GetCombinedExpression(left, right, conj);                
                }
                method = func.Invoke(left, right);
            }
            else
            {
                var methodInfo = propType.GetMethod(filter.Method, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance, null, new[] { propType }, null);
                method = Expression.Call(left, methodInfo, right);
            }
            return Expression.Lambda<Func<TEntity, bool>>(filter.Not ? Expression.Not(method) : method, parameter);
        }

        private static Expression<Func<TEntity, bool>> GetCombinedExpression(Expression left, Expression right, Conjunction conj)
        {
            var starter = PredicateBuilder.New<TEntity>();
            starter.Start(left as Expression<Func<TEntity, bool>>);
            return conj == Conjunction.And ? starter.And(right as Expression<Func<TEntity, bool>>) : starter.Or(right as Expression<Func<TEntity, bool>>);
        }
        #endregion
    }
}
