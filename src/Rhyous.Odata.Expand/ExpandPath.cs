namespace Rhyous.Odata.Expand
{
    public class ExpandPath
    {
        /// <summary>
        /// The entity to expand.
        /// </summary>
        public string Entity { get; set; }
        /// <summary>
        /// Quer
        /// </summary>
        public string Parenthesis { get; set; }
        /// <summary>
        /// A sublevel expansion
        /// </summary>
        public ExpandPath SubExpandPath { get; set; }
    }
}
