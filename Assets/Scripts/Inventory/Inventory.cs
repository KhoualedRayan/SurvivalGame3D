using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    private List<ItemInInventory> content = new List<ItemInInventory>();

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
        //Met l'item dans itemininventory si on l'a dans l'inventaire sinon on met la var a null
        ItemInInventory itemInInventory = GetItemIfExistsInInventory(item);
        //Si on l'a et qu'il est stackable
        if (itemInInventory != null && item.stackable)
        {
            //On rajoute un stack
            itemInInventory.count++;
        }
        else
        {
            //Sinon on le rajoute a l'inventaire
            content.Add(new ItemInInventory(item,1));
        }
        RefreshContent();
    }
    public void RemoveItem(ItemData item, int count =1) {
        ItemInInventory itemInInventory = GetItemIfExistsInInventory(item);
        //Si on a plus d'un élément dans l'inventaire
        if(itemInInventory.count> count)
        {
            //On en enlève 1
            itemInInventory.count-= count;
        }
        else
        {
            //Sinon on le supprime
            content.Remove(itemInInventory);
        }
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
            currentSlot.GetCountText().enabled = false;
        }
        //On peuple le visuel des slots selon le contenu réel de l'inventaire
        for (int i = 0; i < content.Count; ++i)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.SetItemData(content[i].itemData);
            currentSlot.SetItemVisual(content[i].itemData.visual);
            if(currentSlot.GetItemData().stackable)
            {
                currentSlot.GetCountText().enabled = true;
                currentSlot.SetCountText(content[i].count.ToString());
            }
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
    public List<ItemInInventory> GetContent()
    {
        return content;
    }
    public ItemInInventory GetItemIfExistsInInventory(ItemData item)
    {
        return content.Where(elem => elem.itemData == item).FirstOrDefault();
    }

}
[System.Serializable]
public class ItemInInventory
{
    public ItemData itemData;
    public int count;
    public ItemInInventory(ItemData _itemData, int _count)
    {
        itemData = _itemData;
        count = _count; 
    }
}