using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Items/New Item")]
public class ItemData : ScriptableObject
{
    [Header("DATA")]
    public string itemName;
    public string description;
    public Sprite visual;
    public GameObject prefab;
    public bool stackable;
    public int maxStack;

    [Header("TYPES")]
    public ItemType type;
    public EquipmentType equipmentType;

    [Header("ARMOR STATS")]
    public float armorPoints;

    [Header("EFFECTS")]
    public float healthEffect;
    public float hungerEffect;
    public float thirstEffect;
}
public enum ItemType
{
    Ressource,
    Equipment,
    Consumable
}

public enum EquipmentType
{
    Head,
    Chest,
    Hands,
    Pants,
    Feet
}