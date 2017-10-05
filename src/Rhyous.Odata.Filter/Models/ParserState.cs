using System;
using System.Text;

namespace Rhyous.Odata
{
    public class ParserState<TEntity>
    {
        public ParserState(string str) { FilterString = str; }

        public String FilterString;
        public int CharIndex = 0;
        public char Char { get { return FilterString[CharIndex]; } }

        public Filter<TEntity> CurrentFilter = new Filter<TEntity>();
        public StringBuilder Builder { get; set; } = new StringBuilder();
        public Group QuoteGroup { get; set; } = new Group();

        public void Append() { Builder.Append(Char); }

        public bool AppendIfInQuoteGroup()
        {
            if (QuoteGroup.IsOpen && QuoteGroup.WrapChar != Char)
            {
                Append();
                return true;
            }
            return false;
        }

        public void Apply()
        {
            if (SetLeftIfEmpty() || SetMethodIfEmpty() || SetRightIfEmpty())
                return;
            throw new InvalidFilterSyntaxException(CharIndex, FilterString);
        }

        internal bool SetLeftIfEmpty()
        {
            if (!CurrentFilter.IsLeftComplete)
            {
                CurrentFilter.Left = Builder.ToString();
                Builder.Clear();
                return true;
            }
            return false;
        }

        internal bool SetMethodIfEmpty()
        {
            if (!CurrentFilter.IsMethodComplete)
            {
                CurrentFilter.Method = Builder.ToString();
                Builder.Clear();
                return true;
            }
            return false;
        }
        internal bool SetRightIfEmpty()
        {
            if (!CurrentFilter.IsRightComplete)
            {
                CurrentFilter.Right = Builder.ToString();
                Builder.Clear();
                return true;
            }
            return false;
        }
    }
}
