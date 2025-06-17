using UnityEngine;

[System.Serializable]
public class SerializableWeapon : ISerializationCallbackReceiver
{
    [SerializeField] public int weaponID;

    public WeaponItem GetWeapon()
    {
        WeaponItem weapon = WorldItemDatabase.Instance.GetWeaponFromSerializedData(this);
        return weapon;
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
    }
}