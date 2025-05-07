using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ProjectVersion2.Model
{
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
        INotifyPropertyChanged, INotifyCollectionChanged
    {
        private readonly Dictionary<TKey, TValue> _dictionary = new();

        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set
            {
                bool exists = _dictionary.ContainsKey(key);
                if (exists)
                {
                    var oldValue = _dictionary[key];
                    _dictionary[key] = value;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Replace,
                        new KeyValuePair<TKey, TValue>(key, value),
                        new KeyValuePair<TKey, TValue>(key, oldValue)));
                }
                else
                {
                    _dictionary[key] = value;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Add,
                        new KeyValuePair<TKey, TValue>(key, value)));
                }
                OnPropertyChanged(nameof(Count));
            }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Add,
                new KeyValuePair<TKey, TValue>(key, value)));
        }

        public bool Remove(TKey key)
        {
            if (_dictionary.TryGetValue(key, out TValue value) && _dictionary.Remove(key))
            {
                OnPropertyChanged(nameof(Count));
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    new KeyValuePair<TKey, TValue>(key, value)));
                return true;
            }
            return false;
        }

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);
        public void Clear()
        {
            _dictionary.Clear();
            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.ContainsKey(item.Key) && EqualityComparer<TValue>.Default.Equals(_dictionary[item.Key], item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < _dictionary.Count) throw new ArgumentException("The array is too small.");

            foreach (var pair in _dictionary)
            {
                array[arrayIndex++] = pair;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (Contains(item))
            {
                return Remove(item.Key);
            }
            return false;
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args) =>
            CollectionChanged?.Invoke(this, args);
    }
}
