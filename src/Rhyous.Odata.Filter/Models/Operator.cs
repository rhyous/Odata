namespace Rhyous.Odata.Filter
{
    /// <summary>
    /// An enum repsenting common $filter operators
    /// </summary>
    public enum Operator
    {
        /// <summary>EQ is equals</summary>
        EQ,
        /// <summary>NE is not equals</summary>
        NE,
        /// <summary>GT is greater than</summary>
        GT,
        /// <summary>GE is greater than or equal</summary>
        GE,
        /// <summary>LT is less Than</summary>
        LT,
        /// <summary>LE is less than or Equal</summary>
        LE,
        /// <summary>IN is in a list or array</summary>
        IN,
    }
}