using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Rhyous.Odata.Expand
{
    public class ExpandParser
    {
        public List<ExpandPath> Parse(string urlParameterValue)
        {
            if (string.IsNullOrWhiteSpace(urlParameterValue))
                return null;
            var list = new List<ExpandPath>();
            var level1Expansions = urlParameterValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
            foreach (var expansion in level1Expansions)
            {
                var subLevelExpansions = expansion.Split('/').Select(s => s.Trim()).ToList();
                ExpandPath expandPath = new ExpandPath();
                list.Add(expandPath);
                while (subLevelExpansions.Any())
                {
                    if (expandPath == null)
                        expandPath = new ExpandPath();
                    var item = subLevelExpansions.First();
                    var parenIndex = item.IndexOf("(");
                    if (parenIndex >= 0)
                    {
                        expandPath.Entity = item.Substring(0, parenIndex);
                        expandPath.Parenthesis = item.Substring(parenIndex + 1, item.Length - parenIndex - 2).Trim();
                    }
                    else
                    {
                        expandPath.Entity = item;
                    }
                    subLevelExpansions.Remove(item);
                    expandPath = expandPath.SubExpandPath;
                }
            }
            return list;
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