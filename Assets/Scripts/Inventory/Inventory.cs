using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    [Header("OTHER SCRIPTS REFERENCES")]

    [SerializeField]
    private Equipment equipment;

    [SerializeField]
    private ItemActionsSystem itemActionsSystem;

    [SerializeField]
    private CraftingSystem craftingSystem;

    [Header("INVETORY SYSTEM VARIABLES")]

    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotParent;

    [SerializeField]
    private Sprite emptySlotVisual;

    /* Données non visibles sur la scène */
    const int InventorySize = 24;

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CloseInventory();
        RefreshContent();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            ToolTipSystem.instance.Hide();
            itemActionsSystem.CloseActionPanel();

        }
    }
    public void AddItem(ItemData item) {
        content.Add(item); 
        RefreshContent();
    }
    public void RemoveItem(ItemData item) {
        content.Remove(item); 
        RefreshContent();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        itemActionsSystem.CloseActionPanel();
    }

    public void RefreshContent()
    {
        //On vide tous les visuels des slots
        for (int i = 0; i < inventorySlotParent.childCount; ++i)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.SetItemData(null);
            currentSlot.SetItemVisual(emptySlotVisual);   
        }
        //On peuple le visuel des slots selon le contenu réel de l'inventaire
        for (int i = 0; i < content.Count; ++i)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.SetItemData(content[i]);
            currentSlot.SetItemVisual(content[i].visual);
        }
        equipment.UpdateEquipmentsDesequipButton();
        craftingSystem.UpdateDisplayRecipes();
    }
    public bool IsFull()
    {
        return InventorySize == content.Count;
    }



    /* GETTERS */
    public Sprite GetEmptySlotVisual()
    {
        return emptySlotVisual;
    }
    public List<ItemData> GetContent()
    {
        return content;
    }

}
