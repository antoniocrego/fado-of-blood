using UnityEngine;

[System.Serializable]
public class SerializableItem : ISerializationCallbackReceiver
{
    [SerializeField] public int itemID;

    public Item GetItem()
    {
        Item item = WorldItemDatabase.Instance.GetItemFromSerializedData(this);
        return item;
    }

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        
    }
}