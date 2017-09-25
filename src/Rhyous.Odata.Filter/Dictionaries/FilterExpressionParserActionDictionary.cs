using System;
using System.Collections.Generic;

namespace Rhyous.Odata
{
    public class FilterExpressionParserActionDictionary<TEntity> : Dictionary<char, Action<ParserState<TEntity>>>, 
                                                                   IDictionaryDefaultValueProvider<char, Action<ParserState<TEntity>>>
    {
        public FilterExpressionParserActionDictionary()
        {
            Add(' ', DelimiterHandler.Action);
            Add('"', QuoteHandler.Action);
            Add('\'', QuoteHandler.Action);
            Add('(', OpenParenthesisHander);
            Add(')', CloseParenthesisHander);
            Add('.', PeriodHander);
        }

        public IHandler<ParserState<TEntity>> DelimiterHandler
        {
            get { return _DelimiterHandler ?? (_DelimiterHandler = new DelimiterHandler<TEntity>()); }
            set { _DelimiterHandler = value; }
        } private IHandler<ParserState<TEntity>> _DelimiterHandler;

        public IHandler<ParserState<TEntity>> QuoteHandler
        {
            get { return _QuoteHandler ?? (_QuoteHandler = new QuoteHandler<TEntity>()); }
            set { _QuoteHandler = value; }
        } private IHandler<ParserState<TEntity>> _QuoteHandler;
        #region Character handlers
        public Action<ParserState<TEntity>> DefaultValue => (state) => { state.Append(); };

        public Action<ParserState<TEntity>> DefaultValueProvider(char c)
        {
            if (char.IsWhiteSpace(c))
                return DelimiterHandler.Action;
            return DefaultValue;
        }

        internal void OpenParenthesisHander(ParserState<TEntity> state)
        {
            if (state.AppendIfInWrappedGroup())
                return;
        }

        internal void CloseParenthesisHander(ParserState<TEntity> state)
        {
            if (state.AppendIfInWrappedGroup())
                return;

        }

        internal void PeriodHander(ParserState<TEntity> state)
        {
            if (state.AppendIfInWrappedGroup())
                return;
        }
        #endregion
    }
}
