using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Rhyous.Odata.Csdl
{
    public abstract class AttributeFuncDictionary : IFuncDictionary<Type, MemberInfo>
    {
        internal IDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> _Dict = new ConcurrentDictionary<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>();

        public Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> this[Type key] { get => _Dict[key]; set => _Dict[key] = value; }

        public ICollection<Type> Keys => _Dict.Keys;

        public ICollection<Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> Values => _Dict.Values;

        public int Count => _Dict.Count;

        public bool IsReadOnly => _Dict.IsReadOnly;

        public void Add(Type key, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> value)
        {
            _Dict.Add(key, value);
        }

        public void Add(KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> item)
        {
            _Dict.Add(item);
        }

        public void Clear()
        {
            _Dict.Clear();
        }

        public bool Contains(KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> item)
        {
            return _Dict.Contains(item);
        }

        public bool ContainsKey(Type key)
        {
            return _Dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>[] array, int arrayIndex)
        {
            _Dict.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>>> GetEnumerator()
        {
            return _Dict.GetEnumerator();
        }

        public bool Remove(Type key)
        {
            return _Dict.Remove(key);
        }

        public bool Remove(KeyValuePair<Type, Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>>> item)
        {
            return _Dict.Remove(item);
        }

        public bool TryGetValue(Type key, out Func<MemberInfo, IEnumerable<KeyValuePair<string, object>>> value)
        {
            return _Dict.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Dict.GetEnumerator();
        }
    }
}
