using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Rhyous.Odata.Expand
{
    public class ExpandParser
    {
        public List<ExpandPath> Parse(string urlParameterValue)
        {
            if (string.IsNullOrWhiteSpace(urlParameterValue))
                return null;
            var list = new List<ExpandPath>();

            int i = 0;
            while (i < urlParameterValue.Length)
            {
                list.Add(Parse(urlParameterValue, ref i));
            }
            return list;
        }

        internal ExpandPath Parse(string s, ref int i)
        {
            var expandPath = new ExpandPath();
            char c;
            var openParenthesisCount = 0;
            while (i < s.Length)
            {
                c = s[i++];
                if (c != ',' && c != '(' && c != ')' && c != '/')
                {
                    expandPath.Entity += c;
                    continue;
                }
                if (c == '(')
                {
                    openParenthesisCount++;
                    while (openParenthesisCount > 0 && i < s.Length)
                    {
                        c = s[i++];
                        if (c == '(')
                        {
                            openParenthesisCount++;
                        }
                        if (c == ')')
                        {
                            openParenthesisCount--;
                            if (openParenthesisCount == 0)
                                continue;
                        }
                        expandPath.Parenthesis += c;
                    }
                    if (openParenthesisCount > 0)
                        throw new ArgumentException($"The $expand URL parameter has a syntax error at character index {i}. Close paranthesis missing.");
                }
                if (c == ',')
                {
                    return expandPath;
                }
                if (c == '/')
                {
                    expandPath.SubExpandPath = Parse(s, ref i);
                    return expandPath;
                }
            }
            return expandPath;
        }

        public List<ExpandPath> Parse(NameValueCollection parameters)
        {
            var paramValue = parameters?["$expand"];
            if (string.IsNullOrWhiteSpace(paramValue))
                return null;
            return Parse(paramValue);
        }
    }
}