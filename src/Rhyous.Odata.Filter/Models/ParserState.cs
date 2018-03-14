using System;
using System.Collections.Generic;
using System.Linq;
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

        internal void LastApply()
        {
            if (LastApplyComplete)
                return;
            LastApplyComplete = true;
            Apply();
        } private bool LastApplyComplete = false;

        internal Stack<ParenthesisType> OpenParentheses = new Stack<ParenthesisType>();
        internal bool IsLastChar=> CharIndex == FilterString.Length - 1;
        internal bool ParenthesisIsOpen => OpenParentheses.Any();

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

        internal void OpenParenthesis(ParenthesisType type)
        {
            if (type == ParenthesisType.Method)
            {
                SetMethodIfEmpty();
            }
            OpenParentheses.Push(type);
        }

        internal void CloseParenthesis()
        {
            ParenthesisType type;
            if (ParenthesisIsOpen)
                type = OpenParentheses.Pop();
            else
                throw new InvalidFilterSyntaxException(CharIndex, FilterString);
        }

        internal void CloseMethodGroup()
        {
            throw new NotImplementedException();
        }
    }
}
