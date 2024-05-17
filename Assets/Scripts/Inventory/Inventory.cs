using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotParent;

    const int InventorySize = 24;
    private void Start()
    {
        RefreshContent();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            ToolTipSystem.instance.Hide();
        }
    }
    public void AddItem(ItemData item) {
        content.Add(item); 
        RefreshContent();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }

    private void RefreshContent()
    {
        for (int i = 0; i < content.Count; ++i)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.SetItemData(content[i]);
            currentSlot.SetItemVisual(content[i].visual);
        }
    }
    public bool IsFull()
    {
        return InventorySize == content.Count;
    }
}
