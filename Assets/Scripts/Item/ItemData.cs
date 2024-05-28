using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Items/New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite visual;
    public GameObject prefab;
    public bool stackable;
    public ItemType type;
    public EquipmentType equipmentType;
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