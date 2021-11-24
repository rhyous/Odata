using System;
using System.Collections.Generic;

namespace Rhyous.Odata
{
    public partial class Filter<TEntity> : IParent<Filter<TEntity>>, IEnumerable<Filter<TEntity>>
    {
        private static int InstanceId;
        private static int MyInstanceId;        
        public Filter() { MyInstanceId = ++InstanceId; }

        #region Properties
        private string NonFilter { get; set; }

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
        public bool HasSubFilters { get { return !Right.IsSimpleString || !Left.IsSimpleString; } }

        public int Length { get { return ToString().Length; } }

        #endregion
    }
}