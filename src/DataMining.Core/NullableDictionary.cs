using System;
using System.Collections.Generic;

namespace DataMiningSharp.Core
{
    public class NullableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private bool _containsNulllableValue;
        private TValue _nullableValue;

        public new void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                if (_containsNulllableValue)
                {
                    throw new ArgumentException();
                }
                _nullableValue = value;
                _containsNulllableValue = true;
            }
            else
            {
                base.Add(key, value);
            }
        }

        public new bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                return _containsNulllableValue;
            }
            return base.ContainsKey(key);
        }

        public new bool ContainsValue(TValue value)
        {
            return ((_containsNulllableValue && value.Equals(value)) || base.ContainsValue(value));
        }

        public new bool Remove(TKey key)
        {
            if (key != null)
            {
                return base.Remove(key);
            }
            if (!_containsNulllableValue)
            {
                return false;
            }
            _containsNulllableValue = false;
            return true;
        }

        public new bool TryGetValue(TKey key, out TValue value)
        {
            if (key != null)
            {
                return base.TryGetValue(key, out value);
            }
            if (_containsNulllableValue)
            {
                value = _nullableValue;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public new TValue this[TKey key]
        {
            get
            {
                if (key != null)
                {
                    return base[key];
                }
                if (!_containsNulllableValue)
                {
                    throw new KeyNotFoundException();
                }
                return _nullableValue;
            }
            set
            {
                if (key != null)
                {
                    base[key] = value;
                }
                _nullableValue = value;
                _containsNulllableValue = true;
            }
        }
    }
}