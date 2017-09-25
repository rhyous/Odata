namespace Rhyous.Odata
{
    public static class FilterExtensions
    {
        public static Filter<TEntity> Contain<TEntity>(this Filter<TEntity> filter, Conjunction conj)
        {
            if (filter.Parent == null )
                return filter.ContainLeft(conj);
            Filter<TEntity> containerFilter = null;
            if (conj == Conjunction.And || filter.Parent.Method.ToEnum<Conjunction>() != Conjunction.And)
                return filter.ContainLeft(conj, filter.Parent.Right = new Filter <TEntity>());
            if (filter.Parent.Parent != null)
                containerFilter = new Filter<TEntity>().ContainRight(conj, filter.Parent.Parent);
            return filter.Parent.ContainLeft(conj, containerFilter);
        }

        public static Filter<TEntity> ContainLeft<TEntity>(this Filter<TEntity> filter, Conjunction conj, Filter<TEntity> containerFilter = null)
        {
            containerFilter = containerFilter ?? new Filter<TEntity>();
            containerFilter.Parent = filter.Parent;
            containerFilter.Method = conj.ToString();
            containerFilter.Left = filter;
            if (containerFilter.Right == null)
                containerFilter.Right = new Filter<TEntity>();
            return containerFilter;
        }

        public static Filter<TEntity> ContainRight<TEntity>(this Filter<TEntity> filter, Conjunction conj, Filter<TEntity> containerFilter = null)
        {
            containerFilter = containerFilter ?? new Filter<TEntity>();
            containerFilter.Parent = filter.Parent;
            containerFilter.Method = conj.ToString();
            containerFilter.Right = filter;
            if (containerFilter.Left == null)
                containerFilter.Left = new Filter<TEntity>();
            return containerFilter;
        }
    }
}
