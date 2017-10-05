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
            Add(',', DelimiterHandler.Action);

            var quoteHandler = new QuoteHandler<TEntity>();
            Add('"', quoteHandler.Action);
            Add('\'', quoteHandler.Action);

            Add('(', new OpenParanthesisHandler<TEntity>().Action);
            Add(')', new CloseParanthesisHandler<TEntity>().Action);
        }

        #region Character handlers

        public IHandler<ParserState<TEntity>> DelimiterHandler
        {
            get { return _DelimiterHandler ?? (_DelimiterHandler = new DelimiterHandler<TEntity>()); }
            set { _DelimiterHandler = value; }
        } private IHandler<ParserState<TEntity>> _DelimiterHandler;

        public Action<ParserState<TEntity>> DefaultValue => (state) => { state.Append(); };

        public Action<ParserState<TEntity>> DefaultValueProvider(char c)
        {
            if (char.IsWhiteSpace(c))
                return DelimiterHandler.Action;
            return DefaultValue;
        }

        #endregion
    }
}
