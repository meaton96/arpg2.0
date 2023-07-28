using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> {
    public List<TKey> keys;
    public List<TValue> values;

    public SerializableDictionary(Dictionary<TKey, TValue> dictionary) {
        keys = new List<TKey>(dictionary.Keys);
        values = new List<TValue>(dictionary.Values);
    }
    public SerializableDictionary() {
        keys = new();
        values = new(); 
    } 

    public Dictionary<TKey, TValue> ToDictionary() {
        Dictionary<TKey, TValue> dictionary = new();

        if (keys.Count != values.Count) {
            Debug.LogWarning("SerializableDictionary: Keys and values have different counts.");
            return dictionary;
        }

        for (int i = 0; i < keys.Count; i++) {
            dictionary[keys[i]] = values[i];
        }

        return dictionary;
    }

    // Indexer to access values using the [] operator
    public TValue this[TKey key] {
        get {
            int index = keys.IndexOf(key);
            if (index != -1) {
                return values[index];
            }
            else {
                Debug.LogWarning("SerializableDictionary: Key not found in the dictionary.");
                return default(TValue);
            }
        }
        //set {
        //    int index = keys.IndexOf(key);
        //    if (index != -1) {
        //        values[index] = value;
        //    }
        //    else {
        //        Debug.LogWarning("SerializableDictionary: Key not found in the dictionary. Adding a new entry.");
        //        keys.Add(key);
        //        values.Add(value);
        //    }
        //}
    }
}
