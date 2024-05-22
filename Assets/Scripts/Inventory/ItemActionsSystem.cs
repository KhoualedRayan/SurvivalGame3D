using UnityEngine;

public class ItemActionsSystem : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipment equipment;

    [Header("ITEM ACTIONS SYSTEM VARIABLES")]

    [SerializeField]
    private GameObject actionPanel;

    [SerializeField]
    private Transform dropPoint;

    [SerializeField]
    private GameObject useItemButton;

    [SerializeField]
    private GameObject equipItemButton;

    [SerializeField]
    private GameObject dropItemButton;

    [SerializeField]
    private GameObject destroyItemButton;

    private ItemData itemCurrentlySelected;

    public void OpenActionPanel(ItemData itemData, Vector3 slotPosition)
    {
        itemCurrentlySelected = itemData;
        switch (itemData.type)
        {
            case ItemType.Ressource:
                SetUseAndEquipButton(false, false);
                break;
            case ItemType.Equipment:
                SetUseAndEquipButton(false, true);
                break;
            case ItemType.Consumable:
                SetUseAndEquipButton(true, false);
                break;
        }
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }
    private void SetUseAndEquipButton(bool useItem, bool equipItem)
    {
        useItemButton.SetActive(useItem);
        equipItemButton.SetActive(equipItem);
    }
    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
        Inventory.instance.RefreshContent();
    }
    public void UseItemButton()
    {
        Debug.Log("Use item : " + itemCurrentlySelected.name);
        CloseActionPanel();
    }

    public void EquipActionButton()
    {
        equipment.EquipAction();
    }

    public void DropItemButton()
    {
        //Déposer l'objet au dropPoint qui est dans le player
        GameObject instantiated = Instantiate(itemCurrentlySelected.prefab);
        instantiated.transform.position = dropPoint.position;
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        CloseActionPanel();
    }
    public void DestroyItemButton()
    {
        Inventory.instance.RemoveItem(itemCurrentlySelected);
        CloseActionPanel();
    }

    /* GETTERS */
    public ItemData GetItemCurrentlySelected()
    {
        return itemCurrentlySelected;
    }
}
