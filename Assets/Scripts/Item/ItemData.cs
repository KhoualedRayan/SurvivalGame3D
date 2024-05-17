using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Items/New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite visual;
    public GameObject prefab;
    
}
