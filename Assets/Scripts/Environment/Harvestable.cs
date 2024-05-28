using UnityEngine;

public class Harvestable : MonoBehaviour
{
    [SerializeField]
    private Ressource[] harverstableItems;

    [Header("OPTIONS")]
    [SerializeField]
    private bool disableKinematicOnHarvset;

    [SerializeField]
    private float destroyDelay;

    [SerializeField]
    private Tool tool;
    /* GETTERS */
    public Ressource[] GetHarvestablesItems()
    {
        return harverstableItems;
    }
    public bool IsDisableKinematicOnHarvest()
    {
        return disableKinematicOnHarvset;
    }
    public float GetDestroyDelay()
    {
        return destroyDelay;
    }
    public Tool GetTool()
    {
        return tool;
    }
}

[System.Serializable]
public class Ressource
{
    public ItemData itemData;

    //Drop Rate d'un loot d'item en cassant la ressource
    [Range(0, 100)]
    public int dropRate;
}
public enum Tool
{
    Pickaxe,
    Axe
}