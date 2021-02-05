using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhyous.Odata
{
    public class ParserState<TEntity>
    {
        public ParserState(string str) { FilterString = str; }

        public string FilterString;
        public int CharIndex = 0;
        public char Char { get { return FilterString[CharIndex]; } }
        public char? NextChar
        {
            get
            {
                var nextCharIndex = CharIndex + 1;
                if (nextCharIndex >= 0 && nextCharIndex < FilterString.Length)
                    return FilterString[nextCharIndex];
                return null;
            }
        }

        public char? PreviousChar
        {
            get
            {
                var prevCharIndex = CharIndex - 1;
                if (prevCharIndex >= 0 && prevCharIndex < FilterString.Length)
                    return FilterString[prevCharIndex];
                return null;
            }
        }
        internal char LastAppendedChar { get; set; }
        internal int LastAppendedCharIndex { get; set; } = -1;

        public Filter<TEntity> CurrentFilter = new Filter<TEntity>();
        public StringBuilder Builder { get; set; } = new StringBuilder();
        public Group QuoteGroup { get; set; } = new Group();
        public Group ParenthesisGroup { get; set; } = new Group('(',')');

        public void Append()
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

        internal bool IsLastChar=> CharIndex == FilterString.Length - 1;
        
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
        
        internal void CloseMethodGroup()
        {
            throw new NotImplementedException();
        }
    }
}
