using System;
using System.Text;

namespace Rhyous.Odata
{
    /// <summary>
    /// On object to hold the state of parsing a $filter expression string.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to apply an expression to.</typeparam>
    public class ParserState<TEntity>
    {
        /// <summary>
        /// The constructor
        /// </summary>
        /// <param name="str">The initial filter expression as a string</param>
        public ParserState(string str) { FilterString = str; }

        internal string FilterString;
        internal int CharIndex = 0;
        internal int NextCharIndex => CharIndex + 1;
        internal int PreviousCharIndex => CharIndex -1;
        internal char Char { get { return FilterString[CharIndex]; } }
        internal char? NextChar
        {
            get
            {
                if (NextCharIndex >= 0 && NextCharIndex < FilterString.Length)
                    return FilterString[NextCharIndex];
                return null;
            }
        }

        internal char? PreviousChar
        {
            get
            {
                if (PreviousCharIndex >= 0 && PreviousCharIndex < FilterString.Length)
                    return FilterString[PreviousCharIndex];
                return null;
            }
        }
        internal char LastAppendedChar { get; set; }
        internal int LastAppendedCharIndex { get; set; } = -1;

        internal Filter<TEntity> CurrentFilter = new Filter<TEntity>();
        internal StringBuilder Builder { get; set; } = new StringBuilder();
        internal Group QuoteGroup { get; set; } = new Group();
        internal Group ParenthesisGroup { get; set; } = new Group('(',')');

        internal void Append()
        {
            Builder.Append(Char);
            LastAppendedChar = Char;
            LastAppendedCharIndex = CharIndex;
        }

        internal void LastApply()
        {
            if (!LastApplyComplete)
            {
                Apply();
                LastApplyComplete = true;
            }
            if (QuoteGroup.IsOpen)
                throw new InvalidFilterSyntaxException(CharIndex, FilterString, "An open quote was not closed");
            if (ParenthesisGroup.IsOpen)
                throw new InvalidFilterSyntaxException(CharIndex, FilterString, "An open parenthesis was not closed");
        } private bool LastApplyComplete = false;

        internal bool IsLastChar => CharIndex == FilterString.Length - 1;

        internal bool IsPenultimateChar => CharIndex == FilterString.Length - 2;
        internal string RemainingFilterString => FilterString.Substring(NextCharIndex);

        internal bool AppendIfInQuoteGroup()
        {
            if (QuoteGroup.IsOpen && QuoteGroup.WrapChar != Char)
            {
                Append();
                return true;
            }
            return false;
        }

        internal bool MethodIsInArray() { return "IN".Equals(CurrentFilter.Method, StringComparison.OrdinalIgnoreCase); }

        internal bool AppendIfMethodIsInArray()
        {
            if (MethodIsInArray())
            {
                Append();
                return true;
            }
            return false;
        }

        internal void Apply()
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
                 var methodStr = Builder.ToString();
                Builder.Clear();
                if (!string.IsNullOrWhiteSpace(methodStr) && methodStr.StartsWith("NOT", StringComparison.OrdinalIgnoreCase))
                {
                    CurrentFilter.Not = true;
                    methodStr = methodStr.Substring("NOT".Length);
                }
                CurrentFilter.Method = methodStr;
                return true;
            }
            return false;
        }
        internal bool SetRightIfEmpty()
        {
            if (!CurrentFilter.IsRightComplete)
            {
                var rightExpression = Builder.ToString();
                Builder.Clear();
                if (MethodIsInArray())
                    CurrentFilter.Right = new ArrayFilter<TEntity, string> { Array = rightExpression.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)};
                else
                    CurrentFilter.Right = rightExpression;
                return true;
            }
            return false;
        }
        
        internal void CloseMethodGroup()
        {
            throw new NotImplementedException();
        }
    }
}
