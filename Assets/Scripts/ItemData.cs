using UnityEngine;

[CreateAssetMenu(fileName ="Item",menuName ="Items/New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite visual;
    public GameObject prefab;

}
