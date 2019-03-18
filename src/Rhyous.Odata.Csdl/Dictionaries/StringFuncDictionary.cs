using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rhyous.Odata.Csdl
{
    public abstract class StringFuncDictionary : IFuncDictionary<string>
    {
        internal IDictionary<string, Func<IEnumerable<KeyValuePair<string, object>>>> _Dict = new ConcurrentDictionary<string, Func<IEnumerable<KeyValuePair<string, object>>>>();

        public Func<IEnumerable<KeyValuePair<string, object>>> this[string key] { get => _Dict[key]; set => _Dict[key] = value; }

        public ICollection<string> Keys => _Dict.Keys;

        public ICollection<Func<IEnumerable<KeyValuePair<string, object>>>> Values => _Dict.Values;

        public int Count => _Dict.Count;

        public bool IsReadOnly => _Dict.IsReadOnly;

        public void Add(string key, Func<IEnumerable<KeyValuePair<string, object>>> value)
        {
            _Dict.Add(key, value);
        }

        public void Add(KeyValuePair<string, Func<IEnumerable<KeyValuePair<string, object>>>> item)
        {
            _Dict.Add(item);
        }

        public void Clear()
        {
            _Dict.Clear();
        }

        public bool Contains(KeyValuePair<string, Func<IEnumerable<KeyValuePair<string, object>>>> item)
        {
            return _Dict.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _Dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, Func<IEnumerable<KeyValuePair<string, object>>>>[] array, int arrayIndex)
        {
            _Dict.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, Func<IEnumerable<KeyValuePair<string, object>>>>> GetEnumerator()
        {
            return _Dict.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _Dict.Remove(key);
        }

        public bool Remove(KeyValuePair<string, Func<IEnumerable<KeyValuePair<string, object>>>> item)
        {
            return _Dict.Remove(item);
        }

        public bool TryGetValue(string key, out Func<IEnumerable<KeyValuePair<string, object>>> value)
        {
            return _Dict.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Dict.GetEnumerator();
        }
    }
}
