using Rhyous.Collections;
using System;
using System.Collections.Generic;

namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// The Filter{TEntity} class, used to handle a $filter expression tree and convert it to an expression tree.
    /// </summary>
    /// <typeparam name="TEntity">The Entity type to filter on.</typeparam>
    public partial class Filter<TEntity> : IParent<Filter<TEntity>>, IEnumerable<Filter<TEntity>>
    {
        private static int InstanceId;
        private static int MyInstanceId;
        /// <summary>The constructor.</summary>
        public Filter() { MyInstanceId = ++InstanceId; }

        #region Properties

        /// <summary>The constant string that is not a filter.</summary>
        public string NonFilter { get; set; }

        /// <summary>The parent Filter{TEntity}.</summary>
        public Filter<TEntity> Parent
        {
            get { return _Parent; }
            set
            {
                if (value == this)
                    throw new Exception("Parent cannot be the same as the child.");
                _Parent = value;
            }
        } private Filter<TEntity> _Parent;

        /// <summary>The Left Filter{TEntity}.</summary>
        public Filter<TEntity> Left
        {
            get { return _Left; }
            set { SafeSet(value, ref _Left, this); }
        } private Filter<TEntity> _Left;

        /// <summary>The Right Filter{TEntity}.</summary>
        public Filter<TEntity> Right
        {
            get { return _Right; }
            set { SafeSet(value, ref _Right, this); }
        } private Filter<TEntity> _Right;

        /// <summary>The method or operator of the filter.</summary>
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

        /// <summary>Applies a NOT operator to the entire expression.</summary>
        public bool Not { get; set; }

        /// <summary>Whether the is Filter{TEntity} is started. A new instance is not started. If no $filter expression is added yet, this is false.</summary>
        public bool IsStarted { get { return Length > 0; } }

        /// <summary>Whether this is an Array or not. This always returns false. It can only return false through inheritance.</summary>
        public virtual bool IsArray => false;
        /// <summary>True if this instance of Filter{TEntity} is a simple string, not a query, such as a Property name or a primitive value.</summary>
        public bool IsSimpleString { get { return (NonFilter?.Length ?? 0) > 0; } }
        /// <summary>True if this instance of Filter{TEntity} has all three parts: Left, Method, and Right.</summary>
        public bool IsComplete { get { return IsSimpleString || IsArray || (IsLeftComplete && IsMethodComplete && IsRightComplete); } }
        /// <summary>True if this instance of Filter{TEntity} has a Left part.</summary>
        public bool IsLeftComplete { get { return (Left?.Length ?? 0) > 0; } }
        /// <summary>True if this instance of Filter{TEntity} has a Method part.</summary>
        public bool IsMethodComplete { get { return (Method?.Length ?? 0) > 0; } }
        /// <summary>True if this instance of Filter{TEntity} has a Right part.</summary>
        public bool IsRightComplete { get { return (Right?.Length ?? 0) > 0; } }
        /// <summary>True if the instance of Filter{TEntity} has a Right or Left part that is it's own complex Filter{TEntity}. An ArrayFilter{TEntity} is not considered a subfilter.</summary>
        public bool HasSubFilters { get { return (Right != null && !Right.IsSimpleString && !Right.IsArray) || (Left != null && !Left.IsSimpleString && !Right.IsArray); } }
        /// <summary>The length of this filter string.</summary>
        public int Length { get { return ToString().Length; } }
        /// <summary>Whether this is a root filter or not.</summary>
        public bool IsRoot { get { return Parent == null; } }

        #endregion
    }
}