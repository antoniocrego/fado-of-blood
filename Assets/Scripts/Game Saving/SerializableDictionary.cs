using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    public void OnBeforeSerialize()
    {
        // This method is called before serialization.
        // You can implement any logic needed before the dictionary is serialized.
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> kvp in this)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        // This method is called after deserialization.
        // You can implement any logic needed after the dictionary is deserialized.
        Clear();

        if (keys.Count != values.Count)
        {
            Debug.LogError("Keys and values count mismatch during deserialization: " + keys.Count + " keys and " + values.Count + " values.");
        }

        for (int i = 0; i < Mathf.Min(keys.Count, values.Count); i++)
        {
            Add(keys[i], values[i]);
        }
    }

}
