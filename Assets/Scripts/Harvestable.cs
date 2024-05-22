using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField]
    private Ressource[] harverstableItems;

    /* GETTERS */
    public Ressource[] GetHarvestablesItems()
    {
        return harverstableItems;
    }

}

[System.Serializable]
public class Ressource
{
    public ItemData itemData;
    public int minAmountSpawned;
    public int maxAmountSpawned;
}